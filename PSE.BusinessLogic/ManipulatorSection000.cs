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

namespace PSE.BusinessLogic
{

    public class ManipulatorSection000 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection000(CultureInfo? culture = null) : base(Enumerations.ManipolationTypes.AsSection000, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData, decimal? totalAssets = null)
        {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section000 output = new()
            {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)))
            {
                Dictionary<string, object> propertyParams = new Dictionary<string, object>() { { nameof(IDE.Language_18), ITALIAN_LANGUAGE_CODE } };
                IDE ideItem = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().First();
                ExternalCodifyRequestEventArgs extEventArgsPortfolio = new ExternalCodifyRequestEventArgs(nameof(Section000), nameof(KeyInformation.Portfolio), ideItem.ModelCode_21, propertyParams);
                OnExternalCodifyRequest(extEventArgsPortfolio);
                if (!extEventArgsPortfolio.Cancel) {
                    ExternalCodifyRequestEventArgs extEventArgsAdvisor = new ExternalCodifyRequestEventArgs(nameof(Section000), nameof(AssetStatement.Advisory), ideItem.Manager_8);
                    OnExternalCodifyRequest(extEventArgsAdvisor);
                    if (!extEventArgsAdvisor.Cancel) {
                        ISection000Content sectionContent = new Section000Content();
                        IAssetStatement assetStatement = new AssetStatement() {
                            CustomerID = AssignRequiredString(ideItem.CustomerId_6),                            
                            Customer = AssignRequiredString(ideItem.CustomerNameShort_5),
                            Advisory = AssignRequiredString(extEventArgsAdvisor.PropertyValue),
                            Settled = ideItem.Date_15 != null ? new List<ISettled>() { new Settled(ideItem.Date_15.Value.ToString(DEFAULT_DATE_FORMAT, _culture), ideItem.Time_16.Value.ToString(DEFAULT_TIME_FORMAT, _culture)) } : new List<ISettled>() { new Settled() },
                            Portfolio = extEventArgsPortfolio.PropertyValue
                        };
                        sectionContent.AssetStatements.Add(assetStatement);
                        output.Content = new Section000Content(sectionContent);
                    }
                }
            }
            return output;
        }

    }

}
