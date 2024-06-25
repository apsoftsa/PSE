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

    public class ManipulatorSection10 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection10(CultureInfo? culture = null) : base(PositionClassifications.INVESTIMENTI_FIDUCIARI, ManipolationTypes.AsSection10, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section10 output = new()
            {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) && extractedData.Any(flt => flt.RecordType == nameof(POS)))
            {
                decimal customerSumAmounts, currentBaseValue;
                IFiduciaryInvestmentAccount account;
                ISection10Content sectionContent;
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> posItems = extractedData.Where(flt => flt.AlreadyUsed == false && flt.RecordType == nameof(POS)).OfType<POS>().Where(fltSubCat => ManipulatorOperatingRules.IsRowDestinatedToManipulator(this, fltSubCat.SubCat4_15));
                foreach (IDE ideItem in ideItems)
                {
                    customerSumAmounts = extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>().Where(subFlt => subFlt.CustomerNumber_2 == ideItem.CustomerNumber_2 && subFlt.Amount1Base_23.HasValue).Sum(sum => sum.Amount1Base_23.Value);
                    customerSumAmounts += extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>().Where(subFlt => subFlt.CustomerNumber_2 == ideItem.CustomerNumber_2 && subFlt.ProRataBase_56.HasValue).Sum(sum => sum.ProRataBase_56.Value);
                    if (posItems != null && posItems.Any(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2))
                    {
                        sectionContent = new Section10Content();
                        foreach (POS posItem in posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2))
                        {
                            currentBaseValue = posItem.Amount1Base_23.HasValue ? posItem.Amount1Base_23.Value : 0;
                            currentBaseValue += posItem.ProRataBase_56.HasValue ? posItem.ProRataBase_56.Value : 0;
                            account = new FiduciaryInvestmentAccount()
                            {
                                Account = posItem.HostPositionReference_6,
                                Currency = posItem.Currency1_17,
                                MarketValueReportingCurrency = posItem.Amount1Base_23 != null ? posItem.Amount1Base_23.Value : 0,
                                FaceValue = posItem.Quantity_28 != null ? posItem.Quantity_28.Value : 0,
                                PercentInterest = posItem.InterestRate_47 != null ? posItem.InterestRate_47.Value : 0,
                                ExpirationDate = posItem.MaturityDate_36 != null ? posItem.MaturityDate_36.Value.ToString(DEFAULT_DATE_FORMAT, _culture) : "",
                                OpeningDate = posItem.ConversionDateStart_41 != null ? posItem.ConversionDateStart_41.Value.ToString(DEFAULT_DATE_FORMAT, _culture) : "",
                                NoDeposit = posItem.MovementKey_31,
                                Correspondent = posItem.Description2_33,
                                AccruedInterestReportingCurrency = posItem.ProRataBase_56 != null ? posItem.ProRataBase_56.Value : 0,
                                PercentAsset = customerSumAmounts != 0 && currentBaseValue != 0 ? Math.Round(currentBaseValue / customerSumAmounts * 100m, DEFAULT_MEANINGFUL_DECIMAL_DIGITS_FOR_CALCULATION) : 0
                            };
                            sectionContent.Accounts.Add(account);
                            posItem.AlreadyUsed = true;
                        }
                        output.Content = new Section10Content(sectionContent);
                    }
                }
            }
            return output;
        }

    }

}
