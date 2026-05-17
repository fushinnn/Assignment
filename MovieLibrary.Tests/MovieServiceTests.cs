using MovieLibrary.Models;
using MovieLibrary.Services;
using Xunit;

namespace MovieLibrary.Tests
{
    public class MovieServiceTests
    {
        private MovieService CreateService()
        {
            var service = new MovieService();
            service.AddMovie(new Movie("M001", "Inception", "Christopher Nolan", "Sci-Fi", 2010));
            service.AddMovie(new Movie("M002", "The Matrix", "Wachowski Sisters", "Sci-Fi", 1999));
            service.AddMovie(new Movie("M003", "Interstellar", "Christopher Nolan", "Sci-Fi", 2014));
            service.AddMovie(new Movie("M004", "Parasite", "Bong Joon-ho", "Thriller", 2019));
            return service;
        }

        // ── ADD ───────────────────────────────────────────────────────────────

        [Fact]
        public void AddMovie_ValidMovie_ReturnsTrue()
        {
            var service = new MovieService();
            var movie = new Movie("M001", "Inception", "Nolan", "Sci-Fi", 2010);
            Assert.True(service.AddMovie(movie));
        }

        [Fact]
        public void AddMovie_DuplicateId_ReturnsFalse()
        {
            var service = new MovieService();
            var movie = new Movie("M001", "Inception", "Nolan", "Sci-Fi", 2010);
            service.AddMovie(movie);
            Assert.False(service.AddMovie(movie));
        }

        // ── REMOVE ────────────────────────────────────────────────────────────

        [Fact]
        public void RemoveMovie_ExistingMovie_ReturnsTrue()
        {
            var service = CreateService();
            Assert.True(service.RemoveMovie("M001"));
        }

        [Fact]
        public void RemoveMovie_NonExistentMovie_ReturnsFalse()
        {
            var service = CreateService();
            Assert.False(service.RemoveMovie("M999"));
        }

        // ── SEARCH ────────────────────────────────────────────────────────────

        [Fact]
        public void SearchByTitle_ExistingTitle_ReturnsResults()
        {
            var service = CreateService();
            var results = service.SearchByTitle("Inception");
            Assert.Single(results);
        }

        [Fact]
        public void SearchByTitle_CaseInsensitive_ReturnsResults()
        {
            var service = CreateService();
            var results = service.SearchByTitle("inception");
            Assert.Single(results);
        }

        [Fact]
        public void SearchByTitle_NoMatch_ReturnsEmpty()
        {
            var service = CreateService();
            var results = service.SearchByTitle("Batman");
            Assert.Empty(results);
        }

        [Fact]
        public void BinarySearchById_ExistingId_ReturnsMovie()
        {
            var service = CreateService();
            var movie = service.BinarySearchById("M001");
            Assert.NotNull(movie);
            Assert.Equal("Inception", movie.Title);
        }

        [Fact]
        public void BinarySearchById_NonExistentId_ReturnsNull()
        {
            var service = CreateService();
            var movie = service.BinarySearchById("M999");
            Assert.Null(movie);
        }

        // ── SORT ──────────────────────────────────────────────────────────────

        [Fact]
        public void BubbleSortByTitle_ReturnsSortedList()
        {
            var service = CreateService();
            var sorted = service.BubbleSortByTitle();
            Assert.Equal("Inception", sorted[0].Title);
            Assert.Equal("Interstellar", sorted[1].Title);
            Assert.Equal("Parasite", sorted[2].Title);
            Assert.Equal("The Matrix", sorted[3].Title);
        }

        [Fact]
        public void MergeSortByYear_ReturnsSortedList()
        {
            var service = CreateService();
            var sorted = service.MergeSortByYear();
            Assert.Equal(1999, sorted[0].ReleaseYear);
            Assert.Equal(2010, sorted[1].ReleaseYear);
            Assert.Equal(2014, sorted[2].ReleaseYear);
            Assert.Equal(2019, sorted[3].ReleaseYear);
        }

        // ── BORROW / RETURN ───────────────────────────────────────────────────

        [Fact]
        public void BorrowMovie_AvailableMovie_MarksAsBorrowed()
        {
            var service = CreateService();
            service.BorrowMovie("M001", "Lorenzo");
            var movie = service.GetById("M001");
            Assert.False(movie!.IsAvailable);
        }

        [Fact]
        public void BorrowMovie_UnavailableMovie_AddsToQueue()
        {
            var service = CreateService();
            service.BorrowMovie("M001", "Lorenzo");
            service.BorrowMovie("M001", "Alice");
            Assert.Equal(1, service.GetQueueLength("M001"));
        }

        [Fact]
        public void ReturnMovie_NoQueue_MarksAsAvailable()
        {
            var service = CreateService();
            service.BorrowMovie("M001", "Lorenzo");
            service.ReturnMovie("M001");
            var movie = service.GetById("M001");
            Assert.True(movie!.IsAvailable);
        }

        [Fact]
        public void ReturnMovie_WithQueue_AssignsToNextUser()
        {
            var service = CreateService();
            service.BorrowMovie("M001", "Lorenzo");
            service.BorrowMovie("M001", "Alice");
            string result = service.ReturnMovie("M001");
            Assert.Contains("Alice", result);
        }

        // ── EDGE CASES ────────────────────────────────────────────────────────

        [Fact]
        public void GetAllMovies_EmptyService_ReturnsEmptyList()
        {
            var service = new MovieService();
            Assert.Empty(service.GetAllMovies());
        }

        [Fact]
        public void BorrowMovie_NonExistentMovie_ReturnsNotFound()
        {
            var service = CreateService();
            var result = service.BorrowMovie("M999", "Lorenzo");
            Assert.Equal("Movie not found.", result);
        }

        [Fact]
        public void ReturnMovie_NonExistentMovie_ReturnsNotFound()
        {
            var service = CreateService();
            var result = service.ReturnMovie("M999");
            Assert.Equal("Movie not found.", result);
        }
    }
}