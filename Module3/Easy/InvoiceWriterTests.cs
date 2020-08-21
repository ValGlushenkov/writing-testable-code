﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMoq;
using Moq;
using NUnit.Framework;
using TestableCodeDemos.Module3.Shared;

namespace TestableCodeDemos.Module3.Easy
{
    [TestFixture]
    public class InvoiceWriterTests
    {
        private InvoiceWriter _writer;
        private AutoMoqer _mocker;
        private Invoice _invoice;

        [SetUp]
        public void SetUp()
        {
            _invoice = new Invoice()
            { 
                Id = 1,
                IsOverdue = false
            };

            _mocker = new AutoMoqer();

            _writer = _mocker.Create<InvoiceWriter>();
        }

        [Test]
        public void TestWriteShouldSetPageLayout()
        {
            _writer.Write(_invoice);

            var layout = _mocker
                .GetMock<IPageLayout>().Object;

            _mocker.GetMock<IPrinter>()
                .Verify(p => p.SetPageLayout(layout),
                Times.Once);
        }

        [Test]
        public void TestWriteShouldPrintOverdueInvoiceRed()
        {
            _invoice.IsOverdue = true;

            _writer.Write(_invoice);

            _mocker.GetMock<IPrinter>()
                .Verify(p => p.SetInkColor("Red"),
                Times.Once);
        }

        [Test]
        public void TestWriteShouldPrintOnTimeInvoiceInDefaultColor()
        {
            _writer.Write(_invoice);

            _mocker.GetMock<IPrinter>()
                .Verify(p => p.SetInkColor(It.IsAny<string>()), 
                Times.Never);
        }

        [Test]   
        [TestCase("Invoice ID: 1")]
        public void TestWriteShouldPrintInvoiceNumber(string line)
        {
            _writer.Write(_invoice);

            _mocker.GetMock<IPrinter>()
                .Verify(p => p.WriteLine(line), 
                Times.Once);
        }
    }
}
