using System.Globalization;
using PSE.Model.Events;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using PSE.Model.SupportTables;
using PSE.Dictionary;
using PSE.BusinessLogic.Common;
using PSE.BusinessLogic.Interfaces;
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection040 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection040(CultureInfo? culture = null) : base(PositionClassifications.UNKNOWN, ManipolationTypes.AsSection040, culture) { }

        public override IOutputModel Manipulate(IPSEDictionaryService dictionaryService, IList<IInputRecord> extractedData, decimal? totalAssets = null)
        {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section040 output = new()
            {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) && extractedData.Any(flt => flt.RecordType == nameof(POS)))
            {
                string cultureCode;
                ExternalCodifyRequestEventArgs extEventArgsAdvisor;
                IInvestmentAsset investmentAsset;                
                ISection040Content sectionContent;
                Dictionary<string, object> propertyParams = new Dictionary<string, object>() { { nameof(IDE.Language_18), Model.Common.Constants.ITALIAN_LANGUAGE_CODE } };
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> posItems = extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>();
                foreach (IDE ideItem in ideItems)
                {
                    if (ManipulatorOperatingRules.CheckInputLanguage(ideItem.Language_18))
                        propertyParams[nameof(IDE.Language_18)] = ideItem.Language_18;
                    cultureCode = dictionaryService.GetCultureCodeFromLanguage(ideItem.Language_18); 
                    sectionContent = new Section040Content();
                    // Exclude 'informative positions'
                    IEnumerable<IGrouping<string, POS>> groupByCategory = posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2 && flt.SubCat3_14 != ((int)PositionClassifications.INFORMATION_POSITIONS).ToString()).GroupBy(gb => gb.SubCat3_14).OrderBy(ob => ob.Key);
                    IEnumerable<IGrouping<string, POS>> groupBySubCategory = posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2 && flt.SubCat4_15.StartsWith(((int)PositionClassifications.INFORMATION_POSITIONS).ToString()) == false).GroupBy(gb => gb.SubCat4_15).OrderBy(ob => ob.Key);
                    if (groupBySubCategory != null && groupBySubCategory.Any())
                    {
                        string categoryDescr = string.Empty;
                        string prevCategory = string.Empty;
                        string currCategory = string.Empty;
                        sectionContent.SubSection4000 = new SubSection4000Content();
                        foreach (IGrouping<string, POS> subCategory in groupBySubCategory)
                        {                            
                            currCategory = subCategory.First().SubCat3_14;
                            if (currCategory != prevCategory)
                            {
                                categoryDescr = "(Unknown)";
                                extEventArgsAdvisor = new ExternalCodifyRequestEventArgs(nameof(Section040), nameof(InvestmentAsset.AssetClass), currCategory, propertyParams);
                                OnExternalCodifyRequest(extEventArgsAdvisor);
                                if (!extEventArgsAdvisor.Cancel)
                                    categoryDescr = extEventArgsAdvisor.PropertyValue;
                                prevCategory = currCategory;
                            }
                            extEventArgsAdvisor = new ExternalCodifyRequestEventArgs(nameof(Section040), nameof(InvestmentAsset.TypeInvestment), subCategory.Key, propertyParams);
                            OnExternalCodifyRequest(extEventArgsAdvisor);
                            if (!extEventArgsAdvisor.Cancel)
                            {
                                investmentAsset = new InvestmentAsset()
                                {
                                    MarketValueReportingCurrency = Math.Round(subCategory.Sum(sum => sum.Amount1Base_23).Value, 2),
                                    AssetClass = categoryDescr,
                                    TypeInvestment = extEventArgsAdvisor.PropertyValue,
                                    MarketValueReportingCurrencyT = Math.Round(groupByCategory.First(flt => flt.Key == subCategory.First().SubCat3_14).Sum(sum => sum.Amount1Base_23).Value, 2)
                                };
                                sectionContent.SubSection4000.Content.Add(investmentAsset);
                            }                            
                        }
                        decimal? totalSum = sectionContent.SubSection4000.Content.Sum(sum => sum.MarketValueReportingCurrency);
                        if(!totalSum.HasValue) totalSum = decimal.Zero;
                        sectionContent.SubSection4010 = new SubSection4010Content();
                        foreach (IInvestmentAsset assetToUpgrd in sectionContent.SubSection4000.Content)
                        {

                            assetToUpgrd.PercentInvestment = totalSum != 0 ? Math.Round((assetToUpgrd.MarketValueReportingCurrency / totalSum.Value * 100m).Value, 2) : 0m;
                        }
                        IEnumerable<IGrouping<string, IInvestmentAsset?>> groupByAssetClasses = sectionContent.SubSection4000.Content.GroupBy(gb => gb.AssetClass);
                        foreach (IGrouping<string, IInvestmentAsset> groupByAssetClass in groupByAssetClasses)
                        {
                            foreach(IInvestmentAsset assetToUpgrd in sectionContent.SubSection4000.Content.Where(flt => flt.AssetClass == groupByAssetClass.Key))
                            {
                                assetToUpgrd.PercentInvestmentT = groupByAssetClass.Sum(sum => sum.PercentInvestment);
                            }
                            sectionContent.SubSection4010.Content.Add(new AssetClassChart() { AssetClass = groupByAssetClass.Key, PercentInvestment = groupByAssetClass.Where(f => f.PercentInvestment.HasValue).Sum(sum => sum.PercentInvestment.Value) });
                        }
                        decimal sumAccrued = posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2 && flt.ProRataBase_56 != null).Sum(sum => sum.ProRataBase_56.Value);
                        investmentAsset = new InvestmentAsset() {
                            AssetClass = dictionaryService.GetTranslation("total_investments_upper", cultureCode),
                            MarketValueReportingCurrencyT = Math.Round(totalSum.Value, 2),
                            PercentInvestmentT = 100.0m,
                            MarketValueReportingCurrency = null,
                            PercentInvestment = null,
                            TypeInvestment = null
                        };                            
                        sectionContent.SubSection4000.Content.Add(investmentAsset);
                        investmentAsset = new InvestmentAsset() {
                            AssetClass = dictionaryService.GetTranslation("total_investments_upper", cultureCode),
                            TypeInvestment = dictionaryService.GetTranslation("accrued_interest_upper", cultureCode),
                            MarketValueReportingCurrency = Math.Round(sumAccrued, 2),
                            PercentInvestment = totalSum != 0 ? Math.Round(sumAccrued / totalSum.Value * 100m, 2) : 0m,
                            MarketValueReportingCurrencyT = null,
                            PercentInvestmentT = null
                        };
                        sectionContent.SubSection4000.Content.Add(investmentAsset);
                        investmentAsset = new InvestmentAsset() {
                            AssetClass = dictionaryService.GetTranslation("total_assets_upper", cultureCode),
                            //MarketValueReportingCurrency = Math.Round(totalSum.Value + sumAccrued, 2),
                            MarketValueReportingCurrency = null,
                            TypeInvestment = null,                                
                            PercentInvestment = null,
                            //MarketValueReportingCurrencyT = null,
                            MarketValueReportingCurrencyT = Math.Round(totalSum.Value + sumAccrued, 2),
                            PercentInvestmentT = null
                        };
                        sectionContent.SubSection4000.Content.Add(investmentAsset);
                        TotalAssets = AssignRequiredDecimals(sumAccrued, totalSum);                        
                    }
                    // Take only 'informative positions' if exists
                    groupByCategory = posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2 && flt.SubCat3_14 == ((int)PositionClassifications.INFORMATION_POSITIONS).ToString()).GroupBy(gb => gb.SubCat3_14).OrderBy(ob => ob.Key);
                    groupBySubCategory = posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2 && flt.SubCat4_15.StartsWith(((int)PositionClassifications.INFORMATION_POSITIONS).ToString())).GroupBy(gb => gb.SubCat4_15).OrderBy(ob => ob.Key);
                    if (groupBySubCategory != null && groupBySubCategory.Any())
                    {
                        string categoryDescr = string.Empty;
                        string prevCategory = string.Empty;
                        string currCategory = string.Empty;
                        foreach (IGrouping<string, POS> subCategory in groupBySubCategory)
                        {
                            currCategory = subCategory.First().SubCat3_14;
                            if (currCategory != prevCategory)
                            {
                                categoryDescr = "(Unknown)";
                                extEventArgsAdvisor = new ExternalCodifyRequestEventArgs(nameof(Section040), nameof(InvestmentAsset.AssetClass), currCategory, propertyParams);
                                OnExternalCodifyRequest(extEventArgsAdvisor);
                                if (!extEventArgsAdvisor.Cancel)
                                    categoryDescr = extEventArgsAdvisor.PropertyValue;
                                prevCategory = currCategory;
                            }
                            extEventArgsAdvisor = new ExternalCodifyRequestEventArgs(nameof(Section040), nameof(InvestmentAsset.TypeInvestment), subCategory.Key, propertyParams);
                            OnExternalCodifyRequest(extEventArgsAdvisor);
                            if (!extEventArgsAdvisor.Cancel)
                            {
                                investmentAsset = new InvestmentAsset()
                                {
                                    MarketValueReportingCurrency = Math.Round(subCategory.Sum(sum => sum.Amount1Base_23).Value, 2),
                                    AssetClass = categoryDescr,
                                    TypeInvestment = extEventArgsAdvisor.PropertyValue,
                                    MarketValueReportingCurrencyT = Math.Round(groupByCategory.First(flt => flt.Key == subCategory.First().SubCat3_14).Sum(sum => sum.Amount1Base_23).Value, 2)
                                };
                                sectionContent.SubSection4000.Content.Add(investmentAsset);
                            }
                        }
                    }
                    output.Content = new Section040Content(sectionContent);
                }
            }
            return output;
        }

    }

}
