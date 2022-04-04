using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Motor
{
    public class Constants
    {
        public const byte STX = 0xFF; //this is same as #define in C language
    }

    public class Instructions
    {
        public const byte speedInst = 0x01; //this is same as #define in C language
        public const byte angleInst = 0x02;
        public const byte directionInst = 0x03;
    }

    public class PKT_INDEX
    {
        public const int HEADER = 0;
        public const int LENGTH = 1;
        public const int INST = 2;
        public const int PARAM_1 = 3;
        public const int PARAM_2 = 4;
        public const int CHECK = 5;
    }

    public class PKT_STATE
    {
        public const int HEADER = 0;
        public const int LENGTH = 1;
        public const int INST = 2;
        public const int PARAM_1 = 3;
        public const int PARAM_2 = 4;
        public const int CHECK = 5;

    }


    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Motor());
        }
    }
}
