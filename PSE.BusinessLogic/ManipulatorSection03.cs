﻿using System.Globalization;
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

    public class ManipulatorSection3 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection3(CultureInfo? culture = null) : base(Enumerations.ManipolationTypes.AsSection3, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section3 output = new()
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
                        extEventArgsPortfolio = new ExternalCodifyRequestEventArgs(nameof(Section3), nameof(KeyInformation.Portfolio), ideItem.ModelCode_21, propertyParams);
                        OnExternalCodifyRequest(extEventArgsPortfolio);
                        if (!extEventArgsPortfolio.Cancel)
                        {
                            extEventArgsService = new ExternalCodifyRequestEventArgs(nameof(Section3), nameof(KeyInformation.Service), ideItem.Mandate_11, propertyParams);
                            OnExternalCodifyRequest(extEventArgsService);
                            if (!extEventArgsService.Cancel)
                            {
                                currKeyInf = new KeyInformation()
                                {
                                    CustomerName = ideItem.CustomerNameShort_5,
                                    CustomerNumber = ideItem.CustomerNumber_2,
                                    Portfolio = extEventArgsPortfolio.PropertyValue,
                                    Service = extEventArgsService.PropertyValue,
                                    RiskProfile = "[RiskProfile]", // not still recovered (!)
                                    PercentWeightedPerformance = perItem.TWR_14 != null ? perItem.TWR_14.Value : null,
                                };
                                output.Content.KeysInformation.Add(new KeyInformation(currKeyInf));
                                if (perItem.StartValue_8 != null && perItem.StartDate_6 != null)
                                {
                                    decimal startValue, cashIn, cashOut, secIn, secOut, portfolioValueRectified;
                                    cashIn = cashOut = secIn = secOut = 0;
                                    startValue = perItem.StartValue_8.Value;
                                    currAsstExtr = new AssetExtract()
                                    {
                                        AssetClass = "Portfolio Value " + ((DateTime)perItem.StartDate_6).ToString(DEFAULT_DATE_FORMAT, _culture),
                                        MarketValueReportingCurrencyT = startValue,
                                        AssetType = "+ Contributions"
                                    };
                                    output.Content.AssetsExtract.Add(new AssetExtract(currAsstExtr));
                                    hasValue = false;
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
                                    if (hasValue)
                                    {
                                        currAsstExtr = new AssetExtract()
                                        {
                                            AssetClass = "Portfolio Value " + ((DateTime)perItem.StartDate_6).ToString(DEFAULT_DATE_FORMAT, _culture),
                                            MarketValueReportingCurrencyT = startValue,
                                            AssetType = "- Withdrawals",
                                            MarketValueReportingCurrency = cashOut + secOut
                                        };
                                        output.Content.AssetsExtract.Add(new AssetExtract(currAsstExtr));
                                        if (perItem.CashIn_10 != null)
                                            cashIn = perItem.CashIn_10.Value;
                                        if (perItem.SecIn_12 != null)
                                            secIn = perItem.SecIn_12.Value;
                                        portfolioValueRectified = startValue + (cashIn + secIn + cashOut + secOut);
                                        currAsstExtr = new AssetExtract()
                                        {
                                            AssetClass = "Portfolio Value Rectified",
                                            MarketValueReportingCurrencyT = portfolioValueRectified
                                        };
                                        output.Content.AssetsExtract.Add(new AssetExtract(currAsstExtr));
                                        if (perItem.EndValue_9 != null && perItem.EndDate_7 != null)
                                        {
                                            currAsstExtr = new AssetExtract()
                                            {
                                                AssetClass = "Portfolio Value " + ((DateTime)perItem.EndDate_7).ToString(DEFAULT_DATE_FORMAT, _culture),
                                                MarketValueReportingCurrencyT = perItem.EndValue_9.Value
                                            };
                                            output.Content.AssetsExtract.Add(new AssetExtract(currAsstExtr));
                                            currAsstExtr = new AssetExtract()
                                            {
                                                AssetClass = "Plus/less value",
                                                MarketValueReportingCurrencyT = perItem.EndValue_9.Value - portfolioValueRectified
                                            };
                                            output.Content.AssetsExtract.Add(new AssetExtract(currAsstExtr));

                                        }
                                    }
                                }
                                if (perItem.Interest_15 != null)
                                {
                                    decimal interest, realEquity, realCurr, nonRealEquity, nonRealCurrency;
                                    realEquity = realCurr = nonRealEquity = nonRealCurrency = 0;
                                    interest = perItem.Interest_15.Value;
                                    currAsstExtr = new AssetExtract()
                                    {
                                        AssetClass = "Dividend and Interest",
                                        MarketValueReportingCurrencyT = interest,
                                    };
                                    output.Content.DividendsInterests.Add(new AssetExtract(currAsstExtr));
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
                                        currAsstExtr = new AssetExtract()
                                        {
                                            AssetClass = "Realized gains/losses",
                                            MarketValueReportingCurrencyT = realEquity + realCurr,
                                            AssetType = "on which ongoing",
                                            MarketValueReportingCurrency = realEquity
                                        };
                                        output.Content.DividendsInterests.Add(new AssetExtract(currAsstExtr));
                                        currAsstExtr = new AssetExtract()
                                        {
                                            AssetClass = "Realized gains/losses",
                                            MarketValueReportingCurrencyT = realEquity + realCurr,
                                            AssetType = "on which on currency",
                                            MarketValueReportingCurrency = realCurr
                                        };
                                        output.Content.DividendsInterests.Add(new AssetExtract(currAsstExtr));
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
                                            currAsstExtr = new AssetExtract()
                                            {
                                                AssetClass = "Not realized gains/losses",
                                                MarketValueReportingCurrencyT = nonRealEquity + nonRealCurrency,
                                                AssetType = "on which ongoing",
                                                MarketValueReportingCurrency = nonRealEquity
                                            };
                                            output.Content.DividendsInterests.Add(new AssetExtract(currAsstExtr));
                                            currAsstExtr = new AssetExtract()
                                            {
                                                AssetClass = "Not realized gains/losses",
                                                MarketValueReportingCurrencyT = nonRealEquity + nonRealCurrency,
                                                AssetType = "on which on currency",
                                                MarketValueReportingCurrency = nonRealCurrency
                                            };
                                            output.Content.DividendsInterests.Add(new AssetExtract(currAsstExtr));
                                            currAsstExtr = new AssetExtract()
                                            {
                                                AssetClass = "Plus/less value",
                                                MarketValueReportingCurrencyT = interest + realEquity + realCurr +
                                                                                nonRealEquity + nonRealCurrency
                                            };
                                            output.Content.DividendsInterests.Add(new AssetExtract(currAsstExtr));
                                        }
                                    }
                                }
                                output.Content.FooterInformation.Add(new FooterInformation() { Footer1 = "[footer1]", Footer2 = "[footer2]" }); // not still recovered (!)
                            }
                        }
                    }
                }
            }
            return output;
        }

    }

}
