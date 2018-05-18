using System.Collections.Generic;

namespace ObjectManager.Rest.Interfaces
{
    public class EventHandlerStatus
    {
        public EventHandlerStatus() { }
        public EventHandlerStatus(string message)
        {
            this.Message = message;
            this.Success = false;
        }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
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