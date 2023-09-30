﻿using Bam.Net.CommandLine;
using Bam.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys.Console
{
    public class ConsoleMenuHeaderRenderer : IMenuHeaderRenderer
    {
        public void RenderMenuHeader(IMenu menu)
        {
            Message.PrintLine(menu.DisplayName);
            Message.PrintLine();
            Message.PrintLine(menu.HeaderText);
        }
    }
}