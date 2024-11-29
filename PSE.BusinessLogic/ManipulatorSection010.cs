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

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
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
                bool hasValue;
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
                            if (!extEventArgsService.Cancel)
                            {
                                currKeyInf = new KeyInformation()
                                {
                                    Customer = ideItem.CustomerNameShort_5,
                                    CustomerID = ideItem.CustomerNumber_2,
                                    Portfolio = extEventArgsPortfolio.PropertyValue,
                                    EsgProfile = extEventArgsService.PropertyValue,
                                    RiskProfile = "[RiskProfile]", // not still recovered (!)
                                    //PercentWeightedPerformance = perItem.TWR_14 != null ? perItem.TWR_14.Value : null,
                                };
                                output.Content.SubSection1000.Content.Add(new KeyInformation(currKeyInf));
                                if (perItem.StartValue_8 != null && perItem.StartDate_6 != null && perItem.EndDate_7 != null)
                                {
                                    decimal startValue, cashIn, cashOut, secIn, secOut, portfolioValueRectified;
                                    cashIn = cashOut = secIn = secOut = 0;
                                    startValue = perItem.StartValue_8.Value;
                                    output.Content.SubSection1010.Name = $"Managment report {((DateTime)perItem.StartDate_6).ToString(DEFAULT_DATE_FORMAT, _culture)} – {((DateTime)perItem.EndDate_7).ToString(DEFAULT_DATE_FORMAT, _culture)}";
                                    hasValue = false;
                                    cashOut = secOut = 0;
                                    if (perItem.CashOut_11 != null)
                                    {
                                        cashOut = perItem.CashOut_11.Value;
                                        hasValue = true;
                                    }
                                    if (perItem.SecOut_13 != null)
                                    {
                                        secOut = perItem.SecOut_13.Value;
                                        hasValue = true;
                                    }
                                    currAsstExtr = new AssetExtract()
                                    {
                                        Entry = "Portfolio Value " + ((DateTime)perItem.StartDate_6).ToString(DEFAULT_DATE_FORMAT, _culture),
                                        MarketValueReportingCurrency = startValue,
                                        AssetType = "Contributions",
                                        MarketValueReportingCurrencyContr = cashOut + secOut
                                    };
                                    output.Content.SubSection1010.Content.Add(new AssetExtract(currAsstExtr));
                                    if (hasValue)
                                    {
                                        currAsstExtr = new AssetExtract()
                                        {
                                            Entry = "Portfolio Value " + ((DateTime)perItem.StartDate_6).ToString(DEFAULT_DATE_FORMAT, _culture),
                                            MarketValueReportingCurrency = startValue,
                                            AssetType = "Withdrawals"                                            
                                        };
                                        output.Content.SubSection1010.Content.Add(new AssetExtract(currAsstExtr));
                                        if (perItem.CashIn_10 != null)
                                            cashIn = perItem.CashIn_10.Value;
                                        if (perItem.SecIn_12 != null)
                                            secIn = perItem.SecIn_12.Value;
                                        portfolioValueRectified = startValue + (cashIn + secIn + cashOut + secOut);
                                        currAsstExtr = new AssetExtract()
                                        {
                                            Entry = "Portfolio Value Rectified",
                                            MarketValueReportingCurrency = portfolioValueRectified
                                        };
                                        output.Content.SubSection1010.Content.Add(new AssetExtract(currAsstExtr));
                                        if (perItem.EndValue_9 != null && perItem.EndDate_7 != null)
                                        {
                                            currAsstExtr = new AssetExtract()
                                            {
                                                Entry = "Portfolio Value " + ((DateTime)perItem.EndDate_7).ToString(DEFAULT_DATE_FORMAT, _culture),
                                                MarketValueReportingCurrency = perItem.EndValue_9.Value
                                            };
                                            output.Content.SubSection1010.Content.Add(new AssetExtract(currAsstExtr));
                                            currAsstExtr = new AssetExtract()
                                            {
                                                Entry = "Plus/less value",
                                                MarketValueReportingCurrency = perItem.EndValue_9.Value - portfolioValueRectified
                                            };
                                            output.Content.SubSection1010.Content.Add(new AssetExtract(currAsstExtr));
                                        }
                                    }
                                }
                                if (perItem.Interest_15 != null)
                                {
                                    decimal interest, realEquity, realCurr, nonRealEquity, nonRealCurrency;
                                    realEquity = realCurr = nonRealEquity = nonRealCurrency = 0;
                                    interest = perItem.Interest_15.Value;
                                    currDivInt = new DividendInterest()
                                    {
                                        Entry = "Dividend and Interest",
                                        MarketValueReportingCurrencyT = interest,
                                    };
                                    output.Content.SubSection1011.Content.Add(new DividendInterest(currDivInt));
                                    hasValue = false;
                                    if (perItem.PlRealEquity_16 != null)
                                    {
                                        realEquity = perItem.PlRealEquity_16.Value;
                                        hasValue = true;
                                    }
                                    if (perItem.PlRealCurrency_17 != null)
                                    {
                                        realCurr = perItem.PlRealCurrency_17.Value;
                                        hasValue = true;
                                    }
                                    if (hasValue)
                                    {
                                        currDivInt = new DividendInterest()
                                        {
                                            Entry = "Realized gains/losses",
                                            MarketValueReportingCurrencyT = realEquity + realCurr,
                                            AssetType = "of which ongoing",
                                            MarketValueReportingCurrency = realEquity
                                        };
                                        output.Content.SubSection1011.Content.Add(new DividendInterest(currDivInt));
                                        currDivInt = new DividendInterest()
                                        {
                                            Entry = "Realized gains/losses",
                                            MarketValueReportingCurrencyT = realEquity + realCurr,
                                            AssetType = "of which on currency",
                                            MarketValueReportingCurrency = realCurr
                                        };
                                        output.Content.SubSection1011.Content.Add(new DividendInterest(currDivInt));
                                        hasValue = false;
                                        if (perItem.PlNonRealEquity_18 != null)
                                        {
                                            nonRealEquity = perItem.PlNonRealEquity_18.Value;
                                            hasValue = true;
                                        }
                                        if (perItem.PlNonRealCurrency_19 != null)
                                        {
                                            nonRealCurrency = perItem.PlNonRealCurrency_19.Value;
                                            hasValue = true;
                                        }
                                        if (hasValue)
                                        {
                                            currDivInt = new DividendInterest()
                                            {
                                                Entry = "Not realized gains/losses",
                                                MarketValueReportingCurrencyT = nonRealEquity + nonRealCurrency,
                                                AssetType = "of which ongoing",
                                                MarketValueReportingCurrency = nonRealEquity
                                            };
                                            output.Content.SubSection1011.Content.Add(new DividendInterest(currDivInt));
                                            currDivInt = new DividendInterest()
                                            {
                                                Entry = "Not realized gains/losses",
                                                MarketValueReportingCurrencyT = nonRealEquity + nonRealCurrency,
                                                AssetType = "of which on currency",
                                                MarketValueReportingCurrency = nonRealCurrency
                                            };
                                            output.Content.SubSection1011.Content.Add(new DividendInterest(currDivInt));
                                            currDivInt = new DividendInterest()
                                            {
                                                Entry = "Plus/less value",
                                                MarketValueReportingCurrencyT = interest + realEquity + realCurr +
                                                                                nonRealEquity + nonRealCurrency
                                            };
                                            output.Content.SubSection1011.Content.Add(new DividendInterest(currDivInt));
                                        }
                                    }
                                }                                
                            }
                        }
                    }
                }
            }
            return output;
        }

    }

}
