using System.ComponentModel;

namespace PSE.Model.Events
{

    public abstract class BaseExternalRequestEventArgs : CancelEventArgs {

        public string SectionName { get; }
        public string PropertyName { get; }
        public string PropertyKey { get; }
        public Dictionary<string, object>? PropertyParams { get; set; }
        public string? ErrorOccurred { get; set; }

        public bool HasError { get { return !string.IsNullOrEmpty(ErrorOccurred); } }

        public BaseExternalRequestEventArgs() : base(false) {
            SectionName = string.Empty;
            PropertyName = string.Empty;
            PropertyKey = string.Empty;
            PropertyParams = null;
            ErrorOccurred = string.Empty;
        }

        public BaseExternalRequestEventArgs(string sectionName, string propertyName, string propertyKey) : base(false) {
            SectionName = sectionName;
            PropertyName = propertyName;
            PropertyKey = propertyKey;
            PropertyParams = null;
            ErrorOccurred = string.Empty;
        }

        public BaseExternalRequestEventArgs(string sectionName, string propertyName, string propertyKey, Dictionary<string, object> propertyParams) : base(false) {
            SectionName = sectionName;
            PropertyName = propertyName;
            PropertyKey = propertyKey;
            PropertyParams = new Dictionary<string, object>(propertyParams);
            ErrorOccurred = string.Empty;
        }

    }

    public class ExternalCodifyRequestEventArgs : BaseExternalRequestEventArgs {

        public string? PropertyValue { get; set; }
        

        public ExternalCodifyRequestEventArgs() : base()
        {
            PropertyValue = string.Empty;
        }

        public ExternalCodifyRequestEventArgs(string sectionName, string propertyName, string propertyKey) : base(sectionName, propertyName, propertyKey)
        {
            PropertyValue = string.Empty;
        }

        public ExternalCodifyRequestEventArgs(string sectionName, string propertyName, string propertyKey, Dictionary<string, object> propertyParams) : base(sectionName, propertyName, propertyKey, propertyParams)
        {
            PropertyValue = string.Empty;
        }       

    }

    public class ExternalCodifiesRequestEventArgs : BaseExternalRequestEventArgs {

        public Dictionary<int, Dictionary<string, object>> PropertyValues { get; set; }


        public ExternalCodifiesRequestEventArgs() : base() {
            PropertyValues = [];  
        }

        public ExternalCodifiesRequestEventArgs(string sectionName, string propertyName, string propertyKey) : base(sectionName, propertyName, propertyKey) {
            PropertyValues = [];
        }

        public ExternalCodifiesRequestEventArgs(string sectionName, string propertyName, string propertyKey, Dictionary<string, object> propertyParams) : base(sectionName, propertyName, propertyKey, propertyParams) {
            PropertyValues = [];
        }

    }

}
