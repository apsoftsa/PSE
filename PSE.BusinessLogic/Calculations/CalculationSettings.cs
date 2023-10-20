namespace PSE.BusinessLogic.Calculations
{

    public class CalculationSettings
    {

        public string ClassGroup { get; set; } // (????) significato ed origine da chiarire...
        public bool ZeroHistoricalPurchasePriceSet { get; set; } // (????) significato sconosciuto...
        public int SetNetting { get; set; } // (????) significato sconosciuto, si usa nella determinazione del segno + o -

        public int MeaningfulDecimalDigits { get; set; }    

        // to-do (???)
        // corrisponde a: "FlagCambioMercato"
        // implementazione sconosciuta...
        public bool StockMarketToChange(string code) { return string.IsNullOrEmpty(code); }

        public CalculationSettings()
        {
            ClassGroup = string.Empty;
            ZeroHistoricalPurchasePriceSet = false;
            SetNetting = 0;
            MeaningfulDecimalDigits = Model.Common.Constants.DEFAULT_MEANINGFUL_DECIMAL_DIGITS_FOR_CALCULATION; // default ???
        }

    }

}
