namespace FoodManager.Application.Users.Dtos
{
    #nullable disable
    public class CreatedByDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string UserRegistrationNumber { get; set; }
    }
}