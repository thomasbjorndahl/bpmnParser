namespace bjorndahl.Parsers
{
    public class BpmnExcluciveGatewayTask : BpmnTask
    {
        public BpmnExcluciveGatewayTask(string id, string name) : base(id, name) 
        {
            _elementType = "exclusiveGateway";
        }
    }
}