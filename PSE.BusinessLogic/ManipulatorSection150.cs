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

    public class ManipulatorSection150 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection150(CultureInfo? culture = null) : base(new List<PositionClassifications>() { PositionClassifications.MUTUI_IPOTECARI_E_CREDITI_DI_COSTRUZIONE, PositionClassifications.IMPEGNI_EVENTUALI }, ManipolationTypes.AsSection150, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData, decimal? totalAssets = null)
        {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section150 output = new()
            {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };            
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) && extractedData.Any(flt => flt.RecordType == nameof(POS)))
            {                
                ISection150Content sectionContent;
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> posItems = extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>();                
                foreach (IDE ideItem in ideItems)
                {
                    sectionContent = new Section150Content();
                    IEnumerable<POS> possibleCommitments = posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2 && flt.SubCat3_14 == ((int)PositionClassifications.POSIZIONI_INFORMATIVE).ToString() && flt.SubCat4_15 == ((int)PositionClassifications.IMPEGNI_EVENTUALI).ToString());
                    if (possibleCommitments != null && possibleCommitments.Any())
                    {
                        foreach (var possibleCommitment in possibleCommitments)
                        {
                            sectionContent.SubSection15000.Content.Add(new PossibleCommitment()
                            {
                                Description1 = BuildComposedDescription([AssignRequiredString(possibleCommitment.HostPositionReference_6), AssignRequiredString(possibleCommitment.Currency1_17), AssignRequiredString(possibleCommitment.Description2_33)]),
                                Description2 = string.Empty,
                                OpeningDate = string.Empty,
                                ExpirationDate = string.Empty,
                                CurrentBalance = AssignRequiredDecimal(possibleCommitment.Amount1Cur1_22),
                                MarketValueReportingCurrency = AssignRequiredDecimal(possibleCommitment.Amount1Base_23),
                                AccruedInterestReportingCurrency = 0
                            });
                        }
                    }
                    output.Content = new Section150Content(sectionContent);
                }
            }    
            return output;
        }

    }

}
