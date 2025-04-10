using Domain.Models;
using System.Globalization;
using System.Text;

public static class EmailTemplates
{
    public static string DailyReportMergeUsersHtml(User admin, DateTime data, List<User> users, string envDisplay)
    {
        var sb = new StringBuilder();

        if (users == null || users.Count == 0)
        {
            sb.Append($@"
            <div style='text-align: center; padding: 40px 20px; background-color: #f8f9fa; border-radius: 8px;'>
                <p style='font-size: 50px; margin: 0;'>🕵️‍♂️</p>
                <h3 style='color: #2e86de; font-size: 20px; margin-top: 10px;'>Nenhum novo usuário foi encontrado hoje!</h3>
                <p style='color: #666; font-size: 15px;'>Tudo tranquilo por aqui. Ninguém novo apareceu no sistema em <strong>{data:dd/MM/yyyy}</strong>.</p>
            </div>");
        }
        else
        {
            foreach (var user in users)
            {
                sb.Append($@"
                <div style='border-left: 4px solid #2e86de; padding-left: 15px; margin-bottom: 20px;'>
                    <p><strong>👤 Nome:</strong> {user.Name}</p>
                    <p><strong>📧 Email:</strong> <span style='color: #2c3e50;'>{user.Email}</span></p>
                    <p><strong>📞 Número de telefone:</strong> {user.PhoneNumber ?? "Não informado"}</p>
                    <p><strong>📦 Quantidade de pedidos:</strong> {user.Orders.Count()}</p>
                    <p><strong>🆔 ID:</strong> {user.Id.ToString() ?? "-"}</p>
                </div>
                <hr>");
            }
        }

        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <title>Relatório de Usuários</title>
</head>
<body style='margin: 0; padding: 0; font-family: Segoe UI, Tahoma, Geneva, Verdana, sans-serif; background-color: #f0f2f5;'>

    <div style='max-width: 700px; margin: 40px auto; background-color: #ffffff; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 12px rgba(0,0,0,0.1);'>
        <div style='background-color: #2e86de; color: white; padding: 30px 40px;'>
            <h1 style='margin: 0; font-size: 28px;'>👋 Olá, {admin.Name}!</h1>
            <p style='margin: 5px 0 0;'>Aqui está o relatório dos usuários sincronizados em {envDisplay}.</p>
        </div>

        <div style='padding: 30px 40px;'>
            <h2 style='color: #2e86de; font-size: 20px; margin-bottom: 20px;'>📋 Detalhes dos Usuários</h2>
            <p>Data: <strong>{data:dd/MM/yyyy}</strong></p>
            {sb}
            <p style='color: #888; font-size: 13px; margin-top: 40px;'>Este é um e-mail automático. Não é necessário respondê-lo.</p>
        </div>
    </div>

</body>
</html>";
    }
}
