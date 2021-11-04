using Myra.Graphics2D.UI;
using CustomProgram;
using Myra.Graphics2D.Brushes;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using System;
using LocalResouces;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Myra.Graphics2D;

namespace MonoCustomProgram
{
    class Controller
    {
        private TextButton _playPauseButton, _resetButton, _viewModeButton, _cloneButton, _brushModeButton;
        private HorizontalSlider _brushSizeSlider, _brushStrengthSlider, _thermalMidpointSlider;

        private int _mouseScrollPosition = 0;
        private BlockBrush _blockBrush;
        private EraserBrush _eraserBrush;
        private TemperatureBrush _hotTemperatureBrush;
        private TemperatureBrush _coldTemperatureBrush;
        public Controller()
        {
            _blockBrush = new BlockBrush(2, 1);
            _blockBrush.AlterBrush(3, 1);
            _eraserBrush = new EraserBrush(2, 1);
            _hotTemperatureBrush = new TemperatureBrush(2, 1, 500);
            _coldTemperatureBrush = new TemperatureBrush(2, 1, -1*500);
        }
        /// <summary>
        /// The selected block form the block brush
        /// </summary>
        public string SelectedBlock 
        { get 
            { 
                
                return (_blockBrush.CurrentlySelectedBlock as Block).Name; 
            } 
        }
        /// <summary>
        /// The selected view mode from the view mode button
        /// </summary>
        public string ViewMode 
        { get 
            { 
                if (_viewModeButton.IsPressed)
                {
                    return "Temperature Mode";
                }
                return "Block Mode";
            } 
        }
        /// <summary>
        /// The selected brush mode from the brush mode button
        /// </summary>
        public string BrushMode 
        { get 
            {
                if (_brushModeButton.IsPressed)
                {
                    return "Temperature Painting";
                }
                return "Block Painting";
            } 
        }
        /// <summary>
        /// Creates the UI
        /// </summary>
        public void CreateUI(Desktop desktop, IModel model, SpriteFont font, int UiHeight, int UiWidth, int UiTop, int UiLeft) //ran in load content
        {
            //Creates the Myra Grid which holdes Myra Widgets
            var myraUiGrid = new Myra.Graphics2D.UI.Grid
            {
                //ShowGridLines = true,
                RowSpacing = 8,
                ColumnSpacing = 8,
                Left = UiLeft,
                Top = UiTop,
                Width = UiWidth,
                Height = UiHeight,
                Background = new SolidBrush("#00000000")
            };

            myraUiGrid.ColumnsProportions.Add(new Proportion(ProportionType.Part));
            myraUiGrid.RowsProportions.Add(new Proportion(ProportionType.Part));

            // Create buttons
            _playPauseButton = new TextButton
            {
                Font = font,
                GridColumn = 0,
                GridRow = 0,
                Text = "Play / Pause",
                Toggleable = true
            };
            _resetButton = new TextButton
            {
                Font = font,
                GridColumn = 0,
                GridRow = 1,
                Text = "Reset"
            };
            _viewModeButton = new TextButton
            {
                Font = font,
                GridColumn = 1,
                GridRow = 0,
                Text = "Next view mode",
                Toggleable = true
            };
            _brushModeButton = new TextButton
            {
                Font = font,
                GridColumn = 2,
                GridRow = 0,
                Text = "Toggle Brush Mode",
                Toggleable = true
            };
            _cloneButton = new TextButton
            {
                Font = font,
                GridColumn = 1,
                GridRow = 1,
                Text = "Clone Block",
                Toggleable = true
            };
            // Add functionality on click
            _viewModeButton.Click += (s, a) =>
            {
                //Debug.WriteLine("Change View Mode Clicked");
                model.ChangeDisplayType();
            };
            _cloneButton.Click += (s, a) =>
            {
                _blockBrush.CloneBlockPlacement = _cloneButton.IsPressed;
            };
            _resetButton.Click += (s, a) =>
            {
                //Debug.WriteLine("Reset button Clicked");
                model.CreateMap();
            };
            // Add buttons to Grid
            myraUiGrid.Widgets.Add(_playPauseButton);
            myraUiGrid.Widgets.Add(_resetButton);
            myraUiGrid.Widgets.Add(_viewModeButton);
            myraUiGrid.Widgets.Add(_brushModeButton);
            myraUiGrid.Widgets.Add(_cloneButton);

            // Create lables
            var brushSizeLabel = new Label
            {
                Font = font,
                TextColor = Color.Black,
                Text = "Brush Size ",
                GridColumn = 3,
                GridRow = 0,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            var brushStrengthLabel = new Label
            {
                Font = font,
                TextColor = Color.Black,
                Text = "Brush Strength ",
                GridColumn = 3,
                GridRow = 1,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            var thermalMidpointLabel = new Label
            {
                Font = font,
                TextColor = Color.Black,
                Text = "Thermal Midpoint ",
                GridColumn = 5,
                GridRow = 0,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            // Add labels to Grid
            myraUiGrid.Widgets.Add(brushSizeLabel);
            myraUiGrid.Widgets.Add(brushStrengthLabel);
            myraUiGrid.Widgets.Add(thermalMidpointLabel);

            // Create the sliders
            _brushSizeSlider = new HorizontalSlider
            {
                Minimum = 0,
                Maximum = 10,
                Value = 2,
                GridColumn = 4,
                GridRow = 0
            };
            _brushStrengthSlider = new HorizontalSlider
            {
                Minimum = 0,
                Maximum = 1,
                Value = 1,
                GridColumn = 4,
                GridRow = 1
            };
            _thermalMidpointSlider = new HorizontalSlider
            {
                Minimum = 0,
                Maximum = 2000,
                Value = 500,
                GridColumn = 6,
                GridRow = 0
            };
            // Action on 
            _brushSizeSlider.ValueChanged += (s, a) => 
            {

                int valueInt = (int)_brushSizeSlider.Value;
                _blockBrush.AlterBrush(valueInt, _brushStrengthSlider.Value);
                _eraserBrush.AlterBrush(valueInt, 1);
                _hotTemperatureBrush.AlterBrush(valueInt, 1);
                _coldTemperatureBrush.AlterBrush(valueInt, 1);
            };
            _brushStrengthSlider.ValueChanged += (s, a) => 
            {
                _blockBrush.AlterBrush((int)_brushSizeSlider.Value, _brushStrengthSlider.Value);
                float tempmag = (float)GeneralResources.Map(_brushStrengthSlider.Value, 0, 1, 10, 500);
                _hotTemperatureBrush.TemperatureDifferential = tempmag;
                _coldTemperatureBrush.TemperatureDifferential = tempmag * -1;
            };
            _thermalMidpointSlider.ValueChanged += (s, a) => 
            {
                model.TemperatureMidpoint = (int)_thermalMidpointSlider.Value;
            };
            // Add sliders to Grid
            myraUiGrid.Widgets.Add(_brushSizeSlider);
            myraUiGrid.Widgets.Add(_brushStrengthSlider);
            myraUiGrid.Widgets.Add(_thermalMidpointSlider);

            // Add it to the desktop
            desktop.Root = myraUiGrid;
        }
        /// <summary>
        ///     Processes user input
        /// </summary>
        public void ProcessInput(IModel model, MouseState mouseState)
        {
            HandleScrollWheel(mouseState);
            HandleMouseOnGrid(model, mouseState);
        }
        /// <summary>
        /// Ticks the model if game is unpaused
        /// </summary>
        public void TickModel(IModel model)
        {
            if (_playPauseButton.IsPressed)
            {
                model.ProcessUserInput();
            } else
            {
                model.Tick();
            }
        }
        private void HandleMouseOnGrid(IModel model, MouseState mouseState)
        {
            AbsoluteCoordinate mousePos = new AbsoluteCoordinate(mouseState.X, mouseState.Y);
            if (model.CheckMouseOnGrid(mousePos))
            {
                if (_brushModeButton.IsPressed) //On Temperature mode
                {
                    
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        _hotTemperatureBrush.Draw(model, mousePos);
                    }
                    else if (mouseState.RightButton == ButtonState.Pressed)
                    {
                        _coldTemperatureBrush.Draw(model, mousePos);
                    }
                } else //On Block Mode
                {
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        _blockBrush.Draw(model, mousePos);
                    }
                    else if (mouseState.RightButton == ButtonState.Pressed)
                    {
                        _eraserBrush.Draw(model, mousePos);
                    }
                }
            }
        }
        private void HandleScrollWheel(MouseState mouseState)
        {
            if (mouseState.ScrollWheelValue < _mouseScrollPosition)
            {
                _mouseScrollPosition = mouseState.ScrollWheelValue;
                _blockBrush.IncrementSelectedBlock();
            }
            else if (mouseState.ScrollWheelValue > _mouseScrollPosition)
            {
                _mouseScrollPosition = mouseState.ScrollWheelValue;
                _blockBrush.DecrementSelectedBlock();
            }

        }
    }
}
