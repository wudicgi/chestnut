// $Id$

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace WudiStudio.Chestnut.Utilities
{
    public class ProgressSpiner
    {
        private int m_anim_index = 0;
        private int m_anim_count = 4;
        private char[] m_anim_chars = new char[] {
            '-', '\\', '|', '/'
        };

        private Timer m_timer;

        private bool m_active = false;

        public ProgressSpiner()
        {
        }

        public void Start()
        {
            m_active = true;
            m_timer = new Timer(new TimerCallback(this.TimerCallback), null, 0, 50);
        }

        public void End()
        {
            m_active = false;
            m_timer.Dispose();
        }

        public void TimerCallback(object obj)
        {
            if (!m_active)
            {
                return;
            }
            Console.Write(m_anim_chars[m_anim_index % m_anim_count]);
            Console.Write('\b');
            //Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            m_anim_index++;
        }
    }

    public class ProgressBar
    {
        private const char c_start = '[';
        private const char c_block = '-';
        private const char c_end = ']';
        private const int c_length = 10;
        private const string c_percentage = "{0,9:0.00%}";

        private int m_anim_index = 0;
        private int m_anim_count = 4;
        private char[] m_anim_chars = new char[] {
            '-', '\\', '|', '/'
        };

        private int m_length = 50;

        private int m_minimum = 0;
        private int m_maximum = 100;

        private int m_value = 0;

        public int Minimum {
            get { return m_minimum; }
            set { m_minimum = value; }
        }

        public int Maximum {
            get { return m_maximum; }
            set { m_maximum = value; }
        }

        public int Value {
            get { return m_value; }
            set
            {
                if (value > m_maximum)
                {
                    m_value = m_maximum;
                }
                else
                {
                    m_value = value;
                }
            }
        }

        public int Width
        {
            get { return m_length + c_length; }
            set { m_length = value - c_length; }
        }

        public void Start(int max)
        {
            m_minimum = 0;
            m_maximum = max;
            m_value = 0;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
        }

        public void End()
        {
            m_value = m_maximum;
#if DEBUG
            this.Update();
            Console.WriteLine();
#else
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth - 1));
            Console.SetCursorPosition(0, Console.CursorTop);
#endif
            Console.ResetColor();
        }

        public void Increment(int value)
        {
            m_value += value;
        }

        public void Update()
        {
            Console.SetCursorPosition(0, Console.CursorTop);

            float tmp_1 = m_value / (float)m_maximum;
            int tmp_2 = (int)Math.Floor(tmp_1 * m_length);

            Console.Write(c_start);
            Console.Write((new string('*', tmp_2)).PadRight(m_length, '.'));
            Console.Write(c_end);
            Console.Write(c_percentage, tmp_1);
            Console.Write("  ");
            Console.Write(m_anim_chars[m_anim_index % m_anim_count]);

            /*
            string str = String.Concat(
                c_start,
                new string('*', tmp_2),
                new string('.', m_length - tmp_2),
                c_end,
                String.Format(c_percentage, tmp_1),
                "  ",
                m_anim_chars[m_anim_index % m_anim_count]
            );
            str = String.Format("[{0,-50:.}]  {1,8:0.00%}  {2}", new string('*', tmp_2), tmp_1, m_anim_chars[m_anim_index % m_anim_count]);
            Console.Write(str);
            */

            /*
            string str_1 = new string('*', tmp_2);
            string str_2 = new string('.', m_length - tmp_2);
            string str_3 = string.Format(c_percentage, tmp_1);

            Console.Write(c_start);
            Console.Write(str_1);
            Console.Write(str_2);
            Console.Write(c_end);
            Console.Write(str_3);
            Console.Write(' ');
            Console.Write(' ');
            if (tmp_2 < m_length)
            {
                Console.Write(m_anim_chars[m_anim_index % m_anim_count]);
            }
            */

            m_anim_index++;
        }
    }
    /*
    public class ProgressBar
    {
        private int m_counter = 0;
        private string[] m_chars = new string[] {
            "-", "\\", "|", "/"
        };

        private int m_shown;
        private int m_current;
        private int m_total;

        public int Current
        {
            get
            {
                return m_current;
            }
            set
            {
                if (value > m_total)
                {
                    m_current = m_total;
                }
                else
                {
                    m_current = value;
                }
            }
        }

        public ProgressBar()
        {
        }

        public void Start(int total)
        {
            m_current = 0;
            m_shown = 0;
            m_total = total;
        }

        public void End()
        {
            Console.WriteLine();
        }

        public void Spin() {
            string perc = String.Format("{0,8:0.00%}", (m_current / (float)m_total));
            int temp = (int)Math.Floor((double)((m_current * 50) / m_total));

            Console.SetCursorPosition(0, Console.CursorTop);

            Console.Write("[");
            Console.Write(new string('-', temp));
            Console.Write(new string(' ', 50 - temp));
            Console.Write(perc);
            Console.Write("]");
            m_counter++;
        }
    }
    */
}
