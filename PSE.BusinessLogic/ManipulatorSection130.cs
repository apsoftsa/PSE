using System.Globalization;
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

namespace PSE.BusinessLogic
{

    public class ManipulatorSection130 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection130(CultureInfo? culture = null) : base(ManipolationTypes.AsSection130, culture) { }

        public override IOutputModel Manipulate(IPSEDictionaryService dictionaryService, IList<IInputRecord> extractedData, decimal? totalAssets = null)
        {            
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section130 output = new()
            {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) && extractedData.Any(flt => flt.RecordType == nameof(ORD)))
            {
                string cultureCode;
                ISection130Content sectionContent;
                IStockOrder stockOrder;
                ExternalCodifyRequestEventArgs extEventArgsOperation;
                Dictionary<string, object> propertyParams = new Dictionary<string, object>() { { nameof(IDE.Language_18), ITALIAN_LANGUAGE_CODE } };
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<ORD> ordItems = extractedData.Where(flt => flt.RecordType == nameof(ORD)).OfType<ORD>();
                foreach (IDE ideItem in ideItems)
                {
                    if (ordItems != null && ordItems.Any(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2))
                    {
                        if (ManipulatorOperatingRules.CheckInputLanguage(ideItem.Language_18))
                            propertyParams[nameof(IDE.Language_18)] = ideItem.Language_18;
                        cultureCode = dictionaryService.GetCultureCodeFromLanguage(ideItem.Language_18);
                        sectionContent = new Section130Content();
                        foreach (ORD ordItem in ordItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2))
                        {
                            extEventArgsOperation = new ExternalCodifyRequestEventArgs(nameof(Section130), nameof(StockOrder.Operation), ordItem.Direction_10, propertyParams);
                            OnExternalCodifyRequest(extEventArgsOperation);
                            if (!extEventArgsOperation.Cancel)
                            {

                                stockOrder = new StockOrder()
                                {
                                    Order = decimal.TryParse(ordItem.Reference_8, out decimal order) ? order : 0,
                                    Operation = extEventArgsOperation.PropertyValue,
                                    ExpirationDate = ordItem.Limit_Date_End_11 != null ? ordItem.Limit_Date_End_11.Value.ToString(DEFAULT_DATE_FORMAT, _culture) : "",
                                    Amount = ordItem.Quantity_13.HasValue ? ordItem.Quantity_13.Value :  0,
                                    OrderValue = ordItem.Sec_Num_14.HasValue ? ordItem.Sec_Num_14.Value : 0,
                                    Description = ordItem.Sec_Description_15,
                                    Currency = ordItem.Currency_16,
                                    Price = ordItem.Quote_17.HasValue ? ordItem.Quote_17.Value : 0,
                                    LimitStopLoss = ordItem.Limit_Price_18.HasValue ? ordItem.Limit_Price_18.Value : 0
                                };
                                sectionContent.SubSection13000.Content.Add(stockOrder);
                            }
                        }
                        output.Content = new Section130Content(sectionContent);
                    }
                }
            }
            return output;
        }

    }

}
