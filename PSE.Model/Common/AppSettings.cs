namespace PSE.Model.Common
{

    public class AppSettings
    {

        public string DecoderStringConnection { get; set; }
        public bool DecoderEnabled { get; set; }

        public string DictionariesPath { get; set; }

        public string FamUrl { get; set; }

        public AppSettings()
        {
            DecoderStringConnection = string.Empty;
            DecoderEnabled = false;
            DictionariesPath = string.Empty; 
            FamUrl = string.Empty;  
        }

    }

}



