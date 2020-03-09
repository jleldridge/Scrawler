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
    public class NotebookSerializer
    {
        public static readonly string InkMetadataSubfolderName = ".ink";
        public static readonly string BackgroundMetadataSubfolderName = ".backgrounds";
        public static readonly string NotebookFileName = "Notebook.json";
        public static readonly string CurrentNotebookKey = "CurrentNotebook";

        public static SemaphoreSlim NotebookFileSemaphore = new SemaphoreSlim(1,1);

        #region Public Methods

        public static async Task<Notebook> LoadNotebookArchive(StorageFile file)
        {
            await NotebookFileSemaphore.WaitAsync();
            try
            {
                using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    using (var archive = new ZipArchive(stream.AsStream(), ZipArchiveMode.Update))
                    {
                        var notebookFile = archive.GetEntry(NotebookFileName);
                        var notebook = DeserializeNotebook(notebookFile);

                        foreach (var page in notebook.Pages)
                        {
                            await LoadPage(page, archive);
                        }

                        foreach (var background in notebook.SavedPageBackgrounds)
                        {
                            if (background is ImageBackground)
                            {
                                var imageBackground = (ImageBackground)background;
                                imageBackground.Image = await DeserializeBackgroundImage(
                                    archive.GetEntry(BackgroundMetadataSubfolderName + "/" + imageBackground.ImageFileName));
                            }
                        }

                        if (notebook.Defaults.Background is ImageBackground)
                        {
                            var imageBackground = (ImageBackground)notebook.Defaults.Background;
                            imageBackground.Image = await DeserializeBackgroundImage(
                                    archive.GetEntry(BackgroundMetadataSubfolderName + "/" + imageBackground.ImageFileName));
                        }

                        return notebook;
                    }
                }
            }
            finally
            {
                NotebookFileSemaphore.Release();
            }
        }

        public static async Task SaveNotebook(Notebook notebook, StorageFile file)
        {
            await NotebookFileSemaphore.WaitAsync();
            try
            {    
                using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    using (var archive = new ZipArchive(stream.AsStream(), ZipArchiveMode.Update))
                    {
                        ClearArchive(archive);

                        var notebookFile = archive.CreateEntry(NotebookFileName);
                        await SerializeNotebook(notebook, notebookFile);

                        foreach (var page in notebook.Pages)
                        {
                            await SavePage(page, archive);
                        }

                        foreach (var background in notebook.SavedPageBackgrounds)
                        {
                            if (background is ImageBackground)
                            {
                                var imageBackground = (ImageBackground)background;
                                var backgroundImageFile = archive.CreateEntry(BackgroundMetadataSubfolderName + "/" + imageBackground.ImageFileName);
                                await SerializeBackgroundImage(imageBackground.Image, backgroundImageFile);
                            }
                        }

                        if (notebook.Defaults.Background is ImageBackground)
                        {
                            var imageBackground = (ImageBackground)notebook.Defaults.Background;
                            var backgroundImageFile = archive.CreateEntry(BackgroundMetadataSubfolderName + "/" + imageBackground.ImageFileName);
                            await SerializeBackgroundImage(imageBackground.Image, backgroundImageFile);
                        }
                    }
                }
            }
            finally
            {
                NotebookFileSemaphore.Release();
            }
        }

        private static async Task SavePage(Page page, ZipArchive archive)
        {
            var inkFile = archive.CreateEntry(InkMetadataSubfolderName + "/" + page.InkFileName);
            await SerializeInkCanvas(page.StrokeContainer, inkFile);
            //todo: serialize background file
            if (page.Background is ImageBackground)
            {
                var imageBackground = page.Background as ImageBackground;
                if (imageBackground.Image != null)
                {
                    var backgroundImageFile = archive.CreateEntry(BackgroundMetadataSubfolderName + "/" + imageBackground.ImageFileName);
                    await SerializeBackgroundImage(imageBackground.Image, backgroundImageFile);
                }
            }
        }

        private static async Task LoadPage(Page page, ZipArchive archive)
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

        private static async Task SerializeNotebook(Notebook notebook, ZipArchiveEntry file)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Notebook));

            var stream = file.Open();
            serializer.WriteObject(stream, notebook);
            await stream.FlushAsync();
            stream.Dispose();
        }

        private static Notebook DeserializeNotebook(ZipArchiveEntry file)
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

        private static async Task SerializeInkCanvas(InkStrokeContainer strokeContainer, ZipArchiveEntry file)
        {
            if (strokeContainer.GetStrokes().Any())
            {    
                // Open a file stream for writing.
                using (var outputStream = file.Open().AsOutputStream())
                {
                    // Write the ink strokes to the output stream.
                    await strokeContainer.SaveAsync(outputStream);
                    await outputStream.FlushAsync();
                }
            }
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

        private static async Task SerializeBackgroundImage(CanvasBitmap background, ZipArchiveEntry file)
        {
            // Open a file stream for writing.
            using (var stream = file.Open().AsRandomAccessStream())
            {
                await background.SaveAsync(stream, CanvasBitmapFileFormat.Png);
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

        private static void ClearArchive(ZipArchive archive)
        {
            for (int i = archive.Entries.Count - 1; i >= 0; i--)
            {
                var entry = archive.Entries[i];
                entry.Delete();
            }
        }

        #endregion
    }
}
