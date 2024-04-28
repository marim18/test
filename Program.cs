using System.Device.Gpio;
using System;
using System.Threading;
using System.Diagnostics;

namespace Blinky
{
    public class Program
    {
        private static GpioController s_GpioController;
        private static PinButtonPair[] pinButtonPairs;
        private static int[] sequence;
        private static Timer timer;
        private static int elapsedTime;

        public class PinButtonPair
        {
            public GpioPin Pin { get; set; }
            public GpioPin Button { get; set; }

            public PinButtonPair(GpioPin pin, GpioPin button)
            {
                Pin = pin;
                Button = button;
            }
        }

        public static void Main()
        {
            s_GpioController = new GpioController();

            int[] pinPorts = { 12, 27, 25, 32 };
            int[] buttonPorts = { 14, 26, 16, 33 };

            pinButtonPairs = new PinButtonPair[pinPorts.Length];
            sequence = new int[pinButtonPairs.Length];

            // Open pins and buttons and create pairs
            for (int i = 0; i < pinPorts.Length; i++)
            {
                GpioPin pin = s_GpioController.OpenPin(pinPorts[i], PinMode.Output);
                GpioPin button = s_GpioController.OpenPin(buttonPorts[i], PinMode.InputPullUp);
                pinButtonPairs[i] = new PinButtonPair(pin, button);
            }

            LightUpSequence();
            Debug.WriteLine(DateTime.UtcNow.ToString() + ": creating timer, due in 1 second");

            //Timer testTimer = new Timer(CheckStatusTimerCallback, null, 1000, 1000);
            InputMode();

            // Keep the program running
            Thread.Sleep(Timeout.Infinite);
        }

        public static void LightUpSequence()
        {
            Random rand = new Random();

            // Generate a random sequence
            for (int i = 0; i < sequence.Length; i++)
            {
                sequence[i] = rand.Next(pinButtonPairs.Length);
            }

            // Display the sequence
            foreach (int index in sequence)
            {
                TurnOnLed(pinButtonPairs[index].Pin);
                Thread.Sleep(500); // LED on time
                TurnOffLed(pinButtonPairs[index].Pin);
                Thread.Sleep(200); // Wait between LEDs
            }
        }
        public static void TurnOnLed(GpioPin pin)
        {
            pin.Write(PinValue.High); // Turn on the LED
        }

        public static void TurnOffLed(GpioPin pin)
        {
            pin.Write(PinValue.Low); // Turn off the LED
        }
        void CheckStatusTimerCallback(object state)
        {
            Debug.WriteLine(DateTime.UtcNow.ToString() + ": blink");
            Thread.Sleep(125);
        }

        public static void InputMode()
        {
            //DateTime startTime = DateTime.Now;

            // Check if player presses buttons in the correct sequence
            for (int i = 0; i < sequence.Length; i++)
            {
                while (true)
                {
                    for (int j = 0; j < pinButtonPairs.Length; j++)
                    {
                        if (pinButtonPairs[j].Button.Read() == PinValue.Low)
                        {
                            // Button pressed
                            if (j == sequence[i])
                            {
                                // Correct button pressed
                                break;
                            }
                            else
                            {
                                // Incorrect button pressed
                                // Handle incorrect sequence here
                            }
                        }
                    }

                    // Check if the time limit has been reached
                    //   if ((DateTime.Now - startTime).TotalMilliseconds > 10000) // Adjust time limit as needed
                    {
                        // Time's up
                        // Handle time's up condition here
                        //      break;
                        //}
                    }

                    if (i == sequence.Length - 1)
                    {
                        // Sequence completed successfully within time limit
                        // Calculate score based on time and display it

                        string score = (DateTime.UtcNow.ToString() + ": destroying timer");
                        //  testTimer.Dispose(); ;// Adjust scoring mechanism as needed
                        Console.WriteLine($"Score: {score}");
                    }

                }
            }

        }
    }
}