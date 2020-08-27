﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMoq;
using Moq;
using NUnit.Framework;
using TestableCodeDemos.Module6.Shared;

namespace TestableCodeDemos.Module6.Easy
{
    [TestFixture]
    public class EmailInvoiceCommandTests
    {
        private EmailInvoiceCommand _command;
        private AutoMoqer _mocker;
        private Invoice _invoice;

        private const int InvoiceId = 1;
        private const string EmailAddress = "email@test.com";

        [SetUp]
        public void SetUp()
        {
            _invoice = new Invoice() 
            {
                EmailAddress = EmailAddress
            };

            _mocker = new AutoMoqer();

            _mocker.GetMock<IDatabase>()
                .Setup(p => p.GetInvoice(InvoiceId))
                .Returns(_invoice);

            _command = _mocker.Create<EmailInvoiceCommand>();
        }

        [Test]
        public void TestExecuteForInvoiceWithNoEmailAddressShouldThrowException()
        {
            Assert.That(() => _command.Execute(InvoiceId),
                Throws.TypeOf<EmailAddressIsBlankException>());
        }

        [Test]
        public void TestExecuteShouldEmailInvoice()
        {
            _command.Execute(InvoiceId);

            _mocker.GetMock<IInvoiceEmailer>()
                .Verify(p => p.Email(_invoice),
                Times.Once);
        }
    }
}
