using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoClicker
{
    /// <summary>
    /// The runner class for the instructions
    /// </summary>
    public class Runner
    {
        /// <summary>
        /// The window
        /// </summary>
        private readonly MainWindow window;

        /// <summary>
        /// Gets or sets the running instruction.
        /// </summary>
        /// <value>
        /// The running instruction.
        /// </value>
        private Instructions.Instruction RunningInstruction { get; set; } = new Instructions.Instruction();

        /// <summary>
        /// Gets or sets the repetitions.
        /// </summary>
        /// <value>
        /// The repetitions.
        /// </value>
        private int Repetitions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is playing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is playing; otherwise, <c>false</c>.
        /// </value>
        private bool IsPlaying { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Runner"/> class.
        /// </summary>
        /// <param name="mainWindow">The main window.</param>
        public Runner(MainWindow mainWindow)
        {
            window = mainWindow;
        }

        /// <summary>
        /// Runs the specified instructions.
        /// </summary>
        /// <param name="instructions">The instructions.</param>
        /// <param name="repetitions">The repetitions.</param>
        public void Run(List<Instructions.Instruction> instructions, int repetitions=1, bool isInfiniteLooping=true)
        {
            Stop();
            Repetitions = repetitions;
            IsPlaying = true;

            BackgroundWorker bw = new BackgroundWorker()
            {
                WorkerReportsProgress = true
            };

            bw.DoWork += (sender, args) =>
            {
                for (int i = 0; isInfiniteLooping || i < Repetitions; i++)
                {
                    for (int j = 0; j < instructions.Count; j++)
                    {
                        if (!IsPlaying)
                        {
                            Stop();
                            return;
                        }

                        bw.ReportProgress((i + 1) / 101, new[] { i + 1, j });
                        //instructions[j].Run();

                        RunningInstruction = instructions[j];
                        RunningInstruction.Run();
                        if (RunningInstruction is Instructions.Loop)
                        {
                            //    {
                            //        int start = j + 1;
                            //        int end = Instructions.Count;
                            //        for (int l = start; l < Instructions.Count; l++)
                            //        {
                            //            if (Instructions[l].Type == InstructionType.END_LOOP)
                            //            {
                            //                end = l;
                            //                break;
                            //            }
                            //        }

                            //        int save = Instructions[j].Repetitions;
                            //        int loops = Instructions[j].Repetitions + random.Next(Instructions[j].RandomRepetitions);
                            //        for (int l = 0; l < loops; l++)
                            //        {
                            //            Instructions[j].Repetitions = l + 1;

                            //            for (int m = start; m < end; m++)
                            //            {
                            //                if (!IsPlaying)
                            //                {
                            //                    StopAll();
                            //                    Instructions[j].Repetitions = loops;
                            //                    return;
                            //                }
                            //                bw.ReportProgress((i + 1) / 101, new[] { i + 1, m });
                            //                runningInstruction = Instructions[m];
                            //                runningInstruction.Run();
                            //            }
                            //        }
                            //        Instructions[j].Repetitions = save;
                            //        j = end;
                        }
                        else
                        {
                            bw.ReportProgress((i + 1) / 101, new[] { i + 1, j });
                        }
                    }
                }
            };

            bw.ProgressChanged += (sender, args) =>
            {
                window.RepetitionsTextBox.Text = "" + ((int[])args.UserState)[0];
            };

            bw.RunWorkerCompleted += (sender, args) =>
            {
                if (args.Error != null)  // if an exception occurred during DoWork,
                {
                    Console.WriteLine(args.Error.ToString());  // do your error handling here
                }
                IsPlaying = false;
                window.RepetitionsTextBox.Text = Repetitions + "";
            };

            bw.RunWorkerAsync(); // start the background worker
        }

        public void Stop()
        {
            RunningInstruction.IsRunning = false;
            IsPlaying = false;
        }
    }
}
