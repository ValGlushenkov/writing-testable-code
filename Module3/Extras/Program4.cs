using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using Ninject.Extensions.Conventions;
using TestableCodeDemos.Module3.Easy;

namespace TestableCodeDemos.Module3.Extras
{
    public class Program4
    {
        //Using Ninject to bind all classes in the assembly to default interfaces
        static void Main(string[] args)
        {
            var container = new StandardKernel();
            //Classees and Interfaces must share the same name excluding "I" prefix.
            
            container.Bind(p =>
            {
                p.FromThisAssembly()
                    .SelectAllClasses()
                    .BindDefaultInterface();
            });

            var invoiceId = int.Parse(args[0]);

            var command = container.Get<PrintInvoiceCommand>();

            command.Execute(invoiceId);
        }
    }
}
