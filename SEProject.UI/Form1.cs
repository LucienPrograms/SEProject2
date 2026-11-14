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
        private IReadOnlyList<(int X, int Y)> currentPath = Array.Empty<(int X, int Y)>();


        public Form1()
        {
            InitializeComponent();

            Text = "SEProject2 - Programming App";
            Width = 1000;
            Height = 600;
            StartPosition = FormStartPosition.CenterScreen;

            BuildInterface();
        }

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

        private void LoadBasic()
        {
            LoadExample("example1.txt");
        }

        private void LoadAdvanced()
        {
            LoadExample("example2.txt");
        }

        private void LoadExpert()
        {
            LoadExample("example3.txt");
        }

        private void RunProgram()
        {
            currentPath = Array.Empty<(int X, int Y)>();
            pathPanel.Invalidate();

            try
            {
                programService.LoadProgramFromText("Lunar Program", programEditor.Text);
                var result = programService.RunProgram();
                outputBox.Text = result.Trace + Environment.NewLine + $"End state: {result.FinalState}";

                currentPath = result.Path;
                pathPanel.Invalidate(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Program error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowMetrics()
        {
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
                MessageBox.Show(ex.Message, "Metrics error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DrawPath(object? sender, PaintEventArgs e)
        {
            if (currentPath.Count == 0)
                return;

            Graphics graphics = e.Graphics;

            int margin = 30;
            int availableWidth = Math.Max(1, pathPanel.Width - 2 * margin);
            int availableHeight = Math.Max(1, pathPanel.Height - 2 * margin);
            
            int minX = currentPath.Min(p => p.X);
            int maxX = currentPath.Max(p => p.X);
            int minY = currentPath.Min(p => p.Y);
            int maxY = currentPath.Max(p => p.Y);

            int rangeX = Math.Max(1, maxX - minX);
            int rangeY = Math.Max(1, maxY - minY);

            float scale = Math.Min(
            (float)availableWidth / rangeX, 
            (float)availableHeight / rangeY);

            PointF[] points = currentPath.Select(p => new PointF(
                (p.X - minX) * scale + margin,
                (p.Y - minY) * scale + margin
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
