namespace bjorndahl.Parsers
{
    public enum BpmnTaskTypes : short 
    {
        None = 0,
        SendTask = 1,
        StartTask = 2,
        ServiceTask = 3,
        UserTask = 4,
        TextAnnotation = 5,       
        SequenceFlow = 6,
        ExclusiveGateway = 7,
    }
}