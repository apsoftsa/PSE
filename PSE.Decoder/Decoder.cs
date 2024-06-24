using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PSE.Decoder.Database;
using PSE.Model.Events;
using PSE.Model.Common;
using PSE.Model.Input.Models;
using PSE.Model.Output.Models;
using PSE.Model.Output.Interfaces;

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
                ServiceCollection _serviceCollection = new ServiceCollection();
                _serviceCollection.AddDbContext<BOSSDbContext>(_options =>
                {
                    _options.UseSqlServer(appSettings.DecoderStringConnection);                       
                    _options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });
                _serviceProvider = _serviceCollection.BuildServiceProvider();
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
                    BOSSDbContext? _context = _serviceProvider.GetService<BOSSDbContext>();
                    if (_context != null)
                    {                        
                        if (e.SectionName == nameof(Section1) && e.PropertyName == nameof(AssetStatement.Advisor))
                        {
                            if (_context.AdaAuIde.Any(flt => flt.AuiNumPer == e.PropertyKey))
                                e.PropertyValue = _context.AdaAuIde.First(flt => flt.AuiNumPer == e.PropertyKey).AuiNome;
                        }
                        else if (e.SectionName == nameof(Section3) && e.PropertyName == nameof(KeyInformation.Portfolio))
                        {
                            if (e.PropertyKey.Length > 0 && _context.Tabelle.Any(flt => flt.Tab == "N121" && flt.Code == e.PropertyKey.Substring(0, 1)))
                            {
                                string? languageToCheck = Constants.ITALIAN_LANGUAGE_CODE;
                                if (e.PropertyParams != null && e.PropertyParams.ContainsKey(nameof(IDE.Language_18))
                                    && e.PropertyParams[nameof(IDE.Language_18)] != null)
                                    languageToCheck = e.PropertyParams[nameof(IDE.Language_18)].ToString();
                                e.PropertyValue = languageToCheck switch
                                {
                                    Constants.ENGLISH_LANGUAGE_CODE => _context.Tabelle.First(flt => flt.Tab == "N121" && flt.Code == e.PropertyKey.Substring(0, 1)).TextE,
                                    Constants.GERMAN_LANGUAGE_CODE => _context.Tabelle.First(flt => flt.Tab == "N121" && flt.Code == e.PropertyKey.Substring(0, 1)).TextT,
                                    Constants.FRENCH_LANGUAGE_CODE => _context.Tabelle.First(flt => flt.Tab == "N121" && flt.Code == e.PropertyKey.Substring(0, 1)).TextF,
                                    _ => _context.Tabelle.First(flt => flt.Tab == "N121" && flt.Code == e.PropertyKey.Substring(0, 1)).TextI,
                                };
                                e.PropertyValue = e.PropertyKey[1..] + " - " + e.PropertyValue;
                            }
                        }
                        else if (e.SectionName == nameof(Section3) && e.PropertyName == nameof(KeyInformation.Service))
                        {
                            if (_context.Tabelle.Any(flt => flt.Tab == "L066" && flt.Code == e.PropertyKey))
                            {
                                string? languageToCheck = Constants.ITALIAN_LANGUAGE_CODE;
                                if (e.PropertyParams != null && e.PropertyParams.ContainsKey(nameof(IDE.Language_18))
                                    && e.PropertyParams[nameof(IDE.Language_18)] != null)
                                    languageToCheck = e.PropertyParams[nameof(IDE.Language_18)].ToString();
                                e.PropertyValue = languageToCheck switch
                                {
                                    Constants.ENGLISH_LANGUAGE_CODE => _context.Tabelle.First(flt => flt.Tab == "L066" && flt.Code == e.PropertyKey).TextE,
                                    Constants.GERMAN_LANGUAGE_CODE => _context.Tabelle.First(flt => flt.Tab == "L066" && flt.Code == e.PropertyKey).TextT,
                                    Constants.FRENCH_LANGUAGE_CODE => _context.Tabelle.First(flt => flt.Tab == "L066" && flt.Code == e.PropertyKey).TextF,
                                    _ => _context.Tabelle.First(flt => flt.Tab == "L066" && flt.Code == e.PropertyKey).TextI,
                                };
                            }
                        }
                        else if (e.SectionName == nameof(Section6) && (e.PropertyName == nameof(Asset.AssetClass) || e.PropertyName == nameof(Asset.TypeInvestment)))
                        {
                            if (_context.Tabelle.Any(flt => flt.Tab == "E185" && flt.Code == e.PropertyKey))
                            {
                                string? languageToCheck = Constants.ITALIAN_LANGUAGE_CODE;
                                if (e.PropertyParams != null && e.PropertyParams.ContainsKey(nameof(IDE.Language_18))
                                    && e.PropertyParams[nameof(IDE.Language_18)] != null)
                                    languageToCheck = e.PropertyParams[nameof(IDE.Language_18)].ToString();
                                e.PropertyValue = languageToCheck switch
                                {
                                    Constants.ENGLISH_LANGUAGE_CODE => _context.Tabelle.First(flt => flt.Tab == "E185" && flt.Code == e.PropertyKey).TextE,
                                    Constants.GERMAN_LANGUAGE_CODE => _context.Tabelle.First(flt => flt.Tab == "E185" && flt.Code == e.PropertyKey).TextT,
                                    Constants.FRENCH_LANGUAGE_CODE => _context.Tabelle.First(flt => flt.Tab == "E185" && flt.Code == e.PropertyKey).TextF,
                                    _ => _context.Tabelle.First(flt => flt.Tab == "E185" && flt.Code == e.PropertyKey).TextI,
                                };
                            }
                        }
                        else if (e.SectionName == nameof(Section23) && e.PropertyName == nameof(EconSector.Sector))
                        {
                            if (e.PropertyKey.Length > 0 && _context.Tabelle.Any(flt => flt.Tab == "T003" && flt.Code == e.PropertyKey))
                            {
                                string? languageToCheck = Constants.ITALIAN_LANGUAGE_CODE;
                                if (e.PropertyParams != null && e.PropertyParams.ContainsKey(nameof(IDE.Language_18))
                                    && e.PropertyParams[nameof(IDE.Language_18)] != null)
                                    languageToCheck = e.PropertyParams[nameof(IDE.Language_18)].ToString();
                                e.PropertyValue = languageToCheck switch
                                {
                                    Constants.ENGLISH_LANGUAGE_CODE => _context.Tabelle.First(flt => flt.Tab == "T003" && flt.Code == e.PropertyKey).TextE,
                                    Constants.GERMAN_LANGUAGE_CODE => _context.Tabelle.First(flt => flt.Tab == "T003" && flt.Code == e.PropertyKey).TextT,
                                    Constants.FRENCH_LANGUAGE_CODE => _context.Tabelle.First(flt => flt.Tab == "T003" && flt.Code == e.PropertyKey).TextF,
                                    _ => _context.Tabelle.First(flt => flt.Tab == "T003" && flt.Code == e.PropertyKey).TextI,
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
