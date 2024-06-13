using System.Text.Json.Serialization;

namespace Choco.Backend.Api.Data.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PaymentStatus
{
    Pending,
    Paid
}