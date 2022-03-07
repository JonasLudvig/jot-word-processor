using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Jot.Models;

namespace Jot
{
    class DataFileManager
    {
        public static string AppDataDirPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public static string DirPath = Path.Combine(AppDataDirPath, "Jot Word Processor");

        private bool _fileIsBusy;
        public bool FileIsBusy
        {
            get => _fileIsBusy;
            set
            {
                _fileIsBusy = value;
                OnFileIsBusyChanged();
            }
        }

        private void OnFileIsBusyChanged()
        {
            FileIsBusyChanged?.Invoke();
        }

        public event Action FileIsBusyChanged;
        public DataFileManager()
        {
            Directory.CreateDirectory(DirPath);
        }

        public static async Task<Note> ReadDataFile(string filename)
        {
            string filePath = Path.Combine(DirPath, $"{filename}.json");
            if (File.Exists(filePath))
            {
                await using FileStream readFileStream = File.OpenRead(filePath);
                Note instanceOfNote = JsonSerializer.DeserializeAsync<Note>(readFileStream).Result;
                return instanceOfNote;
            }
            else
            {
                // File not found
                return null;
            }
        }

        public async Task NoteToFile(Note userNote)
        {
            FileIsBusy = true;
            if (userNote.Reads != null)
            {
                // Assigning filename and timestamp if not previously save
                if (userNote.Filename == null)
                {
                    userNote.SetFilename(DateTime.Now.ToString("yyyyMMmmssff"));

                    userNote.SetCreatedPresentationFormat(DateTime.Now.ToString("MMMM d yyyy, HH:mm"));
                    userNote.SetCreatedSortableFormat(DateTime.Now.ToString("yyyyMMmmssff"));
                }
                userNote.SetModifiedPresentationFormat(DateTime.Now.ToString("MMMM d yyyy, HH:mm"));
                userNote.SetModifiedSortableFormat(DateTime.Now.ToString("yyyyMMmmssff"));

                //Copy
                Note userNoteCopy = new(userNote);

                // Save changes to note
                using FileStream outNoteFileStream = File.Create(Path.Combine(DirPath, userNoteCopy.Filename + ".json"));
                JsonSerializerOptions options = new() { WriteIndented = true };
                await JsonSerializer.SerializeAsync(outNoteFileStream, userNoteCopy, options);
                await outNoteFileStream.DisposeAsync();
            }
            FileIsBusy = false;
        }
    }
}
