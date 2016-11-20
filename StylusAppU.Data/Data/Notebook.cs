using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

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
        }

        [DataMember]
        public Guid Guid { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<Page> Pages { get; set; }

        public Page AddPage()
        {
            var page = new Page();
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
