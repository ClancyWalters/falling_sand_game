using CustomProgram;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D.UI;
using System;

namespace MonoCustomProgram
{
    public class MonoMain : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Controller _controller;
        private IModel _model;
        private View _view;

        private Desktop _desktop; //From Myra

        private SpriteFont _defaultFont;
        private SpriteFont _hoverInfoFont;

        private MouseState _mouseState;
        public MonoMain()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsFixedTimeStep = false;

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //Set Screen Res
            _graphics.PreferredBackBufferWidth = 1020;
            _graphics.PreferredBackBufferHeight = 860;
            _graphics.ApplyChanges();

            _controller = new Controller();
            _model = new CustomProgram.Grid(new AbsoluteCoordinate(10, 10), 10, 1000, 750);
            _view = new View();

            _mouseState = Mouse.GetState();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            MyraEnvironment.Game = this;
            _desktop = new Desktop();

            _defaultFont = Content.Load<SpriteFont>("Fonts/DefaultFont");
            _hoverInfoFont = Content.Load<SpriteFont>("Fonts/HoverInfoFont");

            _controller.CreateUI(_desktop, _model, _defaultFont, 60, 1000, 770, 10);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            
            _mouseState = Mouse.GetState();
            _controller.ProcessInput(_model, _mouseState);
            //Update model

            _controller.TickModel(_model);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here

            //Draw UI
            _view.DrawUI(_desktop);

            //Draw Model
            _spriteBatch.Begin();
            _view.DrawGrid(_model, _spriteBatch);
            _view.DrawHoverInfo(_model, _spriteBatch, _hoverInfoFont, gameTime, _mouseState);
            _view.DrawControllerInfo(_controller, _spriteBatch, _defaultFont, 830, 10, 30);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
