using System.Globalization;
using PSE.Model.BOSS;
using PSE.Model.Events;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using PSE.Model.SupportTables;
using PSE.Dictionary;
using PSE.BusinessLogic.Common;
using PSE.BusinessLogic.Interfaces;
using static PSE.Model.Common.Constants;
using static PSE.Model.Common.Enumerations;


namespace PSE.BusinessLogic {

    public class ManipulatorSection140 : ManipulatorBase, IManipulator {

        public ManipulatorSection140(CultureInfo? culture = null) : base(ManipolationTypes.AsSection140, culture) { }

        public override IOutputModel Manipulate(IPSEDictionaryService dictionaryService, IList<IInputRecord> extractedData, decimal? totalAssets = null) {            
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section140? output = null;
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE))) {
                string cultureCode;
                ISection140Content sectionContent;
                IFundAccumulationPlan fundAccumulationPlan;
                IFundAccumulationPlanPayment fundAccumulationPlanPayment;
                ExternalCodifiesRequestEventArgs extEventArgsOperation;
                Dictionary<string, object> propertyParams = new Dictionary<string, object>() { { nameof(IDE.Language_18), ITALIAN_LANGUAGE_CODE } };
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                foreach (IDE ideItem in ideItems) {
                    if (ManipulatorOperatingRules.CheckInputLanguage(ideItem.Language_18))
                        propertyParams[nameof(IDE.Language_18)] = ideItem.Language_18;
                    cultureCode = dictionaryService.GetCultureCodeFromLanguage(ideItem.Language_18);
                    // !!!! tmp
                    extEventArgsOperation = new ExternalCodifiesRequestEventArgs(nameof(Section140), "customerId", ideItem.CustomerId_6, propertyParams);
                    //extEventArgsOperation = new ExternalCodifiesRequestEventArgs(nameof(Section140), "customerId", "4139936", propertyParams);
                    OnExternalCodifiesRequest(extEventArgsOperation);
                    if (!extEventArgsOperation.Cancel) {
                        if (extEventArgsOperation.PropertyValues != null && extEventArgsOperation.PropertyValues.Any()) {
                            sectionContent = new Section140Content();
                            foreach (var fundFoundProperties in extEventArgsOperation.PropertyValues.Values) {
                                fundAccumulationPlan = new FundAccumulationPlan();
                                fundAccumulationPlanPayment = new FundAccumulationPlanPayment();    
                                foreach (KeyValuePair<string, object?> fundProperty in fundFoundProperties.Where(f => f.Value != null)) {
                                    switch (fundProperty.Key) {
                                        case nameof(TmpAdordlat.UniImpLat10): {
                                                if (fundProperty.Value != null) {
                                                    fundAccumulationPlan.Description1 = fundProperty.Value.ToString(); // not definitive !!!!
                                                    fundAccumulationPlan.Description2 = "?"; 
                                                }
                                            }
                                            break;
                                        case nameof(TmpAdordlat.DatFinLat): {
                                                if (fundProperty.Value != null && DateTime.TryParse(fundProperty.Value.ToString(), out DateTime tmpDate)) 
                                                    fundAccumulationPlan.ExpirationDate = AssignRequiredDate(tmpDate, _culture);
                                            }
                                            break;
                                        case nameof(TmpAdordlat.OpeStoLat): {
                                                if (fundProperty.Value != null) {
                                                    fundAccumulationPlanPayment.Frequency = fundProperty.Value.ToString(); // need decoding from key to descr !!!!
                                                }
                                            }
                                            break;
                                        case nameof(TmpAdordlat.Cod010Lat): {
                                                if (fundProperty.Value != null) {
                                                    fundAccumulationPlanPayment.Currency = fundProperty.Value.ToString().Trim(); 
                                                }
                                            }
                                            break;
                                        case nameof(TmpAdordlat.ImpImpLat1): {
                                                if (fundProperty.Value != null && decimal.TryParse(fundProperty.Value.ToString(), CultureInfo.InvariantCulture, out decimal tmpDec)) 
                                                    fundAccumulationPlanPayment.Amount = AssignRequiredDecimal(tmpDec); // need decoding from key to descr !!!!
                                            }
                                            break;
                                        case nameof(TmpAdordlat.ValUniLat4): {
                                                if (fundProperty.Value != null && decimal.TryParse(fundProperty.Value.ToString(), CultureInfo.InvariantCulture, out decimal tmpDec))
                                                    fundAccumulationPlan.AveragePurchasePrice = AssignRequiredDecimal(tmpDec);
                                            }
                                            break;
                                        case nameof(TmpAdordlat.ImpImpLat10): {
                                                if (fundProperty.Value != null && decimal.TryParse(fundProperty.Value.ToString(), CultureInfo.InvariantCulture, out decimal tmpDec))
                                                    fundAccumulationPlan.SharesPurchased = AssignRequiredDecimal(tmpDec);
                                            }
                                            break;
                                        case nameof(TmpAdordlat.ImpImpLat4): {
                                                if (fundProperty.Value != null && decimal.TryParse(fundProperty.Value.ToString(), CultureInfo.InvariantCulture, out decimal tmpDec))
                                                    fundAccumulationPlan.MarketValueReportingCurrency = AssignRequiredDecimal(tmpDec);
                                            }
                                            break;
                                    }
                                }
                                fundAccumulationPlan.Payments.Add(fundAccumulationPlanPayment);
                                fundAccumulationPlan.PercentWeigth = CalculatePercentWeight(totalAssets, fundAccumulationPlan.MarketValueReportingCurrency);
                                sectionContent.SubSection14000.Content.Add(fundAccumulationPlan);
                            }
                            output = new() {
                                SectionId = sectionDest.SectionId,
                                SectionCode = sectionDest.SectionCode,
                                SectionName = sectionDest.SectionContent,
                                Content = new Section140Content(sectionContent)
                            };
                        }
                    }
                }
            }
            return output;
        }

    }

}
