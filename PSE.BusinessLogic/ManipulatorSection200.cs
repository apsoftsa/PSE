using System.Globalization;
using PSE.BusinessLogic.Common;
using PSE.BusinessLogic.Interfaces;
using PSE.Model.Common;
using PSE.Model.Events;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using PSE.Model.SupportTables;
using static PSE.Model.Common.Constants;

namespace PSE.BusinessLogic {

    public class ManipulatorSection200 : ManipulatorBase, IManipulator {

        public ManipulatorSection200(CultureInfo? culture = null) : base(Enumerations.ManipolationTypes.AsSection200, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData, decimal? totalAssets = null) {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section200 output = new() {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) && extractedData.Any(flt => flt.RecordType == nameof(PER))) {
                IEndExtractCustomer endExtractCustomer;
                ExternalCodifyRequestEventArgs extEventArgsPortfolio;
                Dictionary<string, object> propertyParams = new Dictionary<string, object>() { { nameof(IDE.Language_18), ITALIAN_LANGUAGE_CODE } };
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                List<PER> perItems = extractedData.Where(flt => flt.RecordType == nameof(PER)).OfType<PER>().ToList();
                foreach (IDE ideItem in ideItems) {
                    if (ManipulatorOperatingRules.CheckInputLanguage(ideItem.Language_18))
                        propertyParams[nameof(IDE.Language_18)] = ideItem.Language_18;
                    if (perItems.Any(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2)) {
                        PER perItem = perItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2).OrderBy(ob => ob.Type_5).First();
                        extEventArgsPortfolio = new ExternalCodifyRequestEventArgs(nameof(Section010), nameof(KeyInformation.Portfolio), ideItem.ModelCode_21, propertyParams);
                        OnExternalCodifyRequest(extEventArgsPortfolio);
                        if (!extEventArgsPortfolio.Cancel) {
                            endExtractCustomer = new EndExtractCustomer() {
                                CustomerID = AssignRequiredString(ideItem.CustomerNumber_2),
                                Customer = AssignRequiredString(ideItem.CustomerNameShort_5),
                                Portfolio = AssignRequiredString(extEventArgsPortfolio.PropertyValue),
                                EsgProfile = "", // ??                                
                                RiskProfile = "" // ??
                            };
                            output.Content.SubSection20000 = new SubSection20000("");
                            output.Content.SubSection20000.Content.Add(new EndExtractCustomer(endExtractCustomer));
                        }
                    }
                }
            }
            return output;
        }

    }

}
