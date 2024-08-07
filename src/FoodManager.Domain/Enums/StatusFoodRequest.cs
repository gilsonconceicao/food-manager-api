using System.ComponentModel;

namespace FoodManager.Domain.Enums;
public enum OrderStatusEnum
{
    [Description("Recebido")]
    Received = 0,

    [Description("Em preparo")]
    Inpreparation = 1,

    [Description("Enviado")]
    sent = 2,

    [Description("Enviado")]
    Delivered = 3 
}