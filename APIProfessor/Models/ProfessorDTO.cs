namespace APIProfessor.Models
{
    public class ProfessorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Description { get; set; }
        public string? Photo { get; set; } // <-- Aquí lo cambiamos a string base64
        public string? SocialLink { get; set; }
        public bool? StatusProfessor { get; set; }
    }

}
