using System.Xml.Serialization;
using PSE.Model.Events;
using PSE.Model.Input.Models;
using PSE.Model.Input.Interfaces;
using PSE.Model.Output;
using PSE.Model.Output.Models;
using PSE.Model.Output.Interfaces;
using PSE.BusinessLogic;
using static PSE.BusinessLogic.Common.ManipulatorOperatingRules;
using static PSE.Model.Common.Enumerations;
using PSE.BusinessLogic.Utility;
using PSE.BusinessLogic.Calculations;

namespace PSE.Builder
{

    public class Builder : IBuilder
    {

        private readonly CalculationSettings _calcSettings;
        private readonly ManipulatorHeader _manHeader;
        private readonly ManipulatorSection1 _manSect1;
        private readonly ManipulatorSection3 _manSect3;
        private readonly ManipulatorSection4 _manSect4;
        private readonly ManipulatorSection6 _manSect6;
        private readonly ManipulatorSection7 _manSect7;
        private readonly ManipulatorSection8 _manSect8;
        private readonly ManipulatorSection9 _manSect9;
        private readonly ManipulatorSection10 _manSect10;
        private readonly ManipulatorSection11 _manSect11;
        private readonly ManipulatorSection12 _manSect12;
        private readonly ManipulatorSection13 _manSect13;
        private readonly ManipulatorSection14 _manSect14;
        private readonly ManipulatorSection15 _manSect15;
        private readonly ManipulatorSection16And17 _manSect16And17;
        private readonly ManipulatorSection18And19 _manSect18And19;
        private readonly ManipulatorSection20 _manSect20;
        private readonly ManipulatorSection23 _manSect23;
        private readonly ManipulatorFooter _manFooter;

        private readonly List<(ManipolationTypes manipolationType, bool isMandatory)> _manipolationTypesToManage;

        public delegate void ExternalCodifyEventHandler(object sender, ExternalCodifyRequestEventArgs e);
        public event ExternalCodifyEventHandler? ExternalCodifyRequest;

        private void ExternalCodifyRequestManagement(object sender, ExternalCodifyRequestEventArgs e)
        {
            ExternalCodifyRequest?.Invoke(sender, e);
        }

