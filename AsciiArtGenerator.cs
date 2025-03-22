using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Resources;

namespace AsciiArtGenerator
{
    public class AsciiArtGenerator : Form
    {
        private Panel scrollablePanel;
        private PictureBox outputPictureBox;
        private TextBox fontSizeInput;
        private ComboBox fontDropdown;
        private ComboBox scaleOptionsDropdown;
        private RichTextBox asciiTextBox;
        private Bitmap inputImage;
        private Font defaultFont;
        private string selectedFontName = "Courier New";
        int fontSize = 12; // Default size 12 
        //
        private Button loadImageButton;
        private Button generateAsciiButton;
        private Button textColorButton;
        private Button backgroundColorButton;
        private Button saveAsciiButton;
        private Button saveImageButton;
        private Button outputSizeButton;
        private Button ZoomPlusButton;
        private Button ZoomMinusButton;
        //
        private Label textColorLbl;
        private Label backColorLbl;
        private Label imageSizeLbl;
        private Label imageWidthLbl;
        private Label xLbl;
        private Label imageHeightLbl;
        private Label outputSizeLbl;
        private Label outputWidthLbl;
        private Label x2Lbl;
        private Label outputHeightLbl;
        private Label scaleOptionsLbl;
        //
        private int inputImageWidth;
        private int inputImageHeight;
        private int outputImageWidth;
        private int outputImageHeight;
        private int charWidth;
        private int scaleOption;
        private int zoomFactor = 100;
        //
        private Color textColor = Color.Black;
        private Color backgroundColor = Color.White;
        string asciiArt;
        private bool useTransparentBackground = false;
        /// <summary>
        /// AsciiArtGenerator
        /// </summary>
        /// 
        /// Simple little program to take an image and convert it to ASCII art.
        /// 
        /// Created by: Jerry L. Price Jester Development Inc. Copyright 2025
        /// 
        /// Feel free to use this program as you see fit. If you find it useful, please consider donating to the creator.
        public AsciiArtGenerator()
        {
            this.Icon = Properties.Resources.JesterIcon;
            this.Text = "ASCII Art Generator";
            this.Size = new Size(1230, 900);
            defaultFont = new Font("Arial", 10, FontStyle.Regular);
            inputImageWidth = 1024;
            inputImageHeight = 1024;
            outputImageWidth = 1024;
            outputImageHeight = 1024;

            #region UI Controls
            loadImageButton = new Button { Text = "Load Image", Location = new Point(4, 4), Font = defaultFont, Size = new Size(100, 25) };
        
            textColorButton = new Button { Text = "Fore Color", Location = new Point(108, 4), Font = defaultFont,Size = new Size(100, 25) };
            textColorLbl = new Label { Text = "",ForeColor = textColor, Location = new Point(212, 5),Size   = new Size(40, 20), BackColor = backgroundColor, Font = defaultFont, BorderStyle = BorderStyle.Fixed3D };
        
            backgroundColorButton = new Button { Text = "Back Color", Location = new Point(250, 4), Size = new Size(100, 25), BackColor = backgroundColor, Font = defaultFont };
            backColorLbl = new Label { Text = "", ForeColor = textColor, Location = new Point(354, 5), Size = new Size(40, 20), BackColor = backgroundColor, Font = defaultFont, BorderStyle = BorderStyle.Fixed3D };
        
            fontSizeInput = new TextBox { Location = new Point(398, 5), Width = 30, Text = "12", Font = defaultFont };
            fontDropdown = new ComboBox { Location = new Point(432, 5), Width = 200, Font = defaultFont };

            scaleOptionsLbl = new Label { Text = "Scale Options:", Location = new Point(636, 7), Size = new Size(100, 20), BackColor = this.BackColor, Font = defaultFont, BorderStyle = BorderStyle.None };
            scaleOptionsDropdown = new ComboBox { Location = new Point(737, 5), Width = 130, Font = defaultFont };

            saveAsciiButton = new Button { Text = "Save ASCII", Location = new Point(871, 4), Font = defaultFont, Size = new Size(100, 25) };
            saveImageButton = new Button { Text = "Save Image", Location = new Point(975, 4), Font = defaultFont, Size = new Size(100, 25) };
            generateAsciiButton = new Button { Text = "Generate ASCII", Location = new Point(1079, 4), Font = defaultFont, Size = new Size(130, 25) };
            // second row
            imageSizeLbl = new Label { Text = "Input Image Size:", ForeColor = this.ForeColor, Location = new Point(4, 36), Size = new Size(115, 20), BackColor = this.BackColor, Font = defaultFont, BorderStyle = BorderStyle.None };
            imageWidthLbl = new Label { Text = "", ForeColor = this.ForeColor, Location = new Point(127, 36), Size = new Size(40, 20), BackColor = this.BackColor, Font = defaultFont, BorderStyle = BorderStyle.None };
            xLbl = new Label { Text = " X ", ForeColor = this.ForeColor, Location = new Point(166, 36), Size = new Size(30, 20), BackColor = this.BackColor, Font = defaultFont, BorderStyle = BorderStyle.None };
            imageHeightLbl = new Label { Text = "", ForeColor = this.ForeColor, Location = new Point(190, 36), Size = new Size(40, 20), BackColor = this.BackColor, Font = defaultFont, BorderStyle = BorderStyle.None };

            // Third row
            outputSizeLbl = new Label { Text = "Output Image Size:", ForeColor = this.ForeColor, Location = new Point(4, 60), Size = new Size(130, 20), BackColor = this.BackColor, Font = defaultFont, BorderStyle = BorderStyle.None };
            outputWidthLbl = new Label { Text = "", ForeColor = this.ForeColor, Location = new Point(127, 60), Size = new Size(40, 20), BackColor = this.BackColor, Font = defaultFont, BorderStyle = BorderStyle.None };
            x2Lbl = new Label { Text = " X ", ForeColor = this.ForeColor, Location = new Point(166, 60), Size = new Size(30, 20), BackColor = this.BackColor, Font = defaultFont, BorderStyle = BorderStyle.None };
            outputHeightLbl = new Label { Text = "", ForeColor = this.ForeColor, Location = new Point(190, 60), Size = new Size(40, 20), BackColor = this.BackColor, Font = defaultFont, BorderStyle = BorderStyle.None };

            outputSizeButton = new Button { Text = "Set Output Size", Location = new Point(230, 56), Font = defaultFont, Size = new Size(120, 25) };

            ZoomPlusButton = new Button {  Image = Properties.Resources.Plus, Location = new Point(380, 41), Font = defaultFont, Size = new Size(40, 40) };
            ZoomMinusButton = new Button { Image = Properties.Resources.Minus, Location = new Point(424, 41), Font = defaultFont, Size = new Size(40, 40) };
         
            scrollablePanel = new Panel { Location = new Point(4, 88), Size = new Size(768, 768),  AutoScroll = true, BackColor = Color.DarkBlue, BorderStyle = BorderStyle.Fixed3D };
            outputPictureBox = new PictureBox { Location = new Point(0, 0), Size = new Size(600, 600), BorderStyle = BorderStyle.Fixed3D, BackColor = Color.DarkGray, SizeMode = PictureBoxSizeMode.Zoom};
            scrollablePanel.Controls.Add(outputPictureBox);
            asciiTextBox = new RichTextBox { Location = new Point(778, 88), Size = new Size(400, 500), ReadOnly = true, BackColor = Color.DarkGreen, ForeColor = textColor, Font = defaultFont };
        
            #endregion

            ToolTip scaleOptionsTooltip = new ToolTip();
            scaleOptionsTooltip.SetToolTip(scaleOptionsDropdown, "Select how the image scaling should be applied:\n" +
                "- Before generation: Resizes the image before generating ASCII art.\n" +
                "- After generation: Resizes the final ASCII-rendered image.\n" +
                "- 1/4 Scale: Reduces the image or ASCII art to 1/4 of its original size.\n" +
                "- 1/2 Scale: Reduces the image or ASCII art to 1/2 of its original size.\n" +
                "- 3/4 Scale: Reduces the image or ASCII art to 3/4 of its original size.\n" +
                "- 125% Scale: Increases the image or ASCII art by 25%.\n" + 
                "- 150% Scale: Increases the image or ASCII art by 50%.\n" + 
                "- 175% Scale: Increases the image or ASCII art by 75%.\n" + 
                "- 200% Scale: Increases the image or ASCII art by 100%.");
            // Event handlers
            loadImageButton.Click += LoadImageButton_Click;
            saveAsciiButton.Click += SaveAsciiButton_Click;
            saveImageButton.Click += SaveImageButton_Click;
            generateAsciiButton.Click += GenerateAsciiButton_Click;
            textColorButton.Click += TextColorButton_Click;
            textColorLbl.Click += TextColorLabel_Click;
            backgroundColorButton.Click += BackgroundColorButton_Click;
            backColorLbl.Click += BackColorLabel_Click;
            outputSizeButton.Click += OutputSizeButton_Click;
            scaleOptionsDropdown.SelectedIndexChanged += ScaleOptonsDropdown_SelectedIndexChanged;
            ZoomPlusButton.Click += ZoomPlusButton_Click;
            ZoomMinusButton.Click += ZoomMinusButton_Click;
            // Add controls
            this.Controls.Add(loadImageButton);
            this.Controls.Add(textColorButton);
            this.Controls.Add(textColorLbl);
            this.Controls.Add(backgroundColorButton);
            this.Controls.Add(backColorLbl);
            this.Controls.Add(fontSizeInput);
            this.Controls.Add(fontDropdown);
            this.Controls.Add(scaleOptionsLbl);
            this.Controls.Add(scaleOptionsDropdown);
            this.Controls.Add(saveAsciiButton);
            this.Controls.Add(saveImageButton);
            this.Controls.Add(generateAsciiButton);

            this.Controls.Add(ZoomPlusButton);
            this.Controls.Add(ZoomMinusButton);
            this.Controls.Add(scrollablePanel);
            this.Controls.Add(asciiTextBox);

            this.Controls.Add(imageSizeLbl);
            this.Controls.Add(imageWidthLbl);
            this.Controls.Add(xLbl);
            this.Controls.Add(imageHeightLbl);

            this.Controls.Add(outputSizeLbl);
            this.Controls.Add(outputWidthLbl);
            this.Controls.Add(x2Lbl);
            this.Controls.Add(outputHeightLbl);   
            this.Controls.Add(outputSizeButton);
            // Fill dropdowns
            PopulateFonts();
            FillScaleOptionsList();
        }

