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
using System.Windows.Media;
using System.Collections.ObjectModel;

namespace AutoClicker
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int Repetitions { get; set; }

        private ObservableCollection<Instruction> Instructions = new ObservableCollection<Instruction>();
        private Recorder recorder;
        private KeyboardInterupt interupt;
        private InstructionsParser parser;

        public bool IsRunning { get => _isRunning; set => _isRunning = value; }

        private bool _withDelay;
        private bool _isRunning;

        public bool WithDelay
        {
            get => _withDelay;
            set
            {
                _withDelay = value;
                recorder.WithDelay = value;
            }
        }

        private const string FILE_FILTER = "AutoClicker Files (*.autocl)|*.autocl";
        private bool SpellCorrect { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            parser = new InstructionsParser(this);
            recorder = new Recorder(this);
            interupt = new KeyboardInterupt(OnKeyboardInterupt);
            InstructionsDataGrid.ItemsSource = Instructions;
            Instructions.Add(new MouseClick());
            Instructions.Add(new MouseDrag());
            Instructions.Add(new Keyboard());
            Instructions.Add(new Delay());
        }

        #region Buttons
        public void OnKeyboardInterupt()
        {
            RecordButton.IsChecked = false;
            PlayButton.IsChecked = false;
            interupt.RemoveHooks();
            recorder.RemoveHooks();
        }


        private void RecordButton_Checked(object sender, RoutedEventArgs e)
        {
            PlayButton.IsEnabled = false;
            IsRunning = false;
            interupt.AddHooks();
            recorder.AddHooks();
        }

        private void RecordButton_Unchecked(object sender, RoutedEventArgs e)
        {
            PlayButton.IsEnabled = SpellCorrect;
            IsRunning = false;
            e.Handled = true;

            interupt.RemoveHooks();
            recorder.RemoveHooks();
        }

        private void PlayButton_Checked(object sender, RoutedEventArgs e)
        {
            RecordButton.IsEnabled = false;
            interupt.AddHooks();
            OnPlay();
        }

        private void PlayButton_Unchecked(object sender, RoutedEventArgs e)
        {
            RecordButton.IsEnabled = true;
            IsRunning = false;
            interupt.RemoveHooks();
        }

        private void OnPlay()
        {
            BackgroundWorker bw = new BackgroundWorker();

            bw.DoWork += (sender, args) =>
            {
                //Instru = parser.Parse(StringManager.RichTextBoxToString(InstructionsTextBox), 0, Repetitions);
                foreach (Instruction instruction in Instructions)
                {
                    instruction.Execute();
                }
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

            //InstructionsTextBox.Document.Blocks.Clear();
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
                box.Text = 0 + "";
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
                //InstructionsTextBox.Document.Blocks.Clear();
                string script = File.ReadAllText(dialog.FileName);
                //InstructionsTextBox.Document.Blocks.Add(new Paragraph(new Run(script)));
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
                //File.WriteAllLines(dialog.FileName, new[] { StringManager.RichTextBoxToString(InstructionsTextBox) });
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
            Instructions.Add(new MouseClick());
        }
        private void AddNormalKeyboard_Click(object sender, RoutedEventArgs e)
        {
            AddInstruction(new Keyboard());
        }
        private void AddSpecialKeyboard_Click(object sender, RoutedEventArgs e)
        {
            AddInstruction(new SpecialKeyboard());
        }
        private void AddDelay_Click(object sender, RoutedEventArgs e)
        {
            AddInstruction(new Delay());
        }
        private void AddLoop_Click(object sender, RoutedEventArgs e)
        {
            AddInstruction(new Loop());
        }
        private void AddEndLoop_Click(object sender, RoutedEventArgs e)
        {
            AddInstruction(new EndLoop());
        }
        private void AddDrag_Click(object sender, RoutedEventArgs e)
        {
            AddInstruction(new MouseDrag());
        }

        #endregion

        #region Instructions 
        private void InstructionsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool isRecording = RecordButton.IsChecked ?? false;
            //SpellCorrect = parser.SpellCheck(InstructionsTextBox.Document);
            PlayButton.IsEnabled = SpellCorrect && !isRecording;
        }

        public void AddInstruction(Instruction instruction)
        {
            //InstructionsTextBox.Document.Blocks.InsertBefore(InstructionsTextBox.CaretPosition.Paragraph, new Paragraph(new Run(instruction.ToString())));
        }

        public void UpdateLast(Instruction instruction)
        {
            //InstructionsTextBox.CaretPosition = InstructionsTextBox.CaretPosition.GetLineStartPosition(-1);
            //InstructionsTextBox.CaretPosition.DeleteTextInRun(InstructionsTextBox.CaretPosition.GetTextRunLength(LogicalDirection.Forward));
            //InstructionsTextBox.CaretPosition.InsertTextInRun(instruction.ToString());
            //InstructionsTextBox.CaretPosition = InstructionsTextBox.CaretPosition.GetLineStartPosition(1);
        }

        public void EnableTextBox(bool enabled)
        {
            //InstructionsTextBox.IsEnabled = enabled;
        }

        public void HighlightLine(int number, SolidColorBrush brush)
        {
            //BlockCollection blocks = InstructionsTextBox.Document.Blocks;

            //int i = 0;
            //foreach (Block block in blocks)
            //{
            //    if (i++ == number)
            //    {
            //        TextRange range = new TextRange(block.ContentStart, block.ContentEnd);
            //        range.ApplyPropertyValue(TextElement.ForegroundProperty, brush);
            //    }
            //}
        }
        #endregion
    }
}