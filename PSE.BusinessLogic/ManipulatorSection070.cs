using System.Globalization;
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

    public class ManipulatorSection070 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection070(CultureInfo? culture = null) : base(new List<PositionClassifications>() { PositionClassifications.ACCOUNT , PositionClassifications.SHORT_TERM_FUND, PositionClassifications.FIDUCIARY_INVESTMENTS, PositionClassifications.TEMPORARY_DEPOSITS, PositionClassifications.FORWARD_EXCHANGE_TRANSACTIONS, PositionClassifications.CURRENCY_DERIVATIVE_PRODUCTS }, ManipolationTypes.AsSection070, culture) { }

        public override IOutputModel Manipulate(IPSEDictionaryService dictionaryService, IList<IInputRecord> extractedData, decimal? totalAssets = null)
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
                ISection070Content sectionContent;
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<CUR> curItems = extractedData.Where(flt => flt.RecordType == nameof(CUR)).OfType<CUR>();
                foreach (IDE ideItem in ideItems)
                {
                    sectionContent = new Section070Content();
                    IEnumerable<IGrouping<string, POS>> posItemsGroupBySubCat = extractedData.Where(flt => flt.AlreadyUsed == false && flt.RecordType == nameof(POS)).OfType<POS>().Where(fltSubCat => fltSubCat.CustomerNumber_2 == ideItem.CustomerNumber_2 && ManipulatorOperatingRules.IsRowDestinatedToManipulator(this, fltSubCat.SubCat4_15)).GroupBy(gb => gb.SubCat4_15).OrderBy(ob => ob.Key);
                    if (posItemsGroupBySubCat != null && posItemsGroupBySubCat.Any())
                    {
                        foreach (IGrouping<string, POS> subCategoryItems in posItemsGroupBySubCat)
                        {
                            switch((PositionClassifications)int.Parse(subCategoryItems.Key))
                            {
                                case PositionClassifications.ACCOUNT:
                                    {
                                        ILiquidityAccount account;
                                        decimal currentBaseValue;
                                        decimal customerSumAmounts = subCategoryItems.Where(subFlt => subFlt.Amount1Base_23.HasValue).Sum(sum => sum.Amount1Base_23.Value);
                                        customerSumAmounts += subCategoryItems.Where(subFlt => subFlt.ProRataBase_56.HasValue).Sum(sum => sum.ProRataBase_56.Value);
                                        sectionContent.SubSection7000 = new SubSection7000Content();
                                        foreach (POS posItem in subCategoryItems.OrderByDescending(ob => ob.Amount1Base_23))
                                        {
                                            currentBaseValue = posItem.Amount1Base_23.HasValue ? posItem.Amount1Base_23.Value : 0;
                                            currentBaseValue += posItem.ProRataBase_56.HasValue ? posItem.ProRataBase_56.Value : 0;
                                            account = new LiquidityAccount()
                                            {
                                                Description = string.Concat(AssignRequiredString(posItem.Description1_32), " ", AssignRequiredString(posItem.Currency1_17), " ", AssignRequiredString(posItem.HostPositionReference_6), " ", AssignRequiredString(posItem.Description2_33)).Trim(),
                                                MarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23),
                                                CurrentBalance = AssignRequiredDecimal(posItem.Quantity_28),
                                                Iban = AssignRequiredString(posItem.IsinIban_85),
                                                PercentWeight = CalculatePercentWeight(totalAssets, posItem.Amount1Base_23)
                                            };
                                            sectionContent.SubSection7000.Content.Add(account);
                                            posItem.AlreadyUsed = true;
                                        }
                                    }
                                    break;
                                case PositionClassifications.SHORT_TERM_FUND:
                                    {
                                        ILiquidityShortTermFund shortTermFund;
                                        ISummaryTo summaryTo;
                                        ISummaryBeginningYear summaryBeginningYear;
                                        ISummaryPurchase summaryPurchase;
                                        sectionContent.SubSection7010 = new SubSection7010Content();
                                        foreach (POS posItem in subCategoryItems.OrderByDescending(ob => ob.Amount1Base_23))
                                        {
                                            shortTermFund = new LiquidityShortTermFund()
                                            {
                                                Description1 = string.Concat(AssignRequiredString(posItem.Description1_32), " ", AssignRequiredString(posItem.Description2_33)),
                                                Description2 = AssignRequiredString(posItem.IsinIban_85),
                                                Quantity = AssignRequiredDecimal(posItem.Quantity_28),
                                                Currency = AssignRequiredString(posItem.Currency1_17),
                                                CapitalMarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23),
                                                InterestMarketValueReportingCurrency = AssignRequiredDecimal(posItem.ProRataBase_56),      
                                                PercentWeight = CalculatePercentWeight(totalAssets, posItem.Amount1Base_23),
                                                TotalMarketValueReportingCurrency = AssignRequiredDecimals(posItem.Amount1Base_23, posItem.ProRataBase_56)
                                            };
                                            summaryTo = new SummaryTo()
                                            {
                                                ValuePrice = AssignRequiredDecimal(posItem.Quote_48),
                                                ValueDate = AssignRequiredDate(posItem.QuoteDate_49, _culture),
                                                ExchangeValue = (curItems != null && curItems.Any(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == shortTermFund.Currency && flt.Rate_6 != null)) ? curItems.First(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == shortTermFund.Currency && flt.Rate_6.HasValue).Rate_6.Value : 0,
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
                                            shortTermFund.TotalMarketValueReportingCurrency = shortTermFund.CapitalMarketValueReportingCurrency + shortTermFund.InterestMarketValueReportingCurrency;
                                            CalculateSummaries(ideItem.Currency_19, summaryTo, summaryBeginningYear, summaryPurchase, posItem.QuoteType_51, curItems, posItem.Currency1_17, posItem.Quantity_28);
                                            shortTermFund.SummaryTo.Add(summaryTo);
                                            shortTermFund.SummaryBeginningYear.Add(summaryBeginningYear);
                                            shortTermFund.SummaryPurchase.Add(summaryPurchase);
                                            sectionContent.SubSection7010.Content.Add(shortTermFund);
                                            posItem.AlreadyUsed = true;
                                        }
                                    }
                                    break;
                                case PositionClassifications.FIDUCIARY_INVESTMENTS:
                                    {
                                        ILiquidityFiduciaryInvestmentTemporaryDeposit liquidityFiduciaryInvestmentTemporaryDeposit;
                                        sectionContent.SubSection7020 = new SubSection7020Content();
                                        foreach (POS posItem in subCategoryItems.OrderByDescending(ob => ob.Amount1Base_23))
                                        {
                                            liquidityFiduciaryInvestmentTemporaryDeposit = new LiquidityFiduciaryInvestmentTemporaryDeposit()
                                            {
                                                Description1 = string.Concat(AssignRequiredString(posItem.Currency1_17), " ", AssignRequiredString(posItem.HostPositionReference_6)),
                                                Description2 = string.Empty,
                                                MarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23),
                                                CurrentBalance = AssignRequiredDecimal(posItem.Quantity_28),
                                                Correspondent = AssignRequiredString(posItem.Description2_33),
                                                ExpirationDate = AssignRequiredDate(posItem.MaturityDate_36, _culture),
                                                OpeningDate = AssignRequiredDate(posItem.ConversionDateStart_41, _culture),
                                                AccruedInterestReportingCurrency = AssignRequiredDecimal(posItem.ProRataBase_56),
                                                PercentWeight = CalculatePercentWeight(totalAssets, posItem.Amount1Base_23)
                                            };
                                            sectionContent.SubSection7020.Content.Add(liquidityFiduciaryInvestmentTemporaryDeposit);
                                            posItem.AlreadyUsed = true;
                                        }
                                    }
                                    break;
                                case PositionClassifications.TEMPORARY_DEPOSITS:
                                    {
                                        ILiquidityFiduciaryInvestmentTemporaryDeposit liquidityFiduciaryInvestmentTemporaryDeposit;
                                        sectionContent.SubSection7030 = new SubSection7030Content();
                                        foreach (POS posItem in subCategoryItems.OrderByDescending(ob => ob.Amount1Base_23))
                                        {
                                            liquidityFiduciaryInvestmentTemporaryDeposit = new LiquidityFiduciaryInvestmentTemporaryDeposit()
                                            {
                                                Description1 = string.Concat(AssignRequiredString(posItem.Currency1_17), " ", AssignRequiredString(posItem.HostPositionReference_6)),
                                                Description2 = string.Empty,
                                                MarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23),
                                                CurrentBalance = AssignRequiredDecimal(posItem.Quantity_28),
                                                Correspondent = AssignRequiredString(posItem.Description2_33),
                                                ExpirationDate = AssignRequiredDate(posItem.MaturityDate_36, _culture),
                                                OpeningDate = AssignRequiredDate(posItem.ConversionDateStart_41, _culture),
                                                AccruedInterestReportingCurrency = AssignRequiredDecimal(posItem.ProRataBase_56),
                                                PercentWeight = CalculatePercentWeight(totalAssets, posItem.Amount1Base_23)
                                            };
                                            sectionContent.SubSection7030.Content.Add(liquidityFiduciaryInvestmentTemporaryDeposit);
                                            posItem.AlreadyUsed = true;
                                        }
                                    }
                                    break;
                                case PositionClassifications.FORWARD_EXCHANGE_TRANSACTIONS:
                                    {
                                        decimal amount1, amount2;
                                        ILiquidityForwardExchangeOperation liquidityForwardExchangeOperation;
                                        sectionContent.SubSection7040 = new SubSection7040Content();
                                        foreach (POS posItem in subCategoryItems.OrderByDescending(ob => ob.Amount1Base_23))
                                        {
                                            if (!decimal.TryParse(posItem.Amount1Request_90, out amount1)) amount1 = 0m;
                                            if (!decimal.TryParse(posItem.Amount2Request_91, out amount2)) amount2 = 0m;
                                            liquidityForwardExchangeOperation = new LiquidityForwardExchangeOperation()
                                            {
                                                Currency1 = AssignRequiredString(posItem.Currency1_17),
                                                Currency2 = AssignRequiredString(posItem.Currency2_18),
                                                CurrencyValue = AssignRequiredDecimal(posItem.Amount1Cur1_22),
                                                ExpirationDate = AssignRequiredDate(posItem.MaturityDate_36, _culture),
                                                CurrentRate = AssignRequiredDecimal(posItem.Quote_48),
                                                ExchangeRate = AssignRequiredDecimal(posItem.BuyPriceHistoric_53),
                                                ExchangeValue = AssignRequiredDecimal(posItem.Amount2Cur2_59),
                                                ProfitLoss = amount1 + amount2, 
                                                PercentWeight = CalculatePercentWeight(totalAssets, amount1 + amount2)  
                                            };
                                            sectionContent.SubSection7040.Content.Add(liquidityForwardExchangeOperation);
                                            posItem.AlreadyUsed = true;
                                        }
                                    }
                                    break;
                                case PositionClassifications.CURRENCY_DERIVATIVE_PRODUCTS:
                                    {
                                        ILiquidityCurrencyDerivativeProduct liquidityCurrencyDerivativeProduct;
                                        ISummaryTo summaryTo;
                                        ISummaryBeginningYear summaryBeginningYear;
                                        ISummaryPurchase summaryPurchase;
                                        string currency;
                                        sectionContent.SubSection7050 = new SubSection7050Content();
                                        foreach (POS posItem in subCategoryItems.OrderBy(ob => ob.NumSecurity_29))
                                        {
                                            currency = AssignRequiredString(posItem.Currency1_17);
                                            liquidityCurrencyDerivativeProduct = new LiquidityCurrencyDerivativeProduct()
                                            {
                                                Description1 = AssignRequiredLong(posItem.NumSecurity_29).ToString(),
                                                Description2 = AssignRequiredString(posItem.Description1_32),
                                                Description3 = AssignRequiredDate(posItem.CallDate_38, _culture),
                                                Amount = AssignRequiredDecimal(posItem.Quantity_28),                                                
                                                MarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23),
                                                Strike = AssignRequiredString(posItem.Strike_67),                                                
                                                PercentWeight = CalculatePercentWeight(totalAssets, posItem.Amount1Base_23)
                                            };
                                            summaryTo = new SummaryTo()
                                            {
                                                ValuePrice = AssignRequiredDecimal(posItem.Quote_48),
                                                ExchangeValue = (curItems != null && curItems.Any(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == currency && flt.Rate_6 != null)) ? curItems.First(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == currency && flt.Rate_6.HasValue).Rate_6.Value : 0,
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
                                            CalculateSummaries(ideItem.Currency_19, summaryTo, summaryBeginningYear, summaryPurchase, posItem.QuoteType_51, curItems, posItem.Currency1_17, posItem.Quantity_28);
                                            liquidityCurrencyDerivativeProduct.SummaryTo.Add(summaryTo);
                                            liquidityCurrencyDerivativeProduct.SummaryBeginningYear.Add(summaryBeginningYear);
                                            liquidityCurrencyDerivativeProduct.SummaryPurchase.Add(summaryPurchase);
                                            sectionContent.SubSection7050.Content.Add(liquidityCurrencyDerivativeProduct);
                                            posItem.AlreadyUsed = true;
                                        }
                                    }
                                    break;
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
