using System.ComponentModel;

namespace PSE.Model.Events
{

    public class ExternalCodifyRequestEventArgs : CancelEventArgs
    {

        public string SectionName { get; } 
        public string PropertyName { get; }
        public string PropertyKey { get; }
        public string? PropertyValue { get; set; }
        public string? ErrorOccurred { get; set; }
        public bool HasError { get {  return !string.IsNullOrEmpty(ErrorOccurred); } }

        public ExternalCodifyRequestEventArgs() : base(false)
        {
            SectionName = string.Empty;
            PropertyName = string.Empty;
            PropertyKey = string.Empty;
            PropertyValue = string.Empty;
            ErrorOccurred = string.Empty;   
        }

        public ExternalCodifyRequestEventArgs(string sectionName, string propertyName, string propertyKey) : base(false)
        {
            SectionName = sectionName;
            PropertyName = propertyName;
            PropertyKey = propertyKey;
            PropertyValue = string.Empty;
            ErrorOccurred = string.Empty;
        }

    }

}
