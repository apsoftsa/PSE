using System.Xml.Serialization;
using PSE.BusinessLogic;
using PSE.BusinessLogic.Calculations;
using PSE.Dictionary;
using PSE.Model.Events;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using static PSE.BusinessLogic.Common.ManipulatorOperatingRules;
using static PSE.Model.Common.Enumerations;

namespace PSE.Builder
{

    public class Builder : IBuilder
    {

        private readonly CalculationSettings _calcSettings;
        private readonly ManipulatorHeader _manHeader;
        private readonly ManipulatorSection000 _manSect000;
        private readonly ManipulatorSection010 _manSect010;
        private readonly ManipulatorSection020 _manSect020;
        private readonly ManipulatorSection040 _manSect040;
        private readonly ManipulatorSection060 _manSect060;
        private readonly ManipulatorSection070 _manSect070;
        private readonly ManipulatorSection080 _manSect080;
        private readonly ManipulatorSection090 _manSect090;
        private readonly ManipulatorSection110 _manSect110;
        private readonly ManipulatorSection100 _manSect100;
        private readonly ManipulatorSection150 _manSect150;        
        private readonly ManipulatorSection160 _manSect160;
        private readonly ManipulatorSection170 _manSect170;
        private readonly ManipulatorSection130 _manSect130;
        private readonly ManipulatorSection190 _manSect190;
        private readonly ManipulatorSection200 _manSect200;
        private readonly ManipulatorFooter _manFooter;

