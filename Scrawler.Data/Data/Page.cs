using System;
using System.Runtime.Serialization;
using Windows.UI;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml.Media.Imaging;
using Scrawler.Data.Serialization;

namespace Scrawler.Data.Data
{
    [DataContract]
    public class Page : IEquatable<Page>, IDeepCopiable<Page>
    {
        internal Page(Defaults defaults)
        {
            Guid = Guid.NewGuid();
            InkFileName = Guid.ToString();
            Width = defaults.PageWidth;
            Height = defaults.PageHeight;
            Background = defaults.Background.GetDeepCopy();
            StrokeContainer = new InkStrokeContainer();
        }

        private Page(){}

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (Width == 0) Width = 800;
            if (Height == 0) Height = 600;
            if (Background == null) Background = new SolidBackground();
            StrokeContainer = new InkStrokeContainer();
        }

        [DataMember]
        public double Width { get; set; }

        [DataMember]
        public double Height { get; set; }

        [DataMember]
        public Guid Guid { get; set; }

        [DataMember]
        public string InkFileName { get; set; }

        [DataMember]
        public BackgroundBase Background { get; set; }

        public InkStrokeContainer StrokeContainer { get; set; }

        public bool Equals(Page other)
        {
            return other.Guid.Equals(Guid) 
                && other.InkFileName.Equals(InkFileName);
        }

        public Page GetDeepCopy()
        {
            var copy = new Page();
            copy.Width = Width;
            copy.Height = Height;
            copy.Guid = Guid.NewGuid();
            copy.InkFileName = copy.Guid.ToString();
            copy.Background = Background.GetDeepCopy();
            copy.StrokeContainer = new InkStrokeContainer();
            //todo make sure this actually works
            copy.StrokeContainer.AddStrokes(StrokeContainer.GetStrokes());

            return copy;
        }
    }
}
