using MovieLibrary.DataStructures;
using MovieLibrary.Models;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;

namespace MovieLibrary.Services
{
    public class MovieService
    {
        private readonly MovieLinkedList _movies = new();
        private readonly MovieHashtable _hashtable = new();
        private readonly BorrowQueue _borrowQueue = new();
        private readonly Dictionary<string, List<string>> _borrowHistory = new();



        //-----CREATE READ UPDATE DELETE-------------------

        public bool AddMovie(Movie movie)
        {
            if (_hashtable.Contains(movie.MovieId))
                return false;
            
            _movies.Add(movie);
            _hashtable.Add(movie);
            return true;
        }

        public bool RemoveMovie(string movieId)
        {
            if (!_hashtable.Contains(movieId))
                return false;

            _movies.Remove(movieId);
            _hashtable.Remove(movieId);
            return true;
        }

        public List<Movie> GetAllMovies() => _movies.ToList();
        public Movie? GetById(string movieId) => _hashtable.Get(movieId);


        //----SEARCH--------------------------------------

        public List<Movie> SearchByTitle(string title)
        {
            var results = new List<Movie>();

            foreach (var movie in _movies.ToList())
            {
                if (movie.Title.Contains(title, System.StringComparison.OrdinalIgnoreCase))
                results.Add(movie);
            }
            
            return results;
        }


        public Movie? BinarySearchById(string movieId)
        {
            var sorted = new List<Movie>(_movies.ToList() .OrderBy(m => m.MovieId));
            int low = 0, high = sorted.Count - 1;

            while (low <= high)
            {
                int mid = (low + high) / 2;
                int cmp = string.Compare(sorted[mid].MovieId, movieId, System.StringComparison.OrdinalIgnoreCase);
                if (cmp == 0) return sorted[mid];
                if (cmp < 0) low = mid + 1;
                else high = mid - 1;
            }

            return null;
        }


        //----SORT---------------------------------------

        public List<Movie> BubbleSortByTitle()
        {
            var list = _movies.ToList();
            int n = list.Count;

            for (int i = 0; i < n - 1; i++)
                for (int j = 0; j < n - i - 1; j++)
                
                if 
                (string.Compare(list[j].Title, list[j + 1].Title) > 0)
                (list[j], list[j + 1]) = (list[j + 1], list [j]);

            _movies.ReplaceAll(list);
            return list;
        }


        public List<Movie> MergeSortByYear()
        {
            var sorted = MergeSort(_movies.ToList());
            _movies.ReplaceAll(sorted);
            return sorted;
        }


        private List<Movie> MergeSort(List<Movie> list)
        {
            if (list.Count <= 1) return list;

            int mid = list.Count / 2;
            var left = MergeSort(list.GetRange(0, mid));
            var right = MergeSort(list.GetRange(mid, list.Count - mid));

            return Merge (left, right);
        } 


        private List<Movie> Merge(List<Movie> left, List<Movie> right)
        {
            var result = new List<Movie>();

            int i = 0, j = 0;

            while (i < left.Count && j < right.Count)
            {
                if (left[i].ReleaseYear <= right[j].ReleaseYear)
                    result.Add(left[i++]);
                else
                    result.Add(right[j++]);
            }

            while (i < left.Count) result.Add(left[i++]);
            while (j < right.Count) result.Add(right[j++]);
            return result;

        }


        //-----BORROW--------------------------------------

        public string BorrowMovie(string movieId, string userName)
        {

            var movie = _hashtable.Get(movieId);
            if (movie == null) return "Movie not found.";

            if (movie.IsAvailable)
            {
                movie.IsAvailable = false;
                
                //History tracking
                if (!_borrowHistory.ContainsKey(movieId))
                    _borrowHistory[movieId] = new List<string>();
                    _borrowHistory[movieId].Add($"{userName} - {DateTime.Now:dd/MM/yy}");


                return $"'{movie.Title}' borrowed successfully by {userName}.";
            }
            else
            {
                _borrowQueue.Enqueue(movieId, userName);
                int position = _borrowQueue.QueueLength(movieId);
                return $"'{movie.Title}' is unavailable. {userName} added to queue (position {position}).";
            }
        }

        public List<string> GetBorrowHistory(string movieId)
        {
            return _borrowHistory.TryGetValue(movieId, out var result) ? result : new List<string>();
        }

        public string ReturnMovie(string movieId)
        {
            var movie = _hashtable.Get(movieId);
            if (movie == null) return "Movie not found.";

            if (_borrowQueue.HasQueue(movieId))
            {
                string? nextUser = _borrowQueue.Dequeue(movieId);
                return $"'{movie.Title}' returned and assigned to {nextUser}.";
            }
            else
            {
                movie.IsAvailable = true;
                return $"'{movie.Title}' returned and is now available.";
            }
        }


        //-----INFO-----------------------------------------

        public int GetQueueLength(string movieId) => _borrowQueue.QueueLength(movieId);


    }
}