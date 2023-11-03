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
            Childs = childs;
        }

    }

    [Serializable]
    public record OutputContent
    {

        public string? JsonGenerated { get; set; }
        public List<OutputLog>? Logs { get; set; }

        public OutputContent()
        {
            JsonGenerated = string.Empty;
            Logs = new List<OutputLog>();
        }

    }

}
