using System.ComponentModel;

namespace Domain.Enums;

public enum OrderStatus
{
    /// <summary>
    /// Criado
    /// </summary>
    [Description("Criado")]
    Created = 0,

    /// <summary>
    /// Solicitado
    /// </summary>
    [Description("Solicitado")]
    Requested = 1,

    /// <summary>
    /// Aguardando confirmação
    /// </summary>
    [Description("Aguardando confirmação")]
    AwaitingConfirmation = 2,

    /// <summary>
    /// Em preparo
    /// </summary>
    [Description("Em preparo")]
    InPreparation = 3,

    /// <summary>
    /// Pronto
    /// </summary>
    [Description("Pronto")]
    Done = 4,

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