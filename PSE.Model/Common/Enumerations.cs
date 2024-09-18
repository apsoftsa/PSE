
using System.IO;
using System.Threading.Tasks;

namespace PSE.Model.Common
{

    public static class Enumerations
    {
        
        public enum StreamAcquisitionOutcomes : int
        {
            Unknown = -1,
            Success = 0,
            WithErrors = 1,
            Aborted = 2
        };

        public enum BuildingOutcomes : int
        {
            Unknown = -1,
            Success = 0,
            Failed = 1,
            Ignored = 2
        };

        public enum ManipolationTypes : int
        {
            Undefined = -1,
            AsHeader = 0,
            AsSection000 = 1,
            AsSection010 = 3,
            AsSection020 = 4,
            AsSection040 = 6,
            AsSection060 = 7,
            AsSection070 = 8,
            //AsSection9 = 9,
            //AsSection10 = 10,
            //AsSection11 = 11,
            AsSection080 = 12,
            //AsSection13 = 13,
            //AsSection14 = 14,
            AsSection090 = 15,
            //AsSection16And17 = 1617,
            AsSection110 = 1819,
            AsSection100 = 20,
            AsSection150 = 21,
            //AsSection22 = 22,
            AsSection160 = 23,
            AsSection130 = 24,
            AsSection190 = 25,
            //AsSection26 = 26,
            AsSection170 = 170,
            AsSection200 = 200,
            AsFooter = 99
        };

        public enum BuildFormats : int
        {
            Undefined = -1,
            Json = 0,
            Xml = 1
        };

        #region Support tables

        public enum PositionClassifications : int
        {
            UNKNOWN = -1,
            LIQUIDITA = 10,
            CONTI = 1010,
            INVESTIMENTI_BREVE_TERMINE = 1015,
            INVESTIMENTI_FIDUCIARI = 1020,
            MONEY_MARKET_FUNDS = 1025,
            OPERAZIONI_CAMBI_A_TERMINE = 1030,
            PRODOTTI_DERIVATI_SU_DIVISE = 1050,
            BENI_NOMINALI = 40,
            OBBLIGAZIONI_CON_SCADENZA_MINOR_OR_EQUAL_1_ANNO = 4005,
            OBBLIGAZIONI_CON_SCADENZA_MINOR_OR_EQUAL_5_ANNI = 4010,
            OBBLIGAZIONI_CON_SCADENZA_MAJOR_THAN_5_ANNI_FONDI_OBBLIGAZIONARI = 4020,
            OBBLIGAZIONI_CONVERTIBILI_OBBLIGAZIONI_CON_WARRANTS = 4030,
            BENI_REALI = 50,
            AZIONI_FONDI_AZIONARI = 5010,
            PRODOTTI_DERIVATI_SU_TITOLI = 5020,
            METALLI = 60,
            CONTI_METALLO_METALLI_FONDI_METALLO = 6010,
            OPERAZIONI_METALLI_A_TERMINE = 6020,
            PRODOTTI_DERIVATI_SU_METALLI = 6030,
            ALTRI_TIPI_DI_INVESTIMENTO = 70,
            FONDI_MISTI = 7010,
            PRODOTTI_DERIVATI = 7020,
            PARTECIPAZIONI_IMMOBILIARI_FONDI_IMMOBILIARI = 7030,
            PRODOTTI_ALTERNATIVI_DIVERSI = 7090,
            POSIZIONI_INFORMATIVE = 90,
            MUTUI_IPOTECARI_E_CREDITI_DI_COSTRUZIONE = 9010,
            IMPEGNI_EVENTUALI = 9020,
            BENE_IN_CUSTODIA = 9030
        };

