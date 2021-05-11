using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace bjorndahl.Parsers
{
    /// <summary>
    /// The main class for the BpmnParser
    /// </summary>
    public class BpmnParser : IDisposable
    {
        private string _diagramXml;
        private bool _disposed;
        private bool _diagramIsValid;
        private Exception _diagramException;
        private BpmnReader _reader;
        private List<BpmnTask> _organized = new List<BpmnTask>();

        #region constructors
        /// <summary>
        /// Basic constructor. Protected
        /// </summary>
        protected BpmnParser() { }

        /// <summary>
        /// Constructor with diagram XML
        /// </summary>
        /// <param name="diagramXml"></param>
        public BpmnParser(string diagramXml) : base()
        {
            if (string.IsNullOrEmpty(diagramXml))
            {
                _diagramIsValid = false;
                _diagramException = new ApplicationException("The input was null or empty. Please load a valid BPMN.io diagram XML");
            }
            else
            {
                _diagramXml = diagramXml;
                _diagramIsValid = LoadReader();
                if (_diagramIsValid)
                {
                    Parse();
                }
            }            
        }


        private void Parse()
        {
            //First, find all start elements
            var startElements = AllTasks.Where(t => t is BpmnStartTask).ToList();

            if(null != startElements && startElements.Count() > 0)
            {
                foreach(var elm in startElements)
                {
                    FindChildren(elm);
                }
            }

            _organized = startElements;

        }

        /// <summary>
        /// Getting the children elements based on the elm.Id
        /// </summary>
        /// <param name="elm">The element</param>
        private void FindChildren(BpmnTask elm)
        {
            var tasks = AllTasks.Where(t => t is BpmnSequenceFlow).Where(t => ((BpmnSequenceFlow)t).SourceRef.Equals(elm.Id,StringComparison.OrdinalIgnoreCase));
            foreach(BpmnSequenceFlow task in tasks)
            {
                var parameter = task.Name;
                if (!string.IsNullOrEmpty(task.TargetRef))
                {
                    var child = AllTasks.Where(t => t.Id.Equals(task.TargetRef, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if(null != child)
                    {
                        elm.Children.AddAndReturn(child);
                        FindChildren(child);
                    }
                    
                }
            }

        }
        #endregion
        private bool LoadReader()
        {
            try
            {
                _reader = new BpmnReader(_diagramXml);
                return _reader.Load();                               
            }
            catch(Exception ex)
            {
                _diagramException = ex;
                return false;
            }
        }

        /// <summary>
        /// The tasks
        /// </summary>
        public BpmnTaskContainer AllTasks { get { return _reader?.Tasks; } }

        /// <summary>
        /// True if the loaded diagram is valid
        /// </summary>
        public bool DiagramIsValid { get { return _diagramIsValid; } }

        /// <summary>
        /// True if the loaded diagram is valid
        /// </summary>
        public string DiagramXml { get { return _diagramXml; } }

        /// <summary>
        /// The exception if anything goes wrong
        /// </summary>
        public Exception Exception { get { return _diagramException; } }

        public IEnumerable<BpmnTask> Organized { get{ return _organized; } }

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
            
            if(null != _organized)
            {
                foreach (var t in _organized)
                {
                    t.Dispose();
                }
            }            
            _reader.Dispose();
            _organized = null;
            _diagramXml = null;
            _diagramIsValid = false;
            _diagramException = null;
            _disposed = true;
            _reader = null;
        }
       
    }
}
