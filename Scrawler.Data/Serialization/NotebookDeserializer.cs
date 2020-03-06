using System;
using Scrawler.Data.Data;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Input.Inking;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using Microsoft.Graphics.Canvas;

namespace Scrawler.Data.Serialization
{
    public class NotebookDeserializer
    {
        public static readonly string InkMetadataSubfolderName = ".ink";
        public static readonly string BackgroundMetadataSubfolderName = ".backgrounds";
        public static readonly string NotebookFileName = "Notebook.json";
        public static readonly string CurrentNotebookKey = "CurrentNotebook";

        public StorageFile NotebookArchiveFile { get; set; }
        public SemaphoreSlim NotebookFileSemaphore = new SemaphoreSlim(1, 1);

        private Notebook _notebook;

        #region Constructor

        public NotebookDeserializer()
        {
        }

        #endregion

        #region properties

        public Notebook Notebook { get { return _notebook; } }

        #endregion

        #region Public Methods

        public async Task LoadNotebookArchive(StorageFile file)
        {
            await NotebookFileSemaphore.WaitAsync();
            try
            {
                NotebookArchiveFile = file;
                using (var stream = await NotebookArchiveFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    using (var archive = new ZipArchive(stream.AsStream(), ZipArchiveMode.Update))
                    {
                        var notebookFile = archive.GetEntry(NotebookFileName);
                        _notebook = DeserializeNotebook(notebookFile);

                        foreach (var page in _notebook.Pages)
                        {
                            await LoadPage(page, archive);
                        }
                    }
                }
            }
            finally
            {
                NotebookFileSemaphore.Release();
            }
        }

        private async Task LoadPage(Page page, ZipArchive archive)
        {
            var inkFile = archive.GetEntry(InkMetadataSubfolderName + "/" + page.InkFileName);
            if (inkFile != null)
            {
                await DeserializeInkCanvas(inkFile, page.StrokeContainer);
            }

            if (page.Background is ImageBackground)
            {
                var imageBackground = page.Background as ImageBackground;
                var backgroundFile = archive.GetEntry(BackgroundMetadataSubfolderName + "/" + imageBackground.ImageFileName);
                if (backgroundFile != null)
                {
                    imageBackground.Image = await DeserializeBackgroundImage(backgroundFile);
                }
            }
        }

        #endregion

        #region Private Methods

        private Notebook DeserializeNotebook(ZipArchiveEntry file)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Notebook));
            Notebook notebook = null;
            using (var stream = file.Open())
            {
                notebook = serializer.ReadObject(stream) as Notebook;
            }

            if (notebook == null)
            {
                throw new FileLoadException("Could not load a notebook from file.", NotebookFileName);
            }

            return notebook;
        }

        private static async Task DeserializeInkCanvas(ZipArchiveEntry file, InkStrokeContainer container)
        {
            // Open a file stream for reading.
            // Read from file.
            using (var inputStream = file.Open().AsRandomAccessStream())
            {
                if (inputStream.Size > 0)
                {
                    await container.LoadAsync(inputStream);
                }
            }
        }

        private static async Task<CanvasBitmap> DeserializeBackgroundImage(ZipArchiveEntry file)
        {
            using (var stream = file.Open().AsRandomAccessStream())
            {
                if (stream.Size > 0)
                {
                    return await CanvasBitmap.LoadAsync(CanvasDevice.GetSharedDevice(), stream);
                }
            }

            return null;
        }

        #endregion
    }
}
