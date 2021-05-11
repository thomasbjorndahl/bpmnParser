using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace bjorndahl.Parsers
{
    internal class BpmnReader : IDisposable
    {
        private string _diagramXml;
        private bool _disposed;
        private BpmnTaskContainer _tasks;

        public BpmnReader(string diagramXml)
        {
            this._diagramXml = diagramXml;
            _tasks = new BpmnTaskContainer();
        }

        public BpmnTaskContainer Tasks { get { return _tasks; } }

        internal bool Load()
        {
            try
            {
                using (var _reader = new XmlTextReader(new StringReader(_diagramXml)))
                {
                    BpmnTask activeTask = null;
                    BpmnStartTask activeStartTask = null;
                    Action<string> getOnNext = null;
                
                    while (_reader.Read())
                    {
                        switch (_reader.NodeType)
                        {
                            default:
                                if (null != getOnNext)
                                {
                                    getOnNext(_reader.Value);
                                    getOnNext = null;
                                }
                                break;
                            case XmlNodeType.Element:
                                switch( (_reader.Name ?? "").ToLowerInvariant())
                                {
                                    case "bpmn:startevent":
                                        activeStartTask = _tasks.AddAndReturn(new BpmnStartTask(_reader.GetAttribute("id"), _reader.GetAttribute("name"))) as BpmnStartTask;
                                        activeTask = activeStartTask;
                                        break;
                                    case "bpmn:usertask":
                                        activeTask = _tasks.AddAndReturn(new BpmnUserTask(_reader.GetAttribute("id"), _reader.GetAttribute("name")));
                                        break;
                                    case "bpmn:sendtask":
                                        activeTask = _tasks.AddAndReturn(new BpmnSendTask(_reader.GetAttribute("id"), _reader.GetAttribute("name")));
                                        break;
                                    case "bpmn:servicetask":
                                        activeTask = _tasks.AddAndReturn(new BpmnServiceTask(_reader.GetAttribute("id"), _reader.GetAttribute("name")));
                                        break;
                                    case "bpmn:exclusivegateway":
                                        activeTask = _tasks.AddAndReturn(new BpmnExcluciveGatewayTask(_reader.GetAttribute("id"), _reader.GetAttribute("name")));
                                        break;
                                    case "bpmn:outgoing":
                                    case "bpmn:incoming":
                                        if (null != activeTask)
                                        {
                                            getOnNext = (value) =>
                                            {
                                                if (!string.IsNullOrEmpty(value))
                                                {
                                                    if(_reader.Name.Equals("bpmn:incoming", StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        activeTask.AddIncomming(value);
                                                    }
                                                    if (_reader.Name.Equals("bpmn:outgoing", StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        activeTask.AddOutgoing(value);
                                                    }                                                    
                                                }
                                            };
                                        }
                                        break;
                                    case "bpmn:textannotation":
                                        activeTask = _tasks.AddAndReturn(new BpmnTextAnnotation(_reader.GetAttribute("id"), ""));
                                        break;
                                    case "bpmn:text":
                                        if (null != activeTask && activeTask is BpmnTextAnnotation)
                                        {
                                            getOnNext = (value) =>
                                            {
                                                if (!string.IsNullOrEmpty(value))
                                                {
                                                    ((BpmnTextAnnotation)activeTask).Text = value;
                                                }
                                            };
                                        }
                                        break;
                                    case "bpmn:sequenceflow":
                                        activeTask = _tasks.AddAndReturn(new BpmnSequenceFlow(_reader.GetAttribute("id"), _reader.GetAttribute("name"), _reader.GetAttribute("sourceRef"), _reader.GetAttribute("targetRef")));
                                        break;
                                }                               
                                break;
                        }
                    }
                    return true;
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        

        /// <summary>
        /// Disposing the parser
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            _diagramXml = null;
            foreach(var item in _tasks)
            {
                item.Dispose();
            }
            _tasks = null;
            _disposed = true;
        }
    }
}