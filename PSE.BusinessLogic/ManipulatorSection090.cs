using System.Globalization;
using PSE.BusinessLogic.Calculations;
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

    public class ManipulatorSection090 : ManipulatorBase, IManipulator
    {

        private readonly SharesCalculation _calcShares;

        public ManipulatorSection090(CalculationSettings calcSettings, CultureInfo? culture = null) : base(new List<PositionClassifications>() { PositionClassifications.SHARES, PositionClassifications.DERIVATIVE_PRODUCTS_ON_SECURITIES }, ManipolationTypes.AsSection090, culture) 
        {
            _calcShares = new SharesCalculation(calcSettings);
        }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData, decimal? totalAssets = null)
        {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section090 output = new()
            {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) && extractedData.Any(flt => flt.RecordType == nameof(POS)))
            {
                ISection090Content sectionContent;
                ISummaryTo summaryTo;
                ISummaryBeginningYear summaryBeginningYear;
                ISummaryPurchase summaryPurchase;
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<CUR> curItems = extractedData.Where(flt => flt.RecordType == nameof(CUR)).OfType<CUR>();
                foreach (IDE ideItem in ideItems)
                {
                    sectionContent = new Section090Content();
                    IEnumerable<IGrouping<string, POS>> posItemsGroupBySubCat = extractedData.Where(flt => flt.AlreadyUsed == false && flt.RecordType == nameof(POS)).OfType<POS>().Where(fltSubCat => fltSubCat.CustomerNumber_2 == ideItem.CustomerNumber_2 && ManipulatorOperatingRules.IsRowDestinatedToManipulator(this, fltSubCat.SubCat4_15)).GroupBy(gb => gb.SubCat4_15).OrderBy(ob => ob.Key);
                    if (posItemsGroupBySubCat != null && posItemsGroupBySubCat.Any())
                    {
                        foreach (IGrouping<string, POS> subCategoryItems in posItemsGroupBySubCat)
                        {
                            switch ((PositionClassifications)int.Parse(subCategoryItems.Key))
                            {
                                case PositionClassifications.SHARES:
                                    {
                                        IShareDetail shareDetail;
                                        IEquityFundDetail equityFundDetail; 
                                        foreach (POS posItem in subCategoryItems)
                                        {
                                            if (string.IsNullOrEmpty(posItem.Category_11) == false && posItem.Category_11.Trim().EndsWith("FA")) { // equity funds
                                                if (sectionContent.SubSection9020 == null)
                                                    sectionContent.SubSection9020 = new EquityFundSubSection("Equity Funds");
                                                equityFundDetail = new EquityFundDetail() {
                                                    Currency = AssignRequiredString(posItem.Currency1_17),
                                                    CapitalMarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23),
                                                    TotalMarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23),
                                                    Amount = AssignRequiredDecimal(posItem.Quantity_28),
                                                    Description1 = AssignRequiredString(posItem.Description2_33),
                                                    Description2 = AssignRequiredString(posItem.Description1_32),
                                                    Description3 = BuildComposedDescription([AssignRequiredLong(posItem.NumSecurity_29).ToString(), AssignRequiredString(posItem.IsinIban_85)]),                                                      
                                                    PercentWeight = CalculatePercentWeight(totalAssets, posItem.Amount1Base_23),
                                                };
                                                summaryTo = new SummaryTo() {
                                                    ValuePrice = AssignRequiredDecimal(posItem.Quote_48),
                                                    ExchangeValue = (curItems != null && curItems.Any(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == equityFundDetail.Currency && flt.Rate_6 != null)) ? curItems.First(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == equityFundDetail.Currency && flt.Rate_6.HasValue).Rate_6.Value : 0,
                                                    PercentPrice = 0m,
                                                    ProfitLossNotRealizedValue = 0m
                                                };
                                                summaryBeginningYear = new SummaryBeginningYear() {
                                                    ValuePrice = AssignRequiredDecimal(posItem.BuyPriceAverage_87),
                                                    ExchangeValue = AssignRequiredDecimal(posItem.BuyExchangeRateAverage_88)
                                                };
                                                summaryPurchase = new SummaryPurchase() {
                                                    ValuePrice = AssignRequiredDecimal(posItem.BuyPriceHistoric_53),
                                                    ExchangeValue = AssignRequiredDecimal(posItem.BuyExchangeRateHistoric_66)
                                                };
                                                CalculateSummaries(summaryTo, summaryBeginningYear, summaryPurchase, posItem.QuoteType_51, curItems, posItem.Currency1_17, posItem.Quantity_28);
                                                equityFundDetail.SummaryTo.Add(summaryTo);
                                                equityFundDetail.SummaryBeginningYear.Add(summaryBeginningYear);
                                                equityFundDetail.SummaryPurchase.Add(summaryPurchase);
                                                sectionContent.SubSection9020.Content.Add(equityFundDetail);
                                            } else {
                                                if (sectionContent.SubSection9010 == null) 
                                                    sectionContent.SubSection9010 = new ShareSubSection("Shares");
                                                shareDetail = new ShareDetail() {
                                                    Currency = AssignRequiredString(posItem.Currency1_17),
                                                    Description1 = AssignRequiredString(posItem.Description1_32),
                                                    Description2 = BuildComposedDescription([AssignRequiredLong(posItem.NumSecurity_29).ToString(), AssignRequiredString(posItem.IsinIban_85)]),
                                                    Amount = AssignRequiredDecimal(posItem.Quantity_28),
                                                    CapitalMarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23),
                                                    PercentWeight = CalculatePercentWeight(totalAssets, posItem.Amount1Base_23),
                                                };
                                                summaryTo = new SummaryTo() {
                                                    ValuePrice = AssignRequiredDecimal(posItem.Quote_48),
                                                    ExchangeValue = (curItems != null && curItems.Any(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == shareDetail.Currency && flt.Rate_6 != null)) ? curItems.First(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == shareDetail.Currency && flt.Rate_6.HasValue).Rate_6.Value : 0,
                                                    PercentPrice = 0m,
                                                    ProfitLossNotRealizedValue = 0m
                                                };
                                                summaryBeginningYear = new SummaryBeginningYear() {
                                                    ValuePrice = AssignRequiredDecimal(posItem.BuyPriceAverage_87),
                                                    ExchangeValue = AssignRequiredDecimal(posItem.BuyExchangeRateAverage_88)
                                                };
                                                summaryPurchase = new SummaryPurchase() {
                                                    ValuePrice = AssignRequiredDecimal(posItem.BuyPriceHistoric_53),
                                                    ExchangeValue = AssignRequiredDecimal(posItem.BuyExchangeRateHistoric_66)
                                                };
                                                CalculateSummaries(summaryTo, summaryBeginningYear, summaryPurchase, posItem.QuoteType_51, curItems, posItem.Currency1_17, posItem.Quantity_28);
                                                shareDetail.SummaryTo.Add(summaryTo);
                                                shareDetail.SummaryBeginningYear.Add(summaryBeginningYear);
                                                shareDetail.SummaryPurchase.Add(summaryPurchase);
                                                sectionContent.SubSection9010.Content.Add(shareDetail);
                                            }
                                            posItem.AlreadyUsed = true;
                                        }                                        
                                        break;
                                    }
                                case PositionClassifications.DERIVATIVE_PRODUCTS_ON_SECURITIES: {
                                    IDerivateDetail derivateDetail;
                                    foreach (POS posItem in subCategoryItems) {
                                        if (sectionContent.SubSection9030 == null)
                                            sectionContent.SubSection9030 = new DerivateSubSection("Derivative products on securities");
                                        derivateDetail = new DerivateDetail() {
                                            Currency = AssignRequiredString(posItem.Currency1_17),
                                            CapitalMarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23),
                                            TotalMarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23),
                                            Amount = AssignRequiredDecimal(posItem.Quantity_28),
                                            Description1 = AssignRequiredLong(posItem.NumSecurity_29).ToString(),
                                            Description2 = AssignRequiredString(posItem.Description1_32),
                                            Description3 = AssignRequiredDate(posItem.CallaDate_38, _culture),
                                            PercentWeight = CalculatePercentWeight(totalAssets, posItem.Amount1Base_23),
                                        };
                                        summaryTo = new SummaryTo() {
                                            ValuePrice = AssignRequiredDecimal(posItem.Quote_48),
                                            ExchangeValue = (curItems != null && curItems.Any(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == derivateDetail.Currency && flt.Rate_6 != null)) ? curItems.First(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == derivateDetail.Currency && flt.Rate_6.HasValue).Rate_6.Value : 0,
                                            PercentPrice = 0m,
                                            ProfitLossNotRealizedValue = 0m
                                        };
                                        summaryBeginningYear = new SummaryBeginningYear() {
                                            ValuePrice = AssignRequiredDecimal(posItem.BuyPriceAverage_87),
                                            ExchangeValue = AssignRequiredDecimal(posItem.BuyExchangeRateAverage_88)
                                        };
                                        summaryPurchase = new SummaryPurchase() {
                                            ValuePrice = AssignRequiredDecimal(posItem.BuyPriceHistoric_53),
                                            ExchangeValue = AssignRequiredDecimal(posItem.BuyExchangeRateHistoric_66)
                                        };
                                        CalculateSummaries(summaryTo, summaryBeginningYear, summaryPurchase, posItem.QuoteType_51, curItems, posItem.Currency1_17, posItem.Quantity_28);
                                        derivateDetail.SummaryTo.Add(summaryTo);
                                        derivateDetail.SummaryBeginningYear.Add(summaryBeginningYear);
                                        derivateDetail.SummaryPurchase.Add(summaryPurchase);
                                        sectionContent.SubSection9030.Content.Add(derivateDetail);
                                        posItem.AlreadyUsed = true;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    output.Content = new Section090Content(sectionContent);                    
                }
            }
            return output;
        }

    }

}
