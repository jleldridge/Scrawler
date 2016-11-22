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
        private Notebook _notebook;
        private StorageFolder _notebookFolder, _inkMetadataFolder, _backgroundMetadataFolder;

        public NotebookSerializer(Notebook notebook)
        {
            _notebook = notebook;
        }

        public async void InitializeLocalNotebookFolder()
        {
            var appFolder = ApplicationData.Current.LocalFolder;
            // create or get the local notebook folder for this notebook
            _notebookFolder = await appFolder.CreateFolderAsync(_notebook.Guid.ToString(), CreationCollisionOption.OpenIfExists);
            // create or open the metadata folders
            _inkMetadataFolder = await _notebookFolder.CreateFolderAsync(InkMetadataSubfolderName, CreationCollisionOption.OpenIfExists);
            _backgroundMetadataFolder = await _notebookFolder.CreateFolderAsync(BackgroundMetadataSubfolderName, CreationCollisionOption.OpenIfExists);
        }

        public async void SaveNotebook()
        {
            var notebookFile = await _notebookFolder.CreateFileAsync(_notebook.Name, CreationCollisionOption.ReplaceExisting);
            SerializeNotebook(_notebook, notebookFile);
        }

        public async void SavePage(Page page, InkStrokeContainer strokeContainer)
        {
            var inkFile = await _inkMetadataFolder.CreateFileAsync(page.InkFileName, CreationCollisionOption.ReplaceExisting);
            await SerializeInkCanvas(strokeContainer, inkFile);
            //todo
            //var backgroundFile = await _inkMetadataFolder.CreateFileAsync(page.BackgroundFileName, CreationCollisionOption.ReplaceExisting);
        }

        private static void SerializeNotebook(Notebook notebook, StorageFile file)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Notebook));
        }

        private static Notebook DeserializeNotebook(StorageFile file)
        {
            return null;
        }

        private static async Task<bool> SerializeInkCanvas(InkStrokeContainer strokeContainer, StorageFile file)
        {
            // Prevent updates to the file until updates are 
            // finalized with call to CompleteUpdatesAsync.
            //CachedFileManager.DeferUpdates(file);
            // Open a file stream for writing.
            //using (IOutputStream outputStream = file.Open().AsOutputStream())
            //{
            //    // Write the ink strokes to the output stream.
            //    await strokeContainer.SaveAsync(outputStream);
            //    await outputStream.FlushAsync();
            //}
            //stream.Dispose();

            // Finalize write so other apps can update file.
            //Windows.Storage.Provider.FileUpdateStatus status =
            //    await CachedFileManager.CompleteUpdatesAsync(file);

            //if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            return true;
        }

        private static async Task<InkStrokeContainer> DeserializeInkCanvas(ZipArchiveEntry file)
        {
            var strokeContainer = new InkStrokeContainer();
            // Open a file stream for reading.
            // Read from file.
            using (var inputStream = file.Open().AsInputStream())
            {
                await strokeContainer.LoadAsync(inputStream);
            }
            return strokeContainer;
        }
    }
}
