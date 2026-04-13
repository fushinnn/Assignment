using MovieLibrary.Models;

namespace MovieLibrary.DataStructures
{
    public class MovieNode
    {
        public Movie Data {get; set;}
        public MovieNode? Next {get; set;}

        public MovieNode(Movie movie)
        {
            Data = movie;
            Next = null;
        }
    }
}