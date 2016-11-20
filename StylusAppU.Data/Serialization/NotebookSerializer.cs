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

namespace StylusAppU.Data.Serialization
{
    public class NotebookSerializer
    {
        public static readonly string InkMetadataSubfolder = ".ink";
        public static readonly string BackgroundMetadataSubfolder = ".backgrounds";

        private ZipArchive _notebookArchive;

        public NotebookSerializer(Notebook notebook)
        {
            
        }

        public static bool SerializeNotebook(Notebook notebook, ZipArchiveEntry file)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Notebook));

            //serializer.WriteObject(fileStream, notebook);

            return false;
        }

        public static Notebook DeserializeNotebook(StorageFile file)
        {

            return null;
        }

        public static async Task<bool> SerializeInkCanvas(InkStrokeContainer strokeContainer, ZipArchiveEntry file)
        {
            // Prevent updates to the file until updates are 
            // finalized with call to CompleteUpdatesAsync.
            //CachedFileManager.DeferUpdates(file);
            // Open a file stream for writing.
            using (IOutputStream outputStream = file.Open().AsOutputStream())
            {
                // Write the ink strokes to the output stream.
                await strokeContainer.SaveAsync(outputStream);
                await outputStream.FlushAsync();
            }
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

        public static async Task<InkStrokeContainer> DeserializeInkCanvas(ZipArchiveEntry file)
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
