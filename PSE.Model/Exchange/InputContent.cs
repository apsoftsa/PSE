namespace PSE.Model.Exchange
{

    public record InputContent
    {

        public string? ContentType { get; init; }
        public string? ContentDisposition { get; init; }
        public long Length { get; init; }
        public string? Name { get; init; }
        public string? FileName { get; init; }
        public string? Content { get; init; }

    }

}
