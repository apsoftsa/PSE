using static PSE.Model.Common.Enumerations;

namespace PSE.Model.SupportTables
{

    public class InvestmentType
    {

        public InvestmentTypes Id { get; }
        public string Code { get; }    
        public string Description { get; }
        public string Abbr { get; }
        public int Seq { get; }
        public int E185 { get; }
        public int Risk { get; }

        public InvestmentType()
        {
            Id = InvestmentTypes.UNKNOWN;
            Code = string.Empty;
            Description = string.Empty;
            Abbr = string.Empty;
            Seq = 0;
            E185 = 0;   
            Risk = 0;
        }

        public InvestmentType(InvestmentTypes id, string code, string description, string abbr, int seq, int e185, int risk)
        {
            Id = id;
            Code = code;
            Description = description;
            Abbr = abbr;
            Seq = seq;
            E185 = e185;
            Risk = risk;
        }

    }

}