using System.Globalization;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using PSE.Model.SupportTables;
using PSE.Model.Fam;
using PSE.Dictionary;
using PSE.BusinessLogic.Common;
using PSE.BusinessLogic.Interfaces;
using PSE.FamConnector.Multiline;
using static PSE.Model.Common.Constants;
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic {

    public class ManipulatorSection030 : ManipulatorBase, IMultilineManipulator {

        public ManipulatorSection030(CultureInfo? culture = null) : base(ManipolationTypes.AsSection030, culture) { }

        public IOutputModel? Manipulate(IPSEDictionaryService dictionaryService, IMultilineReader multilineReader, IList<IInputRecord> extractedData, decimal? totalAssets = null) {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section030? output = null;
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE))) {
                ISection030Content sectionContent;
                IList<ILineAllocationEvolutionChartModelLine> modelLines;
                string cultureCode;
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                PseReportResult? multilines = Task.Run(() => multilineReader.GetCustomersMultiline(
                    new PseReportRequest(ideItems.Select(sel => sel.CustomerId_6).Distinct().ToList()))).Result;
                //PseReportResult? multilines = Task.Run(() => multilineReader.GetCustomersMultiline(
                //   new PseReportRequest(new List<string>() { "1140041" }))).Result;
                if (multilines != null && multilines.ReportList != null && multilines.ReportList.Count > 0) {
                    output = new() {
                        SectionId = sectionDest.SectionId,
                        SectionCode = sectionDest.SectionCode,
                        SectionName = sectionDest.SectionContent
                    };
                    sectionContent = new Section030Content();
                    foreach (PseReportData multilineItem in multilines.ReportList) {
                        cultureCode = dictionaryService.GetCultureCodeFromLanguage(ideItems.FirstOrDefault(f => f.CustomerId_6 == multilineItem.InfoRelazione.Numide.ToString()).Language_18);
                        //cultureCode = "I";
                        sectionContent.KeyInformation.Add(new MultilineKeyInformation {
                            Currency = multilineItem.InfoRelazione.Moneta,
                            PercentPerformance = AssignRequiredCurrencyDecimal(multilineItem.InfoRelazione.Performance),
                            Period = string.Concat(
                                    AssignRequiredDate(multilineItem.InfoRelazione.StartDate, _culture),
                                    " - ",
                                    AssignRequiredDate(multilineItem.InfoRelazione.EndDate, _culture)
                            ),
                            AssetsTo = [new MultilineAssetsTo() {
                                ValueDate = AssignRequiredDate(multilineItem.InfoRelazione.EndDate, _culture),
                                ValueAsset = AssignRequiredCurrencyDecimal(multilineItem.InfoRelazione.Portafoglio)
                            }]
                        });
                        int elementIdex = 1;
                        Dictionary<Guid, int> keyAndIndex = new Dictionary<Guid, int>();
                        foreach (PseReportDataContributoPerformance contrPerfItem in multilineItem.ContributiPerformance) {                            
                            sectionContent.SubSection3000.Content.Add(new LinePerformanceAnalysis() { 
                                Currency = contrPerfItem.DivisaModello, 
                                ModelLine = contrPerfItem.NomeModello, 
                                PercentNetContribution = AssignRequiredCurrencyDecimal(contrPerfItem.ContribuzioneNetta),
                                Class = CLASS_ENTRY,
                                ElementIndex = elementIdex, 
                            });
                            keyAndIndex.Add(contrPerfItem.Id, elementIdex);
                            elementIdex++;
                        }
                        sectionContent.SubSection3000.Content.Add(new LinePerformanceAnalysis() {
                            Currency = string.Empty,
                            ModelLine = dictionaryService.GetTranslation("total", cultureCode),
                            PercentNetContribution = AssignRequiredCurrencyDecimal(multilineItem.InfoRelazione.Performance),
                            Class = CLASS_TOTAL
                        });
                        foreach (PseReportDataComposizioneMultilinea compMultilineItem in multilineItem.ComposizioniMultilinea) {
                            modelLines = [];
                            foreach (PseReportDataPesoMultilinea compItem in compMultilineItem.Composizione) {
                                modelLines.Add(new LineAllocationEvolutionChartModelLine() { 
                                    ModelLine = multilineItem.ContributiPerformance.FirstOrDefault(f => f.Id == compItem.Id).NomeModello, 
                                    PercentNetContribution = AssignRequiredCurrencyDecimal(compItem.Peso) 
                                });
                                sectionContent.SubSection3020.Content.Add(new LineAllocationEvolutionChartFlat() {
                                    Period = AssignRequiredDate(compMultilineItem.StartDate, _culture),
                                    ModelLine = multilineItem.ContributiPerformance.FirstOrDefault(f => f.Id == compItem.Id).NomeModello,
                                    ElementIndex = keyAndIndex[compItem.Id],  
                                    PercentNetContribution = AssignRequiredCurrencyDecimal(compItem.Peso)
                                });
                            }
                            sectionContent.SubSection3010.Content.Add(new LineAllocationEvolutionChart() {
                                 Period = AssignRequiredDate(compMultilineItem.StartDate, _culture),
                                 ModelLines = [.. modelLines]
                            });                            
                        }  
                    }
                    output.Content = new Section030Content(sectionContent);
                }
            }
            return output;            
        }

        public override IOutputModel Manipulate(IPSEDictionaryService dictionaryService, IList<IInputRecord> extractedData, decimal? totalAssets = null) {
            throw new NotImplementedException();
        }

    }

}
