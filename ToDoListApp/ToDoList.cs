using System.Text.Json.Serialization;

public class ToDoList
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string Content { get; set; } = string.Empty;

    public DateOnly DueDate { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Priority Priority { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TaskStatus Status { get; set;}
}

public enum TaskStatus
{
    Pending,
    InProgress,
    Completed
}

public enum Priority
{
    Low,
    Medium,
    High
}