        private void PopulateFonts()
        {
            foreach (FontFamily font in new System.Drawing.Text.InstalledFontCollection().Families)
            {
                fontDropdown.Items.Add(font.Name);
            }
            fontDropdown.SelectedIndex = 0; // Select the first font by default
        }
        private void FillScaleOptionsList()
        {
            scaleOptionsDropdown.Items.Clear();
            scaleOptionsDropdown.Items.Add("After generation"); // This will be the default.
            scaleOptionsDropdown.Items.Add("Before generation");
            scaleOptionsDropdown.Items.Add("1/4 Scale");
            scaleOptionsDropdown.Items.Add("1/2 Scale");
            scaleOptionsDropdown.Items.Add("3/4 Scale");
            scaleOptionsDropdown.Items.Add("125% Scale");
            scaleOptionsDropdown.Items.Add("150% Scale");
            scaleOptionsDropdown.Items.Add("175% Scale");
            scaleOptionsDropdown.Items.Add("200% Scale");
            scaleOptionsDropdown.SelectedIndex = 0;
        }

        private void LoadImageButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.tif;";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    inputImage = new Bitmap(openFileDialog.FileName);
                    outputPictureBox.Image = inputImage;
                    inputImageWidth = inputImage.Width;
                    inputImageHeight = inputImage.Height;
                    outputImageHeight = inputImageHeight;
                    outputImageWidth = inputImageWidth;
                    imageWidthLbl.Text = inputImageWidth.ToString();
                    imageHeightLbl.Text = inputImageHeight.ToString();
                    outputWidthLbl.Text = inputImageWidth.ToString();
                    outputHeightLbl.Text = inputImageHeight.ToString();
                }
            }
        }
        private void TextColorButton_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    textColor = colorDialog.Color;
                    textColorLbl.BackColor = textColor;
                    asciiTextBox.ForeColor = textColor;
                }
            }
        }
        private void TextColorLabel_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    textColor = colorDialog.Color;
                    textColorLbl.BackColor = textColor;
                    asciiTextBox.ForeColor = textColor;
                }
            }
        }
        private void BackgroundColorButton_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    backgroundColor = colorDialog.Color;
                    backColorLbl.BackColor = backgroundColor;
                    outputPictureBox.BackColor = backgroundColor;
                }
            }
        }
        private void BackColorLabel_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    backgroundColor = colorDialog.Color;
                    backColorLbl.BackColor = backgroundColor;
                    outputPictureBox.BackColor = backgroundColor;
                }
            }
        }
        private void GenerateAsciiButton_Click(object sender, EventArgs e)
        {
            if (inputImage == null)
            {
                MessageBox.Show("Please load an image first.");
                return;
            }
            // Step 1: Convert Image to ASCII Art
            string asciiArt = ConvertToAscii(inputImage, inputImageWidth, inputImageHeight);

            string selectedFont = fontDropdown.SelectedItem.ToString();
            int fontSize = int.TryParse(fontSizeInput.Text, out int size) ? size : 12;

            if(inputImageWidth < outputImageWidth || inputImageHeight < outputImageHeight)
{
                MessageBox.Show($"Input image dimensions ({inputImageWidth}x{inputImageHeight}) are smaller than the output dimensions ({outputImageWidth}x{outputImageHeight}). Please choose a larger input image.");
                return;
            }
            Bitmap finalImage;
            switch (scaleOption)
            {
                case 0: // After Generation
                    Bitmap asciiImage = RenderAsciiToTransparentImage(asciiArt, inputImage.Width, inputImage.Height);
                    finalImage = ResizeImage(asciiImage, outputImageWidth, outputImageHeight);
                    break;

                case 1: // Before Generation
                    finalImage = RenderAsciiToTransparentImage(asciiArt, outputImageWidth, outputImageHeight);
                    break;

                case 2: // 1/4 Scale
                    Bitmap quarterScaleImage = ResizeImage(inputImage, inputImage.Width / 4, inputImage.Height / 4);
                    finalImage = RenderAsciiToTransparentImage(ConvertToAscii(quarterScaleImage, quarterScaleImage.Width, quarterScaleImage.Height), quarterScaleImage.Width, quarterScaleImage.Height);
                    break;

                case 3: // 1/2 Scale
                    Bitmap halfScaleImage = ResizeImage(inputImage, inputImage.Width / 2, inputImage.Height / 2);
                    finalImage = RenderAsciiToTransparentImage(ConvertToAscii(halfScaleImage, halfScaleImage.Width, halfScaleImage.Height), halfScaleImage.Width, halfScaleImage.Height);
                    break;

                case 4: // 3/4 Scale
                    Bitmap threeQuarterScaleImage = ResizeImage(inputImage, (inputImage.Width * 3) / 4, (inputImage.Height * 3) / 4);
                    finalImage = RenderAsciiToTransparentImage(ConvertToAscii(threeQuarterScaleImage, threeQuarterScaleImage.Width, threeQuarterScaleImage.Height), threeQuarterScaleImage.Width, threeQuarterScaleImage.Height);
                    break;
                case 5: // 125% Scale
                    Bitmap doubleScaleImage = ResizeImage(inputImage, (int)(inputImage.Width * 1.25), (int)(inputImage.Height * 1.25));
                    finalImage = RenderAsciiToTransparentImage(ConvertToAscii(doubleScaleImage, doubleScaleImage.Width, doubleScaleImage.Height), doubleScaleImage.Width, doubleScaleImage.Height);
                    break;
                case 6: // 150% Scale
                    Bitmap tripleScaleImage = ResizeImage(inputImage, (int)(inputImage.Width * 1.5), (int)(inputImage.Height * 1.5));
                    finalImage = RenderAsciiToTransparentImage(ConvertToAscii(tripleScaleImage, tripleScaleImage.Width, tripleScaleImage.Height), tripleScaleImage.Width, tripleScaleImage.Height);
                    break;
                    case 7: // 175% Scale
                    Bitmap quadScaleImage = ResizeImage(inputImage, (int)(inputImage.Width * 1.75), (int)(inputImage.Height * 1.75));
                    finalImage = RenderAsciiToTransparentImage(ConvertToAscii(quadScaleImage, quadScaleImage.Width, quadScaleImage.Height), quadScaleImage.Width, quadScaleImage.Height);
                    break;
                    case 8: // 200% Scale
                    Bitmap quintScaleImage = ResizeImage(inputImage, inputImage.Width * 2, inputImage.Height * 2);
                    finalImage = RenderAsciiToTransparentImage(ConvertToAscii(quintScaleImage, quintScaleImage.Width, quintScaleImage.Height), quintScaleImage.Width, quintScaleImage.Height);
                    break;
                default:
                    MessageBox.Show("Please select a valid scale option.");
                    return;
            }
            // Step 4: Display the New Image
            outputPictureBox.BackColor = backgroundColor;
            outputPictureBox.Image = finalImage;
            asciiTextBox.BackColor = backgroundColor;
            asciiTextBox.Text = asciiArt;
        }
        private void SaveAsciiButton_Click(object sender, EventArgs e)
        {
            if (asciiTextBox.Text == string.Empty)
            {
                MessageBox.Show("No ASCII art to save. Please generate it first.");
                return;
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text Files|*.txt";
                saveFileDialog.Title = "Save ASCII Art";
                saveFileDialog.FileName = "ascii_art.txt";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveFileDialog.FileName, asciiTextBox.Text);
                    MessageBox.Show($"ASCII art saved to {saveFileDialog.FileName}.");
                }
            }
        }
        private void SaveImageButton_Click(object sender, EventArgs e)
        {
            if (outputPictureBox.Image == null)
            {
                MessageBox.Show("No image to save. Please generate it first.");
                return;
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg;*.jpeg|Bitmap Image|*.bmp|TIF Image|*.tif";
                saveFileDialog.Title = "Save Rendered Image";
                saveFileDialog.FileName = "ascii_art_image.png";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    outputPictureBox.Image.Save(saveFileDialog.FileName);
                    MessageBox.Show($"Image saved to {saveFileDialog.FileName}.");
                }
            }
        }
        private void OutputSizeButton_Click(object sender, EventArgs e)
        {
            using (Form inputForm = new Form())
            {
                inputForm.Text = "Change Output Size";
                inputForm.Size = new Size(300, 150);

                Label widthLabel = new Label { Text = "Width:", Location = new Point(10, 10) };
                TextBox widthInput = new TextBox { Location = new Point(110, 10), Text = outputImageWidth.ToString() };
                Label heightLabel = new Label { Text = "Height:", Location = new Point(10, 40) };
                TextBox heightInput = new TextBox { Location = new Point(110, 40), Text = outputImageHeight.ToString() };
                Button applyButton = new Button { Text = "Apply", Location = new Point(100, 70) };

                // Add event handlers for Enter key navigation
                widthInput.KeyDown += (s, args) =>
                {
                    if (args.KeyCode == Keys.Enter)
                    {
                        heightInput.Focus(); // Move focus to heightInput
                        args.Handled = true; // Prevent default behavior
                    }
                };

                heightInput.KeyDown += (s, args) =>
                {
                    if (args.KeyCode == Keys.Enter)
                    {
                        applyButton.Focus(); // Move focus to applyButton
                        args.Handled = true; // Prevent default behavior
                    }
                };
                // Optional: Select all text when clicking into textboxes for easy editing
                widthInput.Click += (s, args) => { widthInput.SelectAll(); };
                heightInput.Click += (s, args) => { heightInput.SelectAll(); }; inputForm.Controls.Add(widthLabel);

                applyButton.Click += (s, args) =>
                {
                    if (int.TryParse(widthInput.Text, out int newWidth) && int.TryParse(heightInput.Text, out int newHeight))
                    {
                        outputImageWidth = newWidth;
                        outputImageHeight = newHeight;
                        outputWidthLbl.Text = newWidth.ToString();
                        outputHeightLbl.Text = newHeight.ToString();
                        inputForm.Close();
                    }
                    else
                    {
                        MessageBox.Show("Please enter valid numeric values for width and height.");
                    }
                };

                inputForm.Controls.Add(widthInput);
                inputForm.Controls.Add(heightLabel);
                inputForm.Controls.Add(heightInput);
                inputForm.Controls.Add(applyButton);

                inputForm.ShowDialog();
            }
        }
        private void ScaleOptonsDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            scaleOption = scaleOptionsDropdown.SelectedIndex;
        }
        private void ZoomMinusButton_Click(object sender, EventArgs e)
        {
            zoomFactor -= 10;
            if (zoomFactor < 25)
            {
                MessageBox.Show("Cannot zoom out further!");
                zoomFactor = 25;
            }
            UpdateZoom();
        }
        private void ZoomPlusButton_Click(object sender, EventArgs e)
        {
            zoomFactor += 10;
            if (zoomFactor > 200)
            {
                MessageBox.Show("Cannot zoom in further!");
                zoomFactor = 200;
            }
            UpdateZoom();
        }

        private string ConvertToAscii(Bitmap bitmap, int imageWidth, int imageHeight)
        {
            // Retrieve font from UI controls
            string selectedFontName = fontDropdown.SelectedItem?.ToString() ?? "Courier New"; // Default to Courier New if no selection
            int fontSize = int.TryParse(fontSizeInput.Text, out int size) ? size : 12; // Default size 12 if invalid input
            Font font = new Font(selectedFontName, fontSize);

            // Calculate fixed cell dimensions
            var (cellWidth, cellHeight) = CalculateCellDimensions(font);

            // Determine the number of columns and rows based on image dimensions
            int columns = imageWidth / cellWidth;
            int rows = imageHeight / cellHeight;

            // Resize the input image to match the character grid dimensions
            Bitmap resizedBitmap = new Bitmap(bitmap, new Size(columns, rows));
            StringBuilder asciiBuilder = new StringBuilder();

            char[] asciiChars = GetAsciiCharacterSet(); // Use the full ASCII character set (32–127)
            for (int y = 0; y < resizedBitmap.Height; y++)
            {
                for (int x = 0; x < resizedBitmap.Width; x++)
                {
                    Color pixelColor = resizedBitmap.GetPixel(x, y);
                    int grayValue = (int)(pixelColor.R * 0.3 + pixelColor.G * 0.59 + pixelColor.B * 0.11);
                    int charIndex = grayValue * (asciiChars.Length - 1) / 255; // Map grayscale value to ASCII character
                    asciiBuilder.Append(asciiChars[charIndex]);
                }
                asciiBuilder.AppendLine(); // Add a newline for each row
            }

            return asciiBuilder.ToString();
        }
        private Bitmap RenderAsciiToTransparentImage(string asciiArt, int imageWidth, int imageHeight)
        {
            string fontName = fontDropdown.SelectedItem?.ToString() ?? "Courier New"; // Default to Courier New if no selection
            int fontSize = int.TryParse(fontSizeInput.Text, out int size) ? size : 12; // Default size 12 if invalid input
            Font font = new Font(fontName, fontSize);
            var (cellWidth, cellHeight) = CalculateCellDimensions(font);

            string[] lines = asciiArt.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            Bitmap image = new Bitmap(imageWidth, imageHeight);
            image.MakeTransparent();
            using (Graphics graphics = Graphics.FromImage(image))
            {
                graphics.Clear(Color.Transparent);

                using (Brush textBrush = new SolidBrush(textColor))
                {
                    for (int row = 0; row < lines.Length; row++)
                    {
                        for (int col = 0; col < lines[row].Length; col++)
                        {
                            float x = col * cellWidth; // Fixed horizontal position
                            float y = row * cellHeight; // Fixed vertical position

                            graphics.DrawString(lines[row][col].ToString(), font, textBrush, new PointF(x, y));
                        }
                    }
                }
            }

            return image;
        }
        private (int cellWidth, int cellHeight) CalculateCellDimensions(Font font)
        {
            using (Graphics g = Graphics.FromImage(new Bitmap(1, 1)))
            {
                // Measure the width and height of the widest character
                int cellWidth = (int)Math.Ceiling(g.MeasureString("W", font).Width); // Widest character
                int cellHeight = (int)Math.Ceiling(font.GetHeight()); // Character height
                return (cellWidth, cellHeight);
            }
        }
        private char[] GetAsciiCharacterSet()
        {
            char[] asciiChars = new char[128 - 32]; // Size of the printable range
            for (int i = 32; i < 128; i++)
            {
                asciiChars[i - 32] = (char)i; // Map ASCII characters
            }
            return asciiChars;
        }
        private Bitmap ResizeImage(Bitmap originalImage, int targetWidth, int targetHeight)
        {
            Bitmap resizedImage = new Bitmap(targetWidth, targetHeight);
            using (Graphics graphics = Graphics.FromImage(resizedImage))
            {
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                graphics.DrawImage(originalImage, 0, 0, targetWidth, targetHeight);
            }
            return resizedImage;
        }
        private void UpdateZoom()
        {
            outputPictureBox.Width = (outputImageWidth * zoomFactor) / 100;
            outputPictureBox.Height = (outputImageHeight * zoomFactor) / 100;
        }

    }
}
