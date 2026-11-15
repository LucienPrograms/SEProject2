using SEProject2.Application;

namespace SEProject2.UI
{
    public partial class Form1 : Form
    {

        private readonly ProgramService programService = new();
        private readonly TextBox programEditor = new();

        private readonly TextBox outputBox = new();
        private readonly TextBox metricsBox = new();
        private readonly Panel pathPanel = new();
        private string[] currentGrid = Array.Empty<string>();
        private IReadOnlyList<(int X, int Y)> currentPath = Array.Empty<(int X, int Y)>();

        /// <summary>
        /// Initializes the main application window and builds the user interface
        /// </summary>

        public Form1()
        {
            InitializeComponent();

            Text = "SEProject2 - Programming App";
            Width = 1000;
            Height = 600;
            StartPosition = FormStartPosition.CenterScreen;

            BuildInterface();
        }

        /// <summary>
        /// Builds and arranges all controls in the main application window
        /// </summary>

        private void BuildInterface()
        {
            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 3,
                Padding = new Padding(10)
            };

            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 65));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 35));

            Controls.Add(mainLayout);
            
            var buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
            };

            buttonPanel.Controls.Add(CreateButton("Load File", LoadFile));
            buttonPanel.Controls.Add(CreateButton("Basic", LoadBasic));
            buttonPanel.Controls.Add(CreateButton("Advanced", LoadAdvanced));
            buttonPanel.Controls.Add(CreateButton("Expert", LoadExpert));
            buttonPanel.Controls.Add(CreateButton("Run", RunProgram));
            buttonPanel.Controls.Add(CreateButton("Metrics", ShowMetrics));
            buttonPanel.Controls.Add(CreateButton("Load grid", LoadGrid));

            mainLayout.Controls.Add(buttonPanel, 0, 0);
            mainLayout.SetColumnSpan(buttonPanel, 2);

            //Program editor
            var editorGroup = new GroupBox
            {
                Text = "Program Editor",
                Dock = DockStyle.Fill
            };

            programEditor.Dock = DockStyle.Fill;
            programEditor.Multiline = true;
            programEditor.AcceptsTab = true;
            programEditor.AcceptsReturn = true;
            programEditor.ScrollBars = ScrollBars.Both;
            programEditor.Font= new Font("Consolas", 12);

            editorGroup.Controls.Add(programEditor);
            mainLayout.Controls.Add(editorGroup, 0, 1);

            //Output 

            var outputGroup = new GroupBox
            {
                Text = "Output",
                Dock = DockStyle.Fill
            };

            outputBox.Dock = DockStyle.Fill;
            outputBox.Multiline = true;
            outputBox.ReadOnly = true;
            outputBox.ScrollBars = ScrollBars.Vertical;

            outputGroup.Controls.Add(outputBox);
            mainLayout.Controls.Add(outputGroup, 1, 1);

            //Metrics
            var metricsGroup = new GroupBox
            {
                Text = "Metrics",
                Dock = DockStyle.Fill
            };

            metricsBox.Dock = DockStyle.Fill;
            metricsBox.Multiline = true;
            metricsBox.ReadOnly = true;

            metricsGroup.Controls.Add(metricsBox);
            mainLayout.Controls.Add(metricsGroup, 0, 2);

            //Path display
            var pathGroup = new GroupBox
            {
                Text = "Character Path",
                Dock = DockStyle.Fill
            };

            pathPanel.Dock = DockStyle.Fill;
            pathPanel.BackColor = Color.White;
            pathPanel.Paint += DrawPath;

            pathGroup.Controls.Add(pathPanel);
            mainLayout.Controls.Add(pathGroup, 1, 2);
        }

        /// <summary>
        /// Creates a button and connects it to a specified action.
        /// </summary>
        /// <param name="text"> The text displayed on the button </param>
        /// <param name="action"> The action executed when the button is called </param>
        /// <returns> The created button </returns>

        private Button CreateButton(string text, Action action)
        {
            var button = new Button
            {
                Text = text,
                AutoSize = true,
                Height = 30,
                Margin = new Padding(5)
            };

            button.Click += (_, _) => action();

            return button;
        }

        /// <summary>
        /// Loads grid text into the program service and prepares it for display.
        /// Clears the editor, so the user can write a new solution for the grid
        /// </summary>
        /// <param name="gridText"> The text represention of the grid </param>

        public void LoadGridText(string gridText)
        {
            if (string.IsNullOrWhiteSpace(gridText))
            {
                throw new ArgumentException("Grid cannot be empty.", nameof (gridText));
            }

            string normalizedGrid = gridText.Replace("\r\n", "\n").Replace('\r', '\n').Trim('\n');

            string[] lines = normalizedGrid.Split('\n');

            if (lines.Any(string.IsNullOrWhiteSpace))
            {
                throw new ArgumentException("Grid cannot contain empty rows.", nameof(gridText));
            }

            programService.LoadGridFromText(normalizedGrid);

            currentGrid = lines;
            currentPath = Array.Empty<(int X, int Y)>();

            //The user writes a new solution for the loaded grid
            programEditor.Clear();

            outputBox.Clear();
            metricsBox.Clear();

            pathPanel.Invalidate();
        }


        /// <summary>
        /// Opens a text file, loads its grid into the program service,
        /// and refreshes the grid displayed in the GUI.
        /// </summary>

        private void LoadGrid()
        {
            using var dialog = new OpenFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt|All Files(*.*)|*.*",
                Title = "Select a grid file"
            };

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                string gridText = File.ReadAllText(dialog.FileName);
                LoadGridText(gridText);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Grid load error", MessageBoxButtons.OK, MessageBoxIcon.Error);


            }
        }

        /// <summary>
        /// Opens a file dialog and loads the selected program into the editor
        /// </summary>

        private void LoadFile()
        {
            using var dialog = new OpenFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                Title = "Select a program "
            };

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            programEditor.Text = File.ReadAllText(dialog.FileName);

            outputBox.Clear();
            metricsBox.Clear();

            currentPath = Array.Empty<(int X, int Y)>();
            pathPanel.Invalidate();
        }

        /// <summary>
        /// Loads an example program from the Programs directory
        /// </summary>
        /// <param name="filename"> The name of the example program file </param>

        private void LoadExample (string filename)
        {
            string filePath = Path.Combine(AppContext.BaseDirectory, "Programs",filename);

            try
            {
                programEditor.Text = File.ReadAllText(filePath);
                outputBox.Clear();
                metricsBox.Clear();

                currentPath = Array.Empty<(int X, int Y)>();
                pathPanel.Invalidate();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, $"Could not load example", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// Loads the basic example program
        /// </summary>
        private void LoadBasic()
        {
            LoadExample("example1.txt");
        }

        /// <summary>
        /// Loads the advanced example program
        /// </summary>

        private void LoadAdvanced()
        {
            LoadExample("example2.txt");
        }

        /// <summary>
        /// Loads the expert example program
        /// </summary>

        private void LoadExpert()
        {
            LoadExample("example3.txt");
        }

        /// <summary>
        /// Executes the program currently written in the editor.
        /// Displays the execution trace, final path and character path.
        /// Runtime errors are displayed in the output box
        /// </summary>

        private void RunProgram()
        {
            currentPath = Array.Empty<(int X, int Y)>();
            pathPanel.Invalidate();
            outputBox.Clear();

            try
            {
                programService.LoadProgramFromText("Lunar Program", programEditor.Text);
                var result = programService.RunProgram();
                outputBox.Text = result.Trace + Environment.NewLine + $"End state: {result.FinalState}";

                currentPath = result.Path;
                pathPanel.Invalidate(); 

                if (result.RuntimeError != null)
                {
                    outputBox.AppendText(Environment.NewLine + $"Runtime error: {result.RuntimeError}");
                }
                else if (result.GoalReached)
                {
                    outputBox.AppendText(Environment.NewLine + $"Success: Goal reached");
                }
                else
                {
                    outputBox.AppendText(Environment.NewLine + $"Failure: Goal not reached");
                }

            }
            catch (Exception ex)
            {
                outputBox.Text = $"Runtime error: {ex.Message}";
                currentPath = Array.Empty<(int X, int Y)>();
                pathPanel.Invalidate();
            }
        }

        /// <summary>
        /// Calculates and displays the command count, maximum nesting depth 
        /// and repeat count of the current program 
        /// </summary>
        private void ShowMetrics()
        {
            metricsBox.Clear();
            try
            {
                programService.LoadProgramFromText("Lunar Program", programEditor.Text);

                var metrics = programService.GetMetrics();

                metricsBox.Text = $"Command count: {metrics.CommandCount}\r\n" + 
                                  $"Max nesting depth: {metrics.MaxNestingDepth}\r\n" + 
                                  $"Repeat count: {metrics.RepeatCount}\r\n";
            }
            catch (Exception ex)
            {
                metricsBox.Text = $"Metrics error: {ex.Message}";
            }
        }


        /// <summary>
        /// Draws the loaded asteroid field and the character's path. 
        /// Blocked cells are grey, open cells are white and the goal is green.
        /// The character's starting position is shown in blue and the final
        /// position is shown in red. 
        /// </summary>
        /// <param name="sender"> The control of that raised the paint event </param>
        /// <param name="e"> The paint event arguments </param>
        private void DrawPath(object? sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;

            if (currentGrid.Length == 0)
                return;

            int rows = currentGrid.Length;
            int columns = currentGrid[0].Length;
            
            int cellSize = Math.Max(1, Math.Min(pathPanel.Width / columns, pathPanel.Height / rows));

            int gridWidth = cellSize * columns;
            int gridHeight = cellSize * rows;

            int offsetX = (pathPanel.Width - gridWidth) / 2;
            int offsetY = (pathPanel.Height - gridHeight) / 2;

            using Pen gridPen = new(Color.Black, 1);
            using Brush openBrush = new SolidBrush(Color.White);
            using Brush blockedBrush = new SolidBrush(Color.Gray);
            using Brush goalBrush = new SolidBrush(Color.LightGreen);

            //Draw the grid cells
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    Brush cellBrush = currentGrid[row][col] switch
                    {
                        '+' => blockedBrush,
                        'x' => goalBrush,
                        _ => openBrush
                    };

                    Rectangle cell = new
                        (offsetX + col * cellSize, offsetY + row * cellSize, cellSize, cellSize);
                    
                    graphics.FillRectangle(cellBrush, cell);
                    graphics.DrawRectangle(gridPen, cell);
                }
            }

            if (currentPath.Count == 0)
                return;

            PointF[] points = currentPath.Select(p => new PointF(
                offsetX + p.Y * cellSize + cellSize / 2f,
                offsetY + p.X * cellSize + cellSize / 2f
            )).ToArray();

            if (points.Length > 1)
            {
                using Pen pathPen = new(Color.Blue, 3);
                graphics.DrawLines(pathPen, points);
            }

            using Brush startBrush = new SolidBrush(Color.Green);
            using Brush endBrush = new SolidBrush(Color.Red);

            graphics.FillEllipse(startBrush, points[0].X - 5, points[0].Y - 5, 10, 10);

            PointF last = points[^1];

            graphics.FillEllipse(endBrush, last.X - 5, last.Y - 5, 10, 10);
        }

    }
}
