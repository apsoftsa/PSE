namespace PSE.Model.Exchange
{

    [Serializable]
    public record OutputLog
    {

        public string ActivityType { get; init; }
        public string? Content { get; init; }
        public List<OutputLog>? Childs { get; init; }

        public OutputLog(string activityType, string? content, List<OutputLog>? childs = null)
        {
            ActivityType = activityType;
            Content = content;
            Childs = new List<OutputLog>();    
            if (childs != null && childs.Count > 0) {
                foreach (var child in childs) {
                    Childs.Add(new OutputLog(child));
                }
            }
        }

    }

    public interface IOutputContent {

        string? JsonGenerated { get; set; }

        List<OutputLog>? Logs { get; set; }

    }

    [Serializable]
    public record OutputContent : IOutputContent
    {

        public string? JsonGenerated { get; set; }
        public List<OutputLog>? Logs { get; set; }

        public OutputContent()
        {
            JsonGenerated = string.Empty;
            Logs = new List<OutputLog>();
        }

        public OutputContent(IOutputContent source) {
            JsonGenerated = source.JsonGenerated;
            Logs = new List<OutputLog>();
            if (source.Logs != null && source.Logs.Any()) {
                foreach (var log in source.Logs) {
                    Logs.Add(new OutputLog(log.ActivityType, log.Content, log.Childs));
                }
            }
        }

    }

    [Serializable]
    public record OutputContentWithFile : OutputContent, IOutputContent {

        public string FileName { get; set; }

        public byte[]? FileContent { get; set; }

        public OutputContentWithFile() : base() {
            FileName = string.Empty;
            FileContent = null;
        }

        public OutputContentWithFile(IOutputContent source) : base(source) {
            FileName = string.Empty;
            FileContent = null;
        }

    }

}
