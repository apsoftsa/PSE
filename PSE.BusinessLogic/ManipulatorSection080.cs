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

        public ManipulatorSection080(CalculationSettings calcSettings, CultureInfo? culture = null) : base(PositionClassifications.OBBLIGAZIONI_CON_SCADENZA_MINOR_OR_EQUAL_5_ANNI, ManipolationTypes.AsSection080, culture) 
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
                decimal currencyRate, customerSumAmounts, quoteType, currentBaseValue;
                IBondDetail bondLessThan5;
                ISection080Content sectionContent;                
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> posItems = extractedData.Where(flt => flt.AlreadyUsed == false && flt.RecordType == nameof(POS)).OfType<POS>().Where(fltSubCat => ManipulatorOperatingRules.IsRowDestinatedToManipulator(this, fltSubCat.SubCat4_15));
                IEnumerable<CUR> curItems = extractedData.Where(flt => flt.RecordType == nameof(CUR)).OfType<CUR>(); 
                foreach (IDE ideItem in ideItems)
                {
                    customerSumAmounts = extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>().Where(subFlt => subFlt.CustomerNumber_2 == ideItem.CustomerNumber_2 && subFlt.Amount1Base_23.HasValue).Sum(sum => sum.Amount1Base_23.Value);
                    customerSumAmounts += extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>().Where(subFlt => subFlt.CustomerNumber_2 == ideItem.CustomerNumber_2 && subFlt.ProRataBase_56.HasValue).Sum(sum => sum.ProRataBase_56.Value);
                    if (posItems != null && posItems.Any(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2))
                    {
                        sectionContent = new Section080Content();  
                        sectionContent.Subsection8010 = new BondSubSection("Bonds with maturity <= 5 year");
                        foreach (POS posItem in posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2))                        
                        {
                            currentBaseValue = posItem.Amount1Base_23.HasValue ? posItem.Amount1Base_23.Value : 0;
                            currentBaseValue += posItem.ProRataBase_56.HasValue ? posItem.ProRataBase_56.Value : 0;
                            quoteType = string.IsNullOrEmpty(posItem.QuoteType_51) == false && posItem.QuoteType_51.Trim() == "%" ? 100m : 1m;
                            bondLessThan5 = new BondDetail()
                            {
                                //ValorNumber = posItem.NumSecurity_29 != null ? posItem.NumSecurity_29 : 0,
                                Currency = posItem.Currency1_17,
                                Description1 = ((string.IsNullOrEmpty(posItem.Description1_32) ? "" : posItem.Description1_32) + " " + (string.IsNullOrEmpty(posItem.Description2_33) ? "" : posItem.Description2_33)).Trim(),
                                //Expiration = posItem.MaturityDate_36 != null ? ((DateTime)posItem.MaturityDate_36).ToString(DEFAULT_DATE_FORMAT, _culture) : "",
                                //CurrentPrice = posItem.Quote_48 != null ? posItem.Quote_48.Value : 0,
                                //PurchasePrice = posItem.BuyPriceHistoric_53 != null ? posItem.BuyPriceHistoric_53.Value : 0,
                                //Isin = posItem.IsinIban_85,
                                //PriceBeginningYear = posItem.BuyPriceAverage_87 != null ? posItem.BuyPriceAverage_87.Value : 0,
                                //PercentCoupon = posItem.InterestRate_47 != null ? posItem.InterestRate_47.Value : 0,
                                //PercentYTM = 0, // not still recovered (!)
                                NominalAmount = posItem.Quantity_28 != null ? posItem.Quantity_28.Value : 0,
                                //ExchangeRateImpactPurchase = posItem.BuyExchangeRateHistoric_66 != null ? posItem.BuyExchangeRateHistoric_66.Value : 0, //temporary
                                SpRating = (string.IsNullOrEmpty(posItem.AgeRat_97) == false && posItem.AgeRat_97.Trim() == "SP") ? posItem.Rating_98 : string.Empty,
                                //MsciEsg = (string.IsNullOrEmpty(posItem.AgeRat_97) == false && posItem.AgeRat_97.Trim() == "ES") ? posItem.Rating_98 : string.Empty,
                                //ExchangeRateImpactYTD = posItem.BuyExchangeRateAverage_88 != null ? posItem.BuyExchangeRateAverage_88.Value : 0, // temporary                                                             
                                //PercentAsset = customerSumAmounts != 0 && currentBaseValue != 0 ? Math.Round(currentBaseValue / customerSumAmounts * 100m, _calcBonds.MeaningfulDecimalDigits) : 0
                            };
                            currencyRate = (curItems != null && curItems.Any(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == bondLessThan5.Currency && flt.Rate_6 != null)) ? curItems.First(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == bondLessThan5.Currency && flt.Rate_6 != null).Rate_6.Value : 0;
                            //bondLessThan5.PerformancePurchase = Math.Round((bondLessThan5.CurrentPrice.Value - bondLessThan5.PurchasePrice.Value) * bondLessThan5.NominalAmount.Value / quoteType, _calcBonds.MeaningfulDecimalDigits);
                            //bondLessThan5.PerformanceYTD = Math.Round((bondLessThan5.CurrentPrice.Value - bondLessThan5.PriceBeginningYear.Value) * bondLessThan5.NominalAmount.Value / quoteType, _calcBonds.MeaningfulDecimalDigits);
                            //bondLessThan5.PercentPerformancePurchase = _calcBonds.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcBonds.GetSign(bondLessThan5.NominalAmount, bondLessThan5.CurrentPrice), bondLessThan5.PurchasePrice.Value, bondLessThan5.CurrentPrice.Value));
                            //bondLessThan5.PercentPerformanceYTD = _calcBonds.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcBonds.GetSign(bondLessThan5.NominalAmount, bondLessThan5.CurrentPrice), bondLessThan5.PriceBeginningYear.Value, bondLessThan5.CurrentPrice.Value));
                            //bondLessThan5.ExchangeRateImpactPurchase = _calcBonds.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcBonds.GetSign(bondLessThan5.NominalAmount, bondLessThan5.CurrentPrice), bondLessThan5.ExchangeRateImpactPurchase.Value, currencyRate));
                            //bondLessThan5.ExchangeRateImpactYTD = _calcBonds.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcBonds.GetSign(bondLessThan5.NominalAmount, bondLessThan5.CurrentPrice), bondLessThan5.ExchangeRateImpactYTD.Value, currencyRate));
                            sectionContent.Subsection8010.Content.Add(bondLessThan5);
                            posItem.AlreadyUsed = true;
                        }
                        output.Content = new Section080Content(sectionContent);
                    }
                }
            }
            return output;
        }

    }

}
