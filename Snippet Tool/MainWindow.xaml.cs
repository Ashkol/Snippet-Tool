using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Snippets;
using Utils;
using Drawings;

namespace Snippet_Tool
{
    public partial class MainWindow : Window
    {
        public enum ShapeTool
        {
            None,
            Rectangle,
            Ellipse,
            Freehand,
            Polygon,
            BezierCurve,
            Resize
        }

        public enum BrushColor
        {
            Red,
            Green,
            Blue
        }

        private bool isDragging = false;
        private double startMouseX, startMouseY;
        public ShapeTool chosenTool = ShapeTool.None;
        private BrushColor chosenColor = BrushColor.Red;
        private Polygon freeHandPolygon, polygon;
        private Brush strokeBrush;
        private SolidColorBrush fillBrush;
        private bool isFirstFrameOfDrawing = true;
        private string currentImageName = "";
        private string saveDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"/GK2/";

        private Drawer drawer;

        List<Snippet> snippets = new List<Snippet>();
        List<string> descList = new List<string>();
        private List<CroppedBitmap> croppedImages;

        #region Initialization

        public MainWindow()
        {
            InitializeComponent();
            InitializeBrushes();
            croppedImages = new List<CroppedBitmap>();
            InitializeDrawer();
        }

        private void InitializeBrushes()
        {
            strokeBrush = Brushes.Red;
            fillBrush = new SolidColorBrush();
            fillBrush.Opacity = 0.5f;
            fillBrush.Color = Color.FromArgb(255, 255, 0, 0);
        }

        private void InitializeDrawer()
        {
            drawer = new Drawer(ref cnvEditor, ref imgEditor, this);
            drawer.fillBrush = new SolidColorBrush();
            drawer.fillBrush.Opacity = 0.5f;
            drawer.fillBrush.Color = Color.FromArgb(255, 255, 0, 0);
            drawer.strokeBrush = Brushes.Red;
        }

        #endregion

        #region Toolbar Interaction

        private void MnItmLoadFromFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            Uri fileUri = null;
            if (openFileDialog.ShowDialog() == true)
            {
                fileUri = new Uri(openFileDialog.FileName);
                imgEditor.Source = new BitmapImage(fileUri);
                currentImageName = System.IO.Path.GetFileName(fileUri.LocalPath);
                CroppedList.Items.Clear();
                cnvEditor.Children.Clear();
                croppedImages.Clear();
                snippets.Clear();
                descList.Clear();
                Deserializer.DeserializeXML(this, currentImageName, saveDirectory, ref snippets, ref croppedImages);
            }
        }

        private void MnItmSaveAllSnippets_Click(object sender, RoutedEventArgs e)
        {
            Deserializer.SerializeSnippets(CroppedList, cnvEditor, currentImageName);
        }

        private void RedColor_Click(object sender, RoutedEventArgs e)
        {
            drawer.strokeBrush = Brushes.Red;
            drawer.fillBrush = new SolidColorBrush();
            drawer.fillBrush.Opacity = 0.5f;
            drawer.fillBrush.Color = Color.FromArgb(255, 255, 0, 0);
            CurrentColorBlock.Fill = drawer.strokeBrush;
            drawer.chosenColor = BrushColor.Red;
        }

        private void GreenColor_Click(object sender, RoutedEventArgs e)
        {
            drawer.strokeBrush = Brushes.Green;
            drawer.fillBrush = new SolidColorBrush();
            drawer.fillBrush.Opacity = 0.5f;
            drawer.fillBrush.Color = Color.FromArgb(255, 0, 255, 0);
            CurrentColorBlock.Fill = drawer.strokeBrush;
            drawer.chosenColor = BrushColor.Green;

        }

        private void BlueColor_Click(object sender, RoutedEventArgs e)
        {
            drawer.strokeBrush = Brushes.Blue;
            drawer.fillBrush = new SolidColorBrush();
            drawer.fillBrush.Opacity = 0.5f;
            drawer.fillBrush.Color = Color.FromArgb(255, 0, 0, 255);
            CurrentColorBlock.Fill = drawer.strokeBrush;
            drawer.chosenColor = BrushColor.Blue;
        }

        private void RectangleTool_Click(object sender, RoutedEventArgs e)
        {
            chosenTool = ShapeTool.Rectangle;
            foreach (UIElement uiElement in cnvEditor.Children)
            {
                if (uiElement.GetType().Equals(typeof(ContentControl)))
                {
                    ((ContentControl)uiElement).IsHitTestVisible = false;
                }
            }
        }

