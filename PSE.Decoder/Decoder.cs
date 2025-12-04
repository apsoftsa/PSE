using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PSE.Model.BOSS;
using PSE.Model.Events;
using PSE.Model.Common;
using PSE.Model.Input.Models;
using PSE.Model.Output.Models;
using PSE.Decoder.Database;

namespace PSE.Decoder
{

    public class Decoder : IDisposable, IDecoder
    {

        private const string NOT_FOUND = "[NOT_FOUND]";

        private readonly ServiceProvider? _serviceProvider;        

        public event ExternalCodifyErrorOccurredEventHandler? ExternalCodifyErrorOccurred;
        public event ExternalCodifiesErrorOccurredEventHandler? ExternalCodifiesErrorOccurred;

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
                        else if ((e.SectionName == nameof(Section010) || e.SectionName == nameof(Section000) | e.SectionName == nameof(Section200)) && e.PropertyName == nameof(KeyInformation.Portfolio))
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
                        else if ((e.SectionName == nameof(Section010) || e.SectionName == nameof(Section200)) && e.PropertyName == nameof(KeyInformation.EsgProfile))
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
                        else if ((e.SectionName == nameof(Section010) || e.SectionName == nameof(Section200)) && e.PropertyName == nameof(KeyInformation.RiskProfile)) {
                            e.PropertyValue = "0";                            
                            if (context.AdaAnagr.AsNoTracking().Any(flt => flt.AnNumIde == e.PropertyKey && string.IsNullOrEmpty(flt.AnRiskPr) == false)) {
                                string tmpRiskProfile = context.AdaAnagr.AsNoTracking().First(flt => flt.AnNumIde == e.PropertyKey).AnRiskPr;
                                if(int.TryParse(tmpRiskProfile, out int riskPrf))
                                    e.PropertyValue = riskPrf.ToString();
                            }
                        }                         
                        else if ((e.SectionName == nameof(Section040) || e.SectionName == nameof(Section200)) && (e.PropertyName == nameof(InvestmentAsset.AssetClass) || e.PropertyName == nameof(InvestmentAsset.TypeInvestment)))
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
                        } else if ((e.SectionName == nameof(Section080) || e.SectionName == nameof(Section110)) && (e.PropertyName == nameof(BondDetail.Coupon) || e.PropertyName == nameof(BondInvestmentDetail.Coupon))) {
                            if (context.Tabelle.AsNoTracking().Any(flt => flt.Tab == "L013" && flt.Code == e.PropertyKey)) {
                                string? languageToCheck = Constants.ITALIAN_LANGUAGE_CODE;
                                if (e.PropertyParams != null && e.PropertyParams.ContainsKey(nameof(IDE.Language_18))
                                    && e.PropertyParams[nameof(IDE.Language_18)] != null)
                                    languageToCheck = e.PropertyParams[nameof(IDE.Language_18)].ToString();
                                e.PropertyValue = languageToCheck switch {
                                    Constants.ENGLISH_LANGUAGE_CODE => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "L013" && flt.Code == e.PropertyKey).TextE,
                                    Constants.GERMAN_LANGUAGE_CODE => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "L013" && flt.Code == e.PropertyKey).TextT,
                                    Constants.FRENCH_LANGUAGE_CODE => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "L013" && flt.Code == e.PropertyKey).TextF,
                                    _ => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "L013" && flt.Code == e.PropertyKey).TextI,
                                };
                                if (!string.IsNullOrEmpty(e.PropertyValue)) 
                                    e.PropertyValue = e.PropertyValue.Trim();
                                else
                                    e.PropertyValue = "";
                            }
                        } else if (e.SectionName == nameof(Section140) && e.PropertyName == nameof(TmpAdordlat.OpeStoLat)) {
                            if (context.Tabelle.AsNoTracking().Any(flt => flt.Tab == "L013" && flt.Code == e.PropertyKey)) {
                                string? languageToCheck = Constants.ITALIAN_LANGUAGE_CODE;
                                if (e.PropertyParams != null && e.PropertyParams.ContainsKey(nameof(IDE.Language_18))
                                    && e.PropertyParams[nameof(IDE.Language_18)] != null)
                                    languageToCheck = e.PropertyParams[nameof(IDE.Language_18)].ToString();
                                e.PropertyValue = languageToCheck switch {
                                    Constants.ENGLISH_LANGUAGE_CODE => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "L013" && flt.Code == e.PropertyKey).Col15,
                                    Constants.GERMAN_LANGUAGE_CODE => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "L013" && flt.Code == e.PropertyKey).Col13,
                                    Constants.FRENCH_LANGUAGE_CODE => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "L013" && flt.Code == e.PropertyKey).Col14,
                                    _ => context.Tabelle.AsNoTracking().First(flt => flt.Tab == "L013" && flt.Code == e.PropertyKey).Col12,
                                };
                                if (!string.IsNullOrEmpty(e.PropertyValue)) 
                                    e.PropertyValue = e.PropertyValue.Trim();
                                else
                                    e.PropertyValue = "";
                            }
                        } else if (e.SectionName == nameof(Section170) && e.PropertyName == nameof(ShareByCountry.Country))
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
                        else if (e.SectionName == nameof(Section170) && e.PropertyName == "Continent.Code")
                        {
                            if (e.PropertyKey.Length > 0 && context.Tabelle.AsNoTracking().Any(flt => flt.Tab == "L006" && flt.Col6 == e.PropertyKey))
                                e.PropertyValue = context.Tabelle.AsNoTracking().First(flt => flt.Tab == "L006" && flt.Col6 == e.PropertyKey).Col12;
                        }
                        else if (e.SectionName == nameof(Section170) && e.PropertyName == nameof(ShareByNationChart.Nation))
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
                        else if (e.SectionName == nameof(Section160) && e.PropertyName == nameof(ShareEconomicSector.Sector))
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
                        else if (e.SectionName == nameof(Section130) && e.PropertyName == nameof(StockOrder.Operation))
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
                        else if (e.SectionName == nameof(Section190) && (e.PropertyName == nameof(ObjectReportsTransferredToAdministration.Description) || e.PropertyName == nameof(ObjectReportsNotTransferredToAdministration.Description)))
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
                            //e.Cancel = true; // ????
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

        public bool Decode(ExternalCodifiesRequestEventArgs e) {
            bool decoded = false;
            try {
                if (_serviceProvider != null && e != null && string.IsNullOrEmpty(e.PropertyKey) == false) {
                    e.PropertyValues = [];
                    BOSSDbContext? context = _serviceProvider.GetService<BOSSDbContext>();
                    if (context != null) {
                        if (e.SectionName == nameof(Section140) && e.PropertyName == "customerId" && string.IsNullOrEmpty(e.PropertyKey) == false) {
                            if (context.AdOrdLat.Any(flt => flt.OrdPagLat!.Trim() == "59" && flt.LegRapLat1!.Trim().Substring(4, 7) == e.PropertyKey)) {
                                var items = from ordLat in context.AdOrdLat.AsNoTracking() 
                                            join adAnaNatVal in context.AdAnaNatVal.AsNoTracking() 
                                            on ordLat.UniImpLat10!.Trim() equals adAnaNatVal.NumValAnt!.Trim() + ("000000" + adAnaNatVal.ComValAnt!.Trim()).Substring(("000000" + adAnaNatVal.ComValAnt!.Trim()).Length - 6)
                                            into adAnaNatValGroup from adAnaNatVal in adAnaNatValGroup.DefaultIfEmpty()    
                                            join adAnaSoc in context.AdAnaSoc.AsNoTracking() 
                                            on adAnaNatVal.NumSocAnt!.Trim() equals adAnaSoc.NumSocSoc!.Trim()
                                            into adAnaSocGroup from adAnaSoc in adAnaSocGroup.DefaultIfEmpty()
                                            where ordLat.OrdPagLat!.Trim() == "59" && ordLat.LegRapLat1!.Trim().Substring(4, 7) == e.PropertyKey
                                            select new { 
                                                ordLat.UniImpLat10,
                                                ordLat.DatFinLat,
                                                ordLat.OpeStoLat,
                                                ordLat.Cod010Lat,
                                                ordLat.ImpImpLat1,
                                                ordLat.Cod030Lat,
                                                ordLat.ValUniLat4,
                                                ordLat.ImpImpLat10,
                                                ordLat.ImpImpLat4,
                                                ordLat.DatEseLat,
                                                adAnaSoc.TesAbbSoc,
                                                adAnaNatVal.TesAbbAnt,
                                                adAnaNatVal.NumTlkAnt,
                                                adAnaNatVal.NrisinAnt                                                
                                            };
                                Dictionary<string, object> rowValues;
                                int i = 0;
                                foreach(var item in items) {
                                    rowValues = [];
                                    rowValues.Add(nameof(TmpAdordlat.UniImpLat10), item.UniImpLat10);
                                    rowValues.Add(nameof(TmpAdordlat.DatFinLat), item.DatFinLat);
                                    rowValues.Add(nameof(TmpAdordlat.OpeStoLat), item.OpeStoLat);
                                    rowValues.Add(nameof(TmpAdordlat.Cod010Lat), item.Cod010Lat);
                                    rowValues.Add(nameof(TmpAdordlat.ImpImpLat1), item.ImpImpLat1);
                                    rowValues.Add(nameof(TmpAdordlat.Cod030Lat), item.Cod030Lat);
                                    rowValues.Add(nameof(TmpAdordlat.ValUniLat4), item.ValUniLat4);
                                    rowValues.Add(nameof(TmpAdordlat.ImpImpLat10), item.ImpImpLat10);
                                    rowValues.Add(nameof(TmpAdordlat.ImpImpLat4), item.ImpImpLat4);
                                    rowValues.Add(nameof(TmpAdordlat.DatEseLat), item.DatEseLat);
                                    rowValues.Add(nameof(TmpAdanasoc.TesAbbSoc), item.TesAbbSoc);
                                    rowValues.Add(nameof(TmpAdanatval.TesAbbAnt), item.TesAbbAnt);
                                    rowValues.Add(nameof(TmpAdanatval.NumTlkAnt), item.NumTlkAnt);
                                    rowValues.Add(nameof(TmpAdanatval.NrisinAnt), item.NrisinAnt);                                    
                                    e.PropertyValues.Add(i, rowValues);
                                    i++;
                                }
                                decoded = true;
                            }
                        }
                    }
                }
            } catch (Exception ex) {
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
