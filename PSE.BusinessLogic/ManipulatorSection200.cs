using System.Globalization;
using PSE.BusinessLogic.Common;
using PSE.BusinessLogic.Interfaces;
using PSE.Model.Common;
using PSE.Model.Events;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using PSE.Model.SupportTables;
using static PSE.Model.Common.Constants;
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic {

    public class ManipulatorSection200 : ManipulatorBase, IManipulator {

        public ManipulatorSection200(CultureInfo? culture = null) : base(Enumerations.ManipolationTypes.AsSection200, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData, decimal? totalAssets = null) {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section200 output = new() {
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
                string categoryDescr;
                string prevCategory;
                string currCategory;
                Dictionary<string, object> propertyParams = new Dictionary<string, object>() { { nameof(IDE.Language_18), ITALIAN_LANGUAGE_CODE } };
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> posItems = extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>();
                foreach (IDE ideItem in ideItems) {
                    extEventArgsService = new ExternalCodifyRequestEventArgs(nameof(Section200), nameof(EndExtractCustomer.EsgProfile), ideItem.Mandate_11, propertyParams);
                    OnExternalCodifyRequest(extEventArgsService);
                    if (!extEventArgsService.Cancel) {
                        if (ManipulatorOperatingRules.CheckInputLanguage(ideItem.Language_18))
                            propertyParams[nameof(IDE.Language_18)] = ideItem.Language_18;
                            extEventArgsPortfolio = new ExternalCodifyRequestEventArgs(nameof(Section200), nameof(EndExtractCustomer.Portfolio), ideItem.ModelCode_21, propertyParams);
                            OnExternalCodifyRequest(extEventArgsPortfolio);
                        if (!extEventArgsPortfolio.Cancel) {
                            endExtractCustomer = new EndExtractCustomer() {
                                CustomerID = AssignRequiredString(ideItem.CustomerNumber_2),
                                Customer = AssignRequiredString(ideItem.CustomerNameShort_5),
                                Portfolio = AssignRequiredString(extEventArgsPortfolio.PropertyValue),
                                EsgProfile = AssignRequiredString(extEventArgsService.PropertyValue),
                                RiskProfile = "" // ??
                            };
                            output.Content.SubSection20000 = new SubSection20000("");
                            output.Content.SubSection20000.Content.Add(new EndExtractCustomer(endExtractCustomer));
                            IEnumerable<IGrouping<string, POS>> groupByCategory = posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2 && flt.SubCat3_14 != ((int)PositionClassifications.INFORMATION_POSITIONS).ToString()).GroupBy(gb => gb.SubCat3_14).OrderBy(ob => ob.Key);
                            if (groupByCategory != null && groupByCategory.Any()) {
                                categoryDescr = string.Empty;
                                prevCategory = string.Empty;
                                currCategory = string.Empty;
                                output.Content.SubSection20010 = new SubSection20010("Investments");
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
                                        AssetClass = categoryDescr
                                    };
                                    output.Content.SubSection20010.Content.Add(endExtractInvestment);
                                }
                                decimal? totalSum = output.Content.SubSection20010.Content.Sum(sum => sum.MarketValueReportingCurrency);
                                if (!totalSum.HasValue)
                                    totalSum = decimal.Zero;
                                foreach (IEndExtractInvestment assetToUpgrd in output.Content.SubSection20010.Content) {

                                    assetToUpgrd.PercentInvestment = totalSum != 0 ? Math.Round((assetToUpgrd.MarketValueReportingCurrency / totalSum.Value * 100m).Value, 2) : 0m;
                                }
                                decimal sumAccrued = posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2 && flt.ProRataBase_56 != null).Sum(sum => sum.ProRataBase_56.Value);
                                endExtractInvestment = new EndExtractInvestment() {
                                    AssetClass = "TOTAL INVESTMENTS",
                                    MarketValueReportingCurrency = Math.Round(totalSum.Value, 2),
                                    PercentInvestment = 100.0m
                                };
                                output.Content.SubSection20010.Content.Add(endExtractInvestment);
                                endExtractInvestment = new EndExtractInvestment() {
                                    AssetClass = "ACCRUED INTEREST",
                                    MarketValueReportingCurrency = Math.Round(sumAccrued, 2),
                                    PercentInvestment = totalSum != 0 ? Math.Round(sumAccrued / totalSum.Value * 100m, 2) : 0m
                                };
                                output.Content.SubSection20010.Content.Add(endExtractInvestment);
                                endExtractInvestment = new EndExtractInvestment() {
                                    AssetClass = "TOTAL ASSETS",
                                    MarketValueReportingCurrency = Math.Round(totalSum.Value + sumAccrued, 2),
                                    PercentInvestment = null
                                };
                                output.Content.SubSection20010.Content.Add(endExtractInvestment);
                                categoryDescr = string.Empty;
                                prevCategory = string.Empty;
                                currCategory = string.Empty;
                                output.Content.SubSection20020 = new SubSection20020("Investments chart");
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
                                    output.Content.SubSection20020.Content.Add(endExtractInvestmentChart);
                                }
                            }
                        }
                    }
                }
            }            
            return output;
        }

    }

}