        private void EllipseTool_Click(object sender, RoutedEventArgs e)
        {
            chosenTool = ShapeTool.Ellipse;
            foreach (UIElement uiElement in cnvEditor.Children)
            {
                if (uiElement.GetType().Equals(typeof(ContentControl)))
                {
                    ((ContentControl)uiElement).IsHitTestVisible = false;
                }
            }
        }

        private void PolygonTool_Click(object sender, RoutedEventArgs e)
        {
            chosenTool = ShapeTool.Polygon;
            polygon = new Polygon();
            foreach (UIElement uiElement in cnvEditor.Children)
            {
                if (uiElement.GetType().Equals(typeof(ContentControl)))
                {
                    ((ContentControl)uiElement).IsHitTestVisible = false;
                }
            }
        }

        private void FreeTool_Click(object sender, RoutedEventArgs e)
        {
            chosenTool = ShapeTool.Freehand;
            foreach (UIElement uiElement in cnvEditor.Children)
            {
                if (uiElement.GetType().Equals(typeof(ContentControl)))
                {
                    ((ContentControl)uiElement).IsHitTestVisible = false;
                }
            }
        }

        private void ResizeTool_Click(object sender, RoutedEventArgs e)
        {
            chosenTool = ShapeTool.Resize;
            foreach (UIElement uiElement in cnvEditor.Children)
            {
                if (uiElement.GetType().Equals(typeof(ContentControl)))
                {
                    ((ContentControl)uiElement).IsHitTestVisible = true;
                }
            }
        }

        #endregion

        #region Canvas Interaction

