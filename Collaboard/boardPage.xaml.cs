using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;
using System.IO;
using Microsoft.Win32;
using System.Windows.Ink;

namespace Collaboard
{
    /// <summary>
    /// Interaction logic for boardPage.xaml
    /// </summary>
    public partial class boardPage : Page
    {
        String tempTool;
        bool pressed = false;

        Rectangle rect;
        Ellipse ellipse;
        Line line_C = new Line();
        TextBox tB = new TextBox();

        double radiusX = 0, radiusY = 0;
        
        Point sP = new Point(0, 0);
        Point eP = new Point(0, 0);
        DrawingAttributes newAtr = new DrawingAttributes();

        public boardPage()
        {
            InitializeComponent();
            tempTool = pencilImage.Name;
            boardCanvas.EditingMode = InkCanvasEditingMode.None;
        }

        private void pencilImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pencil.Background = Brushes.Tomato;
            
            boardCanvas.EditingMode = InkCanvasEditingMode.InkAndGesture;
            delete.Visibility = Visibility.Collapsed;
            newAtr.Color = (Color)colorStroke.SelectedColor;
            newAtr.Height = newAtr.Width = slider.Value;
            boardCanvas.DefaultDrawingAttributes = newAtr;
            tempTool = pencilImage.Name;
            eraser.Background = rectangle.Background = line.Background = save.Background = textBox.Background = move.Background = circle.Background = Brushes.LightGray;
        }

        private void eraserImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            eraser.Background = Brushes.Tomato;
            boardCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;

