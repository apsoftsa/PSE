using PSE.Model.Input.Interfaces;
using PSE.Model.Output.Interfaces;
using System.Globalization;
using static PSE.Model.Common.Constants;
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic
{

    public abstract class ManipulatorBase : IManipulator
    {

        protected readonly CultureInfo _culture;

        public PositionClassifications PostionClassificationSource { get; }
        public ManipolationTypes SectionDestination { get; }

        protected ManipulatorBase(ManipolationTypes sectionDestination, CultureInfo? culture = null)
        {
            PostionClassificationSource = PositionClassifications.UNKNOWN;
            SectionDestination = sectionDestination;
            if (culture == null)
                _culture = new CultureInfo(DEFAULT_CULTURE);
            else
                _culture = culture;
        }

        protected ManipulatorBase(PositionClassifications postionClassificationSource, ManipolationTypes sectionDestination, CultureInfo? culture = null)
        {
            PostionClassificationSource = postionClassificationSource;
            SectionDestination = sectionDestination;
            if (culture == null)
                _culture = new CultureInfo(DEFAULT_CULTURE); 
            else
                _culture = culture;
        }

        public abstract IOutputModel Manipulate(IList<IInputRecord> extractedData);

    }

}