        public enum InvestmentTypes : int
        {
            UNKNOWN = -1,
            PRODOTTI_ALTERNATIVI,
            DIVERSI1,
            DIVERSI2,
            ACCETTAZIONI_BANCARIE,
            AZIONI,
            CERTIFICATI_DI_DEPOSITO,
            FONDI_AZIONARI,
            FONDI_IMMOBILIARI,
            FONDI_MISTI,
            FONDI_OBBLIGAZIONARI,
            FONDI_MONETARI,
            FUTURES,
            GROI,
            HEDGE_FUND,
            METALLI,
            FONDI_METALLO,
            OBBLIGAZIONI_CON_SCADENZA_MINOR_OR_EQUAL_THAN_1_ANNO,
            OBBLIGAZIONI_CON_SCADENZA_MINOR_OR_EQUAL_THAN_5_ANNI,
            OBBLIGAZIONI_CON_SCADENZA_MAJOR_THAN_5_ANNI,
            OBBLIGAZIONI_CONVERTIBILI,
            OPZIONI,
            OBBLIGAZIONI_CON_WARRANTS,
            PARTECIPAZIONI_IMMOBILIARI,
            PROMISSORY_NOTES,
            TREASURY_BILLS,
            WARRANTS
        };

        public enum ValueTypologies : int
        {
            UNKNOWN = -1,
            OBBLIGAZIONI_CREDITI_ISCRITTI_UNITS_CON_OBBLIGAZIONE = 0,
            AZIONI_UNITS_BUONI_DI_PARTECIPAZIONE = 1,
            OBBL_DI_CASSA = 2,
            CEDOLE_TALLONI = 3,
            DIVERSI_STRUMENTI_SENZA_CASH_FLOW = 4,
            POLIZZE_ASSICURAZIONE_DOCUMENTI_SIMILI = 5,
            STRUMENTI_IBRIDI_TFN_OBBL = 6,
            AZIONI_DI_CONSORZI_TRUST_SHARES = 7,
            INTERESSI_GUIDA = 8,
            QUOTE_PARTI = 9,
            DIR_SOTTOSCR_ATTRIBUZ_QUOTATI = 10,
            DIVISE_NAZIONALI = 11,
            CERTIFICATI_ABS = 12,
            PARTI_SOCIALI_COOPERATIVE = 13,
            LIBRETTI_DI_DEPOSITO_E_DI_RISPARMIO = 14,
            FONDO_SEMPIONE = 15,
            BUONI_DI_GODIMENTO = 16,
            HEDGE_FUND = 18,
            VALORI_TECNICI = 19,
            OBBLIGAZIONI_A_TASSO_VARIABILE = 21,
            CERTIFICATI_DI_OPZIONE = 22,
            BUONO_DI_SOTTOSCRIZIONE = 23,
            FUTURES = 25,
            TITOLI_DI_MERCATO_MONETARIO = 26,
            COMMODITIES_1 = 27,
            STRUMENTI_IBRIDI_TFN_AZION = 28,
            MONETE_ORO_O_ARGENTO = 29,
            METALLI_PREZIOSI = 30,
            OBBLIGAZIONI_CONVERTIBILI = 31,
            CERTIFICATI_DI_VALORE_PER_METALLI_PREZIOSI = 32,
            VALORI_MONETARI_TECNICI = 33,
            INDICI = 34,
            OPZIONI_CALLS_PUTS = 35,
            FONDO_OBBLIGAZIONARIO_INTERNO_SICAV = 40,
            FONDO_AZIONARIO_INTERNO_SICAV = 41,
            FONDO_MONETARIO_INTERNO_SICAV = 42,
            FONDO_OBBLIGAZIONARIO_ESTERNO = 43,
            FONDO_AZIONARIO_ESTERNO = 44,
            FONDO_MONETARIO_ESTERNO = 45,
            FONDO_MISTO_ESTERNO = 46,
            FONDO_IMMOBILIARE_ESTERNO = 47,
            DIVERSI = 900,
            NOTES = 901,
            SCHULDSCHEINE = 902,
            COMMODITIES_2 = 903,
            VALORI_NON_PRESENTI_ESTRATTO = 905,
            NOTES_A_TASSO_VARIABILE = 921,
            NOTES_CONVERTIBILI = 931,
            STRUMENTI_IBRIDI_TFN_ESENTE = 940,
            OPZIONI_GRATUITE = 941,
            DIR_SOTTOSCR_O_ATTRIBUZ_NON_QUOTATI = 942
        };

        #endregion

    }

}
