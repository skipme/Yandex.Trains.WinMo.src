using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data.Common;
using System.Data.SQLite;

namespace uiTest
{
    static class Program
    {
        public static bool FirstStart = false;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            SuburbanContext.SearchCity("");
            data.Config cfg = data.Config.Fetch();
            if (cfg == null)
                FirstStart = true;
            else SuburbanContext.SetCity(cfg.City);

            Application.Run(new Form1());
        }

    }
}