using PSE.Model.Input.Models;
using PSE.Model.SupportTables;
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic.Utility
{

    public static class ManipulatorOperatingRules
    {

        private static readonly List<InvestmentType> _investmentTypes;
        private static readonly List<PositionClassification> _classificationPositions;
        private static readonly List<ValueTypology> _valueTypes;
        private static readonly List<SectionBinding> _sectionsBinding;

        static ManipulatorOperatingRules()
        {
            _investmentTypes = new List<InvestmentType>()
            {
                new InvestmentType(InvestmentTypes.PRODOTTI_ALTERNATIVI, "90", "PRODOTTI ALTERNATIVI", "90", 780, 7090, 2),
                new InvestmentType(InvestmentTypes.DIVERSI1, "98", "DIVERSI", "98", 780, 1010, 1),
                new InvestmentType(InvestmentTypes.DIVERSI2, "99", "DIVERSI", "99", 780, 7090, 5),
                new InvestmentType(InvestmentTypes.ACCETTAZIONI_BANCARIE, "AB", "ACCETTAZIONI BANCARIE", "CA", 120, 1015, 5),
                new InvestmentType(InvestmentTypes.AZIONI, "AZ", "AZIONI", "MA", 740, 5010, 5),
                new InvestmentType(InvestmentTypes.CERTIFICATI_DI_DEPOSITO, "CD", "CERTIFICATI DI DEPOSITO", "DA", 120, 1015, 4),
                new InvestmentType(InvestmentTypes.FONDI_AZIONARI, "FA", "FONDI AZIONARI", "NA", 740, 5010, 6),
                new InvestmentType(InvestmentTypes.FONDI_IMMOBILIARI, "FI", "FONDI IMMOBILIARI", "VA", 770, 7030, 6),
                new InvestmentType(InvestmentTypes.FONDI_MISTI, "FM", "FONDI MISTI", "PA", 750, 7010, 5),
                new InvestmentType(InvestmentTypes.FONDI_OBBLIGAZIONARI, "FO", "FONDI OBBLIGAZIONARI", "JA", 420, 4020, 4),
                new InvestmentType(InvestmentTypes.FONDI_MONETARI, "FR", "FONDI MONETARI", "AA", 120, 1015, 5),
                new InvestmentType(InvestmentTypes.FUTURES, "FU", "FUTURES", "RA", 760, 7020, 6),
                new InvestmentType(InvestmentTypes.GROI, "GR", "GROI", "OA", 760, 7020, 6),
                new InvestmentType(InvestmentTypes.HEDGE_FUND, "HF", "HEDGE FUND", "FA", 780, 7090, 5),
                new InvestmentType(InvestmentTypes.METALLI, "ME", "METALLI", "XA", 790, 6010, 4),
                new InvestmentType(InvestmentTypes.FONDI_METALLO, "MF", "FONDI METALLO", "YA", 790, 6010, 5),
                new InvestmentType(InvestmentTypes.OBBLIGAZIONI_CON_SCADENZA_MINOR_OR_EQUAL_THAN_1_ANNO, "01", "OBBLIGAZIONI CON SCADENZA <= 1 ANNO", "GA", 140, 4005, 1),
                new InvestmentType(InvestmentTypes.OBBLIGAZIONI_CON_SCADENZA_MINOR_OR_EQUAL_THAN_5_ANNI, "05", "OBBLIGAZIONI CON SCADENZA <= 5 ANNI", "HA", 410, 4010, 1),
                new InvestmentType(InvestmentTypes.OBBLIGAZIONI_CON_SCADENZA_MAJOR_THAN_5_ANNI, "09", "OBBLIGAZIONI CON SCADENZA > 5 ANNI", "IA", 420, 4020, 2),
                new InvestmentType(InvestmentTypes.OBBLIGAZIONI_CONVERTIBILI, "OC", "OBBLIGAZIONI CONVERTIBILI   ", "KA", 430, 4030, 3),
                new InvestmentType(InvestmentTypes.OPZIONI, "OP", "OPZIONI", "QA", 760, 7020, 5),
                new InvestmentType(InvestmentTypes.OBBLIGAZIONI_CON_WARRANTS, "OW", "OBBLIGAZIONI CON WARRANTS", "LA", 430, 4030, 4),
                new InvestmentType(InvestmentTypes.PARTECIPAZIONI_IMMOBILIARI, "PI", "PARTECIPAZIONI IMMOBILIARI", "UA", 770, 7030, 5),
                new InvestmentType(InvestmentTypes.PROMISSORY_NOTES, "PN", "PROMISSORY NOTES", "EA", 120, 1015, 5),
                new InvestmentType(InvestmentTypes.TREASURY_BILLS, "TB", "TREASURY BILLS", "BA", 120, 1015, 5),
                new InvestmentType(InvestmentTypes.WARRANTS, "WA", "WARRANTS", "SA", 760, 7020, 5)
            };
            _classificationPositions = new List<PositionClassification>()
            {
                new PositionClassification(PositionClassifications.LIQUIDITA, "LIQUIDITA'", 1),
                new PositionClassification(PositionClassifications.CONTI, "CONTI", 1),
                new PositionClassification(PositionClassifications.INVESTIMENTI_BREVE_TERMINE, "INVESTIMENTI A BREVE TERMINE", 1),
                new PositionClassification(PositionClassifications.INVESTIMENTI_FIDUCIARI, "INVESTIMENTI FIDUCIARI", 1),
                new PositionClassification(PositionClassifications.MONEY_MARKET_FUNDS, "MONEY MARKET FUNDS", 1),
                new PositionClassification(PositionClassifications.OPERAZIONI_CAMBI_A_TERMINE, "OPERAZIONI CAMBI A TERMINE (PROFITTO/PERDITA)", 1),
                new PositionClassification(PositionClassifications.PRODOTTI_DERIVATI_SU_DIVISE, "PRODOTTI DERIVATI SU DIVISE", 1),
                new PositionClassification(PositionClassifications.BENI_NOMINALI, "BENI NOMINALI", 1),
                new PositionClassification(PositionClassifications.OBBLIGAZIONI_CON_SCADENZA_MINOR_OR_EQUAL_1_ANNO, "OBBLIGAZIONI CON SCADENZA <= 1 ANNO", 1),
                new PositionClassification(PositionClassifications.OBBLIGAZIONI_CON_SCADENZA_MINOR_OR_EQUAL_5_ANNI, "OBBLIGAZIONI CON SCADENZA <= 5 ANNI", 1),
                new PositionClassification(PositionClassifications.OBBLIGAZIONI_CON_SCADENZA_MAJOR_THAN_5_ANNI_FONDI_OBBLIGAZIONARI, "OBBLIGAZIONI CON SCADENZA > 5 ANNI, FONDI OBBLIGAZIONARI", 1),
                new PositionClassification(PositionClassifications.OBBLIGAZIONI_CONVERTIBILI_OBBLIGAZIONI_CON_WARRANTS, "OBBLIGAZIONI CONVERTIBILI, OBBLIGAZIONI CON WARRANTS", 1),
                new PositionClassification(PositionClassifications.BENI_REALI, "BENI REALI", 1),
                new PositionClassification(PositionClassifications.AZIONI_FONDI_AZIONARI, "AZIONI, FONDI AZIONARI", 1),
                new PositionClassification(PositionClassifications.PRODOTTI_DERIVATI_SU_TITOLI, "PRODOTTI DERIVATI SU TITOLI", 1),
                new PositionClassification(PositionClassifications.METALLI, "METALLI", 1),
                new PositionClassification(PositionClassifications.CONTI_METALLO_METALLI_FONDI_METALLO, "CONTI METALLO, METALLI, FONDI METALLO", 1),
                new PositionClassification(PositionClassifications.OPERAZIONI_METALLI_A_TERMINE, "OPERAZIONI METALLI A TERMINE", 1),
                new PositionClassification(PositionClassifications.PRODOTTI_DERIVATI_SU_METALLI, "PRODOTTI DERIVATI SU METALLI", 1),
                new PositionClassification(PositionClassifications.ALTRI_TIPI_DI_INVESTIMENTO, "ALTRI TIPI DI INVESTIMENTO", 1),
                new PositionClassification(PositionClassifications.FONDI_MISTI, "FONDI MISTI", 1),
                new PositionClassification(PositionClassifications.PRODOTTI_DERIVATI, "PRODOTTI DERIVATI", 1),
                new PositionClassification(PositionClassifications.PARTECIPAZIONI_IMMOBILIARI_FONDI_IMMOBILIARI, "PARTECIPAZIONI IMMOBILIARI, FONDI IMMOBILIARI", 1),
                new PositionClassification(PositionClassifications.PRODOTTI_ALTERNATIVI_DIVERSI, "PRODOTTI ALTERNATIVI, DIVERSI", 1),
                new PositionClassification(PositionClassifications.POSIZIONI_INFORMATIVE, "POSIZIONI INFORMATIVE", 2),
                new PositionClassification(PositionClassifications.MUTUI_IPOTECARI_E_CREDITI_DI_COSTRUZIONE, "MUTUI IPOTECARI E CREDITI DI COSTRUZIONE", 1),
                new PositionClassification(PositionClassifications.IMPEGNI_EVENTUALI, "IMPEGNI EVENTUALI", 1),
                new PositionClassification(PositionClassifications.BENE_IN_CUSTODIA, "BENE IN CUSTODIA", 2),
            };
            _valueTypes = new List<ValueTypology>()
            {
                new ValueTypology(ValueTypologies.OBBLIGAZIONI_CREDITI_ISCRITTI_UNITS_CON_OBBLIGAZIONE, "OBBLIGAZIONI, CREDITI ISCRITTI, UNITS CON OBBLIGAZIONE"),
                new ValueTypology(ValueTypologies.AZIONI_UNITS_BUONI_DI_PARTECIPAZIONE, "AZIONI, UNITS, BUONI DI PARTECIPAZIONE"),
                new ValueTypology(ValueTypologies.OBBL_DI_CASSA, "OBBL. DI CASSA"),
                new ValueTypology(ValueTypologies.CEDOLE_TALLONI, "CEDOLE/TALLONI"),
                new ValueTypology(ValueTypologies.DIVERSI_STRUMENTI_SENZA_CASH_FLOW, "DIVERSI STRUMENTI SENZA \"CASH-FLOW\""),
                new ValueTypology(ValueTypologies.POLIZZE_ASSICURAZIONE_DOCUMENTI_SIMILI, "POLIZZE D'ASSICURAZIONE E DOCUMENTI SIMILI"),
                new ValueTypology(ValueTypologies.STRUMENTI_IBRIDI_TFN_AZION, "STRUMENTI IBRIDI (TFN OBBL.)"),
                new ValueTypology(ValueTypologies.AZIONI_DI_CONSORZI_TRUST_SHARES, "AZIONI DI CONSORZI (\"TRUST SHARES\")"),
                new ValueTypology(ValueTypologies.INTERESSI_GUIDA, "INTERESSI GUIDA"),
                new ValueTypology(ValueTypologies.QUOTE_PARTI, "QUOTE PARTI"),
                new ValueTypology(ValueTypologies.DIR_SOTTOSCR_ATTRIBUZ_QUOTATI, "DIR. SOTTOSCR. O D'ATTRIBUZ. QUOTATI"),
                new ValueTypology(ValueTypologies.DIVISE_NAZIONALI, "DIVISE NAZIONALI"),
                new ValueTypology(ValueTypologies.CERTIFICATI_ABS, "CERTIFICATI ABS"),
                new ValueTypology(ValueTypologies.PARTI_SOCIALI_COOPERATIVE, "PARTI SOCIALI (COOPERATIVE)"),
                new ValueTypology(ValueTypologies.LIBRETTI_DI_DEPOSITO_E_DI_RISPARMIO, "LIBRETTI DI DEPOSITO E DI RISPARMIO"),
                new ValueTypology(ValueTypologies.FONDO_SEMPIONE, "FONDO SEMPIONE"),
                new ValueTypology(ValueTypologies.BUONI_DI_GODIMENTO, "BUONI DI GODIMENTO"),
                new ValueTypology(ValueTypologies.HEDGE_FUND, "HEDGE FUND"),
                new ValueTypology(ValueTypologies.VALORI_TECNICI, "VALORI TECNICI"),
                new ValueTypology(ValueTypologies.OBBLIGAZIONI_A_TASSO_VARIABILE, "OBBLIGAZIONI A TASSO VARIABILE"),
                new ValueTypology(ValueTypologies.CERTIFICATI_DI_OPZIONE, "CERTIFICATI DI OPZIONE"),
                new ValueTypology(ValueTypologies.BUONO_DI_SOTTOSCRIZIONE, "BUONO DI SOTTOSCRIZIONE"),
                new ValueTypology(ValueTypologies.FUTURES, "FUTURES"),
                new ValueTypology(ValueTypologies.TITOLI_DI_MERCATO_MONETARIO, "TITOLI DI MERCATO MONETARIO"),
                new ValueTypology(ValueTypologies.COMMODITIES_1, "COMMODITIES"),
                new ValueTypology(ValueTypologies.STRUMENTI_IBRIDI_TFN_AZION, "STRUMENTI IBRIDI (TFN AZION.)"),
                new ValueTypology(ValueTypologies.MONETE_ORO_O_ARGENTO, "MONETE D'ORO O D'ARGENTO"),
                new ValueTypology(ValueTypologies.METALLI_PREZIOSI, "METALLI PREZIOSI"),
                new ValueTypology(ValueTypologies.OBBLIGAZIONI_CONVERTIBILI, "OBBLIGAZIONI CONVERTIBILI"),
                new ValueTypology(ValueTypologies.CERTIFICATI_DI_VALORE_PER_METALLI_PREZIOSI, "CERTIFICATI DI VALORE PER METALLI PREZIOSI"),
                new ValueTypology(ValueTypologies.VALORI_MONETARI_TECNICI, "VALORI MONETARI TECNICI"),
                new ValueTypology(ValueTypologies.INDICI, "INDICI"),
                new ValueTypology(ValueTypologies.OPZIONI_CALLS_PUTS, "OPZIONI (CALLS, PUTS)"),
                new ValueTypology(ValueTypologies.FONDO_OBBLIGAZIONARIO_INTERNO_SICAV, "FONDO OBBLIGAZIONARIO INTERNO (SICAV)"),
                new ValueTypology(ValueTypologies.FONDO_AZIONARIO_INTERNO_SICAV, "FONDO AZIONARIO INTERNO (SICAV)"),
                new ValueTypology(ValueTypologies.FONDO_MONETARIO_INTERNO_SICAV, "FONDO MONETARIO INTERNO (SICAV)"),
                new ValueTypology(ValueTypologies.FONDO_OBBLIGAZIONARIO_ESTERNO, "FONDO OBBLIGAZIONARIO ESTERN"),
                new ValueTypology(ValueTypologies.FONDO_AZIONARIO_ESTERNO, "FONDO AZIONARIO ESTERNO"),
                new ValueTypology(ValueTypologies.FONDO_MONETARIO_ESTERNO, "FONDO MONETARIO ESTERNO"),
                new ValueTypology(ValueTypologies.FONDO_MISTO_ESTERNO, "FONDO MISTO ESTERNO"),
                new ValueTypology(ValueTypologies.FONDO_IMMOBILIARE_ESTERNO, "FONDO IMMOBILIARE ESTERNO"),
                new ValueTypology(ValueTypologies.DIVERSI, "DIVERSI"),
                new ValueTypology(ValueTypologies.NOTES, "NOTES"),
                new ValueTypology(ValueTypologies.SCHULDSCHEINE, "SCHULDSCHEINE"),
                new ValueTypology(ValueTypologies.COMMODITIES_2, "COMMODITIES"),
                new ValueTypology(ValueTypologies.VALORI_NON_PRESENTI_ESTRATTO, "VALORI NON PRESENTI SULL'ESTRATTO"),
                new ValueTypology(ValueTypologies.NOTES_A_TASSO_VARIABILE, "NOTES A TASSO VARIABILE"),
                new ValueTypology(ValueTypologies.NOTES_CONVERTIBILI, "NOTES CONVERTIBILI"),
                new ValueTypology(ValueTypologies.STRUMENTI_IBRIDI_TFN_ESENTE, "STRUMENTI IBRIDI (TFN ESENTE)"),
                new ValueTypology(ValueTypologies.OPZIONI_GRATUITE, "OPZIONI GRATUITE"),
                new ValueTypology(ValueTypologies.DIR_SOTTOSCR_O_ATTRIBUZ_NON_QUOTATI, "DIR. SOTTOSCR. O D'ATTRIBUZ. NON QUOTATI")
            };
            _sectionsBinding = new List<SectionBinding>()
            {
                new SectionBinding(ManipolationTypes.AsHeader, "header", "Headers"),
                new SectionBinding(ManipolationTypes.AsSection1, "section1", "ASSETS STATEMENT"),
                new SectionBinding(ManipolationTypes.AsSection3, "section3", "Portfolio Details"),
                new SectionBinding(ManipolationTypes.AsSection4, "section4", "Performance Evolution"),
                new SectionBinding(ManipolationTypes.AsSection8, "section8", "Liquidity", new List<PositionClassifications>() { PositionClassifications.CONTI }),
                new SectionBinding(ManipolationTypes.AsSection9, "section9", "Short-term investments", new List<PositionClassifications>() { PositionClassifications.INVESTIMENTI_BREVE_TERMINE }),
                new SectionBinding(ManipolationTypes.AsSection10, "section10", "Fiduciary Investments", new List<PositionClassifications>() { PositionClassifications.INVESTIMENTI_FIDUCIARI }),
                new SectionBinding(ManipolationTypes.AsSection11, "section11", "Forward Exchange Transactions (PROFIT/LOSS)", new List<PositionClassifications>() { PositionClassifications.OPERAZIONI_CAMBI_A_TERMINE }),
                new SectionBinding(ManipolationTypes.AsSection12, "section12", "Bonds", new List<PositionClassifications>() { PositionClassifications.OBBLIGAZIONI_CON_SCADENZA_MINOR_OR_EQUAL_5_ANNI }),
                new SectionBinding(ManipolationTypes.AsSection13, "section13", "Bonds", new List<PositionClassifications>() { PositionClassifications.OBBLIGAZIONI_CON_SCADENZA_MINOR_OR_EQUAL_1_ANNO }),
                new SectionBinding(ManipolationTypes.AsSection14, "section14", "Bonds", new List<PositionClassifications>() { PositionClassifications.OBBLIGAZIONI_CON_SCADENZA_MAJOR_THAN_5_ANNI_FONDI_OBBLIGAZIONARI }),
                new SectionBinding(ManipolationTypes.AsSection15, "section15", "Shares", new List<PositionClassifications>() { PositionClassifications.AZIONI_FONDI_AZIONARI }),
                new SectionBinding(ManipolationTypes.AsSection16And17, "section16-17", "Funds", new List<PositionClassifications>() { PositionClassifications.OBBLIGAZIONI_CON_SCADENZA_MINOR_OR_EQUAL_1_ANNO, PositionClassifications.OBBLIGAZIONI_CON_SCADENZA_MINOR_OR_EQUAL_5_ANNI, PositionClassifications.OBBLIGAZIONI_CON_SCADENZA_MAJOR_THAN_5_ANNI_FONDI_OBBLIGAZIONARI }),
                new SectionBinding(ManipolationTypes.AsSection18And19, "section18-19", "Others investments", new List<PositionClassifications>() { PositionClassifications.PRODOTTI_DERIVATI_SU_METALLI, PositionClassifications.PRODOTTI_DERIVATI, PositionClassifications.PRODOTTI_ALTERNATIVI_DIVERSI }),
                new SectionBinding(ManipolationTypes.AsSection20, "section20", "Metals", new List<PositionClassifications>() { PositionClassifications.CONTI_METALLO_METALLI_FONDI_METALLO }),
                new SectionBinding(ManipolationTypes.AsFooter, "footer", "Footers"),
            };
        }

        public static bool IsRowDestinatedToManipulator(IManipulator manipulator, string subCategory)
        {
            return string.IsNullOrEmpty(subCategory) == false && string.IsNullOrWhiteSpace(subCategory) == false 
                && Enum.IsDefined(typeof(PositionClassifications), int.Parse(subCategory)) 
                && manipulator.PositionClassificationsSource.Contains((PositionClassifications)int.Parse(subCategory));
        }

        public static bool ArePOSRowsManipulable(IEnumerable<POS> sourcePOSRows, ManipolationTypes currentManipulationType)
        {
            bool _areManipulable = false;
            if (sourcePOSRows != null && _sectionsBinding.Any(_flt => _flt.SectionId == currentManipulationType))
            {
                SectionBinding _sectionDest = _sectionsBinding.First(_flt => _flt.SectionId == currentManipulationType);
                if (_sectionDest.ClassificationsBound != null && _sectionDest.ClassificationsBound.Any())
                {
                    foreach (PositionClassifications _positionClassificationBound in _sectionDest.ClassificationsBound)
                    {
                        if (sourcePOSRows.Any(_flt => _flt.SubCat4_15.Trim() == ((int)_positionClassificationBound).ToString()))
                        {
                            _areManipulable = true; // if there is at least one POS row compatible with one of the classification bound to the destination section,
                            break;                  // this means the the source POS rows can be manipulated by the specific manipulator object
                        }
                    }
                }
            }
            return _areManipulable;
        }

        public static SectionBinding GetDestinationSection(IManipulator manipulator)
        {
            return _sectionsBinding.First(_flt => _flt.SectionId == manipulator.SectionDestination);
        }

        public static List<PositionClassifications> GetClassificationsBoundToSection(ManipolationTypes sectionId)
        {
            return _sectionsBinding.First(_flt => _flt.SectionId == sectionId).ClassificationsBound;
        }

    }

}
