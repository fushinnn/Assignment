namespace MovieLibrary.Models
{
    public class Movie
    {
        public string MovieId {get; set;}
        public string Title {get; set;}
        public string Director {get; set;}
        public string Genre {get; set;}
        public int ReleaseYear {get; set;}
        public bool IsAvailable {get; set;} = true;

        public string Availability => IsAvailable ? "Available" : "Borrowed";

        public Movie (String movieId, string title, string director, string genre, int releaseYear)
        {
            MovieId = movieId;
            Title = title;
            Director = director;
            Genre = genre;
            ReleaseYear = releaseYear;
            IsAvailable = true;
        }

    }
}