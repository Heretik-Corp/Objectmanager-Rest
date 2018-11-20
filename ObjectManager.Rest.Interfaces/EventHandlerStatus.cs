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
}