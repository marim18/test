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
                foreach(PinButtonPair pair in ListofPairs){
                    if( pair.Button.Read() == PinValue.Low)
                    {
                        pair.Pin.Toggle();
                        Thread.Sleep(123);
                    }
                    else
                    {
                        pair.Pin.Write(PinValue.Low);
                    }

                }
              
                
               

            }

        }
    }
}