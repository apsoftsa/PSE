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
             string _errSource = "Section: '" + e.SectionName + "' - Property: '" + e.PropertyName + "' - Key: '" + e.PropertyKey + "'";
            _outCont?.Logs?.Add(new OutputLog("building", "Decoding error occurred: " + e.ErrorOccurred + " (" + _errSource + ")"));
        }

        private static void BuilderExternalCodifyRequestManagement(object sender, ExternalCodifyRequestEventArgs e)
        {
            _decoder?.Decode(e);
        }

        private static string ReadContent(IFormFile file)
        {
            if (file.Length <= 0) 
                return string.Empty;
            using var _stream = file.OpenReadStream();
            using var _reader = new StreamReader(_stream);
            return _reader.ReadToEnd();
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
            string _tmpNodeKey;
            IExtractedData _extrData;
            List<IInputRecord> _allExtractedItems = new();
            _outCont = new OutputContent();
            foreach (InputContent _file in files)
            {
                _outCont?.Logs?.Add(new OutputLog("extraction", "File to extract: '" + _file.FileName + "'"));
                _extrData = _extractor.Extract(Encoding.ASCII.GetBytes(_file.Content));
                _outCont?.Logs?.Add(new OutputLog("extraction", "Stream length: " + _extrData.ExtractionLog.StreamLength.ToString()));
                if (_extrData.ExtractionLog.AcquisitionStart != null)
                    _outCont?.Logs?.Add(new OutputLog("extraction", "Date/time extraction starting: " + ((DateTime)_extrData.ExtractionLog.AcquisitionStart).ToString("dd/MM/yyyy") + " " + ((DateTime)_extrData.ExtractionLog.AcquisitionStart).ToString("HH:mm:ss")));
                if (_extrData.ExtractionLog.AcquisitionEnd != null)
                    _outCont?.Logs?.Add(new OutputLog("extraction", "Date/time extraction ending: " + ((DateTime)_extrData.ExtractionLog.AcquisitionEnd).ToString("dd/MM/yyyy") + " " + ((DateTime)_extrData.ExtractionLog.AcquisitionEnd).ToString("HH:mm:ss")));
                _outCont?.Logs?.Add(new OutputLog("extraction", "Extraction outcome: " + _extrData.ExtractionLog.Outcome.ToString()));
                if (_extrData.ExtractedItems != null && _extrData.ExtractedItems.Any())
                {
                    _allExtractedItems.AddRange(_extrData.ExtractedItems);
                    _outCont?.Logs?.Add(new OutputLog("extraction", "Elements extracted: " + _extrData.ExtractedItems.Count.ToString()));
                }
                else
                    _outCont?.Logs?.Add(new OutputLog("extraction", "No meaningful elements found."));
                if (_extrData.ExtractionLog.RecordsLog != null && _extrData.ExtractionLog.RecordsLog.Any())
                {
                    _tmpNodeKey = Guid.NewGuid().ToString();
                    List<OutputLog> _extractionSubgroupLogs = new List<OutputLog>();
                    foreach (IRecordExtractionLog _error in _extrData.ExtractionLog.RecordsLog)
                    {
                        _extractionSubgroupLogs.Add(new OutputLog(_tmpNodeKey, "Error message: " + _error.FurtherMessage));
                        if (_error.LineNumber != null && _error.LineNumber > 0)
                            _extractionSubgroupLogs.Add(new OutputLog(_tmpNodeKey, "    - Line number: " + _error.LineNumber.ToString()));
                        if (!string.IsNullOrEmpty(_error.RecordTypeName))
                            _extractionSubgroupLogs.Add(new OutputLog(_tmpNodeKey, "    - Record type name: " + _error.RecordTypeName));
                        if (!string.IsNullOrEmpty(_error.RecordInnerContent))
                            _extractionSubgroupLogs.Add(new OutputLog(_tmpNodeKey, "    - Record inner content: " + _error.RecordInnerContent));
                        if (_error.ExceptionOccurred != null)
                            _extractionSubgroupLogs.Add(new OutputLog(_tmpNodeKey, "    - Has inner exception bound: yes (" + _error.ExceptionOccurred.Message + ")"));
                    }
                    _outCont?.Logs?.Add(new OutputLog("extraction", "Extraction errors occurred: ", _extractionSubgroupLogs));
                }
            }
            if (_allExtractedItems.Any())
            {
                IBuiltData _builtData = _builder.Build(_allExtractedItems, BuildFormats.Json);                
                if (_builtData.BuildingLog.BuildingStart != null)
                    _outCont?.Logs?.Add(new OutputLog("building", "Date /time built start: " + ((DateTime)_builtData.BuildingLog.BuildingStart).ToString("dd/MM/yyyy") + " " + ((DateTime)_builtData.BuildingLog.BuildingStart).ToString("HH:mm:ss")));
                if (_builtData.BuildingLog.BuildingEnd != null)
                    _outCont?.Logs?.Add(new OutputLog("building", "Date/time built end: " + ((DateTime)_builtData.BuildingLog.BuildingEnd).ToString("dd/MM/yyyy") + " " + ((DateTime)_builtData.BuildingLog.BuildingEnd).ToString("HH:mm:ss")));
                _outCont?.Logs?.Add(new OutputLog("building", "Built outcome: " + _builtData.BuildingLog.Outcome.ToString()));
                if (!string.IsNullOrEmpty(_builtData.BuildingLog.FurtherErrorMessage))
                    _outCont?.Logs?.Add(new OutputLog("building", "Error message: " + _builtData.BuildingLog.FurtherErrorMessage));
                if (_builtData.BuildingLog.ExceptionOccurred != null)
                    _outCont?.Logs?.Add(new OutputLog("building", "Inner exception bound: " + _builtData.BuildingLog.ExceptionOccurred.Message));
                if (_builtData.BuildingLog.Outcome == BuildingOutcomes.Success || _builtData.BuildingLog.Outcome == BuildingOutcomes.Ignored)
                    _outCont.JsonGenerated = _builtData.OutputData;
            }
            return _outCont;
        }
        
    }

}
