using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace delay_ms {
    class Program {
        private const string HeadFilePath = "../../config/head.txt";
        private const string ExeFilePath = "call_exe_tric.txt";
        private const string DelayTimeFilePath = "../../config/delay_{0}_time.txt";
        private const string DisplayFilePath = "../../config/delay_{0}_display.txt";
        private const string ResultFilePath = "test_head_{0}_result.txt";
        private static string head = "1";
        private static int delayTime = 500;
        private static string displayType = "display";

        static void Main(string[] args) {
            try
            {
                ReadHeadFile();
                File.Delete(HeadFilePath);
                File.WriteAllText(ExeFilePath, "");

                ReadDelayTimeFile();
                ReadDisplayTypeFile();

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Restart();
                while (stopwatch.ElapsedMilliseconds < delayTime)
                {
                    Console.WriteLine(stopwatch.ElapsedMilliseconds.ToString());
                    Thread.Sleep(10);
                }
                WriteResultFile(true);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
                WriteResultFile(false);
            }
        }

        private static void ReadHeadFile() {
            int timeout = 5000; // 5 seconds
            int elapsed = 0;
            while (true)
            {
                try
                {
                    head = File.ReadAllText(HeadFilePath);
                    break;
                } catch (Exception ex)
                {
                    if (elapsed >= timeout)
                    {
                        throw new Exception($"Unable to read file '{HeadFilePath}'.", ex);
                    }
                    elapsed += 50;
                    Thread.Sleep(50);
                }
            }
        }

        private static void ReadDelayTimeFile() {
            string delayTimeFile = string.Format(DelayTimeFilePath, head);
            try
            {
                delayTime = Convert.ToInt32(File.ReadAllText(delayTimeFile));
            } catch (Exception ex)
            {
                throw new Exception($"Unable to read file '{delayTimeFile}'.", ex);
            }
        }

        private static void ReadDisplayTypeFile() {
            string displayFile = string.Format(DisplayFilePath, head);
            try
            {
                displayType = File.ReadAllText(displayFile);
            } catch (Exception ex)
            {
                throw new Exception($"Unable to read file '{displayFile}'.", ex);
            }
        }

        private static void WriteResultFile(bool success) {
            string resultString = success ? "PASS" : "FAIL";
            string resultFile = string.Format(ResultFilePath, head);
            string resultText = string.Format("{0}\r\n{1}", delayTime, (displayType == "display" ? resultString : "NODISPLAY"));
            try
            {
                File.WriteAllText(resultFile, resultText);
            } catch (Exception ex)
            {
                throw new Exception($"Unable to write to file '{resultFile}'.", ex);
            }
        }
    }
}
