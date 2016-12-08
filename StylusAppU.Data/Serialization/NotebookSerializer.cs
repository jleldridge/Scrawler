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
using System.Threading;

namespace StylusAppU.Data.Serialization
{
    public class NotebookSerializer
    {
        public static readonly string InkMetadataSubfolderName = ".ink";
        public static readonly string BackgroundMetadataSubfolderName = ".backgrounds";
        public static readonly string NotebookFileName = "Notebook.json";
        public static readonly string CurrentNotebookKey = "CurrentNotebook";

        public StorageFile NotebookArchiveFile { get; set; }
        public SemaphoreSlim NotebookFileSemaphore = new SemaphoreSlim(1,1);

        private Notebook _notebook;

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

        public void InitializeNotebookArchive(StorageFile file)
        {
            NotebookArchiveFile = file;
        }

        public async Task LoadNotebookArchive(StorageFile file)
        {
            await NotebookFileSemaphore.WaitAsync();
            try
            {
                NotebookArchiveFile = file;
                using (var stream = await NotebookArchiveFile.OpenStreamForReadAsync())
                {
                    using (var archive = new ZipArchive(stream, ZipArchiveMode.Read))
                    {
                        var notebookFile = archive.GetEntry(NotebookFileName);
                        _notebook = DeserializeNotebook(notebookFile);
                    }
                }
            }
            finally
            {
                NotebookFileSemaphore.Release();
            }
        }

        public async Task SaveNotebook()
        {
            await NotebookFileSemaphore.WaitAsync();
            try
            {    
                using (var stream = await NotebookArchiveFile.OpenStreamForWriteAsync())
                {
                    using (var archive = new ZipArchive(stream, ZipArchiveMode.Create))
                    {
                        var notebookFile = archive.GetEntry(NotebookFileName);
                        await SerializeNotebook(_notebook, notebookFile);

                        foreach (var page in _notebook.Pages)
                        {
                            await SavePage(page, archive);
                        }
                    }
                }
            }
            finally
            {
                NotebookFileSemaphore.Release();
            }
        }

        private async Task SavePage(Page page, ZipArchive archive)
        {
            var inkFile = archive.GetEntry(InkMetadataSubfolderName + "/" + page.InkFileName);
            await SerializeInkCanvas(page.StrokeContainer, inkFile);
            //todo: serialize background file
        }

        public async Task<InkStrokeContainer> LoadPage(Page page)
        {
            if (NotebookArchiveFile == null) return null;

            await NotebookFileSemaphore.WaitAsync();
            try
            {    
                using (var stream = await NotebookArchiveFile.OpenAsync(FileAccessMode.ReadWrite))
                {    
                    using (var archive = new ZipArchive(stream.AsStream(), ZipArchiveMode.Update))
                    {
                        var inkFile = archive.GetEntry(InkMetadataSubfolderName + "/" + page.InkFileName);
                        if (inkFile == null) return null;

                        var strokeContainer = await DeserializeInkCanvas(inkFile);
                        return strokeContainer;
                    }
                }
            }
            finally
            {
                NotebookFileSemaphore.Release();
            }
        }

        #endregion

        #region Private Methods

        private async Task SerializeNotebook(Notebook notebook, ZipArchiveEntry file)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Notebook));

            var stream = file.Open();
            serializer.WriteObject(stream, notebook);
            await stream.FlushAsync();
            stream.Dispose();
        }

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

        private static async Task SerializeInkCanvas(InkStrokeContainer strokeContainer, ZipArchiveEntry file)
        {
            // Open a file stream for writing.
            using (var outputStream = file.Open().AsOutputStream())
            {
                // Write the ink strokes to the output stream.
                await strokeContainer.SaveAsync(outputStream);
                await outputStream.FlushAsync();
            }
        }

        private static async Task<InkStrokeContainer> DeserializeInkCanvas(ZipArchiveEntry file)
        {
            var strokeContainer = new InkStrokeContainer();
            // Open a file stream for reading.
            // Read from file.
            using (var inputStream = file.Open().AsRandomAccessStream())
            {
                await strokeContainer.LoadAsync(inputStream);
            }
            return strokeContainer;
        }

        #endregion
    }
}
