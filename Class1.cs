/*using System.Device.Gpio;
using System;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using static Blinky.Program;



namespace Blinky
{
    public class Program
    {
        private static GpioController s_GpioController;
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

            PinButtonPair[] pinButtonPairs = new PinButtonPair[pinPorts.Length];

            // Open pins and buttons and create pairs
            for (int i = 0; i < pinPorts.Length; i++)
            {
                GpioPin pin = s_GpioController.OpenPin(pinPorts[i], PinMode.Output);
                GpioPin button = s_GpioController.OpenPin(buttonPorts[i], PinMode.InputPullUp);
                pinButtonPairs[i] = new PinButtonPair(pin, button);
            }



            // Keep the program running
            Thread.Sleep(Timeout.Infinite);
        }
        public void lightupmode()
        {
            MonitorButtons(pinButtonPairs);
            DateTime startTime = DateTime.Now;

        }
        public void inputmode()
        {
            checkPlayerInputs();
            TimeSpan elapsedTime = DateTime.Now - startTime;
            int score = (int)(10000 - elapsedTime.TotalMilliseconds);
        }
        private static void TurnOnLed(GpioPin pin)
        {
            pin.Write(PinValue.High); // Turn on the LED
        }

        private static void TurnOffLed(GpioPin pin)
        {
            pin.Write(PinValue.Low); // Turn off the LED
        }

        private static void MonitorButtons(PinButtonPair[] pinButtonPairs)
        {
            while (true)
            {

                foreach (var pair in pinButtonPairs)
                {
                    if (pair == null)
                    {
                        continue; // Skip to the next pair
                    }

                    // Check if the pin or button is null
                    if (pair.Pin == null || pair.Button == null)
                    {
                        continue; // Skip to the next pair
                    }
                    if (pair.Button.Read() == PinValue.Low) // Button pressed
                    {
                        if (pair.Pin.Read() == PinValue.High)
                        {
                            TurnOffLed(pair.Pin); // Turn off the LED if it's on
                        }
                        else
                        {
                            TurnOnLed(pair.Pin); // Turn on the LED if it's off
                        }

                        // Add a delay to debounce the button
                        Thread.Sleep(100);
                        //Sequence(pinButtonPairs); // Generate and play a sequence
                    }
                }
            }
            void Sequence(PinButtonPair[] pinButtonPairs)
            {
                int[] sequence = new int[pinButtonPairs.Length];
                Random rand = new Random();

                // Generate a random sequence
                for (int i = 0; i < sequence.Length; i++)
                {
                    sequence[i] = rand.Next(sequence.Length);
                }

                // Play the sequence
                foreach (int index in sequence)
                {
                    TurnOnLed(pinButtonPairs[index].Pin);
                    Thread.Sleep(500); // Wait for half a second
                    TurnOffLed(pinButtonPairs[index].Pin);
                    Thread.Sleep(200); // Wait for 200 milliseconds between LEDs
                }
            }
            public void checkPlayerinput()
            {
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
                                    //  wrong
                                }
                            }
                        }
                    }

                }
            }
        }/*