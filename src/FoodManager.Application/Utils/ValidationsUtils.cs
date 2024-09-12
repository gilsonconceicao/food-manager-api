using System.Text.RegularExpressions;

namespace FoodManager.Application.Utils;

/// <summary>
/// Realiza a validação do CPF
/// </summary>
static class ValidationsUtils
{
    public static bool IsValidRegistrationNumber(string cpf)
    {
        int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        string tempRegistrationNumber;
        string digit;
        int sum;
        int rest;

        if (string.IsNullOrEmpty(cpf))
            return false;

        cpf = cpf.Trim();
        cpf = RemoveSpecialCharacters(cpf);

        if (cpf.Length != 11)
            return false;
        tempRegistrationNumber = cpf.Substring(0, 9);
        sum = 0;

        for (int i = 0; i < 9; i++)
            sum += int.Parse(tempRegistrationNumber[i].ToString()) * multiplicador1[i];
        rest = sum % 11;
        if (rest < 2)
            rest = 0;
        else
            rest = 11 - rest;
        digit = rest.ToString();
        tempRegistrationNumber = tempRegistrationNumber + digit;
        sum = 0;
        for (int i = 0; i < 10; i++)
            sum += int.Parse(tempRegistrationNumber[i].ToString()) * multiplicador2[i];
        rest = sum % 11;
        if (rest < 2)
            rest = 0;
        else
            rest = 11 - rest;
        digit = digit + rest.ToString();
        return cpf.EndsWith(digit);
    }

    public static string RemoveSpecialCharacters(string str)
    {
        return Regex.Replace(str, "[^a-zA-Z0-9_]+", "", RegexOptions.Compiled);
    }
}