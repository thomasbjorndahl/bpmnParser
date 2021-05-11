namespace bjorndahl.Parsers
{
    public class BpmnSendTask : BpmnTask
    {
        public BpmnSendTask(string id, string name) : base(id, name)
        {
            _elementType = "sendTask";
        }
    }
}