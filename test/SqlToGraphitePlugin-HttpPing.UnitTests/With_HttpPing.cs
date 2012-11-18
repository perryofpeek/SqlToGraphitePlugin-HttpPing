using log4net;
using NUnit.Framework;
using Rhino.Mocks;
using SqlToGraphiteInterfaces;

namespace SqlToGraphitePlugin_HttpPing.UnitTests
{
    using System;

    [TestFixture]
    // ReSharper disable InconsistentNaming
    public class With_HttpPing
    {
        [Test]
        public void Should_Get_value_from_BBC_News()
        {
            var log = MockRepository.GenerateMock<ILog>();
            var encryption = MockRepository.GenerateMock<IEncryption>();
            Job job = new HttpPing();
            var httpPing = new HttpPing(log, job, encryption);
            httpPing.Uri = "http://www.bbc.co.uk/news";
            //Test
            var result = httpPing.Get();
            //Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Value, Is.GreaterThanOrEqualTo(10));
            Console.WriteLine(result[0].Value);
        }
    }
}
