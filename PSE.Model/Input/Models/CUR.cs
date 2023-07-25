using FileHelpers;
using PSE.Model.Input.Common;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Input.Models
{

    [FixedLengthRecord(FixedMode.AllowMoreChars)]
    [IgnoreEmptyLines()]
    public class CUR : InputRecordB
    {

        // to-do ...
        [FieldFixedLength(898)]
        [FieldOrder(4)]
        public string? ToDefine { get; set; }

        public CUR() : base(INPUT_CUR_MSG_TYPE)
        {
        }

        public CUR(CUR source) : base(source)
        {
        }

    }

}