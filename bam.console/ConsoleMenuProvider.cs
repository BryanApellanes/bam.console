﻿using Bam.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Console
{
    public class ConsoleMenuProvider : MenuProvider<ConsoleCommandAttribute>
    {
        public ConsoleMenuProvider(IMenuItemProvider menuItemProvider, IMenuItemSelector menuItemSelector, IMenuItemRunner menuItemRunner) : base(menuItemProvider, menuItemSelector, menuItemRunner)
        {
        }

        public override IMenu GetMenu(Type type)
        {
            return GetMenu<ConsoleCommandAttribute>(type);
        }
    }
}
