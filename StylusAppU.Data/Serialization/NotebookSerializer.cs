using System;
using StylusAppU.Data.Data;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Input.Inking;
using System.IO.Compression;
using System.Collections.Generic;

namespace StylusAppU.Data.Serialization
{
    public class NotebookSerializer
    {
        public static readonly string InkMetadataSubfolderName = ".ink";
        public static readonly string BackgroundMetadataSubfolderName = ".backgrounds";
        public static readonly string NotebookFileName = "Notebook.json";
        public static readonly string CurrentNotebookKey = "CurrentNotebook";

        private Notebook _notebook;
        private StorageFolder _notebookFolder, _inkMetadataFolder, _backgroundMetadataFolder;

        #region Constructor

        public NotebookSerializer(Notebook notebook)
        {
            _notebook = notebook;
        }

        public NotebookSerializer()
        {
        }

        #endregion

        #region properties

        public Notebook Notebook { get { return _notebook; } }

        #endregion

        #region Public Methods

        public async Task LoadLocalNotebookFolder(string localNotebookFolderPath)
        {
            var appFolder = ApplicationData.Current.LocalFolder;
            _notebookFolder = await appFolder.GetFolderAsync(localNotebookFolderPath);
            _inkMetadataFolder = await _notebookFolder.GetFolderAsync(InkMetadataSubfolderName);
            _backgroundMetadataFolder = await _notebookFolder.GetFolderAsync(BackgroundMetadataSubfolderName);
            var notebookFile = await _notebookFolder.GetFileAsync(NotebookFileName);
            _notebook = await DeserializeNotebook(notebookFile);
        }

        public async Task InitializeLocalNotebookFolder()
        {
            var appFolder = ApplicationData.Current.LocalFolder;
            // create or get the local notebook folder for this notebook
            _notebookFolder = await appFolder.CreateFolderAsync(_notebook.Guid.ToString(), CreationCollisionOption.OpenIfExists);
            // create or open the metadata folders
            _inkMetadataFolder = await _notebookFolder.CreateFolderAsync(InkMetadataSubfolderName, CreationCollisionOption.OpenIfExists);
            _backgroundMetadataFolder = await _notebookFolder.CreateFolderAsync(BackgroundMetadataSubfolderName, CreationCollisionOption.OpenIfExists);
        }

        public async Task SaveNotebook()
        {
            var notebookFile = await _notebookFolder.CreateFileAsync(NotebookFileName, CreationCollisionOption.ReplaceExisting);
            await SerializeNotebook(_notebook, notebookFile);
        }

        public async Task SavePage(Page page, InkStrokeContainer strokeContainer)
        {
            var inkFile = await _inkMetadataFolder.CreateFileAsync(page.InkFileName, CreationCollisionOption.ReplaceExisting);
            await SerializeInkCanvas(strokeContainer, inkFile);
            //todo
            //var backgroundFile = await _inkMetadataFolder.CreateFileAsync(page.BackgroundFileName, CreationCollisionOption.ReplaceExisting);
        }

        public async Task<InkStrokeContainer> LoadPage(Page page)
        {
            var inkFile = await _inkMetadataFolder.GetFileAsync(page.InkFileName);
            var strokeContainer = await DeserializeInkCanvas(inkFile);

            return strokeContainer;
        }

        #endregion

        #region Private Methods

        private async Task SerializeNotebook(Notebook notebook, StorageFile file)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Notebook));

            var stream = await file.OpenStreamForWriteAsync();
            serializer.WriteObject(stream, notebook);
            await stream.FlushAsync();
            stream.Dispose();
        }

        private async Task<Notebook> DeserializeNotebook(StorageFile file)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Notebook));
            Notebook notebook = null;
            using (var stream = await file.OpenStreamForWriteAsync())
            {
                notebook = serializer.ReadObject(stream) as Notebook;
            }

            if (notebook == null)
            {
                throw new FileLoadException("Could not load a notebook from file.", file.Path);
            }

            return notebook;
        }

        private static async Task<bool> SerializeInkCanvas(InkStrokeContainer strokeContainer, StorageFile file)
        {
            // Prevent updates to the file until updates are 
            // finalized with call to CompleteUpdatesAsync.
            CachedFileManager.DeferUpdates(file);
            // Open a file stream for writing.
            var stream = await file.OpenAsync(FileAccessMode.ReadWrite);
            using (var outputStream = stream.GetOutputStreamAt(0))
            {
                // Write the ink strokes to the output stream.
                await strokeContainer.SaveAsync(outputStream);
                await outputStream.FlushAsync();
            }
            stream.Dispose();

            // Finalize write so other apps can update file.
            Windows.Storage.Provider.FileUpdateStatus status =
                await CachedFileManager.CompleteUpdatesAsync(file);

            if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static async Task<InkStrokeContainer> DeserializeInkCanvas(StorageFile file)
        {
            var strokeContainer = new InkStrokeContainer();
            // Open a file stream for reading.
            // Read from file.
            using (var inputStream = await file.OpenAsync(FileAccessMode.Read))
            {
                await strokeContainer.LoadAsync(inputStream);
            }
            return strokeContainer;
        }

        #endregion
    }
}
