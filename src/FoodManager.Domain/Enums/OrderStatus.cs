using System.ComponentModel;

namespace FoodManager.Domain.Enums;

public enum OrderStatus
{
    [Description("Solicitado")]
    Requested = 0,

    [Description("Aguardando confirmação")]
    AwaitingConfirmation = 1,
    
    [Description("Em preparo")]
    InPreparation = 2,
    
    [Description("Pronto")]
    Done = 3,
    
    [Description("Finalizado")]
    Finished = 4,

    [Description("Cancelado")]
    Canceled = 5
}