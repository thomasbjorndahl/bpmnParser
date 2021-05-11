using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace bjorndahl.Parsers
{
    public class BpmnStartTask : BpmnTask
    {
        public BpmnStartTask(string id, string name) : base(id, name) 
        {
            _elementType = "startEvent";
        }
    }
    public class BpmnUserTask : BpmnTask
    {
        public BpmnUserTask(string id, string name) : base(id, name) 
        {
            _elementType = "userTask";
        }
    }
    public class BpmnServiceTask : BpmnTask 
    {
        public BpmnServiceTask(string id, string name) : base(id, name) 
        {
            _elementType = "serviceTask";
        }        
    }

    public class BpmnExcluciveGatewayTask : BpmnTask
    {
        public BpmnExcluciveGatewayTask(string id, string name) : base(id, name) 
        {
            _elementType = "exclusiveGateway";
        }
    }

    [DebuggerDisplay("{_id} - {_name} - {_elementType}")]
    public abstract class BpmnTask : IDisposable
    {
        private string _id;
        private string _name;
        private List<string> _incomming = new List<string>();
        private List<string> _outgoing = new List<string>();
        private BpmnTaskContainer _children = new BpmnTaskContainer();
        protected string _elementType = "";
        private const string _prefix = "bpmn";

        protected BpmnTask(string id)
        {
            this._id = id;
        }
        public BpmnTask(string id, string name) : this(id)
        {            
            this._name = name;
        }

        public void Dispose()
        {
            _id = null;
            _name = null;
            _incomming = null;
            _outgoing = null;
            _children?.Dispose();
            _children = null;
            GC.SuppressFinalize(this);
        }

        internal void AddIncomming(string value)
        {
            _incomming.Add(value);
        }

        internal void AddOutgoing(string value)
        {
            _outgoing.Add(value);
        }

        public IEnumerable<string> Incomming { get { return _incomming; } }
        public IEnumerable<string> Outgoing { get { return _outgoing; } }

        public string Id { get { return _id; } }
        public string Name { get { return _name; } }
        public string ElementType { get { return _prefix + ":" + _elementType; } }

        public BpmnTaskContainer Children
        {
            get
            {
                return _children;
            }
        }
    }
}