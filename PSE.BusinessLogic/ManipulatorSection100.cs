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

        public ManipulatorSection100(CultureInfo? culture = null) : base(PositionClassifications.CONTI_METALLO_METALLI_FONDI_METALLO, ManipolationTypes.AsSection100, culture){ }

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
                ISection100Content sectionContent;
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                foreach (IDE ideItem in ideItems)
                {
                    sectionContent = new Section100Content();
                    customerSumAmounts = extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>().Where(subFlt => subFlt.CustomerNumber_2 == ideItem.CustomerNumber_2 && subFlt.Amount1Base_23.HasValue).Sum(sum => sum.Amount1Base_23.Value);
                    IEnumerable<IGrouping<string, POS>> posItemsGroupBySubCat = extractedData.Where(flt => flt.AlreadyUsed == false && flt.RecordType == nameof(POS)).OfType<POS>().Where(fltSubCat => fltSubCat.CustomerNumber_2 == ideItem.CustomerNumber_2 && ManipulatorOperatingRules.IsRowDestinatedToManipulator(this, fltSubCat.SubCat4_15)).GroupBy(gb => gb.SubCat4_15).OrderBy(ob => ob.Key);
                    if (posItemsGroupBySubCat != null && posItemsGroupBySubCat.Any()) {
                        foreach (IGrouping<string, POS> subCategoryItems in posItemsGroupBySubCat) {
                            switch ((PositionClassifications)int.Parse(subCategoryItems.Key)) {
                                case PositionClassifications.CONTI_METALLO_METALLI_FONDI_METALLO: {
                                        sectionContent.SubSection10000 = new MetalAccountSubSection("Metals accounts");
                                        foreach (POS posItem in subCategoryItems) {
                                            currentBaseValue = posItem.Amount1Base_23.HasValue ? posItem.Amount1Base_23.Value : 0;
                                            currentBaseValue += posItem.ProRataBase_56.HasValue ? posItem.ProRataBase_56.Value : 0;
                                            metAcc = new MetalAccount() {
                                                Account = AssignRequiredString(posItem.HostPositionReference_6),                                                  
                                                CurrentBalance = AssignRequiredDecimal(posItem.Amount1Cur1_22),
                                                MarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23),
                                                PercentWeight = customerSumAmounts != 0 && currentBaseValue != 0 ? Math.Round(currentBaseValue / customerSumAmounts * 100m, Model.Common.Constants.DEFAULT_MEANINGFUL_DECIMAL_DIGITS_FOR_CALCULATION) : 0
                                            };
                                            sectionContent.SubSection10000.Content.Add(metAcc);
                                            posItem.AlreadyUsed = true;
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            return output;
        }

    }

}
