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
using AutoClicker.Instructions;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

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

        /// <summary>
        /// The global and instructions separator
        /// </summary>
        private const string GLOBAL_AND_INSTRUCTIONS_SEPARATOR = "***********************************************************" +
            "**********************************************************************************************************************" +
            "**********************************************************************************************************************";

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
                        InstructionsTextBox.Document.Blocks.Clear();
                        string script = File.ReadAllText(dialog.FileName);

                        GlobalData = new GlobalData();

                        if (script.Contains(GLOBAL_AND_INSTRUCTIONS_SEPARATOR))
                        {
                            string[] splitted = script.Split(GLOBAL_AND_INSTRUCTIONS_SEPARATOR.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            try
                            {
                                GlobalData = JsonConvert.DeserializeObject<GlobalData>(splitted[0]);
                            }
                            catch
                            {
                                MessageBox.Show("Error deserializing the global values. Using default values.");
                            }

                            InstructionsTextBox.Document.Blocks.Add(StringManager.ConvertStringToBlock(splitted[1].Trim()));
                        }
                        else
                        {
                            MessageBox.Show("Couldn't distinguish between global values and instructions.");
                            InstructionsTextBox.Document.Blocks.Add(StringManager.ConvertStringToBlock(script));
                        }

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
                    string serialized = JsonConvert.SerializeObject(GlobalData);
                    serialized += GLOBAL_AND_INSTRUCTIONS_SEPARATOR;
                    serialized += StringManager.ConvertRichTextBoxToString(InstructionsTextBox);
                    File.WriteAllText(dialog.FileName, serialized);
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

        #region Edit Menu
        
        /// <summary>
        /// Converts the sender.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sender">The sender.</param>
        /// <returns></returns>
        private static T ConvertSender<T>(object sender)
        {
            MenuItem choice = sender as MenuItem;
            return (T)choice.DataContext;
        }

        /// <summary>
        /// Handles the Click event of the Clear control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            GlobalData = new GlobalData();
            InstructionsTextBox.Document.Blocks.Clear();
        }
        #endregion

        #region Context Menus
        /// <summary>
        /// Gets or sets a value indicating whether [minimize on play].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [minimize on play]; otherwise, <c>false</c>.
        /// </value>
        public bool MinimizeOnPlay { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether [maximize on stop play].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [maximize on stop play]; otherwise, <c>false</c>.
        /// </value>
        public bool MaximizeOnStopPlay { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [minimize on record].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [minimize on record]; otherwise, <c>false</c>.
        /// </value>
        public bool MinimizeOnRecord { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether [maximize on stop record].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [maximize on stop record]; otherwise, <c>false</c>.
        /// </value>
        public bool MaximizeOnStopRecord { get; set; } = true;

        /// <summary>
        /// Sets the state of the window.
        /// </summary>
        /// <param name="minimize">if set to <c>true</c> [maximized].</param>
        public void MinimizeWindow(bool minimize)
        {
            if (minimize)
            {
                WindowState = WindowState.Minimized;
            }
        }

        /// <summary>
        /// Sets the state of the window.
        /// </summary>
        /// <param name="minimize">if set to <c>true</c> [maximized].</param>
        public void MaximizeWindow(bool maximize)
        {
            if (maximize)
            {
                WindowState = WindowState.Normal;
                Activate();
            }
        }
        #endregion

        #region Add Menu

        private void AddMenu_Wait(object sender, RoutedEventArgs e)
        {
            AddInstruction(new Wait());
        }
        private void AddMenu_Loop(object sender, RoutedEventArgs e)
        {
            //AddInstruction(new Hover());
        }
        private void AddMenu_Hover(object sender, RoutedEventArgs e)
        {
            AddInstruction(new Hover());
        }

        private void AddMenu_Click(object sender, RoutedEventArgs e)
        {
            AddInstruction(new Click());
        }
        private void AddMenu_Drag(object sender, RoutedEventArgs e)
        {
            AddInstruction(new Drag());
        }
        private void AddMenu_Wheel(object sender, RoutedEventArgs e)
        {
            AddInstruction(new Wheel());
        }
        private void AddMenu_Keystroke(object sender, RoutedEventArgs e)
        {
            AddInstruction(new Keystroke());
        }
        private void AddMenu_Text(object sender, RoutedEventArgs e)
        {
            AddInstruction(new Text());
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
        public Run AddInstruction(Instruction instruction, Run toReplaceRun = null)
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
                foreach (Instruction instruction in Instructions)
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
                    MinimizeWindow(MinimizeOnPlay);
                    runner.Run(Instructions, Repetitions, IsInfiniteLooping);
                }
                else
                {
                    StopAll();
                    MaximizeWindow(MaximizeOnStopPlay);
                }
                InstructionsTextBox.IsEnabled = !value;
                NotifyPropertyChanged();
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
                NotifyPropertyChanged();
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
                NotifyPropertyChanged();
            }
        }
        #endregion
        #endregion

        #region Record
        /// <summary>
        /// The recorder
        /// </summary>
        private Recorder recorder;
        private GlobalData globalData;

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
                NotifyPropertyChanged();
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
                    MinimizeWindow(MinimizeOnRecord);
                }
                else
                {
                    recorder.RemoveHooks();
                    MaximizeWindow(MaximizeOnStopRecord);
                }
                InstructionsTextBox.IsEnabled = !value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        /// <summary>
        /// Gets the instructions.
        /// </summary>
        /// <value>
        /// The instructions.
        /// </value>
        private List<Instruction> Instructions => InstructionsParser.InstructionsParser.Parse(StringManager.ConvertRichTextBoxToString(InstructionsTextBox), GlobalData);

        /// <summary>
        /// Gets or sets the global data.
        /// </summary>
        /// <value>
        /// The global data.
        /// </value>
        public GlobalData GlobalData
        {
            get => globalData;
            set
            {
                globalData = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            DataContext = this;
            GlobalData = new GlobalData();
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
        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}