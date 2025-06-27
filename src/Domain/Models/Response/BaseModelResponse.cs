public class BaseModelResponse
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? CreatedByUserId { get; set; } = null;
    public string? CreatedByUserName { get; set; } = null;
}