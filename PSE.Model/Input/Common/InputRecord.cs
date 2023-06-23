using FileHelpers;
using PSE.Model.Input.Interfaces;

namespace PSE.Model.Input.Common
{

    public abstract class InputRecord : IInputRecord
    {

        [FieldHidden]
        public string RecordType 
        { 
            get
            {
                string _recordType = "";
                if(string.IsNullOrEmpty(MessageType_1) == false && MessageType_1.Length >= 3)
                    _recordType = MessageType_1[..3].ToUpper();
                return _recordType;
            }
            private set { }
        }

        [FieldFixedLength(6)]
        [FieldTrim(TrimMode.Both)]
        public string MessageType_1 { get; set; }

        protected InputRecord(string messageType)
        {
            MessageType_1 = messageType;
        }

        protected InputRecord(InputRecord source)
        {
            MessageType_1 = source.MessageType_1;
        }

    }

    public abstract class InputRecordA : InputRecord, IInputRecordA
    {

        [FieldFixedLength(5)]
        [FieldTrim(TrimMode.Both)]
        public string Block_2 { get; set; }

        [FieldFixedLength(8)]
        [FieldTrim(TrimMode.Both)]
        public string CustomerNumber_3 { get; set; }

        protected InputRecordA(string messageType) : base(messageType)
        {
            Block_2 = "";
            CustomerNumber_3 = "";
        }

        protected InputRecordA(InputRecordA source) : base(source)
        {
            Block_2 = source.Block_2;
            CustomerNumber_3 = source.CustomerNumber_3;
        }

    }

    public abstract class InputRecordB : InputRecord, IInputRecordB
    {
 
        [FieldFixedLength(8)]
        [FieldTrim(TrimMode.Both)]
        public string CustomerNumber_2 { get; set; }

        [FieldFixedLength(8)]
        [FieldTrim(TrimMode.Both)]
        public string Grouping_3 { get; set; }

        protected InputRecordB(string messageType) : base(messageType)
        {
            CustomerNumber_2 = "";
            Grouping_3 = "";
        }

        protected InputRecordB(InputRecordB source) : base(source)
        {
            CustomerNumber_2 = source.CustomerNumber_2;
            Grouping_3 = source.Grouping_3;
        }

    }

}
