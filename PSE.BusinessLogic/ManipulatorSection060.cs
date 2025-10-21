using System.Globalization;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using PSE.Model.SupportTables;
using PSE.Dictionary;
using PSE.BusinessLogic.Common;
using PSE.BusinessLogic.Interfaces;
using static PSE.Model.Common.Constants;
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection060 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection060(CultureInfo? culture = null) : base(PositionClassifications.UNKNOWN, ManipolationTypes.AsSection060, culture) { }

        public override IOutputModel Manipulate(IPSEDictionaryService dictionaryService, IList<IInputRecord> extractedData, decimal? totalAssets = null)
        {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section060 output = new()
            {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) && extractedData.Any(flt => flt.RecordType == nameof(POS)))
            {
                string cultureCode;
                ISection060Content sectionContent;
                IInvestmentCurrency investment;
                IEnumerable<CUR> curItems;
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();                
                IEnumerable<POS> posItems = extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>();
                foreach (IDE ideItem in ideItems)
                {
                    sectionContent = new Section060Content();
                    cultureCode = dictionaryService.GetCultureCodeFromLanguage(ideItem.Language_18);
                    curItems = extractedData.Where(flt => flt.RecordType == nameof(CUR)).OfType<CUR>().Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2);
                    IEnumerable<IGrouping<string, POS>> groupByCurrency = posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2).GroupBy(gb => gb.Currency1_17).OrderBy(ob => ob.Key);
                    if (groupByCurrency != null && groupByCurrency.Any())
                    {
                        sectionContent.SubSection6000 = new SubSection6000Content();
                        foreach (IGrouping<string, POS> currency in groupByCurrency)
                        {                                                        
                            investment = new InvestmentCurrency()
                            {
                                Amount = Math.Round(currency.Where(f => f.Amount1Cur1_22.HasValue).Sum(sum => sum.Amount1Cur1_22).Value + currency.Where(f => f.ProRataCur1_55.HasValue).Sum(sum => sum.ProRataCur1_55).Value, 2),
                                Currency = currency.Key,
                                PercentAsset = 0,
                                MarketValueReportingCurrency = Math.Round(currency.Where(f => f.Amount1Base_23.HasValue).Sum(sum => sum.Amount1Base_23).Value + currency.Where(f => f.ProRataBase_56.HasValue).Sum(sum => sum.ProRataBase_56).Value, 2),
                                Exchange = (curItems != null && curItems.Any(flt => flt.Currency_5 == currency.Key && flt.Rate_6 != null)) ? Math.Round(curItems.First(flt => flt.Currency_5 == currency.Key && flt.Rate_6 != null).Rate_6.Value, Model.Common.Constants.DEFAULT_MEANINGFUL_DECIMAL_DIGITS_FOR_CALCULATION) : 0
                            };
                            sectionContent.SubSection6000.Content.Add(investment);
                        }                        
                        decimal totalAmount = sectionContent.SubSection6000.Content.Where(f => f.MarketValueReportingCurrency.HasValue).Sum(sum => sum.MarketValueReportingCurrency.Value);
                        if (totalAmount != 0)
                        {
                            sectionContent.SubSection6010 = new SubSection6010Content();
                            foreach (IInvestmentCurrency invToUpgrd in sectionContent.SubSection6000.Content)
                            {
                                invToUpgrd.Class = CLASS_ENTRY;
                                invToUpgrd.PercentAsset = Math.Round(invToUpgrd.MarketValueReportingCurrency.Value / totalAmount * 100m, 2);
                                sectionContent.SubSection6010.Content.Add(new ChartInvestmentCurrency() { Currency = invToUpgrd.Currency, PercentAsset = invToUpgrd.PercentAsset });
                            }
                            investment = new InvestmentCurrency()
                            {
                                Amount = null,
                                Currency = dictionaryService.GetTranslation("total", cultureCode), 
                                MarketValueReportingCurrency = Math.Round(totalAmount, 2),
                                Class = CLASS_TOTAL,
                                PercentAsset = 100m,
                                Exchange = null
                            };
                            sectionContent.SubSection6000.Content.Add(investment);
                        }                        
                    }
                    output.Content = new Section060Content(sectionContent);
                }
            }
            return output;
        }

    }

}
