namespace Domain.DTOs;

public record TodoDto
{
    public int Id { get; set; }
    public string Todo { get; set; } = string.Empty;
    public bool Completed { get; set; }
}

public record TodoWithUserDto
{
    public int Id { get; set; }
    public string Todo { get; set; } = string.Empty;
    public bool Completed { get; set; }
    public UserDto User { get; set; } = new();
}