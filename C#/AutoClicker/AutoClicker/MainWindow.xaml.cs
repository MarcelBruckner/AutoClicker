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
        private const string FILE_FILTER = "AutoClicker Files (*.autocl)|*.autocl";
        private bool _withDelay;
        private bool _infinite;
        private bool _isRunning;
        private bool _isRecording;
        private int allRepetitions;

        public ObservableCollection<Instruction> Instructions { get; private set; } = new ObservableCollection<Instruction>();
        private Recorder recorder;
        private KeyboardInterupt interupt;
        //private InstructionsParser parser;

        public bool IsRecording
        {
            get => _isRecording;
            set
            {
                _isRecording = value;
                PlayButton.IsEnabled = !value;

                if (_isRecording)
                {
                    IsRunning = false;
                    interupt.AddHooks();
                    recorder.AddHooks();
                }
                else
                {
                    interupt.RemoveHooks();
                    recorder.RemoveHooks();
                }
            }
        }
        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                _isRunning = value;
                RecordButton.IsEnabled = !value;
                InstructionsDataGrid.IsEnabled = !value;
                if (_isRunning)
                {
                    IsRecording = false;
                    interupt.AddHooks();
                    OnPlay();
                }
                else
                {
                    interupt.RemoveHooks();
                    runningInstruction.IsRunning = value;
                }
            }
        }
        public bool Infinite
        {
            get => _infinite;
            set
            {
                _infinite = value;
                RepetitionsTextBox.IsEnabled = !value;
            }
        }
        public bool WithDelay
        {
            get => _withDelay;
            set
            {
                _withDelay = value;
                recorder.WithDelay = value;
            }
        }
        public int Repetitions { get; set; }

        private Instruction runningInstruction = new Instruction();

        public MainWindow()
        {
            InitializeComponent();
            //parser = new InstructionsParser(this);
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

        private void OnPlay()
        {
            allRepetitions = Repetitions;
            BackgroundWorker bw = new BackgroundWorker()
            {
                WorkerReportsProgress = true
            };

            bw.DoWork += (sender, args) =>
            {
                for (int i = 0; Infinite || i < allRepetitions; i++)
                {
                    for (int j = 0; j < Instructions.Count; j++)
                    {
                        if (!IsRunning)
                        {
                            return;
                        }

                        runningInstruction = Instructions[j];
                        if (runningInstruction.Type == Instruction.Action.LOOP)
                        {
                            int loopRepetitions = runningInstruction.Repetitions;
                            for (int l = 1; l < loopRepetitions ; l++)
                            {
                                int loopCounter = j + 1;
                                while (loopCounter < Instructions.Count)
                                {
                                    if (!IsRunning)
                                    {
                                        return;
                                    }
                                    runningInstruction = Instructions[loopCounter];
                                    if (runningInstruction.Type == Instruction.Action.END_LOOP)
                                    {
                                        loopCounter = j;
                                        break;
                                    }

                                    bw.ReportProgress((i + 1) / 101, new[] { i + 1, loopCounter });
                                    runningInstruction.IsRunning = true;
                                    loopCounter++;
                                }
                            }
                        }
                        else
                        {
                            bw.ReportProgress((i + 1) / 101, new[] { i + 1, j });
                            Instructions[j].IsRunning = true;
                        }
                    }
                }
            };

            bw.ProgressChanged += (sender, args) =>
            {
                RepetitionsTextBox.Text = "" +((int[]) args.UserState)[0];
                HighlightLine(((int[])args.UserState)[1], Brushes.AntiqueWhite, Brushes.Black);
            };

            bw.RunWorkerCompleted += (sender, args) =>
            {
                if (args.Error != null)  // if an exception occurred during DoWork,
                {
                    Console.WriteLine(args.Error.ToString());  // do your error handling here
                }
                PlayButton.IsChecked = false;
                RepetitionsTextBox.Text = allRepetitions + "";

                ResetHighlights();
            };

            bw.RunWorkerAsync(); // start the background worker
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
                foreach (Instruction instruction in read)
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
            if (row >= 0)
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
        public void AddInstruction(Instruction instruction)
        {
            int row = InstructionsDataGrid.SelectedIndex + 1;
            if (row <= 0 || row > Instructions.Count)
            {
                row = Instructions.Count;
            }
            Instructions.Insert(row, instruction);
            InstructionsDataGrid.ScrollIntoView(instruction);
            InstructionsDataGrid.SelectedIndex = row;
        }

        public void UpdateDelay(int line, int delay)
        {
            Instructions[line].Delay = delay;
        }

        public void ResetHighlights()
        {
            for (int i = 0; i < Instructions.Count; i++)
            {
                DataGridRow row = GetDataGridRowFromIndex(i);
                row.Foreground = Brushes.Black;
                row.Background = Brushes.White;
            }
        }

        public void HighlightLine(int number, SolidColorBrush foreground, SolidColorBrush background)
        {
            ResetHighlights();
            DataGridRow row = GetDataGridRowFromIndex(number);
            row.Foreground = foreground;
            row.Background = background;
        }

        public delegate Point GetDragDropPosition(IInputElement element);

        private bool IsMouseOnTargetRow(Visual target, GetDragDropPosition pos)
        {
            try
            {
                Rect posBounds = VisualTreeHelper.GetDescendantBounds(target);
                Point mousePos = pos((IInputElement)target);
                return posBounds.Contains(mousePos);
            }
            catch { return false; }
        }

        private DataGridRow GetDataGridRowFromIndex(int index)
        {
            if(InstructionsDataGrid.ItemContainerGenerator.Status != System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
            {
                return null;
            }
            return InstructionsDataGrid.ItemContainerGenerator.ContainerFromIndex(index) as DataGridRow;
        }

        private int GetDataGridItemCurrentRowIndex(GetDragDropPosition pos)
        {
            int curIndex = -1;
            for (int i = 0; i < Instructions.Count; i++)
            {
                DataGridRow row = GetDataGridRowFromIndex(i);
                if(IsMouseOnTargetRow(row, pos))
                {
                    curIndex = i;
                    break;
                }
            }
            return curIndex;
        }

        int prevRowIndex = -1;

        private void InstructionsDataGrid_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            prevRowIndex = GetDataGridItemCurrentRowIndex(e.GetPosition);

            if(prevRowIndex < 0)
            {
                return;
            }

            InstructionsDataGrid.SelectedIndex = prevRowIndex;
            Instruction selected = Instructions[prevRowIndex];

            if(selected == null)
            {
                return;
            }

            DragDropEffects dragDropEffects = DragDropEffects.Move;

            DeleteRowBox.Visibility = Visibility.Visible;
            try
            {
                if (DragDrop.DoDragDrop(InstructionsDataGrid, selected, dragDropEffects) != DragDropEffects.None)
                {
                    InstructionsDataGrid.SelectedItem = selected;
                }
            }catch(Exception ex)
            {
                Console.WriteLine("Error in preview left mouse: " + ex.Message);
            }
        }

        private void InstructionsDataGrid_Drop(object sender, DragEventArgs e)
        {
            DeleteRowBox.Visibility = Visibility.Hidden;
            if (prevRowIndex < 0)
            {
                return;
            }

            int index = GetDataGridItemCurrentRowIndex(e.GetPosition);

            if(index == prevRowIndex)
            {
                return;
            }
            Instruction toMove = Instructions[prevRowIndex];
            Instructions.RemoveAt(prevRowIndex);
            if (index >= 0)
            {
                Instructions.Insert(index, toMove);
            }
        }
        #endregion
        
        private void DeleteRowBox_Drop(object sender, DragEventArgs e)
        {
            DeleteRowBox.Visibility = Visibility.Hidden;
            if (prevRowIndex < 0)
            {
                return;
            }
            
            Instruction toMove = Instructions[prevRowIndex];
            Instructions.RemoveAt(prevRowIndex);
        }
    }
}