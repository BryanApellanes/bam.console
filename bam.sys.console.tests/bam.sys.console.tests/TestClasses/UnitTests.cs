﻿using Bam.Net.CommandLine;
using Bam.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys.Console.Tests.TestClasses
{
    [Menu<TestAttribute>("Tests")]
    public class UnitTests
    {
        [Test(TestKind.UnitTest)]
        public void FakeTestForTesting()
        {
            Message.PrintLine("it worked", ConsoleColor.Green);
        }
    }
}
