using System.ComponentModel;

namespace PSE.Model.Events
{

    public class ExternalCodifyRequestEventArgs : CancelEventArgs
    {

        public string SectionName { get; } 
        public string PropertyName { get; }
        public string PropertyKey { get; }
        public Dictionary<string, object>? PropertyParams { get; set; }
        public string? PropertyValue { get; set; }
        public string? ErrorOccurred { get; set; }
        
        public bool HasError { get {  return !string.IsNullOrEmpty(ErrorOccurred); } }

        public ExternalCodifyRequestEventArgs() : base(false)
        {
            SectionName = string.Empty;
            PropertyName = string.Empty;
            PropertyKey = string.Empty;
            PropertyParams = null;
            PropertyValue = string.Empty;
            ErrorOccurred = string.Empty;   
        }

        public ExternalCodifyRequestEventArgs(string sectionName, string propertyName, string propertyKey) : base(false)
        {
            SectionName = sectionName;
            PropertyName = propertyName;
            PropertyKey = propertyKey;
            PropertyParams = null;
            PropertyValue = string.Empty;
            ErrorOccurred = string.Empty;
        }

        public ExternalCodifyRequestEventArgs(string sectionName, string propertyName, string propertyKey, Dictionary<string, object> propertyParams) : base(false)
        {
            SectionName = sectionName;
            PropertyName = propertyName;
            PropertyKey = propertyKey;
            PropertyParams = new Dictionary<string, object>(propertyParams);
            PropertyValue = string.Empty;
            ErrorOccurred = string.Empty;
        }       

    }

}
