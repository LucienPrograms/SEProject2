
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using SEProject2.UI;
using Xunit;

namespace SEProject2.Tests.GUITests
{
    /// <summary>
    /// Contains tests for the graphical user interface.
    /// </summary>
    public class GUITests
    {
        /// <summary>
        /// Verifies that the main GUI controls are created.
        /// </summary>
        [Fact]
        public void Form_ShouldContainMainControls()
        {
            RunOnStaThread(() =>
            {
                using Form1 form = new Form1();

                Assert.NotNull(FindButton(form, "Load File"));
                Assert.NotNull(FindButton(form, "Basic"));
                Assert.NotNull(FindButton(form, "Advanced"));
                Assert.NotNull(FindButton(form, "Expert"));
                Assert.NotNull(FindButton(form, "Run"));
                Assert.NotNull(FindButton(form, "Metrics"));
                Assert.NotNull(FindButton(form, "Load grid"));

                Assert.NotNull(FindGroupBox(form, "Program Editor"));
                Assert.NotNull(FindGroupBox(form, "Output"));
                Assert.NotNull(FindGroupBox(form, "Metrics"));
                Assert.NotNull(FindGroupBox(form, "Character Path"));
            });
        }

        /// <summary>
        /// Verifies that loading a new grid clears previous output and metrics.
        /// </summary>
        [Fact]
        public void LoadGridText_ShouldClearPreviousOutputAndMetrics()
        {
            RunOnStaThread(() =>
            {
                using Form1 form = new Form1();
                form.Show();
                System.Windows.Forms.Application.DoEvents();
                form.LoadGridText("ooo\nooo\nooo");

                TextBox editor = FindTextBox(form, "Program Editor");
                TextBox output = FindTextBox(form, "Output");
                TextBox metrics = FindTextBox(form, "Metrics");

                editor.Text = "Move 1";

                FindButton(form, "Metrics").PerformClick();
                FindButton(form, "Run").PerformClick();

                Assert.NotEmpty(output.Text);
                Assert.NotEmpty(metrics.Text);

                form.LoadGridText("ooo\nooo\nooo");

                Assert.Empty(output.Text);
                Assert.Empty(metrics.Text);
            });
        }

        /// <summary>
        /// Verifies that grid text with Windows line endings and
        /// a trailing newline can be loaded.
        /// </summary>
        [Fact]
        public void LoadGridText_ShouldHandleWindowsLineEndings()
        {
            RunOnStaThread(() =>
            {
                using Form1 form = new Form1();
                form.Show();
                System.Windows.Forms.Application.DoEvents();
                string grid = "ooo\r\nooo\r\nooo\r\n";

                form.LoadGridText(grid);

                TextBox editor = FindTextBox(form, "Program Editor");
                TextBox output = FindTextBox(form, "Output");

                editor.Text = "Move 1";

                FindButton(form, "Run").PerformClick();

                Assert.Contains("Move 1", output.Text);
                Assert.Contains("End state:", output.Text);
            });
        }

        /// <summary>
        /// Verifies that loading a grid with unequal row lengths
        /// throws an exception.
        /// </summary>
        [Fact]
        public void LoadGridText_ShouldThrowExceptionForInvalidGrid()
        {
            RunOnStaThread(() =>
            {
                using Form1 form = new Form1();

                Assert.ThrowsAny<Exception>(() =>
                    form.LoadGridText("ooo\noo"));
            });
        }

        /// <summary>
        /// Verifies that running a program displays its trace
        /// and final character state.
        /// </summary>
        [Fact]
        public void RunProgram_ShouldDisplayTraceAndFinalState()
        {
            RunOnStaThread(() =>
            {
                using Form1 form = new Form1();
                form.Show();
                System.Windows.Forms.Application.DoEvents();
                form.LoadGridText("ooo\nooo\nooo");

                TextBox editor = FindTextBox(form, "Program Editor");
                TextBox output = FindTextBox(form, "Output");

                editor.Text = "Move 1";

                FindButton(form, "Run").PerformClick();

                Assert.Contains("Move 1", output.Text);
                Assert.Contains(
                    "End state: (0, 1) facing East",
                    output.Text);
            });
        }

