using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Deficit.Extentions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using System.Xml.Linq;

namespace Deficit.Images
{
    class Image: Texture2D
    {
        List<RectangleName> Rectangles = new List<RectangleName>();
        Dictionary<string, int> AnimationFrames = new Dictionary<string, int>();

        /// <summary>
        /// Use FromStream method instead of this constructor
        /// </summary>
        /// <param name="gDevice"></param>
        public Image(Texture2D texture)
            : base(texture.GraphicsDevice, texture.Width, texture.Height)
        {
            int[] pixels = new int[texture.Width * texture.Height];
            texture.GetData<int>(pixels);
            this.SetData<int>(pixels);
        }

        static public Image FromStream(GraphicsDevice gDevice, FileStream File, string key)
        {
            Texture2D texture = Texture2D.FromStream(gDevice, File);
            Image newImage = new Image(texture);
            //Image newImage = Texture2D.FromStream(gDevice, File) as Image;

            newImage.Rectangles.Add(new RectangleName() { Rect = new Rectangle(0, 0, newImage.Width, newImage.Height), Name = "full" });

            XDocument AtlasInformation = XDocument.Load("XML\\AtlasInfo.xml");

            foreach (XElement element in AtlasInformation.Root.Elements())
                if (element.Name == key)
                {
                    foreach (XElement SubTexture in element.Elements())
                    {
                        if (SubTexture.Name == "texture")
                        {
                            string SubTextureName = SubTexture.GetAttributeValue("name"); //SubTexture.Attribute("name").Value;
                            int Left = SubTexture.GetAttributeValue("left", "0").AsInt(); //Int32.Parse(SubTexture.Attribute("left").Value);
                            int Top = SubTexture.GetAttributeValue("top", "0").AsInt(); //Int32.Parse(SubTexture.Attribute("top").Value);
                            int Width = SubTexture.GetAttributeValue("width").AsInt();  //Int32.Parse(SubTexture.Attribute("width").Value);
                            int Height = SubTexture.GetAttributeValue("height").AsInt(); //Int32.Parse(SubTexture.Attribute("height").Value);
                            newImage.Rectangles.Add(new RectangleName() { Rect = new Rectangle(Left, Top, Width, Height), Name = SubTextureName });
                        }
                        else if (SubTexture.Name == "animation")
                        {
                            string SubTextureName = SubTexture.GetAttributeValue("name"); //SubTexture.Attribute("name").Value;
                            int Left = SubTexture.GetAttributeValue("left", "0").AsInt();
                            int Top = SubTexture.GetAttributeValue("top", "0").AsInt();
                            int Width = SubTexture.GetAttributeValue("width").AsInt();
                            int Height = SubTexture.GetAttributeValue("height").AsInt();
                            int horframes = SubTexture.GetAttributeValue("horframes", "1").AsInt();
                            int vertframes = SubTexture.GetAttributeValue("vertframes", "1").AsInt();

                            int maxFrames = SubTexture.GetAttributeValue("maxframes").AsInt();
                            if (maxFrames == 0) maxFrames = horframes*vertframes;

                            int counter = 0;
                            for (int j = 0; j < vertframes; j++)
                            {
                                for (int i = 0; i < horframes; i++)
                                {
                                    string texturename = string.Format("{0}{1}", SubTextureName, counter);
                                    newImage.Rectangles.Add(new RectangleName()
                                        {
                                            Rect = new Rectangle(Left + Width*i, Top + Height*j, Width, Height),
                                            Name = texturename
                                        });
                                    counter++;
                                    if (counter == maxFrames) break;
                                }
                                if (counter == maxFrames) break;
                            }
                            newImage.AnimationFrames.Add(SubTextureName, horframes * vertframes);
                        }
                        else if (SubTexture.Name == "framescount")
                            newImage.AnimationFrames.Add(SubTexture.GetAttributeValue("name"), SubTexture.GetAttributeValue("value", "0").AsInt());
                    }
                    break;
                }

            texture.Dispose();
            return newImage;
        }

