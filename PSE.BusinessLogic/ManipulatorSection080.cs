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

    public class ManipulatorSection080 : ManipulatorBase, IManipulator
    {
        
        private readonly BondsCalculation _calcBonds;

        public ManipulatorSection080(CalculationSettings calcSettings, CultureInfo? culture = null) : base(new List<PositionClassifications>() { PositionClassifications.BONDS_WITH_MATURITY_MINOR_EQUAL_1_YEAR, PositionClassifications.BONDS_WITH_MATURITY_MINOR_EQUAL_5_YEARS, PositionClassifications.BONDS_WITH_MATURITY_MAJOR_THAN_5_YEARS, PositionClassifications.CONVERTIBLE_BONDS_AND_BONDS_WITH_WARRANTS }, ManipolationTypes.AsSection080, culture) 
        {            
            _calcBonds = new BondsCalculation(calcSettings);
        }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
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
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                foreach (IDE ideItem in ideItems)
                {
                    sectionContent = new Section080Content();
                    //IEnumerable<CUR> curItems = extractedData.Where(flt => flt.RecordType == nameof(CUR)).OfType<CUR>(); // ??
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
                                        sectionContent.Subsection8000 = new BondSubSection("Bonds with maturity <= 1 year");
                                        foreach (POS posItem in subCategoryItems)
                                        {
                                            summaryTo = new SummaryTo()
                                            {
                                                ValuePrice = AssignRequiredDecimal(posItem.Quote_48),
                                                PercentPrice = 0m,
                                                ProfitLossNotRealizedValue = 0m
                                            };
                                            summaryBeginningYear = new SummaryBeginningYear()
                                            {
                                                ValuePrice = AssignRequiredDecimal(posItem.BuyPriceAverage_87),
                                                ExchangeValue = AssignRequiredDecimal(posItem.BuyExchangeRateAverage_88)
                                            };
                                            summaryPurchase = new SummaryPurchase()
                                            {
                                                ValuePrice = AssignRequiredDecimal(posItem.BuyPriceHistoric_53),
                                                ExchangeValue = AssignRequiredDecimal(posItem.BuyExchangeRateHistoric_66)
                                            };
                                            bondLessThan1 = new BondDetail()
                                            {
                                                Currency = AssignRequiredString(posItem.Currency1_17),
                                                NominalAmount = AssignRequiredDecimal(posItem.Quantity_28),
                                                Description1 = string.Concat(AssignRequiredString(posItem.Description1_32), " ", AssignRequiredString(posItem.Description2_33)),
                                                Description2 = string.Concat(AssignRequiredLong(posItem.NumSecurity_29).ToString(), " ", AssignRequiredDate(posItem.MaturityDate_36, _culture), " ", AssignRequiredDate(posItem.IssueDate_46, _culture)),
                                                Description3 = AssignRequiredString(posItem.IsinIban_85),
                                                Coupon = AssignRequiredString(posItem.CouponText_35),                                                 
                                                PercentRate = AssignRequiredDecimal(posItem.InterestRate_47),
                                                CapitalMarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23),
                                                InterestMarketValueReportingCurrency = AssignRequiredDecimal(posItem.ProRataBase_56),
                                                Duration = decimal.TryParse(posItem.Duration_68, out decimal duration) ? duration : 0m,
                                                SpRating = (string.IsNullOrEmpty(posItem.AgeRat_97) == false && posItem.AgeRat_97.Trim() == "SP") ? posItem.Rating_98 : string.Empty,                                                
                                                PercentWeight = 0, // ??
                                                PercentYTD = 0,    // ??
                                            };
                                            bondLessThan1.TotalMarketValueReportingCurrency = bondLessThan1.CapitalMarketValueReportingCurrency + bondLessThan1.InterestMarketValueReportingCurrency;
                                            CalculateSummaries(summaryTo, summaryBeginningYear, summaryPurchase, posItem.Quantity_28);
                                            bondLessThan1.SummaryTo.Add(summaryTo);
                                            bondLessThan1.SummaryBeginningYear.Add(summaryBeginningYear);
                                            bondLessThan1.SummaryPurchase.Add(summaryPurchase);
                                            sectionContent.Subsection8000.Content.Add(bondLessThan1);
                                            posItem.AlreadyUsed = true;
                                        }
                                    }
                                    break;                            
                                case PositionClassifications.BONDS_WITH_MATURITY_MINOR_EQUAL_5_YEARS:
                                    {
                                        IBondDetail bondLessThan5;
                                        ISummaryTo summaryTo;
                                        ISummaryBeginningYear summaryBeginningYear;
                                        ISummaryPurchase summaryPurchase;
                                        sectionContent.Subsection8010 = new BondSubSection("Bonds with maturity <= 5 year");
                                        foreach (POS posItem in subCategoryItems)
                                        {
                                            summaryTo = new SummaryTo()
                                            {
                                                ValuePrice = AssignRequiredDecimal(posItem.Quote_48),
                                                PercentPrice = 0m,
                                                ProfitLossNotRealizedValue = 0m
                                            };
                                            summaryBeginningYear = new SummaryBeginningYear()
                                            {
                                                ValuePrice = AssignRequiredDecimal(posItem.BuyPriceAverage_87),
                                                ExchangeValue = AssignRequiredDecimal(posItem.BuyExchangeRateAverage_88)
                                            };
                                            summaryPurchase = new SummaryPurchase()
                                            {
                                                ValuePrice = AssignRequiredDecimal(posItem.BuyPriceHistoric_53),
                                                ExchangeValue = AssignRequiredDecimal(posItem.BuyExchangeRateHistoric_66)
                                            };
                                            bondLessThan5 = new BondDetail()
                                            {
                                                Currency = AssignRequiredString(posItem.Currency1_17),
                                                NominalAmount = AssignRequiredDecimal(posItem.Quantity_28),
                                                Description1 = string.Concat(AssignRequiredString(posItem.Description1_32), " ", AssignRequiredString(posItem.Description2_33)),
                                                Description2 = string.Concat(AssignRequiredLong(posItem.NumSecurity_29).ToString(), " ", AssignRequiredDate(posItem.MaturityDate_36, _culture), " ", AssignRequiredDate(posItem.IssueDate_46, _culture)),
                                                Description3 = AssignRequiredString(posItem.IsinIban_85),
                                                Coupon = AssignRequiredString(posItem.CouponText_35),
                                                PercentRate = AssignRequiredDecimal(posItem.InterestRate_47),
                                                CapitalMarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23),
                                                InterestMarketValueReportingCurrency = AssignRequiredDecimal(posItem.ProRataBase_56),
                                                Duration = decimal.TryParse(posItem.Duration_68, out decimal duration) ? duration : 0m,
                                                SpRating = (string.IsNullOrEmpty(posItem.AgeRat_97) == false && posItem.AgeRat_97.Trim() == "SP") ? posItem.Rating_98 : string.Empty,
                                                PercentWeight = 0, // ??
                                                PercentYTD = 0,    // ??
                                            };
                                            bondLessThan5.TotalMarketValueReportingCurrency = bondLessThan5.CapitalMarketValueReportingCurrency + bondLessThan5.InterestMarketValueReportingCurrency;
                                            CalculateSummaries(summaryTo, summaryBeginningYear, summaryPurchase, posItem.Quantity_28);
                                            bondLessThan5.SummaryTo.Add(summaryTo);
                                            bondLessThan5.SummaryBeginningYear.Add(summaryBeginningYear);
                                            bondLessThan5.SummaryPurchase.Add(summaryPurchase);
                                            sectionContent.Subsection8010.Content.Add(bondLessThan5);
                                            posItem.AlreadyUsed = true;
                                        }
                                    }
                                    break;
                                case PositionClassifications.BONDS_WITH_MATURITY_MAJOR_THAN_5_YEARS:
                                    {
                                        IBondDetail bondMajorThan5;
                                        ISummaryTo summaryTo;
                                        ISummaryBeginningYear summaryBeginningYear;
                                        ISummaryPurchase summaryPurchase;
                                        sectionContent.Subsection8020 = new BondSubSection("Bonds with maturity > 5 year");
                                        foreach (POS posItem in subCategoryItems)
                                        {
                                            summaryTo = new SummaryTo()
                                            {
                                                ValuePrice = AssignRequiredDecimal(posItem.Quote_48),
                                                PercentPrice = 0m,
                                                ProfitLossNotRealizedValue = 0m
                                            };
                                            summaryBeginningYear = new SummaryBeginningYear()
                                            {
                                                ValuePrice = AssignRequiredDecimal(posItem.BuyPriceAverage_87),
                                                ExchangeValue = AssignRequiredDecimal(posItem.BuyExchangeRateAverage_88)
                                            };
                                            summaryPurchase = new SummaryPurchase()
                                            {
                                                ValuePrice = AssignRequiredDecimal(posItem.BuyPriceHistoric_53),
                                                ExchangeValue = AssignRequiredDecimal(posItem.BuyExchangeRateHistoric_66)
                                            };
                                            bondMajorThan5 = new BondDetail()
                                            {
                                                Currency = AssignRequiredString(posItem.Currency1_17),
                                                NominalAmount = AssignRequiredDecimal(posItem.Quantity_28),
                                                Description1 = string.Concat(AssignRequiredString(posItem.Description1_32), " ", AssignRequiredString(posItem.Description2_33)),
                                                Description2 = string.Concat(AssignRequiredLong(posItem.NumSecurity_29).ToString(), " ", AssignRequiredDate(posItem.MaturityDate_36, _culture), " ", AssignRequiredDate(posItem.IssueDate_46, _culture)),
                                                Description3 = AssignRequiredString(posItem.IsinIban_85),
                                                Coupon = AssignRequiredString(posItem.CouponText_35),
                                                PercentRate = AssignRequiredDecimal(posItem.InterestRate_47),
                                                CapitalMarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23),
                                                InterestMarketValueReportingCurrency = AssignRequiredDecimal(posItem.ProRataBase_56),
                                                Duration = decimal.TryParse(posItem.Duration_68, out decimal duration) ? duration : 0m,
                                                SpRating = (string.IsNullOrEmpty(posItem.AgeRat_97) == false && posItem.AgeRat_97.Trim() == "SP") ? posItem.Rating_98 : string.Empty,
                                                PercentWeight = 0, // ??
                                                PercentYTD = 0,    // ??
                                            };
                                            bondMajorThan5.TotalMarketValueReportingCurrency = bondMajorThan5.CapitalMarketValueReportingCurrency + bondMajorThan5.InterestMarketValueReportingCurrency;
                                            CalculateSummaries(summaryTo, summaryBeginningYear, summaryPurchase, posItem.Quantity_28);
                                            bondMajorThan5.SummaryTo.Add(summaryTo);
                                            bondMajorThan5.SummaryBeginningYear.Add(summaryBeginningYear);
                                            bondMajorThan5.SummaryPurchase.Add(summaryPurchase);
                                            sectionContent.Subsection8020.Content.Add(bondMajorThan5);
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
                                        sectionContent.Subsection8030 = new BondSubSection("Convertible bonds, bonds with warrants");
                                        foreach (POS posItem in subCategoryItems)
                                        {
                                            summaryTo = new SummaryTo()
                                            {
                                                ValuePrice = AssignRequiredDecimal(posItem.Quote_48),
                                                PercentPrice = 0m,
                                                ProfitLossNotRealizedValue = 0m
                                            };
                                            summaryBeginningYear = new SummaryBeginningYear()
                                            {
                                                ValuePrice = AssignRequiredDecimal(posItem.BuyPriceAverage_87),
                                                ExchangeValue = AssignRequiredDecimal(posItem.BuyExchangeRateAverage_88)
                                            };
                                            summaryPurchase = new SummaryPurchase()
                                            {
                                                ValuePrice = AssignRequiredDecimal(posItem.BuyPriceHistoric_53),
                                                ExchangeValue = AssignRequiredDecimal(posItem.BuyExchangeRateHistoric_66)
                                            };
                                            bondConvAndWarrants = new BondDetail()
                                            {
                                                Currency = AssignRequiredString(posItem.Currency1_17),
                                                NominalAmount = AssignRequiredDecimal(posItem.Quantity_28),
                                                Description1 = string.Concat(AssignRequiredString(posItem.Description1_32), " ", AssignRequiredString(posItem.Description2_33)),
                                                Description2 = string.Concat(AssignRequiredLong(posItem.NumSecurity_29).ToString(), " ", AssignRequiredDate(posItem.MaturityDate_36, _culture), " ", AssignRequiredDate(posItem.IssueDate_46, _culture)),
                                                Description3 = AssignRequiredString(posItem.IsinIban_85),
                                                Coupon = AssignRequiredString(posItem.CouponText_35),
                                                PercentRate = AssignRequiredDecimal(posItem.InterestRate_47),
                                                CapitalMarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23),
                                                InterestMarketValueReportingCurrency = AssignRequiredDecimal(posItem.ProRataBase_56),
                                                Duration = decimal.TryParse(posItem.Duration_68, out decimal duration) ? duration : 0m,
                                                SpRating = (string.IsNullOrEmpty(posItem.AgeRat_97) == false && posItem.AgeRat_97.Trim() == "SP") ? posItem.Rating_98 : string.Empty,
                                                PercentWeight = 0, // ??
                                                PercentYTD = 0,    // ??
                                            };
                                            bondConvAndWarrants.TotalMarketValueReportingCurrency = bondConvAndWarrants.CapitalMarketValueReportingCurrency + bondConvAndWarrants.InterestMarketValueReportingCurrency;
                                            CalculateSummaries(summaryTo, summaryBeginningYear, summaryPurchase, posItem.Quantity_28);
                                            bondConvAndWarrants.SummaryTo.Add(summaryTo);
                                            bondConvAndWarrants.SummaryBeginningYear.Add(summaryBeginningYear);
                                            bondConvAndWarrants.SummaryPurchase.Add(summaryPurchase);
                                            sectionContent.Subsection8030.Content.Add(bondConvAndWarrants);
                                            posItem.AlreadyUsed = true;
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
