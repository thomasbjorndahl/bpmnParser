namespace bjorndahl.Parsers
{

    public class BpmnServiceTask : BpmnTask 
    {
        public BpmnServiceTask(string id, string name) : base(id, name) 
        {
            _elementType = "serviceTask";
            _taskType = BpmnTaskTypes.ServiceTask;
        }        
    }
}