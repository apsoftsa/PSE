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
                    IEnumerable<POS> relationshipesNonTransferedToAdmin = posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2 && flt.SubCat3_14 == ((int)PositionClassifications.LIQUIDITA).ToString() && flt.SubCat4_15 == ((int)PositionClassifications.CONTI).ToString() && flt.PortfolioNumber_4 == "00002");
                    if (relationshipesNonTransferedToAdmin != null && relationshipesNonTransferedToAdmin.Any())
                    {
                        foreach (POS relationshipNonTransferedToAdminItem in relationshipesNonTransferedToAdmin)
                        {
                            extEventArgsDescription = new ExternalCodifyRequestEventArgs(nameof(Section25), nameof(RelationshipToAdmin.Description), relationshipNonTransferedToAdminItem.HostPositionType_5, propertyParams);
                            OnExternalCodifyRequest(extEventArgsDescription);
                            if (!extEventArgsDescription.Cancel)
                            {
                                relationshipToAdmin = new RelationshipToAdmin()
                                {
                                    Object = relationshipNonTransferedToAdminItem.HostPositionReference_6,
                                    Description = extEventArgsDescription.PropertyValue,
                                    AddressBook = relationshipNonTransferedToAdminItem.Description2_33,
                                    Currency = relationshipNonTransferedToAdminItem.Currency1_17,
                                    CurrentBalance = relationshipNonTransferedToAdminItem.Amount1Cur1_22,
                                    MarketValueReportingCurrency = relationshipNonTransferedToAdminItem.Amount1Base_23
                                };
                                sectionContent.RelationshipNonTransferedToAdmin.Add(relationshipToAdmin);
                            }
                        }
                        sectionContent.TotalAddressBook = relationshipesNonTransferedToAdmin.Last().Description2_33;
                        sectionContent.TotalMarketValueReportingCurrency = relationshipesNonTransferedToAdmin.Where(flt => flt.Amount1Base_23 != null).Sum(sum => sum.Amount1Base_23.Value);
                    }
                    IEnumerable<POS> relationshipesTransferedToAdmin = posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2 && flt.SubCat3_14 == ((int)PositionClassifications.LIQUIDITA).ToString() && flt.SubCat4_15 == ((int)PositionClassifications.CONTI).ToString() && flt.PortfolioNumber_4 == "00001");
                    if (relationshipesTransferedToAdmin != null && relationshipesTransferedToAdmin.Any())
                    {
                        foreach (POS relationshipTransferedToAdminItem in relationshipesTransferedToAdmin)
                        {
                            extEventArgsDescription = new ExternalCodifyRequestEventArgs(nameof(Section25), nameof(RelationshipToAdmin.Description), relationshipTransferedToAdminItem.HostPositionType_5, propertyParams);
                            OnExternalCodifyRequest(extEventArgsDescription);
                            if (!extEventArgsDescription.Cancel)
                            {
                                relationshipToAdmin = new RelationshipToAdmin()
                                {
                                    Object = relationshipTransferedToAdminItem.HostPositionReference_6,
                                    Description = extEventArgsDescription.PropertyValue,
                                    AddressBook = relationshipTransferedToAdminItem.Description2_33,
                                    Currency = relationshipTransferedToAdminItem.Currency1_17,
                                    CurrentBalance = relationshipTransferedToAdminItem.Amount1Cur1_22,
                                    MarketValueReportingCurrency = relationshipTransferedToAdminItem.Amount1Base_23
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
