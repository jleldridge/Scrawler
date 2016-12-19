using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Windows.UI;

namespace StylusAppU.Data.Data
{
    [DataContract]
    public class Notebook : IEquatable<Notebook>
    {
        public Notebook(string name)
        {
            Name = name;

            Pages = new List<Page>();
            Guid = Guid.NewGuid();
            SavedColors = new List<Color>() { new Color() { A = 255 } };
            Defaults = new Defaults();
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (SavedColors == null)
            {
                SavedColors = new List<Color>() { new Color() { A = 255 } };
            }
            if (Defaults == null)
            {
                Defaults = new Defaults();
            }
        }

        [DataMember]
        public Guid Guid { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<Page> Pages { get; set; }

        [DataMember]
        public List<Color> SavedColors { get; set; }

        [DataMember]
        public Defaults Defaults { get; set; }

        public Page AddPage()
        {
            var page = new Page(Defaults);
            Pages.Add(page);
            return page;
        }

        public bool Equals(Notebook other)
        {
            return other.Guid.Equals(Guid) 
                && other.Name.Equals(Name);
        }
    }
}
