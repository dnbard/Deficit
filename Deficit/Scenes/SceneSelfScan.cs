using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Deficit.GUI;
using Deficit.Images;
using Deficit.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deficit.Scenes
{
    class SceneSelfScan: Scene
    {
        public SceneSelfScan() : base("selfscan")
        {
            Add(new VisualComponent
            {
                Texture = ImagesManager.Get("intro-bg3"),
                Layer = 0.99f
            });
            Add(new FlickerImage
            {
                Texture = ImagesManager.Get("scan-outer"),
                Layer = 0.95f,
                OpacityMinimum = 0.75f,
                OpacityMaximum = 0.85f,
                FirstColor = Color.Green,
                SecondColor = Color.YellowGreen,
                OpacityIncrementValue = 0.0005f
            });
            Add(new FlickerImage
            {
                Texture = ImagesManager.Get("scan-inner"),
                Layer = 0.90f,
                OpacityMinimum = 0.75f,
                OpacityMaximum = 0.95f,
                FirstColor = Color.LightCyan,
                SecondColor = Color.Wheat,
                OpacityIncrementValue = 0.0005f
            });

            var xml = XDocument.Load("XML/ships.xml");
            foreach (var section in xml.Root.Element("selfscan").Elements())
            {
                Add(new SelfScanOverlay
                    {
                        Texture = ImagesManager.Get("scan-hotzones"),
                        TextureKey = section.Attribute("name").Value,
                        X = Int32.Parse(section.Attribute("x").Value),
                        Y = Int32.Parse(section.Attribute("y").Value),
                        OnMouseIn = (sender, args) =>
                            {
                                var self = sender as ShipOverlay;
                                if (self != null)
                                    self.Overlay = Color.Green;
                            },
                        OnMouseOut = (sender, args) =>
                            {
                                var self = sender as ShipOverlay;
                                if (self != null)
                                    self.Overlay = Color.White;
                            },
                        Layer = 0.55f,
                        Opacity = 0.5f,
                        OpacityMinimum = 0.5f,
                        OpacityMaximum = 0.75f,
                        FirstColor = Color.White,
                        SecondColor = Color.Wheat,
                        HoverWidth = Int32.Parse(section.Attribute("hoverx").Value),
                        HoverHeight = Int32.Parse(section.Attribute("hovery").Value),
                        TextOffset = new Vector2(Int32.Parse(section.Attribute("infox").Value),
                            Int32.Parse(section.Attribute("infoy").Value)),
                        Text = section.Attribute("caption").Value
                    });
            }

            foreach (var component in DrawableComponents)
                component.Initialize();
        }
    }
}
