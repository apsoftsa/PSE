using System.Globalization;

namespace PSE.BusinessLogic
{

    public abstract class ManipulatorBase
    {

        protected readonly CultureInfo _culture;

        protected ManipulatorBase(CultureInfo? culture = null)
        {
            if (culture == null)
                _culture = new CultureInfo("en-US"); // default;
            else
                _culture = culture;
        }

    }

}
