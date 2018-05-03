using System.Collections.Generic;

namespace ObjectManager.Rest.Interfaces
{
    public class EventHandlerStatus
    {
        public string Message { get; set; }
        public bool Success { get; set; }
    }
    public class ObjectUpdateResult
    {
        public IEnumerable<EventHandlerStatus> EventHandlerStatuses { get; set; }
    }
}