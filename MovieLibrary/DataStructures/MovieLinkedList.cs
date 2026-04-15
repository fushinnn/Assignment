using MovieLibrary.Models;
using System.Collections.Generic;

namespace MovieLibrary.DataStructures
{
    public class MovieLinkedList
    {
        private MovieNode? _head;
        public int Count {get; private set;}

        public void Add(Movie movie)
        {
            var newNode = new MovieNode(movie);

            if (_head == null)
            {
                _head = newNode;
            }
            else
            {
                var current = _head;
                while (current.Next != null)
                current = current.Next;
                current.Next = newNode;
            }

            Count++;

        }

        public bool Remove(string movieId)
        {
            if (_head == null) return false;

            if (_head.Data.MovieId == movieId)
            {
                _head = _head.Next;
                Count--;
                return true;
            }

            var current = _head;
            while (current.Next != null)
            {
                if (current.Next.Data.MovieId == movieId)
                {
                    current.Next = current.Next.Next;
                    Count--;
                    return true;
                }

                current = current.Next;
            }
            return false;
        }


        public List<Movie> ToList()
        {
            var list = new List<Movie>();
            var current = _head;

            while (current != null)
            {
                list.Add(current.Data);
                current = current.Next;
            }

            return list;
        }


        public void ReplaceAll(List<Movie> movies)
        {
            _head = null;
            Count = 0;
            foreach (var m in movies) Add(m);
        }

    }
}