        public Builder()
        {
            _calcSettings = new CalculationSettings();
            _manHeader = new();
            _manHeader.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect1 = new();
            _manSect1.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect3 = new();
            _manSect3.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect4 = new();
            _manSect4.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect6 = new();
            _manSect6.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect7 = new();
            _manSect7.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect8 = new();
            _manSect8.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect9 = new();
            _manSect9.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect10 = new();
            _manSect10.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect11 = new();
            _manSect11.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect12 = new(_calcSettings);
            _manSect12.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect13 = new(_calcSettings);
            _manSect13.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect14 = new(_calcSettings);
            _manSect14.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect15 = new(_calcSettings);
            _manSect15.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect16And17 = new(_calcSettings);
            _manSect16And17.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect18And19 = new(_calcSettings);
            _manSect18And19.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect20 = new();
            _manSect20.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect23 = new();
            _manSect23.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manFooter = new();
            _manFooter.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manipolationTypesToManage = new List<(ManipolationTypes manipolationType, bool isMandatory)>()
            {
                ( ManipolationTypes.AsHeader, true ),
                ( ManipolationTypes.AsSection1, true ),
                ( ManipolationTypes.AsSection3, true ),
                ( ManipolationTypes.AsSection4, true ),
                ( ManipolationTypes.AsSection6, false ),
                ( ManipolationTypes.AsSection7, false ),
                ( ManipolationTypes.AsSection8, false ),
                ( ManipolationTypes.AsSection9, false ),
                ( ManipolationTypes.AsSection10, false ),
                ( ManipolationTypes.AsSection11, false ),
                ( ManipolationTypes.AsSection16And17, false ), // section with high priority (!)
                ( ManipolationTypes.AsSection12, false ),
                ( ManipolationTypes.AsSection13, false ),
                ( ManipolationTypes.AsSection14, false ),
                ( ManipolationTypes.AsSection15, false ),
                //( ManipolationTypes.AsSection16And17, false ),
                ( ManipolationTypes.AsSection18And19, false ),
                ( ManipolationTypes.AsSection20, false ),
                ( ManipolationTypes.AsSection23, false ),
                ( ManipolationTypes.AsFooter, true )
            };
        }

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
                                    else if (isMandatory)
                                    {
                                        buildData.BuildingLog.FurtherErrorMessage = $"If the manipolation type requested is '{manipolationType}', " +
                                            $"at least one input record data source having the format ' {nameof(IDE)} ' must be provided!";
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
                                    else if (isMandatory)
                                    {
                                        buildData.BuildingLog.FurtherErrorMessage = $"If the manipolation type requested is '{manipolationType}', " +
                                            $"at least one input record data source having the formats: '{nameof(IDE)}, {nameof(PER)}' must be provided!";
                                        buildData.BuildingLog.Outcome = BuildingOutcomes.Failed;
                                    }
                                    else
                                        buildData.BuildingLog.Outcome = BuildingOutcomes.Ignored;
                                }
                                break;
                            case ManipolationTypes.AsSection6:
                            case ManipolationTypes.AsSection7:
                            case ManipolationTypes.AsSection23:
                                {
                                    if (extractedData.Any(_flt => _flt.RecordType == nameof(IDE)) &&
                                        extractedData.Any(_flt => _flt.RecordType == nameof(POS)))
                                    {
                                        if (manipolationType == ManipolationTypes.AsSection7 && extractedData.Any(_flt => _flt.RecordType == nameof(CUR)) == false)
                                        {
                                            if (isMandatory)
                                            {
                                                buildData.BuildingLog.FurtherErrorMessage = $"If the manipolation type requested is '{manipolationType}', " +
                                                $"at least one input record having format '{nameof(CUR)}' must be provided!";
                                                buildData.BuildingLog.Outcome = BuildingOutcomes.Failed;
                                            }
                                            else
                                                buildData.BuildingLog.Outcome = BuildingOutcomes.Ignored;
                                        }
                                        else
                                            buildData.BuildingLog.Outcome = BuildingOutcomes.Success;
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
                                        if (ArePOSRowsManipulable(extractedData.Where(_flt => _flt.RecordType == nameof(POS)).OfType<POS>(), manipolationType))
                                        {
                                            // ????
                                            //if ((manipolationType == ManipolationTypes.AsSection12 || manipolationType == ManipolationTypes.AsSection13 || manipolationType == ManipolationTypes.AsSection14)
                                            //    && isMandatory && extractedData.Any(_flt => _flt.RecordType == nameof(CUR)) == false)                                            
                                            if (isMandatory && extractedData.Any(_flt => _flt.RecordType == nameof(CUR)) == false)
                                            {
                                                buildData.BuildingLog.FurtherErrorMessage = $"If the manipolation type requested is '{manipolationType}', " +
                                                    $"at least one input record having format '{nameof(CUR)}' must be provided!";
                                                buildData.BuildingLog.Outcome = BuildingOutcomes.Failed;
                                            }
                                            else
                                                buildData.BuildingLog.Outcome = BuildingOutcomes.Success;
                                        }
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
                                                $"at least one input record having format '{nameof(POS)}' and sub-category code '{_classificationsDescr.Trim()}' must be provided!";
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

        private IOutputModel? ManipulateInputData(IList<IInputRecord> extractedData, ManipolationTypes manipolationType)
        {
            IOutputModel? _output = manipolationType switch
            {
                ManipolationTypes.AsHeader => _manHeader.Manipulate(extractedData),
                ManipolationTypes.AsSection1 => _manSect1.Manipulate(extractedData),
                ManipolationTypes.AsSection3 => _manSect3.Manipulate(extractedData),
                ManipolationTypes.AsSection4 => _manSect4.Manipulate(extractedData),
                ManipolationTypes.AsSection6 => _manSect6.Manipulate(extractedData),
                ManipolationTypes.AsSection7 => _manSect7.Manipulate(extractedData),
                ManipolationTypes.AsSection8 => _manSect8.Manipulate(extractedData),
                ManipolationTypes.AsSection9 => _manSect9.Manipulate(extractedData),
                ManipolationTypes.AsSection10 => _manSect10.Manipulate(extractedData),
                ManipolationTypes.AsSection11 => _manSect11.Manipulate(extractedData),
                ManipolationTypes.AsSection12 => _manSect12.Manipulate(extractedData),
                ManipolationTypes.AsSection13 => _manSect13.Manipulate(extractedData),
                ManipolationTypes.AsSection14 => _manSect14.Manipulate(extractedData),
                ManipolationTypes.AsSection15 => _manSect15.Manipulate(extractedData),
                ManipolationTypes.AsSection16And17 => _manSect16And17.Manipulate(extractedData),
                ManipolationTypes.AsSection18And19 => _manSect18And19.Manipulate(extractedData),
                ManipolationTypes.AsSection20 => _manSect20.Manipulate(extractedData),
                ManipolationTypes.AsSection23 => _manSect23.Manipulate(extractedData),
                ManipolationTypes.AsFooter => _manFooter.Manipulate(extractedData),
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
