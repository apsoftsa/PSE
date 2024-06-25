using PSE.Model.Params;
using PSE.BusinessLogic.Interfaces;
using PSE.BusinessLogic.Calculations;
using static PSE.Model.Common.Constants;

namespace PSE.BusinessLogic.Utility
{

    public abstract class BaseCalculation : IBaseCalculation
    {

        protected readonly CalculationSettings _calcSettings;

        public int MeaningfulDecimalDigits { get { return _calcSettings.MeaningfulDecimalDigits; } private set { } }

        public BaseCalculation(CalculationSettings calcSettings)
        {
            _calcSettings = calcSettings;
        }

        // Corrisponde a : ValoreSegno
        public virtual string GetSign(decimal? quantity, decimal? number)
        {
            string sign;            
            if(_calcSettings.SetNetting == 2) // SE, absolute netting 
                sign = MULTIPLE_BY;
            else
            {
                string signQta = POSITIVE_SIGN;
                if (quantity.HasValue && quantity.Value < 0)
                    signQta = NEGATIVE_SIGN;
                if (!number.HasValue)
                    sign = signQta;
                else if (number.Value < 0)
                    sign = NEGATIVE_SIGN;
                else if (number.Value == 0)
                    sign = signQta;
                else
                    sign = POSITIVE_SIGN;
            }
            return sign;
        }

        // Corrisponde a: "ValoreVariazioneGlobale"
        public virtual decimal GetGlobalVariationValue(GlobalVariationValueParams recParams)
        {
            decimal globalVariationValue = 0;
            if (recParams != null && recParams.IsValid)
            {
                decimal divider;
                bool isOpposite = false;
                decimal change1c = 0;
                decimal change2c = 0;
                if (recParams.Percentage == -1)
                    isOpposite = true;
                if (isOpposite)
                {
                    if (recParams.Change1 > 0 && recParams.Change2 > 0)
                    {
                        divider = 1;
                        change1c = divider / recParams.Change1;
                        change2c = divider / recParams.Change2;
                    }
                }
                else
                {
                    change1c = recParams.Change1;
                    change2c = recParams.Change2;
                }
                divider = recParams.Trend1 * change1c * 100m;
                if (divider > 0)
                {
                    if (recParams.Sign == NEGATIVE_SIGN)
                        globalVariationValue = (recParams.Trend1 * change1c - recParams.Trend2 * change2c) / divider;
                    else
                        globalVariationValue = (recParams.Trend2 * change2c - recParams.Trend1 * change1c) / divider;
                }
            }
            return Math.Round(globalVariationValue, _calcSettings.MeaningfulDecimalDigits);
        }

        // Corrisponde a: "ValoreDifferenzaCorso"
        public virtual decimal GetPriceDifferenceValue(PriceDifferenceValueParams recParams)
        {
            decimal priceDifferenceValue = 0;
            if (recParams.IsValid)
            {
                if (string.IsNullOrEmpty(_calcSettings.ClassGroup) == false && _calcSettings.ClassGroup == "D") // derivati
                {
                    priceDifferenceValue = GetCourseChangeValue5(new CourseChangeValueParams()
                    {
                        Sign = recParams.Sign,
                        Trend1 = recParams.Trend1,
                        Trend2 = recParams.Trend2
                    });
                }
                else
                {
                    priceDifferenceValue = GetCourseChangeValue(new CourseChangeValueParams()
                    {
                        Sign = recParams.Sign,
                        Trend1 = recParams.Trend1,
                        Trend2 = recParams.Trend2
                    });
                }
            }
            return priceDifferenceValue;
        }
        
        // Corrisponde a: "ValoreVariazioneCorso"
        public virtual decimal GetCourseChangeValue(CourseChangeValueParams recParams)
        {
            decimal courseChangeValue = 0;
            if (recParams != null && recParams.IsValid)
            {
                if (!(recParams.Trend1 < 0.01m || recParams.Trend2 < 0.01m))
                {
                    if (recParams.Sign == NEGATIVE_SIGN)
                        courseChangeValue = (recParams.Trend1 - recParams.Trend2) / recParams.Trend1 * 100m;
                    else
                        courseChangeValue = (recParams.Trend2 - recParams.Trend1) / recParams.Trend1 * 100m;
                }
            }
            return Math.Round(courseChangeValue, _calcSettings.MeaningfulDecimalDigits);
        }

        // Corrisponde a: "ValoreVariazioneCorso5"
        public virtual decimal GetCourseChangeValue5(CourseChangeValueParams recParams)
        {
            decimal courseChangeValue = 0;
            if (recParams != null && recParams.IsValid)
            {
                if (!(recParams.Trend1 < 0.00001m || recParams.Trend2 < 0.00001m))
                {
                    if (recParams.Sign == NEGATIVE_SIGN)
                        courseChangeValue = (recParams.Trend1 - recParams.Trend2) / recParams.Trend1 * 100m;
                    else
                        courseChangeValue = (recParams.Trend2 - recParams.Trend1) / recParams.Trend1 * 100m;
                }
            }
            return Math.Round(courseChangeValue, _calcSettings.MeaningfulDecimalDigits);
        }       