        public int GetNumberOfFrames(string key)
        {
            foreach (KeyValuePair<string, int> pair in AnimationFrames)
                if (Regex.IsMatch(pair.Key, key)) return pair.Value;

            return 0;
        }

        public void DrawPartial(SpriteBatch Batch, string Key, Rectangle Dest, Color Overlay)
        {
            Rectangle Source = GetSourceRect(Key);
            Batch.Draw(this, Dest, new Rectangle(Source.X, Source.Y + Source.Height - Dest.Height, Source.Width, Dest.Height), Overlay);
        }

        public void Draw(SpriteBatch Batch, string Key, Rectangle Dest, Color Overlay, float Layer = 1)
        {
            Batch.Draw(this, Dest, GetSourceRect(Key), Overlay, 0f, Vector2.Zero, SpriteEffects.None, Layer);
        }

        public void Draw(SpriteBatch Batch, string Key, Rectangle Dest, Rectangle Source, Color Overlay, float Layer = 1)
        {
            Batch.Draw(this, Dest, Source, Overlay, 0f, Vector2.Zero, SpriteEffects.None, Layer);
        }

        public void Draw(SpriteBatch Batch, string Key, Rectangle Dest, Color Overlay, SpriteEffects effect, float Layer = 1)
        {
            Batch.Draw(this, Dest, GetSourceRect(Key), Overlay, 0f, Vector2.Zero, effect, Layer);
        }

        public void Draw(SpriteBatch Batch, string Key, Vector2 position, Color Overlay, float Layer = 1)
        {
            Batch.Draw(this, position, GetSourceRect(Key), Overlay, 0f, Vector2.Zero, 1f, SpriteEffects.None, Layer);
        }

        public void Draw(SpriteBatch Batch, string Key, Vector2 position, float Rotation, Vector2 Origin, Color Overlay, float Layer = 1)
        {
            Batch.Draw(this, position, GetSourceRect(Key), Overlay, Rotation, Origin, 1f, SpriteEffects.None, Layer);
        }

        public void Draw(SpriteBatch Batch, string Key, Vector2 position, float Rotation, float Scale, Vector2 Origin, Color Overlay, float Layer = 1)
        {
            Batch.Draw(this, position, GetSourceRect(Key), Overlay, Rotation, Origin, Scale, SpriteEffects.None, Layer);
        }

        public void Draw(SpriteBatch Batch, string Key, int X, int Y, Color Overlay)
        {
            Batch.Draw(this, new Vector2(X, Y), GetSourceRect(Key), Overlay);
        }

        public void Draw(SpriteBatch Batch, string Key, int X, int Y, Color Overlay, SpriteEffects effect, float layer)
        {
            Batch.Draw(this, new Vector2(X, Y), GetSourceRect(Key), Overlay, 0f, Vector2.Zero, 1f, effect, layer);
        }

        public void Draw(SpriteBatch Batch, string Key, int X, int Y, Color Overlay, float Layer)
        {
            Batch.Draw(this, new Vector2(X, Y), GetSourceRect(Key), Overlay, 0f, Vector2.Zero, 1f, SpriteEffects.None, Layer);
        }

        public void Draw(SpriteBatch Batch, string Key, int X, int Y, float HorizontalScale, Color Overlay)
        {
            Rectangle Source = GetSourceRect(Key);
            Source.Width = (int)(Source.Width * HorizontalScale);
            Batch.Draw(this, new Vector2(X, Y), Source, Overlay);
        }

        public Rectangle GetSourceRect(string key)
        {
            long hash = key.GetHashCode();

            for (int index = 0; index < Rectangles.Count; index++)
            {
                RectangleName rect = Rectangles[index];
                if (rect.Hash == hash)
                    return rect.Rect;
            }
            return Rectangle.Empty;
        }

        public bool ContainsKey(string key)
        {
            long Hash = key.GetHashCode();

            foreach (RectangleName rect in Rectangles)
                if (rect.Hash == Hash)
                    return true;
            return false;
        }
    }
}
