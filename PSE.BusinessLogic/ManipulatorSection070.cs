using System.Globalization;
using System.Security.Principal;
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

    public class ManipulatorSection070 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection070(CultureInfo? culture = null) : base(new List<PositionClassifications>() { PositionClassifications.ACCOUNT , PositionClassifications.SHORT_TERM_FUND }, ManipolationTypes.AsSection070, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section070 output = new()
            {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) && extractedData.Any(flt => flt.RecordType == nameof(POS)))
            {
                decimal customerSumAmounts, currentBaseValue;                
                ISection070Content sectionContent;
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();                
                foreach (IDE ideItem in ideItems)
                {
                    sectionContent = new Section070Content();
                    IEnumerable<IGrouping<string, POS>> posItemsGroupBySubCat = extractedData.Where(flt => flt.AlreadyUsed == false && flt.RecordType == nameof(POS)).OfType<POS>().Where(fltSubCat => fltSubCat.CustomerNumber_2 == ideItem.CustomerNumber_2 && ManipulatorOperatingRules.IsRowDestinatedToManipulator(this, fltSubCat.SubCat4_15)).GroupBy(gb => gb.SubCat4_15).OrderBy(ob => ob.Key);
                    if (posItemsGroupBySubCat != null && posItemsGroupBySubCat.Any())
                    {
                        foreach (IGrouping<string, POS> subCategoryItems in posItemsGroupBySubCat)
                        {
                            if (subCategoryItems.Key == ((int)PositionClassifications.ACCOUNT).ToString())
                            {
                                ILiquidityAccount account;
                                customerSumAmounts = subCategoryItems.Where(subFlt => subFlt.Amount1Base_23.HasValue).Sum(sum => sum.Amount1Base_23.Value);
                                customerSumAmounts += subCategoryItems.Where(subFlt => subFlt.ProRataBase_56.HasValue).Sum(sum => sum.ProRataBase_56.Value);
                                sectionContent.SubSection7000 = new SubSection7000Content();
                                foreach (POS posItem in subCategoryItems)
                                {
                                    currentBaseValue = posItem.Amount1Base_23.HasValue ? posItem.Amount1Base_23.Value : 0;
                                    currentBaseValue += posItem.ProRataBase_56.HasValue ? posItem.ProRataBase_56.Value : 0;
                                    account = new LiquidityAccount()
                                    {
                                        Description = string.Concat(posItem.Description1_32, " ", posItem.Currency1_17, " ", posItem.HostPositionReference_6),
                                        MarketValueReportingCurrency = posItem.Amount1Base_23,
                                        CurrentBalance = posItem.Quantity_28 != null ? posItem.Quantity_28.Value : 0,
                                        Iban = posItem.IsinIban_85,
                                        PercentWeight = customerSumAmounts != 0 && currentBaseValue != 0 ? Math.Round(currentBaseValue / customerSumAmounts * 100m, Model.Common.Constants.DEFAULT_MEANINGFUL_DECIMAL_DIGITS_FOR_CALCULATION) : 0
                                    };
                                    sectionContent.SubSection7000.Content.Add(account);
                                    posItem.AlreadyUsed = true;
                                }
                            }
                            else if (subCategoryItems.Key == ((int)PositionClassifications.SHORT_TERM_FUND).ToString())
                            {
                                ILiquidityShortTermFund shortTermFund;
                                ISummaryTo summaryTo;
                                ISummaryBeginningYear summaryBeginningYear;
                                ISummaryPurchase summaryPurchase;
                                sectionContent.SubSection7010 = new SubSection7010Content();
                                foreach (POS posItem in subCategoryItems)
                                {
                                    summaryTo = new SummaryTo()
                                    {
                                        ValuePrice = posItem.Quote_48,
                                        ValueDate = posItem.QuoteDate_49,
                                        PercentPrice = 0m,
                                        ProfitLossNotRealizedValue = 0m
                                    };
                                    summaryBeginningYear = new SummaryBeginningYear()
                                    {
                                        ValuePrice = posItem.BuyPriceAverage_87,
                                        ExchangeValue = posItem.BuyExchangeRateAverage_88
                                    };
                                    summaryPurchase = new SummaryPurchase()
                                    {
                                        ValuePrice = posItem.BuyPriceHistoric_53,
                                        ExchangeValue = posItem.BuyExchangeRateHistoric_66
                                    };
                                    shortTermFund = new LiquidityShortTermFund()
                                    {
                                        Description1 = string.Concat(posItem.Description1_32, " ", posItem.Description2_33),
                                        Description2 = posItem.IsinIban_85,
                                        Quantity = posItem.Quantity_28,
                                        Currency = posItem.Currency1_17,
                                        CapitalMarketValueReportingCurrency = posItem.Amount1Base_23,
                                        //InterestMarketValueReportingCurrency = 0, 
                                        //TotalMarketValueReportingCurrency = 0,  
                                        //PercentWeight = 0,  
                                    };
                                    if (summaryTo.ValuePrice.HasValue)
                                    {
                                        if (summaryBeginningYear.ValuePrice.HasValue && summaryBeginningYear.ValuePrice != 0m)
                                            summaryBeginningYear.PercentPrice = Math.Round((summaryTo.ValuePrice.Value - summaryBeginningYear.ValuePrice.Value) / summaryBeginningYear.ValuePrice.Value, Model.Common.Constants.DEFAULT_MEANINGFUL_DECIMAL_DIGITS_FOR_CALCULATION);
                                        if (summaryPurchase.ValuePrice.HasValue && summaryPurchase.ValuePrice != 0m)
                                            summaryPurchase.PercentPrice = Math.Round((summaryTo.ValuePrice.Value - summaryPurchase.ValuePrice.Value) / summaryPurchase.ValuePrice.Value, Model.Common.Constants.DEFAULT_MEANINGFUL_DECIMAL_DIGITS_FOR_CALCULATION);
                                        if (posItem.Quantity_28.HasValue && posItem.Quantity_28.Value != 0m)
                                        {
                                            summaryBeginningYear.ProfitLossNotRealizedValue = Math.Round((summaryTo.ValuePrice.Value - summaryBeginningYear.ValuePrice.Value) * posItem.Quantity_28.Value, Model.Common.Constants.DEFAULT_MEANINGFUL_DECIMAL_DIGITS_FOR_CALCULATION);
                                            summaryPurchase.ProfitLossNotRealizedValue = Math.Round((summaryTo.ValuePrice.Value - summaryPurchase.ValuePrice.Value) * posItem.Quantity_28.Value, Model.Common.Constants.DEFAULT_MEANINGFUL_DECIMAL_DIGITS_FOR_CALCULATION);
                                        }
                                    }
                                    shortTermFund.SummaryTo.Add(summaryTo);
                                    shortTermFund.SummaryBeginningYear.Add(summaryBeginningYear);
                                    shortTermFund.SummaryPurchase.Add(summaryPurchase);
                                    sectionContent.SubSection7010.Content.Add(shortTermFund);
                                    posItem.AlreadyUsed = true;
                                }
                            }
                        }
                    }
                    output.Content = new Section070Content(sectionContent);
                }
            }
            return output;
        }

    }

}
