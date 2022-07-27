namespace MyFavoriteMovie.WebAPI.Dto.Home
{
    public class ActorDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? AvatarImage { get; set; }
    }
}
