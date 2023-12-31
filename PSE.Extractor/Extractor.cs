﻿using System.Text;
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
                    //nameof(POR) => typeof(POR),
                    nameof(CUR) => typeof(CUR),
                    nameof(POS) => typeof(POS),
                    _ => null,
                };
            }
        }        

        /*
        private static void ConvertInputStream(string inputStream, IExtractedData extractedData)
        {
            var _engine = new FixedFileEngine<IDE>();
            IDE[] _result = _engine.ReadString(inputStream);
            _engine.ErrorManager.ErrorMode = ErrorMode.IgnoreAndContinue;
            object[] _itemsExtracted = _engine.ReadString(inputStream);
            if (_itemsExtracted != null && _itemsExtracted.Length > 0)
            {
                foreach (object _itemExtracted in _itemsExtracted)
                {
                    extractedData.ExtractedItems.Add((IInputRecord)_itemExtracted);
                }
            }
            if (_engine.ErrorManager.HasErrors)
            {
                foreach (var _err in _engine.ErrorManager.Errors)
                {
                    extractedData.ExtractionLog.RecordsLog.Add(new RecordExtractionLog()
                    {
                        LineNumber = _err.LineNumber,
                        ExceptionOccurred = _err.ExceptionInfo,
                        RecordInnerContent = _err.RecordString,
                        RecordTypeName = _err.RecordTypeName,
                        FurtherMessage = _err.ExceptionInfo.Message
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
            MultiRecordEngine _engine = new(
                typeof(REQ),
                typeof(IDE),
                typeof(PER),
                typeof(CUR),
                typeof(POS)
            )
            {
                RecordSelector = new RecordTypeSelector(ModelSelector),                 
                Encoding = Encoding.UTF8
            };
            _engine.ErrorManager.ErrorMode = ErrorMode.ThrowException;
            object[] _itemsExtracted = _engine.ReadString(inputStream);
            if (_itemsExtracted != null && _itemsExtracted.Length > 0)
            {
                StringBuilder? _sb = null;
                if (_saveToFile && string.IsNullOrEmpty(_outputFileNameAndPath) == false)
                {
                    _sb = new StringBuilder();
                    if (File.Exists(_outputFileNameAndPath))
                        File.Delete(_outputFileNameAndPath);
                }
                else
                    _saveToFile = false;    
                foreach (object _itemExtracted in _itemsExtracted)
                {
                    extractedData.ExtractedItems.Add((IInputRecord)_itemExtracted);
                    _sb?.Append(((IInputRecord)_itemExtracted).ToString());
                }
                if (_sb != null)
                {
                    FileInfo _fileInfo = new FileInfo(_outputFileNameAndPath);
                    if (_fileInfo.Exists == false && _fileInfo.Directory != null)
                        Directory.CreateDirectory(_fileInfo.Directory.FullName);
                    File.WriteAllText(_outputFileNameAndPath, _sb.ToString());
                }
            }
            if (_engine.ErrorManager.HasErrors)
            {                
                foreach(var _err in _engine.ErrorManager.Errors) 
                {
                    extractedData.ExtractionLog.RecordsLog.Add(new RecordExtractionLog()
                    {
                        LineNumber = _err.LineNumber,
                        ExceptionOccurred = _err.ExceptionInfo,
                        RecordInnerContent = _err.RecordString,
                        RecordTypeName = _err.RecordTypeName,
                        FurtherMessage = _err.ExceptionInfo.Message
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
            IExtractedData _result = new ExtractedData();            
            try
            {
                _result.ExtractionLog.AcquisitionStart = DateTime.Now;
                if (input != null && input.Length > 0)
                {
                    _result.ExtractionLog.StreamLength = input.Length;
                    ConvertInputStream(ASCIIEncoding.ASCII.GetString(input), _result);
                }
                else
                {
                    _result.ExtractionLog.Outcome = StreamAcquisitionOutcomes.Aborted;
                    _result.ExtractionLog.RecordsLog.Add(new RecordExtractionLog()
                    {
                        FurtherMessage = "The input byte stream is null or empty!"
                    });
                }
            }
            catch (Exception _ex)
            {
                _result.ExtractionLog.Outcome = StreamAcquisitionOutcomes.Aborted;
                _result.ExtractionLog.RecordsLog.Add(new RecordExtractionLog() 
                { 
                    FurtherMessage= "Unexpected error occurred! Please see inner exception for more details.",
                    ExceptionOccurred = _ex 
                });
            }
            finally
            {
                _result.ExtractionLog.AcquisitionEnd = DateTime.Now;
            }
            return _result;
        }

    }

}