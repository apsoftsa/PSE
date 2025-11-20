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

    public class ManipulatorSection010 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection010(CultureInfo? culture = null) : base(ManipolationTypes.AsSection010, culture) { }

        public override IOutputModel Manipulate(IPSEDictionaryService dictionaryService, IList<IInputRecord> extractedData, decimal? totalAssets = null)
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
                string cultureCode;
                IKeyInformation currKeyInf;
                ExternalCodifyRequestEventArgs extEventArgsPortfolio;
                ExternalCodifyRequestEventArgs extEventArgsService;
                ExternalCodifyRequestEventArgs extEventArgsRiskProfile;
                Dictionary<string, object> propertyParams = new Dictionary<string, object>() { { nameof(IDE.Language_18), ITALIAN_LANGUAGE_CODE } };
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                List<PER> perItems = extractedData.Where(flt => flt.RecordType == nameof(PER)).OfType<PER>().ToList();
                foreach (IDE ideItem in ideItems)
                {
                    if (ManipulatorOperatingRules.CheckInputLanguage(ideItem.Language_18))
                        propertyParams[nameof(IDE.Language_18)] = ideItem.Language_18;
                    cultureCode = dictionaryService.GetCultureCodeFromLanguage(ideItem.Language_18);
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
                                extEventArgsRiskProfile = new ExternalCodifyRequestEventArgs(nameof(Section010), nameof(KeyInformation.RiskProfile), ideItem.CustomerId_6);
                                OnExternalCodifyRequest(extEventArgsRiskProfile);
                                if (!extEventArgsRiskProfile.Cancel) {
                                    currKeyInf = new KeyInformation() {
                                        CustomerID = AssignRequiredString(ideItem.CustomerId_6),
                                        Customer = AssignRequiredString(ideItem.CustomerNameShort_5),
                                        Portfolio = AssignRequiredString(extEventArgsPortfolio.PropertyValue),
                                        RiskProfile = int.Parse(extEventArgsRiskProfile.PropertyValue),
                                        EsgProfile = extEventArgsService.PropertyValue
                                    };
                                    output.Content.SubSection1000 = new SubSection1000Content();
                                    output.Content.SubSection1000.Content.Add(new KeyInformation(currKeyInf));
                                }
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
                                    portfolioValueRectified = startValue + cashIn + secIn + cashOut + secOut;
                                    output.Content.SubSection1010 = new SubSection1010Content();
                                    output.Content.SubSection1010.Name = $"Management report {startDate} – {endDate}";
                                    output.Content.SubSection1010.ManagementReportFromDate = startDate;
                                    output.Content.SubSection1010.ManagementReportToDate = endDate;
                                    output.Content.SubSection1010.PortfolioValueDate = startDate;
                                    output.Content.SubSection1010.PortfolioValueReportingCurrency = startValue;
                                    output.Content.SubSection1010.ContributionsValueReportingCurrency = AssignRequiredDecimal(cashIn + secIn);
                                    output.Content.SubSection1010.WithdrawalsValueReportingCurrency = AssignRequiredDecimal(cashOut + secOut);
                                    output.Content.SubSection1010.PortfolioValueRectifiedReportingCurrency = AssignRequiredDecimal(portfolioValueRectified);
                                    output.Content.SubSection1010.PortfolioValueDate2 = endDate;
                                    output.Content.SubSection1010.PortfolioValueReportingCurrency2 = AssignRequiredDecimal(endValue);
                                    output.Content.SubSection1010.PluslessValueReportingCurrency = AssignRequiredDecimal(endValue - portfolioValueRectified);
                                    output.Content.SubSection1010.PercentWightedPerformance = AssignRequiredDecimal(perItem.TWR_14);
                                    output.Content.SubSection1010.PatrimonialFluctuation = totalAssets.HasValue && totalAssets.Value != 0 && endValue != 0 ? totalAssets.Value - endValue : 0;
                                }
                                decimal interest, realEquity, realCurr, nonRealEquity, nonRealCurrency;
                                interest = perItem.Interest_15.HasValue ? perItem.Interest_15.Value : 0;
                                realEquity = perItem.PlRealEquity_16.HasValue ? perItem.PlRealEquity_16.Value : 0;
                                realCurr = perItem.PlRealCurrency_17.HasValue ? perItem.PlRealCurrency_17.Value : 0;
                                nonRealEquity = perItem.PlNonRealEquity_18.HasValue ? perItem.PlNonRealEquity_18.Value : 0;
                                nonRealCurrency = perItem.PlNonRealCurrency_19.HasValue ? perItem.PlNonRealCurrency_19.Value : 0;
                                output.Content.SubSection1011 = new SubSection1011Content();
                                output.Content.SubSection1011.Name = "Dividend and interest";
                                output.Content.SubSection1011.DividendAndInterestValueReportingCurrency = AssignRequiredDecimal(interest);
                                output.Content.SubSection1011.RealizedGainLossesValueReportingCurrency = AssignRequiredDecimal(realEquity + realCurr);
                                output.Content.SubSection1011.OngoingRealizedGainLossesValueReportingCurrency = AssignRequiredDecimal(realEquity);
                                output.Content.SubSection1011.OnCurrencyRealizedGainLossesValueReportingCurrency = AssignRequiredDecimal(realCurr);
                                output.Content.SubSection1011.NotRealizedGainLossesValueReportingCurrency = AssignRequiredDecimal(nonRealEquity + nonRealCurrency);
                                output.Content.SubSection1011.OngoingNotRealizedGainLossesValueReportingCurrency = AssignRequiredDecimal(nonRealEquity);
                                output.Content.SubSection1011.OnCurrencyNotRealizedGainLossesValueReportingCurrency = AssignRequiredDecimal(nonRealCurrency);
                                output.Content.SubSection1011.PlusLessValueReportingCurrency = AssignRequiredDecimal(interest + realEquity + realCurr +
                                                                    nonRealEquity + nonRealCurrency);
                            }
                        }
                    }
                }
            }
            return output;
        }

    }

}
