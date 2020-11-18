using DoctorCSharpServer.Controllers.Exceptions;
using DoctorCSharpServer.Model.Items;
using NUnit.Framework;
using System;

namespace Server.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ValidPatientDataValidationTest()
        {
            var name = new string[] { "TestName", "valid", "Patient Zero" };
            var taj = new string[] { "111 111 111", "000 000 000", "123 456 890" };
            var address = new string[] { "TestAddress", "city streeet 10", "ASD LOL u. 28." };
            var phone = new string[] { "adsadsad", "00478217", "08 213 2131" };
            for (int i = 0; i < name.Length; i++)
            {
                //IF every value is valid
                var patient = new SerializedPatient();
                patient.name = name[i];
                patient.TAJ_nr = taj[i];
                patient.address = address[i];
                patient.phone = phone[i];

                //THEN no exception thrown
                Assert.That(() => patient.validate(), Throws.Nothing);
            }
        }

        [Test]
        public void EmptyRequiredPatientDataValidationTest()
        {
            var name = new string[] { "", "valid", "Patient Zero", null, "valid", "Patient Zero", "Patient Zero", "asd" };
            var taj = new string[] { "111 111 111", "000 000 000", "123 456 890", "111 111 111", "000 000 000", "123 456 890", "", null };
            var address = new string[] { "TestAddress", "", "ASD LOL u. 28.", "TestAddress", null, "ASD LOL u. 28.", "TestAddress", "asdsad"};
            var phone = new string[] { "adsadsad", "00478217", "", "adsadsad", "00478217", null, "adsadsad", "00478217"};

            var exceptionCauseValue = new string[] { "name", "address", "phone", "name", "address", "phone", "TAJ_nr", "TAJ_nr" };
            for (int i = 0; i < name.Length; i++)
            {
                // IF there is a missing required property
                var patient = new SerializedPatient();
                patient.name = name[i];
                patient.TAJ_nr = taj[i];
                patient.address = address[i];
                patient.phone = phone[i];

                // THEN InvalidInputException will be thrown
                Assert.That(() => patient.validate(), Throws.Exception.TypeOf<InvalidInputException>()
                                                                        .With.Property("message")
                                                                        .EqualTo("The value of " + exceptionCauseValue[i] + " can not be empty!"));
            }

        }

        [Test]
        public void InvalidTajPatientDataValidationTest()
        {
            var name = new string[] { "name", "valid", "Patient Zero", "data", "valid", "Patient Zero", "Patient Zero"};
            var taj = new string[] { "111111111", "000  000 000", "123 56 890", "11 11 11", "asdsad", "aaa aaa aaa", "213 adn 123" };
            var address = new string[] { "TestAddress", "data", "ASD LOL u. 28.", "TestAddress", "valid", "ASD LOL u. 28.", "TestAddress" };
            var phone = new string[] { "adsadsad", "00478217", "valid", "adsadsad", "00478217", "data", "adsadsad" };
            for (int i = 0; i < name.Length; i++)
            {
                // IF the format of the TAJ is invalid
                var patient = new SerializedPatient();
                patient.name = name[i];
                patient.TAJ_nr = taj[i];
                patient.address = address[i];
                patient.phone = phone[i];

                //THEN InvalidInputException will be thrown with the correct message
                Assert.That(() => patient.validate(), Throws.Exception.TypeOf<InvalidInputException>()
                                                                            .With.Property("message")
                                                                            .EqualTo("The format of the TAJ_nr is not valid!"));
            }

        }
    }
}