namespace bjorndahl.Parsers
{
    /// <summary>
    /// User task
    /// </summary>
    public class BpmnUserTask : BpmnTask
    {
        public BpmnUserTask(string id, string name) : base(id, name) 
        {
            _elementType = "userTask";
            _taskType = BpmnTaskTypes.UserTask;
        }
    }
}