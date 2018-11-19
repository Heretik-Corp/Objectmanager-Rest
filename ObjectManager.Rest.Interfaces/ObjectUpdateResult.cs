using System.Collections.Generic;

namespace ObjectManager.Rest.Interfaces
{
    public class ObjectUpdateResult
    {
        public ObjectUpdateResult() { }
        public ObjectUpdateResult(string message)
        {
            this.EventHandlerStatuses = new List<EventHandlerStatus>
            {
                new EventHandlerStatus(message)
            };
        }
        public IEnumerable<EventHandlerStatus> EventHandlerStatuses { get; set; } = new List<EventHandlerStatus>();
    }
}