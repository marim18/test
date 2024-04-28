using System.Device.Gpio;
using System;
using System.Threading;
using System.Diagnostics;

namespace fitnessgame
{

    public class Program
    {
        public static GpioController s_GpioController;
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
            GpioPin BluePin = s_GpioController.OpenPin(13, PinMode.Output);
            GpioPin BlueButton = s_GpioController.OpenPin(14, PinMode.InputPullUp);
           // GpioPin examplePin = s_GpioController.OpenPin(13, PinMode.Output);
            //GpioPin exampleButton = s_GpioController.OpenPin(14, PinMode.InputPullUp);
            GpioPin redpin = s_GpioController.OpenPin(32, PinMode.Output);
            GpioPin redButton = s_GpioController.OpenPin(16, PinMode.InputPullUp);
            GpioPin yellowPin = s_GpioController.OpenPin(27, PinMode.Output);
            GpioPin yellowButton = s_GpioController.OpenPin(26, PinMode.InputPullUp);
            GpioPin greenPin = s_GpioController.OpenPin(25, PinMode.Output);
            GpioPin greenButton = s_GpioController.OpenPin(33, PinMode.InputPullUp);
            PinButtonPair Redpair = new PinButtonPair(redpin,redButton);
            PinButtonPair Bluepair = new PinButtonPair(BluePin, BlueButton);
            PinButtonPair yellowpair = new PinButtonPair(yellowPin, yellowButton);
            PinButtonPair greenpair = new PinButtonPair(greenPin, greenButton);
            PinButtonPair[] ListofPairs = new PinButtonPair[4]{ Redpair,Bluepair,yellowpair,greenpair};

            while (true)
            {
                GpioPin[] newsequence = Sequence(ListofPairs);
                inputmode(newsequence);
            }
            GpioPin[] Sequence(PinButtonPair[] pinButtonPairs)
            {
                int[] sequence = new int[pinButtonPairs.Length];
                GpioPin[] Randomsequence = new GpioPin[pinButtonPairs.Length];
                Random rand = new Random();

                // Generate a random sequence
                for (int i = 0; i < sequence.Length; i++)
                {
                    sequence[i] = rand.Next(sequence.Length);
                }

                // Play the sequence
                foreach (int index in sequence)
                {
                    pinButtonPairs[index].Pin.Write(PinValue.High);
                    Thread.Sleep(500); // Wait for half a second
                    Randomsequence[index] = pinButtonPairs[index].Pin;
                    pinButtonPairs[index].Pin.Write(PinValue.Low);
                    Thread.Sleep(200); // Wait for 200 milliseconds between LEDs
                }
                return Randomsequence;//needs to return gpio[] for compare function
            }
            void inputmode(GpioPin[] sequence)
            {
                Console.WriteLine("game start");
                bool playing = true;
                while (playing)
                {
                    GpioPin[]sequencelist = new GpioPin[4];
                    for (int i = 0; i < sequence.Length; )
                    {
                        foreach (PinButtonPair pair in ListofPairs)
                        {
                            if (pair.Button.Read() == PinValue.Low)
                            {
                                pair.Pin.Toggle();
                                sequencelist[i] = pair.Pin;
                                Thread.Sleep(123);
                                i++;
                            }
                            else
                            {
                                pair.Pin.Write(PinValue.Low);
                            }

                        }
                    }
                    for (int i = 0; i < sequence.Length;)
                    {
                        if (i == sequence.Length-1 && sequence[i] == sequencelist[i])
                        {
                            Console.WriteLine("You win");
                            playing = false;
                        }
                        else if(i != sequence.Length && sequence[i] == sequencelist[i])
                        {
                            i++;
                        }
                        else
                        {
                            Console.WriteLine("you lose");
                            playing = false;
                            break;
                            
                        }
                    };
                    

                }
            }

        }
       
    }
}