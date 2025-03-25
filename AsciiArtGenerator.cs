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
        private PictureBox inputPictureBox;
        private TextBox zoomAmount;
        private TextBox scaleAmount;
        private TextBox fontSizeInput;
        private RichTextBox asciiTextBox;
        private ComboBox fontDropdown;
        private ComboBox scaleOptionsDropdown;
        private CheckBox backTransparentCheckbox;
        private CheckBox backSolidCheckbox;
        private Bitmap inputImage;
        private Bitmap outputImage;
        private Font formFont;
        private Font textFont;
        private string textFontName = "Courier New";
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
        private Label percentLbl;
        private Label outputHeightLbl;
        private Label scaleOptionsLbl;
        private Label scaleAmountLbl;
        private Label zoomLbl;
        private Label backstyleLbl;
        private Label inputImageBoxLbl;
        private Label inputImageBoxScale;
        private Label percent2Lbl;
        //
        private int inputImageWidth, inputImageHeight, outputImageWidth, outputImageHeight = 0;
        private int textFontSize = 10;
        private int formFontSize = 12; // Default size 12 
        private int scaleOption;
        private double scaleFactor = 1.0;
        private int zoomFactor = 100;
        //
        private Color textColor = Color.Black;
        private Color backgroundColor = Color.White;
        string asciiArt;
        private string[] scaleOptionsToolTips;
        private ToolTip scaleOptionsTooltip;
        private bool ArtHasBeenGenerated = false;
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
            this.Size = new Size(1240, 900);
            formFont = new Font("Arial", formFontSize, FontStyle.Regular);

            #region UI Controls
            loadImageButton = new Button { Text = "Load Image", Location = new Point(4, 4), Font = formFont, Size = new Size(100, 25) };

            textColorButton = new Button { Text = "Fore Color", Location = new Point(108, 4), Font = formFont, Size = new Size(100, 25) };
            textColorLbl = new Label { Text = "", ForeColor = textColor, Location = new Point(212, 5), Size = new Size(40, 20), BackColor = backgroundColor, Font = formFont, BorderStyle = BorderStyle.Fixed3D };

            backgroundColorButton = new Button { Text = "Back Color", Location = new Point(250, 4), Size = new Size(100, 25), BackColor = backgroundColor, Font = formFont };
            backColorLbl = new Label { Text = "", ForeColor = textColor, Location = new Point(354, 5), Size = new Size(40, 20), BackColor = backgroundColor, Font = formFont, BorderStyle = BorderStyle.Fixed3D };

            fontSizeInput = new TextBox { Location = new Point(398, 5), Width = 30, Text = "12", Font = formFont };
            fontDropdown = new ComboBox { Location = new Point(432, 5), Width = 200, Font = formFont };

            scaleOptionsLbl = new Label { Text = "Scale Options:", Location = new Point(636, 7), Size = new Size(110, 20), BackColor = this.BackColor, Font = formFont, BorderStyle = BorderStyle.None };
            scaleOptionsDropdown = new ComboBox { Location = new Point(747, 5), Width = 130, Font = formFont };
            scaleOptionsDropdown.DropDownStyle = ComboBoxStyle.DropDownList;

            saveAsciiButton = new Button { Text = "Save ASCII", Location = new Point(881, 4), Font = formFont, Size = new Size(100, 25) };
            saveImageButton = new Button { Text = "Save Image", Location = new Point(985, 4), Font = formFont, Size = new Size(100, 25) };
            generateAsciiButton = new Button { Text = "Generate ASCII", Location = new Point(1089, 4), Font = formFont, Size = new Size(130, 25) };
            // second row
            imageSizeLbl = new Label { Text = "Input Image Size:", ForeColor = this.ForeColor, Location = new Point(4, 36), Size = new Size(115, 20), BackColor = this.BackColor, Font = formFont, BorderStyle = BorderStyle.None };
            imageWidthLbl = new Label { Text = "", ForeColor = this.ForeColor, Location = new Point(127, 36), Size = new Size(50, 20), BackColor = this.BackColor, Font = formFont, BorderStyle = BorderStyle.None };
            xLbl = new Label { Text = " X ", ForeColor = this.ForeColor, Location = new Point(176, 36), Size = new Size(30, 20), BackColor = this.BackColor, Font = formFont, BorderStyle = BorderStyle.None };
            imageHeightLbl = new Label { Text = "", ForeColor = this.ForeColor, Location = new Point(200, 36), Size = new Size(50, 20), BackColor = this.BackColor, Font = formFont, BorderStyle = BorderStyle.None };

            scaleAmountLbl = new Label { Text = "Scale Amount:", ForeColor = this.ForeColor, Location = new Point(636, 39), Size = new Size(110, 20), BackColor = this.BackColor, Font = formFont, BorderStyle = BorderStyle.None };
            scaleAmount = new TextBox { Text = "", Location = new Point(747, 36), Width = 30, Font = formFont };
            percentLbl = new Label { Text = "%", ForeColor = this.ForeColor, Location = new Point(780, 40), Size = new Size(20, 20), BackColor = this.BackColor, Font = formFont, BorderStyle = BorderStyle.None };
     
            backstyleLbl = new Label { Text = "Background Style:", ForeColor = this.ForeColor, Location = new Point(881, 36), Size = new Size(140, 20), BackColor = this.BackColor, Font = formFont, BorderStyle = BorderStyle.None };
            backTransparentCheckbox = new CheckBox { Text = "Transparent", Location = new Point(883, 56), Size = new Size(110, 24), BackColor = this.BackColor, Font = formFont };
            backSolidCheckbox = new CheckBox { Text = "Solid", Location = new Point(993, 56), Size = new Size(120, 24), BackColor = this.BackColor, Font = formFont };

            // Third row
            outputSizeLbl = new Label { Text = "Output Image Size:", ForeColor = this.ForeColor, Location = new Point(4, 60), Size = new Size(130, 20), BackColor = this.BackColor, Font = formFont, BorderStyle = BorderStyle.None };
            outputWidthLbl = new Label { Text = "", ForeColor = this.ForeColor, Location = new Point(127, 60), Size = new Size(50, 20), BackColor = this.BackColor, Font = formFont, BorderStyle = BorderStyle.None };
            x2Lbl = new Label { Text = " X ", ForeColor = this.ForeColor, Location = new Point(176, 60), Size = new Size(30, 20), BackColor = this.BackColor, Font = formFont, BorderStyle = BorderStyle.None };
            outputHeightLbl = new Label { Text = "", ForeColor = this.ForeColor, Location = new Point(200, 60), Size = new Size(50, 20), BackColor = this.BackColor, Font = formFont, BorderStyle = BorderStyle.None };

            outputSizeButton = new Button { Text = "Set Output Size", Location = new Point(250, 56), Font = formFont, Size = new Size(130, 25),TextAlign = ContentAlignment.MiddleLeft };

            ZoomPlusButton = new Button { Image = Properties.Resources.Plus, Location = new Point(384, 50), Font = formFont, Size = new Size(32, 32), ImageAlign = ContentAlignment.TopLeft };
            ZoomMinusButton = new Button { Image = Properties.Resources.Minus, Location = new Point(420, 50), Font = formFont, Size = new Size(32, 32), ImageAlign = ContentAlignment.TopLeft };
            zoomAmount = new TextBox { Location = new Point(458, 55), Width = 30, Height = 32, Font = formFont };
            zoomLbl = new Label { Text = "Zoom %", ForeColor = this.ForeColor, Location = new Point(488, 60), Size = new Size(70, 20), BackColor = this.BackColor, Font = formFont, BorderStyle = BorderStyle.None };

            scrollablePanel = new Panel { Location = new Point(4, 88), Size = new Size(768, 768), AutoScroll = true, BackColor = Color.DarkBlue, BorderStyle = BorderStyle.Fixed3D };
            outputPictureBox = new PictureBox { Location = new Point(0, 0), Size = new Size(512, 512), BorderStyle = BorderStyle.Fixed3D, BackColor = Color.DarkGray, SizeMode = PictureBoxSizeMode.Zoom };
            scrollablePanel.Controls.Add(outputPictureBox);
            asciiTextBox = new RichTextBox { Location = new Point(778, 88), Size = new Size(400, 500), ReadOnly = true, BackColor = Color.DarkGreen, ForeColor = textColor, Font = formFont };
            inputPictureBox = new PictureBox { Location = new Point(778, 600), Size = new Size(256, 256), BorderStyle = BorderStyle.Fixed3D, BackColor = Color.DarkSlateBlue, SizeMode = PictureBoxSizeMode.Zoom };
            inputImageBoxLbl = new Label { Text = "Input Image", ForeColor = this.ForeColor, Location = new Point(1038, 600), Size = new Size(100, 20), BackColor = this.BackColor, Font = formFont, BorderStyle = BorderStyle.None };
            inputImageBoxScale = new Label { Text = "25", ForeColor = this.ForeColor, Location = new Point(1038, 620), Size = new Size(30, 20), BackColor = this.BackColor, Font = formFont, BorderStyle = BorderStyle.None };
            percent2Lbl = new Label { Text = "%", ForeColor = this.ForeColor, Location = new Point(1068, 620), Size = new Size(20, 20), BackColor = this.BackColor, Font = formFont, BorderStyle = BorderStyle.None };
        #endregion
            #region ToolTips
            scaleOptionsToolTips = new string[3];
            scaleOptionsToolTips[0] = "Scales the original image to the output image size \n" +
                                                            "before it is converted to ASCII art. This will result \n"+
                                                            "in fewer ASCII characters, but will be more accurate\n."+
                                                            "per character. This is the default option.";
            scaleOptionsToolTips[1] = "Scales the original image to the output image size \n" +
                                                            "after it is converted to ASCII art. This will result \n" +
                                                            "in more ASCII characters, and scaling may cause \n"+
                                                            "distortions or bluring in the characters.";
            scaleOptionsToolTips[2] = "Selects when the image scaling should be applied.";
            
            ToolTip scaleOptionsTooltip = new ToolTip();
            scaleOptionsTooltip.SetToolTip(scaleOptionsDropdown, scaleOptionsToolTips[2]);

            ToolTip scaleAmountTooltip = new ToolTip();
            scaleAmountTooltip.SetToolTip(scaleAmount, "Enter a percentage to scale the original image.\n" +
                                                                                                  "Minimum scale is 25% and Maximum scale is 200%.\n" +
                                                                                                  "Use only whole numbers only for the percentage; no decimals allowed.\n" +
                                                                                                  "Example: 25 for 25%, 100 for 100%, etc.");

            ToolTip ZoomAmountTooltip = new ToolTip();
            ZoomAmountTooltip.SetToolTip(zoomAmount, "Enter a percentage to zoom in or out of the image.\n"+
                "Miinimum zoom is 25% and Maxiimum zoom is 200%");
         
            #endregion
            #region Event handlers
            loadImageButton.Click += LoadImageButton_Click;
            fontSizeInput.KeyDown += FontSizeInput_KeyDown;
            fontDropdown.SelectedValueChanged += FontDropdown_SelectedValueChanged;
            saveAsciiButton.Click += SaveAsciiButton_Click;
            saveImageButton.Click += SaveImageButton_Click;
            generateAsciiButton.Click += GenerateAsciiButton_Click;

            textColorButton.Click += TextColorButton_Click;
            textColorLbl.Click += TextColorLabel_Click;

            backgroundColorButton.Click += BackgroundColorButton_Click;
            backColorLbl.Click += BackColorLabel_Click;

            outputSizeButton.Click += OutputSizeButton_Click;

            scaleOptionsDropdown.SelectedIndexChanged += ScaleOptonsDropdown_SelectedIndexChanged;
            scaleOptionsDropdown.MouseHover += ScaleOptionsDropdown_MouseHover;

            scaleAmount.KeyDown += ScaleAmount_KeyDown;
            scaleAmount.KeyPress += ScaleAmount_KeyPress;

            ZoomPlusButton.Click += ZoomPlusButton_Click;
            ZoomMinusButton.Click += ZoomMinusButton_Click;

            backTransparentCheckbox.CheckedChanged += BackTransparentCheckbox_CheckedChanged;
            backSolidCheckbox.CheckedChanged += BackSolidCheckbox_CheckedChanged;
            #endregion
            #region Add controls
            this.Controls.Add(loadImageButton);
            this.Controls.Add(saveAsciiButton);
            this.Controls.Add(saveImageButton);
            this.Controls.Add(generateAsciiButton);
            this.Controls.Add(scrollablePanel);
            this.Controls.Add(asciiTextBox);
            this.Controls.Add(inputPictureBox);
            this.Controls.Add(inputImageBoxLbl);
            this.Controls.Add(inputImageBoxScale);
            this.Controls.Add(percent2Lbl);

            this.Controls.Add(textColorButton);
            this.Controls.Add(textColorLbl);

            this.Controls.Add(backgroundColorButton);
            this.Controls.Add(backColorLbl);

            this.Controls.Add(fontSizeInput);
            this.Controls.Add(fontDropdown);

            this.Controls.Add(scaleOptionsLbl);
            this.Controls.Add(scaleOptionsDropdown);

            this.Controls.Add(scaleAmountLbl);
            this.Controls.Add(scaleAmount);
            this.Controls.Add(percentLbl);

            this.Controls.Add(ZoomPlusButton);
            this.Controls.Add(ZoomMinusButton);
            this.Controls.Add(zoomAmount);
            this.Controls.Add(zoomLbl);

            this.Controls.Add(imageSizeLbl);
            this.Controls.Add(imageWidthLbl);
            this.Controls.Add(xLbl);
            this.Controls.Add(imageHeightLbl);

            this.Controls.Add(outputSizeLbl);
            this.Controls.Add(outputWidthLbl);
            this.Controls.Add(x2Lbl);
            this.Controls.Add(outputHeightLbl);   
            this.Controls.Add(outputSizeButton);

            this.Controls.Add(backstyleLbl);
            this.Controls.Add(backTransparentCheckbox);
            this.Controls.Add(backSolidCheckbox);
            #endregion
            // Fill dropdowns
            PopulateFonts();
            FillScaleOptionsList();
        }

        // Setup Methods
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
            scaleOptionsDropdown.Items.Add("Before generation");// This will be the default.
            scaleOptionsDropdown.Items.Add("After generation"); 
            scaleOptionsDropdown.DrawMode = DrawMode.OwnerDrawFixed;
            scaleOptionsDropdown.DrawItem += ScaleOptionsDropdown_DrawItem;
            scaleOptionsDropdown.MouseMove += ScaleOptionsDropdown_MouseMove;


            scaleOptionsDropdown.SelectedIndex = 0;
        }

        #region Event handler Methods
        private void ScaleAmount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (inputImage == null)
                {
                    MessageBox.Show("Please load an image first.");
                    return;
                }
                if (scaleAmount.Text.Contains("."))
                {
                    MessageBox.Show("Scale amount cannot contain decimals. It must be a whole number.");
                    scaleAmount.Text = "";
                    return;
                }
                // Get the scale amount.
                int scaleAmountInt = (int)scaleFactor * 100; // This is for startup or when the scale amount is not set.
                if (scaleAmount.Text != "")
                {
                    scaleAmountInt = Convert.ToInt32(scaleAmount.Text);
                    if (scaleAmountInt < 25 || scaleAmountInt > 200)
                    {
                        MessageBox.Show("Scale amount out of range.");
                        scaleAmount.Text = "";
                        return;
                    }
                }
                scaleFactor = scaleAmountInt / 100.0;
                outputImageWidth = (int)(inputImageWidth * scaleFactor);
                outputImageHeight = (int)(inputImageHeight * scaleFactor);
                outputWidthLbl.Text = outputImageWidth.ToString();
                outputHeightLbl.Text = outputImageHeight.ToString();
                outputPictureBox.Size = new Size(outputImageWidth, outputImageHeight);
                e.Handled = true; // Prevent default behavior
            }
        }
        private void ScaleAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the entered key is a period ('.')
            if (e.KeyChar == '.' || e.KeyChar == ',')
            {
                // Block the input by setting Handled to true
                e.Handled = true;
                MessageBox.Show("Scale amount cannot contain decimals. It must be a whole number.");
            }
            // Allow only digits and control characters (like Backspace)
            else if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void ScaleOptionsDropdown_MouseHover(object sender, EventArgs e)
        {
            // Get mouse position and find the corresponding item
            int hoveredIndex = scaleOptionsDropdown.SelectedIndex;

            if (scaleOptionsDropdown.DroppedDown && hoveredIndex >= 0 && hoveredIndex < scaleOptionsToolTips.Length)
            {
                // Dynamically set tooltip based on the hovered item
                string hoveredToolTip = scaleOptionsToolTips[hoveredIndex];
                scaleOptionsTooltip.SetToolTip(scaleOptionsDropdown, hoveredToolTip);
            }
        }
        private void ScaleOptionsDropdown_MouseMove(object sender, MouseEventArgs e)
        {
            ToolTip scaleOptionsTooltip = new ToolTip();
            // Determine which item the mouse is over
            int hoveredIndex = scaleOptionsDropdown.SelectedIndex;
            if (scaleOptionsDropdown.DroppedDown && hoveredIndex >= 0 && hoveredIndex < scaleOptionsToolTips.Length)
            {
                // Dynamically set tooltip based on the hovered item
                string hoveredToolTip = scaleOptionsToolTips[hoveredIndex];
                scaleOptionsTooltip.SetToolTip(scaleOptionsDropdown, hoveredToolTip);
            }
            else
            {
                // Clear tooltip when not hovering over an item
                scaleOptionsTooltip.SetToolTip(scaleOptionsDropdown, scaleOptionsToolTips[2]);
            }
        }
        private void ScaleOptionsDropdown_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Custom draw ComboBox items
            if (e.Index < 0) return;

            e.DrawBackground();
            e.Graphics.DrawString(scaleOptionsDropdown.Items[e.Index].ToString(),
                e.Font, System.Drawing.Brushes.Black, e.Bounds);
            e.DrawFocusRectangle();
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
                    zoomFactor = 100;
                    zoomAmount.Text = zoomFactor.ToString();
                }
            }
        }
        private void TextColorButton_Click(object sender, EventArgs e)
        {
            UpdateColor(
                color => textColor = color,
                textColorLbl,
                asciiTextBox
            );
        }
        private void TextColorLabel_Click(object sender, EventArgs e)
        {
            UpdateColor(
                color => textColor = color,
                textColorLbl,
                asciiTextBox
            );
        }
        private void BackgroundColorButton_Click(object sender, EventArgs e)
        {
            UpdateColor(
                color => backgroundColor = color,
                backColorLbl,
                outputPictureBox
            );
        }
        private void BackColorLabel_Click(object sender, EventArgs e)
        {
            UpdateColor(
                color => backgroundColor = color,
                backColorLbl,
                outputPictureBox
            );
        }
        private void FontDropdown_SelectedValueChanged(object sender, EventArgs e)
        {
            textFontName = fontDropdown.Text;
            textFont = new Font(textFontName, textFontSize);
            if (ArtHasBeenGenerated)
            {
                GenerateAsciiButton_Click(this, EventArgs.Empty);
            }
        }
        private void FontSizeInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int value = Convert.ToInt32(fontSizeInput.Text);
                if (value >= 8 && value <= 100)
                {
                    textFontSize = value;
                    textFont = new Font(textFontName, textFontSize);
                    if (ArtHasBeenGenerated)
                    {
                        GenerateAsciiButton_Click(this, EventArgs.Empty);
                    }
                }
            }
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
        private void GenerateAsciiButton_Click(object sender, EventArgs e)
        {
            if (inputImage == null)
            {
                MessageBox.Show("Please load an image first.");
                return;
            }
            // Step 1: Convert Image to ASCII Art
            asciiArt = ConvertToAscii(inputImage, inputImageWidth, inputImageHeight);

            if((inputImageWidth < outputImageWidth || inputImageHeight < outputImageHeight)&&scaleFactor<=1.0)
{
                MessageBox.Show($"Input image dimensions ({inputImageWidth}x{inputImageHeight}) are smaller than the output dimensions ({outputImageWidth}x{outputImageHeight})\n"+
                                                     "and Scaling is set to 100% or less. Please choose a larger input image or adjust the scaling factor.");
                return;
            }
            Bitmap generatedImage;
            switch (scaleOption)
            {
                case 0: // Before Generation
                    outputImage = RenderAsciiToImage(asciiArt);
                break;
                case 1: // After Generation
                    Bitmap asciiImage = RenderAsciiToImage(asciiArt);
                    outputImage = ResizeImage(asciiImage, scaleFactor);
                    break;
                default:
                    generatedImage = new Bitmap(outputImageWidth, outputImageHeight);
                    break;
            }
            // Step 4: Display the New Image
            outputPictureBox.BackColor = backgroundColor;
            outputPictureBox.Image = outputImage;
            // if the image has a solid background we want to set to that color, otherwise set to light gray.
            if (useTransparentBackground == false) { asciiTextBox.BackColor = backgroundColor; } else asciiTextBox.BackColor = Color.LightGray;
            outputPictureBox.BackColor = backgroundColor;
            asciiTextBox.Text = asciiArt;
            ArtHasBeenGenerated = true;
            // scale a copy of the original image to fit in the inputPictureBox which is set at 256x256.
            // Determine the scale factor based on the larger dimension
            double PicturescaleFactor = Math.Min(256.0 / inputImage.Width, 256.0 / inputImage.Height);

            // Create a scaled copy of the inputImage
            int scaledWidth = (int)(inputImage.Width * PicturescaleFactor);
            int scaledHeight = (int)(inputImage.Height * PicturescaleFactor);
            Bitmap scaledImage = new Bitmap(inputImage, new Size(scaledWidth, scaledHeight));
            // set inputPictureBox to the scaled copy
            inputPictureBox.Image = scaledImage;
            // Update the inputImageBoxScale label with the scale percentage
            inputImageBoxScale.Text = (PicturescaleFactor * 100).ToString();

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
                        outputPictureBox.Size = new Size(outputImageWidth, outputImageHeight);
                        // here we need to determine the scaling factor and update the scale amount accordingly.
                        // Calculate the scale factor based on the larger scaling requirement
                        double widthScaleFactor = (double)outputImageWidth / inputImage.Width;
                        double heightScaleFactor = (double)outputImageHeight / inputImage.Height;
                        // Use the smaller of the two scale factors to avoid distortion
                        scaleFactor = Math.Min(widthScaleFactor, heightScaleFactor);
                        // Update the scale amount label to reflect the effective scale factor as a percentage
                        scaleAmount.Text = (scaleFactor * 100).ToString();

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
        private void ZoomPlusButton_Click(object sender, EventArgs e)
        {
            zoomFactor += 10;
            if (zoomFactor > 200)
            {
                MessageBox.Show("Cannot zoom in further!");
                zoomFactor = 200;
            }
            zoomAmount.Text = zoomFactor.ToString();
            UpdateZoom();
        }
        private void ZoomMinusButton_Click(object sender, EventArgs e)
        {
            zoomFactor -= 10;
            if (zoomFactor < 25)
            {
                MessageBox.Show("Cannot zoom out further!");
                zoomFactor = 25;
            }
            zoomAmount.Text = zoomFactor.ToString();
            UpdateZoom();
        }
        private void ScaleOptonsDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            scaleOption = scaleOptionsDropdown.SelectedIndex;
        }
        private void BackSolidCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (backSolidCheckbox.Checked)
            {
                useTransparentBackground = false;
                backTransparentCheckbox.Checked = false;
            }
            else
            {
                useTransparentBackground = true;
                backTransparentCheckbox.Checked = true;
            }
        }
        private void BackTransparentCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (backTransparentCheckbox.Checked)
            {
                backSolidCheckbox.Checked = false;
                useTransparentBackground = true;
            }
            else
            {
                backSolidCheckbox.Checked = true;
                useTransparentBackground = false;
            }
        }
        #endregion
        // Helper methods
        private void UpdateColor(Action<Color> setColorAction, Control labelOrButton, Control elementToUpdate)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    // Update the color via the Action delegate
                    setColorAction(colorDialog.Color);

                    // Update the label or button's background color
                    labelOrButton.BackColor = colorDialog.Color;

                    // Update the target element's color
                    elementToUpdate.BackColor = colorDialog.Color;

                    // Regenerate ASCII art if it has already been generated
                    if (ArtHasBeenGenerated)
                    {
                        GenerateAsciiButton_Click(this, EventArgs.Empty);
                    }
                }
            }
        }
        private string ConvertToAscii(Bitmap bitmap, int imageWidth, int imageHeight)
        {
            Font font = new Font(textFont.Name, textFontSize);
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
        private Bitmap RenderAsciiToImage(string asciiArt)
        {
            Font font = new Font(textFontName, textFontSize);
            var (cellWidth, cellHeight) = CalculateCellDimensions(font);

            string[] lines = asciiArt.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            Bitmap image = new Bitmap(cellWidth * lines[0].Length, cellHeight * lines.Length);
            using (Graphics graphics = Graphics.FromImage(image))
            {
                if(useTransparentBackground)
                {
                    image.MakeTransparent();
                    graphics.Clear(Color.Transparent);
                }
                else
                {
                    graphics.Clear(backgroundColor);
                }

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
        private Bitmap ResizeImage(Bitmap originalImage, double scaleAmount)
        {
            int targetWidth = (int)(originalImage.Width * scaleAmount);
            int targetHeight = (int)(originalImage.Height * scaleAmount);
            outputWidthLbl.Text = targetWidth.ToString();
            outputHeightLbl.Text = targetHeight.ToString();
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
