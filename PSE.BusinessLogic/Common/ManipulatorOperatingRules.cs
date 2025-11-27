using PSE.BusinessLogic.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.SupportTables;
using static PSE.Model.Common.Constants;
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic.Common
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
                new InvestmentType(InvestmentTypes.METALLI, "ME", "METALS", "XA", 790, 6010, 4),
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
                new PositionClassification(PositionClassifications.LIQUIDITY, "LIQUIDITY'", 1),
                new PositionClassification(PositionClassifications.ACCOUNT, "ACCOUNT", 1),
                new PositionClassification(PositionClassifications.SHORT_TERM_FUND, "INVESTIMENTI A BREVE TERMINE", 1),
                new PositionClassification(PositionClassifications.FIDUCIARY_INVESTMENTS, "INVESTIMENTI FIDUCIARI", 1),
                new PositionClassification(PositionClassifications.MONEY_MARKET_FUNDS, "MONEY MARKET FUNDS", 1),
                new PositionClassification(PositionClassifications.FORWARD_EXCHANGE_TRANSACTIONS, "OPERAZIONI CAMBI A TERMINE (PROFITTO/PERDITA)", 1),
                new PositionClassification(PositionClassifications.CURRENCY_DERIVATIVE_PRODUCTS, "PRODOTTI DERIVATI SU DIVISE", 1),
                //new PositionClassification(PositionClassifications.BONDS, "BENI NOMINALI", 1),
                new PositionClassification(PositionClassifications.BONDS_WITH_MATURITY_MINOR_EQUAL_1_YEAR, "OBBLIGAZIONI CON SCADENZA <= 1 ANNO", 1),
                new PositionClassification(PositionClassifications.BONDS_WITH_MATURITY_MINOR_EQUAL_5_YEARS, "OBBLIGAZIONI CON SCADENZA <= 5 ANNI", 1),
                new PositionClassification(PositionClassifications.BONDS_WITH_MATURITY_MAJOR_THAN_5_YEARS_OR_FUNDS, "OBBLIGAZIONI CON SCADENZA > 5 ANNI, FONDI OBBLIGAZIONARI", 1),
                new PositionClassification(PositionClassifications.CONVERTIBLE_BONDS_AND_BONDS_WITH_WARRANTS, "OBBLIGAZIONI CONVERTIBILI, OBBLIGAZIONI CON WARRANTS", 1),
                //new PositionClassification(PositionClassifications.REAL_GOODS, "BENI REALI", 1),
                new PositionClassification(PositionClassifications.SHARES, "AZIONI, FONDI AZIONARI", 1),
                new PositionClassification(PositionClassifications.DERIVATIVE_PRODUCTS_ON_SECURITIES, "PRODOTTI DERIVATI SU TITOLI", 1),
                //new PositionClassification(PositionClassifications.METALS, "METALS", 1),
                new PositionClassification(PositionClassifications.METALS_ACCOUNTS_FUNDS_PHYSICAL, "ACCOUNT METALLO, METALS, FONDI METALLO", 1),
                new PositionClassification(PositionClassifications.FORWARD_METAL_OPERATIONS_PROFIT_LOSS, "OPERAZIONI METALS A TERMINE", 1),
                new PositionClassification(PositionClassifications.DERIVATIVE_PRODUCTS_ON_METALS, "PRODOTTI DERIVATI SU METALS", 1),
                new PositionClassification(PositionClassifications.OTHER_TYPES_OF_INVESTMENT, "ALTRI TIPI DI INVESTIMENTO", 1),
                new PositionClassification(PositionClassifications.MIX_FUNDS, "FONDI MISTI", 1),
                new PositionClassification(PositionClassifications.DERIVATIVE_PRODUCTS_FUTURES, "PRODOTTI DERIVATI", 1),
                new PositionClassification(PositionClassifications.REAL_ESTATE_INVESTMENTS_FUNDS, "PARTECIPAZIONI IMMOBILIARI, FONDI IMMOBILIARI", 1),
                new PositionClassification(PositionClassifications.ALTERNATIVE_PRODUCTS_DIFFERENT, "PRODOTTI ALTERNATIVI, DIVERSI", 1),
                new PositionClassification(PositionClassifications.INFORMATION_POSITIONS, "POSIZIONI INFORMATIVE", 2),
                new PositionClassification(PositionClassifications.MORTGAGE_LOANS, "MUTUI IPOTECARI E CREDITI DI COSTRUZIONE", 1),
                new PositionClassification(PositionClassifications.POSSIBLE_COMMITMENTS, "IMPEGNI EVENTUALI", 1),
                //new PositionClassification(PositionClassifications.GOOD_IN_CUSTODY, "BENE IN CUSTODIA", 2),
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
                new ValueTypology(ValueTypologies.METALLI_PREZIOSI, "METALS PREZIOSI"),
                new ValueTypology(ValueTypologies.OBBLIGAZIONI_CONVERTIBILI, "OBBLIGAZIONI CONVERTIBILI"),
                new ValueTypology(ValueTypologies.CERTIFICATI_DI_VALORE_PER_METALLI_PREZIOSI, "CERTIFICATI DI VALORE PER METALS PREZIOSI"),
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
                new SectionBinding(ManipolationTypes.AsSection000, "section0", "Cover"),
                new SectionBinding(ManipolationTypes.AsSection010, "section10", "Portfolio Details"),
                new SectionBinding(ManipolationTypes.AsSection020, "section20", "Performance Evolution"),
                new SectionBinding(ManipolationTypes.AsSection030, "section30", "Multiline"),
                new SectionBinding(ManipolationTypes.AsSection040, "section40", "Breakdown by type of investment"),
                new SectionBinding(ManipolationTypes.AsSection060, "section60", "Breakdown by currency"),
                new SectionBinding(ManipolationTypes.AsSection070, "section70", "Liquidity", new List<PositionClassifications>() { PositionClassifications.ACCOUNT , PositionClassifications.SHORT_TERM_FUND, PositionClassifications.FIDUCIARY_INVESTMENTS, PositionClassifications.TEMPORARY_DEPOSITS, PositionClassifications.FORWARD_EXCHANGE_TRANSACTIONS, PositionClassifications.CURRENCY_DERIVATIVE_PRODUCTS }),
                new SectionBinding(ManipolationTypes.AsSection080, "section80", "Bonds", new List<PositionClassifications>() { PositionClassifications.BONDS_WITH_MATURITY_MINOR_EQUAL_1_YEAR, PositionClassifications.BONDS_WITH_MATURITY_MINOR_EQUAL_5_YEARS, PositionClassifications.BONDS_WITH_MATURITY_MAJOR_THAN_5_YEARS_OR_FUNDS, PositionClassifications.CONVERTIBLE_BONDS_AND_BONDS_WITH_WARRANTS }),
                new SectionBinding(ManipolationTypes.AsSection090, "section90", "Shares", new List<PositionClassifications>() { PositionClassifications.SHARES, PositionClassifications.DERIVATIVE_PRODUCTS_ON_SECURITIES }),
                new SectionBinding(ManipolationTypes.AsSection100, "section100", "Metals", new List<PositionClassifications>() { PositionClassifications.METALS_ACCOUNTS_FUNDS_PHYSICAL, PositionClassifications.FORWARD_METAL_OPERATIONS_PROFIT_LOSS, PositionClassifications.DERIVATIVE_PRODUCTS_ON_METALS  }),
                new SectionBinding(ManipolationTypes.AsSection110, "section110", "Others investments", new List<PositionClassifications>() { PositionClassifications.MIX_FUNDS, PositionClassifications.DERIVATIVE_PRODUCTS_FUTURES, PositionClassifications.REAL_ESTATE_INVESTMENTS_FUNDS, PositionClassifications.ALTERNATIVE_PRODUCTS_DIFFERENT }),
                new SectionBinding(ManipolationTypes.AsSection150, "section150", "Information Position", new List<PositionClassifications>() { PositionClassifications.MORTGAGE_LOANS, PositionClassifications.POSSIBLE_COMMITMENTS }),
                new SectionBinding(ManipolationTypes.AsSection160, "section160", "Shares subdivided by economic sector"),
                new SectionBinding(ManipolationTypes.AsSection170, "section170", "Shares by nations"),
                new SectionBinding(ManipolationTypes.AsSection130, "section130", "Pending orders"),
                new SectionBinding(ManipolationTypes.AsSection140, "section140", "Funds accumulation plan"),
                new SectionBinding(ManipolationTypes.AsSection190, "section190", "Accounts and deposit detected", new List<PositionClassifications>() { PositionClassifications.ACCOUNT }),
                new SectionBinding(ManipolationTypes.AsSection200, "section200", "End extract"),
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
            bool areManipulable = false;
            if (sourcePOSRows != null && _sectionsBinding.Any(flt => flt.SectionId == currentManipulationType))
            {
                SectionBinding sectionDest = _sectionsBinding.First(flt => flt.SectionId == currentManipulationType);
                if (sectionDest.ClassificationsBound != null && sectionDest.ClassificationsBound.Any())
                {
                    foreach (PositionClassifications positionClassificationBound in sectionDest.ClassificationsBound)
                    {
                        if (sourcePOSRows.Any(flt => flt.SubCat4_15.Trim() == ((int)positionClassificationBound).ToString()))
                        {
                            areManipulable = true; // if there is at least one POS row compatible with one of the classification bound to the destination section,
                            break;                  // this means the the source POS rows can be manipulated by the specific manipulator object
                        }
                    }
                }
            }
            return areManipulable;
        }

        public static SectionBinding GetDestinationSection(IManipulator manipulator)
        {
            return _sectionsBinding.First(flt => flt.SectionId == manipulator.SectionDestination);
        }

        public static List<PositionClassifications> GetClassificationsBoundToSection(ManipolationTypes sectionId)
        {
            return _sectionsBinding.First(flt => flt.SectionId == sectionId).ClassificationsBound;
        }

        public static bool CheckInputLanguage(string language)
        {
            bool @checked = false;
            if (!(string.IsNullOrEmpty(language) || language.Trim() == ""))
                @checked = language == ENGLISH_LANGUAGE_CODE || language == GERMAN_LANGUAGE_CODE || language == FRENCH_LANGUAGE_CODE || language == ITALIAN_LANGUAGE_CODE;
            return @checked;
        }

    }

}
