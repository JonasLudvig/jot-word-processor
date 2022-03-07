using System.Collections.Generic;
using System.Collections.ObjectModel;
using Jot.Models;

namespace Jot.ViewModels
{
    public partial class MainViewModel
    {
        #region Objects

        private readonly DataFileManager _instanceOfDataFileManager = new();

        private Note _userNote = new();
        public Note UserNote
        {
            get => _userNote;
            set => SetProperty(ref _userNote, value);
        }

        private ObservableCollection<Note> _library = new();
        public ObservableCollection<Note> Library
        {
            get => _library;
            set => SetProperty(ref _library, value);
        }

        private List<Note> _sortedLibrary = new();
        public List<Note> SortedLibrary
        {
            get => _sortedLibrary;
            set => SetProperty(ref _sortedLibrary, value);
        }
        #endregion

        #region Operations
        private string _operationLibraryHeader = "_Library";
        public string OperationLibraryHeader
        {
            get => _operationLibraryHeader;
            set => SetProperty(ref _operationLibraryHeader, value);
        }

        private string _operationLibraryDetailsHeader = "_Library Details ✓";
        public string OperationLibraryDetailsHeader
        {
            get => _operationLibraryDetailsHeader;
            set => SetProperty(ref _operationLibraryDetailsHeader, value);
        }

        private string _operationFormattingHeader = "_Formatting";
        public string OperationFormattingHeader
        {
            get => _operationFormattingHeader;
            set => SetProperty(ref _operationFormattingHeader, value);
        }

        private int _drawSearchResult;
        public int DrawSearchResult
        {
            get => _drawSearchResult;
            set => SetProperty(ref _drawSearchResult, value);
        }

        private bool _searchLive;

        private string _textBoxSearch = "Search";

        public string TextBoxSearch
        {
            get => _textBoxSearch;
            set => SetProperty(ref _textBoxSearch, value);
        }

        private string _textBoxTitle = "My Title";
        public string TextBoxTitle
        {
            get => _textBoxTitle;
            set => SetProperty(ref _textBoxTitle, value);
        }
        #endregion

        #region User Interface toggles
        private string _drawCanvas = "Visible";
        public string DrawCanvas
        {
            get => _drawCanvas;
            set => SetProperty(ref _drawCanvas, value);
        }

        private string _drawTextBox = "Hidden";
        public string DrawTextBox
        {
            get => _drawTextBox;
            set => SetProperty(ref _drawTextBox, value);
        }

        private int _drawLibrary;
        public int DrawLibrary
        {
            get => _drawLibrary;
            set => SetProperty(ref _drawLibrary, value);
        }

        private int _drawLibraryDetails = 78;
        public int DrawLibraryDetails
        {
            get => _drawLibraryDetails;
            set => SetProperty(ref _drawLibraryDetails, value);
        }

        private int _drawToolBar;
        public int DrawToolBar
        {
            get => _drawToolBar;
            set => SetProperty(ref _drawToolBar, value);
        }
        #endregion

        #region User formatting configuration
        private int _selectedFontIndex;
        public int SelectedFontIndex
        {
            get => _selectedFontIndex;
            set => SetProperty(ref _selectedFontIndex, value);
        }

        private string _selectedFont = "/Fonts/#Fira Mono";
        public string SelectedFont
        {
            get => _selectedFont;
            set => SetProperty(ref _selectedFont, value);
        }

        private int _selectedFontSizeIndex = 1;
        public int SelectedFontSizeIndex
        {
            get => _selectedFontSizeIndex;
            set => SetProperty(ref _selectedFontSizeIndex, value);
        }

        private string _selectedFontSize = "12pt";
        public string SelectedFontSize
        {
            get => _selectedFontSize;
            set => SetProperty(ref _selectedFontSize, value);
        }
        #endregion

        #region Selection and focusing
        public bool FileIsBusyMirror => !_instanceOfDataFileManager.FileIsBusy;

        private string _textBoxIsEnabled = "True";

        public string TextBoxIsEnabled
        {
            get => _textBoxIsEnabled;
            set => SetProperty(ref _textBoxIsEnabled, value);
        }

        private int _listViewSelectedIndex = -1;

        public int ListViewSelectedIndex
        {
            get => _listViewSelectedIndex;
            set
            {
                _listViewSelectedIndex = value;

                if (_listViewSelectedIndex != -1)
                {
                    if (UserNote.Reads != null)
                    {
                        _instanceOfDataFileManager.NoteToFile(UserNote);
                    }
                }
            }
        }

        private string _lastModified;

        public string LastModified
        {
            get => _lastModified;
            set => SetProperty(ref _lastModified, value);
        }

        private string _dateCreated;
        public string DateCreated
        {
            get => _dateCreated;
            set => SetProperty(ref _dateCreated, value);
        }

        private string _searchResult;
        public string SearchResult
        {
            get => _searchResult;
            set => SetProperty(ref _searchResult, value);
        }
        #endregion
    }
}