            tempTool = eraserImage.Name;
            pencil.Background = rectangle.Background = line.Background = save.Background = textBox.Background =  move.Background = circle.Background = Brushes.LightGray;

        }

        private void lineImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            boardCanvas.EditingMode = InkCanvasEditingMode.None;
            delete.Visibility = Visibility.Collapsed;
            line.Background = Brushes.Tomato;
            tempTool = lineImage.Name;
            eraser.Background = rectangle.Background = pencil.Background = save.Background = textBox.Background =  move.Background = circle.Background = Brushes.LightGray;

        }

        private void circleImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            boardCanvas.EditingMode = InkCanvasEditingMode.None;
            delete.Visibility = Visibility.Collapsed;
            circle.Background = Brushes.Tomato;
            tempTool = circleImage.Name;
            eraser.Background = rectangle.Background = line.Background = save.Background = textBox.Background = move.Background = pencil.Background = Brushes.LightGray;

        }

        private void rectangleImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            boardCanvas.EditingMode = InkCanvasEditingMode.None;
            delete.Visibility = Visibility.Collapsed;
            rectangle.Background = Brushes.Tomato;
            tempTool = rectangleImage.Name;
            eraser.Background = pencil.Background = line.Background = save.Background = move.Background = textBox.Background = circle.Background = Brushes.LightGray;

        }

        private void moveImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            move.Background = Brushes.Tomato;
            delete.Visibility = Visibility.Visible;
            boardCanvas.EditingMode = InkCanvasEditingMode.Select;
            tempTool = moveImage.Name;
            eraser.Background = rectangle.Background = line.Background = save.Background = pencil.Background = textBox.Background = circle.Background = Brushes.LightGray;

        }

        private void undoImage_MouseDown(object sender, MouseButtonEventArgs e) {
            int count =  boardCanvas.Children.Count;
            delete.Visibility = Visibility.Collapsed;

            if (count >= 1)
            {
                boardCanvas.Children.RemoveAt(count - 1);
            }
        }

        private void deleteImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyBinding k = new KeyBinding();
            k.Key = Key.Delete;
            
        }

        private void textBoxImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            boardCanvas.EditingMode = InkCanvasEditingMode.None;
            delete.Visibility = Visibility.Collapsed;
            textBox.Background = Brushes.Tomato;
            tempTool = textBoxImage.Name;
            eraser.Background = pencil.Background = line.Background = save.Background = move.Background = circle.Background = rectangle.Background = Brushes.LightGray;
        }

        private void saveImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog();
            saveDialog.FileName = "savedimage"; // Default file name
            saveDialog.DefaultExt = ".png"; // Default file extension
            saveDialog.Filter = "Image (.png, .bmp, .jpg)|*.png, *.bmp, *.jpg"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = saveDialog.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string fileName = saveDialog.FileName;
                //get the dimensions of the ink control
                int margin = (int)this.boardCanvas.Margin.Left;
                int width = (int)this.boardCanvas.ActualWidth - margin;
                int height = (int)this.boardCanvas.ActualHeight - margin;
                //render ink to bitmap
                RenderTargetBitmap rtb =
                new RenderTargetBitmap(width, height, 96d, 96d, PixelFormats.Default);
                rtb.Render(boardCanvas);
                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(rtb));
                    encoder.Save(fs);
                }
            }
        }

        private void boardCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pressed = true;
            sP = e.GetPosition(boardCanvas);
            newAtr.Color = (Color)colorStroke.SelectedColor;
            newAtr.Height = newAtr.Width = slider.Value;
            boardCanvas.DefaultDrawingAttributes = newAtr;

            if (tempTool == pencilImage.Name || tempTool == eraserImage.Name || tempTool == circleImage.Name || tempTool == rectangleImage.Name)
            {

               if (tempTool == circleImage.Name)
                {
                    ellipse = new Ellipse();
                }
                else if (tempTool == rectangleImage.Name)
                {
                    rect = new Rectangle();
                }
            }
            else if (tempTool == lineImage.Name)
            {
                line_C = new Line();
            }

            else if (tempTool == textBoxImage.Name) {
                tB = new TextBox();
            }

            eP = new Point(0, 0);

        }

        private void boardCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            pressed = true;
            eP = e.GetPosition(boardCanvas);
            if (tempTool == "rectangleImage" || tempTool == "circleImage")
                inflateRect();
            else if (tempTool == "lineImage")
            {
                eP = e.GetPosition(boardCanvas);
            }
            else if (tempTool == "textBoxImage") {
                eP = e.GetPosition(boardCanvas);
            }
        }

        private void inflateRect()
        {
                   if (eP.X > sP.X && eP.Y > sP.Y)
                    {
                        radiusX = eP.X - sP.X;
                        radiusY = eP.Y - sP.Y;
                    }
                    else if (eP.X > sP.X && eP.Y < sP.Y)
                    {
                        radiusX = eP.X - sP.X;
                        radiusY = sP.Y - eP.Y;
                    }
                    else if (eP.X < sP.X && eP.Y < sP.Y)
                    {
                        radiusX = sP.X - eP.X;
                        radiusY = sP.Y - eP.Y;
                    }
                    else if (eP.X < sP.X && eP.Y > sP.Y)
                    {
                        radiusX = sP.X - eP.X;
                        radiusY = eP.Y - sP.Y;
                    }
            
        }
       
        private void boardCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
                pressed = false;
            Brush sb = (SolidColorBrush)(new BrushConverter().ConvertFrom(colorStroke.SelectedColorText));
            Brush fb = (SolidColorBrush)(new BrushConverter().ConvertFrom(colorFill.SelectedColorText));
            
            if (tempTool == "rectangleImage")
            {

                rect = new Rectangle() { Width = radiusX, Height = radiusY, Stroke = sb, Fill = fb, StrokeThickness = slider.Value };

                if (eP.X > sP.X && eP.Y > sP.Y)
                {
                    InkCanvas.SetTop(rect, sP.Y);
                    InkCanvas.SetLeft(rect, sP.X);
                }
                else if (eP.X > sP.X && eP.Y < sP.Y)
                {
                    InkCanvas.SetTop(rect, eP.Y);
                    InkCanvas.SetLeft(rect, sP.X);
                }
                else if (eP.X < sP.X && eP.Y < sP.Y)
                {
                    InkCanvas.SetTop(rect, eP.Y);
                    InkCanvas.SetLeft(rect, eP.X);
                }
                else if (eP.X < sP.X && eP.Y > sP.Y)
                {
                    InkCanvas.SetTop(rect, sP.Y);
                    InkCanvas.SetLeft(rect, eP.X);
                }
                boardCanvas.Children.Add(rect);
            }
            else if (tempTool == "circleImage")
            {
                ellipse = new Ellipse() { Width = radiusX, Height = radiusY, Stroke = sb, Fill = fb, StrokeThickness = slider.Value };

                if (eP.X > sP.X && eP.Y > sP.Y)
                {
                    InkCanvas.SetTop(ellipse, sP.Y);
                    InkCanvas.SetLeft(ellipse, sP.X);
                }
                else if (eP.X > sP.X && eP.Y < sP.Y)
                {
                    InkCanvas.SetTop(ellipse, eP.Y);
                    InkCanvas.SetLeft(ellipse, sP.X);
                }
                else if (eP.X < sP.X && eP.Y < sP.Y)
                {
                    InkCanvas.SetTop(ellipse, eP.Y);
                    InkCanvas.SetLeft(ellipse, eP.X);
                }
                else if (eP.X < sP.X && eP.Y > sP.Y)
                {
                    InkCanvas.SetTop(ellipse, sP.Y);
                    InkCanvas.SetLeft(ellipse, eP.X);
                }

                boardCanvas.Children.Add(ellipse);
            }
            else if (tempTool == "lineImage")
            {
                line_C = new Line() { Stroke = sb, StrokeThickness = slider.Value };
                line_C.X1 = sP.X;
                line_C.X2 = eP.X;
                line_C.Y1 = sP.Y;
                line_C.Y2 = eP.Y;

                boardCanvas.Children.Add(line_C);
            }

            else if (tempTool == "textBoxImage") {
                tB = new TextBox() {Width= 50, Foreground = sb, FontSize = 15, AcceptsReturn = true, TextWrapping = TextWrapping.Wrap};
                tB.Focus();
                if (eP.X > sP.X && eP.Y > sP.Y)
                {
                    InkCanvas.SetTop(tB, sP.Y);
                    InkCanvas.SetLeft(tB, sP.X);
                }
                else if (eP.X > sP.X && eP.Y < sP.Y)
                {
                    InkCanvas.SetTop(tB, eP.Y);
                    InkCanvas.SetLeft(tB, sP.X);
                }
                else if (eP.X < sP.X && eP.Y < sP.Y)
                {
                    InkCanvas.SetTop(tB, eP.Y);
                    InkCanvas.SetLeft(tB, eP.X);
                }
                else if (eP.X < sP.X && eP.Y > sP.Y)
                {
                    InkCanvas.SetTop(tB, sP.Y);
                    InkCanvas.SetLeft(tB, eP.X);
                }
                boardCanvas.Children.Add(tB);
                
            }
        }
    }

    }