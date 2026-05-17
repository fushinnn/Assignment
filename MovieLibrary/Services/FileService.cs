using Microsoft.Win32;
using MovieLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.IO;

namespace MovieLibrary.Services
{
    public class FileService
    {
        private static readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true
        };


        public static void ExportMovies(List<Movie> movies)
        {
            SaveFileDialog dialog = new()
            {
                Title = "Export Movie List",
                Filter = "JSON Files (*.json)|*.json",
                FileName = "MovieLibrary"
            };

            if (dialog.ShowDialog() == true)
            {
                string json = JsonSerializer.Serialize(movies, _options);
                File.WriteAllText(dialog.FileName, json);
            }
        }

        public static List<Movie>? ImportMovies()
        {
            OpenFileDialog dialog = new()
            {
                Title = "Import Movie List",
                Filter = "JSON Files (*.json)|*.json"
            };

            if (dialog.ShowDialog() == true)
            {
                string json = File.ReadAllText(dialog.FileName);
                return JsonSerializer.Deserialize<List<Movie>>(json);
            }

            return null;
        }

    }
}
