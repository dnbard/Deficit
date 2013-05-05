using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework.Graphics;

namespace Deficit.Images
{
    class ImagesManager
    {
        private static object syncRoot = new Object();

        private static ImagesManager _instance;
        private static ImagesManager instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                            _instance = new ImagesManager();
                    }
                }

                return _instance;
            }
        }

        Dictionary<string, Image> images = new Dictionary<string, Image>();

        private ImagesManager()
        {
            LoadImages();
        }

        public static Image Get(string name)
        {
            if (instance.images.ContainsKey(name))
                return instance.images[name];
            return null;
        }

        public void LoadImages()
        {
            const string path = @"images";
            string[] directories = Directory.GetDirectories(path);
            foreach (string directory in directories)
                extractImages(directory);
            extractImages(path);
        }

        private void extractImages(string path)
        {
            const string _path = @"images";
            string[] ImageTypes = { ".jpg", ".jpeg", ".gif", ".png" };

            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                string key = Regex.Replace(file, string.Format(@"{0}\\", _path), String.Empty);
                bool isImage = false;
                foreach (string imageType in ImageTypes)
                    if (Regex.IsMatch(key, imageType))
                    {
                        isImage = true;
                        key = Regex.Replace(key, imageType, String.Empty);
                    }

                if (!isImage) continue;

                FileStream stream = new FileStream(file, FileMode.Open);
                key = Regex.Replace(key, @"\\", @"-");
                key = key.ToLower();
                
                Image texture = Image.FromStream(Program.Game.GraphicsDevice, stream, key);
                if (!images.ContainsKey(key))
                    images.Add(key, texture);
                stream.Close();
            }
        }
    }
}
