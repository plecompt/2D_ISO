using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace FDF_Monogame
{
    public class Game1 : Game
    {
        //Engine
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        //Graphics
        Texture2D _canvas;
        UInt32[] _pixels;
        int _windowWidth = 600;
        int _windowHeight = 600;

        //Map
        int _mapNumberOfPointsByLine;
        int _mapNumberOfLines;
        string _mapPath = @"Maps\Map1.txt";
        List<List<int>> _map = new List<List<int>>();
        List<Tuple<int, int>> _pointsCoordinates = new List<Tuple<int, int>>();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            _graphics.PreferredBackBufferWidth = _windowWidth;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = _windowHeight;   // set this value to the desired height of your window
            _graphics.ApplyChanges();

            _canvas = new Texture2D(GraphicsDevice, _windowWidth, _windowHeight, false, SurfaceFormat.Color);
            _pixels = new UInt32[_windowWidth * _windowHeight];

            for (int i = 0; i < (_windowHeight * _windowWidth); i++)
                _pixels[i] = 0x000000;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            LoadMap();
            CalculatePointsCoordinates();

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            //GraphicsDevice.Textures[0] = null;

            //Liner();

            base.Update(gameTime);
        }

        protected void LoadMap()
        {
            var lines = File.ReadLines(_mapPath);
            
            //foreach line in lines
            foreach (var line in lines)
            {
                //removing useless characters.
                line.Replace("\r", "");
                //splitting to get each number
                var numbersAsString = line.Split(' ');

                var lineNumbers = new List<int>();
                foreach(var nb in numbersAsString)
                    lineNumbers.Add(Int32.Parse(nb));

                _map.Add(lineNumbers);
            }

            _mapNumberOfLines = _map.Count;
            _mapNumberOfPointsByLine = _map[0].Count;
        }

        protected void CalculatePointsCoordinates()
        {
            int initialYPosition = _windowHeight / 2;
            int initialXPosition = _windowWidth / 2;
            int horizontalDistanceBetweenTwoPoints = (_windowWidth / 2) / (_mapNumberOfPointsByLine - 1);
            int verticalDistanceBetweenTwoPoints = (_windowHeight / 2) / (_mapNumberOfLines - 1);

            for(int i = 0; i < _mapNumberOfLines; i++)
            {
                int initialX = (verticalDistanceBetweenTwoPoints * i);
                int initialY = (_windowHeight / 2) + (verticalDistanceBetweenTwoPoints * i);

                for (int j = 0; j < _mapNumberOfPointsByLine; j++)
                {
                    var yPos = initialY - (verticalDistanceBetweenTwoPoints * j);
                    var xPos = initialX + (horizontalDistanceBetweenTwoPoints * j);
                    _pointsCoordinates.Add(new Tuple<int, int>(xPos, yPos));
                }
            }

            foreach (var point in _pointsCoordinates)
            {
                if (point.Item2 != 600 && point.Item1 != 600)
                {
                    int pixelNumber = (point.Item2 * _windowHeight) + point.Item1;
                    _pixels[pixelNumber] = 0xFFFFFF;
                }
            }

            _canvas.SetData<UInt32>(_pixels, 0, _windowWidth * _windowHeight);

        }

        protected void Liner()
        {
            
        }

        protected override void Draw(GameTime gameTime)
        {
            //cleaning
            GraphicsDevice.Clear(Color.Black);

            //Applying the canva.
            _spriteBatch.Begin();
            _spriteBatch.Draw(_canvas, new Vector2(0, 0), Color.Red);
            _spriteBatch.End();

            //draw
            base.Draw(gameTime);
        }
    }
}
