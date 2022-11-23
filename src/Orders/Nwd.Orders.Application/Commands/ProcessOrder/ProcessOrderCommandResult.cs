namespace Nwd.Orders.Application.Commands.ProcessOrder
{
    public class ProcessOrderCommandResult
    {
        public ProcessOrderCommandResult(string orderId, string status, string message)
        {
            OrderId = orderId;
            Status = status;
            Message = message;
        }

        public string OrderId { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
