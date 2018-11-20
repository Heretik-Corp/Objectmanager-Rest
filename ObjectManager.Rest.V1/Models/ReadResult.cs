using System.Collections.Generic;
using ObjectManager.Rest.Interfaces;

namespace ObjectManager.Rest.V1.Models
{
    internal class ReadResult
    {
        public RelativityObjectV1 RelativityObject { get; set; }
        public IEnumerable<EventHandlerStatus> EventHandlerStatuses { get; set; }
    }
}
