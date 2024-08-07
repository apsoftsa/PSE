using System.Globalization;
using PSE.BusinessLogic.Common;
using PSE.BusinessLogic.Interfaces;
using PSE.Model.Events;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using PSE.Model.SupportTables;
using static PSE.Model.Common.Constants;
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection25 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection25(CultureInfo? culture = null) : base(ManipolationTypes.AsSection25, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {            
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section25 output = new()
            {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) && extractedData.Any(flt => flt.RecordType == nameof(POS)))
            {
                ISection25Content sectionContent;
                IRelationshipToAdmin relationshipToAdmin;
                ExternalCodifyRequestEventArgs extEventArgsDescription;
                Dictionary<string, object> propertyParams = new Dictionary<string, object>() { { nameof(IDE.Language_18), ITALIAN_LANGUAGE_CODE } };
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> posItems = extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>();
                foreach (IDE ideItem in ideItems)
                {
                    sectionContent = new Section25Content();
                    IEnumerable<IGrouping<string, POS>> relationshipesNonTransferedToAdmin = posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2 && flt.PortfolioNumber_4 == "00002").GroupBy(gb => gb.HostPositionReference_6);
                    if (relationshipesNonTransferedToAdmin != null && relationshipesNonTransferedToAdmin.Any())
                    {
                        foreach (IGrouping<string, POS> relationshipNonTransferedToAdminItem in relationshipesNonTransferedToAdmin)
                        {
                            extEventArgsDescription = new ExternalCodifyRequestEventArgs(nameof(Section25), nameof(RelationshipToAdmin.Description), relationshipNonTransferedToAdminItem.First().HostPositionType_5, propertyParams);
                            OnExternalCodifyRequest(extEventArgsDescription);
                            if (!extEventArgsDescription.Cancel)
                            {
                                relationshipToAdmin = new RelationshipToAdmin()
                                {
                                    Object = relationshipNonTransferedToAdminItem.First().HostPositionReference_6,
                                    Description = extEventArgsDescription.PropertyValue,
                                    AddressBook = relationshipNonTransferedToAdminItem.First().Description2_33,
                                    Currency = relationshipNonTransferedToAdminItem.First().HostPositionCurrency_8,
                                    CurrentBalance = relationshipNonTransferedToAdminItem.Sum(s => s.Amount1ProRataHostCur_27),
                                    MarketValueReportingCurrency = relationshipNonTransferedToAdminItem.Sum(s => s.Amount1Base_23) + relationshipNonTransferedToAdminItem.Sum(s => s.ProRataBase_56)
                                };
                                sectionContent.RelationshipNonTransferedToAdmin.Add(relationshipToAdmin);
                            }
                        }
                        sectionContent.TotalAddressBook = sectionContent.RelationshipNonTransferedToAdmin.Last().Description;
                        sectionContent.TotalMarketValueReportingCurrency = sectionContent.RelationshipNonTransferedToAdmin.Sum(sum => sum.MarketValueReportingCurrency);
                    }
                    IEnumerable<IGrouping<string, POS>> relationshipesTransferedToAdmin = posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2 && flt.PortfolioNumber_4 == "00001").GroupBy(gb => gb.HostPositionReference_6);
                    if (relationshipesTransferedToAdmin != null && relationshipesTransferedToAdmin.Any())
                    {
                        foreach (IGrouping<string, POS> relationshipTransferedToAdminItem in relationshipesTransferedToAdmin)
                        {
                            extEventArgsDescription = new ExternalCodifyRequestEventArgs(nameof(Section25), nameof(RelationshipToAdmin.Description), relationshipTransferedToAdminItem.First().HostPositionType_5, propertyParams);
                            OnExternalCodifyRequest(extEventArgsDescription);
                            if (!extEventArgsDescription.Cancel)
                            {
                                relationshipToAdmin = new RelationshipToAdmin()
                                {
                                    Object = relationshipTransferedToAdminItem.First().HostPositionReference_6,
                                    Description = extEventArgsDescription.PropertyValue,
                                    AddressBook = relationshipTransferedToAdminItem.First().Description2_33,
                                    Currency = relationshipTransferedToAdminItem.First().HostPositionCurrency_8,
                                    CurrentBalance = relationshipTransferedToAdminItem.Sum(s => s.Amount1ProRataHostCur_27),
                                    MarketValueReportingCurrency = relationshipTransferedToAdminItem.Sum(s => s.Amount1Base_23) + relationshipTransferedToAdminItem.Sum(s => s.ProRataBase_56)
                                };
                                sectionContent.RelationshipTransferedToAdmin.Add(relationshipToAdmin);
                            }
                        }
                    }
                    output.Content = new Section25Content(sectionContent);                    
                }
            }
            return output;
        }

    }

}
