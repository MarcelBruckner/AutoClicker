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
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;

namespace AutoClicker
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int Repetitions { get; set; }

        public ObservableCollection<Instruction> Instructions { get; private set; } = new ObservableCollection<Instruction>();
        private Recorder recorder;
        private KeyboardInterupt interupt;
        private InstructionsParser parser;

        public bool IsRunning { get; set; }

        private bool _withDelay;

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
            PlayButton.IsEnabled = true;
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
                for(int i = 0; i < Instructions.Count; i++)
                {
                    //HighlightLine(i, Brushes.Blue);
                    Instructions[i].Execute();
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
                string script = File.ReadAllText(dialog.FileName);
                Instructions.Clear();
                List<Instruction> read = JsonConvert.DeserializeObject<List<Instruction>>(script);
                foreach(Instruction instruction in read)
                {
                    Instructions.Add(instruction); 
                }
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
                string json = JsonConvert.SerializeObject(Instructions);
                File.WriteAllText(dialog.FileName, json);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
        #endregion

        #region Edit Menu
        private void AddClick_Click(object sender, RoutedEventArgs e)
        {
            AddInstruction(new Instruction(Instruction.Action.CLICK));
        }
        private void AddKeyboard_Click(object sender, RoutedEventArgs e)
        {
            AddInstruction(new Instruction(Instruction.Action.KEYBOARD));
        }
        private void AddDelay_Click(object sender, RoutedEventArgs e)
        {
            AddInstruction(new Instruction(Instruction.Action.DELAY));
        }
        private void AddLoop_Click(object sender, RoutedEventArgs e)
        {
            AddInstruction(new Instruction(Instruction.Action.LOOP));
        }
        private void AddEndLoop_Click(object sender, RoutedEventArgs e)
        {
            AddInstruction(new Instruction(Instruction.Action.END_LOOP));
        }
        private void AddDrag_Click(object sender, RoutedEventArgs e)
        {
            AddInstruction(new Instruction(Instruction.Action.DRAG));
        }

        private void SelectedLineRemove_Click(object sender, RoutedEventArgs e)
        {
            int row = InstructionsDataGrid.SelectedIndex;
            if(row >= 0)
            {
                Instructions.RemoveAt(row);
            }
        }
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Instructions.Clear();
        }

        #endregion

        #region Instructions 
        private void InstructionsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool isRecording = RecordButton.IsChecked ?? false;
            PlayButton.IsEnabled = SpellCorrect && !isRecording;
        }

        public void AddInstruction(Instruction instruction)
        {
            int row = InstructionsDataGrid.SelectedIndex;
            if(row < 0)
            {
                row = Instructions.Count;
            }
            Instructions.Insert(row, instruction);
            InstructionsDataGrid.ScrollIntoView(instruction);
        }

        public void UpdateDelay(int line, int delay)
        {
            Instructions[line].Delay = delay;
        }
        
        public void HighlightLine(int number, SolidColorBrush brush)
        {
            for (int i = 0; i < Instructions.Count; i++)
            {
                DataGridRow row = (DataGridRow)InstructionsDataGrid.ItemContainerGenerator.ContainerFromIndex(i);
                if (i == number)
                {
                    row.Background = brush;
                }
                else
                {
                    row.Background = Brushes.White;
                }
            }
        }
        #endregion
    }
}