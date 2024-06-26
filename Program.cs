using System.Device.Gpio;
using System;
using System.Threading;
using System.Diagnostics;
using System.Collections;

namespace fitnessgame
{

    public class Program
    {
        public static GpioController s_GpioController;

        public class PinButtonPair
        {
            public GpioPin Pin { get; set; }
            public GpioPin Button { get; set; }
            public string Color { get; set; }

            public PinButtonPair(GpioPin pin, GpioPin button,string color)
            {
                Pin = pin;
                Button = button;
                Color = color;
            }
        }
        public static void Main()
        {
            s_GpioController = new GpioController();
            GpioPin BluePin = s_GpioController.OpenPin(13, PinMode.Output);
            GpioPin BlueButton = s_GpioController.OpenPin(14, PinMode.InputPullUp);
            GpioPin redpin = s_GpioController.OpenPin(32, PinMode.Output);
            GpioPin redButton = s_GpioController.OpenPin(16, PinMode.InputPullUp);
            GpioPin yellowPin = s_GpioController.OpenPin(26, PinMode.Output);
            GpioPin yellowButton = s_GpioController.OpenPin(27, PinMode.InputPullUp);
            GpioPin greenPin = s_GpioController.OpenPin(25, PinMode.Output);
            GpioPin greenButton = s_GpioController.OpenPin(33, PinMode.InputPullUp);
            PinButtonPair Redpair = new PinButtonPair(redpin, redButton,"red");
            PinButtonPair Bluepair = new PinButtonPair(BluePin, BlueButton,"blue");
            PinButtonPair yellowpair = new PinButtonPair(yellowPin, yellowButton,"yellow");
            PinButtonPair greenpair = new PinButtonPair(greenPin, greenButton,"green");
            PinButtonPair[] ListofPairs = new PinButtonPair[4] { Redpair, Bluepair, yellowpair, greenpair };

            while (true)
            {
                foreach (PinButtonPair pair in ListofPairs)
                {
                    pair.Pin.Write(PinValue.Low);
                   
                }
                Console.WriteLine("start setup");
                GpioPin[] newsequence = Sequence(ListofPairs);

                inputmode(newsequence);
                foreach (PinButtonPair pair in ListofPairs)
                {
                    pair.Pin.Write(PinValue.Low);
                  
                }
                Console.WriteLine("end setup");
            }
            GpioPin[] Sequence(PinButtonPair[] pinButtonPairs)
            {
                int[] sequence = new int[pinButtonPairs.Length];
                GpioPin[] Randomsequence = new GpioPin[pinButtonPairs.Length];
                Random rand = new Random();

     
                for (int i = 0; i < sequence.Length; i++)
                {
                    sequence[i] = rand.Next(sequence.Length);
                }

             
                
                for (int i = 0; i < sequence.Length; i++)
                {
                    pinButtonPairs[sequence[i]].Pin.Write(PinValue.High);
                    Console.WriteLine(pinButtonPairs[sequence[i]].Color+ "was lit");
                    Thread.Sleep(500);
                    Randomsequence[i] = pinButtonPairs[sequence[i]].Pin;
                    pinButtonPairs[sequence[i]].Pin.Write(PinValue.Low);  
                    Thread.Sleep(200); 
                }
                
                return Randomsequence; 
            }
             bool winnerchecker(GpioPin[] sequence, GpioPin[] sequencelist)
            {
                for (int i = 0; i < sequence.Length; i++)
                {
                    if (sequence[i] != sequencelist[i])
                    {
                        Console.WriteLine();
                        return false;
                    }
                }
                return true;
            }
            void inputmode(GpioPin[] sequence)
            {
                Console.WriteLine("game start");
                bool playing = true;
                while (playing)
                {
                    GpioPin[] sequencelist = new GpioPin[4];
                    for (int i = 0; i < sequence.Length; i++)
                    {
                        bool pressed_button = false;
                        while (!pressed_button)
                        {
                            foreach (PinButtonPair pair in ListofPairs)
                            {
                                if (pair.Button.Read() == PinValue.Low)
                                {

                                    pair.Pin.Write(PinValue.High);
                                    Console.WriteLine(pair.Color + "Was pressed");
                                    sequencelist[i] = pair.Pin;
                                    Thread.Sleep(100);
                                    pair.Pin.Write(PinValue.Low);
                                    pressed_button = true;
                                }
                                else
                                {
                                    pair.Pin.Write(PinValue.Low);
                                }
                            }
                        }
                        bool released_buttons = false;
                        while (!released_buttons)
                        {
                            bool is_button_down = false;
                            foreach (PinButtonPair pair in ListofPairs)
                            {
                                if (pair.Button.Read() == PinValue.Low)
                                {
                                    is_button_down = true;
                                }
                            }
                            Thread.Sleep(100);
                            if (!is_button_down)
                            { break; }
                        }
                        if (winnerchecker(sequence, sequencelist))
                        {
                            Console.WriteLine("You win!");
                            foreach (PinButtonPair pair in ListofPairs)
                            {
                                pair.Pin.Write(PinValue.High);
                            }
                            playing = false;
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            Console.WriteLine("you lose");
                            playing = false;
                        }
                    }
                  


                }
            }

        }

    }
}