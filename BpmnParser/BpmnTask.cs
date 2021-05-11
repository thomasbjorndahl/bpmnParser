using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace bjorndahl.Parsers
{
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
        protected BpmnTaskTypes _taskType = BpmnTaskTypes.None;

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

        public BpmnTaskTypes TaskType { get { return _taskType; } }

        public BpmnTaskContainer Children
        {
            get
            {
                return _children;
            }
        }
    }
}