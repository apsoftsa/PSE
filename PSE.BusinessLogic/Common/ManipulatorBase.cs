using System.Globalization;
using PSE.Model.Events;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Dictionary;
using PSE.BusinessLogic.Interfaces;
using static PSE.Model.Common.Constants;
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic.Common
{

    public abstract class ManipulatorBase : IManipulator
    {

        protected readonly CultureInfo _culture;

        public List<PositionClassifications> PositionClassificationsSource { get; }
        public ManipolationTypes SectionDestination { get; }
        public decimal? TotalAssets { get; protected set; } = null;

        public event ExternalCodifyEventHandler? ExternalCodifyRequest;
        public event ExternalCodifiesEventHandler? ExternalCodifiesRequest;

        protected virtual void OnExternalCodifyRequest(ExternalCodifyRequestEventArgs e)
        {
            ExternalCodifyRequest?.Invoke(this, e);
        }

        protected virtual void OnExternalCodifiesRequest(ExternalCodifiesRequestEventArgs e) {
            ExternalCodifiesRequest?.Invoke(this, e);
        }

        protected ManipulatorBase(ManipolationTypes sectionDestination, CultureInfo? culture = null)
        {
            PositionClassificationsSource = new List<PositionClassifications>();
            SectionDestination = sectionDestination;
            if (culture == null)
                _culture = new CultureInfo(DEFAULT_CULTURE);
            else
                _culture = culture;
        }

        protected ManipulatorBase(PositionClassifications positionClassificationSource, ManipolationTypes sectionDestination, CultureInfo? culture = null)
        {
            PositionClassificationsSource = new List<PositionClassifications>() { positionClassificationSource };
            SectionDestination = sectionDestination;
            if (culture == null)
                _culture = new CultureInfo(DEFAULT_CULTURE);
            else
                _culture = culture;
        }

        protected ManipulatorBase(List<PositionClassifications> positionClassificationsSource, ManipolationTypes sectionDestination, CultureInfo? culture = null)
        {
            PositionClassificationsSource = new List<PositionClassifications>(positionClassificationsSource);
            SectionDestination = sectionDestination;
            if (culture == null)
                _culture = new CultureInfo(DEFAULT_CULTURE);
            else
                _culture = culture;
        }        

        public abstract IOutputModel Manipulate(IPSEDictionaryService dictionaryService, IList<IInputRecord> extractedData, decimal? totalAssets = null);

        public virtual string GetObjectNameDestination(IInputRecord inputRecord) { return string.Empty; }

        protected void CalculateSummaries(string currencyOfReference, ISummaryTo summaryTo, ISummaryBeginningYear summaryBeginningYear, ISummaryPurchase summaryPurchase, string quoteType, IEnumerable<CUR>? curItems, string currency, decimal? quantity)
        {
            decimal quote = string.IsNullOrEmpty(quoteType) == false && quoteType == "%" ? 100.0m : 1.0m;
            summaryTo.PercentExchange = 0;
            summaryTo.PercentProfitLossN = 0;   
            if (summaryTo.ValuePrice.HasValue)
            {                
                decimal exchangeRate = curItems != null && curItems.Any(f => f.Currency_5 == currency && f.Rate_6.HasValue) ? curItems.First(f => f.Currency_5 == currency && f.Rate_6.HasValue).Rate_6.Value : 0;
                if (summaryBeginningYear.ValuePrice.HasValue && summaryBeginningYear.ValuePrice != 0m)
                    summaryBeginningYear.PercentPrice = Math.Round((summaryTo.ValuePrice.Value - summaryBeginningYear.ValuePrice.Value) / summaryBeginningYear.ValuePrice.Value * 100m, DEFAULT_MEANINGFUL_DECIMAL_DIGITS_FOR_CALCULATION);
                else
                    summaryBeginningYear.PercentPrice = 0m;
                if (summaryPurchase.ValuePrice.HasValue && summaryPurchase.ValuePrice != 0m)
                    summaryPurchase.PercentPrice = Math.Round((summaryTo.ValuePrice.Value - summaryPurchase.ValuePrice.Value) / summaryPurchase.ValuePrice.Value * 100m, DEFAULT_MEANINGFUL_DECIMAL_DIGITS_FOR_CALCULATION);
                else
                    summaryPurchase.PercentPrice = 0m;
                if (quantity.HasValue && summaryBeginningYear.ValuePrice.HasValue && quantity.Value != 0m && summaryBeginningYear.ExchangeValue.HasValue) {
                    if (currencyOfReference.Equals(CURRENCY_BASE, StringComparison.InvariantCultureIgnoreCase)) {
                        summaryBeginningYear.ProfitLossNotRealizedValue = Math.Round((summaryTo.ValuePrice.Value * quantity.Value / quote * exchangeRate) - (summaryBeginningYear.ValuePrice.Value * quantity.Value / quote * summaryBeginningYear.ExchangeValue.Value), DEFAULT_CURRENCY_DECIMAL_DIGITS);
                        if (summaryPurchase.ExchangeValue.HasValue && summaryPurchase.ValuePrice.HasValue)
                            summaryPurchase.ProfitLossNotRealizedValue = Math.Round((summaryTo.ValuePrice.Value * quantity.Value / quote * exchangeRate) - (summaryPurchase.ValuePrice.Value * quantity.Value / quote * summaryPurchase.ExchangeValue.Value), DEFAULT_CURRENCY_DECIMAL_DIGITS);
                    } else if (summaryBeginningYear.ExchangeValue.Value != 0) {
                        summaryBeginningYear.ProfitLossNotRealizedValue = Math.Round((summaryTo.ValuePrice.Value * quantity.Value / quote / exchangeRate) - (summaryBeginningYear.ValuePrice.Value * quantity.Value / quote / summaryBeginningYear.ExchangeValue.Value), DEFAULT_CURRENCY_DECIMAL_DIGITS);
                        if (summaryPurchase.ExchangeValue.HasValue && summaryPurchase.ValuePrice.HasValue)
                            summaryPurchase.ProfitLossNotRealizedValue = Math.Round((summaryTo.ValuePrice.Value * quantity.Value / quote / exchangeRate) - (summaryPurchase.ValuePrice.Value * quantity.Value / quote / summaryPurchase.ExchangeValue.Value), DEFAULT_CURRENCY_DECIMAL_DIGITS);
                    }
                }
            }
            if (summaryTo.ExchangeValue.HasValue && summaryBeginningYear.ExchangeValue.HasValue && summaryBeginningYear.ExchangeValue.Value > 0)
                summaryBeginningYear.PercentExchange = Math.Round((summaryTo.ExchangeValue.Value - summaryBeginningYear.ExchangeValue.Value) / summaryBeginningYear.ExchangeValue.Value * 100m, DEFAULT_MEANINGFUL_DECIMAL_DIGITS_FOR_CALCULATION);
            if (summaryTo.ExchangeValue.HasValue && summaryPurchase.ExchangeValue.HasValue && summaryPurchase.ExchangeValue.Value > 0)
                summaryPurchase.PercentExchange = Math.Round((summaryTo.ExchangeValue.Value - summaryPurchase.ExchangeValue.Value) / summaryPurchase.ExchangeValue.Value * 100m, DEFAULT_MEANINGFUL_DECIMAL_DIGITS_FOR_CALCULATION);
            if (summaryBeginningYear.ProfitLossNotRealizedValue.HasValue && summaryBeginningYear.ValuePrice.HasValue && quantity.HasValue && quote > 0 && summaryBeginningYear.ExchangeValue.HasValue) {
                decimal temp = (summaryBeginningYear.ValuePrice.Value * quantity.Value / quote * summaryBeginningYear.ExchangeValue.Value);
                if (temp != 0)
                    summaryBeginningYear.PercentProfitLossN = Math.Round(summaryBeginningYear.ProfitLossNotRealizedValue.Value / temp * 100m, DEFAULT_CURRENCY_DECIMAL_DIGITS);
            }
            if (summaryPurchase.ProfitLossNotRealizedValue.HasValue && summaryPurchase.ValuePrice.HasValue && quantity.HasValue && quote > 0 && summaryPurchase.ExchangeValue.HasValue) {
                decimal temp = (summaryPurchase.ValuePrice.Value * quantity.Value / quote * summaryPurchase.ExchangeValue.Value);
                if (temp != 0)
                    summaryPurchase.PercentProfitLossN = Math.Round(summaryPurchase.ProfitLossNotRealizedValue.Value / temp * 100m, DEFAULT_CURRENCY_DECIMAL_DIGITS);
            }
        }

        protected string GetCoupon(string couponFreq, string couponText)
        {
            string coupon = couponFreq.Trim();            
            if (couponText.Trim().Length >= 3)
            {
                if (couponText.Trim().Length == 3) couponText = "0" + couponText.Trim();
                if (coupon.Length > 0) coupon += " ";
                coupon += string.Concat(couponText.Substring(2, 2), ".", couponText.Substring(0, 2));   
            }
            return coupon;
        }

        protected string AssignRequiredString(string value) { return string.IsNullOrEmpty(value) ? "" : value.Trim(); }

        protected string BuildComposedDescription(string[] elements, string separator = "/") {
            string outcome = "";
            if (elements.Length > 0) {
                for (int i = 0; i < elements.Length; i++) {
                    if (elements[i] != string.Empty) {
                        if(i > 0 && outcome != "") outcome += " " + separator + " ";  
                        outcome += elements[i];
                    }
                }
            }
            return outcome;
        }

        protected decimal AssignRequiredDecimal(decimal? value) { return value.HasValue ? value.Value : decimal.Zero; }

        protected decimal AssignRequiredDecimals(decimal? value1, decimal? value2) 
        { 
            decimal total = decimal.Zero;   
            if (value1.HasValue) total += value1.Value;
            if (value2.HasValue) total += value2.Value;
            return total;
        }

        protected decimal AssignRequiredCurrencyDecimal(decimal? value) { 
            return value.HasValue ? Math.Round(value.Value, DEFAULT_CURRENCY_DECIMAL_DIGITS) : decimal.Zero; 
        }

        protected long AssignRequiredLong(long? value) { return value.HasValue ? value.Value : 0; }

        protected string AssignRequiredDate(DateTime? value, CultureInfo culture) { return value.HasValue ? value.Value.ToString(DEFAULT_DATE_FORMAT, culture) : string.Empty; }

        protected string AssignRequiredDate(string value, CultureInfo culture) {
            if (!(string.IsNullOrWhiteSpace(value) || string.IsNullOrEmpty(value))) {
                if (DateTime.TryParseExact(value, COMPACT_DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedExact)) 
                    return parsedExact.ToString(DEFAULT_DATE_FORMAT, culture);
            }
            return string.Empty;
        }

        protected decimal CalculatePercentWeight(decimal? totalAssests, decimal? marketValue) {
            decimal percentWeight = 0;
            if(totalAssests.HasValue && totalAssests.Value != 0 && marketValue.HasValue) 
                percentWeight = Math.Round(marketValue.Value / totalAssests.Value * 100m, DEFAULT_MEANINGFUL_DECIMAL_DIGITS_FOR_CALCULATION);
            return percentWeight;
        }

    }

}
