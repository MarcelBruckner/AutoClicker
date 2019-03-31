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
        #region Global Attributes
        int prevRowIndex = -1;
        private static double _globalSpeed = 1;
        private static double _globalRandomSpeed = 0;
        private static int _globalDelay = 50;
        private static int _globalRandomDelay = 5;
        private static int _globalRepetitions = 1;
        private static int _globalRandomRepetitions;
        private static int _globalRandomX;
        private static int _globalRandomY;
        private static int _globalRandomDragX;
        private static int _globalRandomDragY;
        private static int _globalWheel;
        private static int _globalRandomWheel;
        private static bool _globalCtrl;
        private static bool _globalShift;
        private static bool _globalAlt;
        private static MovementType _globalMovement = MovementType.SPRING;
        #endregion

        Random random = new Random();

        private const string FILE_FILTER = "AutoClicker Files (*.autocl)|*.autocl";
        public static readonly System.Windows.Forms.Keys RECORD_HOTKEY = System.Windows.Forms.Keys.F8;
        public static readonly System.Windows.Forms.Keys PLAY_HOTKEY = System.Windows.Forms.Keys.F7;

        private bool _withDelay;
        private bool _infinite = true;
        private bool _isPlaying;
        private bool _isRecording;
        private int allRepetitions;
        private int _repetitions;

        public static ObservableCollection<Instruction> Instructions { get; private set; } = new ObservableCollection<Instruction>()
        {
            new Instruction(InstructionType.CLICK),
            new Instruction(InstructionType.M_CLICK),
            new Instruction(InstructionType.DRAG),
            new Instruction(InstructionType.WHEEL),
            new Instruction(InstructionType.KEYBOARD),
            new Instruction(InstructionType.LOOP),
            new Instruction(InstructionType.END_LOOP),
            new Instruction(InstructionType.DELAY)
        };

        private Recorder recorder;
        private Instruction runningInstruction = new Instruction();

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
                    IsPlaying = false;
                    recorder.AddHooks();
                }
                else
                {
                    Activate();
                    recorder.RemoveHooks();
                }
                OnPropertyChanged("IsRecording");
            }
        }
        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                _isPlaying = value;
                RecordButton.IsEnabled = !value;
                InstructionsDataGrid.IsEnabled = !value;
                if (_isPlaying)
                {
                    IsRecording = false;
                    WindowState = WindowState.Minimized;
                    OnPlay();
                }
                else
                {
                    StopAll();
                    WindowState = WindowState.Normal;
                    Activate();
                }
                OnPropertyChanged("IsPlaying");
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

        #region Globals
        public static MovementType GlobalMovement
        {
            get => _globalMovement;
            set {
                _globalMovement = value;
                if (MessageBoxYes())
                {
                    foreach (Instruction instruction in Instructions)
                    {
                        instruction.Movement = value;
                    }
                }
            }
        }
        public static double GlobalSpeed
        {
            get => _globalSpeed;
            set
            {
                _globalSpeed = value;
                if (MessageBoxYes())
                {
                    foreach (Instruction instruction in Instructions)
                    {
                        instruction.Speed = value;
                    }
                }
            }
        }
        public static double GlobalRandomSpeed
        {
            get => _globalRandomSpeed;
            set
            {
                _globalRandomSpeed = value;
                if (MessageBoxYes())
                {
                    foreach (Instruction instruction in Instructions)
                    {
                        instruction.RandomSpeed = value;
                    }
                }
            }
        }
        public static int GlobalDelay
        {
            get => _globalDelay;
            set
            {
                _globalDelay = value;
                if (MessageBoxYes())
                {
                    foreach (Instruction instruction in Instructions)
                    {
                        instruction.Delay = value;
                    }
                }
            }
        }
        public static int GlobalRandomDelay
        {
            get => _globalRandomDelay;
            set
            {
                _globalRandomDelay = value;
                if (MessageBoxYes())
                {
                    foreach (Instruction instruction in Instructions)
                    {
                        instruction.RandomDelay = value;
                    }
                }
            }
        }
        public static int GlobalRepetitions
        {
            get => _globalRepetitions;
            set
            {
                _globalRepetitions = value;
                if (MessageBoxYes())
                {
                    foreach (Instruction instruction in Instructions)
                    {
                        instruction.Repetitions = value;
                    }
                }
            }
        }
        public static int GlobalRandomRepetitions
        {
            get => _globalRandomRepetitions;
            set
            {
                _globalRandomRepetitions = value;
                if (MessageBoxYes())
                {
                    foreach (Instruction instruction in Instructions)
                    {
                        instruction.RandomRepetitions = value;
                    }
                }
            }
        }
        public static int GlobalRandomX
        {
            get => _globalRandomX;
            set
            {
                _globalRandomX = value;
                if (MessageBoxYes())
                {
                    foreach (Instruction instruction in Instructions)
                    {
                        instruction.RandomX = value;
                    }
                }
            }
        }
        public static int GlobalRandomY
        {
            get => _globalRandomY;
            set
            {
                _globalRandomY = value;
                if (MessageBoxYes())
                {
                    foreach (Instruction instruction in Instructions)
                    {
                        instruction.RandomY = value;
                    }
                }
            }
        }
        public static int GlobalRandomDragX
        {
            get => _globalRandomDragX;
            set
            {
                _globalRandomDragX = value;
                if (MessageBoxYes())
                {
                    foreach (Instruction instruction in Instructions)
                    {
                        instruction.RandomEndX = value;
                    }
                }
            }
        }
        public static int GlobalRandomDragY
        {
            get => _globalRandomDragY;
            set
            {
                _globalRandomDragY = value;
                if (MessageBoxYes())
                {
                    foreach (Instruction instruction in Instructions)
                    {
                        instruction.RandomEndY = value;
                    }
                }
            }
        }
        public static int GlobalWheel
        {
            get => _globalWheel;
            set
            {
                _globalWheel = value;
                if (MessageBoxYes())
                {
                    foreach (Instruction instruction in Instructions)
                    {
                        instruction.Wheel = value;
                    }
                }
            }
        }
        public static int GlobalRandomWheel
        {
            get => _globalRandomWheel;
            set
            {
                _globalRandomWheel = value;
                if (MessageBoxYes())
                {
                    foreach (Instruction instruction in Instructions)
                    {
                        instruction.RandomWheel = value;
                    }
                }
            }
        }
        public static bool GlobalCtrl
        {
            get => _globalCtrl;
            set
            {
                _globalCtrl = value;
                if (MessageBoxYes())
                {
                    foreach (Instruction instruction in Instructions)
                    {
                        instruction.Ctrl = value;
                    }
                }
            }
        }
        public static bool GlobalShift
        {
            get => _globalShift;
            set
            {
                _globalShift = value;
                if (MessageBoxYes())
                {
                    foreach (Instruction instruction in Instructions)
                    {
                        instruction.Shift = value;
                    }
                }
            }
        }
        public static bool GlobalAlt
        {
            get => _globalAlt;
            set
            {
                _globalAlt = value;
                if (MessageBoxYes())
                {
                    foreach (Instruction instruction in Instructions)
                    {
                        instruction.Alt = value;
                    }
                }
            }
        }
        #endregion


        //private TCPServer server;

        public MainWindow()
        {
            InitializeComponent();
            new HotkeyControl(PLAY_HOTKEY, OnPlayHotkey);
            new HotkeyControl(RECORD_HOTKEY, OnRecorderHotkey);
            recorder = new Recorder(this);
            //server = new TCPServer();
            InstructionsDataGrid.ItemsSource = Instructions;



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

        public void OnRecorderHotkey()
        {
            if (IsPlaying)
            {
                return;
            }
            StopAll();
            IsRecording = !IsRecording;
        }

        public void OnPlayHotkey()
        {
            if (IsRecording)
            {
                return;
            }
            StopAll();
            IsPlaying = !IsPlaying;
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
                        if (!IsPlaying)
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
                                if (Instructions[l].Type == InstructionType.END_LOOP)
                                {
                                    end = l;
                                    break;
                                }
                            }

                            int save = Instructions[j].Repetitions;
                            int loops = Instructions[j].Repetitions + random.Next(Instructions[j].RandomRepetitions);
                            for (int l = 0; l < loops; l++)
                            {
                                Instructions[j].Repetitions = l + 1;

                                for (int m = start; m < end; m++)
                                {
                                    if (!IsPlaying)
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
                            Instructions[j].Repetitions = save;
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
                IsPlaying = false;
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
        private void AddMoveAndClickClick_Click(object sender, RoutedEventArgs e)
        {
            AddInstruction(new Instruction(InstructionType.M_CLICK));
        }
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
        private void MovementType_Click(object sender, RoutedEventArgs e)
        {
            GlobalMovement = ConvertSender<MovementType>(sender);

            if (MessageBoxYes())
            {
                foreach (Instruction instruction in Instructions)
                {
                    instruction.Movement = GlobalMovement;
                }
            }
        }
        private void Speed_Click(object sender, RoutedEventArgs e)
        {
            GlobalSpeed = ConvertSender<double>(sender);
        }

        private void Alt_Click(object sender, RoutedEventArgs e)
        {
            HotkeyState state = ConvertSender<HotkeyState>(sender);
            GlobalAlt = state == HotkeyState.ON;
        }
        private void Shift_Click(object sender, RoutedEventArgs e)
        {
            HotkeyState state = ConvertSender<HotkeyState>(sender);
            GlobalShift = state == HotkeyState.ON;
        }
        private void Ctrl_Click(object sender, RoutedEventArgs e)
        {
            HotkeyState state = ConvertSender<HotkeyState>(sender);
            GlobalCtrl = state == HotkeyState.ON;
        }

        private static T ConvertSender<T>(object sender)
        {
            MenuItem choice = sender as MenuItem;
            return (T)choice.DataContext;
        }

        private static bool MessageBoxYes()
        {
            return MessageBox.Show("Update all existing instructions?", "Update", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
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


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private Instruction SelectClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            prevRowIndex = GetDataGridItemCurrentRowIndex(e.GetPosition);

            if (sender is TextBox box)
            {
                box.Focus();
                box.SelectAll();
            }
            else if (sender is ComboBox comboBox)
            {
                comboBox.Focus();
            }
            else if (sender is CheckBox checkBox)
            {
                checkBox.Focus();
                checkBox.IsChecked = !checkBox.IsChecked;
            }

            if (prevRowIndex < 0)
            {
                return null;
            }

            InstructionsDataGrid.SelectedIndex = prevRowIndex;
            return Instructions[prevRowIndex];
        }

        private void InstructionsDataGrid_PreviewMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Instruction selected = SelectClicked(sender, e);
        }

        private void InstructionsDataGrid_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Instruction selected = SelectClicked(sender, e);

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
            try
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
            catch
            {
                Console.WriteLine("Error by delete");
            }
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
        private Instruction GetInstructionFromCell(object sender)
        {
            DataGridCell cell = sender as DataGridCell;
            return cell.DataContext as Instruction;
        }
        #endregion
    }
}