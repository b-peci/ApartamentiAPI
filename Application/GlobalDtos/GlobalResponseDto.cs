namespace Application.GlobalDtos;

public class GlobalResponseDto<T>
{
    public string Message { get; set; }
    public bool IsSuccess { get; set; }
    public T Data { get; set; }
    public GlobalResponseDto(string message,  bool isSuccess, T data = default)
    {
        Message = message;
        IsSuccess = isSuccess;
        Data = data;
    }
}