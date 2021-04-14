using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Device.Gpio;
using System.Threading.Tasks;
using System.Threading;

namespace button_tester
{
    public partial class frmRpiGPIO : Form
    {
        public bool setDState;
        public int setDChannel;
        public bool getDState = false;
        public int getDChannel;
        
        public frmRpiGPIO()
        {
            InitializeComponent();
#if MOCKUP
#elif DEBUG
            //Défini les pins du Raspberry Pi lors de la compilation du programme.
            LJ.gpio.OpenPin(9, PinMode.Input);
            LJ.gpio.OpenPin(10, PinMode.Input);
            LJ.gpio.OpenPin(11, PinMode.Input);
            LJ.gpio.OpenPin(12, PinMode.Input);
            LJ.gpio.OpenPin(13, PinMode.Input);
            LJ.gpio.OpenPin(14, PinMode.Input);
            LJ.gpio.OpenPin(15, PinMode.Input);
            LJ.gpio.OpenPin(16, PinMode.Input);
            LJ.gpio.OpenPin(1, PinMode.Output);
            LJ.gpio.OpenPin(2, PinMode.Output);
            LJ.gpio.OpenPin(3, PinMode.Output);
            LJ.gpio.OpenPin(4, PinMode.Output);
            LJ.gpio.OpenPin(5, PinMode.Output);
            LJ.gpio.OpenPin(6, PinMode.Output);
            LJ.gpio.OpenPin(7, PinMode.Output);
            LJ.gpio.OpenPin(8, PinMode.Output);
            //MessageBox.Show($"1 : {setDChannel}, {setDState}");
#else
#endif

        }

        public bool GetDState(int channel, GpioController gpio)
        {
            channel++;
            foreach (Control cnt in Controls)
                if (cnt.Tag != null && (int)cnt.Tag == channel)
                {
                    if (channel >= 9 && channel <= 16)
                    {
                        if (gpio.Read(channel) == PinValue.High)
                        {
                            cnt.BackColor = Color.Green;
                            return true;
                        }
                        else
                        {
                            cnt.BackColor = Color.Red;
                            return false;
                        }
                    }
                }
            return false;
            //throw new Exception("Wrong channel");
        }

        public void SetDState(int channel, bool state, GpioController gpio)
        {
            foreach (Control cnt in Controls)
                if (cnt.Tag!=null&&(int)cnt.Tag == channel)
                {
                    if (channel >= 1 && channel <= 8)
                    {
                        if (state)
                        {
                            gpio.Write(channel, state);
                            cnt.BackColor = Color.Green;
                        }
                        else
                        {
                            gpio.Write(channel, state);
                            cnt.BackColor = Color.Red;
                        }
                    }
                }

            //throw new Exception("Wrong channel");
        }

        public float GetAState(int channel)
        {
            return Up.BackColor == Color.Green ? 2.0f :
                Still.BackColor == Color.Green ? 0.0f :
                -2.0f;
        }

        Label Up, Down, Still;

        private void frmRpiGPIO_Load(object sender, EventArgs e)
        {
            int x = 5, y = 5, h = 32, w = 32, s = 5;
            Label lbl;

            for (var i = 0; i < 16; ++i)
            {
                lbl = new Label();
                lbl.Text = "D" + (i + 1);
                lbl.TextAlign = ContentAlignment.MiddleCenter;
                lbl.BackColor = Color.Red;
                lbl.Size = new Size(w, h);
                lbl.Top = y;
                lbl.Left = x;
                lbl.Tag = i+1;

                x += w + s;

                if (i == 7)
                {
                    x = s;
                    y += s + h;
                }

                Controls.Add(lbl);
            }

            x = 5;
            y += h + s;

            Up = new Label();
            Up.Text = "^";
            Up.TextAlign = ContentAlignment.MiddleCenter;
            Up.BackColor = Color.Red;
            Up.Size = new Size(w, h);
            Up.Top = y;
            Up.Left = x;
            Controls.Add(Up);
            x += w + s;

            Still = new Label();
            Still.Text = "0";
            Still.TextAlign = ContentAlignment.MiddleCenter;
            Still.BackColor = Color.Green;
            Still.Size = new Size(w, h);
            Still.Top = y;
            Still.Left = x;
            Controls.Add(Still);
            x += w + s;

            Down = new Label();
            Down.Text = "v";
            Down.TextAlign = ContentAlignment.MiddleCenter;
            Down.BackColor = Color.Red;
            Down.Size = new Size(w, h);
            Down.Top = y;
            Down.Left = x;
            Controls.Add(Down);
            x += w + s;

            Width = 17 * s + 8 * w;
            Height = 4 * s + 3 * h+30;
        }
    }
}
