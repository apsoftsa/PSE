using FileHelpers;
using PSE.Model.Input.Common;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Input.Models
{

    [FixedLengthRecord(FixedMode.AllowMoreChars)]
    [IgnoreEmptyLines()]
    public class POR : InputRecordB
    {

        // to-do ...
        [FieldFixedLength(898)]
        public string? ToDefine { get; set; }

        public POR() : base(INPUT_POR_MSG_TYPE)
        {
        }

        public POR(POR source) : base(source)
        {
        }

    }

}