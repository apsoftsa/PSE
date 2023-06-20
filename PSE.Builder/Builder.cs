using System.Xml.Serialization;
using PSE.Model.Input.Models;
using PSE.Model.Input.Interfaces;
using PSE.Model.Output;
using PSE.Model.Output.Interfaces;
using PSE.BusinessLogic;
using static PSE.Model.Common.Enumerations;
using PSE.Model.Output.Models;
using PSE.Model.Output.Common;

namespace PSE.Builder
{

    public class Builder : IBuilder
    {

        private static bool CheckParamsCompatibility(IBuiltData buildData, IList<IInputRecord> extractedData, ManipolationTypes manipolationType, BuildFormats formatToBuild)
        {
            bool _checked = false;
            if (formatToBuild != BuildFormats.Undefined)
            {
                if (manipolationType != ManipolationTypes.Undefined)
                {
                    if (extractedData != null && extractedData.Any())
                    {
                        bool _inputOutputCompatible = false;
                        if (manipolationType == ManipolationTypes.AsSection3
                            && extractedData.Any(_flt => _flt.RecordType == nameof(IDE))
                            && extractedData.Any(_flt => _flt.RecordType == nameof(PER)))
                            _inputOutputCompatible = true;
                        else
                        {
                            buildData.BuildingLog.FurtherErrorMessage = $"If the manipolation type requested is '{manipolationType}', " +
                                $"at least one input record data source having the formats: '{nameof(IDE)}, {nameof(PER)}' must be provided!";
                            buildData.BuildingLog.Outcome = BuildingOutcomes.Failed;
                        }
                        if (_inputOutputCompatible)
                            _checked = true;
                    }
                    else
                    {
                        buildData.BuildingLog.FurtherErrorMessage = "No extracted data received as input data source!";
                        buildData.BuildingLog.Outcome = BuildingOutcomes.Failed;
                    }
                }
                else
                {
                    buildData.BuildingLog.FurtherErrorMessage = "Manipolation data typology undefined!";
                    buildData.BuildingLog.Outcome = BuildingOutcomes.Failed;
                }
            }
            else
            {
                buildData.BuildingLog.FurtherErrorMessage = "Build output format unknown!";
                buildData.BuildingLog.Outcome = BuildingOutcomes.Failed;
            }
            return _checked;
        }

        private static IOutputModel? ManipulateInputData(IList<IInputRecord> extractedData, ManipolationTypes manipolationType)
        {
            IOutputModel? _output;
            switch (manipolationType)
            {
                case ManipolationTypes.AsSection3:
                    _output = new ManipolatorSection3().Manipulate(extractedData);
                    break;
                default:
                    _output = null;
                    break;
            }
            return _output;
        }

        private static string SerializeOutputData(SectionsContainer outputDataContainer, BuildFormats formatToBuild)
        {
            string? _serializedData = null;
            switch (formatToBuild)
            {
                case BuildFormats.Xml:
                    {
                        var _serializer = new XmlSerializer(typeof(SectionsContainer));
                        using StringWriter textWriter = new();
                        _serializer.Serialize(textWriter, outputDataContainer);
                        _serializedData = textWriter.ToString();
                    }
                    break;
                case BuildFormats.Json:
                    _serializedData = Utility.JsonObjectSerialization<SectionsContainer>(outputDataContainer);
                    break;
            }
            return _serializedData ?? string.Empty;
        }

        public IBuiltData Build(IList<IInputRecord> extractedData, BuildFormats formatToBuild)
        {
            List<ManipolationTypes> _manipolationTypesToManage = new List<ManipolationTypes>() { ManipolationTypes.AsSection3 };
            IBuiltData _buildData = new BuiltData();
            IList<IOutputModel> _sections = new List<IOutputModel>();
            try
            {
                _buildData.BuildingLog.BuildingStart = DateTime.Now;
                foreach (ManipolationTypes _manipolationTypeToManage in _manipolationTypesToManage)
                {
                    if (CheckParamsCompatibility(_buildData, extractedData, _manipolationTypeToManage, formatToBuild))
                    {
                        IOutputModel? _output = ManipulateInputData(extractedData, _manipolationTypeToManage);
                        if (_output != null)
                            _sections.Add(_output);
                        else
                        {
                            _buildData.BuildingLog.FurtherErrorMessage = "Input data manipulation failed, impossible to generate the output object!";
                            _buildData.BuildingLog.Outcome = BuildingOutcomes.Failed;
                        }
                    }
                }
                _buildData.OutputData = SerializeOutputData(new SectionsContainer(_sections), formatToBuild);
                if (string.IsNullOrEmpty(_buildData.OutputData))
                {
                    _buildData.BuildingLog.FurtherErrorMessage = "Input data serialization failed, impossible to generate the output file!";
                    _buildData.BuildingLog.Outcome = BuildingOutcomes.Failed;
                }
                else
                    _buildData.BuildingLog.Outcome = BuildingOutcomes.Success;
            }
            catch (Exception _ex)
            {
                _buildData.BuildingLog.FurtherErrorMessage = "Unexpected error occurred! Please see inner exception for more details.";
                _buildData.BuildingLog.ExceptionOccurred = _ex;
                _buildData.BuildingLog.Outcome = BuildingOutcomes.Failed;
            }
            finally
            {                
                _buildData.BuildingLog.BuildingEnd = DateTime.Now;
            }
            return _buildData;
        }

    }

}
