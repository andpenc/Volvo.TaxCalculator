using System;
using System.Collections.Generic;
using NUnit.Framework;
using Volvo.TaxCalculator.Core.Interfaces;
using Volvo.TaxCalculator.Core.Models;

namespace Volvo.TaxCalculator.Core.Tests.ServiceTests
{
    public class CongestionTaxServiceTestCases
    {
        private ICongestionTaxService _service;

        private static readonly IEnumerable<TestCaseData> oneEntryTestCases = new[]
        {
            new TestCaseData(new DateTime(2013,01,14,21,00,00), 0).SetName("One entry, 2013, 14 of January, 21:00 - Expected fee amount: 0"),
            new TestCaseData(new DateTime(2013,01,15,21,00,00), 0).SetName("One entry, 2013, 15 of January, 21:00 - Expected fee amount: 0"),
            new TestCaseData(new DateTime(2013,03,26,14,25,00), 8).SetName("One entry, 2013, 26 of March, 14:25:00 - Expected fee amount: 8"),
            new TestCaseData(new DateTime(2013,03,28,14,07,27), 0).SetName("One entry, 2013, 28 of March, 14:07:27 - Expected fee amount: 0"),
            new TestCaseData(new DateTime(2013,03,29,15,34,00), 0).SetName("One entry, 2013, 29 of March, 15:34:00 - Expected fee amount: 0"),
            new TestCaseData(new DateTime(2013,03,29,17,19,13), 0).SetName("One entry, 2013, 29 of March, 17:19:13 - Expected fee amount: 0"),
            new TestCaseData(new DateTime(2013,07,15,15,21,42), 0).SetName("One entry, 2013, 15 of July, 15:21:42 - Expected fee amount: 0")
        };

        private static readonly IEnumerable<TestCaseData> multipleEntriesTestCases = new[]
        {
            new TestCaseData(new DateTime[]
            {
                new DateTime(2013,02,07,06,23,27),
                new DateTime(2013,02,07,15,27,00)
            }
            , 21)
            .SetName("Multiple Entries, 2013, 7 of February, 2 entries - Expected fee amount: 21"),

            new TestCaseData(new DateTime[]
            {
                new DateTime(2013,02,08,06,27,00),
                new DateTime(2013,02,08,06,20,27),
                new DateTime(2013,02,08,14,35,00),
                new DateTime(2013,02,08,15,29,00),
                new DateTime(2013,02,08,15,47,00),
                new DateTime(2013,02,08,16,01,00),
                new DateTime(2013,02,08,16,48,00),
                new DateTime(2013,02,08,17,49,00),
                new DateTime(2013,02,08,18,29,00),
                new DateTime(2013,02,08,18,35,00)
            }
            , 60)
            .SetName("Multiple Entries, 2013, 8 of February, 10 entries - Expected fee amount: 60")
        };

        [OneTimeSetUp]
        public void TestSetup()
        {
            _service = new CongestionTaxService();
        }

        [Test]
        [TestCaseSource(nameof(oneEntryTestCases))]
        public void OneEntryTestCases(DateTime dto, int expectedFee)
        {
            var calculatedFee = _service.GetTax(new DateTime[] { dto }, new Vehicle() { VehicleType = "Car" });
            Assert.AreEqual(expectedFee, calculatedFee);
        }

        [Test]
        [TestCaseSource(nameof(multipleEntriesTestCases))]
        public void MultipleEntriesTestCases(DateTime[] dtos, int expectedFee)
        {
            var calculatedFee = _service.GetTax(dtos, new Vehicle() { VehicleType = "Car" });
            Assert.AreEqual(expectedFee, calculatedFee);
        }
    }
}