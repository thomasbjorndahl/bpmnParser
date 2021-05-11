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
        private XmlTextReader _reader;
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
                _reader = new XmlTextReader(new StringReader(_diagramXml));
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
                            if ((_reader.Name ?? "").Equals("bpmn:startEvent", StringComparison.OrdinalIgnoreCase))
                            {
                                //Never mind start events
                                //Starter tasks (circle at the start)
                                //activeTask = template.SetDiagramTask(reader.GetAttribute("id"), reader.GetAttribute("name")); 
                                activeStartTask = _tasks.AddAndReturn(new BpmnStartTask(_reader.GetAttribute("id"), _reader.GetAttribute("name"))) as BpmnStartTask;
                                activeTask = activeStartTask;
                            }
                            else if ((_reader.Name ?? "").Equals("bpmn:userTask", StringComparison.OrdinalIgnoreCase))
                            {
                                //A manual task by the user
                                activeTask = _tasks.AddAndReturn(new BpmnUserTask(_reader.GetAttribute("id"), _reader.GetAttribute("name")));

                            }
                            else if ((_reader.Name ?? "").Equals("bpmn:sendTask", StringComparison.OrdinalIgnoreCase))
                            {
                                //A manual task by the user
                                activeTask = _tasks.AddAndReturn(new BpmnUserTask(_reader.GetAttribute("id"), _reader.GetAttribute("name")));

                            }
                            else if ((_reader.Name ?? "").Equals("bpmn:serviceTask", StringComparison.OrdinalIgnoreCase))
                            {
                                //A service task (process task)
                                activeTask = _tasks.AddAndReturn(new BpmnServiceTask(_reader.GetAttribute("id"), _reader.GetAttribute("name")));
                            }
                            else if ((_reader.Name ?? "").Equals("bpmn:exclusiveGateway", StringComparison.OrdinalIgnoreCase))
                            {
                                //Approval task
                                activeTask = _tasks.AddAndReturn(new BpmnExcluciveGatewayTask(_reader.GetAttribute("id"), _reader.GetAttribute("name")));
                                //activeTask = decission;
                            }
                            else if ((_reader.Name ?? "").Equals("bpmn:incoming", StringComparison.OrdinalIgnoreCase))
                            {
                                if(null != activeTask)
                                {
                                    getOnNext = (value) =>
                                    {
                                        if (!string.IsNullOrEmpty(value))
                                        {
                                            activeTask.AddIncomming(value);
                                        }

                                    };
                                }
                            }
                            else if ((_reader.Name ?? "").Equals("bpmn:outgoing", StringComparison.OrdinalIgnoreCase))
                            {
                                if (null != activeTask)
                                {
                                    getOnNext = (value) =>
                                    {
                                        if (!string.IsNullOrEmpty(value))
                                        {
                                            activeTask.AddOutgoing(value);
                                        }

                                    };
                                    
                                }
                            }
                            else if((_reader.Name ?? "").Equals("bpmn:textAnnotation", StringComparison.OrdinalIgnoreCase))
                            {
                                activeTask = _tasks.AddAndReturn(new BpmnTextAnnotation(_reader.GetAttribute("id"),""));
                                
                            }
                            else if ((_reader.Name ?? "").Equals("bpmn:text", StringComparison.OrdinalIgnoreCase))
                            {
                                if(null != activeTask && activeTask is BpmnTextAnnotation)
                                {
                                    getOnNext = (value) =>
                                    {
                                        if (!string.IsNullOrEmpty(value))
                                        {
                                            ((BpmnTextAnnotation)activeTask).Text = value;
                                        }

                                    };
                                }
                                
                            }
                            else if ((_reader.Name ?? "").Equals("bpmn:sequenceFlow", StringComparison.OrdinalIgnoreCase))
                            {
                                activeTask = _tasks.AddAndReturn(new BpmnSequenceFlow(_reader.GetAttribute("id"), _reader.GetAttribute("name"),  _reader.GetAttribute("sourceRef"), _reader.GetAttribute("targetRef")));                                
                            }
                            else
                            {
                                Debug.WriteLine(_reader.Name + " not implemented");
                            }
                            break;
                    }
                }
                return true;
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
            _reader = null;
            foreach(var item in _tasks)
            {
                item.Dispose();
            }
            _tasks = null;
            _disposed = true;
        }
    }
}