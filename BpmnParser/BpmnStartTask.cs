namespace bjorndahl.Parsers
{
    public class BpmnStartTask : BpmnTask
    {
        public BpmnStartTask(string id, string name) : base(id, name) 
        {
            _elementType = "startEvent";
            _taskType = BpmnTaskTypes.StartTask;
        }
    }
}