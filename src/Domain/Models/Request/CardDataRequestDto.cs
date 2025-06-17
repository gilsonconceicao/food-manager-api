namespace Domain.Models.Request;
#nullable disable

public class CardDataRequestDto
{
  public string CardNumber {get; set;}
  public string CardHolderNamenoCartão {get; set;}
  public string IdentificationType {get; set;}
  public string IdentificationNumber {get; set;}
  public string ExpirationMonth {get; set;}
  public string ExpirationYear {get; set;}
  public string Cvv {get; set;} 
}