using System.Text.Json.Serialization;

namespace Api.Data.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderStatus
{
    Pending, // This is the initial status when the customer has placed an order, but it has not been processed yet.
    Processing, // The order has been received and is being processed by the merchant.
    Shipped, // The order has been shipped and is on its way to the customer.
    Delivered, // The order has been successfully delivered to the customer.
    Cancelled, // The order has been cancelled, either by the customer or the merchant.
    OnHold, // The order has been placed on hold, usually due to some issue with the payment or the customer's information.
}