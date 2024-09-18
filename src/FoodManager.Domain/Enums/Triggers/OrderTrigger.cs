namespace FoodManager.Domain.Enums.Triggers;

public enum OrderTrigger
{
    Process = 0,
    ConfirmOrder = 1, 
    Cancel = 2,
    CheckHowDone = 3,
    Finish = 4,
}