using System.Globalization;
using PSE.BusinessLogic.Common;
using PSE.BusinessLogic.Interfaces;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using PSE.Model.SupportTables;
using static PSE.Model.Common.Constants;
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection9 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection9(CultureInfo? culture = null) : base(PositionClassifications.INVESTIMENTI_BREVE_TERMINE, ManipolationTypes.AsSection9, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section9 output = new()
            {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) && extractedData.Any(flt => flt.RecordType == nameof(POS)))
            {
                decimal customerSumAmounts, currentBaseValue;
                IShortTermInvestment shortTermInvestiment;
                ISection9Content sectionContent;
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> posItems = extractedData.Where(flt => flt.AlreadyUsed == false && flt.RecordType == nameof(POS)).OfType<POS>().Where(fltSubCat => ManipulatorOperatingRules.IsRowDestinatedToManipulator(this, fltSubCat.SubCat4_15));
                foreach (IDE ideItem in ideItems)
                {
                    customerSumAmounts = extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>().Where(subFlt => subFlt.CustomerNumber_2 == ideItem.CustomerNumber_2 && subFlt.Amount1Base_23.HasValue).Sum(sum => sum.Amount1Base_23.Value);
                    customerSumAmounts += extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>().Where(subFlt => subFlt.CustomerNumber_2 == ideItem.CustomerNumber_2 && subFlt.ProRataBase_56.HasValue).Sum(sum => sum.ProRataBase_56.Value);
                    if (posItems != null && posItems.Any(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2))
                    {
                        sectionContent = new Section9Content();
                        foreach (POS posItem in posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2))
                        {
                            currentBaseValue = posItem.Amount1Base_23.HasValue ? posItem.Amount1Base_23.Value : 0;
                            currentBaseValue += posItem.ProRataBase_56.HasValue ? posItem.ProRataBase_56.Value : 0;
                            shortTermInvestiment = new ShortTermInvestment()
                            {
                                Description = ((string.IsNullOrEmpty(posItem.Description1_32) ? "" : posItem.Description1_32) + " " + (string.IsNullOrEmpty(posItem.Description2_33) ? "" : posItem.Description2_33)).Trim(),
                                Currency = posItem.Currency1_17,
                                NominalAmount = posItem.Quantity_28 != null ? posItem.Quantity_28.Value : 0,
                                ValorNumber = posItem.NumSecurity_29 != null ? posItem.NumSecurity_29.Value : 0,
                                CurrentPrice = posItem.Quote_48 != null ? posItem.Quote_48.Value : null,
                                PurchasePrice = posItem.BuyPriceHistoric_53 != null ? posItem.BuyPriceHistoric_53.Value : 0,
                                Isin = posItem.IsinIban_85,
                                PriceBeginningYear = posItem.BuyPriceAverage_87 != null ? posItem.BuyPriceAverage_87.Value : 0,
                                DescriptionExtra = "[DescriptionExtra]", // not still recovered (!)
                                SPRating = (string.IsNullOrEmpty(posItem.AgeRat_97) == false && posItem.AgeRat_97.Trim() == "SP") ? posItem.Rating_98 : string.Empty,
                                MsciEsg = (string.IsNullOrEmpty(posItem.AgeRat_97) == false && posItem.AgeRat_97.Trim() == "ES") ? posItem.Rating_98 : string.Empty,
                                ExchangeRateImpactPurchase = 0, // not still recovered (!)
                                ExchangeRateImpactYTD = 0, // not still recovered (!)
                                PerformancePurchase = 0, // not still recovered (!)
                                PercentPerformancePurchase = 0, // not still recovered (!)
                                PerformanceYTD = 0, // not still recovered (!)
                                PercentPerformanceYTD = 0, // not still recovered (!)
                                PercentAsset = customerSumAmounts != 0 && currentBaseValue != 0 ? Math.Round(currentBaseValue / customerSumAmounts * 100m, DEFAULT_MEANINGFUL_DECIMAL_DIGITS_FOR_CALCULATION) : 0
                            };
                            sectionContent.Investments.Add(shortTermInvestiment);
                            posItem.AlreadyUsed = true;
                        }
                        output.Content = new Section9Content(sectionContent);
                    }
                }
            }
            return output;
        }

    }

}
