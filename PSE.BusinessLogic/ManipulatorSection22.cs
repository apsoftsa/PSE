using System.Globalization;
using PSE.BusinessLogic.Common;
using PSE.BusinessLogic.Interfaces;
using PSE.Model.Events;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using PSE.Model.SupportTables;
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection22 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection22(CultureInfo? culture = null) : base(PositionClassifications.UNKNOWN, ManipolationTypes.AsSection22, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section22 output = new()
            {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };            
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) && extractedData.Any(flt => flt.RecordType == nameof(POS)))
            {                
                ExternalCodifyRequestEventArgs extEventArgsAdvisor;
                ICountry countryItem;                
                ISection22Content sectionContent;
                string tmpValue;
                decimal totalSum;
                List<ICountry> continentCountries;
                Dictionary<string, List<ICountry>> continentsCountryItems;
                Dictionary<string, string> continentsDescription;
                Dictionary<string, object> propertyParams = new Dictionary<string, object>() { { nameof(IDE.Language_18), Model.Common.Constants.ITALIAN_LANGUAGE_CODE } };
                IEnumerable<IGrouping<string, POS>> groupByCountries;
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> posItems = extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>();
                foreach (IDE ideItem in ideItems)
                {
                    totalSum = 0;
                    if (ManipulatorOperatingRules.CheckInputLanguage(ideItem.Language_18))
                        propertyParams[nameof(IDE.Language_18)] = ideItem.Language_18;
                    sectionContent = new Section22Content();
                    continentsCountryItems = new Dictionary<string, List<ICountry>>();
                    continentsDescription = new Dictionary<string, string>();
                    groupByCountries = posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2 && string.IsNullOrEmpty(flt.Country_20) == false && flt.SubCat4_15 == ((int)PositionClassifications.AZIONI_FONDI_AZIONARI).ToString()).GroupBy(gb => gb.Country_20).OrderBy(ob => ob.Key);
                    foreach (IGrouping<string, POS> groupByCountry in groupByCountries)
                    {
                        tmpValue = string.Empty;
                        extEventArgsAdvisor = new ExternalCodifyRequestEventArgs(nameof(Section22), nameof(Country.CountryName), groupByCountry.Key, propertyParams);
                        OnExternalCodifyRequest(extEventArgsAdvisor);
                        if (extEventArgsAdvisor.Cancel == false && string.IsNullOrEmpty(extEventArgsAdvisor.PropertyValue) == false)
                        {
                            tmpValue = extEventArgsAdvisor.PropertyValue;
                            countryItem = new Country()
                            {
                                CountryName = tmpValue,
                                MarketValueReportingCurrency = Math.Round(groupByCountry.Sum(sum => sum.Amount1Base_23).Value, 2),
                                PercentShares = 0
                            };
                            totalSum += countryItem.MarketValueReportingCurrency.Value;
                            tmpValue = string.Empty;
                            extEventArgsAdvisor = new ExternalCodifyRequestEventArgs(nameof(Section22), "Continent.Code", groupByCountry.Key, propertyParams);
                            OnExternalCodifyRequest(extEventArgsAdvisor);
                            if (extEventArgsAdvisor.Cancel == false && string.IsNullOrEmpty(extEventArgsAdvisor.PropertyValue) == false)
                            {
                                tmpValue = extEventArgsAdvisor.PropertyValue;
                                if (continentsCountryItems.ContainsKey(tmpValue))
                                    continentsCountryItems[tmpValue].Add(countryItem);
                                else
                                    continentsCountryItems.Add(tmpValue, new List<ICountry>() { countryItem });
                                if (!continentsDescription.ContainsKey(tmpValue))
                                {
                                    extEventArgsAdvisor = new ExternalCodifyRequestEventArgs(nameof(Section22), nameof(ChartSharesByContinent.Continent), tmpValue, propertyParams);
                                    OnExternalCodifyRequest(extEventArgsAdvisor);
                                    if (extEventArgsAdvisor.Cancel == false && string.IsNullOrEmpty(extEventArgsAdvisor.PropertyValue) == false)
                                        continentsDescription.Add(tmpValue, extEventArgsAdvisor.PropertyValue);
                                }
                            }
                        }
                    }
                    if (continentsCountryItems.Any())
                    {
                        foreach (KeyValuePair<string, List<ICountry>> continentCountryItems in continentsCountryItems)
                        {
                            foreach (var country in continentCountryItems.Value)
                            {
                                country.PercentShares = country.MarketValueReportingCurrency.HasValue ? Math.Round(country.MarketValueReportingCurrency.Value * 100.0m / totalSum, 2) : 0;
                            }
                        }
                        sectionContent.SharesByNations.Add(new SharesByNation()
                        {
                            Continents = new List<IContinent>(),
                            TotalMarketValue = totalSum,
                            TotalPercentShares = 100.0m
                        });
                        foreach (KeyValuePair<string, List<ICountry>> continentCountryItems in continentsCountryItems)
                        {
                            // manca un livello precedente che tenga solo la descrizione del continente e i relativi country !!!!
                            continentCountries = new List<ICountry>();
                            foreach (var continentCountry in continentCountryItems.Value)
                            {
                                continentCountries.Add(new Country()
                                {
                                    CountryName = continentCountry.CountryName,
                                    MarketValueReportingCurrency = continentCountry.MarketValueReportingCurrency,
                                    PercentShares = continentCountry.PercentShares
                                });
                            }
                            sectionContent.SharesByNations.First().Continents.Add(new Continent()
                            {
                                Countries = new List<ICountry>(continentCountries),
                                MarketValueReportingCurrency = continentCountryItems.Value.Sum(s => s.MarketValueReportingCurrency),
                                PercentShares = continentCountryItems.Value.Sum(s => s.PercentShares),
                                ContinentName = continentsDescription[continentCountryItems.Key]
                            });
                            sectionContent.ChartSharesByContinents.Add(new ChartSharesByContinent() 
                            { 
                                 Continent = continentsDescription[continentCountryItems.Key],
                                 PercentShares = continentCountryItems.Value.Sum(s => s.PercentShares)
                            });
                            if (continentCountryItems.Key == "EU")
                            {
                                foreach(ICountry europeCountry in continentCountryItems.Value)
                                    sectionContent.ChartSharesbyCountriesEuropa.Add(new ChartSharesByCountry() { Country = europeCountry.CountryName, PercentShares = europeCountry.PercentShares });
                            }
                            if (continentCountryItems.Key == "AM")
                            {
                                foreach (ICountry americaCountry in continentCountryItems.Value)
                                    sectionContent.ChartSharesbyCountriesAmerica.Add(new ChartSharesByCountry() { Country = americaCountry.CountryName, PercentShares = americaCountry.PercentShares });
                            }
                            if (continentCountryItems.Key == "AS")
                            {
                                foreach (ICountry asiaCountry in continentCountryItems.Value)
                                    sectionContent.ChartSharesbyCountriesAsia.Add(new ChartSharesByCountry() { Country = asiaCountry.CountryName, PercentShares = asiaCountry.PercentShares });
                            }
                        }
                    }                
                    output.Content = new Section22Content(sectionContent);
                }
            }    
            return output;
        }

    }

}
