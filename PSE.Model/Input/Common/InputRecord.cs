using System.Text;
using System.Reflection;
using FileHelpers;
using PSE.Model.Input.Interfaces;
using Newtonsoft.Json.Linq;

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
        [FieldOrder(1)]
        public string MessageType_1 { get; set; }

        protected InputRecord(string messageType)
        {
            MessageType_1 = messageType;
        }

        protected InputRecord(InputRecord source)
        {
            MessageType_1 = source.MessageType_1;
        }

        public override string ToString()
        {
            StringBuilder _sb = new();
            //_sb.Append("----------------------------------------");
            _sb.Append(Environment.NewLine);
            _sb.Append(RecordType);
            _sb.Append(Environment.NewLine);
            _sb.Append("----------------------------------------");
            _sb.Append(Environment.NewLine);
            object? _objOrder;
            int _orderIndex;
            List<(string name, int order, object? value)> _propsOrderAndValue = new();
            foreach (PropertyInfo _property in this.GetType().GetProperties())
            {
                _orderIndex = 0;
                _objOrder = null;
                if (_property.CustomAttributes != null && _property.CustomAttributes.Any()
                && _property.CustomAttributes.Last().NamedArguments != null && _property.CustomAttributes.Last().ConstructorArguments.Any()
                && _property.CustomAttributes.Last().ConstructorArguments.Last().Value != null)
                    _objOrder = _property.CustomAttributes.Last().ConstructorArguments.Last().Value;
                if(_objOrder != null && int.TryParse(_objOrder.ToString(), out _orderIndex))
                    _propsOrderAndValue.Add((_property.Name, _orderIndex, _property.GetValue(this, null)));
            }
            foreach(var (name, order, value) in _propsOrderAndValue.OrderBy(_ob => _ob.order))
            {
                _sb.Append(name);
                _sb.Append(": ");
                _sb.Append(value);
                _sb.Append(Environment.NewLine);
            }
            /*
            foreach (PropertyInfo _property in this.GetType().GetProperties())
            {
                _sb.Append(_property.Name);
                _sb.Append(": ");
                _sb.Append(_property.GetValue(this, null));
                _sb.Append(Environment.NewLine);
            }
            */            
            _sb.Append("----------------------------------------");
            _sb.Append(Environment.NewLine);
            _sb.Append(Environment.NewLine);
            return _sb.ToString();
        }

    }

    public abstract class InputRecordA : InputRecord, IInputRecordA
    {

        [FieldFixedLength(5)]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(2)]
        public string Block_2 { get; set; }

        [FieldFixedLength(8)]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(3)]
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
        [FieldOrder(2)] 
        public string CustomerNumber_2 { get; set; }

        [FieldFixedLength(8)]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(3)]
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
