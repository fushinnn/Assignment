using MovieLibrary.Models;
using System.Collections;

namespace MovieLibrary.DataStructures
{
    public class MovieHashtable
    {
        private readonly Hashtable _table = new Hashtable();

        public void Add(Movie movie)
        {
            _table[movie.MovieId] = movie;
        }

        public Movie? Get(string movidId)
        {
            return _table[movidId] as Movie;
        }

        public bool Remove(string movidId)
        {
            if (!_table.Contains(movidId)) return false;
            _table.Remove(movidId);
            return true;
        }

        public bool Contains(string movidId) => _table.Contains(movidId);
    }
}