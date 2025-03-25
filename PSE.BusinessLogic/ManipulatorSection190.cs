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

    public class ManipulatorSection190 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection190(CultureInfo? culture = null) : base(ManipolationTypes.AsSection190, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData, decimal? totalAssets = null)
        {            
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section190 output = new()
            {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) && extractedData.Any(flt => flt.RecordType == nameof(POS)))
            {
                ISection190Content sectionContent;
                IReportsTransferredToAdministration acctAndDepReportTrans;
                IReportsNotTransferredToAdministration acctAndDepReportNotTrans;
                ExternalCodifyRequestEventArgs extEventArgsDescription;
                Dictionary<string, object> propertyParams = new Dictionary<string, object>() { { nameof(IDE.Language_18), ITALIAN_LANGUAGE_CODE } };
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> posItems = extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>();
                foreach (IDE ideItem in ideItems)
                {
                    sectionContent = new Section190Content();                    
                    IEnumerable<IGrouping<string, POS>> relationshipesNonTransferedToAdmin = posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2 && flt.PortfolioNumber_4 != "00001").GroupBy(gb => gb.HostPositionReference_6);
                    if (relationshipesNonTransferedToAdmin != null && relationshipesNonTransferedToAdmin.Any())
                    {
                        sectionContent.SubSection19000 = new SubSection19000("Reports not transferred to administration");
                        acctAndDepReportNotTrans = new ReportsNotTransferredToAdministration();                        
                        foreach (IGrouping<string, POS> relationshipNonTransferedToAdminItem in relationshipesNonTransferedToAdmin)
                        {
                            extEventArgsDescription = new ExternalCodifyRequestEventArgs(nameof(Section190), nameof(ObjectReportsNotTransferredToAdministration.Description), relationshipNonTransferedToAdminItem.First().HostPositionType_5, propertyParams);
                            OnExternalCodifyRequest(extEventArgsDescription);
                            if (!extEventArgsDescription.Cancel)
                            {
                                acctAndDepReportNotTrans.TotalAsset = ""; // ??
                                acctAndDepReportNotTrans.TotalAssetsNotTransferred = ""; // ??
                                acctAndDepReportNotTrans.TotalMarketValueReportingCurrency = 0; // ??
                                acctAndDepReportNotTrans.TotalNotTransferredMarketValueReportingCurrency = 0; // ??
                                acctAndDepReportNotTrans.Objects.Add(new ObjectReportsNotTransferredToAdministration()
                                {
                                    Object = AssignRequiredString(relationshipNonTransferedToAdminItem.First().HostPositionReference_6),
                                    Description = extEventArgsDescription.PropertyValue,                                    
                                    AddressBook = relationshipNonTransferedToAdminItem.First().HostPositionType_5 != "50" ? AssignRequiredString(relationshipNonTransferedToAdminItem.First().Description2_33) : "",
                                    Currency = AssignRequiredString(relationshipNonTransferedToAdminItem.First().HostPositionCurrency_8),
                                    CurrentBalance = AssignRequiredDecimal(relationshipNonTransferedToAdminItem.Where(f => f.Amount1ProRataHostCur_27.HasValue).Sum(s => s.Amount1ProRataHostCur_27.Value)),
                                    MarketValueReportingCurrency = AssignRequiredDecimal(relationshipNonTransferedToAdminItem.Where(f => f.Amount1Base_23.HasValue).Sum(s => s.Amount1Base_23.Value) + relationshipNonTransferedToAdminItem.Where(f => f.ProRataBase_56.HasValue).Sum(s => s.ProRataBase_56.Value))
                                    //TotalAddressBook = sectionContent.RelationshipNonTransferedToAdmin.Last().Description;
                                    //TotalMarketValueReportingCurrency = sectionContent.RelationshipNonTransferedToAdmin.Sum(sum => sum.MarketValueReportingCurrency);
                                });                                
                            }
                        }
                        sectionContent.SubSection19000.Content.Add(acctAndDepReportNotTrans);                        
                        //sectionContent.TotalAddressBook = sectionContent.RelationshipNonTransferedToAdmin.Last().Description;
                        //sectionContent.TotalMarketValueReportingCurrency = sectionContent.RelationshipNonTransferedToAdmin.Sum(sum => sum.MarketValueReportingCurrency);
                    }
                    IEnumerable<IGrouping<string, POS>> relationshipesTransferedToAdmin = posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2 && flt.PortfolioNumber_4 == "00001").GroupBy(gb => gb.HostPositionReference_6);
                    if (relationshipesTransferedToAdmin != null && relationshipesTransferedToAdmin.Any())
                    {
                        sectionContent.SubSection19010 = new SubSection19010("Reports transferred to administration");
                        acctAndDepReportTrans = new ReportsTransferredToAdministration();                        
                        foreach (IGrouping<string, POS> relationshipTransferedToAdminItem in relationshipesTransferedToAdmin)
                        {
                            extEventArgsDescription = new ExternalCodifyRequestEventArgs(nameof(Section190), nameof(ObjectReportsTransferredToAdministration.Description), relationshipTransferedToAdminItem.First().HostPositionType_5, propertyParams);
                            OnExternalCodifyRequest(extEventArgsDescription);
                            if (!extEventArgsDescription.Cancel)
                            {
                                acctAndDepReportTrans.TotalAsset = ""; // ??
                                acctAndDepReportTrans.TotalAssetsNotTransferred = ""; // ??
                                acctAndDepReportTrans.TotalMarketValueReportingCurrency = 0; // ??
                                acctAndDepReportTrans.TotalNotTransferredMarketValueReportingCurrency = 0; // ??
                                acctAndDepReportTrans.Objects.Add(new ObjectReportsTransferredToAdministration()
                                {
                                    Object = AssignRequiredString(relationshipTransferedToAdminItem.First().HostPositionReference_6),
                                    Description = extEventArgsDescription.PropertyValue,                                    
                                    AddressBook = relationshipTransferedToAdminItem.First().HostPositionType_5 != "50" ? AssignRequiredString(relationshipTransferedToAdminItem.First().Description2_33) : "",
                                    Currency = AssignRequiredString(relationshipTransferedToAdminItem.First().HostPositionCurrency_8),
                                    CurrentBalance = AssignRequiredDecimal(relationshipTransferedToAdminItem.Where(f => f.Amount1ProRataHostCur_27.HasValue).Sum(s => s.Amount1ProRataHostCur_27.Value)),
                                    MarketValueReportingCurrency = AssignRequiredDecimal(relationshipTransferedToAdminItem.Where(f => f.Amount1Base_23.HasValue).Sum(s => s.Amount1Base_23.Value) + relationshipTransferedToAdminItem.Where(f => f.ProRataBase_56.HasValue).Sum(s => s.ProRataBase_56.Value))
                                });
                            }
                        }
                        sectionContent.SubSection19010.Content.Add(acctAndDepReportTrans);
                    }
                    output.Content = new Section190Content(sectionContent);                    
                }
            }
            return output;
        }

    }

}
