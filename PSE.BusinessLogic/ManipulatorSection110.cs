using System.Globalization;
using PSE.BusinessLogic.Calculations;
using PSE.BusinessLogic.Common;
using PSE.BusinessLogic.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Input.Interfaces;
using PSE.Model.Output.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.SupportTables;
using static PSE.Model.Common.Constants;
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection110 : ManipulatorBase, IManipulator
    {

        private readonly OtherInvestmentsCalculation _calcOtherInvs;

        public ManipulatorSection110(CalculationSettings calcSettings, CultureInfo? culture = null) : base(new List<PositionClassifications>()
            {
                PositionClassifications.PRODOTTI_DERIVATI_SU_METALLI, 
                PositionClassifications.PRODOTTI_DERIVATI, 
                PositionClassifications.PRODOTTI_ALTERNATIVI_DIVERSI
            }, 
            ManipolationTypes.AsSection110, culture) 
        {
            _calcOtherInvs = new OtherInvestmentsCalculation(calcSettings);
        }

        public override string GetObjectNameDestination(IInputRecord inputRecord)
        {
            string destinationObjectName = string.Empty;
            if (inputRecord != null && inputRecord.GetType() == typeof(POS))
            {
                string subCategory = ((POS)inputRecord).SubCat4_15;
                if (subCategory != null && Enum.IsDefined(typeof(PositionClassifications), int.Parse(subCategory)))
                {
                    if (this.PositionClassificationsSource.Contains((PositionClassifications)int.Parse(subCategory)))
                    {
                        switch ((PositionClassifications)int.Parse(subCategory))
                        {
                            case PositionClassifications.PRODOTTI_DERIVATI_SU_METALLI:
                                destinationObjectName = "DerivativesOnMetals";
                                break;
                            case PositionClassifications.PRODOTTI_DERIVATI:
                            case PositionClassifications.PRODOTTI_ALTERNATIVI_DIVERSI:
                                destinationObjectName = "Different";
                                break;
                        }
                    }
                }
            }
            return destinationObjectName;
        }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData, decimal? totalAssets = null)
        {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section110 output = new()
            {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) && extractedData.Any(flt => flt.RecordType == nameof(POS)))
            {
                decimal currencyRate, customerSumAmounts, currentBaseValue, quoteType;
                string destinationObjectName;
                IBondInvestmentDetail altProdDetails;
                ISection110Content sectionContent;
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> posItems = extractedData.Where(flt => flt.AlreadyUsed == false && flt.RecordType == nameof(POS)).OfType<POS>().Where(fltSubCat => ManipulatorOperatingRules.IsRowDestinatedToManipulator(this, fltSubCat.SubCat4_15));
                IEnumerable<CUR> curItems = extractedData.Where(flt => flt.RecordType == nameof(CUR)).OfType<CUR>();
                foreach (IDE ideItem in ideItems)
                {
                    customerSumAmounts = extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>().Where(subFlt => subFlt.CustomerNumber_2 == ideItem.CustomerNumber_2 && subFlt.Amount1Base_23.HasValue).Sum(sum => sum.Amount1Base_23.Value);
                    customerSumAmounts += extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>().Where(subFlt => subFlt.CustomerNumber_2 == ideItem.CustomerNumber_2 && subFlt.ProRataBase_56.HasValue).Sum(sum => sum.ProRataBase_56.Value);
                    if (posItems != null && posItems.Any(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2))
                    {
                        sectionContent = new Section110Content();
                        sectionContent.SubSection11010 = new SubSection11010("Alternative Products – Different");
                        foreach (POS posItem in posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2))
                        {
                            if ((destinationObjectName = GetObjectNameDestination(posItem)) != string.Empty)
                            {
                                currentBaseValue = posItem.Amount1Base_23.HasValue ? posItem.Amount1Base_23.Value : 0;
                                currentBaseValue += posItem.ProRataBase_56.HasValue ? posItem.ProRataBase_56.Value : 0;
                                quoteType = string.IsNullOrEmpty(posItem.QuoteType_51) == false && posItem.QuoteType_51.Trim() == "%" ? 100m : 1m;
                                altProdDetails = new BondInvestmentDetail()
                                {
                                    //ValorNumber = posItem.NumSecurity_29 != null ? posItem.NumSecurity_29 : 0,
                                    Currency = posItem.Currency1_17,
                                    Description1 = ((string.IsNullOrEmpty(posItem.Description1_32) ? "" : posItem.Description1_32) + " " + (string.IsNullOrEmpty(posItem.Description2_33) ? "" : posItem.Description2_33)).Trim(),
                                    Description2 = posItem.CallaDate_38 != null ? ((DateTime)posItem.CallaDate_38).ToString(DEFAULT_DATE_FORMAT, _culture) : "",
                                    //CurrentPrice = posItem.Quote_48 != null ? posItem.Quote_48.Value : 0,
                                    //PurchasePrice = posItem.BuyPriceHistoric_53 != null ? posItem.BuyPriceHistoric_53.Value : 0,
                                    //Isin = posItem.IsinIban_85,
                                    //PriceBeginningYear = posItem.BuyPriceAverage_87 != null ? posItem.BuyPriceAverage_87.Value : 0,
                                    NominalAmount = posItem.Quantity_28 != null ? posItem.Quantity_28.Value : 0,
                                    //UnderlyingDescription = posItem.ConversionDesc_45,
                                    //ExchangeRateImpactPurchase = posItem.BuyExchangeRateHistoric_66 != null ? posItem.BuyExchangeRateHistoric_66.Value : 0, // temporary
                                    //ExchangeRateImpactYTD = posItem.BuyExchangeRateAverage_88 != null ? posItem.BuyExchangeRateAverage_88.Value : 0, // temporary
                                    //PercentAsset = customerSumAmounts != 0 && currentBaseValue != 0 ? Math.Round(currentBaseValue / customerSumAmounts * 100m, _calcOtherInvs.MeaningfulDecimalDigits) : 0
                                };
                                currencyRate = (curItems != null && curItems.Any(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == altProdDetails.Currency && flt.Rate_6 != null)) ? curItems.First(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == altProdDetails.Currency && flt.Rate_6 != null).Rate_6.Value : 0;
                                //altProdDetails.PerformancePurchase = Math.Round((altProdDetails.CurrentPrice.Value - altProdDetails.PurchasePrice.Value) * altProdDetails.NominalAmount.Value / quoteType, _calcOtherInvs.MeaningfulDecimalDigits);
                                //altProdDetails.PerformanceYTD = Math.Round((altProdDetails.CurrentPrice.Value - altProdDetails.PriceBeginningYear.Value) * altProdDetails.NominalAmount.Value / quoteType, _calcOtherInvs.MeaningfulDecimalDigits);
                                //altProdDetails.PercentPerformancePurchase = _calcOtherInvs.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcOtherInvs.GetSign(altProdDetails.NominalAmount, altProdDetails.CurrentPrice), altProdDetails.PurchasePrice.Value, altProdDetails.CurrentPrice.Value));
                                //altProdDetails.PercentPerformanceYTD = _calcOtherInvs.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcOtherInvs.GetSign(altProdDetails.NominalAmount, altProdDetails.CurrentPrice), altProdDetails.PriceBeginningYear.Value, altProdDetails.CurrentPrice.Value));
                                //altProdDetails.ExchangeRateImpactPurchase = _calcOtherInvs.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcOtherInvs.GetSign(altProdDetails.NominalAmount, altProdDetails.CurrentPrice), altProdDetails.ExchangeRateImpactPurchase.Value, currencyRate));
                                //altProdDetails.ExchangeRateImpactYTD = _calcOtherInvs.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcOtherInvs.GetSign(altProdDetails.NominalAmount, altProdDetails.CurrentPrice), altProdDetails.ExchangeRateImpactYTD.Value, currencyRate));
                                //if (destinationObjectName == "Different")
                                //    altProdDefinitions.Different.Add(altProdDetails);
                                //else if (destinationObjectName == "DerivativesOnMetals")
                                //    altProdDefinitions.DerivativesOnMetals.Add(altProdDetails);
                                posItem.AlreadyUsed = true;
                                sectionContent.SubSection11010.Content.Add(altProdDetails);
                            }
                        }                                                                        
                        output.Content = new Section110Content(sectionContent);
                    }
                }
            }
            return output;
        }

    }

}
