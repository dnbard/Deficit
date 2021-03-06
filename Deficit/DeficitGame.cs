﻿#region Using Statements
using System;
using System.Collections.Generic;
using System.IO;
using Deficit.Scenes;
using Deficit.core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace Deficit
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class DeficitGame : Game
    {
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        public DateTime GameDate { get; set; }

        public Vector2 Viewport
        {
            get { return new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight); }
        }

        public DeficitGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            GraphicsDevice.Clear(Color.Black);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            GraphicsDevice.Clear(Color.Black);
            Window.Title = "Deficit";
            Window.AllowUserResizing = false;
            Components.Add(MouseManager.Instance);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            SceneManager.Current = new SceneMainMenu();

            base.Initialize();
            LoadEffects();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public TimeSpan GameTime { get; set; }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            GameTime = gameTime.TotalGameTime;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.F4))
            {
                graphics.IsFullScreen = !graphics.IsFullScreen;
                graphics.ApplyChanges();
            }

            base.Update(gameTime);
            SceneManager.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            SceneManager.Draw(gameTime);

            base.Draw(gameTime);
        }

        private void LoadEffects()
        {
            //var file = new FileStream("Content/Effect/GaussianBlur.xnb",FileMode.Open);
            //byte[] buffer = new byte[file.Length];
            //file.Read(buffer, 0, (int)file.Length);
            //var effect = new Effect(GraphicsDevice, buffer);
            //file.Close();
        }

    }
}
