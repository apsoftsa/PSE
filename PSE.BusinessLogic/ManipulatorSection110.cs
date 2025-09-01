using System.Globalization;
using PSE.BusinessLogic.Calculations;
using PSE.BusinessLogic.Common;
using PSE.BusinessLogic.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Input.Interfaces;
using PSE.Model.Output.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.SupportTables;
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection110 : ManipulatorBase, IManipulator
    {

        private readonly OtherInvestmentsCalculation _calcOtherInvs;

        public ManipulatorSection110(CalculationSettings calcSettings, CultureInfo? culture = null) : base(new List<PositionClassifications>()
            {
                PositionClassifications.MIX_FUNDS,
                PositionClassifications.DERIVATIVE_PRODUCTS_FUTURES,
                PositionClassifications.REAL_ESTATE_INVESTMENTS_FUNDS,
                PositionClassifications.ALTERNATIVE_PRODUCTS_DIFFERENT
            },
            ManipolationTypes.AsSection110, culture) 
        {
            _calcOtherInvs = new OtherInvestmentsCalculation(calcSettings);
        }        

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData, decimal? totalAssets = null) {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section110 output = new() {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) && extractedData.Any(flt => flt.RecordType == nameof(POS))) {
                decimal customerSumAmounts;
                IInvestmentDetail investmentDetail;
                IBondInvestmentDetail bondInvestmentDetail;
                ISection110Content sectionContent;
                ISummaryTo summaryTo;
                ISummaryBeginningYear summaryBeginningYear;
                ISummaryPurchase summaryPurchase;
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<CUR> curItems = extractedData.Where(flt => flt.RecordType == nameof(CUR)).OfType<CUR>();
                foreach (IDE ideItem in ideItems) {
                    sectionContent = new Section110Content();
                    customerSumAmounts = extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>().Where(subFlt => subFlt.CustomerNumber_2 == ideItem.CustomerNumber_2 && subFlt.Amount1Base_23.HasValue).Sum(sum => sum.Amount1Base_23.Value);
                    IEnumerable<IGrouping<string, POS>> posItemsGroupBySubCat = extractedData.Where(flt => flt.AlreadyUsed == false && flt.RecordType == nameof(POS)).OfType<POS>().Where(fltSubCat => fltSubCat.CustomerNumber_2 == ideItem.CustomerNumber_2 && ManipulatorOperatingRules.IsRowDestinatedToManipulator(this, fltSubCat.SubCat4_15)).GroupBy(gb => gb.SubCat4_15).OrderBy(ob => ob.Key);
                    if (posItemsGroupBySubCat != null && posItemsGroupBySubCat.Any()) {
                        foreach (IGrouping<string, POS> subCategoryItems in posItemsGroupBySubCat) {
                            switch ((PositionClassifications)int.Parse(subCategoryItems.Key)) {
                                case PositionClassifications.MIX_FUNDS: {
                                        foreach (POS posItem in subCategoryItems) {
                                            if (string.IsNullOrEmpty(posItem.Category_11) == false && posItem.Category_11.Trim().EndsWith("FM")) { // Metal funds
                                                if (sectionContent.SubSection11000 == null)
                                                    sectionContent.SubSection11000 = new SubSection11000("Mix funds");
                                                investmentDetail = new InvestmentDetail() {
                                                    Currency = AssignRequiredString(posItem.Currency1_17),
                                                    Amount = AssignRequiredDecimal(posItem.Quantity_28),                                                    
                                                    CapitalMarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23),
                                                    PercentWeight = CalculatePercentWeight(totalAssets, posItem.Amount1Base_23),
                                                    TotalMarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23),
                                                    Description1 = AssignRequiredString(posItem.Description2_33),
                                                    Description2 = AssignRequiredString(posItem.Description1_32),
                                                    Description3 = BuildComposedDescription([AssignRequiredLong(posItem.NumSecurity_29).ToString(), AssignRequiredString(posItem.IsinIban_85)])
                                                };
                                                summaryTo = new SummaryTo() {
                                                    ValueDate = AssignRequiredDate(posItem.CallaDate_38, _culture),
                                                    ValuePrice = AssignRequiredDecimal(posItem.Quote_48),
                                                    ExchangeValue = (curItems != null && curItems.Any(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == investmentDetail.Currency && flt.Rate_6 != null)) ? curItems.First(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == investmentDetail.Currency && flt.Rate_6.HasValue).Rate_6.Value : 0,
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
                                                investmentDetail.SummaryTo.Add(summaryTo);
                                                investmentDetail.SummaryBeginningYear.Add(summaryBeginningYear);
                                                investmentDetail.SummaryPurchase.Add(summaryPurchase);
                                                sectionContent.SubSection11000.Content.Add(investmentDetail);
                                            }
                                            posItem.AlreadyUsed = true;
                                        }
                                    }
                                    break;
                                case PositionClassifications.DERIVATIVE_PRODUCTS_FUTURES: {
                                        sectionContent.SubSection11020 = new SubSection11020("Derivative Products – Futures");
                                        foreach (POS posItem in subCategoryItems) {
                                            investmentDetail = new InvestmentDetail() {
                                                Currency = AssignRequiredString(posItem.Currency1_17),
                                                Amount = AssignRequiredDecimal(posItem.Quantity_28),
                                                CapitalMarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23),
                                                PercentWeight = CalculatePercentWeight(totalAssets, posItem.Amount1Base_23),
                                                TotalMarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23),
                                                Description1 = AssignRequiredString(posItem.Description2_33),
                                                Description2 = AssignRequiredString(posItem.Description1_32),
                                                Description3 = BuildComposedDescription([AssignRequiredLong(posItem.NumSecurity_29).ToString(), AssignRequiredString(posItem.IsinIban_85)])
                                            };
                                            summaryTo = new SummaryTo() {
                                                ValueDate = AssignRequiredDate(posItem.CallaDate_38, _culture),
                                                ValuePrice = AssignRequiredDecimal(posItem.Quote_48),
                                                ExchangeValue = (curItems != null && curItems.Any(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == investmentDetail.Currency && flt.Rate_6 != null)) ? curItems.First(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == investmentDetail.Currency && flt.Rate_6.HasValue).Rate_6.Value : 0,
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
                                            investmentDetail.SummaryTo.Add(summaryTo);
                                            investmentDetail.SummaryBeginningYear.Add(summaryBeginningYear);
                                            investmentDetail.SummaryPurchase.Add(summaryPurchase);
                                            sectionContent.SubSection11020.Content.Add(investmentDetail);
                                            posItem.AlreadyUsed = true;
                                        }
                                    }
                                    break;
                                case PositionClassifications.ALTERNATIVE_PRODUCTS_DIFFERENT: {
                                    sectionContent.SubSection11010 = new SubSection11010("Alternative Products – Different");
                                    foreach (POS posItem in subCategoryItems) {
                                        bondInvestmentDetail = new BondInvestmentDetail() {
                                            Currency = AssignRequiredString(posItem.Currency1_17),
                                            CapitalMarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23),
                                            NominalAmount = AssignRequiredDecimal(posItem.Quantity_28),
                                            Description1 = AssignRequiredString(posItem.Description2_33),
                                            Description2 = BuildComposedDescription([AssignRequiredDate(posItem.IssueDate_46, _culture), AssignRequiredDate(posItem.MaturityDateObbl_84, _culture), AssignRequiredString(posItem.Description1_32)]),
                                            Description3 = BuildComposedDescription([AssignRequiredLong(posItem.NumSecurity_29).ToString(), AssignRequiredString(posItem.IsinIban_85)]),
                                            Coupon = GetCoupon(AssignRequiredString(posItem.CouponFrequency_34), AssignRequiredString(posItem.CouponText_35)),
                                            PercentRate = AssignRequiredDecimal(posItem.InterestRate_47),
                                            InterestMarketValueReportingCurrency = AssignRequiredDecimal(posItem.ProRataBase_56),
                                            PercentWeight = CalculatePercentWeight(totalAssets, posItem.Amount1Base_23)
                                        };
                                        bondInvestmentDetail.TotalMarketValueReportingCurrency = bondInvestmentDetail.CapitalMarketValueReportingCurrency + bondInvestmentDetail.InterestMarketValueReportingCurrency;
                                        summaryTo = new SummaryTo() {
                                            ValueDate = AssignRequiredString(posItem.QuoteDate_49),
                                            ValuePrice = AssignRequiredDecimal(posItem.Quote_48),
                                            ExchangeValue = (curItems != null && curItems.Any(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == bondInvestmentDetail.Currency && flt.Rate_6 != null)) ? curItems.First(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == bondInvestmentDetail.Currency && flt.Rate_6.HasValue).Rate_6.Value : 0,
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
                                        bondInvestmentDetail.SummaryTo.Add(summaryTo);
                                        bondInvestmentDetail.SummaryBeginningYear.Add(summaryBeginningYear);
                                        bondInvestmentDetail.SummaryPurchase.Add(summaryPurchase);
                                        sectionContent.SubSection11010.Content.Add(bondInvestmentDetail);
                                        posItem.AlreadyUsed = true;
                                    }
                                }
                                break;
                                case PositionClassifications.REAL_ESTATE_INVESTMENTS_FUNDS: {
                                    sectionContent.SubSection11030 = new SubSection11030("Real estate investments – Real estate funds");
                                    foreach (POS posItem in subCategoryItems) {
                                        investmentDetail = new InvestmentDetail() {
                                            Currency = AssignRequiredString(posItem.Currency1_17),
                                            CapitalMarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23),
                                            Amount = AssignRequiredDecimal(posItem.Quantity_28),
                                            Description1 = AssignRequiredString(posItem.Description2_33),
                                            Description2 = AssignRequiredString(posItem.Description1_32),
                                            Description3 = BuildComposedDescription([AssignRequiredLong(posItem.NumSecurity_29).ToString(), AssignRequiredString(posItem.IsinIban_85)]),
                                            TotalMarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23),                                                  PercentWeight = CalculatePercentWeight(totalAssets, posItem.Amount1Base_23)
                                        };
                                        summaryTo = new SummaryTo() {                                                
                                            ValuePrice = AssignRequiredDecimal(posItem.Quote_48),
                                            PercentPrice = 0m,
                                            ExchangeValue = (curItems != null && curItems.Any(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == investmentDetail.Currency && flt.Rate_6 != null)) ? curItems.First(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == investmentDetail.Currency && flt.Rate_6.HasValue).Rate_6.Value : 0,
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
                                        investmentDetail.SummaryTo.Add(summaryTo);
                                        investmentDetail.SummaryBeginningYear.Add(summaryBeginningYear);
                                        investmentDetail.SummaryPurchase.Add(summaryPurchase);
                                        sectionContent.SubSection11030.Content.Add(investmentDetail);
                                        posItem.AlreadyUsed = true;
                                    }
                                }
                                break;
                            }
                        }
                    }
                    output.Content = new Section110Content(sectionContent);
                }
            }
            return output;
        }

    }

}
