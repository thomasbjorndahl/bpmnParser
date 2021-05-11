using System;
using System.Collections.Generic;

namespace bjorndahl.Parsers
{
    public class BpmnTaskContainer : List<BpmnTask>, IDisposable
    {
        private BpmnTask _baseItem;

        public BpmnTaskContainer() { }

        public BpmnTaskContainer(BpmnTask baseItem) : this()
        {
            _baseItem = baseItem;
        }

        public BpmnTask AddAndReturn(BpmnTask item)
        {
            Add(item);
            return item;
        }

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