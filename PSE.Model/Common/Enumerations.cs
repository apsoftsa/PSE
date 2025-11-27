
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
            AsSection010 = 10,
            AsSection020 = 20,
            AsSection030 = 30,
            AsSection040 = 40,
            AsSection060 = 60,
            AsSection070 = 70,
            AsSection080 = 80,
            AsSection090 = 90,
            AsSection110 = 110,
            AsSection100 = 100,
            AsSection130 = 130,
            AsSection140 = 140,
            AsSection150 = 150,
            AsSection160 = 160,
            AsSection170 = 170,            
            AsSection190 = 190,
            AsSection200 = 200,
            AsFooter = 999
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
            LIQUIDITY = 10,
            ACCOUNT = 1010,
            SHORT_TERM_FUND = 1015,
            FIDUCIARY_INVESTMENTS = 1020,
            TEMPORARY_DEPOSITS = 1023,
            MONEY_MARKET_FUNDS = 1025,
            FORWARD_EXCHANGE_TRANSACTIONS = 1030,
            CURRENCY_DERIVATIVE_PRODUCTS = 1050,
            BONDS = 40,
            BONDS_WITH_MATURITY_MINOR_EQUAL_1_YEAR = 4005,
            BONDS_WITH_MATURITY_MINOR_EQUAL_5_YEARS = 4010,
            BONDS_WITH_MATURITY_MAJOR_THAN_5_YEARS_OR_FUNDS = 4020,
            CONVERTIBLE_BONDS_AND_BONDS_WITH_WARRANTS = 4030,
            REAL_GOODS = 50,
            SHARES = 5010,
            DERIVATIVE_PRODUCTS_ON_SECURITIES = 5020, 
            METALS = 60,
            METALS_ACCOUNTS_FUNDS_PHYSICAL = 6010,
            FORWARD_METAL_OPERATIONS_PROFIT_LOSS = 6020,
            DERIVATIVE_PRODUCTS_ON_METALS = 6030,
            OTHER_TYPES_OF_INVESTMENT = 70,
            MIX_FUNDS = 7010,
            DERIVATIVE_PRODUCTS_FUTURES = 7020,
            REAL_ESTATE_INVESTMENTS_FUNDS = 7030,
            ALTERNATIVE_PRODUCTS_DIFFERENT = 7090,
            INFORMATION_POSITIONS = 90,
            MORTGAGE_LOANS = 9010,
            POSSIBLE_COMMITMENTS = 9020,
            GOOD_IN_CUSTODY = 9030
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
