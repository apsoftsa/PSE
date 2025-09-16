using PSE.BusinessLogic.Calculations;
using PSE.BusinessLogic.Common;
using PSE.BusinessLogic.Interfaces;
using PSE.Model.Events;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using PSE.Model.SupportTables;
using System.Globalization;
using static PSE.Model.Common.Constants;
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection080 : ManipulatorBase, IManipulator
    {
        
        private readonly BondsCalculation _calcBonds;

        public ManipulatorSection080(CalculationSettings calcSettings, CultureInfo? culture = null) : base(new List<PositionClassifications>() { PositionClassifications.BONDS_WITH_MATURITY_MINOR_EQUAL_1_YEAR, PositionClassifications.BONDS_WITH_MATURITY_MINOR_EQUAL_5_YEARS, PositionClassifications.BONDS_WITH_MATURITY_MAJOR_THAN_5_YEARS_OR_FUNDS, PositionClassifications.CONVERTIBLE_BONDS_AND_BONDS_WITH_WARRANTS }, ManipolationTypes.AsSection080, culture) 
        {            
            _calcBonds = new BondsCalculation(calcSettings);
        }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData, decimal? totalAssets = null)
        {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section080 output = new()
            {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) && extractedData.Any(flt => flt.RecordType == nameof(POS)))
            {
                ISection080Content sectionContent;
                ExternalCodifyRequestEventArgs extEventArgsOperation;
                Dictionary<string, object> propertyParams = new Dictionary<string, object>() { { nameof(IDE.Language_18), ITALIAN_LANGUAGE_CODE } };
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<CUR> curItems = extractedData.Where(flt => flt.RecordType == nameof(CUR)).OfType<CUR>();
                foreach (IDE ideItem in ideItems)
                {
                    if (ManipulatorOperatingRules.CheckInputLanguage(ideItem.Language_18))
                        propertyParams[nameof(IDE.Language_18)] = ideItem.Language_18;
                    sectionContent = new Section080Content();
                    IEnumerable<IGrouping<string, POS>> posItemsGroupBySubCat = extractedData.Where(flt => flt.AlreadyUsed == false && flt.RecordType == nameof(POS)).OfType<POS>().Where(fltSubCat => fltSubCat.CustomerNumber_2 == ideItem.CustomerNumber_2 && ManipulatorOperatingRules.IsRowDestinatedToManipulator(this, fltSubCat.SubCat4_15)).GroupBy(gb => gb.SubCat4_15).OrderBy(ob => ob.Key);
                    if (posItemsGroupBySubCat != null && posItemsGroupBySubCat.Any())
                    {
                        foreach (IGrouping<string, POS> subCategoryItems in posItemsGroupBySubCat)
                        {
                            switch ((PositionClassifications)int.Parse(subCategoryItems.Key))
                            {
                                case PositionClassifications.BONDS_WITH_MATURITY_MINOR_EQUAL_1_YEAR:
                                    {
                                        IBondDetail bondLessThan1;
                                        ISummaryTo summaryTo;
                                        ISummaryBeginningYear summaryBeginningYear;
                                        ISummaryPurchase summaryPurchase;
                                        sectionContent.SubSection8000 = new BondSubSection("Bonds with maturity <= 1 year");
                                        foreach (POS posItem in subCategoryItems.OrderBy(ob => ob.NumSecurity_29)) {
                                            extEventArgsOperation = new ExternalCodifyRequestEventArgs(nameof(Section080), nameof(BondDetail.Coupon), AssignRequiredString(posItem.CouponFrequency_34), propertyParams);
                                            OnExternalCodifyRequest(extEventArgsOperation);
                                            if (!extEventArgsOperation.Cancel) {
                                                bondLessThan1 = new BondDetail() {
                                                    Currency = AssignRequiredString(posItem.Currency1_17),
                                                    NominalAmount = AssignRequiredDecimal(posItem.Quantity_28),
                                                    Description1 = BuildComposedDescription([AssignRequiredString(posItem.Description1_32), AssignRequiredString(posItem.Description2_33)]),
                                                    Description2 = BuildComposedDescription([AssignRequiredDate(posItem.IssueDate_46, _culture), AssignRequiredDate(posItem.MaturityDate_36, _culture)]),
                                                    Description3 = BuildComposedDescription([AssignRequiredLong(posItem.NumSecurity_29).ToString(), AssignRequiredString(posItem.IsinIban_85)]),
                                                    Coupon = GetCoupon(extEventArgsOperation.PropertyValue, AssignRequiredString(posItem.CouponText_35)),
                                                    PercentRate = AssignRequiredDecimal(posItem.InterestRate_47),
                                                    CapitalMarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23),
                                                    InterestMarketValueReportingCurrency = AssignRequiredDecimal(posItem.ProRataBase_56),
                                                    TotalMarketValueReportingCurrency = AssignRequiredDecimals(posItem.Amount1Base_23, posItem.ProRataBase_56),
                                                    Duration = decimal.TryParse(posItem.Duration_68, out decimal duration) ? duration : 0m,
                                                    SpRating = (string.IsNullOrEmpty(posItem.AgeRat_97) == false && posItem.AgeRat_97.Trim() == "SP") ? posItem.Rating_98 : "N/A",
                                                    PercentWeight = CalculatePercentWeight(totalAssets, posItem.Amount1Base_23),
                                                    PercentYTD = AssignRequiredDecimal(posItem.DirectRoi_64)
                                                };
                                                summaryTo = new SummaryTo() {
                                                    ValuePrice = AssignRequiredDecimal(posItem.Quote_48),
                                                    ValueDate = AssignRequiredDate(posItem.QuoteDate_49, _culture),
                                                    PercentPrice = 0m,
                                                    ExchangeValue = (curItems != null && curItems.Any(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == bondLessThan1.Currency && flt.Rate_6.HasValue)) ? curItems.First(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == bondLessThan1.Currency && flt.Rate_6 != null).Rate_6.Value : 0,
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
                                                bondLessThan1.TotalMarketValueReportingCurrency = bondLessThan1.CapitalMarketValueReportingCurrency + bondLessThan1.InterestMarketValueReportingCurrency;
                                                CalculateSummaries(ideItem.Currency_19, summaryTo, summaryBeginningYear, summaryPurchase, posItem.QuoteType_51, curItems, posItem.Currency1_17, posItem.Quantity_28);
                                                bondLessThan1.SummaryTo.Add(summaryTo);
                                                bondLessThan1.SummaryBeginningYear.Add(summaryBeginningYear);
                                                bondLessThan1.SummaryPurchase.Add(summaryPurchase);
                                                sectionContent.SubSection8000.Content.Add(bondLessThan1);
                                                posItem.AlreadyUsed = true;
                                            }
                                        }
                                    }
                                    break;                            
                                case PositionClassifications.BONDS_WITH_MATURITY_MINOR_EQUAL_5_YEARS:
                                    {
                                        IBondDetail bondLessThan5;
                                        ISummaryTo summaryTo;
                                        ISummaryBeginningYear summaryBeginningYear;
                                        ISummaryPurchase summaryPurchase;
                                        sectionContent.SubSection8010 = new BondSubSection("Bonds with maturity <= 5 year");
                                        foreach (POS posItem in subCategoryItems.OrderBy(ob => ob.NumSecurity_29))
                                        {
                                            extEventArgsOperation = new ExternalCodifyRequestEventArgs(nameof(Section080), nameof(BondDetail.Coupon), AssignRequiredString(posItem.CouponFrequency_34), propertyParams);
                                            OnExternalCodifyRequest(extEventArgsOperation);
                                            if (!extEventArgsOperation.Cancel) {
                                                bondLessThan5 = new BondDetail() {
                                                    Currency = AssignRequiredString(posItem.Currency1_17),
                                                    NominalAmount = AssignRequiredDecimal(posItem.Quantity_28),
                                                    Description1 = BuildComposedDescription([AssignRequiredString(posItem.Description1_32), AssignRequiredString(posItem.Description2_33)]),
                                                    Description2 = BuildComposedDescription([AssignRequiredDate(posItem.IssueDate_46, _culture), AssignRequiredDate(posItem.MaturityDate_36, _culture)]),
                                                    Description3 = BuildComposedDescription([AssignRequiredLong(posItem.NumSecurity_29).ToString(), AssignRequiredString(posItem.IsinIban_85)]),
                                                    Coupon = GetCoupon(extEventArgsOperation.PropertyValue, AssignRequiredString(posItem.CouponText_35)),
                                                    PercentRate = AssignRequiredDecimal(posItem.InterestRate_47),
                                                    CapitalMarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23),
                                                    InterestMarketValueReportingCurrency = AssignRequiredDecimal(posItem.ProRataBase_56),
                                                    TotalMarketValueReportingCurrency = AssignRequiredDecimals(posItem.Amount1Base_23, posItem.ProRataBase_56),
                                                    Duration = decimal.TryParse(posItem.Duration_68, out decimal duration) ? duration : 0m,
                                                    SpRating = (string.IsNullOrEmpty(posItem.AgeRat_97) == false && posItem.AgeRat_97.Trim() == "SP") ? posItem.Rating_98 : "N/A",
                                                    PercentWeight = CalculatePercentWeight(totalAssets, posItem.Amount1Base_23),
                                                    PercentYTD = AssignRequiredDecimal(posItem.DirectRoi_64)
                                                };
                                                summaryTo = new SummaryTo() {
                                                    ValuePrice = AssignRequiredDecimal(posItem.Quote_48),
                                                    ExchangeValue = (curItems != null && curItems.Any(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == bondLessThan5.Currency && flt.Rate_6 != null)) ? curItems.First(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == bondLessThan5.Currency && flt.Rate_6.HasValue).Rate_6.Value : 0,
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
                                                bondLessThan5.TotalMarketValueReportingCurrency = bondLessThan5.CapitalMarketValueReportingCurrency + bondLessThan5.InterestMarketValueReportingCurrency;
                                                CalculateSummaries(ideItem.Currency_19, summaryTo, summaryBeginningYear, summaryPurchase, posItem.QuoteType_51, curItems, posItem.Currency1_17, posItem.Quantity_28);
                                                bondLessThan5.SummaryTo.Add(summaryTo);
                                                bondLessThan5.SummaryBeginningYear.Add(summaryBeginningYear);
                                                bondLessThan5.SummaryPurchase.Add(summaryPurchase);
                                                sectionContent.SubSection8010.Content.Add(bondLessThan5);
                                                posItem.AlreadyUsed = true;
                                            }
                                        }
                                    }
                                    break;
                                case PositionClassifications.BONDS_WITH_MATURITY_MAJOR_THAN_5_YEARS_OR_FUNDS:
                                    {
                                        IBondDetail bondMajorThan5;
                                        IBondFundDetail bondFunds;
                                        ISummaryTo summaryTo;
                                        ISummaryBeginningYear summaryBeginningYear;
                                        ISummaryPurchase summaryPurchase;                                        
                                        foreach (POS posItem in subCategoryItems.OrderBy(ob => ob.NumSecurity_29))
                                        {
                                            if (string.IsNullOrEmpty(posItem.Category_11) == false && posItem.Category_11.Trim().EndsWith("FO")) { // bond funds
                                                if (sectionContent.SubSection8040 == null)
                                                    sectionContent.SubSection8040 = new FundSubSection("Bond funds");
                                                bondFunds = new BondFundDetail() {
                                                    Currency = AssignRequiredString(posItem.Currency1_17),
                                                    Quantity = AssignRequiredDecimal(posItem.Quantity_28),
                                                    Description1 = AssignRequiredString(posItem.Description2_33),
                                                    Description2 = AssignRequiredString(posItem.Description1_32),
                                                    Description3 = BuildComposedDescription([AssignRequiredLong(posItem.NumSecurity_29).ToString(), AssignRequiredString(posItem.IsinIban_85)]),                                                    
                                                    PercentWeight = CalculatePercentWeight(totalAssets, posItem.Amount1Base_23),
                                                    CapitalMarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23),
                                                    TotalMarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23)
                                                };
                                                summaryTo = new SummaryTo() {
                                                    ValuePrice = AssignRequiredDecimal(posItem.Quote_48),
                                                    ExchangeValue = (curItems != null && curItems.Any(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == bondFunds.Currency && flt.Rate_6 != null)) ? curItems.First(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == bondFunds.Currency && flt.Rate_6.HasValue).Rate_6.Value : 0,
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
                                                CalculateSummaries(ideItem.Currency_19, summaryTo, summaryBeginningYear, summaryPurchase, posItem.QuoteType_51, curItems, posItem.Currency1_17, posItem.Quantity_28);
                                                bondFunds.SummaryTo.Add(summaryTo);
                                                bondFunds.SummaryBeginningYear.Add(summaryBeginningYear);
                                                bondFunds.SummaryPurchase.Add(summaryPurchase);
                                                sectionContent.SubSection8040.Content.Add(bondFunds);

                                            } else {
                                                if (sectionContent.SubSection8020 == null)
                                                    sectionContent.SubSection8020 = new BondSubSection("Bonds with maturity > 5 year");
                                                extEventArgsOperation = new ExternalCodifyRequestEventArgs(nameof(Section080), nameof(BondDetail.Coupon), AssignRequiredString(posItem.CouponFrequency_34), propertyParams);
                                                OnExternalCodifyRequest(extEventArgsOperation);
                                                if (!extEventArgsOperation.Cancel) {
                                                    bondMajorThan5 = new BondDetail() {
                                                        Currency = AssignRequiredString(posItem.Currency1_17),
                                                        NominalAmount = AssignRequiredDecimal(posItem.Quantity_28),
                                                        Description1 = BuildComposedDescription([AssignRequiredString(posItem.Description1_32), AssignRequiredString(posItem.Description2_33)]),
                                                        Description2 = BuildComposedDescription([AssignRequiredDate(posItem.IssueDate_46, _culture), AssignRequiredDate(posItem.MaturityDate_36, _culture)]),
                                                        Description3 = BuildComposedDescription([AssignRequiredLong(posItem.NumSecurity_29).ToString(), AssignRequiredString(posItem.IsinIban_85)]),
                                                        Coupon = GetCoupon(extEventArgsOperation.PropertyValue, AssignRequiredString(posItem.CouponText_35)),
                                                        PercentRate = AssignRequiredDecimal(posItem.InterestRate_47),
                                                        CapitalMarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23),
                                                        InterestMarketValueReportingCurrency = AssignRequiredDecimal(posItem.ProRataBase_56),
                                                        TotalMarketValueReportingCurrency = AssignRequiredDecimals(posItem.Amount1Base_23, posItem.ProRataBase_56),
                                                        Duration = decimal.TryParse(posItem.Duration_68, out decimal duration) ? duration : 0m,
                                                        SpRating = (string.IsNullOrEmpty(posItem.AgeRat_97) == false && posItem.AgeRat_97.Trim() == "SP") ? posItem.Rating_98 : "N/A",
                                                        PercentWeight = CalculatePercentWeight(totalAssets, posItem.Amount1Base_23),
                                                        PercentYTD = AssignRequiredDecimal(posItem.DirectRoi_64)
                                                    };
                                                    summaryTo = new SummaryTo() {
                                                        ValuePrice = AssignRequiredDecimal(posItem.Quote_48),
                                                        ExchangeValue = (curItems != null && curItems.Any(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == bondMajorThan5.Currency && flt.Rate_6 != null)) ? curItems.First(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == bondMajorThan5.Currency && flt.Rate_6.HasValue).Rate_6.Value : 0,
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
                                                    bondMajorThan5.TotalMarketValueReportingCurrency = bondMajorThan5.CapitalMarketValueReportingCurrency + bondMajorThan5.InterestMarketValueReportingCurrency;
                                                    CalculateSummaries(ideItem.Currency_19, summaryTo, summaryBeginningYear, summaryPurchase, posItem.QuoteType_51, curItems, posItem.Currency1_17, posItem.Quantity_28);
                                                    bondMajorThan5.SummaryTo.Add(summaryTo);
                                                    bondMajorThan5.SummaryBeginningYear.Add(summaryBeginningYear);
                                                    bondMajorThan5.SummaryPurchase.Add(summaryPurchase);
                                                    sectionContent.SubSection8020.Content.Add(bondMajorThan5);
                                                }
                                            }
                                            posItem.AlreadyUsed = true;
                                        }
                                    }
                                    break;
                                case PositionClassifications.CONVERTIBLE_BONDS_AND_BONDS_WITH_WARRANTS:
                                    {
                                        IBondDetail bondConvAndWarrants;
                                        ISummaryTo summaryTo;
                                        ISummaryBeginningYear summaryBeginningYear;
                                        ISummaryPurchase summaryPurchase;
                                        sectionContent.SubSection8030 = new BondSubSection("Convertible bonds, bonds with warrants");
                                        foreach (POS posItem in subCategoryItems.OrderBy(ob => ob.NumSecurity_29))
                                        {
                                            extEventArgsOperation = new ExternalCodifyRequestEventArgs(nameof(Section080), nameof(BondDetail.Coupon), AssignRequiredString(posItem.CouponFrequency_34), propertyParams);
                                            OnExternalCodifyRequest(extEventArgsOperation);
                                            if (!extEventArgsOperation.Cancel) {
                                                bondConvAndWarrants = new BondDetail() {
                                                    Currency = AssignRequiredString(posItem.Currency1_17),
                                                    NominalAmount = AssignRequiredDecimal(posItem.Quantity_28),
                                                    Description1 = BuildComposedDescription([AssignRequiredString(posItem.Description1_32), AssignRequiredString(posItem.Description2_33)]),
                                                    Description2 = BuildComposedDescription([AssignRequiredDate(posItem.IssueDate_46, _culture), AssignRequiredDate(posItem.MaturityDate_36, _culture)]),
                                                    Description3 = BuildComposedDescription([AssignRequiredLong(posItem.NumSecurity_29).ToString(), AssignRequiredString(posItem.IsinIban_85)]),
                                                    Coupon = GetCoupon(extEventArgsOperation.PropertyValue, AssignRequiredString(posItem.CouponText_35)),
                                                    PercentRate = AssignRequiredDecimal(posItem.InterestRate_47),
                                                    CapitalMarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23),
                                                    InterestMarketValueReportingCurrency = AssignRequiredDecimal(posItem.ProRataBase_56),
                                                    TotalMarketValueReportingCurrency = AssignRequiredDecimals(posItem.Amount1Base_23, posItem.ProRataBase_56),
                                                    Duration = decimal.TryParse(posItem.Duration_68, out decimal duration) ? duration : 0m,
                                                    SpRating = (string.IsNullOrEmpty(posItem.AgeRat_97) == false && posItem.AgeRat_97.Trim() == "SP") ? posItem.Rating_98 : "N/A",
                                                    PercentWeight = CalculatePercentWeight(totalAssets, posItem.Amount1Base_23),
                                                    PercentYTD = AssignRequiredDecimal(posItem.DirectRoi_64)
                                                };
                                                summaryTo = new SummaryTo() {
                                                    ValuePrice = AssignRequiredDecimal(posItem.Quote_48),
                                                    ExchangeValue = (curItems != null && curItems.Any(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == bondConvAndWarrants.Currency && flt.Rate_6 != null)) ? curItems.First(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == bondConvAndWarrants.Currency && flt.Rate_6.HasValue).Rate_6.Value : 0,
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
                                                bondConvAndWarrants.TotalMarketValueReportingCurrency = bondConvAndWarrants.CapitalMarketValueReportingCurrency + bondConvAndWarrants.InterestMarketValueReportingCurrency;
                                                CalculateSummaries(ideItem.Currency_19, summaryTo, summaryBeginningYear, summaryPurchase, posItem.QuoteType_51, curItems, posItem.Currency1_17, posItem.Quantity_28);
                                                bondConvAndWarrants.SummaryTo.Add(summaryTo);
                                                bondConvAndWarrants.SummaryBeginningYear.Add(summaryBeginningYear);
                                                bondConvAndWarrants.SummaryPurchase.Add(summaryPurchase);
                                                sectionContent.SubSection8030.Content.Add(bondConvAndWarrants);
                                                posItem.AlreadyUsed = true;
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    output.Content = new Section080Content(sectionContent);
                }
            }
            return output;
        }

    }

}
