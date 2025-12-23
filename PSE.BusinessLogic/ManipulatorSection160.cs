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

    public class ManipulatorSection160 : ManipulatorBase, IManipulator
    {

        private readonly bool _applyMaxElementCount;

        public ManipulatorSection160(CultureInfo? culture = null) : base(PositionClassifications.SHARES, ManipolationTypes.AsSection160, culture) { _applyMaxElementCount = true; }

        public override IOutputModel Manipulate(IPSEDictionaryService dictionaryService, IList<IInputRecord> extractedData, decimal? totalAssets = null)
        {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section160 output = new()
            {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };            
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) && extractedData.Any(flt => flt.RecordType == nameof(POS)))
            {                
                ExternalCodifyRequestEventArgs extEventArgsAdvisor;
                IShareEconomicSector econSector;                
                ISection160Content sectionContent;
                string cultureCode;
                string sectorDescr;
                decimal totalSum, totalPerc, currPerc;
                Dictionary<string, object> propertyParams = new Dictionary<string, object>() { { nameof(IDE.Language_18), ITALIAN_LANGUAGE_CODE } };
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> posItems = extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>();                
                foreach (IDE ideItem in ideItems)
                {
                    if (ManipulatorOperatingRules.CheckInputLanguage(ideItem.Language_18))
                        propertyParams[nameof(IDE.Language_18)] = ideItem.Language_18;
                    cultureCode = dictionaryService.GetCultureCodeFromLanguage(ideItem.Language_18);
                    sectionContent = new Section160Content();
                    sectionContent.SubSection16000 = new ShareEconomicSectorSubSection("Shares by economic sector"); 
                    totalSum = totalPerc = currPerc = 0;
                    IEnumerable<IGrouping<string, POS>> groupByEconomicalSector = posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2 && ManipulatorOperatingRules.IsRowDestinatedToManipulator(this, flt.SubCat4_15)).GroupBy(gb => gb.SubCat1_12).OrderBy(ob => ob.Key);
                    if (groupByEconomicalSector.Any()) {
                        foreach (IGrouping<string, POS> economicalSector in groupByEconomicalSector) {
                            if (economicalSector.Sum(sum => sum.Amount1Base_23).Value != 0) {
                                sectorDescr = "(Unknown)";
                                extEventArgsAdvisor = new ExternalCodifyRequestEventArgs(nameof(Section160), nameof(ShareEconomicSector.Sector), economicalSector.Key, propertyParams);
                                OnExternalCodifyRequest(extEventArgsAdvisor);
                                if (!extEventArgsAdvisor.Cancel)
                                    sectorDescr = extEventArgsAdvisor.PropertyValue;
                                econSector = new ShareEconomicSector() {
                                    MarketValueReportingCurrency = Math.Round(economicalSector.Sum(sum => sum.Amount1Base_23).Value, 2),
                                    Sector = string.IsNullOrEmpty(sectorDescr) ? "NON CLASSIFICABILI" : sectorDescr,
                                    Class = CLASS_ENTRY,
                                    PercentShares = 0
                                };
                                totalSum += Math.Abs(econSector.MarketValueReportingCurrency.Value);
                                sectionContent.SubSection16000.Content.Add(econSector);
                            }
                        }
                        totalPerc = currPerc = 0;
                        foreach (var sector in sectionContent.SubSection16000.Content) {
                            if (totalSum > 0) {
                                currPerc = Math.Round(sector.MarketValueReportingCurrency.Value * 100.0m / totalSum, 2);
                                totalPerc += currPerc;
                                sector.PercentShares = currPerc;
                            }
                        }
                        if (_applyMaxElementCount && sectionContent.SubSection16000.Content.Count(f => f.Class != CLASS_TOTAL) > MAX_CHART_ELEMENT_COUNT) {
                            ShareEconomicSector otherEconomicSector = new ShareEconomicSector() { Class = CLASS_GROUP, Sector = "<OTHER>", MarketValueReportingCurrency = 0, PercentShares = 0 };
                            for (int i = 0; i < sectionContent.SubSection16000.Content.Count(f => f.Class != CLASS_TOTAL); i++) {
                                if (i >= MAX_CHART_ELEMENT_COUNT - 1) {
                                    otherEconomicSector.PercentShares += sectionContent.SubSection16000.Content.ElementAt(i).PercentShares;
                                    otherEconomicSector.MarketValueReportingCurrency += sectionContent.SubSection16000.Content.ElementAt(i).MarketValueReportingCurrency;
                                    sectionContent.SubSection16000.Content.ElementAt(i).Class = "<TO_REMOVE>";
                                }
                            }
                            for (int r = sectionContent.SubSection16000.Content.Count - 1; r >= 0; r--) {
                                if (sectionContent.SubSection16000.Content.ElementAt(r).Class == "<TO_REMOVE>")
                                    sectionContent.SubSection16000.Content.RemoveAt(r);
                            }
                            currPerc = otherEconomicSector.PercentShares.Value;
                            sectionContent.SubSection16000.Content.Add(new ShareEconomicSector(otherEconomicSector));
                        }
                    }
                    sectionContent.SubSection16010 = new ShareEconomicSectorChartSubSection("Shares subdivision by economic sector chart");
                    if (groupByEconomicalSector.Any()) {
                        if (totalPerc > 100.0m) {
                            currPerc = currPerc - (totalPerc - 100.0m);
                            sectionContent.SubSection16000.Content.Last().PercentShares = currPerc;
                        } else if (totalPerc < 100.0m) {
                            currPerc = currPerc + (100.0m - totalPerc);
                            sectionContent.SubSection16000.Content.Last().PercentShares = currPerc;
                        }
                        sectionContent.SubSection16000.Content.Add(new ShareEconomicSector() {
                            MarketValueReportingCurrency = totalSum,
                            Sector = dictionaryService.GetTranslation("total_shares_upper", cultureCode),
                            Class = CLASS_TOTAL,
                            PercentShares = 100.0m
                        });
                        if (_applyMaxElementCount && sectionContent.SubSection16000.Content.Count(f => f.Class != CLASS_TOTAL) > MAX_CHART_ELEMENT_COUNT) {
                            int count = sectionContent.SubSection16000.Content.Count(f => f.Class != CLASS_TOTAL);
                            IList<IShareEconomicSector> tmpItems = sectionContent.SubSection16000.Content.Where(f => f.Class != CLASS_TOTAL).OrderByDescending(ob => ob.PercentShares).ToList();
                            for (int i = 0; i < MAX_CHART_ELEMENT_COUNT; i++) {
                                if (i < MAX_CHART_ELEMENT_COUNT - 1)
                                    sectionContent.SubSection16010.Content.Add(new ShareEconomicSectorChart() { Sector = tmpItems[i].Sector, PercentShares = tmpItems[i].PercentShares, Class = tmpItems[i].Class });
                                else {
                                    decimal otherTotal = 0;
                                    for(int o = i; o < tmpItems.Count; o++)
                                        otherTotal += tmpItems[o].PercentShares.HasValue ? tmpItems[o].PercentShares.Value : 0;
                                    sectionContent.SubSection16010.Content.Add(new ShareEconomicSectorChart() { Sector = "<OTHER>", PercentShares = otherTotal, Class = CLASS_ENTRY });
                                }
                            }
                        } else {
                            foreach (var sector in sectionContent.SubSection16000.Content.Where(f => f.Class != CLASS_TOTAL).OrderBy(ob=> ob.Class).ThenByDescending(tb => tb.PercentShares)) {
                                sectionContent.SubSection16010.Content.Add(new ShareEconomicSectorChart() { Sector = sector.Sector, PercentShares = sector.PercentShares, Class = sector.Class });
                            }
                        }
                    }
                    output.Content = new Section160Content(sectionContent);
                }
            }    
            return output;
        }

    }

}
