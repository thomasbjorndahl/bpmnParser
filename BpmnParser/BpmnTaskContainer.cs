using System;
using System.Collections.Generic;

namespace bjorndahl.Parsers
{
    /// <summary>
    /// The container
    /// </summary>
    public class BpmnTaskContainer : List<BpmnTask>, IDisposable
    {
        private BpmnTask _baseItem;
        private string _parameter;

        public BpmnTaskContainer() { }

        public BpmnTaskContainer(BpmnTask baseItem) : this()
        {
            _baseItem = baseItem;
        }

        public BpmnTask AddAndReturn(BpmnTask item) 
        {
            return AddAndReturn(item, string.Empty);
        }

        public BpmnTask AddAndReturn(BpmnTask item, string parameter)
        {
            Add(item);
            _parameter = parameter;
            return item;
        }

        public string Parameter { get { return _parameter; } }

        public void Dispose()
        {
            foreach(var item in this)
            {
                item.Dispose();
            }
            _baseItem?.Dispose();
            GC.SuppressFinalize(this);
        }

    }
}