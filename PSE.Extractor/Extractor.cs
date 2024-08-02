using System.Text;
using FileHelpers;
using PSE.Model.Input;
using PSE.Model.Input.Log;
using PSE.Model.Input.Models;
using PSE.Model.Input.Interfaces;
using static PSE.Model.Common.Enumerations;

namespace PSE.Extractor
{

    public class Extractor : IExtractor
    {

        private static bool _saveToFile;
        private static string _outputFileNameAndPath;

        private static Type? ModelSelector(MultiRecordEngine engine, string recordContent) 
        {
            if (string.IsNullOrEmpty(recordContent) || recordContent.Trim().Length < 3)
                return null;
            else
            {
                return recordContent.Trim()[..3].ToUpper() switch
                {
                    nameof(REQ) => typeof(REQ),
                    nameof(IDE) => typeof(IDE),
                    nameof(PER) => typeof(PER),
                    nameof(ORD) => typeof(ORD),
                    nameof(CUR) => typeof(CUR),
                    nameof(POS) => typeof(POS),
                    _ => null,
                };
            }
        }        

        /*
        private static void ConvertInputStream(string inputStream, IExtractedData extractedData)
        {
            var engine = new FixedFileEngine<IDE>();
            IDE[] result = engine.ReadString(inputStream);
            engine.ErrorManager.ErrorMode = ErrorMode.IgnoreAndContinue;
            object[] itemsExtracted = engine.ReadString(inputStream);
            if (itemsExtracted != null && itemsExtracted.Length > 0)
            {
                foreach (object _itemExtracted in itemsExtracted)
                {
                    extractedData.ExtractedItems.Add((IInputRecord)_itemExtracted);
                }
            }
            if (engine.ErrorManager.HasErrors)
            {
                foreach (var err in engine.ErrorManager.Errors)
                {
                    extractedData.ExtractionLog.RecordsLog.Add(new RecordExtractionLog()
                    {
                        LineNumber = err.LineNumber,
                        ExceptionOccurred = err.ExceptionInfo,
                        RecordInnerContent = err.RecordString,
                        RecordTypeName = err.RecordTypeName,
                        FurtherMessage = err.ExceptionInfo.Message
                    });
                }
                extractedData.ExtractionLog.Outcome = StreamAcquisitionOutcomes.WithErrors;
            }
            else
                extractedData.ExtractionLog.Outcome = StreamAcquisitionOutcomes.Success;
        }
        */

        private static void ConvertInputStream(string inputStream, IExtractedData extractedData)
        {
            MultiRecordEngine engine = new(
                typeof(REQ),
                typeof(IDE),
                typeof(PER),
                typeof(ORD),
                typeof(CUR),
                typeof(POS)
            )
            {
                RecordSelector = new RecordTypeSelector(ModelSelector),                 
                Encoding = Encoding.UTF8
            };
            engine.ErrorManager.ErrorMode = ErrorMode.ThrowException;
            object[] itemsExtracted = engine.ReadString(inputStream);
            if (itemsExtracted != null && itemsExtracted.Length > 0)
            {
                StringBuilder? sb = null;
                if (_saveToFile && string.IsNullOrEmpty(_outputFileNameAndPath) == false)
                {
                    sb = new StringBuilder();
                    if (File.Exists(_outputFileNameAndPath))
                        File.Delete(_outputFileNameAndPath);
                }
                else
                    _saveToFile = false;    
                foreach (object itemExtracted in itemsExtracted)
                {
                    extractedData.ExtractedItems.Add((IInputRecord)itemExtracted);
                    sb?.Append(((IInputRecord)itemExtracted).ToString());
                }
                if (sb != null)
                {
                    FileInfo fileInfo = new FileInfo(_outputFileNameAndPath);
                    if (fileInfo.Exists == false && fileInfo.Directory != null)
                        Directory.CreateDirectory(fileInfo.Directory.FullName);
                    File.WriteAllText(_outputFileNameAndPath, sb.ToString());
                }
            }
            if (engine.ErrorManager.HasErrors)
            {                
                foreach(var err in engine.ErrorManager.Errors) 
                {
                    extractedData.ExtractionLog.RecordsLog.Add(new RecordExtractionLog()
                    {
                        LineNumber = err.LineNumber,
                        ExceptionOccurred = err.ExceptionInfo,
                        RecordInnerContent = err.RecordString,
                        RecordTypeName = err.RecordTypeName,
                        FurtherMessage = err.ExceptionInfo.Message
                    });
                }
                extractedData.ExtractionLog.Outcome = StreamAcquisitionOutcomes.WithErrors;
            }
            else
                extractedData.ExtractionLog.Outcome = StreamAcquisitionOutcomes.Success;
        }

        static Extractor()
        {
            _saveToFile = false;
            _outputFileNameAndPath = "";
        }

        public Extractor(bool saveToFile = false, string outputFileNameAndPath = "") 
        { 
            _saveToFile = saveToFile; 
            _outputFileNameAndPath = outputFileNameAndPath;
        }        

        public IExtractedData Extract(byte[] input)
        {
            IExtractedData result = new ExtractedData();            
            try
            {
                result.ExtractionLog.AcquisitionStart = DateTime.Now;
                if (input != null && input.Length > 0)
                {
                    result.ExtractionLog.StreamLength = input.Length;
                    ConvertInputStream(ASCIIEncoding.ASCII.GetString(input), result);
                }
                else
                {
                    result.ExtractionLog.Outcome = StreamAcquisitionOutcomes.Aborted;
                    result.ExtractionLog.RecordsLog.Add(new RecordExtractionLog()
                    {
                        FurtherMessage = "The input byte stream is null or empty!"
                    });
                }
            }
            catch (Exception ex)
            {
                result.ExtractionLog.Outcome = StreamAcquisitionOutcomes.Aborted;
                result.ExtractionLog.RecordsLog.Add(new RecordExtractionLog() 
                { 
                    FurtherMessage= "Unexpected error occurred! Please see inner exception for more details.",
                    ExceptionOccurred = ex 
                });
            }
            finally
            {
                result.ExtractionLog.AcquisitionEnd = DateTime.Now;
            }
            return result;
        }

    }

}