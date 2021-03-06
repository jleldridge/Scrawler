﻿using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Windows.UI;

namespace Scrawler.Data.Data
{
    [DataContract]
    public class Notebook : IEquatable<Notebook>, IDeepCopyable<Notebook>
    {
        public Notebook(string name)
        {
            Name = name;

            Pages = new List<Page>();
            Guid = Guid.NewGuid();
            SavedColors = new List<Color>() { new Color() { A = 255 } };
            SavedPageBackgrounds = new List<BackgroundBase>();
            Defaults = new Defaults();
            BackgroundImages = new Dictionary<string, CanvasBitmap>();
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (SavedColors == null)
            {
                SavedColors = new List<Color>() { new Color() { A = 255 } };
            }
            if (SavedPageBackgrounds == null)
            {
                SavedPageBackgrounds = new List<BackgroundBase>();
            }
            if (Defaults == null)
            {
                Defaults = new Defaults();
            }

            BackgroundImages = new Dictionary<string, CanvasBitmap>();
        }

        [DataMember]
        public Guid Guid { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<Page> Pages { get; set; }

        [DataMember]
        public List<BackgroundBase> SavedPageBackgrounds { get; set; }

        [DataMember]
        public List<Color> SavedColors { get; set; }

        [DataMember]
        public Defaults Defaults { get; set; }

        public Dictionary<string, CanvasBitmap> BackgroundImages;

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

        public Notebook GetDeepCopy()
        {
            var copy = new Notebook(Name);

            foreach (var page in Pages)
            {
                copy.Pages.Add(page.GetDeepCopy());
            }

            foreach (var background in SavedPageBackgrounds)
            {
                copy.SavedPageBackgrounds.Add(background.GetDeepCopy());
            }

            copy.SavedColors.Clear();
            foreach (var color in SavedColors)
            {
                copy.SavedColors.Add(color);
            }

            copy.Defaults = Defaults.GetDeepCopy();

            return copy;
        }
    }
}
