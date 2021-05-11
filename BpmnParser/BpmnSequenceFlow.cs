namespace bjorndahl.Parsers
{
    public class BpmnSequenceFlow : BpmnTask
    {    
        private string _sourceRef;
        private string _targetRef;

        public BpmnSequenceFlow(string id, string name, string sourceRef, string targetRef) : base(id, name)
        {           
            this._sourceRef = sourceRef;
            this._targetRef = targetRef;
            _elementType = "sequenceFlow";
            _taskType = BpmnTaskTypes.SequenceFlow;
        }

        public string SourceRef { get { return _sourceRef; } }
        public string TargetRef { get { return _targetRef; } }
    }
}