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

        #region Save Notebook

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
                                await SerializeBackgroundImage(imageBackground, archive);
                            }
                        }

                        if (notebook.Defaults.Background is ImageBackground)
                        {
                            var imageBackground = (ImageBackground)notebook.Defaults.Background;
                            await SerializeBackgroundImage(imageBackground, archive);
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
                await SerializeBackgroundImage(imageBackground, archive);
            }
        }

        private static async Task SerializeNotebook(Notebook notebook, ZipArchiveEntry file)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Notebook));

            var stream = file.Open();
            serializer.WriteObject(stream, notebook);
            await stream.FlushAsync();
            stream.Dispose();
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

        private static async Task SerializeBackgroundImage(ImageBackground background, ZipArchive archive)
        {
            var fileName = BackgroundMetadataSubfolderName + "/" + background.ImageFileName;
            if (background.Image == null || archive.GetEntry(fileName) != null)
            {
                return;
            }

            var backgroundImageFile = archive.CreateEntry(fileName);

            // Open a file stream for writing.
            using (var stream = backgroundImageFile.Open().AsRandomAccessStream())
            {
                await background.Image.SaveAsync(stream, CanvasBitmapFileFormat.Png);
            }
        }

        #endregion

        #region Load Notebook

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
                            await LoadPage(notebook, page, archive);
                        }

                        foreach (var background in notebook.SavedPageBackgrounds)
                        {
                            if (background is ImageBackground)
                            {
                                var imageBackground = (ImageBackground)background;
                                imageBackground.Image = await DeserializeBackgroundImage(notebook, imageBackground, archive);
                            }
                        }

                        if (notebook.Defaults.Background is ImageBackground)
                        {
                            var imageBackground = (ImageBackground)notebook.Defaults.Background;
                            imageBackground.Image = await DeserializeBackgroundImage(notebook, imageBackground, archive);
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

        private static async Task LoadPage(Notebook notebook, Page page, ZipArchive archive)
        {
            var inkFile = archive.GetEntry(InkMetadataSubfolderName + "/" + page.InkFileName);
            if (inkFile != null)
            {
                await DeserializeInkCanvas(inkFile, page.StrokeContainer);
            }

            if (page.Background is ImageBackground)
            {
                var imageBackground = page.Background as ImageBackground;
                imageBackground.Image = await DeserializeBackgroundImage(notebook, imageBackground, archive);
            }
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

        private static async Task<CanvasBitmap> DeserializeBackgroundImage(Notebook notebook, ImageBackground background, ZipArchive archive)
        {
            if (notebook.BackgroundImages.ContainsKey(background.ImageFileName))
            {
                return notebook.BackgroundImages[background.ImageFileName];
            }
            var file = archive.GetEntry(BackgroundMetadataSubfolderName + "/" + background.ImageFileName);
            if (file == null) return null;

            using (var stream = file.Open().AsRandomAccessStream())
            {
                if (stream.Size > 0)
                {
                    var image = await CanvasBitmap.LoadAsync(CanvasDevice.GetSharedDevice(), stream);
                    notebook.BackgroundImages[background.ImageFileName] = image;
                    return image;
                }
            }

            return null;
        }

        #endregion

        private static void ClearArchive(ZipArchive archive)
        {
            for (int i = archive.Entries.Count - 1; i >= 0; i--)
            {
                var entry = archive.Entries[i];
                entry.Delete();
            }
        }
    }
}
