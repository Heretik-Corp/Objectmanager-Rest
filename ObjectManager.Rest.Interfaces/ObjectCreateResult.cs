using System.Collections.Generic;

namespace ObjectManager.Rest.Interfaces
{
    public class ObjectCreateResult
    {
        public IEnumerable<EventHandlerStatus> EventHandlerStatuses { get; set; } = new List<EventHandlerStatus>();
        public RelativityObject Object { get; set; }
        public ObjectCreateResult(string message)
        {
            this.EventHandlerStatuses = new List<EventHandlerStatus>
            {
                new EventHandlerStatus(message)
            };
        }
        public ObjectCreateResult() { }
    }
}