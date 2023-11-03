namespace PSE.Model.Common
{

    public class AppSettings
    {

        public string DecoderStringConnection { get; set; }
        public bool DecoderEnabled { get; set; }

        public AppSettings()
        {
            DecoderStringConnection = string.Empty;
            DecoderEnabled = false;
        }

    }

}