        /// <summary>
        /// Verifies that the GUI displays the metrics of a program.
        /// </summary>
        [Fact]
        public void ShowMetrics_ShouldDisplayProgramMetrics()
        {
            RunOnStaThread(() =>
            {
                using Form1 form = new Form1();
                form.Show();
                System.Windows.Forms.Application.DoEvents();
                form.LoadGridText("ooo\nooo\nooo");

                TextBox editor = FindTextBox(form, "Program Editor");
                TextBox metrics = FindTextBox(form, "Metrics");

                editor.Text = "Move 1\nTurn right";

                FindButton(form, "Metrics").PerformClick();

                Assert.Contains("Command count: 2", metrics.Text);
                Assert.Contains("Max nesting depth: 0", metrics.Text);
                Assert.Contains("Repeat count: 0", metrics.Text);
            });
        }

        /// <summary>
        /// Verifies that the grid and character path can be drawn
        /// without throwing an exception.
        /// </summary>
        [Fact]
        public void DrawPath_ShouldNotThrowException()
        {
            RunOnStaThread(() =>
            {
                using Form1 form = new Form1();

                form.ClientSize = new Size(1000, 600);
                form.Show();
                System.Windows.Forms.Application.DoEvents();

                form.LoadGridText("ooo\nooo\nooo");

                TextBox editor = FindTextBox(form, "Program Editor");
                editor.Text = "Move 1";

                FindButton(form, "Run").PerformClick();

                GroupBox pathGroup = FindGroupBox(
                    form,
                    "Character Path");

                Panel pathPanel = FindControl<Panel>(
                    pathGroup,
                    control => true);

                pathPanel.Refresh();

                Assert.True(pathPanel.Width > 0);
                Assert.True(pathPanel.Height > 0);
            });
        }

        /// <summary>
        /// Runs a test action on a separate STA thread.
        /// This is required because Windows Forms controls use STA.
        /// </summary>
        /// <param name="action">The test action to execute.</param>
        private static void RunOnStaThread(Action action)
        {
            Exception? caughtException = null;

            Thread staThread = new Thread(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }
            });

            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            staThread.Join();

            if (caughtException != null)
            {
                System.Runtime.ExceptionServices.ExceptionDispatchInfo
                    .Capture(caughtException)
                    .Throw();
            }
        }

        /// <summary>
        /// Finds a button by its displayed text.
        /// </summary>
        private static Button FindButton(Form form, string text)
        {
            return FindControl<Button>(
                form,
                button => button.Text == text);
        }

        /// <summary>
        /// Finds a group box by its displayed title.
        /// </summary>
        private static GroupBox FindGroupBox(Form form, string text)
        {
            return FindControl<GroupBox>(
                form,
                groupBox => groupBox.Text == text);
        }

        /// <summary>
        /// Finds the text box inside a group box with the specified title.
        /// </summary>
        private static TextBox FindTextBox(Form form, string groupBoxText)
        {
            GroupBox groupBox = FindGroupBox(form, groupBoxText);

            return FindControl<TextBox>(
                groupBox,
                textBox => true);
        }

        /// <summary>
        /// Recursively searches for a control of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of control to find.</typeparam>
        /// <param name="parent">The control in which to search.</param>
        /// <param name="predicate">The condition the control must satisfy.</param>
        /// <returns>The matching control.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no matching control is found.
        /// </exception>
        private static T FindControl<T>(
            Control parent,
            Func<T, bool> predicate) where T : Control
        {
            foreach (Control child in parent.Controls)
            {
                if (child is T matchingControl &&
                    predicate(matchingControl))
                {
                    return matchingControl;
                }

                T? nestedControl = FindControlOrNull<T>(
                    child,
                    predicate);

                if (nestedControl != null)
                {
                    return nestedControl;
                }
            }

            throw new InvalidOperationException(
                $"Could not find control of type {typeof(T).Name}.");
        }

        /// <summary>
        /// Searches recursively for a matching control and returns null
        /// if no matching control is found.
        /// </summary>
        private static T? FindControlOrNull<T>(
            Control parent,
            Func<T, bool> predicate) where T : Control
        {
            foreach (Control child in parent.Controls)
            {
                if (child is T matchingControl &&
                    predicate(matchingControl))
                {
                    return matchingControl;
                }

                T? nestedControl = FindControlOrNull<T>(
                    child,
                    predicate);

                if (nestedControl != null)
                {
                    return nestedControl;
                }
            }

            return null;
        }
    }
}

