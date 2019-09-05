using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using Microsoft.Win32;
using System.IO;
using System.Windows.Documents;
using System.Collections.Generic;
using Enums;

namespace AutoClicker
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Constants
        /// <summary>
        /// The random number generator
        /// </summary>
        private static readonly Random random = new Random();

        /// <summary>
        /// The file filter
        /// </summary>
        private const string FILE_FILTER = "AutoClicker Files (*.autocl)|*.autocl";

        /// <summary>
        /// The record hotkey
        /// </summary>
        public const System.Windows.Forms.Keys RECORD_HOTKEY = System.Windows.Forms.Keys.F8;

        /// <summary>
        /// The play hotkey
        /// </summary>
        public const System.Windows.Forms.Keys PLAY_HOTKEY = System.Windows.Forms.Keys.F7;
        #endregion

        #region Menus
        #region File Menu

        /// <summary>
        /// Handles the Click event of the Open control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
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
                        InstructionsTextBox.Document.Blocks.Add(StringManager.ConvertStringToBlock(script));
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

        /// <summary>
        /// Handles the Click event of the Save control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
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
                    File.WriteAllText(dialog.FileName, StringManager.ConvertRichTextBoxToString(InstructionsTextBox));
                }
                catch
                {
                    MessageBox.Show("Couldn't write to file.");
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the Close control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
        #endregion
        
        #region Defaults Menu
        #region Global Menu Properties

        /// <summary>
        /// Gets or sets the type of the global movement.
        /// </summary>
        /// <value>
        /// The type of the global movement.
        /// </value>
        public static MovementType GlobalMovementType { get; set; } = MovementType.SPRING;

        /// <summary>
        /// Gets or sets the type of the global button.
        /// </summary>
        /// <value>
        /// The type of the global button.
        /// </value>
        public static ButtonType GlobalButtonType { get; set; }

        /// <summary>
        /// Gets or sets the global speed.
        /// </summary>
        /// <value>
        /// The global speed.
        /// </value>
        public static double GlobalSpeed { get; set; } = 1;

        /// <summary>
        /// Gets or sets the global random speed.
        /// </summary>
        /// <value>
        /// The global random speed.
        /// </value>
        public static double GlobalRandomSpeed { get; set; } = 0;

        /// <summary>
        /// Gets or sets the global delay.
        /// </summary>
        /// <value>
        /// The global delay.
        /// </value>
        public static int GlobalDelay { get; set; } = 50;

        /// <summary>
        /// Gets or sets the global random delay.
        /// </summary>
        /// <value>
        /// The global random delay.
        /// </value>
        public static int GlobalRandomDelay { get; set; } = 5;

        /// <summary>
        /// Gets or sets the global repetitions.
        /// </summary>
        /// <value>
        /// The global repetitions.
        /// </value>
        public static int GlobalRepetitions { get; set; } = 1;

        /// <summary>
        /// Gets or sets the global random repetitions.
        /// </summary>
        /// <value>
        /// The global random repetitions.
        /// </value>
        public static int GlobalRandomRepetitions { get; set; } = 0;

        /// <summary>
        /// Gets or sets the global random x.
        /// </summary>
        /// <value>
        /// The global random x.
        /// </value>
        public static int GlobalRandomX { get; set; } = 0;

        /// <summary>
        /// Gets or sets the global random y.
        /// </summary>
        /// <value>
        /// The global random y.
        /// </value>
        public static int GlobalRandomY { get; set; } = 0;

        /// <summary>
        /// Gets or sets the global random drag x.
        /// </summary>
        /// <value>
        /// The global random drag x.
        /// </value>
        public static int GlobalRandomDragX { get; set; } = 0;

        /// <summary>
        /// Gets or sets the global random drag y.
        /// </summary>
        /// <value>
        /// The global random drag y.
        /// </value>
        public static int GlobalRandomDragY { get; set; } = 0;

        /// <summary>
        /// Gets or sets the global wheel.
        /// </summary>
        /// <value>
        /// The global wheel.
        /// </value>
        public static int GlobalWheel { get; set; } = 0;

        /// <summary>
        /// Gets or sets the global random wheel.
        /// </summary>
        /// <value>
        /// The global random wheel.
        /// </value>
        public static int GlobalRandomWheel { get; set; } = 0;

        /// <summary>
        /// Gets or sets a value indicating whether [global control].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [global control]; otherwise, <c>false</c>.
        /// </value>
        public static bool GlobalCtrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [global shift].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [global shift]; otherwise, <c>false</c>.
        /// </value>
        public static bool GlobalShift { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [global alt].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [global alt]; otherwise, <c>false</c>.
        /// </value>
        public static bool GlobalAlt { get; set; }
        #endregion
        
        #region Edit Menu
        private void MovementType_Click(object sender, RoutedEventArgs e)
        {
            GlobalMovementType = ConvertSender<MovementType>(sender);
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

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            InstructionsTextBox.Document.Blocks.Clear();
        }
        #endregion
        #endregion

        #region Add Menu

        private void AddMenu_Delay(object sender, RoutedEventArgs e)
        {
            //AddInstruction(new Instructions.Hover());
        }
        private void AddMenu_Loop(object sender, RoutedEventArgs e)
        {
            //AddInstruction(new Instructions.Hover());
        }
        private void AddMenu_Hover(object sender, RoutedEventArgs e)
        {
            AddInstruction(new Instructions.Hover());
        }

        private void AddMenu_Click(object sender, RoutedEventArgs e)
        {
            AddInstruction(new Instructions.Click());
        }
        private void AddMenu_Drag(object sender, RoutedEventArgs e)
        {
            AddInstruction(new Instructions.Drag());
        }
        private void AddMenu_Wheel(object sender, RoutedEventArgs e)
        {
            AddInstruction(new Instructions.Wheel());
        }
        private void AddMenu_Keystroke(object sender, RoutedEventArgs e)
        {
            AddInstruction(new Instructions.Keystroke());
        }
        private void AddMenu_Text(object sender, RoutedEventArgs e)
        {
            AddInstruction(new Instructions.Text());
        }

        #endregion
        #endregion

        #region Form Components
        #region Buttons

        /// <summary>
        /// Stops all.
        /// </summary>
        private void StopAll()
        {
            runner.Stop();
        }

        /// <summary>
        /// Called when recorder hotkey is pressed.
        /// </summary>
        public void OnRecorderHotkeyPressed()
        {
            if (IsPlaying)
            {
                return;
            }
            IsRecording = !IsRecording;
        }

        /// <summary>
        /// Called when play hotkey is pressed.
        /// </summary>
        public void OnPlayHotkeyPressed()
        {
            if (IsRecording)
            {
                return;
            }
            IsPlaying = !IsPlaying;
        }

        #endregion

        #region TextBoxes

        /// <summary>
        /// Handles the TextChanged event of the RepetitionsTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Removes the not number characters.
        /// </summary>
        /// <param name="textBox">The text box.</param>
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

        /// <summary>
        /// Sets the cursor posititon.
        /// </summary>
        /// <param name="textBox">The text box.</param>
        /// <param name="position">The position.</param>
        private void SetCursorPosititon(TextBox textBox, int position)
        {
            textBox.SelectionStart = position;
            textBox.SelectionLength = 0;
        }
        #endregion

        #region InstructionsTextBox        
        /// <summary>
        /// Adds the instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <returns></returns>
        public Run AddInstruction(Instructions.Instruction instruction, Run toReplaceRun = null)
        {
            if (toReplaceRun is null)
            {
                Run newRun = new Run(instruction.ToString());
                Block block = new Paragraph(newRun);
                InstructionsTextBox.Document.Blocks.Add(block);
                InstructionsTextBox.ScrollToEnd();
                return newRun;
            }
            else
            {
                toReplaceRun.Text = instruction.ToString();
                return toReplaceRun;
            }
        }

        private void InstructionsTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Return ||
                e.Key == System.Windows.Input.Key.Tab ||
                e.Key == System.Windows.Input.Key.Space)
            {
                foreach (Instructions.Instruction instruction in Instructions)
                {
                    Console.WriteLine(instruction);
                }
            }
        }

        private void InstructionsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
        #endregion
        #endregion

        #region Run

        /// <summary>
        /// The runner
        /// </summary>
        private Runner runner;

        #region Run Attributes
        /// <summary>
        /// The with delay
        /// </summary>
        private bool _withDelay;

        /// <summary>
        /// The infinite
        /// </summary>
        private bool _infinite = true;

        /// <summary>
        /// The is playing
        /// </summary>
        private bool _isPlaying;

        /// <summary>
        /// The is recording
        /// </summary>
        private bool _isRecording;
        
        /// <summary>
        /// The repetitions
        /// </summary>
        private int _repetitions;
        #endregion

        #region Run Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance is playing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is playing; otherwise, <c>false</c>.
        /// </value>
        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                _isPlaying = value;
                RecordButton.IsEnabled = !value;
                if (_isPlaying)
                {
                    IsRecording = false;
                    //WindowState = WindowState.Minimized;
                    runner.Run(Instructions, Repetitions, IsInfiniteLooping);
                }
                else
                {
                    StopAll();
                    //WindowState = WindowState.Normal;
                    Activate();
                }
                InstructionsTextBox.IsEnabled = !value;
                OnPropertyChanged("IsPlaying");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is infinite looping.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is infinite looping; otherwise, <c>false</c>.
        /// </value>
        public bool IsInfiniteLooping
        {
            get => _infinite;
            set
            {
                _infinite = value;
                RepetitionsTextBox.IsEnabled = !value;
                OnPropertyChanged("Infinite");
            }
        }

        /// <summary>
        /// Gets or sets the repetitions.
        /// </summary>
        /// <value>
        /// The repetitions.
        /// </value>
        public int Repetitions
        {
            get => _repetitions;
            set
            {
                _repetitions = value;
                OnPropertyChanged("Repetitions");
            }
        }
        #endregion
        #endregion

        #region Record
        /// <summary>
        /// The recorder
        /// </summary>
        private Recorder recorder;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is recording with delay.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is recording with delay; otherwise, <c>false</c>.
        /// </value>
        public bool IsRecordingWithDelay
        {
            get => _withDelay;
            set
            {
                _withDelay = value;
                recorder.IsRecordingWithDelay = value;
                OnPropertyChanged("WithDelay");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is recording.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is recording; otherwise, <c>false</c>.
        /// </value>
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
                InstructionsTextBox.IsEnabled = !value;
                OnPropertyChanged("IsRecording");
            }
        }
        #endregion

        /// <summary>
        /// Gets the instructions.
        /// </summary>
        /// <value>
        /// The instructions.
        /// </value>
        private List<Instructions.Instruction> Instructions => InstructionsParser.InstructionsParser.Parse(StringManager.ConvertRichTextBoxToString(InstructionsTextBox));
        
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            InstructionsTextBox.Document.Blocks.Clear();
            new HotkeyControl(PLAY_HOTKEY, OnPlayHotkeyPressed);
            new HotkeyControl(RECORD_HOTKEY, OnRecorderHotkeyPressed);
            recorder = new Recorder(this);
            runner = new Runner(this);
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}