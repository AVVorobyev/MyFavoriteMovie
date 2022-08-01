﻿using MyFavoriteMovie.Core.Models;

namespace MyFavoriteMovie.WebAPI.Dto.Actor
{
    public class ActorDtoIndex
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? AvatarImage { get; set; }
    }
}
