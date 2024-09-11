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

    public class ManipulatorSection0 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection0(CultureInfo? culture = null) : base(Enumerations.ManipolationTypes.AsSection0, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section0 output = new()
            {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)))
            {
                IDE ideItem = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().First();
                ExternalCodifyRequestEventArgs extEventArgsAdvisor = new ExternalCodifyRequestEventArgs(nameof(Section0), nameof(AssetStatement.Advisory), ideItem.Manager_8);
                OnExternalCodifyRequest(extEventArgsAdvisor);
                if (!extEventArgsAdvisor.Cancel)
                {
                    ISection0Content sectionContent = new Section0Content();
                    IAssetStatement assetStatement = new AssetStatement()
                    {
                        Portfolio = ideItem.CustomerNumber_2,
                        CustomerID = ideItem.CustomerId_6,
                        Advisory = extEventArgsAdvisor.PropertyValue,
                        Customer = ideItem.CustomerNameShort_5,
                        Settled = ideItem.Date_15 != null ? new List<ISettled>() { new Settled(ideItem.Date_15.Value.ToString(DEFAULT_DATE_FORMAT, _culture), ideItem.Time_16.Value.ToString(DEFAULT_TIME_FORMAT, _culture)) } : null
                    };
                    sectionContent.AssetStatements.Add(assetStatement);
                    output.Content = new Section0Content(sectionContent);
                }
            }
            return output;
        }

    }

}
