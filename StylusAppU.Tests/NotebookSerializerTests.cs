using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using StylusAppU.Data.Data;
using StylusAppU.Data.Serialization;
using Windows.Storage;
using System.Threading.Tasks;

namespace StylusAppU.Tests
{
    [TestClass]
    public class NotebookSerializerTests
    {
        [TestMethod]
        public async Task SaveNotebookTest()
        {
            var notebook = new Notebook("TestNotebook");
            notebook.AddPage();
            notebook.AddPage();

            var notebookSerializer = new NotebookSerializer(notebook);
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(notebook.Name);
            notebookSerializer.InitializeNotebookArchive(file);
            await notebookSerializer.SaveNotebook();

            await file.DeleteAsync();
        }

        [TestMethod]
        public async Task LoadNotebookTest()
        {
            var notebook = new Notebook("TestNotebook");
            notebook.AddPage();
            notebook.AddPage();

            var notebookSerializer = new NotebookSerializer(notebook);
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(notebook.Name);
            notebookSerializer.InitializeNotebookArchive(file);
            await notebookSerializer.SaveNotebook();

            var newNotebookSerializer = new NotebookSerializer();
            await newNotebookSerializer.LoadNotebookArchive(file);

            await file.DeleteAsync();
        }
    }
}
