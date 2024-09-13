using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PSE.Decoder.Database;
using PSE.Model.Events;
using PSE.Model.Common;
using PSE.Model.Input.Models;
using PSE.Model.Output.Models;

namespace PSE.Decoder
{

    public class Decoder : IDisposable, IDecoder
    {

        private const string NOT_FOUND = "[NOT_FOUND]";

        private readonly ServiceProvider? _serviceProvider;        

        public event ExternalCodifyErrorOccurredEventHandler? ExternalCodifyErrorOccurred;

        public Decoder(AppSettings appSettings)
        {
            _serviceProvider = null;
            if (appSettings.DecoderEnabled)
            {
                ServiceCollection serviceCollection = new ServiceCollection();
                serviceCollection.AddDbContext<BOSSDbContext>(options =>
                {
                    options.UseSqlServer(appSettings.DecoderStringConnection);                       
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });
                _serviceProvider = serviceCollection.BuildServiceProvider();
            }
        }

        public virtual bool Decode(ExternalCodifyRequestEventArgs e)
        {
            bool decoded = false;
            try
            {
                if (_serviceProvider != null && e != null && string.IsNullOrEmpty(e.PropertyKey) == false)
                {
                    e.PropertyValue = NOT_FOUND;
                    BOSSDbContext? context = _serviceProvider.GetService<BOSSDbContext>();
                    if (context != null)
                    {                        
                        if (e.SectionName == nameof(Section000) && e.PropertyName == nameof(AssetStatement.Advisory))
                        {
                            if (context.AdaAuIde.Any(flt => flt.AuiNumPer == e.PropertyKey))
                                e.PropertyValue = context.AdaAuIde.First(flt => flt.AuiNumPer == e.PropertyKey).AuiNome;
                        }
                        else if ((e.SectionName == nameof(Section010) || e.SectionName == nameof(Section26)) && e.PropertyName == nameof(KeyInformation.Portfolio))
                        {
                            if (e.PropertyKey.Length > 0 && context.Tabelle.AsNoTracking().Any(flt => flt.Tab == "N121" && flt.Code == e.PropertyKey.Substring(0, 1)))
                            {
                                string? languageToCheck = Constants.ITALIAN_LANGUAGE_CODE;
                                if (e.PropertyParams != null && e.PropertyParams.ContainsKey(nameof(IDE.Language_18))
                                    && e.PropertyParams[nameof(IDE.Language_18)] != null)
                                    languageToCheck = e.PropertyParams[nameof(IDE.Language_18)].ToString();
                                e.PropertyValue = languageToCheck switch
                                {
                                    Constants.ENGLISH_LANGUAGE_CODE => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "N121" && flt.Code == e.PropertyKey.Substring(0, 1)).TextE,
                                    Constants.GERMAN_LANGUAGE_CODE => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "N121" && flt.Code == e.PropertyKey.Substring(0, 1)).TextT,
                                    Constants.FRENCH_LANGUAGE_CODE => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "N121" && flt.Code == e.PropertyKey.Substring(0, 1)).TextF,
                                    _ => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "N121" && flt.Code == e.PropertyKey.Substring(0, 1)).TextI,
                                };
                                e.PropertyValue = e.PropertyKey[1..] + " - " + e.PropertyValue;
                            }
                        }
                        else if ((e.SectionName == nameof(Section010) || e.SectionName == nameof(Section26)) && e.PropertyName == nameof(KeyInformation.EsgProfile))
                        {
                            if (context.Tabelle.AsNoTracking().Any(flt => flt.Tab == "L066" && flt.Code == e.PropertyKey))
                            {
                                string? languageToCheck = Constants.ITALIAN_LANGUAGE_CODE;
                                if (e.PropertyParams != null && e.PropertyParams.ContainsKey(nameof(IDE.Language_18))
                                    && e.PropertyParams[nameof(IDE.Language_18)] != null)
                                    languageToCheck = e.PropertyParams[nameof(IDE.Language_18)].ToString();
                                e.PropertyValue = languageToCheck switch
                                {
                                    Constants.ENGLISH_LANGUAGE_CODE => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "L066" && flt.Code == e.PropertyKey).TextE,
                                    Constants.GERMAN_LANGUAGE_CODE => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "L066" && flt.Code == e.PropertyKey).TextT,
                                    Constants.FRENCH_LANGUAGE_CODE => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "L066" && flt.Code == e.PropertyKey).TextF,
                                    _ => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "L066" && flt.Code == e.PropertyKey).TextI,
                                };
                            }
                        }
                        else if ((e.SectionName == nameof(Section040) || e.SectionName == nameof(Section26)) && (e.PropertyName == nameof(InvestmentAsset.AssetClass) || e.PropertyName == nameof(InvestmentAsset.TypeInvestment)))
                        {
                            if (context.Tabelle.AsNoTracking().Any(flt => flt.Tab == "E185" && flt.Code == e.PropertyKey))
                            {
                                string? languageToCheck = Constants.ITALIAN_LANGUAGE_CODE;
                                if (e.PropertyParams != null && e.PropertyParams.ContainsKey(nameof(IDE.Language_18))
                                    && e.PropertyParams[nameof(IDE.Language_18)] != null)
                                    languageToCheck = e.PropertyParams[nameof(IDE.Language_18)].ToString();
                                e.PropertyValue = languageToCheck switch
                                {
                                    Constants.ENGLISH_LANGUAGE_CODE => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "E185" && flt.Code == e.PropertyKey).TextE,
                                    Constants.GERMAN_LANGUAGE_CODE => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "E185" && flt.Code == e.PropertyKey).TextT,
                                    Constants.FRENCH_LANGUAGE_CODE => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "E185" && flt.Code == e.PropertyKey).TextF,
                                    _ => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "E185" && flt.Code == e.PropertyKey).TextI,
                                };
                            }
                        }
                        else if (e.SectionName == nameof(Section22) && e.PropertyName == nameof(Country.CountryName))
                        {
                            if (e.PropertyKey.Length > 0 && context.Tabelle.AsNoTracking().Any(flt => flt.Tab == "L006" && flt.Col6 == e.PropertyKey))
                            {
                                string? languageToCheck = Constants.ITALIAN_LANGUAGE_CODE;
                                if (e.PropertyParams != null && e.PropertyParams.ContainsKey(nameof(IDE.Language_18))
                                    && e.PropertyParams[nameof(IDE.Language_18)] != null)
                                    languageToCheck = e.PropertyParams[nameof(IDE.Language_18)].ToString();
                                e.PropertyValue = languageToCheck switch
                                {
                                    Constants.ENGLISH_LANGUAGE_CODE => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "L006" && flt.Col6 == e.PropertyKey).TextE,
                                    Constants.GERMAN_LANGUAGE_CODE => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "L006" && flt.Col6 == e.PropertyKey).TextT,
                                    Constants.FRENCH_LANGUAGE_CODE => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "L006" && flt.Col6 == e.PropertyKey).TextF,
                                    _ => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "L006" && flt.Col6 == e.PropertyKey).TextI,
                                };
                            }
                        }
                        else if (e.SectionName == nameof(Section22) && e.PropertyName == "Continent.Code")
                        {
                            if (e.PropertyKey.Length > 0 && context.Tabelle.AsNoTracking().Any(flt => flt.Tab == "L006" && flt.Col6 == e.PropertyKey))
                                e.PropertyValue = context.Tabelle.AsNoTracking().First(flt => flt.Tab == "L006" && flt.Col6 == e.PropertyKey).Col12;
                        }
                        else if (e.SectionName == nameof(Section22) && e.PropertyName == nameof(ChartSharesByContinent.Continent))
                        {
                            if (e.PropertyKey.Length > 0 && context.Tabelle.AsNoTracking().Any(flt => flt.Tab == "BS24" && flt.Code == e.PropertyKey))
                            {
                                string? languageToCheck = Constants.ITALIAN_LANGUAGE_CODE;
                                if (e.PropertyParams != null && e.PropertyParams.ContainsKey(nameof(IDE.Language_18))
                                    && e.PropertyParams[nameof(IDE.Language_18)] != null)
                                    languageToCheck = e.PropertyParams[nameof(IDE.Language_18)].ToString();
                                e.PropertyValue = languageToCheck switch
                                {
                                    Constants.ENGLISH_LANGUAGE_CODE => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "BS24" && flt.Code == e.PropertyKey).TextE,
                                    Constants.GERMAN_LANGUAGE_CODE => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "BS24" && flt.Code == e.PropertyKey).TextT,
                                    Constants.FRENCH_LANGUAGE_CODE => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "BS24" && flt.Code == e.PropertyKey).TextF,
                                    _ => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "BS24" && flt.Code == e.PropertyKey).TextI,
                                };
                            }
                        }
                        else if (e.SectionName == nameof(Section23) && e.PropertyName == nameof(EconSector.Sector))
                        {
                            if (e.PropertyKey.Length > 0 && context.Tabelle.AsNoTracking().Any(flt => flt.Tab == "T003" && flt.Code == e.PropertyKey))
                            {
                                string? languageToCheck = Constants.ITALIAN_LANGUAGE_CODE;
                                if (e.PropertyParams != null && e.PropertyParams.ContainsKey(nameof(IDE.Language_18))
                                    && e.PropertyParams[nameof(IDE.Language_18)] != null)
                                    languageToCheck = e.PropertyParams[nameof(IDE.Language_18)].ToString();
                                e.PropertyValue = languageToCheck switch
                                {
                                    Constants.ENGLISH_LANGUAGE_CODE => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "T003" && flt.Code == e.PropertyKey).TextE,
                                    Constants.GERMAN_LANGUAGE_CODE => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "T003" && flt.Code == e.PropertyKey).TextT,
                                    Constants.FRENCH_LANGUAGE_CODE => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "T003" && flt.Code == e.PropertyKey).TextF,
                                    _ => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "T003" && flt.Code == e.PropertyKey).TextI,
                                };
                            }
                        }
                        else if (e.SectionName == nameof(Section24) && e.PropertyName == nameof(Exchange.Operation))
                        {
                            if (e.PropertyKey.Length > 0 && context.Tabelle.AsNoTracking().Any(flt => flt.Tab == "N047" && string.IsNullOrEmpty(flt.Col7) == false && flt.Col7.Trim() == "X" && flt.Code.Substring(0, 1) == e.PropertyKey))
                            {
                                string? languageToCheck = Constants.ITALIAN_LANGUAGE_CODE;
                                if (e.PropertyParams != null && e.PropertyParams.ContainsKey(nameof(IDE.Language_18))
                                    && e.PropertyParams[nameof(IDE.Language_18)] != null)
                                    languageToCheck = e.PropertyParams[nameof(IDE.Language_18)].ToString();
                                e.PropertyValue = languageToCheck switch
                                {
                                    Constants.ENGLISH_LANGUAGE_CODE => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "N047" && flt.Col7.Trim() == "X" && flt.Code.Substring(0, 1) == e.PropertyKey).TextE,
                                    Constants.GERMAN_LANGUAGE_CODE => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "N047" && flt.Col7.Trim() == "X" && flt.Code.Substring(0, 1) == e.PropertyKey).TextT,
                                    Constants.FRENCH_LANGUAGE_CODE => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "N047" && flt.Col7.Trim() == "X" && flt.Code.Substring(0, 1) == e.PropertyKey).TextF,
                                    _ => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "N047" && flt.Col7.Trim() == "X" && flt.Code.Substring(0, 1) == e.PropertyKey).TextI,
                                };
                            }
                        }
                        else if (e.SectionName == nameof(Section25) && e.PropertyName == nameof(RelationshipToAdmin.Description))
                        {
                            if (e.PropertyKey.Length > 0 && context.Tabelle.AsNoTracking().Any(flt => flt.Tab == "N003" && string.IsNullOrEmpty(flt.Code) == false && flt.Code == e.PropertyKey))
                            {
                                string? languageToCheck = Constants.ITALIAN_LANGUAGE_CODE;
                                if (e.PropertyParams != null && e.PropertyParams.ContainsKey(nameof(IDE.Language_18))
                                    && e.PropertyParams[nameof(IDE.Language_18)] != null)
                                    languageToCheck = e.PropertyParams[nameof(IDE.Language_18)].ToString();
                                e.PropertyValue = languageToCheck switch
                                {
                                    Constants.ENGLISH_LANGUAGE_CODE => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "N003" && flt.Code == e.PropertyKey).TextE,
                                    Constants.GERMAN_LANGUAGE_CODE => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "N003" && flt.Code == e.PropertyKey).TextT,
                                    Constants.FRENCH_LANGUAGE_CODE => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "N003" && flt.Code == e.PropertyKey).TextF,
                                    _ => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "N003" && flt.Code == e.PropertyKey).TextI,
                                };
                            }
                        }
                        if (!(string.IsNullOrEmpty(e.PropertyValue) || string.IsNullOrWhiteSpace(e.PropertyValue) || e.PropertyValue == NOT_FOUND))
                        {
                            e.PropertyValue = e.PropertyValue.Trim();
                            decoded = true;
                        }
                        else if (string.IsNullOrEmpty(e.PropertyValue) == false && e.PropertyValue == NOT_FOUND)
                        {
                            e.ErrorOccurred = "Element having the specified property key has not been found into target db!";
                            e.Cancel = true;
                            this.ExternalCodifyErrorOccurred?.Invoke(this, e);
                        }
                    }
                }
                else if (_serviceProvider == null && e != null && string.IsNullOrEmpty(e.PropertyKey) == false) // feature not activated
                    e.PropertyValue = e.PropertyKey;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return decoded;
        }

        public void Dispose()
        {
            if (_serviceProvider != null)
                _serviceProvider.Dispose();
        }

    }

}
