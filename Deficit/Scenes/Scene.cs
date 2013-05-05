using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Deficit.Scenes
{
    class Scene : DrawableGameComponent
    {
        public string Name = "SceneDefaultName";

        protected List<DrawableGameComponent> Components = new List<DrawableGameComponent>();
        public int Count { get { return Components.Count; } }

        public EventHandler ElementsChanged;
        public Action<Vector2> OnMouseClick;
        public Action<Vector2> OnMouseHover;
        public Action<Keys[]> OnKeyDown;

        protected TimeSpan LastAction = TimeSpan.FromMilliseconds(0);

        public Scene(string _name)
            : base(Program.Game)
        {
            Name = _name;
        }

        public void Remove(DrawableGameComponent[] elements)
        {
            foreach (var element in elements)
            {
                if (Components.Contains(element))
                    Components.Remove(element);
            }
            if (ElementsChanged != null) ElementsChanged.Invoke(this, EventArgs.Empty);
        }
        
        public void Remove(DrawableGameComponent element)
        {
            if (Components.Contains(element))
                Components.Remove(element);
            if (ElementsChanged != null) ElementsChanged.Invoke(this, EventArgs.Empty);
        }

        public void Add(DrawableGameComponent[] elements)
        {
            foreach (var element in elements)
            {
                if (!Components.Contains(element))
                    Components.Add(element);
            }
            if (ElementsChanged != null) ElementsChanged.Invoke(this, EventArgs.Empty);
        }

        public void Add(DrawableGameComponent element)
        {
            if (!Components.Contains(element))
                Components.Add(element);
            if (ElementsChanged != null) ElementsChanged.Invoke(this, EventArgs.Empty);
        }

        protected Texture2D GetTexture()
        {
            Vector2 ViewPort = Program.Game.Viewport;

            RenderTarget2D maskRenderTarget = new RenderTarget2D(GraphicsDevice, (int)ViewPort.X, (int)ViewPort.Y);
            GraphicsDevice.SetRenderTarget(maskRenderTarget);
            GraphicsDevice.Clear(Color.Black);

            Draw(new GameTime());

            GraphicsDevice.SetRenderTarget(null);
            return maskRenderTarget;
        }

        const int PassCount = 1;
        public virtual Texture2D DrawToTexture()
        {
            Texture2D result = GetTexture();
            Vector2 ViewPort = Program.Game.Viewport;
            SpriteBatch gr = Program.Game.spriteBatch;
            //Effect PostProcessing = Game.Content.Load<Effect>("Shaders/Blur");

            for (int i = 0; i < PassCount; i++)
            {
                RenderTarget2D maskRenderTarget = new RenderTarget2D(GraphicsDevice, (int)ViewPort.X, (int)ViewPort.Y);
                GraphicsDevice.SetRenderTarget(maskRenderTarget);
                GraphicsDevice.Clear(Color.Black);

                gr.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null);
                gr.Draw(result, Vector2.Zero, Color.White);
                gr.End();

                GraphicsDevice.SetRenderTarget(null);

                result = maskRenderTarget;
            }
            GC.Collect();

            return result;
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch batch = Program.Game.spriteBatch;

            batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            Components.ForEach(element => element.Draw(gameTime));
            batch.End();
        }

        protected bool UpdateElements
        {
            get { return Enabled; }
        }

        /// <summary>
        /// Количество апдейтов, в которых сцена не чувствительна к изменениям.
        /// Используется при переходах между сценами.
        /// </summary>
        private int GracePeriod = 1;
        public override void Update(GameTime gameTime)
        {
            if (GracePeriod > 0) { GracePeriod--; return; }
            base.Update(gameTime);
            MouseState MouseState = Mouse.GetState();

            if (UpdateElements)
            {
                for (int i = 0; i < Components.Count; i++)
                {
                    DrawableGameComponent element = Components[i];
                    element.Update(gameTime);
                }
            }

            if (OnMouseClick != null)
                if (MouseState.LeftButton == ButtonState.Pressed && gameTime.TotalGameTime - LastAction > TimeSpan.FromSeconds(1))
                {
                    OnMouseClick(new Vector2(MouseState.X, MouseState.Y));
                    LastAction = gameTime.TotalGameTime;
                }

            if (OnMouseHover != null)
                OnMouseHover(new Vector2(MouseState.X, MouseState.Y));

            if (OnKeyDown != null)
            {
                KeyboardState KeyState = Keyboard.GetState();
                Keys[] pressed = KeyState.GetPressedKeys();
                if (pressed.Length > 0) OnKeyDown(pressed);
            }
        }
    }
}