        private void ImgEditor_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                switch (chosenTool)
                {
                    case ShapeTool.Rectangle:
                        drawer.DrawRectangle(e, isFirstFrameOfDrawing);
                        break;
                    case ShapeTool.Ellipse:
                        drawer.DrawEllipse(e, isFirstFrameOfDrawing);
                        break;
                    case ShapeTool.Freehand:
                        drawer.DrawFreehand(e, ref freeHandPolygon, isFirstFrameOfDrawing);
                        break;
                    default:
                        break;
                }
                isFirstFrameOfDrawing = false;
            }
            else if (chosenTool == ShapeTool.Polygon)
            {
                drawer.DrawPolygon(e, ref polygon, false);
            }
        }

        private void CnvEditor_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Cursor = Cursors.Hand;
            ImgEditor_MouseDown(sender, e);
        }

        private void CnvEditor_LeftMouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
            if (isDragging == true)
            {
                isDragging = false;
                isFirstFrameOfDrawing = true;
            }
        }

        private void CnvEditor_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                switch (chosenTool)
                {
                    case ShapeTool.Rectangle:
                        drawer.DrawRectangle(e, isFirstFrameOfDrawing);
                        break;
                    case ShapeTool.Ellipse:
                        drawer.DrawEllipse(e, isFirstFrameOfDrawing);
                        break;
                    case ShapeTool.Freehand:
                        drawer.DrawFreehand(e, ref freeHandPolygon, isFirstFrameOfDrawing);
                        break;
                    default:
                        break;
                }
                isFirstFrameOfDrawing = false;
            }
            else if (chosenTool == ShapeTool.Polygon)
            {
                drawer.DrawPolygon(e, ref polygon, false);
            }
            if (System.Windows.Input.Mouse.LeftButton != MouseButtonState.Pressed)
            {
                if (isDragging == true)
                {
                    isDragging = false;
                    isFirstFrameOfDrawing = true;
                    this.Cursor = Cursors.Arrow;
                }
            }
        }

        private void CnvEditor_MouseRightDown(object sender, MouseButtonEventArgs e)
        {
            IInputElement clickedElement = Mouse.DirectlyOver;
            if (clickedElement is Rectangle)
            {
                ContextMenu cm = this.FindResource("contextMenuButton") as ContextMenu;
                cm.PlacementTarget = sender as Button;
                cm.IsOpen = true;
            }
            else if (clickedElement is Ellipse)
            {
                ContextMenu cm = this.FindResource("contextMenuButton") as ContextMenu;
                cm.PlacementTarget = sender as Button;
                cm.IsOpen = true;
            }
            else if (clickedElement is Polygon)
            {
                ContextMenu cm = this.FindResource("contextMenuButton") as ContextMenu;
                cm.PlacementTarget = sender as Button;
                cm.IsOpen = true;
            }
        }

        private void ImgEditor_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
            if (isDragging == true)
            {
                isDragging = false;
                isFirstFrameOfDrawing = true;
            }
        }

        private void ImgEditor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Cursor = Cursors.Hand;
            if (isDragging == false)
            {
                isDragging = true;
                startMouseX = e.GetPosition(imgEditor).X;
                startMouseY = e.GetPosition(imgEditor).Y;
                drawer.startMouseX = e.GetPosition(imgEditor).X;
                drawer.startMouseY = e.GetPosition(imgEditor).Y;
            }
            if (chosenTool == ShapeTool.Freehand)
            {
                freeHandPolygon = new Polygon();
            }
            if (chosenTool == ShapeTool.Polygon)
            {
                Console.WriteLine("Drawing polygon");
                drawer.DrawPolygon(e, ref polygon, true);
            }
        }

        private void AddShapeToSnippets_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu cm = this.FindResource("contextMenuButton") as ContextMenu;
            cm.IsOpen = false;
            IInputElement clickedShape = Mouse.DirectlyOver;

            if (clickedShape is Rectangle || clickedShape is Ellipse)
            {
                Shape shape = (Shape)clickedShape;
                ContentControl control = new ContentControl();
                control.Template = (ControlTemplate)FindResource("DesignerItemTemplate");
                control.Width = shape.Width;
                control.Height = shape.Height;
                cnvEditor.Children.Add(control);
                cnvEditor.Children.Remove(shape);
                foreach (UIElement child in cnvEditor.Children)
                {
                    if (child.GetType().Equals(typeof(ContentControl)))
                    {

                        if (((ContentControl)child).Content != null && ((ContentControl)child).Content.Equals(shape))
                        {
                            cnvEditor.Children.Remove(child);
                            break;
                        }
                    }
                }
                if (Canvas.GetLeft(shape).Equals(Double.NaN))
                {
                    Canvas.SetRight(control, Canvas.GetRight(shape));
                }
                else
                    Canvas.SetLeft(control, Canvas.GetLeft(shape));
                if (Canvas.GetTop(shape).Equals(Double.NaN))
                {
                    Canvas.SetBottom(control, Canvas.GetBottom(shape));
                }
                else
                    Canvas.SetTop(control, Canvas.GetTop(shape));
                control.Content = shape;
                if (chosenTool != ShapeTool.Resize)
                {
                    control.IsHitTestVisible = false;
                }

                CroppedBitmap croppedImage = Cropper.Crop(control, this);
                croppedImages.Add(croppedImage);

                bool isNewPanelAdded = true;
                foreach (SnippetStackPanel panel in CroppedList.Items)
                {
                    if (panel.shape.Equals(shape))
                    {
                        panel.SetImage(Cropper.Crop(shape, this));
                        isNewPanelAdded = false;
                        break;
                    }

                }

                if (isNewPanelAdded)
                {
                    SnippetStackPanel snippetPanel = new SnippetStackPanel((croppedImage), shape, "", CroppedList.Items.Count);
                    snippetPanel.deleteButton.Click += BtnDeleteSnippet_Click;
                    snippetPanel.MouseLeftButtonUp += StackPanel_MouseUp;
                    snippetPanel.color = drawer.chosenColor;
                    CroppedList.Items.Add(snippetPanel);
                }
            }
            if (clickedShape is Polygon)
            {
                CroppedBitmap croppedImage = Cropper.Crop((UIElement)clickedShape, this);
                croppedImages.Add(croppedImage);

                SnippetStackPanel snippetPanel = new SnippetStackPanel((croppedImage), (Polygon)clickedShape, "", croppedImages.Count - 1);
                snippetPanel.deleteButton.Click += BtnDeleteSnippet_Click;
                snippetPanel.MouseLeftButtonUp += StackPanel_MouseUp;
                snippetPanel.color = drawer.chosenColor;
                CroppedList.Items.Add(snippetPanel);
            }
        }

        #endregion

        #region Snippets Panel Interaction

        public void BtnDeleteSnippet_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Deleting snippet");
            int imageIndex = (int)((Button)sender).Tag;

            //cnvEditor.Children.Remove(((SnippetStackPanel)CroppedList.Items[imageIndex]).shape);
            CroppedList.Items.RemoveAt(imageIndex);
            croppedImages.RemoveAt(imageIndex);
            cnvEditor.Children.RemoveAt(imageIndex);

            for (int i = 0; i < CroppedList.Items.Count; i++)
            {
                ((SnippetStackPanel)CroppedList.Items[i]).UpdateIndex(i);
            }
        }

        public void StackPanel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // Color changing function
            int snippetIndex = (int)((SnippetStackPanel)sender).Index;
            for (int i = 0; i < CroppedList.Items.Count; i++)
            {
                if (((SnippetStackPanel)sender).shape.Stroke != Brushes.Orange && snippetIndex == (i))
                {
                    ((SnippetStackPanel)CroppedList.Items[i]).shape.Stroke = Brushes.Orange;
                    ((SnippetStackPanel)CroppedList.Items[i]).shape.Fill = Brushes.Orange;
                    ((SnippetStackPanel)CroppedList.Items[i]).shape.Opacity = 0.7;
                }
                else
                {
                    ((SnippetStackPanel)CroppedList.Items[i]).shape.Stroke = GetBrushColor(((SnippetStackPanel)(CroppedList.Items[i])).color);
                    ((SnippetStackPanel)CroppedList.Items[i]).shape.Fill = GetBrushColorFill(((SnippetStackPanel)(CroppedList.Items[i])).color);
                    ((SnippetStackPanel)CroppedList.Items[i]).shape.Opacity = 0.7;
                }
            }
        }

        #endregion

        #region Brushes
        private BrushColor GetBrushColorEnum(Brush brush)
        {
            if (brush == Brushes.Green)
                return BrushColor.Green;
            if (brush == Brushes.Blue)
                return BrushColor.Blue;
            else
                return BrushColor.Red;
        }

        private Brush GetBrushColor(BrushColor enumerated)
        {
            if (enumerated == BrushColor.Green)
                return Brushes.Green;
            if (enumerated == BrushColor.Blue)
                return Brushes.Blue;
            else
                return Brushes.Red;
        }

        private SolidColorBrush GetBrushColorFill(BrushColor enumerated)
        {
            SolidColorBrush brush = new SolidColorBrush();
            brush.Opacity = 0.7f;
            switch (enumerated)
            {
                case BrushColor.Red:
                    brush.Color = Color.FromArgb(255, 255, 0, 0);
                    break;
                case BrushColor.Green:
                    brush.Color = Color.FromArgb(255, 0, 255, 0);
                    break;
                case BrushColor.Blue:
                    brush.Color = Color.FromArgb(255, 0, 0, 255);
                    break;
                default:
                    brush.Color = Color.FromArgb(255, 0, 0, 0);
                    brush.Opacity = 0.7;
                    break;
            }

            return brush;
        }
        #endregion

        #region Unused Methods
        // Possible further use, not necessary for assignment
        private void MnItmSaveAllCutImage_Click(object sender, RoutedEventArgs e)
        {
            int i = 1;
            string basicPath = @"C:\Users\adams\Desktop\test";
            foreach (CroppedBitmap cImage in croppedImages)
            {
                SaveImage((BitmapSource)cImage, basicPath + i + ".jpg");
                i++;
            }
        }

        private void BtnDeleteCutImage_Click(object sender, RoutedEventArgs e)
        {
            int imageIndex = (int)((Button)sender).Tag;
            CroppedList.Items.RemoveAt(imageIndex);
            croppedImages.RemoveAt(imageIndex);
        }

        private void SaveImage(BitmapSource image, string filePath)
        {
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            using (System.IO.FileStream stream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                encoder.Save(stream);
        }

        private void SerializeSnippets(List<Shape> shapes, List<string> descriptions)
        {
            List<Snippet> shapeSnippets = new List<Snippet>();
            for (int i = 0; i < shapes.Count; i++)
            {
                Snippet snippet;
                if (shapes[i].GetType() == typeof(Rectangle))
                {
                    snippet = new RectangleSnippet();
                }
                else if (shapes[i].GetType() == typeof(Ellipse))
                {
                    snippet = new EllipseSnippet();
                }
                else
                {
                    snippet = new FreehandSnippet();
                }

                snippet.SetAttributes(shapes[i], cnvEditor);
                snippet.description = ((SnippetStackPanel)CroppedList.Items[i]).description.Text;
                snippet.colorEnum = (int)GetBrushColorEnum(shapes[i].Stroke);

                shapeSnippets.Add(snippet);
            }

            System.Xml.Serialization.XmlSerializer writer =
            new System.Xml.Serialization.XmlSerializer(typeof(List<Snippet>));

            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\GK2\" + currentImageName.Split('.')[0] + ".xml";
            System.IO.FileStream file = System.IO.File.Create(path);
            MessageBox.Show(path);
            writer.Serialize(file, shapeSnippets);
            file.Close();
        }

        #endregion
    }
}