        // Corrisponde a: "ValoreDifferenzaGlobale"
        public virtual decimal GetGlobalDifferenceValue(GlobalDifferenceValueParams recParams)
        {
            decimal globalDifferenceValue = 0;
            if (recParams.IsValid)
            {
                if (_calcSettings.StockMarketToChange(recParams.Code))
                {
                    globalDifferenceValue = GetGlobalVariationValue(new GlobalVariationValueParams() { 
                        Sign = recParams.Sign,
                        Trend1 = recParams.TrendAcq,
                        Trend2 = recParams.Trend,
                        Change1 = recParams.ChangeAcqM,
                        Change2 = recParams.ChangeM,
                        Percentage = recParams.Reverse
                    });
                }
                else
                {
                    globalDifferenceValue = GetGlobalVariationValue(new GlobalVariationValueParams()
                    {
                        Sign = recParams.Sign,
                        Trend1 = recParams.TrendAcq,
                        Trend2 = recParams.Trend,
                        Change1 = recParams.ChangeAcq,
                        Change2 = recParams.Change,
                        Percentage = recParams.Reverse
                    });
                }
            }
            return globalDifferenceValue;
        }

        // Corrisponde a: "ValoreDifferenzaCambio"
        public virtual decimal GetExchangeRateDifferenceValue(ExchangeRateDifferenceValueParams recParams)
        {
            decimal exchangeRateDifferenceValue = 0;
            if (recParams.IsValid)
            {
                if (_calcSettings.StockMarketToChange(recParams.Code))
                {
                    exchangeRateDifferenceValue = GetExchangeRateChangeValue(new ExchangeRateChangeValueParams()
                    {
                        Sign = recParams.Sign,
                        Change1 = recParams.ChangeAcqM,
                        Change2 = recParams.ChangeM,
                        Percentage = recParams.Reverse
                    });
                }
                else
                {
                    exchangeRateDifferenceValue = GetExchangeRateChangeValue(new ExchangeRateChangeValueParams()
                    {
                        Sign = recParams.Sign,
                        Change1 = recParams.ChangeAcq,
                        Change2 = recParams.Change,
                        Percentage = recParams.Reverse
                    });
                }
            }
            return exchangeRateDifferenceValue;
        }

        // Corrisponde a: "ValoreNonRealizzato"
        public virtual decimal GetUnrealizedValue(UnrealizedValueParams recParams)
        {
            decimal unrealizedValue = 0;
            if (recParams != null && recParams.IsValid)
            {
                bool continueCheck = true;
                if (_calcSettings.ZeroHistoricalPurchasePriceSet)
                {
                    decimal? change1, change2;
                    decimal? trend1 = recParams.TrendAcq;
                    decimal? trend2 = recParams.Trend;
                    if (_calcSettings.StockMarketToChange(recParams.Code))
                    {
                        change1 = recParams.ChangeAcqM;
                        change2 = recParams.ChangeM;
                    }
                    else
                    {
                        change1 = recParams.ChangeAcq;
                        change2 = recParams.Change;
                    }
                    if (change1.HasValue && change2.HasValue && trend1.HasValue && trend2.HasValue)
                    {
                        if (trend1 == 0 && change1 != 0 && trend2 != 0 && change2 != 0)
                        {
                            if (recParams.Sign == NEGATIVE_SIGN)
                                unrealizedValue = Math.Abs(recParams.ImpCtv) * -1m;
                            else
                                unrealizedValue = recParams.ImpCtv;
                            continueCheck = false;
                        }
                    }
                }
                if (continueCheck)
                {
                    decimal global = GetGlobalDifferenceValue(new GlobalDifferenceValueParams(
                        recParams.Code, 
                        recParams.Sign, 
                        recParams.TrendAcq, 
                        recParams.Trend, 
                        recParams.ChangeAcq, 
                        recParams.Change, 
                        recParams.ChangeAcqM,
                        recParams.ChangeM, 
                        recParams.Reverse)
                    );
                    global /= 100m;
                    if (global == -1) // ???
                        unrealizedValue = 0 - recParams.ImpCtv;
                    else if(global != 0)
                    {
                        if (recParams.Sign == NEGATIVE_SIGN)
                            unrealizedValue = Math.Abs(recParams.ImpCtv) * (global / (1m - global));
                        else
                            unrealizedValue = recParams.ImpCtv * (global / (1m + global));
                    }
                }
            }
            return Math.Round(unrealizedValue, _calcSettings.MeaningfulDecimalDigits);
        }

        // Corrisponde a: "ValoreVariazioneCambio"
        public virtual decimal GetExchangeRateChangeValue(ExchangeRateChangeValueParams recParams)
        {
            decimal exchangeRateChangeValue = 0;
            if (recParams != null && recParams.IsValid)
            {
                decimal change1c, change2c;                
                bool isOpposite = false;                
                if (recParams.Percentage == -1)
                    isOpposite = true;
                if (isOpposite)
                {
                    decimal divider = 1;
                    change1c = divider / recParams.Change1;
                    change2c = divider / recParams.Change2;
                }
                else
                {
                    change1c = recParams.Change1;
                    change2c = recParams.Change2;
                }
                if (recParams.Sign == NEGATIVE_SIGN)
                    exchangeRateChangeValue = (change1c - change2c) / change1c * 100m;
                else
                    exchangeRateChangeValue = (change2c - change1c) / change1c * 100m;
            }
            return Math.Round(exchangeRateChangeValue, _calcSettings.MeaningfulDecimalDigits);
        }

    }

}
