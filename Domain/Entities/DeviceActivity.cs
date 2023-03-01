using Domain.Enums;

namespace Domain.Entities;

public class DeviceActivity : BaseEntity
{
    public string DeviceName { get; set; }
    public string RefreshToken { get; set; }
    public string IPAddress { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public DeviceStatus Status { get; set; }

    public Guid UserId { get; set; }
}