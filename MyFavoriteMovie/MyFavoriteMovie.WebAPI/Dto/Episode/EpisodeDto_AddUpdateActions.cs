namespace MyFavoriteMovie.WebAPI.Dto.Episode
{
    public class EpisodeDto_AddUpdateActions
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Season { get; set; }
        public string? Duration { get; set; }
        public string? RealeseDate { get; set; }
        public int MovieId { get; set; }
    }
}
