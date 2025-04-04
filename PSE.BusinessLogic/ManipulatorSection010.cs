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

    public class ManipulatorSection010 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection010(CultureInfo? culture = null) : base(Enumerations.ManipolationTypes.AsSection010, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData, decimal? totalAssets = null)
        {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section010 output = new()
            {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) && extractedData.Any(flt => flt.RecordType == nameof(PER)))
            {
                IKeyInformation currKeyInf;
                IAssetExtract currAsstExtr;
                IDividendInterest currDivInt;
                ExternalCodifyRequestEventArgs extEventArgsPortfolio;
                ExternalCodifyRequestEventArgs extEventArgsService;
                Dictionary<string, object> propertyParams = new Dictionary<string, object>() { { nameof(IDE.Language_18), ITALIAN_LANGUAGE_CODE } };
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                List<PER> perItems = extractedData.Where(flt => flt.RecordType == nameof(PER)).OfType<PER>().ToList();
                foreach (IDE ideItem in ideItems)
                {
                    if (ManipulatorOperatingRules.CheckInputLanguage(ideItem.Language_18))
                        propertyParams[nameof(IDE.Language_18)] = ideItem.Language_18;
                    if (perItems.Any(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2))
                    {
                        // it is necessary to take only the PER item having the property Type_5 value smallest (!!!!)
                        PER perItem = perItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2).OrderBy(ob => ob.Type_5).First();
                        extEventArgsPortfolio = new ExternalCodifyRequestEventArgs(nameof(Section010), nameof(KeyInformation.Portfolio), ideItem.ModelCode_21, propertyParams);
                        OnExternalCodifyRequest(extEventArgsPortfolio);
                        if (!extEventArgsPortfolio.Cancel)
                        {
                            extEventArgsService = new ExternalCodifyRequestEventArgs(nameof(Section010), nameof(KeyInformation.EsgProfile), ideItem.Mandate_11, propertyParams);
                            OnExternalCodifyRequest(extEventArgsService);
                            if (!extEventArgsService.Cancel) {
                                currKeyInf = new KeyInformation() {
                                    CustomerID = AssignRequiredString(ideItem.CustomerId_6),
                                    Customer = AssignRequiredString(ideItem.CustomerNameShort_5),
                                    Portfolio = AssignRequiredString(extEventArgsPortfolio.PropertyValue),
                                    RiskProfile = 0, // ??
                                    EsgProfile = extEventArgsService.PropertyValue
                                };
                                output.Content.SubSection1000 = new SubSection1000Content();
                                output.Content.SubSection1000.Content.Add(new KeyInformation(currKeyInf));
                                if (perItem.StartValue_8 != null && perItem.StartDate_6 != null && perItem.EndDate_7 != null) {
                                    string startDate = AssignRequiredDate(perItem.StartDate_6.Value, _culture);
                                    string endDate = AssignRequiredDate(perItem.EndDate_7.Value, _culture);
                                    decimal startValue, endValue, cashIn, cashOut, secIn, secOut, portfolioValueRectified;
                                    cashIn = cashOut = secIn = secOut = startValue = endValue = 0;
                                    if (perItem.StartValue_8.HasValue)
                                        startValue = perItem.StartValue_8.Value;
                                    if (perItem.EndValue_9.HasValue)
                                        endValue = perItem.EndValue_9.Value;
                                    if (perItem.CashIn_10.HasValue)
                                        cashIn = perItem.CashIn_10.Value;
                                    if (perItem.CashOut_11.HasValue)
                                        cashOut = perItem.CashOut_11.Value;
                                    if (perItem.SecIn_12.HasValue)
                                        secIn = perItem.SecIn_12.Value;
                                    if (perItem.SecOut_13.HasValue)
                                        secOut = perItem.SecOut_13.Value;
                                    output.Content.SubSection1010 = new SubSection1010Content();
                                    output.Content.SubSection1010.Name = $"Managment report {startDate} – {endDate}";
                                    currAsstExtr = new AssetExtract() {
                                        Entry = "Portfolio Value " + startDate,
                                        MarketValueReportingCurrency = startValue,
                                        AssetType = "Contributions",
                                        MarketValueReportingCurrencyContr = AssignRequiredDecimal(cashIn + secIn)
                                    };
                                    output.Content.SubSection1010.Content.Add(new AssetExtract(currAsstExtr));
                                    currAsstExtr = new AssetExtract() {
                                        Entry = "Portfolio Value " + ((DateTime)perItem.StartDate_6).ToString(DEFAULT_DATE_FORMAT, _culture),
                                        MarketValueReportingCurrency = startValue,
                                        AssetType = "Withdrawals",
                                        MarketValueReportingCurrencyContr = AssignRequiredDecimal(cashOut + secOut)
                                    };
                                    output.Content.SubSection1010.Content.Add(new AssetExtract(currAsstExtr));
                                    portfolioValueRectified = startValue + cashIn + secIn + cashOut + secOut;
                                    currAsstExtr = new AssetExtract() {
                                        Entry = "Portfolio Value Rectified",
                                        MarketValueReportingCurrency = AssignRequiredDecimal(portfolioValueRectified)
                                    };
                                    output.Content.SubSection1010.Content.Add(new AssetExtract(currAsstExtr));
                                    currAsstExtr = new AssetExtract() {
                                        Entry = "Portfolio Value " + endDate,
                                        MarketValueReportingCurrency = AssignRequiredDecimal(endValue)
                                    };
                                    output.Content.SubSection1010.Content.Add(new AssetExtract(currAsstExtr));
                                    currAsstExtr = new AssetExtract() {
                                        Entry = "Plus/less value",
                                        MarketValueReportingCurrency = AssignRequiredDecimal(endValue - portfolioValueRectified)
                                    };
                                    output.Content.SubSection1010.Content.Add(new AssetExtract(currAsstExtr));
                                    currAsstExtr = new AssetExtract() {
                                        Entry = "Percent Weighted Performance"
                                    };
                                    output.Content.SubSection1010.Content.Add(new AssetExtract(currAsstExtr));
                                }
                                decimal interest, realEquity, realCurr, nonRealEquity, nonRealCurrency;
                                interest = perItem.Interest_15.HasValue ? perItem.Interest_15.Value : 0;
                                realEquity = perItem.PlRealEquity_16.HasValue ? perItem.PlRealEquity_16.Value : 0;
                                realCurr = perItem.PlRealCurrency_17.HasValue ? perItem.PlRealCurrency_17.Value : 0;
                                nonRealEquity = perItem.PlNonRealEquity_18.HasValue ? perItem.PlNonRealEquity_18.Value : 0;
                                nonRealCurrency = perItem.PlNonRealCurrency_19.HasValue ? perItem.PlNonRealCurrency_19.Value : 0;
                                output.Content.SubSection1011 = new SubSection1011Content();
                                currDivInt = new DividendInterest() {
                                    Entry = "Dividend and Interest",
                                    MarketValueReportingCurrencyT = AssignRequiredDecimal(interest),
                                };
                                output.Content.SubSection1011.Content.Add(new DividendInterest(currDivInt));
                                currDivInt = new DividendInterest() {
                                    Entry = "Realized gains/losses",
                                    MarketValueReportingCurrencyT = AssignRequiredDecimal(realEquity + realCurr),
                                    AssetType = "of which ongoing",
                                    MarketValueReportingCurrency = AssignRequiredDecimal(realEquity)
                                };
                                output.Content.SubSection1011.Content.Add(new DividendInterest(currDivInt));
                                currDivInt = new DividendInterest() {
                                    Entry = "Realized gains/losses",
                                    MarketValueReportingCurrencyT = AssignRequiredDecimal(realEquity + realCurr),
                                    AssetType = "of which on currency",
                                    MarketValueReportingCurrency = AssignRequiredDecimal(realCurr)
                                };
                                output.Content.SubSection1011.Content.Add(new DividendInterest(currDivInt));
                                currDivInt = new DividendInterest() {
                                    Entry = "Not realized gains/losses",
                                    MarketValueReportingCurrencyT = AssignRequiredDecimal(nonRealEquity + nonRealCurrency),
                                    AssetType = "of which ongoing",
                                    MarketValueReportingCurrency = AssignRequiredDecimal(nonRealEquity)
                                };
                                output.Content.SubSection1011.Content.Add(new DividendInterest(currDivInt));
                                currDivInt = new DividendInterest() {
                                    Entry = "Not realized gains/losses",
                                    MarketValueReportingCurrencyT = AssignRequiredDecimal(nonRealEquity + nonRealCurrency),
                                    AssetType = "of which on currency",
                                    MarketValueReportingCurrency = AssignRequiredDecimal(nonRealCurrency)
                                };
                                output.Content.SubSection1011.Content.Add(new DividendInterest(currDivInt));
                                currDivInt = new DividendInterest() {
                                    Entry = "Plus/less value",
                                    MarketValueReportingCurrencyT = AssignRequiredDecimal(interest + realEquity + realCurr +
                                                                    nonRealEquity + nonRealCurrency)
                                };
                                output.Content.SubSection1011.Content.Add(new DividendInterest(currDivInt));
                            }
                        }
                    }
                }
            }
            return output;
        }

    }

}
