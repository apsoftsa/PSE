using System.Globalization;
using PSE.BusinessLogic.Calculations;
using PSE.BusinessLogic.Common;
using PSE.BusinessLogic.Interfaces;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using PSE.Model.Params;
using PSE.Model.SupportTables;
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection15 : ManipulatorBase, IManipulator
    {

        private readonly SharesCalculation _calcShares;

        public ManipulatorSection15(CalculationSettings calcSettings, CultureInfo? culture = null) : base(PositionClassifications.AZIONI_FONDI_AZIONARI, ManipolationTypes.AsSection15, culture) 
        {
            _calcShares = new SharesCalculation(calcSettings);
        }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section15 output = new()
            {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) && extractedData.Any(flt => flt.RecordType == nameof(POS)))
            {
                decimal currencyRate, customerSumAmounts, currentBaseValue, quoteType;
                IBondsWithMaturityGreatherThanFiveYears bondGreatThan5;
                ISection15Content sectionContent;
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> posItems = extractedData.Where(flt => flt.AlreadyUsed == false && flt.RecordType == nameof(POS)).OfType<POS>().Where(fltSubCat => ManipulatorOperatingRules.IsRowDestinatedToManipulator(this, fltSubCat.SubCat4_15));
                IEnumerable<CUR> curItems = extractedData.Where(flt => flt.RecordType == nameof(CUR)).OfType<CUR>();
                foreach (IDE ideItem in ideItems)
                {
                    customerSumAmounts = extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>().Where(subFlt => subFlt.CustomerNumber_2 == ideItem.CustomerNumber_2 && subFlt.Amount1Base_23.HasValue).Sum(sum => sum.Amount1Base_23.Value);
                    customerSumAmounts += extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>().Where(subFlt => subFlt.CustomerNumber_2 == ideItem.CustomerNumber_2 && subFlt.ProRataBase_56.HasValue).Sum(sum => sum.ProRataBase_56.Value);
                    if (posItems != null && posItems.Any(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2))
                    {
                        sectionContent = new Section15Content();
                        foreach (POS posItem in posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2))
                        {
                            currentBaseValue = posItem.Amount1Base_23.HasValue ? posItem.Amount1Base_23.Value : 0;
                            currentBaseValue += posItem.ProRataBase_56.HasValue ? posItem.ProRataBase_56.Value : 0;
                            quoteType = string.IsNullOrEmpty(posItem.QuoteType_51) == false && posItem.QuoteType_51.Trim() == "%" ? 100m : 1m;
                            bondGreatThan5 = new BondsWithMaturityGreatherThanFiveYears()
                            {
                                ValorNumber = posItem.NumSecurity_29 != null ? posItem.NumSecurity_29 : 0,
                                Currency = posItem.Currency1_17,
                                Description = ((string.IsNullOrEmpty(posItem.Description1_32) ? "" : posItem.Description1_32) + " " + (string.IsNullOrEmpty(posItem.Description2_33) ? "" : posItem.Description2_33)).Trim(),
                                CurrentPrice = posItem.Quote_48 != null ? posItem.Quote_48.Value : 0,
                                PurchasePrice = posItem.BuyPriceHistoric_53 != null ? posItem.BuyPriceHistoric_53.Value : 0,
                                PriceBeginningYear = posItem.BuyPriceAverage_87 != null ? posItem.BuyPriceAverage_87.Value : 0,
                                Isin = posItem.IsinIban_85,
                                NominalAmount = posItem.Quantity_28 != null ? posItem.Quantity_28.Value : 0,
                                SPRating = (string.IsNullOrEmpty(posItem.AgeRat_97) == false && posItem.AgeRat_97.Trim() == "SP") ? posItem.Rating_98 : string.Empty,
                                MsciEsg = (string.IsNullOrEmpty(posItem.AgeRat_97) == false && posItem.AgeRat_97.Trim() == "ES") ? posItem.Rating_98 : string.Empty,
                                ExchangeRateImpactPurchase = posItem.BuyExchangeRateHistoric_66 != null ? posItem.BuyExchangeRateHistoric_66.Value : 0,  //temporary
                                ExchangeRateImpactYTD = posItem.BuyExchangeRateAverage_88 != null ? posItem.BuyExchangeRateAverage_88.Value : 0,  //temporary
                                PercentAsset = customerSumAmounts != 0 && currentBaseValue != 0 ? Math.Round(currentBaseValue / customerSumAmounts * 100m, _calcShares.MeaningfulDecimalDigits) : 0
                            };
                            currencyRate = (curItems != null && curItems.Any(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == bondGreatThan5.Currency && flt.Rate_6 != null)) ? curItems.First(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == bondGreatThan5.Currency && flt.Rate_6 != null).Rate_6.Value : 0;
                            bondGreatThan5.PerformancePurchase = Math.Round((bondGreatThan5.CurrentPrice.Value - bondGreatThan5.PurchasePrice.Value) * bondGreatThan5.NominalAmount.Value / quoteType, _calcShares.MeaningfulDecimalDigits);
                            bondGreatThan5.PerformanceYTD = Math.Round((bondGreatThan5.CurrentPrice.Value - bondGreatThan5.PriceBeginningYear.Value) * bondGreatThan5.NominalAmount.Value / quoteType, _calcShares.MeaningfulDecimalDigits);
                            bondGreatThan5.PercentPerformancePurchase = _calcShares.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcShares.GetSign(bondGreatThan5.NominalAmount, bondGreatThan5.CurrentPrice), bondGreatThan5.PurchasePrice.Value, bondGreatThan5.CurrentPrice.Value));
                            bondGreatThan5.PercentPerformanceYTD = _calcShares.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcShares.GetSign(bondGreatThan5.NominalAmount, bondGreatThan5.CurrentPrice), bondGreatThan5.PriceBeginningYear.Value, bondGreatThan5.CurrentPrice.Value));
                            bondGreatThan5.ExchangeRateImpactPurchase = _calcShares.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcShares.GetSign(bondGreatThan5.NominalAmount, bondGreatThan5.CurrentPrice), bondGreatThan5.ExchangeRateImpactPurchase.Value, currencyRate));
                            bondGreatThan5.ExchangeRateImpactYTD = _calcShares.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcShares.GetSign(bondGreatThan5.NominalAmount, bondGreatThan5.CurrentPrice), bondGreatThan5.ExchangeRateImpactYTD.Value, currencyRate));
                            sectionContent.BondsWithMatGreatThanFiveYears.Add(bondGreatThan5);
                            posItem.AlreadyUsed = true;
                        }
                        output.Content = new Section15Content(sectionContent);
                    }
                }
            }
            return output;
        }

    }

}
