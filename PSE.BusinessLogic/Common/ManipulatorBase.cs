using PSE.BusinessLogic.Interfaces;
using PSE.Model.Events;
using PSE.Model.Input.Interfaces;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using System.Globalization;
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

        protected virtual void OnExternalCodifyRequest(ExternalCodifyRequestEventArgs e)
        {
            ExternalCodifyRequest?.Invoke(this, e);
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

        public abstract IOutputModel Manipulate(IList<IInputRecord> extractedData, decimal? totalAssets = null);
        public virtual string GetObjectNameDestination(IInputRecord inputRecord) { return string.Empty; }

        protected void CalculateSummaries(ISummaryTo summaryTo, ISummaryBeginningYear summaryBeginningYear, ISummaryPurchase summaryPurchase, decimal? quantity)
        {
            if (summaryTo.ValuePrice.HasValue)
            {
                if (summaryBeginningYear.ValuePrice.HasValue && summaryBeginningYear.ValuePrice != 0m)
                    summaryBeginningYear.PercentPrice = Math.Round((summaryTo.ValuePrice.Value - summaryBeginningYear.ValuePrice.Value) / summaryBeginningYear.ValuePrice.Value, DEFAULT_MEANINGFUL_DECIMAL_DIGITS_FOR_CALCULATION);
                else
                    summaryBeginningYear.PercentPrice = 0m;
                if (summaryPurchase.ValuePrice.HasValue && summaryPurchase.ValuePrice != 0m)
                    summaryPurchase.PercentPrice = Math.Round((summaryTo.ValuePrice.Value - summaryPurchase.ValuePrice.Value) / summaryPurchase.ValuePrice.Value, DEFAULT_MEANINGFUL_DECIMAL_DIGITS_FOR_CALCULATION);
                else
                    summaryPurchase.PercentPrice = 0m;
                if (quantity.HasValue && quantity.Value != 0m)
                {
                    summaryBeginningYear.ProfitLossNotRealizedValue = Math.Round((summaryTo.ValuePrice.Value - summaryBeginningYear.ValuePrice.Value) * quantity.Value, DEFAULT_MEANINGFUL_DECIMAL_DIGITS_FOR_CALCULATION);
                    summaryPurchase.ProfitLossNotRealizedValue = Math.Round((summaryTo.ValuePrice.Value - summaryPurchase.ValuePrice.Value) * quantity.Value, DEFAULT_MEANINGFUL_DECIMAL_DIGITS_FOR_CALCULATION);
                }
            }
        }

        protected void CalculateBondsSummaries(ISummaryTo summaryTo, ISummaryBeginningYear summaryBeginningYear, ISummaryPurchase summaryPurchase, decimal? quantity) {
            if (summaryTo.ValuePrice.HasValue) {
                if (summaryBeginningYear.ValuePrice.HasValue && summaryBeginningYear.ValuePrice != 0m)
                    summaryBeginningYear.PercentPrice = Math.Round((summaryTo.ValuePrice.Value - summaryBeginningYear.ValuePrice.Value) / summaryBeginningYear.ValuePrice.Value * 100m, DEFAULT_MEANINGFUL_DECIMAL_DIGITS_FOR_CALCULATION);
                else
                    summaryBeginningYear.PercentPrice = 0m;
                if (summaryPurchase.ValuePrice.HasValue && summaryPurchase.ValuePrice != 0m)
                    summaryPurchase.PercentPrice = Math.Round((summaryTo.ValuePrice.Value - summaryPurchase.ValuePrice.Value) / summaryPurchase.ValuePrice.Value * 100m, DEFAULT_MEANINGFUL_DECIMAL_DIGITS_FOR_CALCULATION);
                else
                    summaryPurchase.PercentPrice = 0m;
                if (quantity.HasValue && quantity.Value != 0m) {
                    summaryBeginningYear.ProfitLossNotRealizedValue = Math.Round((summaryTo.ValuePrice.Value - summaryBeginningYear.ValuePrice.Value) * quantity.Value / 100m, DEFAULT_MEANINGFUL_DECIMAL_DIGITS_FOR_CALCULATION);
                    summaryPurchase.ProfitLossNotRealizedValue = Math.Round(((summaryTo.ValuePrice.Value - summaryPurchase.ValuePrice.Value) * quantity.Value) / 100m, DEFAULT_MEANINGFUL_DECIMAL_DIGITS_FOR_CALCULATION);
                }
            }
        }

        protected void CalculateSharesSummaries(ISummaryTo summaryTo, ISummaryBeginningYear summaryBeginningYear, ISummaryPurchase summaryPurchase, decimal? quantity) {
            if (summaryTo.ValuePrice.HasValue) {
                if (summaryBeginningYear.ValuePrice.HasValue && summaryBeginningYear.ValuePrice != 0m)
                    summaryBeginningYear.PercentPrice = Math.Round((summaryTo.ValuePrice.Value - summaryBeginningYear.ValuePrice.Value) / summaryBeginningYear.ValuePrice.Value * 100m, DEFAULT_MEANINGFUL_DECIMAL_DIGITS_FOR_CALCULATION);
                else
                    summaryBeginningYear.PercentPrice = 0m;
                if (summaryPurchase.ValuePrice.HasValue && summaryPurchase.ValuePrice != 0m)
                    summaryPurchase.PercentPrice = Math.Round((summaryTo.ValuePrice.Value - summaryPurchase.ValuePrice.Value) / summaryPurchase.ValuePrice.Value * 100m, DEFAULT_MEANINGFUL_DECIMAL_DIGITS_FOR_CALCULATION);
                else
                    summaryPurchase.PercentPrice = 0m;
                if (quantity.HasValue && quantity.Value != 0m) {
                    summaryBeginningYear.ProfitLossNotRealizedValue = Math.Round((summaryTo.ValuePrice.Value - summaryBeginningYear.ValuePrice.Value) * quantity.Value, DEFAULT_MEANINGFUL_DECIMAL_DIGITS_FOR_CALCULATION);
                    summaryPurchase.ProfitLossNotRealizedValue = Math.Round((summaryTo.ValuePrice.Value - summaryPurchase.ValuePrice.Value) * quantity.Value, DEFAULT_MEANINGFUL_DECIMAL_DIGITS_FOR_CALCULATION);
                }
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

        protected decimal AssignRequiredDecimal(decimal? value) { return value.HasValue ? value.Value : decimal.Zero; }

        protected long AssignRequiredLong(long? value) { return value.HasValue ? value.Value : 0; }

        protected string AssignRequiredDate(DateTime? value, CultureInfo culture) { return value.HasValue ? value.Value.ToString(DEFAULT_DATE_FORMAT, culture) : ""; }

        protected decimal CalculatePercentWeight(decimal? totalAssests, decimal? marketValue) {
            decimal percentWeight = 0;
            if(totalAssests.HasValue && totalAssests.Value != 0 && marketValue.HasValue) 
                percentWeight = Math.Round(marketValue.Value / totalAssests.Value * 100m, DEFAULT_MEANINGFUL_DECIMAL_DIGITS_FOR_CALCULATION);
            return percentWeight;
        }

    }

}
