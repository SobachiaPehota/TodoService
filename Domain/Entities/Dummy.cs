using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class DummyUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
    }

    public class DummyTodo
    {
        public int Id { get; set; }
        public string Todo { get; set; } = string.Empty;
        public bool Completed { get; set; }
        public int UserId { get; set; }
    }
}
