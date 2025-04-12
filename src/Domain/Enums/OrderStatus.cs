using System.ComponentModel;

namespace Domain.Enums;

public enum OrderStatus
{
    /// <summary>
    /// Solicitado
    /// </summary>
    [Description("Solicitado")]
    Created = 0,

    /// <summary>
    /// Aguardando pagamento
    /// </summary>
    [Description("Aguardando pagamento")]
    AwaitingPayment = 1,

    /// <summary>
    /// Pago
    /// </summary>
    [Description("Pago")]
    Paid = 2,

    /// <summary>
    /// Em preparo"
    /// </summary>
    [Description("Em preparo")]
    InPreparation = 3,

    /// <summary>
    /// Pronto
    /// </summary>
    [Description("Pronto")]
    Done = 4,

    /// <summary>
    /// Enviando
    /// </summary>
    [Description("Enviando")]
    Delivery = 5,

    /// <summary>
    /// Finalizado
    /// </summary>
    [Description("Finalizado")]
    Finished = 6,

    /// <summary>
    /// Cancelado
    /// </summary>
    [Description("Cancelado")]
    Canceled = 7
}