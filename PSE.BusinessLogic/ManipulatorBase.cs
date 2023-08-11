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

        public List<PositionClassifications> PositionClassificationsSource { get; }
        public ManipolationTypes SectionDestination { get; }

        protected ManipulatorBase(ManipolationTypes sectionDestination, CultureInfo? culture = null)
        {
            PositionClassificationsSource = null;
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

        public abstract IOutputModel Manipulate(IList<IInputRecord> extractedData);

        public virtual string GetObjectNameDestination(IInputRecord inputRecord) { return string.Empty; }

    }

}
