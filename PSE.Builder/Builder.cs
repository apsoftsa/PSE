using System.Xml.Serialization;
using PSE.Model.Events;
using PSE.Model.Input.Models;
using PSE.Model.Input.Interfaces;
using PSE.Model.Output;
using PSE.Model.Output.Models;
using PSE.Model.Output.Interfaces;
using PSE.BusinessLogic;
using PSE.BusinessLogic.Calculations;
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
        //private readonly ManipulatorSection9 _manSect9;
        //private readonly ManipulatorSection10 _manSect10;
        //private readonly ManipulatorSection11 _manSect11;
        private readonly ManipulatorSection080 _manSect080;
        //private readonly ManipulatorSection13 _manSect13;
        //private readonly ManipulatorSection14 _manSect14;
        private readonly ManipulatorSection090 _manSect090;
        //private readonly ManipulatorSection16And17 _manSect16And17;
        private readonly ManipulatorSection110 _manSect110;
        private readonly ManipulatorSection100 _manSect100;
        private readonly ManipulatorSection150 _manSect150;        
        private readonly ManipulatorSection160 _manSect160;
        private readonly ManipulatorSection170 _manSect170;
        private readonly ManipulatorSection130 _manSect130;
        private readonly ManipulatorSection190 _manSect190;
        private readonly ManipulatorSection200 _manSect200;
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
            /*
            _manSect9 = new();
            _manSect9.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect10 = new();
            _manSect10.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect11 = new();
            _manSect11.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            */
            _manSect080 = new(_calcSettings);
            _manSect080.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            /*
            _manSect13 = new(_calcSettings);
            _manSect13.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            _manSect14 = new(_calcSettings);
            _manSect14.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            */
            _manSect090 = new(_calcSettings);
            _manSect090.ExternalCodifyRequest += ExternalCodifyRequestManagement;
            //_manSect16And17 = new(_calcSettings);
            //_manSect16And17.ExternalCodifyRequest += ExternalCodifyRequestManagement;
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
            _manipolationTypesToManage = new List<(ManipolationTypes manipolationType, bool isMandatory)>()
            {
                ( ManipolationTypes.AsHeader, true ),
                ( ManipolationTypes.AsSection000, true ),
                ( ManipolationTypes.AsSection010, true ),
                ( ManipolationTypes.AsSection020, true ),
                ( ManipolationTypes.AsSection040, false ),
                ( ManipolationTypes.AsSection060, false ),
                ( ManipolationTypes.AsSection070, false ),
                //( ManipolationTypes.AsSection9, false ),
                //( ManipolationTypes.AsSection10, false ),
                //( ManipolationTypes.AsSection11, false ),
                //( ManipolationTypes.AsSection16And17, false ), // section with high priority (!)
                ( ManipolationTypes.AsSection080, false ),
                //( ManipolationTypes.AsSection13, false ),
                //( ManipolationTypes.AsSection14, false ),
                ( ManipolationTypes.AsSection090, false ),
                //( ManipolationTypes.AsSection16And17, false ),
                ( ManipolationTypes.AsSection110, false ),
                ( ManipolationTypes.AsSection100, false ),
                ( ManipolationTypes.AsSection150, false ),
                //( ManipolationTypes.AsSection22, false ),
                ( ManipolationTypes.AsSection160, false ),
                ( ManipolationTypes.AsSection170, false ),
                ( ManipolationTypes.AsSection130, false ),
                ( ManipolationTypes.AsSection190, false ),
                //( ManipolationTypes.AsSection26, false ),
                ( ManipolationTypes.AsSection200, false ),
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
                            case ManipolationTypes.AsSection200: {
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
                            //case ManipolationTypes.AsSection9:
                            //case ManipolationTypes.AsSection10:
                            //case ManipolationTypes.AsSection11:
                            case ManipolationTypes.AsSection080:
                            //case ManipolationTypes.AsSection13:
                            //case ManipolationTypes.AsSection14:
                            case ManipolationTypes.AsSection090:
                            //case ManipolationTypes.AsSection16And17:
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
                                            // ????
                                            //if ((manipolationType == ManipolationTypes.AsSection080 || manipolationType == ManipolationTypes.AsSection13 || manipolationType == ManipolationTypes.AsSection14)
                                            //    && isMandatory && extractedData.Any(flt => flt.RecordType == nameof(CUR)) == false)                                            
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
            IOutputModel? output = manipolationType switch
            {
                ManipolationTypes.AsHeader => _manHeader.Manipulate(extractedData),
                ManipolationTypes.AsSection000 => _manSect000.Manipulate(extractedData),
                ManipolationTypes.AsSection010 => _manSect010.Manipulate(extractedData),
                ManipolationTypes.AsSection020 => _manSect020.Manipulate(extractedData),
                ManipolationTypes.AsSection040 => _manSect040.Manipulate(extractedData),
                ManipolationTypes.AsSection060 => _manSect060.Manipulate(extractedData),
                ManipolationTypes.AsSection070 => _manSect070.Manipulate(extractedData, _manSect040.TotalAssets),
                /*
                ManipolationTypes.AsSection9 => _manSect9.Manipulate(extractedData),
                ManipolationTypes.AsSection10 => _manSect10.Manipulate(extractedData),
                ManipolationTypes.AsSection11 => _manSect11.Manipulate(extractedData),
                */
                ManipolationTypes.AsSection080 => _manSect080.Manipulate(extractedData, _manSect040.TotalAssets),
                //ManipolationTypes.AsSection13 => _manSect13.Manipulate(extractedData),
                //ManipolationTypes.AsSection14 => _manSect14.Manipulate(extractedData),
                ManipolationTypes.AsSection090 => _manSect090.Manipulate(extractedData, _manSect040.TotalAssets),
                //ManipolationTypes.AsSection16And17 => _manSect16And17.Manipulate(extractedData),
                ManipolationTypes.AsSection110 => _manSect110.Manipulate(extractedData),
                ManipolationTypes.AsSection100 => _manSect100.Manipulate(extractedData),
                ManipolationTypes.AsSection150 => _manSect150.Manipulate(extractedData),
                //ManipolationTypes.AsSection22 => _manSect22.Manipulate(extractedData),
                ManipolationTypes.AsSection160 => _manSect160.Manipulate(extractedData),
                ManipolationTypes.AsSection170 => _manSect170.Manipulate(extractedData),
                ManipolationTypes.AsSection130 => _manSect130.Manipulate(extractedData),
                ManipolationTypes.AsSection190 => _manSect190.Manipulate(extractedData),
                ManipolationTypes.AsSection200 => _manSect200.Manipulate(extractedData),
                ManipolationTypes.AsFooter => _manFooter.Manipulate(extractedData),
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

        public IBuiltData Build(IList<IInputRecord> extractedData, BuildFormats formatToBuild)
        {
            IBuiltData buildData = new BuiltData();
            IList<IOutputModel> sections = new List<IOutputModel>();
            try
            {
                buildData.BuildingLog.BuildingStart = DateTime.Now;
                foreach ((ManipolationTypes manipolationType, bool isMandatory) in _manipolationTypesToManage.OrderBy( ob => ob.manipolationType))
                {
                    CheckInputData(buildData, extractedData, manipolationType, isMandatory, formatToBuild);
                    if (buildData.BuildingLog.Outcome == BuildingOutcomes.Success)
                    {
                        IOutputModel? output = ManipulateInputData(extractedData, manipolationType);
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
