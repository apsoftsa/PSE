using PSE.Model.Output.Interfaces;

namespace PSE.Model.Output.Models
{
    public class Settled : ISettled
    {

        public string Date { get; set; }

        public string Time { get; set; }

        public Settled()
        {
            Date = string.Empty;
            Time = string.Empty;
        }

        public Settled(ISettled source)
        {
            Date = source.Date;
            Time = source.Time;
        }

        public Settled(string date, string time)
        {
            Date = date;
            Time = time;
        }

    }

}
