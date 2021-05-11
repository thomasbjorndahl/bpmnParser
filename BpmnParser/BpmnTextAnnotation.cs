namespace bjorndahl.Parsers
{
    public class BpmnTextAnnotation : BpmnTask
    {
        public string Text { get; set; }

        public BpmnTextAnnotation(string id, string name) : base(id, name)
        {
            _elementType = "textAnnotation";
        }
    }
}