        private readonly List<(ManipolationTypes manipolationType, bool isMandatory, int sequence)> _manipolationTypesToManage;

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
            _manSect000 = new();
            _manSect000.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect010 = new();
            _manSect010.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect020 = new();
            _manSect020.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect040 = new();
            _manSect040.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect060 = new();
            _manSect060.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect070 = new();
            _manSect070.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect080 = new(_calcSettings);
            _manSect080.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect090 = new(_calcSettings);
            _manSect090.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect110 = new(_calcSettings);
            _manSect110.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect100 = new();
            _manSect100.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect150 = new();
            _manSect150.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect160 = new();
            _manSect160.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect170 = new();
            _manSect170.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect130 = new();
            _manSect130.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect190 = new();
            _manSect190.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect200 = new();
            _manSect200.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manFooter = new();
            _manFooter.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manipolationTypesToManage = new List<(ManipolationTypes manipolationType, bool isMandatory, int sequence)>()
            {
                ( ManipolationTypes.AsHeader, true, 0),
                ( ManipolationTypes.AsSection000, true, 1),
                ( ManipolationTypes.AsSection010, true, 3),
                ( ManipolationTypes.AsSection020, true , 4),
                ( ManipolationTypes.AsSection040, false, 2),
                ( ManipolationTypes.AsSection060, false, 5),
                ( ManipolationTypes.AsSection070, false, 6),
                ( ManipolationTypes.AsSection080, false, 7),
                ( ManipolationTypes.AsSection090, false, 8),
                ( ManipolationTypes.AsSection110, false, 9),
                ( ManipolationTypes.AsSection100, false, 10),
                ( ManipolationTypes.AsSection150, false, 11),
                ( ManipolationTypes.AsSection160, false, 12),
                ( ManipolationTypes.AsSection170, false, 13),
                ( ManipolationTypes.AsSection130, false, 14),
                ( ManipolationTypes.AsSection190, false, 15),
                ( ManipolationTypes.AsSection200, false, 16),
                ( ManipolationTypes.AsFooter, true, 17)
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
                                    if (extractedData.Any(flt => flt.RecordType == nameof(IDE)))
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
                            case ManipolationTypes.AsSection000:
                                {
                                    if (extractedData.Any(flt => flt.RecordType == nameof(IDE)))
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
                            case ManipolationTypes.AsSection010:
                            case ManipolationTypes.AsSection020:
                            {
                                    if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) &&
                                        extractedData.Any(flt => flt.RecordType == nameof(PER)))
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
                            case ManipolationTypes.AsSection130:
                                {
                                    if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) &&
                                        extractedData.Any(flt => flt.RecordType == nameof(ORD)))
                                        buildData.BuildingLog.Outcome = BuildingOutcomes.Success;
                                    else if (isMandatory)
                                    {
                                        buildData.BuildingLog.FurtherErrorMessage = $"If the manipolation type requested is '{manipolationType}', " +
                                            $"at least one input record data source having the formats: '{nameof(IDE)}, {nameof(ORD)}' must be provided!";
                                        buildData.BuildingLog.Outcome = BuildingOutcomes.Failed;
                                    }
                                    else
                                        buildData.BuildingLog.Outcome = BuildingOutcomes.Ignored;
                                }
                                break;
                            case ManipolationTypes.AsSection040:
                            case ManipolationTypes.AsSection060:                            
                            case ManipolationTypes.AsSection160:
                            case ManipolationTypes.AsSection170:
                            case ManipolationTypes.AsSection200: 
                                {
                                    if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) &&
                                        extractedData.Any(flt => flt.RecordType == nameof(POS)))
                                    {
                                        if (manipolationType == ManipolationTypes.AsSection060 && extractedData.Any(flt => flt.RecordType == nameof(CUR)) == false)
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
                            case ManipolationTypes.AsSection070:
                            case ManipolationTypes.AsSection080:
                            case ManipolationTypes.AsSection090:
                            case ManipolationTypes.AsSection110:
                            case ManipolationTypes.AsSection100:
                            case ManipolationTypes.AsSection150:
                            case ManipolationTypes.AsSection190:
                                {
                                    if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) &&
                                        extractedData.Any(flt => flt.RecordType == nameof(POS)))
                                    {
                                        if (ArePOSRowsManipulable(extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>(), manipolationType))
                                        {
                                            if (isMandatory && extractedData.Any(flt => flt.RecordType == nameof(CUR)) == false)
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
                                            List<PositionClassifications> classificationsBound = GetClassificationsBoundToSection(manipolationType);
                                            string classificationsDescr = string.Empty;
                                            if (classificationsBound != null && classificationsBound.Any())
                                            {
                                                foreach (PositionClassifications classificationBound in classificationsBound)
                                                {
                                                    classificationsDescr += classificationBound.ToString() + " ";
                                                }
                                            }
                                            buildData.BuildingLog.FurtherErrorMessage = $"If the manipolation type requested is '{manipolationType}', " +
                                                $"at least one input record having format '{nameof(POS)}' and sub-category code '{classificationsDescr.Trim()}' must be provided!";
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
                                    if (extractedData.Any()) 
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

        private IOutputModel? ManipulateInputData(IPSEDictionaryService dictionaryService, IList<IInputRecord> extractedData, ManipolationTypes manipolationType)
        {
            IOutputModel? output = manipolationType switch
            {
                ManipolationTypes.AsHeader => _manHeader.Manipulate(dictionaryService, extractedData),
                ManipolationTypes.AsSection000 => _manSect000.Manipulate(dictionaryService, extractedData),
                ManipolationTypes.AsSection010 => _manSect010.Manipulate(dictionaryService, extractedData, _manSect040.TotalAssets),
                ManipolationTypes.AsSection020 => _manSect020.Manipulate(dictionaryService, extractedData),
                ManipolationTypes.AsSection040 => _manSect040.Manipulate(dictionaryService, extractedData),
                ManipolationTypes.AsSection060 => _manSect060.Manipulate(dictionaryService, extractedData),
                ManipolationTypes.AsSection070 => _manSect070.Manipulate(dictionaryService, extractedData, _manSect040.TotalAssets),
                ManipolationTypes.AsSection080 => _manSect080.Manipulate(dictionaryService, extractedData, _manSect040.TotalAssets),
                ManipolationTypes.AsSection090 => _manSect090.Manipulate(dictionaryService, extractedData, _manSect040.TotalAssets),
                ManipolationTypes.AsSection100 => _manSect100.Manipulate(dictionaryService, extractedData, _manSect040.TotalAssets),
                ManipolationTypes.AsSection110 => _manSect110.Manipulate(dictionaryService, extractedData, _manSect040.TotalAssets),
                ManipolationTypes.AsSection130 => _manSect130.Manipulate(dictionaryService, extractedData),
                ManipolationTypes.AsSection150 => _manSect150.Manipulate(dictionaryService, extractedData),
                ManipolationTypes.AsSection160 => _manSect160.Manipulate(dictionaryService, extractedData),
                ManipolationTypes.AsSection170 => _manSect170.Manipulate(dictionaryService, extractedData),                
                ManipolationTypes.AsSection190 => _manSect190.Manipulate(dictionaryService, extractedData),
                ManipolationTypes.AsSection200 => _manSect200.Manipulate(dictionaryService, extractedData),
                ManipolationTypes.AsFooter => _manFooter.Manipulate(dictionaryService, extractedData),
                _ => null,
            };
            return output;
        }

        private static string SerializeOutputData(SectionsContainer outputDataContainer, BuildFormats formatToBuild)
        {
            string? serializedData = null;
            switch (formatToBuild)
            {
                case BuildFormats.Xml:
                    {
                        var serializer = new XmlSerializer(typeof(SectionsContainer));
                        using StringWriter textWriter = new();
                        serializer.Serialize(textWriter, outputDataContainer);
                        serializedData = textWriter.ToString();
                    }
                    break;
                case BuildFormats.Json:
                    serializedData = Model.Common.JsonUtility.JsonObjectSerialization<SectionsContainer>(outputDataContainer);
                    break;
            }
            return serializedData ?? string.Empty;
        }

        public IBuiltData Build(IPSEDictionaryService dictionaryService, IList<IInputRecord> extractedData, BuildFormats formatToBuild)
        {
            IBuiltData buildData = new BuiltData();
            IList<IOutputModel> sections = new List<IOutputModel>();
            try
            {
                buildData.BuildingLog.BuildingStart = DateTime.Now;
                foreach ((ManipolationTypes manipolationType, bool isMandatory, int sequence) in _manipolationTypesToManage.OrderBy( ob => ob.sequence))
                {
                    CheckInputData(buildData, extractedData, manipolationType, isMandatory, formatToBuild);
                    if (buildData.BuildingLog.Outcome == BuildingOutcomes.Success)
                    {
                        IOutputModel? output = ManipulateInputData(dictionaryService, extractedData, manipolationType);
                        if (output != null)
                            sections.Add(output);
                        else
                        {
                            buildData.BuildingLog.FurtherErrorMessage = "Input data manipulation failed, impossible to generate the output object!";
                            buildData.BuildingLog.Outcome = BuildingOutcomes.Failed;
                        }
                    }
                }
                buildData.OutputData = SerializeOutputData(new SectionsContainer(sections), formatToBuild);
                if (string.IsNullOrEmpty(buildData.OutputData))
                {
                    buildData.BuildingLog.FurtherErrorMessage = "Input data serialization failed, impossible to generate the output file!";
                    buildData.BuildingLog.Outcome = BuildingOutcomes.Failed;
                }
                else
                    buildData.BuildingLog.Outcome = BuildingOutcomes.Success;
            }
            catch (Exception ex)
            {
                buildData.BuildingLog.FurtherErrorMessage = "Unexpected error occurred! Please see inner exception for more details.";
                buildData.BuildingLog.ExceptionOccurred = ex;
                buildData.BuildingLog.Outcome = BuildingOutcomes.Failed;
            }
            finally
            {
                buildData.BuildingLog.BuildingEnd = DateTime.Now;
            }
            return buildData;
        }

    }

}
