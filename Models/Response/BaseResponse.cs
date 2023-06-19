namespace Api_Almacen.Models.Response
{
    public class BaseResponse
    {
        public bool IsSuccess { get; set; }
        public object? Data { get; set; }
        public string? Message { get; set; }
    }
}
