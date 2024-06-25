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

    public class ManipulatorSection11 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection11(CultureInfo? culture = null) : base(PositionClassifications.OPERAZIONI_CAMBI_A_TERMINE, ManipolationTypes.AsSection11, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section11 output = new()
            {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) && extractedData.Any(flt => flt.RecordType == nameof(POS)))
            {
                decimal tmp1, tmp2, customerSumAmounts, currentBaseValue;
                IProfitLossOperation profitLossOperation;
                ISection11Content sectionContent;
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> posItems = extractedData.Where(flt => flt.AlreadyUsed == false && flt.RecordType == nameof(POS)).OfType<POS>().Where(fltSubCat => ManipulatorOperatingRules.IsRowDestinatedToManipulator(this, fltSubCat.SubCat4_15));
                foreach (IDE ideItem in ideItems)
                {
                    customerSumAmounts = extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>().Where(subFlt => subFlt.CustomerNumber_2 == ideItem.CustomerNumber_2 && subFlt.Amount1Base_23.HasValue).Sum(sum => sum.Amount1Base_23.Value);
                    customerSumAmounts += extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>().Where(subFlt => subFlt.CustomerNumber_2 == ideItem.CustomerNumber_2 && subFlt.ProRataBase_56.HasValue).Sum(sum => sum.ProRataBase_56.Value);
                    if (posItems != null && posItems.Any(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2))
                    {
                        sectionContent = new Section11Content();
                        foreach (POS posItem in posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2))
                        {
                            currentBaseValue = posItem.Amount1Base_23.HasValue ? posItem.Amount1Base_23.Value : 0;
                            currentBaseValue += posItem.ProRataBase_56.HasValue ? posItem.ProRataBase_56.Value : 0;
                            tmp1 = posItem.Amount1Base_23 != null ? posItem.Amount1Base_23.Value : 0;
                            tmp2 = posItem.Amount2Base_60 != null ? posItem.Amount2Base_60.Value : 0;
                            profitLossOperation = new ProfitLossOperation()
                            {
                                CurrencyLoss = posItem.Currency1_17,
                                Currency2 = posItem.Currency2_18,
                                AmountLoss = posItem.Amount1Cur1_22 != null ? posItem.Amount1Cur1_22.Value : 0,
                                ExpirationDate = posItem.MaturityDate_36 != null ? ((DateTime)posItem.MaturityDate_36).ToString(DEFAULT_DATE_FORMAT, _culture) : "",
                                CurrentExchangeRate = posItem.Quote_48 != null ? posItem.Quote_48.Value : 0,
                                Change = posItem.BuyPriceHistoric_53 != null ? posItem.BuyPriceHistoric_53.Value : 0,
                                Amount2 = posItem.Amount2Cur2_59 != null ? posItem.Amount2Cur2_59.Value : 0,
                                ProfitlossReportingCurrency = tmp1 + tmp2,
                                PercentAssets = customerSumAmounts != 0 && currentBaseValue != 0 ? Math.Round(currentBaseValue / customerSumAmounts * 100m, DEFAULT_MEANINGFUL_DECIMAL_DIGITS_FOR_CALCULATION) : 0
                            };
                            posItem.AlreadyUsed = true;
                            sectionContent.Operations.Add(profitLossOperation);
                        }
                        output.Content = new Section11Content(sectionContent);
                    }
                }
            }
            return output;
        }

    }

}
