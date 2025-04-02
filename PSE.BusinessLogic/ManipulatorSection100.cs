using System.Globalization;
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

    public class ManipulatorSection100 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection100(CultureInfo? culture = null) : base(new List<PositionClassifications>()
            {
                PositionClassifications.METALS_ACCOUNTS_FUNDS_PHYSICAL,
                PositionClassifications.FORWARD_METAL_OPERATIONS_PROFIT_LOSS,
                PositionClassifications.DERIVATIVE_PRODUCTS_ON_METALS
            }, ManipolationTypes.AsSection100, culture){ }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData, decimal? totalAssets = null)
        {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section100 output = new()
            {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) && extractedData.Any(flt => flt.RecordType == nameof(POS)))
            {
                decimal customerSumAmounts, currentBaseValue;
                IMetalAccount metAcc;
                IDerivateMetalDetail derivateMetalDetail;
                IForwardMetalOperation forwardMetalOperation;
                IMetalDetail metalDetail;
                ISection100Content sectionContent;
                ISummaryTo summaryTo;
                ISummaryBeginningYear summaryBeginningYear;
                ISummaryPurchase summaryPurchase;
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<CUR> curItems = extractedData.Where(flt => flt.RecordType == nameof(CUR)).OfType<CUR>();
                foreach (IDE ideItem in ideItems)
                {
                    sectionContent = new Section100Content();
                    customerSumAmounts = extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>().Where(subFlt => subFlt.CustomerNumber_2 == ideItem.CustomerNumber_2 && subFlt.Amount1Base_23.HasValue).Sum(sum => sum.Amount1Base_23.Value);
                    IEnumerable<IGrouping<string, POS>> posItemsGroupBySubCat = extractedData.Where(flt => flt.AlreadyUsed == false && flt.RecordType == nameof(POS)).OfType<POS>().Where(fltSubCat => fltSubCat.CustomerNumber_2 == ideItem.CustomerNumber_2 && ManipulatorOperatingRules.IsRowDestinatedToManipulator(this, fltSubCat.SubCat4_15)).GroupBy(gb => gb.SubCat4_15).OrderBy(ob => ob.Key);
                    if (posItemsGroupBySubCat != null && posItemsGroupBySubCat.Any()) {
                        foreach (IGrouping<string, POS> subCategoryItems in posItemsGroupBySubCat) {
                            switch ((PositionClassifications)int.Parse(subCategoryItems.Key)) {
                                case PositionClassifications.METALS_ACCOUNTS_FUNDS_PHYSICAL: {
                                    foreach (POS posItem in subCategoryItems) {
                                        if (string.IsNullOrEmpty(posItem.Category_11) == false && posItem.Category_11.Trim().EndsWith("MF")) { // Metal funds
                                            if (sectionContent.SubSection10010 == null)
                                                sectionContent.SubSection10010 = new MetalSubSection("Metals funds");
                                            metalDetail = new MetalDetail() {
                                                CapitalMarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23),
                                                PercentWeight = CalculatePercentWeight(totalAssets, posItem.Amount1Base_23),
                                                Quantity = AssignRequiredDecimal(posItem.Quantity_28),
                                                Description1 = AssignRequiredString(posItem.Description2_33),
                                                Description2 = AssignRequiredString(posItem.Description1_32),
                                                Currency = AssignRequiredString(posItem.Currency1_17),
                                                TotalMarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23)
                                            };
                                            summaryTo = new SummaryTo() {
                                                ValuePrice = posItem.Quote_48,
                                                ExchangeValue = (curItems != null && curItems.Any(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == metalDetail.Currency && flt.Rate_6 != null)) ? curItems.First(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == metalDetail.Currency && flt.Rate_6.HasValue).Rate_6.Value : 0,
                                                PercentPrice = 0m,
                                                ProfitLossNotRealizedValue = 0m
                                            };
                                            summaryBeginningYear = new SummaryBeginningYear() {
                                                ValuePrice = posItem.BuyPriceAverage_87,
                                                ExchangeValue = posItem.BuyExchangeRateAverage_88
                                            };
                                            summaryPurchase = new SummaryPurchase() {
                                                ValuePrice = posItem.BuyPriceHistoric_53,
                                                ExchangeValue = posItem.BuyExchangeRateHistoric_66
                                            };
                                            CalculateSharesSummaries(summaryTo, summaryBeginningYear, summaryPurchase, posItem.Quantity_28);
                                            metalDetail.SummaryTo.Add(summaryTo);
                                            metalDetail.SummaryBeginningYear.Add(summaryBeginningYear);
                                            metalDetail.SummaryPurchase.Add(summaryPurchase);
                                            sectionContent.SubSection10010.Content.Add(metalDetail);
                                        } else if (string.IsNullOrEmpty(posItem.Category_11) == false && posItem.Category_11.Trim().EndsWith("ME")) { // Physical metals
                                            if (sectionContent.SubSection10020 == null)
                                                sectionContent.SubSection10020 = new MetalSubSection("Physical metals");
                                            metalDetail = new MetalDetail() {
                                                CapitalMarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23),
                                                PercentWeight = CalculatePercentWeight(totalAssets, posItem.Amount1Base_23),
                                                Quantity = AssignRequiredDecimal(posItem.Quantity_28),
                                                Description1 = AssignRequiredString(posItem.Description2_33),
                                                Description2 = AssignRequiredString(posItem.Description1_32),
                                                Currency = AssignRequiredString(posItem.Currency1_17),
                                                TotalMarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23)
                                            };
                                            summaryTo = new SummaryTo() {
                                                ValuePrice = posItem.Quote_48,
                                                ExchangeValue = (curItems != null && curItems.Any(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == metalDetail.Currency && flt.Rate_6 != null)) ? curItems.First(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == metalDetail.Currency && flt.Rate_6.HasValue).Rate_6.Value : 0,
                                                PercentPrice = 0m,
                                                ProfitLossNotRealizedValue = 0m
                                            };
                                            summaryBeginningYear = new SummaryBeginningYear() {
                                                ValuePrice = posItem.BuyPriceAverage_87,
                                                ExchangeValue = posItem.BuyExchangeRateAverage_88
                                            };
                                            summaryPurchase = new SummaryPurchase() {
                                                ValuePrice = posItem.BuyPriceHistoric_53,
                                                ExchangeValue = posItem.BuyExchangeRateHistoric_66
                                            };
                                            CalculateSharesSummaries(summaryTo, summaryBeginningYear, summaryPurchase, posItem.Quantity_28);
                                            metalDetail.SummaryTo.Add(summaryTo);
                                            metalDetail.SummaryBeginningYear.Add(summaryBeginningYear);
                                            metalDetail.SummaryPurchase.Add(summaryPurchase);
                                            sectionContent.SubSection10020.Content.Add(metalDetail);
                                        } else {
                                            if (sectionContent.SubSection10000 == null)
                                                sectionContent.SubSection10000 = new MetalAccountSubSection("Metals accounts");
                                            currentBaseValue = posItem.Amount1Base_23.HasValue ? posItem.Amount1Base_23.Value : 0;
                                            currentBaseValue += posItem.ProRataBase_56.HasValue ? posItem.ProRataBase_56.Value : 0;
                                            metAcc = new MetalAccount() {
                                                Account = AssignRequiredString(posItem.HostPositionReference_6),
                                                CurrentBalance = AssignRequiredDecimal(posItem.Quantity_28),
                                                MarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23),
                                                PercentWeight = customerSumAmounts != 0 && currentBaseValue != 0 ? Math.Round(currentBaseValue / customerSumAmounts * 100m, Model.Common.Constants.DEFAULT_MEANINGFUL_DECIMAL_DIGITS_FOR_CALCULATION) : 0
                                            };
                                            sectionContent.SubSection10000.Content.Add(metAcc);
                                        }
                                        posItem.AlreadyUsed = true;
                                    }
                                }
                                break;
                                case PositionClassifications.FORWARD_METAL_OPERATIONS_PROFIT_LOSS: {
                                    decimal amount1, amount2;
                                    sectionContent.SubSection10040 = new ForwardMetalOperationSubSection("Forward metal operations (profit/loss)");
                                    foreach (POS posItem in subCategoryItems) {
                                        if (!decimal.TryParse(posItem.Amount1Request_90, out amount1)) amount1 = 0m;
                                        if (!decimal.TryParse(posItem.Amount2Request_91, out amount2)) amount2 = 0m;
                                        forwardMetalOperation = new ForwardMetalOperation() {
                                            Currency1 = AssignRequiredString(posItem.Currency1_17),
                                            Currency2 = AssignRequiredString(posItem.Currency2_18),
                                            CurrencyValue = AssignRequiredDecimal(posItem.Amount1Cur1_22),
                                            ExpirationDate = AssignRequiredDate(posItem.MaturityDate_36, _culture),
                                            CurrentRate = AssignRequiredDecimal(posItem.Quote_48),
                                            Exchange = AssignRequiredDecimal(posItem.BuyPriceHistoric_53),
                                            ExchangeValue = AssignRequiredDecimal(posItem.Amount2Cur2_59),
                                            ProfitLoss = amount1 + amount2,
                                            PercentWeight = CalculatePercentWeight(totalAssets, amount1 + amount2)
                                        };
                                        sectionContent.SubSection10040.Content.Add(forwardMetalOperation);
                                        posItem.AlreadyUsed = true;
                                    }
                                }
                                break;
                                case PositionClassifications.DERIVATIVE_PRODUCTS_ON_METALS: {
                                    string tmpCurrency;
                                    sectionContent.SubSection10030 = new DerivateMetalSubSection("Derivative Products on Metals");
                                    foreach (POS posItem in subCategoryItems) {                                            
                                        derivateMetalDetail = new DerivateMetalDetail() {
                                            MarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23),
                                            PercentWeight = CalculatePercentWeight(totalAssets, posItem.Amount1Base_23),                                            
                                            Amount = AssignRequiredDecimal(posItem.Quantity_28),
                                            Description1 = AssignRequiredLong(posItem.NumSecurity_29).ToString(),
                                            Description2 = AssignRequiredString(posItem.Description1_32),                                                
                                            Description3 = AssignRequiredDate(posItem.CallaDate_38, _culture),
                                            Strike = AssignRequiredDecimal(posItem.InterestRate_47)
                                        };
                                        tmpCurrency = AssignRequiredString(posItem.Currency1_17);
                                        summaryTo = new SummaryTo() {
                                            ValuePrice = posItem.Quote_48,
                                            ExchangeValue = (curItems != null && curItems.Any(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == tmpCurrency && flt.Rate_6 != null)) ? curItems.First(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == tmpCurrency && flt.Rate_6.HasValue).Rate_6.Value : 0,
                                            PercentPrice = 0m,
                                            ProfitLossNotRealizedValue = 0m
                                        };
                                        summaryBeginningYear = new SummaryBeginningYear() {
                                            ValuePrice = posItem.BuyPriceAverage_87,
                                            ExchangeValue = posItem.BuyExchangeRateAverage_88
                                        };
                                        summaryPurchase = new SummaryPurchase() {
                                            ValuePrice = posItem.BuyPriceHistoric_53,
                                            ExchangeValue = posItem.BuyExchangeRateHistoric_66
                                        };
                                        CalculateSharesSummaries(summaryTo, summaryBeginningYear, summaryPurchase, posItem.Quantity_28);
                                        derivateMetalDetail.SummaryTo.Add(summaryTo);
                                        derivateMetalDetail.SummaryBeginningYear.Add(summaryBeginningYear);
                                        derivateMetalDetail.SummaryPurchase.Add(summaryPurchase);
                                        sectionContent.SubSection10030.Content.Add(derivateMetalDetail);
                                        posItem.AlreadyUsed = true;
                                    }
                                }
                                break;
                            }
                        }
                    }
                    output.Content = new Section100Content(sectionContent);
                }
            }
            return output;
        }

    }

}
