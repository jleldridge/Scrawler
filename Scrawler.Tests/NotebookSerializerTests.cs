using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Scrawler.Data.Data;
using Scrawler.Data.Serialization;
using Windows.Storage;
using System.Threading.Tasks;

namespace Scrawler.Tests
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

            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(notebook.Name);
            await NotebookSerializer.SaveNotebook(notebook, file);

            await file.DeleteAsync();
        }

        [TestMethod]
        public async Task LoadNotebookTest()
        {
            var notebook = new Notebook("TestNotebook");
            notebook.AddPage();
            notebook.AddPage();

            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(notebook.Name);
            await NotebookSerializer.SaveNotebook(notebook, file);

            await file.DeleteAsync();
            await NotebookSerializer.LoadNotebookArchive(file);

            await file.DeleteAsync();
        }
    }
}
