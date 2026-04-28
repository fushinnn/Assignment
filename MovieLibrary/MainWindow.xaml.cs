using MovieLibrary.Models;
using MovieLibrary.Services;
using System.Collections.Generic;
using System.Windows;

namespace MovieLibrary
{
    public partial class MainWindow : Window
    {
        private readonly MovieService _service = new();

        public MainWindow()
        {
            InitializeComponent();
            LoadSampleData();
            RefreshGrid(_service.GetAllMovies());
        }

        //------Data------
        private void RefreshGrid(List<Movie> movies)
        {
            MovieGrid.ItemsSource = null;
            MovieGrid.ItemsSource = movies;
        }

        private void SetStatus(string message) => TxtStatus.Text = message;

        private void LoadSampleData()
        {
            _service.AddMovie(new Movie("1", "Inception",          "Christopher Nolan", "Sci-Fi",    2010));
            _service.AddMovie(new Movie("2", "The Matrix",         "Wachowski Sisters", "Sci-Fi",    1999));
            _service.AddMovie(new Movie("3", "Interstellar",       "Christopher Nolan", "Sci-Fi",    2014));
            _service.AddMovie(new Movie("4", "Parasite",           "Bong Joon-ho",      "Thriller",  2019));
            _service.AddMovie(new Movie("5", "The Dark Knight",    "Christopher Nolan", "Action",    2008));
            _service.AddMovie(new Movie("6", "Spirited Away",      "Hayao Miyazaki",    "Animation", 2001));
            _service.AddMovie(new Movie("7", "Pulp Fiction",       "Quentin Tarantino", "Crime",     1994));
            _service.AddMovie(new Movie("8", "Mad Max: Fury Road", "George Miller",     "Action",    2015));
        }

        // ------Add------

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtId.Text) || string.IsNullOrWhiteSpace(TxtTitle.Text))
            {
                SetStatus("ID and Title are required.");
                return;
            }

            if (!int.TryParse(TxtYear.Text, out int year))
            {
                SetStatus("Release year must be a number.");
                return;
            }

            var movie = new Movie(
                TxtId.Text.Trim(),
                TxtTitle.Text.Trim(),
                TxtDirector.Text.Trim(),
                TxtGenre.Text.Trim(),
                year);

            if (_service.AddMovie(movie))
            {
                RefreshGrid(_service.GetAllMovies());
                SetStatus($"'{movie.Title}' added successfully.");
                TxtId.Clear(); TxtTitle.Clear();
                TxtDirector.Clear(); TxtGenre.Clear(); TxtYear.Clear();
            }
            else
            {
                SetStatus($"A movie with ID '{movie.MovieId}' already exists.");
            }
        }

        // ------Search------

        private void BtnSearchTitle_Click(object sender, RoutedEventArgs e)
        {
            var results = _service.SearchByTitle(TxtSearch.Text.Trim());
            RefreshGrid(results);
            SetStatus($"{results.Count} result(s) found for '{TxtSearch.Text}'.");
        }

        private void BtnSearchId_Click(object sender, RoutedEventArgs e)
        {
            var movie = _service.BinarySearchById(TxtSearch.Text.Trim());
            RefreshGrid(movie != null ? new List<Movie> { movie } : new List<Movie>());
            SetStatus(movie != null ? $"Found: {movie.Title}" : "No movie found with that ID.");
        }

        // ------Sort------

        private void BtnBubbleSort_Click(object sender, RoutedEventArgs e)
        {
            RefreshGrid(_service.BubbleSortByTitle());
            SetStatus("Sorted by title (Bubble Sort).");
        }

        private void BtnMergeSort_Click(object sender, RoutedEventArgs e)
        {
            RefreshGrid(_service.MergeSortByYear());
            SetStatus("Sorted by release year (Merge Sort).");
        }

        private void BtnShowAll_Click(object sender, RoutedEventArgs e)
        {
            RefreshGrid(_service.GetAllMovies());
            SetStatus("Showing all movies.");
        }

        // ------Borrow / Return------

        private void BtnBorrow_Click(object sender, RoutedEventArgs e)
        {
            var selected = MovieGrid.SelectedItem as Movie;
            if (selected == null) { SetStatus("Select a movie first."); return; }
            if (string.IsNullOrWhiteSpace(TxtUsername.Text)) { SetStatus("Enter a username."); return; }

            string result = _service.BorrowMovie(selected.MovieId, TxtUsername.Text.Trim());
            RefreshGrid(_service.GetAllMovies());
            SetStatus(result);
            MessageBox.Show(result, "Borrow Movie");
        }

        private void BtnReturn_Click(object sender, RoutedEventArgs e)
        {
            var selected = MovieGrid.SelectedItem as Movie;
            if (selected == null) { SetStatus("Select a movie first."); return; }

            string result = _service.ReturnMovie(selected.MovieId);
            RefreshGrid(_service.GetAllMovies());
            SetStatus(result);
            MessageBox.Show(result, "Return Movie");
        }

        // ------Remove------

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            var selected = MovieGrid.SelectedItem as Movie;
            if (selected == null) { SetStatus("Select a movie first."); return; }

            if (_service.RemoveMovie(selected.MovieId))
            {
                RefreshGrid(_service.GetAllMovies());
                SetStatus($"'{selected.Title}' removed.");
            }
        }


        // ------Check------
        private void txtbox_check_Int_Input(object sender, System.Windows.Input.TextCompositionEventArgs e)
            {
                e.Handled = !int.TryParse(e.Text, out _);
            }

        private void MovieGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}