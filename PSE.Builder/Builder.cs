using System.Xml.Serialization;
using PSE.Model.Input.Models;
using PSE.Model.Input.Interfaces;
using PSE.Model.Output;
using PSE.Model.Output.Models;
using PSE.Model.Output.Interfaces;
using PSE.BusinessLogic;
using static PSE.BusinessLogic.Utility.ManipulatorOperatingRules;
using static PSE.Model.Common.Constants;
using static PSE.Model.Common.Enumerations;

namespace PSE.Builder
{

    public class Builder : IBuilder
    {

        private readonly List<(ManipolationTypes manipolationType, bool isMandatory)> _manipolationTypesToManage;

        private static void CheckInputData(IBuiltData buildData, IList<IInputRecord> extractedData, ManipolationTypes manipolationType, bool isMandatory, BuildFormats formatToBuild)
        {
            if (formatToBuild != BuildFormats.Undefined)
            {
                if (manipolationType != ManipolationTypes.Undefined)
                {
                    if (extractedData != null && extractedData.Any())
                    {
                        switch (manipolationType)
                        {
                            case ManipolationTypes.AsHeader:
                                {
                                    if (extractedData.Any()) // !!!! temporaneo, manca analisi
                                        buildData.BuildingLog.Outcome = BuildingOutcomes.Success;
                                    else if (isMandatory)
                                    {
                                        buildData.BuildingLog.FurtherErrorMessage = $"If the manipolation type requested is '{manipolationType}', " +
                                            "at least one input element into data source having must be provided!";
                                        buildData.BuildingLog.Outcome = BuildingOutcomes.Failed;
                                    }
                                    else
                                        buildData.BuildingLog.Outcome = BuildingOutcomes.Ignored;
                                }
                                break;
                            case ManipolationTypes.AsSection1:
                                {
                                    if (extractedData.Any(_flt => _flt.RecordType == nameof(IDE)))
                                        buildData.BuildingLog.Outcome = BuildingOutcomes.Success;
                                    else if(isMandatory)
                                    {
                                        buildData.BuildingLog.FurtherErrorMessage = $"If the manipolation type requested is '{manipolationType}', " +
                                            $"at least one input record data source having the format ' { nameof(IDE) } ' must be provided!";
                                        buildData.BuildingLog.Outcome = BuildingOutcomes.Failed;
                                    }
                                    else
                                        buildData.BuildingLog.Outcome = BuildingOutcomes.Ignored;
                                }
                                break;
                            case ManipolationTypes.AsSection3:
                            case ManipolationTypes.AsSection4:
                                {
                                    if (extractedData.Any(_flt => _flt.RecordType == nameof(IDE)) &&
                                        extractedData.Any(_flt => _flt.RecordType == nameof(PER)))
                                        buildData.BuildingLog.Outcome = BuildingOutcomes.Success;
                                    else if(isMandatory)
                                    {
                                        buildData.BuildingLog.FurtherErrorMessage = $"If the manipolation type requested is '{manipolationType}', " +
                                            $"at least one input record data source having the formats: '{nameof(IDE)}, {nameof(PER)}' must be provided!";
                                        buildData.BuildingLog.Outcome = BuildingOutcomes.Failed;
                                    }
                                    else
                                        buildData.BuildingLog.Outcome = BuildingOutcomes.Ignored;
                                }
                                break;
                            case ManipolationTypes.AsSection8:
                            case ManipolationTypes.AsSection9:
                            case ManipolationTypes.AsSection10:
                            case ManipolationTypes.AsSection11:
                            case ManipolationTypes.AsSection12:
                            case ManipolationTypes.AsSection13:
                            case ManipolationTypes.AsSection14:
                            case ManipolationTypes.AsSection15:
                            case ManipolationTypes.AsSection16And17:
                            case ManipolationTypes.AsSection18And19:
                            case ManipolationTypes.AsSection20:
                                {
                                    if (extractedData.Any(_flt => _flt.RecordType == nameof(IDE)) &&
                                        extractedData.Any(_flt => _flt.RecordType == nameof(POS)))
                                    {
                                        if(ArePOSRowsManipulable(extractedData.Where(_flt => _flt.RecordType == nameof(POS)).OfType<POS>(), manipolationType))
                                            buildData.BuildingLog.Outcome = BuildingOutcomes.Success;
                                        else if (isMandatory)
                                        {
                                            List<PositionClassifications> _classificationsBound = GetClassificationsBoundToSection(manipolationType);
                                            string _classificationsDescr = string.Empty;
                                            if (_classificationsBound != null && _classificationsBound.Any())
                                            {
                                                foreach (PositionClassifications _classificationBound in _classificationsBound)
                                                {
                                                    _classificationsDescr += _classificationBound.ToString() + " ";
                                                }
                                            }
                                            buildData.BuildingLog.FurtherErrorMessage = $"If the manipolation type requested is '{manipolationType}', " +
                                                $"at least one input record having format '{nameof(POS)}' and sub-category code '{ _classificationsDescr.Trim() }' must be provided!";
                                            buildData.BuildingLog.Outcome = BuildingOutcomes.Failed;
                                        }
                                        else
                                            buildData.BuildingLog.Outcome = BuildingOutcomes.Ignored;
                                    }
                                    else if (isMandatory)
                                    {
                                        buildData.BuildingLog.FurtherErrorMessage = $"If the manipolation type requested is '{manipolationType}', " +
                                            $"at least one input record data source having the formats: '{nameof(IDE)}, {nameof(POS)}' must be provided!";
                                        buildData.BuildingLog.Outcome = BuildingOutcomes.Failed;
                                    }
                                    else
                                        buildData.BuildingLog.Outcome = BuildingOutcomes.Ignored;
                                }
                                break;
                            case ManipolationTypes.AsFooter:
                                {
                                    if (extractedData.Any()) // !!!! temporaneo, manca analisi
                                        buildData.BuildingLog.Outcome = BuildingOutcomes.Success;
                                    else if (isMandatory)
                                    {
                                        buildData.BuildingLog.FurtherErrorMessage = $"If the manipolation type requested is '{manipolationType}', " +
                                            "at least one input element into data source having must be provided!";
                                        buildData.BuildingLog.Outcome = BuildingOutcomes.Failed;
                                    }
                                    else
                                        buildData.BuildingLog.Outcome = BuildingOutcomes.Ignored;
                                }
                                break;
                            case ManipolationTypes.Undefined:
                            default:
                                {
                                    buildData.BuildingLog.FurtherErrorMessage = "If the manipolation type requested is undefined or unknown, " +
                                        "it is impossible to determinate its compatibility respeact at the input data!";
                                    buildData.BuildingLog.Outcome = BuildingOutcomes.Failed;
                                }
                                break;
                        }
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
        }

        private static IOutputModel? ManipulateInputData(IList<IInputRecord> extractedData, ManipolationTypes manipolationType)
        {
            IOutputModel? _output = manipolationType switch
            {
                ManipolationTypes.AsHeader => new ManipulatorHeader().Manipulate(extractedData),
                ManipolationTypes.AsSection1 => new ManipulatorSection1().Manipulate(extractedData),
                ManipolationTypes.AsSection3 => new ManipulatorSection3().Manipulate(extractedData),
                ManipolationTypes.AsSection4 => new ManipulatorSection4().Manipulate(extractedData),
                ManipolationTypes.AsSection8 => new ManipulatorSection8().Manipulate(extractedData),
                ManipolationTypes.AsSection9 => new ManipulatorSection9().Manipulate(extractedData),
                ManipolationTypes.AsSection10 => new ManipulatorSection10().Manipulate(extractedData),
                ManipolationTypes.AsSection11 => new ManipulatorSection11().Manipulate(extractedData),
                ManipolationTypes.AsSection12 => new ManipulatorSection12().Manipulate(extractedData),
                ManipolationTypes.AsSection13 => new ManipulatorSection13().Manipulate(extractedData),
                ManipolationTypes.AsSection14 => new ManipulatorSection14().Manipulate(extractedData),
                ManipolationTypes.AsSection15 => new ManipulatorSection15().Manipulate(extractedData),
                ManipolationTypes.AsSection16And17 => new ManipulatorSection16And17().Manipulate(extractedData),
                ManipolationTypes.AsSection18And19 => new ManipulatorSection18And19().Manipulate(extractedData),
                ManipolationTypes.AsSection20 => new ManipulatorSection20().Manipulate(extractedData),
                ManipolationTypes.AsFooter => new ManipulatorFooter().Manipulate(extractedData),
                _ => null,
            };
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
                    _serializedData = Model.Common.JsonUtility.JsonObjectSerialization<SectionsContainer>(outputDataContainer);
                    break;
            }
            return _serializedData ?? string.Empty;
        }

        public Builder()
        {            
            _manipolationTypesToManage = new List<(ManipolationTypes manipolationType, bool isMandatory)>()
            {
                ( ManipolationTypes.AsHeader, true ),
                ( ManipolationTypes.AsSection1, true ),
                ( ManipolationTypes.AsSection3, true ),
                ( ManipolationTypes.AsSection4, true ),
                ( ManipolationTypes.AsSection8, false ),
                ( ManipolationTypes.AsSection9, false ),
                ( ManipolationTypes.AsSection10, false ),
                ( ManipolationTypes.AsSection11, false ),
                ( ManipolationTypes.AsSection12, false ),
                ( ManipolationTypes.AsSection13, false ),
                ( ManipolationTypes.AsSection14, false ),
                ( ManipolationTypes.AsSection15, false ),
                ( ManipolationTypes.AsSection16And17, false ),
                ( ManipolationTypes.AsSection18And19, false ),
                ( ManipolationTypes.AsSection20, false ),
                ( ManipolationTypes.AsFooter, true )
            };           
        }

        public IBuiltData Build(IList<IInputRecord> extractedData, BuildFormats formatToBuild)
        {
            IBuiltData _buildData = new BuiltData();
            IList<IOutputModel> _sections = new List<IOutputModel>();
            try
            {
                _buildData.BuildingLog.BuildingStart = DateTime.Now;
                foreach ((ManipolationTypes manipolationType, bool isMandatory) in _manipolationTypesToManage)
                {
                    CheckInputData(_buildData, extractedData, manipolationType, isMandatory, formatToBuild);
                    if (_buildData.BuildingLog.Outcome == BuildingOutcomes.Success)
                    {
                        IOutputModel? _output = ManipulateInputData(extractedData, manipolationType);
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
