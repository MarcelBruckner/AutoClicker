using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using AutoClicker.Instructions;
using System.ComponentModel;
using Microsoft.Win32;
using System.IO;
using System.Windows.Documents;
using System.Threading;
using System.Collections.Generic;

namespace AutoClicker
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int Repetitions { get; set; }

        private Instruction mainLoop = new Instruction(Instruction.Action.LOOP);
        private Recorder recorder;
        private KeyboardInterupt interupt;
        private const string FILE_FILTER = "AutoClicker Files (*.autocl)|*.autocl";
        
        public MainWindow()
        {
            InitializeComponent();
            InstructionsTextBox.Document.Blocks.Clear();
        }

        #region Buttons
        private void RecordButton_Checked(object sender, RoutedEventArgs e)
        {
            PlayButton.IsEnabled = false;
            mainLoop.IsRunning = false;
            interupt = new KeyboardInterupt(RecordButton, PlayButton);
            recorder = new Recorder(InstructionsTextBox);
        }

        private void RecordButton_Unchecked(object sender, RoutedEventArgs e)
        {
            PlayButton.IsEnabled = true;
            mainLoop.IsRunning = false;
            e.Handled = true;
            interupt.RemoveAllHooks();
            recorder.RemoveAllHooks();
        }

        private void PlayButton_Checked(object sender, RoutedEventArgs e)
        {
            RecordButton.IsEnabled = false;
            interupt = new KeyboardInterupt(RecordButton, PlayButton);
            OnPlay();
        }

        private void PlayButton_Unchecked(object sender, RoutedEventArgs e)
        {
            RecordButton.IsEnabled = true;
            interupt.RemoveAllHooks();
            mainLoop.IsRunning = false;
        }

        private void OnPlay()
        {
            BackgroundWorker bw = new BackgroundWorker();

            bw.DoWork += (sender, args) =>
            {
                mainLoop = InstructionsParser.Instance.Parse(StringManager.RichTextBoxToString(InstructionsTextBox), 0, Repetitions);
                    mainLoop.Execute();
            };

            bw.RunWorkerCompleted += (sender, args) =>
            {
                if (args.Error != null)  // if an exception occurred during DoWork,
                {
                    Console.WriteLine(args.Error.ToString());  // do your error handling here
                }
                PlayButton.IsChecked = false;
            };

            bw.RunWorkerAsync(); // start the background worker
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            InstructionsTextBox.Document.Blocks.Clear();
        }
        #endregion

        #region Infinite Checkbox
        private void InfiniteCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            RepetitionsTextBox.IsEnabled = false;
            Repetitions = -1;
        }

        private void InfiniteCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            RepetitionsTextBox.IsEnabled = true;
        }
        #endregion

        #region TextBoxes
        private void RepetitionsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box = (TextBox)sender;

            RemoveNotNumberCharacters(box);
            if (string.IsNullOrWhiteSpace(box.Text))
            {
                Repetitions = 0;
                return;
            }
            Repetitions = int.Parse(box.Text);
        }

        private void RemoveNotNumberCharacters(TextBox textBox)
        {
            int cursorPosition = textBox.SelectionStart;
            StringBuilder allowedChars = new StringBuilder();
            foreach (char c in textBox.Text)
            {
                if (Char.IsNumber(c))
                {
                    allowedChars.Append(c);
                }
                else
                {
                    cursorPosition--;
                }
            }
            textBox.Text = allowedChars.ToString();
            SetCursorPosititon(textBox, cursorPosition);
        }

        private void SetCursorPosititon(TextBox textBox, int position)
        {
            textBox.SelectionStart = position;
            textBox.SelectionLength = 0;
        }
        #endregion

        #region File Menu
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("On open");
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = FILE_FILTER
            };
            if (dialog.ShowDialog() == true)
            {
                InstructionsTextBox.Document.Blocks.Clear();
                string script = File.ReadAllText(dialog.FileName);
                InstructionsTextBox.Document.Blocks.Add(new Paragraph(new Run(script)));
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("On save");
            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = FILE_FILTER
            };
            if (dialog.ShowDialog() == true)
            {
                File.WriteAllLines(dialog.FileName, new[] { StringManager.RichTextBoxToString(InstructionsTextBox) });
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
        #endregion

        #region Add Menu
        private void AddClick_Click(object sender, RoutedEventArgs e)
        {
            InstructionsTextBox.Document.Blocks.Add(new Paragraph(new Run(new MouseClick().ToString())));
        }
        private void AddNormalKeyboard_Click(object sender, RoutedEventArgs e)
        {
            InstructionsTextBox.Document.Blocks.Add(new Paragraph(new Run(new Keyboard().ToString())));
        }
        private void AddSpecialKeyboard_Click(object sender, RoutedEventArgs e)
        {
            InstructionsTextBox.Document.Blocks.Add(new Paragraph(new Run(new SpecialKeyboard().ToString())));
        }
        private void AddDelay_Click(object sender, RoutedEventArgs e)
        {
            InstructionsTextBox.Document.Blocks.Add(new Paragraph(new Run(new Delay().ToString())));
        }
        private void AddLoop_Click(object sender, RoutedEventArgs e)
        {
            InstructionsTextBox.Document.Blocks.Add(new Paragraph(new Run(new Loop().ToString())));
        }
        private void AddEndLoop_Click(object sender, RoutedEventArgs e)
        {
            InstructionsTextBox.Document.Blocks.Add(new Paragraph(new Run(new EndLoop().ToString())));
        }
        private void AddDrag_Click(object sender, RoutedEventArgs e)
        {
            InstructionsTextBox.Document.Blocks.Add(new Paragraph(new Run(new MouseDrag().ToString())));
        }

        #endregion

        private void InstructionsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            InstructionsParser.Instance.SpellCheck(InstructionsTextBox.Document);
        }
    }
}