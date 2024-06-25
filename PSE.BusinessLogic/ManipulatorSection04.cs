using System.Globalization;
using PSE.BusinessLogic.Common;
using PSE.BusinessLogic.Interfaces;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using PSE.Model.SupportTables;
using static PSE.Model.Common.Constants;
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection4 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection4(CultureInfo? culture = null) : base(ManipolationTypes.AsSection4, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section4 output = new()
            {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) && extractedData.Any(flt => flt.RecordType == nameof(PER)))
            {
                IHistoryEvolutionPerformanceCurrency historyEvo;
                IChartPerformanceEvolution chartEvo;
                ISection4Content sectionContent;
                decimal tmpInpOut;
                string tmpPeriod;
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<PER> perItems = extractedData.Where(flt => flt.RecordType == nameof(PER)).OfType<PER>();
                foreach (IDE ideItem in ideItems)
                {
                    if (perItems != null && perItems.Any(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2))
                    {
                        sectionContent = new Section4Content();
                        foreach (PER perItem in perItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2).OrderByDescending(ob => ob.StartDate_6))
                        {
                            if (perItem.StartDate_6 != null && perItem.EndDate_7 != null
                                && perItem.StartDate_6.Value.Year == perItem.EndDate_7.Value.Year
                                && perItem.StartDate_6.Value.Month == 1 && perItem.EndDate_7.Value.Month == 12
                                && perItem.StartDate_6.Value.Day == 1 && perItem.EndDate_7.Value.Day == 31)
                                tmpPeriod = perItem.StartDate_6.Value.Year.ToString();
                            else if (perItem.StartDate_6 != null && perItem.EndDate_7 != null)
                                tmpPeriod = perItem.StartDate_6.Value.ToString(DEFAULT_DATE_FORMAT, _culture) + "-" + perItem.EndDate_7.Value.ToString(DEFAULT_DATE_FORMAT, _culture);
                            else if (perItem.StartDate_6 != null)
                                tmpPeriod = perItem.StartDate_6.Value.ToString(DEFAULT_DATE_FORMAT, _culture);
                            else if (perItem.EndDate_7 != null)
                                tmpPeriod = perItem.EndDate_7.Value.ToString(DEFAULT_DATE_FORMAT, _culture);
                            else
                                tmpPeriod = string.Empty;
                            tmpInpOut = perItem.CashIn_10 != null ? perItem.CashIn_10.Value : 0;
                            tmpInpOut += perItem.CashOut_11 != null ? perItem.CashOut_11.Value : 0;
                            tmpInpOut += perItem.SecIn_12 != null ? perItem.SecIn_12.Value : 0;
                            tmpInpOut += perItem.SecOut_13 != null ? perItem.SecOut_13.Value : 0;
                            historyEvo = new HistoryEvolutionPerformanceCurrency()
                            {
                                Period = tmpPeriod,
                                PercentPerformance = perItem.TWR_14 != null ? perItem.TWR_14.Value : 0,
                                InitialAmount = perItem.StartValue_8 != null ? perItem.StartValue_8.Value : 0,
                                FinalAmount = perItem.EndValue_9 != null ? perItem.EndValue_9.Value : 0,
                                InputsOutputs = tmpInpOut
                            };
                            sectionContent.HistoryEvolutionPerformancesCurrency.Add(historyEvo);
                            chartEvo = new ChartPerformanceEvolution()
                            {
                                Year = tmpPeriod,
                                PercentPerformance = perItem.TWR_14 != null ? perItem.TWR_14.Value : 0
                            };
                            sectionContent.ChartPerformanceEvolutions.Insert(0, chartEvo);
                        }
                        output.Content = new Section4Content(sectionContent);
                    }
                }
            }
            return output;
        }

    }

}
