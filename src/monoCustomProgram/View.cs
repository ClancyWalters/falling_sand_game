using CustomProgram;
using CustomProgram.Blocks.Temperature;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Myra.Graphics2D.UI;
using System;

namespace MonoCustomProgram
{
    class View
    {
        /// <summary>
        /// Draws each block on the grid.
        /// </summary>
        public void DrawGrid(IModel model, SpriteBatch spriteBatch)
        {
            var colorList = model.Display();
            for (int y = 0; y < colorList.Count; y++)
            {
                for (int x = 0; x < colorList[y].Count; x++)
                {
                    spriteBatch.FillRectangle(model.Origin.X + x * model.Scale, model.Origin.Y + y * model.Scale, model.Scale, model.Scale, new Color((int)colorList[y][x].R, (int)colorList[y][x].G, (int)colorList[y][x].B));
                }
            }
            
        }
        /// <summary>
        /// Draws the UI
        /// </summary>
        public void DrawUI(Desktop desktop)
        {
            desktop.Render();
        }
        /// <summary>
        /// Draws the text information at the bottom of the window
        /// </summary>
        public void DrawControllerInfo(Controller control, SpriteBatch spriteBatch, SpriteFont font, int height, int leftIndent, int textHeight)
        {
            int fontLineSpacing = font.LineSpacing;
            int fontPadding = (textHeight - fontLineSpacing)/2;
            int spacing = 50;

            string brushMode = "Brush Mode: " + control.BrushMode;
            string viewMode = "View Mode: " + control.ViewMode;
            string selectedBlock = "Selected Block: " + control.SelectedBlock;

            int brushModeLength = (int)font.MeasureString(brushMode).X;
            int viewModeLength = (int)font.MeasureString(viewMode).X;

            spriteBatch.DrawString(font, brushMode, new Vector2(leftIndent, height + fontPadding), Color.Black);
            spriteBatch.DrawString(font, viewMode, new Vector2(leftIndent + brushModeLength + spacing, height + fontPadding), Color.Black);
            spriteBatch.DrawString(font, selectedBlock, new Vector2(leftIndent + viewModeLength + brushModeLength + spacing*2, height + fontPadding), Color.Black);
        }
        /// <summary>
        /// Draws contextual information for the currently hovered over block
        /// </summary>
        public void DrawHoverInfo(IModel model, SpriteBatch spriteBatch, SpriteFont font, GameTime gametime, MouseState mouseState) //needs mouse pos
        {
            int linespacing = font.LineSpacing;
            
            //performance metrics
            spriteBatch.DrawString(font,"ms/t: " + Math.Round(gametime.ElapsedGameTime.TotalMilliseconds, 2) + " Fps: " + Math.Round(1/gametime.ElapsedGameTime.TotalSeconds, 4), new Vector2(12+ model.Scale, 10+ model.Scale), Color.Black);
            spriteBatch.DrawString(font, "PX: " + mouseState.X + " PY: " + mouseState.Y, new Vector2(12 + model.Scale, 10 + model.Scale + linespacing), Color.Black);

            if (model.CheckMouseOnGrid(mouseState.X, mouseState.Y))
            {
                GridCoordinate coord = new AbsoluteCoordinate(mouseState.X, mouseState.Y).GetGridCoordinate(model.Scale, model.Origin);
                Block block = model.GetBlock(coord);
                spriteBatch.DrawString(font, "GX: " + coord.X + " GY: " + coord.Y, new Vector2(12 + model.Scale, 10 + model.Scale + linespacing*2), Color.Black);
                spriteBatch.DrawString(font, "Block: " + block.Name, new Vector2(12 + model.Scale, 10 + model.Scale + linespacing*3), Color.Black);

                if (block is IPublicTemperature)
                {
                    spriteBatch.DrawString(font, "Temp: " + (block as IPublicTemperature).Temperature + "K", new Vector2(12 + model.Scale, 10 + model.Scale + linespacing*4), Color.Black);
                    if (block is IPublicDensity)
                    {
                        spriteBatch.DrawString(font, "Density: " + (block as IPublicDensity).Density, new Vector2(12 + model.Scale, 10 + model.Scale + linespacing*5), Color.Black);
                    }
                }
            }
        }
    }
}
