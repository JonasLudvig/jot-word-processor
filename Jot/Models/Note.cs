using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;

namespace Jot.Models
{
    public class Note : ObservableObject
    {
        public string Title { get; set; }
        public string Filename { get; set; }
        public string CreatedPresentationFormat { get; set; }
        public string CreatedSortableFormat { get; set; }
        public string ModifiedPresentationFormat { get; set; }
        public string ModifiedSortableFormat { get; set; }
        public string Reads { get; set; }

        // Constructor
        public Note()
        {
            Title = $"Note on {DateTime.Now:MMMM d, HH:mm}";
        }

        // Duplicate constructor
        public Note(Note userNote)
        {
            Title = userNote.Title;
            Filename = userNote.Filename;
            CreatedPresentationFormat = userNote.CreatedPresentationFormat;
            CreatedSortableFormat = userNote.CreatedSortableFormat;
            ModifiedPresentationFormat = userNote.ModifiedPresentationFormat;
            ModifiedSortableFormat = userNote.ModifiedSortableFormat;
            Reads = userNote.Reads;
        }

        public void Unset()
        {
            Title = $"Note on {DateTime.Now:MMMM d, HH:mm}";
            Filename = null;
            CreatedPresentationFormat = null;
            CreatedSortableFormat = null;
            ModifiedPresentationFormat = null;
            ModifiedSortableFormat = null;
            Reads = null;
        }

        // Methods
        public void SetTitle (string title)
        {
            Title = title;
        }
        public void SetFilename(string filename)
        {
            Filename = filename;
        }
        public void SetCreatedPresentationFormat(string createdPresentationFormat)
        {
            CreatedPresentationFormat = createdPresentationFormat;
        }
        public void SetCreatedSortableFormat(string createdSortableFormat)
        {
            CreatedSortableFormat = createdSortableFormat;
        }
        public void SetModifiedPresentationFormat(string modifiedPresentationFormat)
        {
            ModifiedPresentationFormat = modifiedPresentationFormat;
        }
        public void SetModifiedSortableFormat(string modifiedSortableFormat)
        {
            ModifiedSortableFormat = modifiedSortableFormat;
        }
        public void SetReads(string reads)
        {
            Reads = reads;
        }
    }
}
