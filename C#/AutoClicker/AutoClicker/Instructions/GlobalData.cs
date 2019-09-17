using Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoClicker.Instructions
{
    /// <summary>
    /// The global data structure
    /// </summary>
    public class GlobalData : INotifyPropertyChanged
    {
        private double speed = 1;
        private MovementType movementType = MovementType.SPRING;
        private double randomSpeed = 0;
        private int delay = 50;
        private int randomDelay = 5;
        private int repetitions = 1;
        private int randomRepetitions = 0;
        private int randomX = 0;
        private int randomY = 0;
        private int randomDragX = 0;
        private int randomDragY = 0;
        private int wheel = 0;
        private int randomWheel = 0;
        private bool ctrl = false;
        private bool shift = false;
        private bool alt = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalData"/> class.
        /// </summary>
        public GlobalData() { }

        /// <summary>
        /// Gets or sets the type of the global movement.
        /// </summary>
        /// <value>
        /// The type of the global movement.
        /// </value>
        [JsonProperty]
        public MovementType MovementType
        {
            get => movementType;
            set
            {
                movementType = value;
                NotifyPropertyChanged();
            }
        }
        /// <summary>
        /// Gets or sets the global speed.
        /// </summary>
        /// <value>
        /// The global speed.
        /// </value>
        [JsonProperty]
        public double Speed
        {
            get => speed;
            set
            {
                speed = value;
                NotifyPropertyChanged();
            }
        }
        /// <summary>
        /// Gets or sets the global random speed.
        /// </summary>
        /// <value>
        /// The global random speed.
        /// </value>
        [JsonProperty]
        public double RandomSpeed
        {
            get => randomSpeed;
            set
            {
                randomSpeed = value;
                NotifyPropertyChanged();
            }
        }
        /// <summary>
        /// Gets or sets the global delay.
        /// </summary>
        /// <value>
        /// The global delay.
        /// </value>
        [JsonProperty]
        public int Delay
        {
            get => delay;
            set
            {
                delay = value;
                NotifyPropertyChanged();
            }
        }
        /// <summary>
        /// Gets or sets the global random delay.
        /// </summary>
        /// <value>
        /// The global random delay.
        /// </value>
        [JsonProperty]
        public int RandomDelay
        {
            get => randomDelay;
            set
            {
                randomDelay = value;
                NotifyPropertyChanged();
            }
        }
        /// <summary>
        /// Gets or sets the global repetitions.
        /// </summary>
        /// <value>
        /// The global repetitions.
        /// </value>
        [JsonProperty]
        public int Repetitions
        {
            get => repetitions;
            set
            {
                repetitions = value;
                NotifyPropertyChanged();
            }
        }
        /// <summary>
        /// Gets or sets the global random repetitions.
        /// </summary>
        /// <value>
        /// The global random repetitions.
        /// </value>
        [JsonProperty]
        public int RandomRepetitions
        {
            get => randomRepetitions;
            set
            {
                randomRepetitions = value;
                NotifyPropertyChanged();
            }
        }
        /// <summary>
        /// Gets or sets the global random x.
        /// </summary>
        /// <value>
        /// The global random x.
        /// </value>
        [JsonProperty]
        public int RandomX
        {
            get => randomX;
            set
            {
                randomX = value;
                NotifyPropertyChanged();
            }
        }
        /// <summary>
        /// Gets or sets the global random y.
        /// </summary>
        /// <value>
        /// The global random y.
        /// </value>
        [JsonProperty]
        public int RandomY
        {
            get => randomY;
            set
            {
                randomY = value;
                NotifyPropertyChanged();
            }
        }
        /// <summary>
        /// Gets or sets the global random drag x.
        /// </summary>
        /// <value>
        /// The global random drag x.
        /// </value>
        [JsonProperty]
        public int RandomDragX
        {
            get => randomDragX;
            set
            {
                randomDragX = value;
                NotifyPropertyChanged();
            }
        }
        /// <summary>
        /// Gets or sets the global random drag y.
        /// </summary>
        /// <value>
        /// The global random drag y.
        /// </value>
        [JsonProperty]
        public int RandomDragY
        {
            get => randomDragY;
            set
            {
                randomDragY = value;
                NotifyPropertyChanged();
            }
        }
        /// <summary>
        /// Gets or sets the global wheel.
        /// </summary>
        /// <value>
        /// The global wheel.
        /// </value>
        [JsonProperty]
        public int Wheel
        {
            get => wheel;
            set
            {
                wheel = value;
                NotifyPropertyChanged();
            }
        }
        /// <summary>
        /// Gets or sets the global random wheel.
        /// </summary>
        /// <value>
        /// The global random wheel.
        /// </value>
        [JsonProperty]
        public int RandomWheel
        {
            get => randomWheel;
            set
            {
                randomWheel = value;
                NotifyPropertyChanged();
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [global control].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [global control]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty]
        public bool Ctrl
        {
            get => ctrl;
            set
            {
                ctrl = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [global shift].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [global shift]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty]
        public bool Shift
        {
            get => shift;
            set
            {
                shift = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [global alt].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [global alt]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty]
        public bool Alt
        {
            get => alt;
            set
            {
                alt = value;
                NotifyPropertyChanged();
            }
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
