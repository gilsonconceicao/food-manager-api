using System.ComponentModel;

namespace Domain.Enums;

public enum OrderStatus
{
    /// <summary>
    /// Aguardando pagamento
    /// </summary>
    [Description("Aguardando pagamento")]
    AwaitingPayment = 0,

    /// <summary>
    /// Pago
    /// </summary>
    [Description("Pago")]
    Paid = 1,

    /// <summary>
    /// Em preparo"
    /// </summary>
    [Description("Em preparo")]
    InPreparation = 2,

    /// <summary>
    /// Pronto
    /// </summary>
    [Description("Pronto")]
    Done = 3,

    /// <summary>
    /// Enviando
    /// </summary>
    [Description("Enviando")]
    Delivery = 4,

    /// <summary>
    /// Finalizado
    /// </summary>
    [Description("Finalizado")]
    Finished = 5,

    /// <summary>
    /// Cancelado
    /// </summary>
    [Description("Cancelado")]
    Canceled = 6
}