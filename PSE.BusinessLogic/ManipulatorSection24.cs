using System.Globalization;
using PSE.BusinessLogic.Common;
using PSE.BusinessLogic.Interfaces;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using PSE.Model.SupportTables;
using static PSE.Model.Common.Constants;
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection24 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection24(CultureInfo? culture = null) : base(ManipolationTypes.AsSection24, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section24 output = new()
            {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) && extractedData.Any(flt => flt.RecordType == nameof(ORD)))
            {
                ISection24Content sectionContent;
                IExchange exchange;
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<ORD> ordItems = extractedData.Where(flt => flt.RecordType == nameof(ORD)).OfType<ORD>();
                foreach (IDE ideItem in ideItems)
                {
                    if (ordItems != null && ordItems.Any(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2))
                    {
                        sectionContent = new Section24Content();
                        foreach (ORD ordItem in ordItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2))
                        {
                            exchange = new Exchange()
                            {
                                ExchangeOrder = ordItem.Reference_8,
                                Operation = ordItem.Direction_10,
                                ExpirationDate = ordItem.Limit_Date_End_11 != null ? ordItem.Limit_Date_End_11.Value.ToString(DEFAULT_DATE_FORMAT, _culture) : "",
                                Quantity = ordItem.Quantity_13,
                                ExchangeValue = ordItem.Sec_Num_14,
                                Description = ordItem.Sec_Description_15,
                                Currency = ordItem.Currency_16,
                                CourseCost = ordItem.Quote_17,
                                LimitStopLoss = ordItem.Limit_Price_18 != null ? ordItem.Limit_Price_18.Value.ToString() : string.Empty
                            };
                            sectionContent.Exchange.Add(exchange);  
                        }
                        output.Content = new Section24Content(sectionContent);
                    }
                }
            }
            return output;
        }

    }

}
