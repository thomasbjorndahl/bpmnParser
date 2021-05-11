namespace bjorndahl.Parsers
{
    public class BpmnExclusiveGatewayTask : BpmnTask
    {
        public BpmnExclusiveGatewayTask(string id, string name) : base(id, name) 
        {
            _elementType = "exclusiveGateway";
            _taskType = BpmnTaskTypes.ExclusiveGateway;
        }
    }
}