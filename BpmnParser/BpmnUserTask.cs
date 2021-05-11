namespace bjorndahl.Parsers
{
    public class BpmnUserTask : BpmnTask
    {
        public BpmnUserTask(string id, string name) : base(id, name) 
        {
            _elementType = "userTask";
        }
    }
}