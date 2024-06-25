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
using static PSE.Model.Common.Constants;
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection14 : ManipulatorBase, IManipulator
    {
        
        private readonly BondsCalculation _calcBonds;

        public ManipulatorSection14(CalculationSettings calcSettings, CultureInfo? culture = null) : base(PositionClassifications.OBBLIGAZIONI_CON_SCADENZA_MAJOR_THAN_5_ANNI_FONDI_OBBLIGAZIONARI, ManipolationTypes.AsSection14, culture) 
        {
            _calcBonds = new BondsCalculation(calcSettings);            
        }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section14 output = new()
            {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) && extractedData.Any(flt => flt.RecordType == nameof(POS)))
            {
                decimal currencyRate, customerSumAmounts, currentBaseValue, quoteType;
                IObligationsWithMaturityGreatherThanFiveYears oblWithMatGreatThan5;
                ISection14Content sectionContent;
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> posItems = extractedData.Where(flt => flt.AlreadyUsed == false && flt.RecordType == nameof(POS)).OfType<POS>().Where(fltSubCat => ManipulatorOperatingRules.IsRowDestinatedToManipulator(this, fltSubCat.SubCat4_15));
                IEnumerable<CUR> curItems = extractedData.Where(flt => flt.RecordType == nameof(CUR)).OfType<CUR>();
                foreach (IDE ideItem in ideItems)
                {
                    customerSumAmounts = extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>().Where(subFlt => subFlt.CustomerNumber_2 == ideItem.CustomerNumber_2 && subFlt.Amount1Base_23.HasValue).Sum(sum => sum.Amount1Base_23.Value);
                    customerSumAmounts += extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>().Where(subFlt => subFlt.CustomerNumber_2 == ideItem.CustomerNumber_2 && subFlt.ProRataBase_56.HasValue).Sum(sum => sum.ProRataBase_56.Value);
                    if (posItems != null && posItems.Any(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2))
                    {
                        sectionContent = new Section14Content();
                        foreach (POS posItem in posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2))
                        {
                            currentBaseValue = posItem.Amount1Base_23.HasValue ? posItem.Amount1Base_23.Value : 0;
                            currentBaseValue += posItem.ProRataBase_56.HasValue ? posItem.ProRataBase_56.Value : 0;
                            quoteType = string.IsNullOrEmpty(posItem.QuoteType_51) == false && posItem.QuoteType_51.Trim() == "%" ? 100m : 1m;
                            oblWithMatGreatThan5 = new ObligationsWithMaturityGreatherThanFiveYears()
                            {
                                ValorNumber = posItem.NumSecurity_29 != null ? posItem.NumSecurity_29 : 0,
                                Currency = posItem.Currency1_17,
                                Description = ((string.IsNullOrEmpty(posItem.Description1_32) ? "" : posItem.Description1_32) + " " + (string.IsNullOrEmpty(posItem.Description2_33) ? "" : posItem.Description2_33)).Trim(),
                                CurrentPrice = posItem.Quote_48 != null ? posItem.Quote_48.Value : 0,
                                PurchasePrice = posItem.BuyPriceHistoric_53 != null ? posItem.BuyPriceHistoric_53.Value : 0,
                                PriceBeginningYear = posItem.BuyPriceAverage_87 != null ? posItem.BuyPriceAverage_87.Value : 0,
                                Expiration = posItem.MaturityDate_36 != null ? ((DateTime)posItem.MaturityDate_36).ToString(DEFAULT_DATE_FORMAT, _culture) : "",
                                Isin = posItem.IsinIban_85,
                                PercentCoupon = posItem.InterestRate_47 != null ? posItem.InterestRate_47.Value : 0,
                                PercentYTM = 0, // not still recovered (!)
                                NominalAmount = posItem.Quantity_28 != null ? posItem.Quantity_28.Value : 0,
                                SPRating = (string.IsNullOrEmpty(posItem.AgeRat_97) == false && posItem.AgeRat_97.Trim() == "SP") ? posItem.Rating_98 : string.Empty,
                                MsciEsg = (string.IsNullOrEmpty(posItem.AgeRat_97) == false && posItem.AgeRat_97.Trim() == "ES") ? posItem.Rating_98 : string.Empty,
                                ExchangeRateImpactPurchase = posItem.BuyExchangeRateHistoric_66 != null ? posItem.BuyExchangeRateHistoric_66.Value : 0, //temporary                               
                                ExchangeRateImpactYTD = posItem.BuyExchangeRateAverage_88 != null ? posItem.BuyExchangeRateAverage_88.Value : 0, //temporary
                                PercentAsset = customerSumAmounts != 0 && currentBaseValue != 0 ? Math.Round(currentBaseValue / customerSumAmounts * 100m, _calcBonds.MeaningfulDecimalDigits) : 0
                            };
                            currencyRate = (curItems != null && curItems.Any(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == oblWithMatGreatThan5.Currency && flt.Rate_6 != null)) ? curItems.First(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == oblWithMatGreatThan5.Currency && flt.Rate_6 != null).Rate_6.Value : 0;
                            oblWithMatGreatThan5.PerformancePurchase = Math.Round((oblWithMatGreatThan5.CurrentPrice.Value - oblWithMatGreatThan5.PurchasePrice.Value) * oblWithMatGreatThan5.NominalAmount.Value / quoteType, _calcBonds.MeaningfulDecimalDigits);
                            oblWithMatGreatThan5.PerformanceYTD = Math.Round((oblWithMatGreatThan5.CurrentPrice.Value - oblWithMatGreatThan5.PriceBeginningYear.Value) * oblWithMatGreatThan5.NominalAmount.Value / quoteType, _calcBonds.MeaningfulDecimalDigits);
                            oblWithMatGreatThan5.PercentPerformancePurchase = _calcBonds.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcBonds.GetSign(oblWithMatGreatThan5.NominalAmount, oblWithMatGreatThan5.CurrentPrice), oblWithMatGreatThan5.PurchasePrice.Value, oblWithMatGreatThan5.CurrentPrice.Value));
                            oblWithMatGreatThan5.PercentPerformanceYTD = _calcBonds.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcBonds.GetSign(oblWithMatGreatThan5.NominalAmount, oblWithMatGreatThan5.CurrentPrice), oblWithMatGreatThan5.PriceBeginningYear.Value, oblWithMatGreatThan5.CurrentPrice.Value));
                            oblWithMatGreatThan5.ExchangeRateImpactPurchase = _calcBonds.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcBonds.GetSign(oblWithMatGreatThan5.NominalAmount, oblWithMatGreatThan5.CurrentPrice), oblWithMatGreatThan5.ExchangeRateImpactPurchase.Value, currencyRate));
                            oblWithMatGreatThan5.ExchangeRateImpactYTD = _calcBonds.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcBonds.GetSign(oblWithMatGreatThan5.NominalAmount, oblWithMatGreatThan5.CurrentPrice), oblWithMatGreatThan5.ExchangeRateImpactYTD.Value, currencyRate));
                            sectionContent.ObligationsWithMaturityGreatherThanFiveYears.Add(oblWithMatGreatThan5);
                            posItem.AlreadyUsed = true;
                        }
                        output.Content = new Section14Content(sectionContent);
                    }
                }
            }
            return output;
        }

    }

}
