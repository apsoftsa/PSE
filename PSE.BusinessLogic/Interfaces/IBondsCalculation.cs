using PSE.Model.Params;

namespace PSE.BusinessLogic.Interfaces
{

    public interface IBondsCalculation
    {

        decimal GetGlobalVariationValue(GlobalVariationValueParams recParams);

        decimal GetPriceDifferenceValue(PriceDifferenceValueParams recParams);

        decimal GetCourseChangeValue(CourseChangeValueParams recParams);

        decimal GetCourseChangeValue5(CourseChangeValueParams recParams);

        decimal GetGlobalDifferenceValue(GlobalDifferenceValueParams recParams);

        decimal GetUnrealizedValue(UnrealizedValueParams recParams);

        decimal GetExchangeRateChangeValue(ExchangeRateChangeValueParams recParams);

        decimal GetExchangeRateDifferenceValue(ExchangeRateDifferenceValueParams recParams);

    }

}
