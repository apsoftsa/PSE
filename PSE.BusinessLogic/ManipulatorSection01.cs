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

namespace PSE.BusinessLogic
{

    public class ManipulatorSection1 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection1(CultureInfo? culture = null) : base(Enumerations.ManipolationTypes.AsSection1, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section1 output = new()
            {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)))
            {
                IDE ideItem = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().First();
                ExternalCodifyRequestEventArgs extEventArgsAdvisor = new ExternalCodifyRequestEventArgs(nameof(Section1), nameof(AssetStatement.Advisor), ideItem.Manager_8);
                OnExternalCodifyRequest(extEventArgsAdvisor);
                if (!extEventArgsAdvisor.Cancel)
                {
                    ISection1Content sectionContent = new Section1Content();
                    IAssetStatement assetStatement = new AssetStatement()
                    {
                        Portfolio = ideItem.CustomerNumber_2,
                        Advisor = extEventArgsAdvisor.PropertyValue,
                        Customer = ideItem.CustomerNameShort_5,
                        Date = ideItem.Date_15 != null ? ideItem.Date_15 : string.Empty
                    };
                    sectionContent.AssetStatements.Add(assetStatement);
                    output.Content = new Section1Content(sectionContent);
                }
            }
            return output;
        }

    }

}
