using System.Text;
using PSE.Model.Events;
using PSE.Model.Input.Interfaces;
using PSE.Model.Output.Interfaces;
using PSE.Model.Exchange;
using PSE.Model.Common;
using static PSE.Model.Common.Enumerations;

namespace PSE.WebApi.ApplicationLogic
{

    public class ExtractionManager
    {

        private static Extractor.Extractor _extractor;
        private static Builder.Builder _builder;
        private static Decoder.IDecoder _decoder;

        private static OutputContent _outCont;

        private static void DecoderExternalCodifyErrorOccurredManagement(object sender, ExternalCodifyRequestEventArgs e)
        {            
             string errSource = "Section: '" + e.SectionName + "' - Property: '" + e.PropertyName + "' - Key: '" + e.PropertyKey + "'";
            _outCont?.Logs?.Add(new OutputLog("building", "Decoding error occurred: " + e.ErrorOccurred + " (" + errSource + ")"));
        }

        private static void BuilderExternalCodifyRequestManagement(object sender, ExternalCodifyRequestEventArgs e)
        {
            _decoder?.Decode(e);
        }

        private static string ReadContent(IFormFile file)
        {
            if (file.Length <= 0) 
                return string.Empty;
            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public static void Initialize(AppSettings appSettings)
        {
            _extractor = new Extractor.Extractor(true, AppDomain.CurrentDomain.BaseDirectory + @"PSEStructuresExport.txt");
            _decoder = new Decoder.Decoder(appSettings);
            _decoder.ExternalCodifyErrorOccurred += DecoderExternalCodifyErrorOccurredManagement;
            _builder = new Builder.Builder();
            _builder.ExternalCodifyRequest += BuilderExternalCodifyRequestManagement;
            _outCont = null;
        }

        public static InputContent ReadFile(IFormFile file)
        {
            return new InputContent
            {
                Content = ReadContent(file),
                ContentDisposition = file.ContentDisposition,
                ContentType = file.ContentType,
                FileName = file.FileName,
                Length = file.Length,
                Name = file.Name,
            };
        }

        public static OutputContent ExtractFiles(IList<InputContent> files)
        {
            string tmpNodeKey;
            IExtractedData extrData;
            List<IInputRecord> allExtractedItems = new();
            _outCont = new OutputContent();
            foreach (InputContent file in files)
            {
                _outCont?.Logs?.Add(new OutputLog("extraction", "File to extract: '" + file.FileName + "'"));
                extrData = _extractor.Extract(Encoding.ASCII.GetBytes(file.Content));
                _outCont?.Logs?.Add(new OutputLog("extraction", "Stream length: " + extrData.ExtractionLog.StreamLength.ToString()));
                if (extrData.ExtractionLog.AcquisitionStart != null)
                    _outCont?.Logs?.Add(new OutputLog("extraction", "Date/time extraction starting: " + ((DateTime)extrData.ExtractionLog.AcquisitionStart).ToString("dd/MM/yyyy") + " " + ((DateTime)extrData.ExtractionLog.AcquisitionStart).ToString("HH:mm:ss")));
                if (extrData.ExtractionLog.AcquisitionEnd != null)
                    _outCont?.Logs?.Add(new OutputLog("extraction", "Date/time extraction ending: " + ((DateTime)extrData.ExtractionLog.AcquisitionEnd).ToString("dd/MM/yyyy") + " " + ((DateTime)extrData.ExtractionLog.AcquisitionEnd).ToString("HH:mm:ss")));
                _outCont?.Logs?.Add(new OutputLog("extraction", "Extraction outcome: " + extrData.ExtractionLog.Outcome.ToString()));
                if (extrData.ExtractedItems != null && extrData.ExtractedItems.Any())
                {
                    allExtractedItems.AddRange(extrData.ExtractedItems);
                    _outCont?.Logs?.Add(new OutputLog("extraction", "Elements extracted: " + extrData.ExtractedItems.Count.ToString()));
                }
                else
                    _outCont?.Logs?.Add(new OutputLog("extraction", "No meaningful elements found."));
                if (extrData.ExtractionLog.RecordsLog != null && extrData.ExtractionLog.RecordsLog.Any())
                {
                    tmpNodeKey = Guid.NewGuid().ToString();
                    List<OutputLog> extractionSubgroupLogs = new List<OutputLog>();
                    foreach (IRecordExtractionLog error in extrData.ExtractionLog.RecordsLog)
                    {
                        extractionSubgroupLogs.Add(new OutputLog(tmpNodeKey, "Error message: " + error.FurtherMessage));
                        if (error.LineNumber != null && error.LineNumber > 0)
                            extractionSubgroupLogs.Add(new OutputLog(tmpNodeKey, "    - Line number: " + error.LineNumber.ToString()));
                        if (!string.IsNullOrEmpty(error.RecordTypeName))
                            extractionSubgroupLogs.Add(new OutputLog(tmpNodeKey, "    - Record type name: " + error.RecordTypeName));
                        if (!string.IsNullOrEmpty(error.RecordInnerContent))
                            extractionSubgroupLogs.Add(new OutputLog(tmpNodeKey, "    - Record inner content: " + error.RecordInnerContent));
                        if (error.ExceptionOccurred != null)
                            extractionSubgroupLogs.Add(new OutputLog(tmpNodeKey, "    - Has inner exception bound: yes (" + error.ExceptionOccurred.Message + ")"));
                    }
                    _outCont?.Logs?.Add(new OutputLog("extraction", "Extraction errors occurred: ", extractionSubgroupLogs));
                }
            }
            if (allExtractedItems.Any())
            {
                IBuiltData builtData = _builder.Build(allExtractedItems, BuildFormats.Json);                
                if (builtData.BuildingLog.BuildingStart != null)
                    _outCont?.Logs?.Add(new OutputLog("building", "Date /time built start: " + ((DateTime)builtData.BuildingLog.BuildingStart).ToString("dd/MM/yyyy") + " " + ((DateTime)builtData.BuildingLog.BuildingStart).ToString("HH:mm:ss")));
                if (builtData.BuildingLog.BuildingEnd != null)
                    _outCont?.Logs?.Add(new OutputLog("building", "Date/time built end: " + ((DateTime)builtData.BuildingLog.BuildingEnd).ToString("dd/MM/yyyy") + " " + ((DateTime)builtData.BuildingLog.BuildingEnd).ToString("HH:mm:ss")));
                _outCont?.Logs?.Add(new OutputLog("building", "Built outcome: " + builtData.BuildingLog.Outcome.ToString()));
                if (!string.IsNullOrEmpty(builtData.BuildingLog.FurtherErrorMessage))
                    _outCont?.Logs?.Add(new OutputLog("building", "Error message: " + builtData.BuildingLog.FurtherErrorMessage));
                if (builtData.BuildingLog.ExceptionOccurred != null)
                    _outCont?.Logs?.Add(new OutputLog("building", "Inner exception bound: " + builtData.BuildingLog.ExceptionOccurred.Message));
                if (builtData.BuildingLog.Outcome == BuildingOutcomes.Success || builtData.BuildingLog.Outcome == BuildingOutcomes.Ignored)
                    _outCont.JsonGenerated = builtData.OutputData;
            }
            return _outCont;
        }
        
    }

}
