using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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
using System.Linq;

namespace AutoClicker
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private const string FILE_FILTER = "AutoClicker Files (*.autocl)|*.autocl";
        private bool _withDelay;
        private bool _infinite = true;
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
                    StopAll();
                    IsRunning = false;
                    interupt.AddHooks();
                    recorder.AddHooks();
                }
                else
                {
                    Activate();
                    interupt.RemoveHooks();
                    recorder.RemoveHooks();
                }
                OnPropertyChanged("IsRecording");
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
                    StopAll();
                    Activate();
                }
                OnPropertyChanged("IsRunning");
            }
        }
        public bool Infinite
        {
            get => _infinite;
            set
            {
                _infinite = value;
                RepetitionsTextBox.IsEnabled = !value;
                OnPropertyChanged("Infinite");
            }
        }
        public bool WithDelay
        {
            get => _withDelay;
            set
            {
                _withDelay = value;
                recorder.WithDelay = value;
                OnPropertyChanged("WithDelay");
            }
        }
        public int Repetitions
        {
            get => _repetitions;
            set
            {
                _repetitions = value;
                OnPropertyChanged("Repetitions");
            }
        }
        private Instruction runningInstruction = new Instruction();

        private TCPServer server;

        public MainWindow()
        {
            InitializeComponent();
            recorder = new Recorder(this);
            interupt = new KeyboardInterupt(OnKeyboardInterupt);
            server = new TCPServer(); 
            InstructionsDataGrid.ItemsSource = Instructions;

            //Instructions.Add(new Instruction(InstructionType.CLICK));
            //Instructions.Add(new Instruction(InstructionType.DRAG));
            //Instructions.Add(new Instruction(InstructionType.WHEEL));
            //Instructions.Add(new Instruction(InstructionType.KEYBOARD));
            //Instructions.Add(new Instruction(InstructionType.LOOP));
            //Instructions.Add(new Instruction(InstructionType.END_LOOP));
            //Instructions.Add(new Instruction(InstructionType.DELAY));
        }

        private void StopAll()
        {
            runningInstruction.IsRunning = false;
            foreach (Instruction instruction in Instructions)
            {
                instruction.IsRunning = false;
            }
        }

        #region Buttons
        public void OnKeyboardInterupt()
        {
            StopAll();
            IsRecording = false;
            IsRunning = false;
            interupt.RemoveHooks();
            recorder.RemoveHooks();
        }

        private void OnPlay()
        {
            StopAll();
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
                            StopAll();
                            return;
                        }

                        runningInstruction = Instructions[j];
                        if (runningInstruction.Type == InstructionType.LOOP)
                        {
                            int start = j + 1;
                            int end = Instructions.Count;
                            for (int l = start; l < Instructions.Count; l++)
                            {
                                if(Instructions[l].Type == InstructionType.END_LOOP)
                                {
                                    end = l;
                                    break;
                                }
                            }

                            int loops = Instructions[j].Repetitions;
                            for(int l = 0; l < loops; l++)
                            {
                                Instructions[j].Repetitions = l + 1;

                                for (int m = start; m < end; m++)
                                {
                                    if (!IsRunning)
                                    {
                                        StopAll();
                                        Instructions[j].Repetitions = loops;
                                        return;
                                    }
                                    bw.ReportProgress((i + 1) / 101, new[] { i + 1, m });
                                    runningInstruction = Instructions[m];
                                    runningInstruction.Run();
                                }
                            }
                            Instructions[j].Repetitions = loops;
                            j = end;
                        }
                        else
                        {
                            bw.ReportProgress((i + 1) / 101, new[] { i + 1, j });
                            runningInstruction.Run();
                        }
                    }
                }
            };

            bw.ProgressChanged += (sender, args) =>
            {
                RepetitionsTextBox.Text = "" + ((int[])args.UserState)[0];
            };

            bw.RunWorkerCompleted += (sender, args) =>
            {
                if (args.Error != null)  // if an exception occurred during DoWork,
                {
                    Console.WriteLine(args.Error.ToString());  // do your error handling here
                }
                IsRunning = false;
                RepetitionsTextBox.Text = allRepetitions + "";
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
            while (true)
            {
                if (dialog.ShowDialog() == true)
                {
                    try
                    {
                        StopAll();
                        string script = File.ReadAllText(dialog.FileName);
                        List<Instruction> read = JsonConvert.DeserializeObject<List<Instruction>>(script);
                        Instructions.Clear();
                        read.ForEach(instruction =>
                        {
                            instruction.IsRunning = false;
                            Instructions.Add(instruction);
                        });
                        break;
                    }
                    catch
                    {
                        MessageBox.Show("File has wrong format or is corrupted.");
                    }
                }
                else
                {
                    break;
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
                try
                {
                    StopAll();
                    string json = JsonConvert.SerializeObject(Instructions);
                    File.WriteAllText(dialog.FileName, json);
                }
                catch
                {
                    MessageBox.Show("Couldn't write to file.");
                }
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
            AddInstruction(new Instruction(InstructionType.CLICK));
        }
        private void AddKeyboard_Click(object sender, RoutedEventArgs e)
        {
            AddInstruction(new Instruction(InstructionType.KEYBOARD));
        }
        private void AddDelay_Click(object sender, RoutedEventArgs e)
        {
            AddInstruction(new Instruction(InstructionType.DELAY));
        }
        private void AddLoop_Click(object sender, RoutedEventArgs e)
        {
            AddInstruction(new Instruction(InstructionType.LOOP));
        }
        private void AddEndLoop_Click(object sender, RoutedEventArgs e)
        {
            AddInstruction(new Instruction(InstructionType.END_LOOP));
        }
        private void AddDrag_Click(object sender, RoutedEventArgs e)
        {
            AddInstruction(new Instruction(InstructionType.DRAG));
        }
        private void AddWheel_Click(object sender, RoutedEventArgs e)
        {
            AddInstruction(new Instruction(InstructionType.WHEEL));
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
            if (InstructionsDataGrid.ItemContainerGenerator.Status != System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
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
                if (IsMouseOnTargetRow(row, pos))
                {
                    curIndex = i;
                    break;
                }
            }
            return curIndex;
        }

        int prevRowIndex = -1;
        private int _repetitions;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void InstructionsDataGrid_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            prevRowIndex = GetDataGridItemCurrentRowIndex(e.GetPosition);

            if (prevRowIndex < 0)
            {
                return;
            }

            InstructionsDataGrid.SelectedIndex = prevRowIndex;
            Instruction selected = Instructions[prevRowIndex];

            if (selected == null)
            {
                return;
            }

            DragDropEffects dragDropEffects = DragDropEffects.Move;
            try
            {
                if (DragDrop.DoDragDrop(InstructionsDataGrid, selected, dragDropEffects) != DragDropEffects.None)
                {
                    InstructionsDataGrid.SelectedItem = selected;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in preview left mouse: " + ex.Message);
            }
        }

        private void InstructionsDataGrid_Drop(object sender, DragEventArgs e)
        {
            if (prevRowIndex < 0)
            {
                return;
            }

            int index = GetDataGridItemCurrentRowIndex(e.GetPosition);

            if (index == prevRowIndex)
            {
                return;
            }
            Instruction toMove = Instructions[prevRowIndex];
            Instructions.RemoveAt(prevRowIndex);
            if (index >= 0)
            {
                Instructions.Insert(index, toMove);
            }
            else
            {
                Instructions.Add(toMove);
            }
        }

        private void DeleteFromContextMenu_Click(object sender, RoutedEventArgs e)
        {
            //Get the clicked MenuItem
            var menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            var contextMenu = (ContextMenu)menuItem.Parent;

            //Find the placementTarget
            var item = (DataGrid)contextMenu.PlacementTarget;

            //Get the underlying item, that you cast to your object that is bound
            //to the DataGrid (and has subject and state as property)
            var toDeleteFromBindedList = (Instruction)item.SelectedCells[0].Item;

            //Remove the toDeleteFromBindedList object from your ObservableCollection
            Instructions.Remove(toDeleteFromBindedList);
        }

        private void DuplicateFromContextMenu_Click(object sender, RoutedEventArgs e)
        {
            //Get the clicked MenuItem
            var menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            var contextMenu = (ContextMenu)menuItem.Parent;

            //Find the placementTarget
            var item = (DataGrid)contextMenu.PlacementTarget;

            //Get the underlying item, that you cast to your object that is bound
            //to the DataGrid (and has subject and state as property)
            var toDuplicate = (Instruction)item.SelectedCells[0].Item;

            //Remove the toDeleteFromBindedList object from your ObservableCollection
            Instructions.Insert(Instructions.IndexOf(toDuplicate), new Instruction(toDuplicate));
        }

        
        #endregion

        #region Cells
        private void ShiftColumnCell_Click(object sender, RoutedEventArgs e)
        {
            Instruction i = GetInstructionFromCell(sender);
            i.Shift ^= true;
        }

        private void CtrlColumnCell_Click(object sender, RoutedEventArgs e)
        {
            GetInstructionFromCell(sender).Ctrl ^= true;
        }

        private void AltColumnCell_Click(object sender, RoutedEventArgs e)
        {
            GetInstructionFromCell(sender).Alt ^= true;
        }

        private Instruction GetInstructionFromCell(object sender)
        {
            DataGridCell cell = sender as DataGridCell;
            return cell.DataContext as Instruction;
        }
        #endregion
    }
}