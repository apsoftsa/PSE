﻿using System.Globalization;
using PSE.BusinessLogic.Common;
using PSE.BusinessLogic.Interfaces;
using PSE.Model.Events;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using PSE.Model.SupportTables;
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic {

    public class ManipulatorSection170 : ManipulatorBase, IManipulator {

        public ManipulatorSection170(CultureInfo? culture = null) : base(PositionClassifications.UNKNOWN, ManipolationTypes.AsSection170, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData, decimal? totalAssets = null) {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section170 output = new() {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) && extractedData.Any(flt => flt.RecordType == nameof(POS))) {
                ExternalCodifyRequestEventArgs extEventArgsAdvisor;
                IShareByCountry countryItem;
                ISection170Content sectionContent;
                string tmpValue;
                decimal totalSum;
                List<IShareByCountry> continentCountries;
                Dictionary<string, List<IShareByCountry>> continentsCountryItems;
                Dictionary<string, string> continentsDescription;
                Dictionary<string, object> propertyParams = new Dictionary<string, object>() { { nameof(IDE.Language_18), Model.Common.Constants.ITALIAN_LANGUAGE_CODE } };
                IEnumerable<IGrouping<string, POS>> groupByCountries;
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> posItems = extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>();
                foreach (IDE ideItem in ideItems) {
                    totalSum = 0;
                    if (ManipulatorOperatingRules.CheckInputLanguage(ideItem.Language_18))
                        propertyParams[nameof(IDE.Language_18)] = ideItem.Language_18;
                    sectionContent = new Section170Content();
                    sectionContent.SubSection17000 = new SubSection17000("Shares by nation");
                    sectionContent.SubSection17010 = new SubSection17010("Shares by nations chart");
                    continentsCountryItems = new Dictionary<string, List<IShareByCountry>>();
                    continentsDescription = new Dictionary<string, string>();
                    groupByCountries = posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2 && string.IsNullOrEmpty(flt.Country_20) == false && flt.SubCat4_15 == ((int)PositionClassifications.SHARES).ToString()).GroupBy(gb => gb.Country_20).OrderBy(ob => ob.Key);
                    foreach (IGrouping<string, POS> groupByCountry in groupByCountries) {
                        tmpValue = string.Empty;
                        extEventArgsAdvisor = new ExternalCodifyRequestEventArgs(nameof(Section170), nameof(ShareByCountry.Country), groupByCountry.Key, propertyParams);
                        OnExternalCodifyRequest(extEventArgsAdvisor);
                        if (extEventArgsAdvisor.Cancel == false && string.IsNullOrEmpty(extEventArgsAdvisor.PropertyValue) == false) {
                            tmpValue = extEventArgsAdvisor.PropertyValue;
                            countryItem = new ShareByCountry() {
                                Country = tmpValue,
                                MarketValueReportingCurrency = Math.Round(groupByCountry.Sum(sum => sum.Amount1Base_23).Value, 2),
                                PercentShares = 0
                            };
                            totalSum += countryItem.MarketValueReportingCurrency.Value;
                            tmpValue = string.Empty;
                            extEventArgsAdvisor = new ExternalCodifyRequestEventArgs(nameof(Section170), "Continent.Code", groupByCountry.Key, propertyParams);
                            OnExternalCodifyRequest(extEventArgsAdvisor);
                            if (extEventArgsAdvisor.Cancel == false && string.IsNullOrEmpty(extEventArgsAdvisor.PropertyValue) == false) {
                                tmpValue = extEventArgsAdvisor.PropertyValue;
                                if (continentsCountryItems.ContainsKey(tmpValue))
                                    continentsCountryItems[tmpValue].Add(countryItem);
                                else
                                    continentsCountryItems.Add(tmpValue, new List<IShareByCountry>() { countryItem });
                                if (!continentsDescription.ContainsKey(tmpValue)) {
                                    extEventArgsAdvisor = new ExternalCodifyRequestEventArgs(nameof(Section170), nameof(ShareByNationChart.Nation), tmpValue, propertyParams);
                                    OnExternalCodifyRequest(extEventArgsAdvisor);
                                    if (extEventArgsAdvisor.Cancel == false && string.IsNullOrEmpty(extEventArgsAdvisor.PropertyValue) == false)
                                        continentsDescription.Add(tmpValue, extEventArgsAdvisor.PropertyValue);
                                }
                            }
                        }
                    }
                    if (continentsCountryItems.Any()) {
                        foreach (KeyValuePair<string, List<IShareByCountry>> continentCountryItems in continentsCountryItems) {
                            foreach (var country in continentCountryItems.Value) {
                                country.PercentShares = country.MarketValueReportingCurrency.HasValue ? Math.Round(country.MarketValueReportingCurrency.Value * 100.0m / totalSum, 2) : 0;
                            }
                        }
                        foreach (KeyValuePair<string, List<IShareByCountry>> continentCountryItems in continentsCountryItems) {                            
                            continentCountries = new List<IShareByCountry>();
                            foreach (var continentCountry in continentCountryItems.Value) {
                                continentCountries.Add(new ShareByCountry() {
                                    Country = continentCountry.Country,
                                    MarketValueReportingCurrency = continentCountry.MarketValueReportingCurrency,
                                    PercentShares = continentCountry.PercentShares
                                });
                            }
                            sectionContent.SubSection17000.Content.Add(new ShareByNation() { 
                                Nation = continentsDescription[continentCountryItems.Key],
                                Content = new List<IShareByCountry>(continentCountries),
                                MarketValueReportingCurrency = continentCountryItems.Value.Sum(s => s.MarketValueReportingCurrency),
                                PercentShares = continentCountryItems.Value.Sum(s => s.PercentShares),
                            });
                            sectionContent.SubSection17010.Content.Add(new ShareByNationChart() {
                                Nation = continentsDescription[continentCountryItems.Key],
                                PercentShares = continentCountryItems.Value.Sum(s => s.PercentShares)
                            });

                        }
                    }
                    output.Content = new Section170Content(sectionContent);
                }
            }
            return output;
        }

    }

}
