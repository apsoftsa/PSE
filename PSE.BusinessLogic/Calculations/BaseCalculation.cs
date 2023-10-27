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
            string _sign;            
            if(_calcSettings.SetNetting == 2) // SE, absolute netting 
                _sign = MULTIPLE_BY;
            else
            {
                string _signQta = POSITIVE_SIGN;
                if (quantity.HasValue && quantity.Value < 0)
                    _signQta = NEGATIVE_SIGN;
                if (!number.HasValue)
                    _sign = _signQta;
                else if (number.Value < 0)
                    _sign = NEGATIVE_SIGN;
                else if (number.Value == 0)
                    _sign = _signQta;
                else
                    _sign = POSITIVE_SIGN;
            }
            return _sign;
        }

        // Corrisponde a: "ValoreVariazioneGlobale"
        public virtual decimal GetGlobalVariationValue(GlobalVariationValueParams recParams)
        {
            decimal _globalVariationValue = 0;
            if (recParams != null && recParams.IsValid)
            {
                decimal _divider;
                bool _isOpposite = false;
                decimal _change1c = 0;
                decimal _change2c = 0;
                if (recParams.Percentage == -1)
                    _isOpposite = true;
                if (_isOpposite)
                {
                    if (recParams.Change1 > 0 && recParams.Change2 > 0)
                    {
                        _divider = 1;
                        _change1c = _divider / recParams.Change1;
                        _change2c = _divider / recParams.Change2;
                    }
                }
                else
                {
                    _change1c = recParams.Change1;
                    _change2c = recParams.Change2;
                }
                _divider = recParams.Trend1 * _change1c * 100m;
                if (_divider > 0)
                {
                    if (recParams.Sign == NEGATIVE_SIGN)
                        _globalVariationValue = (recParams.Trend1 * _change1c - recParams.Trend2 * _change2c) / _divider;
                    else
                        _globalVariationValue = (recParams.Trend2 * _change2c - recParams.Trend1 * _change1c) / _divider;
                }
            }
            return Math.Round(_globalVariationValue, _calcSettings.MeaningfulDecimalDigits);
        }

        // Corrisponde a: "ValoreDifferenzaCorso"
        public virtual decimal GetPriceDifferenceValue(PriceDifferenceValueParams recParams)
        {
            decimal _priceDifferenceValue = 0;
            if (recParams.IsValid)
            {
                if (string.IsNullOrEmpty(_calcSettings.ClassGroup) == false && _calcSettings.ClassGroup == "D") // derivati
                {
                    _priceDifferenceValue = GetCourseChangeValue5(new CourseChangeValueParams()
                    {
                        Sign = recParams.Sign,
                        Trend1 = recParams.Trend1,
                        Trend2 = recParams.Trend2
                    });
                }
                else
                {
                    _priceDifferenceValue = GetCourseChangeValue(new CourseChangeValueParams()
                    {
                        Sign = recParams.Sign,
                        Trend1 = recParams.Trend1,
                        Trend2 = recParams.Trend2
                    });
                }
            }
            return _priceDifferenceValue;
        }
        
        // Corrisponde a: "ValoreVariazioneCorso"
        public virtual decimal GetCourseChangeValue(CourseChangeValueParams recParams)
        {
            decimal _courseChangeValue = 0;
            if (recParams != null && recParams.IsValid)
            {
                if (!(recParams.Trend1 < 0.01m || recParams.Trend2 < 0.01m))
                {
                    if (recParams.Sign == NEGATIVE_SIGN)
                        _courseChangeValue = (recParams.Trend1 - recParams.Trend2) / recParams.Trend1 * 100m;
                    else
                        _courseChangeValue = (recParams.Trend2 - recParams.Trend1) / recParams.Trend1 * 100m;
                }
            }
            return Math.Round(_courseChangeValue, _calcSettings.MeaningfulDecimalDigits);
        }

        // Corrisponde a: "ValoreVariazioneCorso5"
        public virtual decimal GetCourseChangeValue5(CourseChangeValueParams recParams)
        {
            decimal _courseChangeValue = 0;
            if (recParams != null && recParams.IsValid)
            {
                if (!(recParams.Trend1 < 0.00001m || recParams.Trend2 < 0.00001m))
                {
                    if (recParams.Sign == NEGATIVE_SIGN)
                        _courseChangeValue = (recParams.Trend1 - recParams.Trend2) / recParams.Trend1 * 100m;
                    else
                        _courseChangeValue = (recParams.Trend2 - recParams.Trend1) / recParams.Trend1 * 100m;
                }
            }
            return Math.Round(_courseChangeValue, _calcSettings.MeaningfulDecimalDigits);
        }       

        // Corrisponde a: "ValoreDifferenzaGlobale"
        public virtual decimal GetGlobalDifferenceValue(GlobalDifferenceValueParams recParams)
        {
            decimal _globalDifferenceValue = 0;
            if (recParams.IsValid)
            {
                if (_calcSettings.StockMarketToChange(recParams.Code))
                {
                    _globalDifferenceValue = GetGlobalVariationValue(new GlobalVariationValueParams() { 
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
                    _globalDifferenceValue = GetGlobalVariationValue(new GlobalVariationValueParams()
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
            return _globalDifferenceValue;
        }

        // Corrisponde a: "ValoreDifferenzaCambio"
        public virtual decimal GetExchangeRateDifferenceValue(ExchangeRateDifferenceValueParams recParams)
        {
            decimal _exchangeRateDifferenceValue = 0;
            if (recParams.IsValid)
            {
                if (_calcSettings.StockMarketToChange(recParams.Code))
                {
                    _exchangeRateDifferenceValue = GetExchangeRateChangeValue(new ExchangeRateChangeValueParams()
                    {
                        Sign = recParams.Sign,
                        Change1 = recParams.ChangeAcqM,
                        Change2 = recParams.ChangeM,
                        Percentage = recParams.Reverse
                    });
                }
                else
                {
                    _exchangeRateDifferenceValue = GetExchangeRateChangeValue(new ExchangeRateChangeValueParams()
                    {
                        Sign = recParams.Sign,
                        Change1 = recParams.ChangeAcq,
                        Change2 = recParams.Change,
                        Percentage = recParams.Reverse
                    });
                }
            }
            return _exchangeRateDifferenceValue;
        }

        // Corrisponde a: "ValoreNonRealizzato"
        public virtual decimal GetUnrealizedValue(UnrealizedValueParams recParams)
        {
            decimal _unrealizedValue = 0;
            if (recParams != null && recParams.IsValid)
            {
                bool _continue = true;
                if (_calcSettings.ZeroHistoricalPurchasePriceSet)
                {
                    decimal? _change1, _change2;
                    decimal? _trend1 = recParams.TrendAcq;
                    decimal? _trend2 = recParams.Trend;
                    if (_calcSettings.StockMarketToChange(recParams.Code))
                    {
                        _change1 = recParams.ChangeAcqM;
                        _change2 = recParams.ChangeM;
                    }
                    else
                    {
                        _change1 = recParams.ChangeAcq;
                        _change2 = recParams.Change;
                    }
                    if (_change1.HasValue && _change2.HasValue && _trend1.HasValue && _trend2.HasValue)
                    {
                        if (_trend1 == 0 && _change1 != 0 && _trend2 != 0 && _change2 != 0)
                        {
                            if (recParams.Sign == NEGATIVE_SIGN)
                                _unrealizedValue = Math.Abs(recParams.ImpCtv) * -1m;
                            else
                                _unrealizedValue = recParams.ImpCtv;
                            _continue = false;
                        }
                    }
                }
                if (_continue)
                {
                    decimal _global = GetGlobalDifferenceValue(new GlobalDifferenceValueParams(
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
                    _global /= 100m;
                    if (_global == -1) // ???
                        _unrealizedValue = 0 - recParams.ImpCtv;
                    else if(_global != 0)
                    {
                        if (recParams.Sign == NEGATIVE_SIGN)
                            _unrealizedValue = Math.Abs(recParams.ImpCtv) * (_global / (1m - _global));
                        else
                            _unrealizedValue = recParams.ImpCtv * (_global / (1m + _global));
                    }
                }
            }
            return Math.Round(_unrealizedValue, _calcSettings.MeaningfulDecimalDigits);
        }

        // Corrisponde a: "ValoreVariazioneCambio"
        public virtual decimal GetExchangeRateChangeValue(ExchangeRateChangeValueParams recParams)
        {
            decimal _exchangeRateChangeValue = 0;
            if (recParams != null && recParams.IsValid)
            {
                decimal _change1c, _change2c;                
                bool _isOpposite = false;                
                if (recParams.Percentage == -1)
                    _isOpposite = true;
                if (_isOpposite)
                {
                    decimal _divider = 1;
                    _change1c = _divider / recParams.Change1;
                    _change2c = _divider / recParams.Change2;
                }
                else
                {
                    _change1c = recParams.Change1;
                    _change2c = recParams.Change2;
                }
                if (recParams.Sign == NEGATIVE_SIGN)
                    _exchangeRateChangeValue = (_change1c - _change2c) / _change1c * 100m;
                else
                    _exchangeRateChangeValue = (_change2c - _change1c) / _change1c * 100m;
            }
            return Math.Round(_exchangeRateChangeValue, _calcSettings.MeaningfulDecimalDigits);
        }

    }

}
