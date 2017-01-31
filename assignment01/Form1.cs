using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;

namespace assignment01
{
    public partial class Form1 : Form
    {
        public static Form form;
        public static Thread thread;
        public static bool song = false;
        public static int counter = 2;
        public static bool color = false;
        public static int fps = 30;
        public static double running_fps = 30.0;
        public static int v;
        public static SoundPlayer player = new SoundPlayer(Properties.Resources.SaxMusic);

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            form = this;
            thread = new Thread(new ThreadStart(run));
            thread.Start();
            

        }

        public static void run()
        {
            DateTime last = DateTime.Now;
            DateTime now = last;
            TimeSpan frameTime = new TimeSpan(10000000 / fps);
            while (true)
            {
                DateTime temp = DateTime.Now;
                running_fps = .9 * running_fps + .1 * 1000.0 / (temp - now).TotalMilliseconds;
                Console.WriteLine(running_fps);
                now = temp;
                TimeSpan diff = now - last;
                if (diff.TotalMilliseconds < frameTime.TotalMilliseconds)
                    Thread.Sleep((frameTime - diff).Milliseconds);
                last = DateTime.Now;
                Iterate();
                form.Invoke(new MethodInvoker(form.Refresh));

            }
            
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                counter += 2;
            }
            else if (e.KeyCode == Keys.R)
            {
                color = !color;
            }
        }

        private void UpdateSize()
        {

        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            thread.Abort();
        }

        protected override void OnResize(EventArgs e)
        {
            Refresh();
        }

        private static void Iterate()
        {
            if(song == false)
            {
                player.PlayLooping();
                song = true;
            }
            v = (int)(200 + 100 * Math.Tan(DateTime.Now.Millisecond / 500.0));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            DateTime now = DateTime.Now;
            for (int i = counter; i > 0; i--)
            {
                Random rng = new Random((int)(i * Math.Sin(v) * Math.Sin(i) * 12));
                Color NoahColor = Color.FromArgb(rng.Next(256), rng.Next(256), rng.Next(256));
                e.Graphics.DrawImage(Properties.Resources.sax, v, v, v, v);
                if (color)
                {
                    this.BackColor = NoahColor;
                }
            }
            System.Drawing.Font font = new System.Drawing.Font("Comic Sans MS", 12);
            e.Graphics.DrawString("Iterations: "+counter.ToString(), font, Brushes.Black, ClientSize.Width/2, 100);
            e.Graphics.DrawString("Press R for a Rave! (WARNING: Flashing Colors)", font, Brushes.Black, ClientSize.Width / 5, 25);
            e.Graphics.DrawString("Press W to tank framerate", font, Brushes.Black, ClientSize.Width / 5, 45);
            e.Graphics.DrawString(running_fps.ToString(), font, Brushes.Black, ClientSize.Width/2 - 30, ClientSize.Height - 50);
        }


    }

}