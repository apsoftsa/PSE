using System.Globalization;
using PSE.BusinessLogic.Common;
using PSE.BusinessLogic.Interfaces;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using PSE.Model.SupportTables;
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection7 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection7(CultureInfo? culture = null) : base(PositionClassifications.UNKNOWN, ManipolationTypes.AsSection7, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section7 output = new()
            {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) && extractedData.Any(flt => flt.RecordType == nameof(POS)))
            {                
                ISection7Content sectionContent;
                IInvestment investment;
                IEnumerable<CUR> curItems;
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();                
                IEnumerable<POS> posItems = extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>();
                foreach (IDE ideItem in ideItems)
                {
                    sectionContent = new Section7Content();
                    curItems = extractedData.Where(flt => flt.RecordType == nameof(CUR)).OfType<CUR>().Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2);
                    IEnumerable<IGrouping<string, POS>> groupByCurrency = posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2).GroupBy(gb => gb.Currency1_17).OrderBy(ob => ob.Key);
                    if (groupByCurrency != null && groupByCurrency.Any())
                    {
                        foreach (IGrouping<string, POS> currency in groupByCurrency)
                        {                                                        
                            investment = new Investment()
                            {
                                Amount = Math.Round(currency.Where(f => f.Amount1Cur1_22.HasValue).Sum(sum => sum.Amount1Cur1_22).Value + currency.Where(f => f.ProRataBase_56.HasValue).Sum(sum => sum.ProRataBase_56).Value, 2),
                                Currency = currency.Key,
                                MarketValueReportingCurrency = Math.Round(currency.Where(f => f.Amount1Base_23.HasValue).Sum(sum => sum.Amount1Base_23).Value + currency.Where(f => f.ProRataBase_56.HasValue).Sum(sum => sum.ProRataBase_56).Value, 2),
                                Exchange = (curItems != null && curItems.Any(flt => flt.Currency_5 == currency.Key && flt.Rate_6 != null)) ? Math.Round(curItems.First(flt => flt.Currency_5 == currency.Key && flt.Rate_6 != null).Rate_6.Value, Model.Common.Constants.DEFAULT_MEANINGFUL_DECIMAL_DIGITS_FOR_CALCULATION) : 0
                            };
                            sectionContent.Investments.Add(investment);
                        }                        
                        decimal? totalAmount = sectionContent.Investments.Sum(sum => sum.MarketValueReportingCurrency);
                        if (totalAmount != null && totalAmount != 0)
                        {
                            foreach (IInvestment invToUpgrd in sectionContent.Investments)
                            {
                                invToUpgrd.PercentAsset = Math.Round((invToUpgrd.MarketValueReportingCurrency / totalAmount.Value * 100m).Value, 2);
                                sectionContent.ChartInvestments.Add(new ChartInvestment() { Currency = invToUpgrd.Currency, PercentAsset = invToUpgrd.PercentAsset });
                            }
                            investment = new Investment()
                            {
                                Amount = null,
                                Currency = "Total",
                                MarketValueReportingCurrency = Math.Round(totalAmount.Value, 2),
                                PercentAsset = 100m,
                                Exchange = null
                            };
                            sectionContent.Investments.Add(investment);
                        }                        
                    }
                    output.Content = new Section7Content(sectionContent);
                }
            }
            return output;
        }

    }

}
