using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Jot.Models;

namespace Jot.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        #region Commands
        public IRelayCommand OperationNew { get; }
        public IRelayCommand OperationClose { get; }
        public IRelayCommand OperationSave { get; }
        public IRelayCommand OperationDelete { get; }
        public IRelayCommand OperationExit { get; }
        public IRelayCommand OperationFormatting { get; }
        public IRelayCommand OperationLibrary { get; }
        public IRelayCommand OperationLibraryDetails { get; }
        public IRelayCommand OperationAbout { get; }
        public IRelayCommand OperationDocumentation { get; }
        public IRelayCommand OperationSearch { get; }
        public IRelayCommand OperationResetView { get; }
        #endregion

        // Constructor
        public MainViewModel()
        {
            // Event handler set-up
            PropertyChanged += OnViewModelPropertyChanged;
            _instanceOfDataFileManager.FileIsBusyChanged += OnFileIsBusyChanged;

            // Commands instancing
            OperationNew = new RelayCommand(ExecuteOperationNew);
            OperationClose = new RelayCommand(ExecuteOperationClose);
            OperationSave = new RelayCommand(ExecuteOperationSave);
            OperationDelete = new RelayCommand(ExecuteOperationDelete);
            OperationExit = new RelayCommand(ExecuteOperationExit);
            OperationLibrary = new RelayCommand(ExecuteOperationLibrary);
            OperationLibraryDetails = new RelayCommand(ExecuteOperationLibraryDetails);
            OperationFormatting = new RelayCommand(ExecuteOperationFormatting);
            OperationAbout = new RelayCommand(ExecuteOperationAbout);
            OperationDocumentation = new RelayCommand(ExecuteOperationDocumentation);
            OperationSearch = new RelayCommand(ExecuteOperationSearch);
            OperationResetView = new RelayCommand(ExecuteOperationResetView);

            Populate();
        }

        private void OnFileIsBusyChanged()
        {
            OnPropertyChanged(nameof(FileIsBusyMirror));
        }

        public async Task Populate()
        {
            // Populating library with whatever .json files available in AppData\Local\Jot Word Processor\
            string appDataDirPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string dirPath = Path.Combine(appDataDirPath, "Jot Word Processor");

            string[] fileEntries = Directory.GetFiles(dirPath, "*.json");
            foreach (string filePath in fileEntries)
            {
                var filename = Path.GetFileNameWithoutExtension(filePath);
                Note instanceOfNote = DataFileManager.ReadDataFile(filename).Result;

                Library.Add(instanceOfNote);
            }
            Sort(true);
        }

        public void Sort(bool sortOnLastModified)
        {
            if (sortOnLastModified)
            {
                SortedLibrary = Library.ToList();
                SortedLibrary.Sort(comparison: (x, y) => string.Compare(x.ModifiedSortableFormat, y.ModifiedSortableFormat, StringComparison.Ordinal));
                Library = new ObservableCollection<Note>(SortedLibrary);
            }
            else
            {
                SortedLibrary = Library.ToList();
                SortedLibrary.Sort(comparison: (x, y) => string.Compare(x.CreatedSortableFormat, y.CreatedSortableFormat, StringComparison.Ordinal));
                Library = new ObservableCollection<Note>(SortedLibrary);
            }
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            #region Font formatting
            SelectedFont = SelectedFontIndex == 0 ? "/Fonts/#Fira Mono" : "/Fonts/#Fira Sans";

            SelectedFontSize = SelectedFontSizeIndex switch
            {
                0 => "11",
                1 => "12",
                2 => "13",
                3 => "14",
                4 => "16",
                5 => "18",
                6 => "21",
                _ => SelectedFontSize
            };

            #endregion
            if (ListViewSelectedIndex != -1 && !_searchLive)
            {
                DrawCanvas = "Hidden";
                DrawTextBox = "Visible";

                LastModified = $"Last modified\n{UserNote.ModifiedPresentationFormat}";
                DateCreated = $"Date created\n{UserNote.CreatedPresentationFormat}";
            }

            // On search match selection
            if (_searchLive && Library.Count >= 1)
            {
                DrawCanvas = "Hidden";
                DrawTextBox = "Visible";
                LastModified = $"Last modified\n–";
                DateCreated = $"Date created\n–";
                _searchLive = false;
            }
        }

        #region Methods: Command executions
        public void ExecuteOperationNew()
        {
            DrawCanvas = "Hidden";
            DrawTextBox = "Visible";
            ListViewSelectedIndex = -1;
            Note unvaluedNote = new();
            UserNote = unvaluedNote;
            Library.Add(UserNote);
            OnPropertyChanged();
        }

        public void ExecuteOperationClose()
        {
            ListViewSelectedIndex = -1;
            DrawCanvas = "Visible";
            DrawTextBox = "Hidden";
        }

        public void ExecuteOperationSave()
        {
            if (_listViewSelectedIndex == -1) return;
            if (UserNote.Reads != null)
            {
                _instanceOfDataFileManager.NoteToFile(UserNote);
            }
        }

        public void ExecuteOperationDelete()
        {
            string appDataDirPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string dirPath = Path.Combine(appDataDirPath, "Jot Word Processor");
            File.Delete(Path.Combine(dirPath, UserNote.Filename + ".json"));

            Library.RemoveAt(ListViewSelectedIndex);
            ListViewSelectedIndex = -1;
        }

        public static void ExecuteOperationExit()
        {
            Application.Current.Shutdown();
        }

        public void ExecuteOperationLibrary()
        {
            DrawLibrary = DrawLibrary == 0 ? 176 : 0;
            OperationLibraryHeader = DrawLibrary == 0 ? "_Library" : "_Library ✓";
        }

        public void ExecuteOperationLibraryDetails()
        {
            DrawLibraryDetails = DrawLibraryDetails == 0 ? 78 : 0;
            OperationLibraryDetailsHeader = DrawLibraryDetails == 0 ? "_Library Details" : "_Library Details ✓";
        }

        public void ExecuteOperationFormatting()
        {
            DrawToolBar = DrawToolBar == 0 ? 28 : 0;
            OperationFormattingHeader = DrawToolBar == 0 ? "_Formatting" : "_Formatting ✓";
        }

        public static void ExecuteOperationAbout()
        {
            _ = MessageBox.Show("Jot: Word Processor and Library\nVersion 0.1.0\n\nCopyright © 2022 Jonas Lind. All rights reserved.", "About");
        }

        public static void ExecuteOperationDocumentation()
        {
            _ = MessageBox.Show("- Self-contained x86 application bound to run on pretty much whatever toaster with .exe support.\n\n- Notes saved asynchronous and intelligently upon library browsing as JSON objects, neatly tucked away within the very minimalistic application resource folder to peacefully roam the magical and far away land of AppData.", "Documentation");
        }

        public void ExecuteOperationSearch()
        {
            ObservableCollection<Note> searchResultLibrary = new();
            foreach (var t in Library)
            {
                if (TextBoxSearch.Split(' ').Any(x => t.Reads.Contains(x)))
                {
                    searchResultLibrary.Add(t);
                }
            }
            DrawCanvas = "Visible";
            DrawTextBox = "Hidden";
            ListViewSelectedIndex = -1;
            Library = searchResultLibrary;
            DrawSearchResult = 45;
            SearchResult = $"Search yields {Library.Count} result(s)";
            _searchLive = true;
        }

        public void ExecuteOperationResetView()
        {
            DrawCanvas = "Visible";
            DrawTextBox = "Hidden";
            DrawSearchResult = 0;
            ListViewSelectedIndex = -1;
            Library.Clear();
            ListViewSelectedIndex = -1;
            Note unvaluedNote = new();
            UserNote = unvaluedNote;
            Populate();
        }
        #endregion
    }
}