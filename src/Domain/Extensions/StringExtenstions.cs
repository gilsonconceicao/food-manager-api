using System.Text.Json;
using System.Text.RegularExpressions;

namespace Domain.Extensions;

public static class StringExtensions
{
    public static string RemoveSpecialCharacters(this string str)
    {
        return Regex.Replace(str, "[^a-zA-Z0-9_]+", "", RegexOptions.Compiled);
    }
    public static string TraduzirStatusDetail(string? statusDetail)
    {
        return statusDetail switch
        {
            "accredited" => "Pagamento aprovado e creditado com sucesso.",
            "partially_refunded" => "Pagamento aprovado com reembolso parcial.",
            "pending_capture" => "Pagamento autorizado aguardando captura.",
            "offline_process" => "O pagamento está sendo processado offline.",
            "pending_contingency" => "Estamos processando seu pagamento. Você será notificado por e-mail em até 2 dias úteis.",
            "pending_review_manual" => "Estamos revisando seu pagamento. Você será notificado por e-mail em até 2 dias úteis.",
            "pending_waiting_transfer" => "Aguardando a finalização do pagamento via transferência bancária.",
            "pending_waiting_payment" => "Aguardando o usuário concluir o pagamento.",
            "pending_challenge" => "Aguardando confirmação adicional do cartão (challenge).",
            "bank_error" => "Pagamento recusado por erro no banco.",
            "cc_rejected_3ds_mandatory" => "Pagamento recusado por falta de autenticação 3DS obrigatória.",
            "cc_rejected_bad_filled_card_number" => "Número do cartão inválido.",
            "cc_rejected_bad_filled_date" => "Data de validade do cartão inválida.",
            "cc_rejected_bad_filled_other" => "Dados do cartão inválidos.",
            "cc_rejected_bad_filled_security_code" => "Código de segurança do cartão inválido.",
            "cc_rejected_blacklist" => "Pagamento não autorizado.",
            "cc_rejected_call_for_authorize" => "Autorize o pagamento com o emissor do cartão.",
            "cc_rejected_card_disabled" => "Cartão desativado. Ligue para o emissor ou use outro cartão.",
            "cc_rejected_card_error" => "Não foi possível processar o pagamento com esse cartão.",
            "cc_rejected_duplicated_payment" => "Pagamento duplicado detectado. Tente outro método.",
            "cc_rejected_high_risk" => "Pagamento recusado por risco elevado. Tente outro método de pagamento.",
            "cc_rejected_insufficient_amount" => "Saldo insuficiente no cartão.",
            "cc_rejected_invalid_installments" => "Número de parcelas inválido para esse cartão.",
            "cc_rejected_max_attempts" => "Limite de tentativas excedido. Tente outro cartão.",
            "cc_rejected_other_reason" => "Pagamento não autorizado pelo emissor.",
            "cc_amount_rate_limit_exceeded" => "Pagamento excedeu o limite permitido pelo método.",
            "rejected_insufficient_data" => "Pagamento recusado por falta de informações obrigatórias.",
            "rejected_by_bank" => "Pagamento recusado pelo banco.",
            "rejected_by_regulations" => "Pagamento recusado por regulamentações.",
            "insufficient_amount" => "Pagamento recusado por valor insuficiente.",
            _ => "Motivo do erro não identificado."
        };
    }

    public static string ExtractMessage(string input)
    {
        try
        {
            var start = input.IndexOf("{");
            var json = input.Substring(start);

            using var doc = JsonDocument.Parse(json);
            var message = doc.RootElement.GetProperty("message").GetString();
            return message;
        }
        catch
        {
            return null;
        }
    }
}