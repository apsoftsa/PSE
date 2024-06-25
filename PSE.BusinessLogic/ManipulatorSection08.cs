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

    public class ManipulatorSection8 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection8(CultureInfo? culture = null) : base(PositionClassifications.CONTI, ManipolationTypes.AsSection8, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section8 output = new()
            {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) && extractedData.Any(flt => flt.RecordType == nameof(POS)))
            {
                decimal customerSumAmounts, currentBaseValue;
                IAccount account;
                ISection8Content sectionContent;
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> posItems = extractedData.Where(flt => flt.AlreadyUsed == false && flt.RecordType == nameof(POS)).OfType<POS>().Where(fltSubCat => ManipulatorOperatingRules.IsRowDestinatedToManipulator(this, fltSubCat.SubCat4_15));
                foreach (IDE ideItem in ideItems)
                {
                    customerSumAmounts = extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>().Where(subFlt => subFlt.CustomerNumber_2 == ideItem.CustomerNumber_2 && subFlt.Amount1Base_23.HasValue).Sum(sum => sum.Amount1Base_23.Value);
                    customerSumAmounts += extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>().Where(subFlt => subFlt.CustomerNumber_2 == ideItem.CustomerNumber_2 && subFlt.ProRataBase_56.HasValue).Sum(sum => sum.ProRataBase_56.Value);
                    if (posItems != null && posItems.Any(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2))
                    {
                        sectionContent = new Section8Content();
                        foreach (POS posItem in posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2))
                        {
                            currentBaseValue = posItem.Amount1Base_23.HasValue ? posItem.Amount1Base_23.Value : 0;
                            currentBaseValue += posItem.ProRataBase_56.HasValue ? posItem.ProRataBase_56.Value : 0;
                            account = new Account()
                            {
                                AccountData = posItem.HostPositionReference_6,
                                Currency = posItem.Currency1_17,
                                MarketValueReportingCurrency = posItem.Amount1Base_23 != null ? posItem.Amount1Base_23.Value : 0,
                                CurrentBalance = posItem.Quantity_28 != null ? posItem.Quantity_28.Value : 0,
                                Iban = posItem.IsinIban_85,
                                AccruedInterestReportingCurrency = 0, // not still recovered (!)
                                PercentAsset = customerSumAmounts != 0 && currentBaseValue != 0 ? Math.Round(currentBaseValue / customerSumAmounts * 100m, Model.Common.Constants.DEFAULT_MEANINGFUL_DECIMAL_DIGITS_FOR_CALCULATION) : 0
                            };
                            sectionContent.Accounts.Add(account);
                            posItem.AlreadyUsed = true;
                        }
                        output.Content = new Section8Content(sectionContent);
                    }
                }
            }
            return output;
        }

    }

}
