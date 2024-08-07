﻿using Bam;
using Bam.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Console
{
    public class ConsoleMenuItemRunResultRenderer : IMenuItemRunResultRenderer
    {
        public Action<IMenuItemRunResult> ItemRunSucceeded { get; set; } = (menuItemRunResult) => Message.PrintLine("{0} succeeded", ConsoleColor.Green, menuItemRunResult?.MenuItem?.DisplayName ?? "run");

        public Action<IMenuItemRunResult> ItemRunFailed { get; set; } = (menuItemRunResult) => Message.PrintLine("{0} failed", ConsoleColor.Green, menuItemRunResult?.MenuItem?.DisplayName ?? "run");

        public Action<IMenuItemRunResult> ItemRunException { get; set; } = (menuItemRunResult) => Message.PrintLine(menuItemRunResult?.Exception?.GetMessageAndStackTrace() ?? "Stacktrace unavailable", ConsoleColor.Magenta);

        public void RenderMenuItemRunResult(IMenuItemRunResult menuItemRunResult)
        {
            if (menuItemRunResult != null)
            {
                if (menuItemRunResult.Success)
                {
                    ItemRunSucceeded(menuItemRunResult);
                }
                else
                {
                    if (menuItemRunResult.MenuItem != null && !string.IsNullOrEmpty(menuItemRunResult.MenuItem.DisplayName))
                    {
                        ItemRunFailed(menuItemRunResult);
                    }

                    if (menuItemRunResult.Exception != null && !string.IsNullOrEmpty(menuItemRunResult.Exception.StackTrace))
                    {
                        ItemRunException(menuItemRunResult);
                    }
                }
            }
        }
    }
}
