
namespace Domain.Entities
{
    public class Todo
    {
        public int Id { get; set; }
        public required string TodoText { get; set; }
        public bool Completed { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
