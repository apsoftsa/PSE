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

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
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
                string tmpDescr;
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
                            tmpDescr = string.IsNullOrEmpty(possibleCommitment.HostPositionReference_6) ? "" : possibleCommitment.HostPositionReference_6.Trim();
                            if (tmpDescr != "") tmpDescr += " ";
                            tmpDescr += string.IsNullOrEmpty(possibleCommitment.Currency1_17) ? "" : possibleCommitment.Currency1_17.Trim();
                            if (tmpDescr != "") tmpDescr += " ";
                            tmpDescr += string.IsNullOrEmpty(possibleCommitment.Description2_33) ? "" : possibleCommitment.Description2_33.Trim();
                            sectionContent.SubSection15000.Content.Add(new PossibleCommitment()
                            {
                                Description1 = tmpDescr,
                                Description2 = string.Empty,
                                OpeningDate = string.Empty,
                                ExpirationDate = string.Empty,
                                CurrentBalance = possibleCommitment.Amount1Cur1_22.HasValue ? Math.Round(possibleCommitment.Amount1Cur1_22.Value, 2) : 0,
                                MarketValueReportingCurrency = possibleCommitment.Amount1Base_23.HasValue ? Math.Round(possibleCommitment.Amount1Base_23.Value, 2) : 0,
                                AccruedInterestReportingCurrency = 0
                            });
                        }
                    }
                    /*
                    IEnumerable<POS> mortgageLoans = posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2 && flt.SubCat3_14 == ((int)PositionClassifications.POSIZIONI_INFORMATIVE).ToString() && flt.SubCat4_15 == ((int)PositionClassifications.MUTUI_IPOTECARI_E_CREDITI_DI_COSTRUZIONE).ToString());
                    if (mortgageLoans != null && mortgageLoans.Any())
                    {
                        foreach (var mortgageLoan in mortgageLoans)
                        {
                            tmpDescr = string.IsNullOrEmpty(mortgageLoan.HostPositionReference_6) ? "" : mortgageLoan.HostPositionReference_6.Trim();
                            if (tmpDescr != "") tmpDescr += " ";
                            tmpDescr += string.IsNullOrEmpty(mortgageLoan.Description2_33) ? "" : mortgageLoan.Description2_33.Trim();
                            sectionContent.MortgageLoans.Add(new MortgageLoan()
                            {
                                Account = tmpDescr,
                                Currency = string.IsNullOrEmpty(mortgageLoan.Currency1_17) ? "" : mortgageLoan.Currency1_17,
                                OpeningDay = string.Empty,
                                ExpirationDate = string.Empty,
                                Rate = string.Empty,
                                CurrentBalance = mortgageLoan.Amount1Cur1_22.HasValue ? Math.Round(mortgageLoan.Amount1Cur1_22.Value, 2) : 0,
                                MarketValueReportingCurrency = mortgageLoan.Amount1Base_23.HasValue ? Math.Round(mortgageLoan.Amount1Base_23.Value, 2) : 0,
                                AccruedInterestReportingCurrency = 0
                            });
                        }
                    }
                    */
                    output.Content = new Section150Content(sectionContent);
                }
            }    
            return output;
        }

    }

}
