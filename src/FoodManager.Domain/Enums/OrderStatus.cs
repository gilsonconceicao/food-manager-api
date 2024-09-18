using System.ComponentModel;

namespace FoodManager.Domain.Enums;

public enum OrderStatus
{
    [Description("Criado")]
    Created = 0,
    [Description("Solicitado")]
    Requested = 1,

    [Description("Aguardando confirmação")]
    AwaitingConfirmation = 2,
    
    [Description("Em preparo")]
    InPreparation = 3,
    
    [Description("Pronto")]
    Done = 4,
    
    [Description("Finalizado")]
    Finished = 5,

    [Description("Cancelado")]
    Canceled = 6
}