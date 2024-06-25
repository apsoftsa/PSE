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

    public class ManipulatorSection16And17 : ManipulatorBase, IManipulator
    {

        private readonly FundsCalculation _calcFunds;

        public ManipulatorSection16And17(CalculationSettings calcSettings, CultureInfo? culture = null) : base(new List<PositionClassifications>()
            {
                PositionClassifications.OBBLIGAZIONI_CON_SCADENZA_MINOR_OR_EQUAL_1_ANNO,
                PositionClassifications.OBBLIGAZIONI_CON_SCADENZA_MINOR_OR_EQUAL_5_ANNI,
                PositionClassifications.OBBLIGAZIONI_CON_SCADENZA_MAJOR_THAN_5_ANNI_FONDI_OBBLIGAZIONARI,
                PositionClassifications.AZIONI_FONDI_AZIONARI,
                PositionClassifications.CONTI_METALLO_METALLI_FONDI_METALLO,
                PositionClassifications.FONDI_MISTI,
                PositionClassifications.PARTECIPAZIONI_IMMOBILIARI_FONDI_IMMOBILIARI
            }
            , ManipolationTypes.AsSection16And17, culture)
        {
            _calcFunds = new FundsCalculation(calcSettings);
        }

        public override string GetObjectNameDestination(IInputRecord inputRecord)
        {
            string destinationObjectName = string.Empty;
            if (inputRecord != null && inputRecord.GetType() == typeof(POS))
            {
                string subCategory = ((POS)inputRecord).SubCat4_15;
                string category = ((POS)inputRecord).Category_11;
                if (subCategory != string.Empty && category != string.Empty && category.Trim().Length >= 8 
                    && Enum.IsDefined(typeof(PositionClassifications), int.Parse(subCategory)))
                {
                    PositionClassifications currPosClass = (PositionClassifications)int.Parse(subCategory);
                    if (this.PositionClassificationsSource.Contains((PositionClassifications)int.Parse(subCategory)))
                    {
                        switch (category.Substring(6,2).ToUpper())
                        {
                            case "FO":
                                {
                                    if(currPosClass == PositionClassifications.OBBLIGAZIONI_CON_SCADENZA_MINOR_OR_EQUAL_1_ANNO
                                        || currPosClass == PositionClassifications.OBBLIGAZIONI_CON_SCADENZA_MINOR_OR_EQUAL_5_ANNI
                                        || currPosClass == PositionClassifications.OBBLIGAZIONI_CON_SCADENZA_MAJOR_THAN_5_ANNI_FONDI_OBBLIGAZIONARI)
                                        destinationObjectName = "BondFunds";
                                    break;
                                }
                            case "FA":
                                {
                                    if (currPosClass == PositionClassifications.AZIONI_FONDI_AZIONARI)
                                        destinationObjectName = "EquityFunds";
                                    break;
                                }
                            case "FI":
                                {
                                    if (currPosClass == PositionClassifications.PARTECIPAZIONI_IMMOBILIARI_FONDI_IMMOBILIARI)
                                        destinationObjectName = "RealEstateFunds";
                                    break;
                                }
                            case "MF":
                                {
                                    if (currPosClass == PositionClassifications.CONTI_METALLO_METALLI_FONDI_METALLO)
                                        destinationObjectName = "MetalFunds";
                                    break;
                                }
                            case "FM":
                                {
                                    if (currPosClass == PositionClassifications.FONDI_MISTI)
                                        destinationObjectName = "MixedFunds";
                                    break;
                                }
                        }
                    }
                }
            }
            return destinationObjectName;
        }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section16And17 output = new()
            {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) && extractedData.Any(flt => flt.RecordType == nameof(POS)))
            {
                decimal currencyRate, customerSumAmounts, currentBaseValue, quoteType;
                string destinationObjectName;
                IFundDetails fundDetails;
                ISection16And17Content sectionContent;
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> posItems = extractedData.Where(flt => flt.AlreadyUsed == false && flt.RecordType == nameof(POS)).OfType<POS>().Where(fltSubCat => ManipulatorOperatingRules.IsRowDestinatedToManipulator(this, fltSubCat.SubCat4_15));
                IEnumerable<CUR> curItems = extractedData.Where(flt => flt.RecordType == nameof(CUR)).OfType<CUR>();
                foreach (IDE ideItem in ideItems)
                {
                    customerSumAmounts = extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>().Where(subFlt => subFlt.CustomerNumber_2 == ideItem.CustomerNumber_2 && subFlt.Amount1Base_23.HasValue).Sum(sum => sum.Amount1Base_23.Value);
                    customerSumAmounts += extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>().Where(subFlt => subFlt.CustomerNumber_2 == ideItem.CustomerNumber_2 && subFlt.ProRataBase_56.HasValue).Sum(sum => sum.ProRataBase_56.Value);
                    if (posItems != null && posItems.Any(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2))
                    {
                        sectionContent = new Section16And17Content();
                        foreach (POS posItem in posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2))
                        {
                            if ((destinationObjectName = GetObjectNameDestination(posItem)) != string.Empty)
                            {
                                currentBaseValue = posItem.Amount1Base_23.HasValue ? posItem.Amount1Base_23.Value : 0;
                                currentBaseValue += posItem.ProRataBase_56.HasValue ? posItem.ProRataBase_56.Value : 0;
                                quoteType = string.IsNullOrEmpty(posItem.QuoteType_51) == false && posItem.QuoteType_51.Trim() == "%" ? 100m : 1m;
                                fundDetails = new FundDetail()
                                {
                                    ValorNumber = posItem.NumSecurity_29 != null ? posItem.NumSecurity_29 : 0,
                                    Currency = posItem.Currency1_17,
                                    Description = ((string.IsNullOrEmpty(posItem.Description1_32) ? "" : posItem.Description1_32) + " " + (string.IsNullOrEmpty(posItem.Description2_33) ? "" : posItem.Description2_33)).Trim(),
                                    CurrentPrice = posItem.Quote_48 != null ? posItem.Quote_48.Value : 0,
                                    PurchasePrice = posItem.BuyPriceHistoric_53 != null ? posItem.BuyPriceHistoric_53.Value : 0,
                                    PriceBeginningYear = posItem.BuyPriceAverage_87 != null ? posItem.BuyPriceAverage_87.Value : 0,
                                    NominalAmount = posItem.Quantity_28 != null ? posItem.Quantity_28.Value : 0,
                                    ExchangeRateImpactPurchase = posItem.BuyExchangeRateHistoric_66 != null ? posItem.BuyExchangeRateHistoric_66.Value : 0, //temporary
                                    Isin = posItem.IsinIban_85,
                                    SPRating = (string.IsNullOrEmpty(posItem.AgeRat_97) == false && posItem.AgeRat_97.Trim() == "SP") ? posItem.Rating_98 : string.Empty,
                                    MsciEsg = (string.IsNullOrEmpty(posItem.AgeRat_97) == false && posItem.AgeRat_97.Trim() == "ES") ? posItem.Rating_98 : string.Empty,
                                    ExchangeRateImpactYTD = posItem.BuyExchangeRateAverage_88 != null ? posItem.BuyExchangeRateAverage_88.Value : 0, //temporary
                                    PercentAsset = customerSumAmounts != 0 && currentBaseValue != 0 ? Math.Round(currentBaseValue / customerSumAmounts * 100m, _calcFunds.MeaningfulDecimalDigits) : 0
                                };
                                currencyRate = (curItems != null && curItems.Any(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == fundDetails.Currency && flt.Rate_6 != null)) ? curItems.First(flt => flt.CustomerNumber_2 == posItem.CustomerNumber_2 && flt.Currency_5 == fundDetails.Currency && flt.Rate_6 != null).Rate_6.Value : 0;
                                fundDetails.PerformancePurchase = Math.Round((fundDetails.CurrentPrice.Value - fundDetails.PurchasePrice.Value) * fundDetails.NominalAmount.Value / quoteType, _calcFunds.MeaningfulDecimalDigits);
                                fundDetails.PerformanceYTD = Math.Round((fundDetails.CurrentPrice.Value - fundDetails.PriceBeginningYear.Value) * fundDetails.NominalAmount.Value / quoteType, _calcFunds.MeaningfulDecimalDigits);
                                fundDetails.PercentPerformancePurchase = _calcFunds.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcFunds.GetSign(fundDetails.NominalAmount, fundDetails.CurrentPrice), fundDetails.PurchasePrice.Value, fundDetails.CurrentPrice.Value));
                                fundDetails.PercentPerformanceYTD = _calcFunds.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcFunds.GetSign(fundDetails.NominalAmount, fundDetails.CurrentPrice), fundDetails.PriceBeginningYear.Value, fundDetails.CurrentPrice.Value));
                                fundDetails.ExchangeRateImpactPurchase = _calcFunds.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcFunds.GetSign(fundDetails.NominalAmount, fundDetails.CurrentPrice), fundDetails.ExchangeRateImpactPurchase.Value, currencyRate));
                                fundDetails.ExchangeRateImpactYTD = _calcFunds.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcFunds.GetSign(fundDetails.NominalAmount, fundDetails.CurrentPrice), fundDetails.ExchangeRateImpactYTD.Value, currencyRate));
                                if (destinationObjectName == "BondFunds")
                                    sectionContent.BondFunds.Add(fundDetails);
                                else if (destinationObjectName == "EquityFunds")
                                    sectionContent.EquityFunds.Add(fundDetails);
                                else if (destinationObjectName == "RealEstateFunds")
                                    sectionContent.RealEstateFunds.Add(fundDetails);
                                else if (destinationObjectName == "MetalFunds")
                                    sectionContent.MetalFunds.Add(fundDetails);
                                else if (destinationObjectName == "MixedFunds")
                                    sectionContent.MixedFunds.Add(fundDetails);
                                posItem.AlreadyUsed = true;
                            }
                        }
                        output.Content = new Section16And17Content(sectionContent);
                    }
                }
            }
            return output;
        }

    }

}
