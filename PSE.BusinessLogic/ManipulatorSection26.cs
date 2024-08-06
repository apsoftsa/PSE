using System.Globalization;
using System.Linq;
using PSE.BusinessLogic.Common;
using PSE.BusinessLogic.Interfaces;
using PSE.Model.Events;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using PSE.Model.SupportTables;
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection26 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection26(CultureInfo? culture = null) : base(PositionClassifications.UNKNOWN, ManipolationTypes.AsSection26, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section26 output = new()
            {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) && extractedData.Any(flt => flt.RecordType == nameof(POS)))
            {
                ExternalCodifyRequestEventArgs extEventArgsPortfolio;
                ExternalCodifyRequestEventArgs extEventArgsService;
                ExternalCodifyRequestEventArgs extEventArgsAdvisor;
                ITypeInvestment typeInvestment;                
                ISection26Content sectionContent;
                Dictionary<string, object> propertyParams = new Dictionary<string, object>() { { nameof(IDE.Language_18), Model.Common.Constants.ITALIAN_LANGUAGE_CODE } };
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> posItems = extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>();
                foreach (IDE ideItem in ideItems)
                {
                    extEventArgsPortfolio = new ExternalCodifyRequestEventArgs(nameof(Section26), nameof(KeyInformation.Portfolio), ideItem.ModelCode_21, propertyParams);
                    OnExternalCodifyRequest(extEventArgsPortfolio);
                    if (!extEventArgsPortfolio.Cancel)
                    {
                        extEventArgsService = new ExternalCodifyRequestEventArgs(nameof(Section26), nameof(KeyInformation.Service), ideItem.Mandate_11, propertyParams);
                        OnExternalCodifyRequest(extEventArgsService);
                        if (!extEventArgsService.Cancel)
                        {
                            if (ManipulatorOperatingRules.CheckInputLanguage(ideItem.Language_18))
                                propertyParams[nameof(IDE.Language_18)] = ideItem.Language_18;
                            sectionContent = new Section26Content();
                            sectionContent.OverviewPortfolio.Add(new OverviewPortfolio()
                            {
                                CustomerName = ideItem.CustomerNameShort_5,
                                CustomerNumber = ideItem.CustomerNumber_2,
                                Portfolio = extEventArgsPortfolio.PropertyValue,
                                Service = extEventArgsService.PropertyValue,
                                RiskProfile = "[RiskProfile]" // not still recovered (!)
                            });
                            // Exclude 'informative positions'
                            IEnumerable<IGrouping<string, POS>> groupByCategory = posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2 && flt.SubCat3_14 != ((int)PositionClassifications.POSIZIONI_INFORMATIVE).ToString()).GroupBy(gb => gb.SubCat3_14).OrderBy(ob => ob.Key);
                            IEnumerable<IGrouping<string, POS>> groupBySubCategory = posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2 && flt.SubCat4_15.StartsWith(((int)PositionClassifications.POSIZIONI_INFORMATIVE).ToString()) == false).GroupBy(gb => gb.SubCat4_15).OrderBy(ob => ob.Key);
                            if (groupBySubCategory != null && groupBySubCategory.Any())
                            {
                                foreach (IGrouping<string, POS> category in groupByCategory)
                                {
                                    extEventArgsAdvisor = new ExternalCodifyRequestEventArgs(nameof(Section26), nameof(Asset.TypeInvestment), category.Key, propertyParams);
                                    OnExternalCodifyRequest(extEventArgsAdvisor);
                                    if (!extEventArgsAdvisor.Cancel)
                                    {
                                        typeInvestment = new TypeInvestment()
                                        {
                                            InvestmentType = extEventArgsAdvisor.PropertyValue,
                                            MarketValueReportingCurrency = Math.Round(category.Where(f => f.Amount1Base_23.HasValue).Sum(sum => sum.Amount1Base_23.Value), 2)
                                        };
                                        sectionContent.Investment.Add(typeInvestment);
                                    }
                                }
                                decimal? totalSum = sectionContent.Investment.Sum(sum => sum.MarketValueReportingCurrency);
                                if (totalSum != null && totalSum != 0)
                                {
                                    foreach (ITypeInvestment typeInv in sectionContent.Investment)
                                    {
                                        sectionContent.ChartGraphicalInvestment.Add(new GraphicalInvestment() { TypeInvestment = typeInv.InvestmentType, Percent = Math.Round((typeInv.MarketValueReportingCurrency / totalSum.Value * 100m).Value, 2) });
                                    }
                                } 
                                typeInvestment = new TypeInvestment()
                                {
                                    InvestmentType = "TOTAL INVESTMENTS",
                                    MarketValueReportingCurrency = Math.Round(totalSum.Value, 2),
                                };
                                sectionContent.Investment.Add(typeInvestment);
                                decimal sumAccrued = posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2 && flt.ProRataBase_56 != null).Sum(sum => sum.ProRataBase_56.Value);
                                typeInvestment = new TypeInvestment()
                                {
                                    InvestmentType = "ACCURED INTEREST",
                                    MarketValueReportingCurrency = Math.Round(sumAccrued, 2),
                                };
                                sectionContent.Investment.Add(typeInvestment);
                            }
                            // Take only 'informative positions' if exists
                            groupByCategory = posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2 && flt.SubCat3_14 == ((int)PositionClassifications.POSIZIONI_INFORMATIVE).ToString()).GroupBy(gb => gb.SubCat3_14).OrderBy(ob => ob.Key);
                            groupBySubCategory = posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2 && flt.SubCat4_15.StartsWith(((int)PositionClassifications.POSIZIONI_INFORMATIVE).ToString())).GroupBy(gb => gb.SubCat4_15).OrderBy(ob => ob.Key);
                            if (groupBySubCategory != null && groupBySubCategory.Any())
                            {
                                foreach (IGrouping<string, POS> subCategory in groupBySubCategory)
                                {
                                    sectionContent.InformationPosition.Add(new AnyCommitments() { MarketValueReportingCurrency = Math.Round(groupByCategory.First(flt => flt.Key == subCategory.First().SubCat3_14).Sum(sum => sum.Amount1Base_23).Value, 2) });
                                }
                            }
                            output.Content = new Section26Content(sectionContent);
                        }
                    }
                }
            }
            return output;
        }

    }

}
