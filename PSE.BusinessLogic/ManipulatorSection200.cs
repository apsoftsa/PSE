using System.Globalization;
using PSE.Model.Common;
using PSE.Model.Events;
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

namespace PSE.BusinessLogic {

    public class ManipulatorSection200 : ManipulatorBase, IManipulator {

        public ManipulatorSection200(CultureInfo? culture = null) : base(Enumerations.ManipolationTypes.AsSection200, culture) { }

        public override IOutputModel Manipulate(IPSEDictionaryService dictionaryService, IList<IInputRecord> extractedData, decimal? totalAssets = null) {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section200 tmpOutput = new() {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) && extractedData.Any(flt => flt.RecordType == nameof(POS))) {
                IEndExtractCustomer endExtractCustomer;
                IEndExtractInvestment endExtractInvestment;
                IEndExtractInvestmentChart endExtractInvestmentChart;
                ExternalCodifyRequestEventArgs extEventArgsPortfolio;
                ExternalCodifyRequestEventArgs extEventArgsService;
                ExternalCodifyRequestEventArgs extEventArgsAdvisor;
                string cultureCode;
                string categoryDescr;
                string prevCategory;
                string currCategory;
                Dictionary<string, object> propertyParams = new Dictionary<string, object>() { { nameof(IDE.Language_18), ITALIAN_LANGUAGE_CODE } };
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> posItems = extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>();
                foreach (IDE ideItem in ideItems) {
                    if (ManipulatorOperatingRules.CheckInputLanguage(ideItem.Language_18))
                        propertyParams[nameof(IDE.Language_18)] = ideItem.Language_18;
                    cultureCode = dictionaryService.GetCultureCodeFromLanguage(ideItem.Language_18);
                    extEventArgsService = new ExternalCodifyRequestEventArgs(nameof(Section200), nameof(EndExtractCustomer.EsgProfile), ideItem.Mandate_11, propertyParams);
                    OnExternalCodifyRequest(extEventArgsService);
                    if (!extEventArgsService.Cancel) {
                        extEventArgsPortfolio = new ExternalCodifyRequestEventArgs(nameof(Section200), nameof(EndExtractCustomer.Portfolio), ideItem.ModelCode_21, propertyParams);
                        OnExternalCodifyRequest(extEventArgsPortfolio);
                        if (!extEventArgsPortfolio.Cancel) {
                            endExtractCustomer = new EndExtractCustomer() {
                                CustomerID = AssignRequiredString(ideItem.CustomerId_6),
                                Customer = AssignRequiredString(ideItem.CustomerNameShort_5),
                                Portfolio = AssignRequiredString(extEventArgsPortfolio.PropertyValue),
                                EsgProfile = AssignRequiredString(extEventArgsService.PropertyValue),
                                RiskProfile = 0 // ??
                            };
                            tmpOutput.Content.SubSection20000 = new SubSection20000("");
                            tmpOutput.Content.SubSection20000.Content.Add(new EndExtractCustomer(endExtractCustomer));
                            IEnumerable<IGrouping<string, POS>> groupByCategory = posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2 && flt.SubCat3_14 != ((int)PositionClassifications.INFORMATION_POSITIONS).ToString()).GroupBy(gb => gb.SubCat3_14).OrderBy(ob => ob.Key);
                            if (groupByCategory != null && groupByCategory.Any()) {
                                categoryDescr = string.Empty;
                                prevCategory = string.Empty;
                                currCategory = string.Empty;
                                tmpOutput.Content.SubSection20010 = new SubSection20010("Investments");
                                foreach (IGrouping<string, POS> category in groupByCategory) {
                                    currCategory = category.First().SubCat3_14;
                                    if (currCategory != prevCategory) {
                                        categoryDescr = "(Unknown)";
                                        extEventArgsAdvisor = new ExternalCodifyRequestEventArgs(nameof(Section200), nameof(EndExtractInvestment.AssetClass), currCategory, propertyParams);
                                        OnExternalCodifyRequest(extEventArgsAdvisor);
                                        if (!extEventArgsAdvisor.Cancel)
                                            categoryDescr = extEventArgsAdvisor.PropertyValue;
                                        prevCategory = currCategory;
                                    }
                                    endExtractInvestment = new EndExtractInvestment() {
                                        MarketValueReportingCurrency = Math.Round(category.Sum(sum => sum.Amount1Base_23).Value, 2),
                                        AssetClass = categoryDescr,
                                        Class = CLASS_ENTRY
                                    };
                                    tmpOutput.Content.SubSection20010.Content.Add(endExtractInvestment);
                                }
                                decimal? totalSum = tmpOutput.Content.SubSection20010.Content.Sum(sum => sum.MarketValueReportingCurrency);
                                if (!totalSum.HasValue)
                                    totalSum = decimal.Zero;
                                foreach (IEndExtractInvestment assetToUpgrd in tmpOutput.Content.SubSection20010.Content) {

                                    assetToUpgrd.PercentInvestment = totalSum != 0 ? Math.Round((assetToUpgrd.MarketValueReportingCurrency / totalSum.Value * 100m).Value, 2) : 0m;
                                }
                                decimal sumAccrued = posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2 && flt.ProRataBase_56 != null).Sum(sum => sum.ProRataBase_56.Value);
                                endExtractInvestment = new EndExtractInvestment() {
                                    AssetClass = dictionaryService.GetTranslation("total_investments_upper", cultureCode),
                                    Class = CLASS_TOTAL,
                                    MarketValueReportingCurrency = Math.Round(totalSum.Value, 2),
                                    PercentInvestment = 100.0m
                                };
                                tmpOutput.Content.SubSection20010.Content.Add(endExtractInvestment);
                                endExtractInvestment = new EndExtractInvestment() {
                                    AssetClass = dictionaryService.GetTranslation("accrued_interest_upper", cultureCode),
                                    Class = CLASS_TOTAL,
                                    MarketValueReportingCurrency = Math.Round(sumAccrued, 2),
                                    PercentInvestment = totalSum != 0 ? Math.Round(sumAccrued / totalSum.Value * 100m, 2) : 0m
                                };
                                tmpOutput.Content.SubSection20010.Content.Add(endExtractInvestment);
                                endExtractInvestment = new EndExtractInvestment() {
                                    AssetClass = dictionaryService.GetTranslation("total_assets_upper", cultureCode),
                                    Class = CLASS_TOTAL,    
                                    MarketValueReportingCurrency = Math.Round(totalSum.Value + sumAccrued, 2),
                                    PercentInvestment = null
                                };
                                tmpOutput.Content.SubSection20010.Content.Add(endExtractInvestment);
                                categoryDescr = string.Empty;
                                prevCategory = string.Empty;
                                currCategory = string.Empty;
                                tmpOutput.Content.SubSection20020 = new SubSection20020("Investments chart");
                                foreach (IGrouping<string, POS> subCategory in groupByCategory) {
                                    currCategory = subCategory.First().SubCat3_14;
                                    if (currCategory != prevCategory) {
                                        categoryDescr = "(Unknown)";
                                        extEventArgsAdvisor = new ExternalCodifyRequestEventArgs(nameof(Section200), nameof(EndExtractInvestmentChart.AssetClass), currCategory, propertyParams);
                                        OnExternalCodifyRequest(extEventArgsAdvisor);
                                        if (!extEventArgsAdvisor.Cancel)
                                            categoryDescr = extEventArgsAdvisor.PropertyValue;
                                        prevCategory = currCategory;
                                    }
                                    endExtractInvestmentChart = new EndExtractInvestmentChart() {
                                        AssetClass = categoryDescr,
                                        PercentInvestment = totalSum != 0 ? Math.Round(subCategory.Where(f => f.Amount1Base_23.HasValue).Sum(sum => sum.Amount1Base_23.Value) / totalSum.Value * 100m, 2) : 0m
                                    };
                                    tmpOutput.Content.SubSection20020.Content.Add(endExtractInvestmentChart);
                                }
                            }
                        }
                    }
                }
            }            
            return new Section200(tmpOutput);
        }

    }

}
