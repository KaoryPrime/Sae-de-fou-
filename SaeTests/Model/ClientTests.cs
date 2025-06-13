using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sae.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sae.Model.Tests
{
    [TestClass()]
    public class ClientTests
    {
        [TestMethod()]
        public void ToStringTest_NominalCase_ReturnsFormattedString()
        {
            Client client = new Client { Nomclient = "Dupont", Prenomclient = "Jean" };
            string expected = "DUPONT Jean";

            string actual = client.ToString();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void EqualsTest_SameId_ReturnsTrue()
        {
            Client client1 = new Client { Numclient = 1, Nomclient = "Dupont" };
            Client client2 = new Client { Numclient = 1, Nomclient = "Martin" };

            Assert.IsTrue(client1.Equals(client2));
        }

        [TestMethod()]
        public void EqualsTest_DifferentId_ReturnsFalse()
        {
            Client client1 = new Client { Numclient = 1 };
            Client client2 = new Client { Numclient = 2 };

            Assert.IsFalse(client1.Equals(client2));
        }

        [TestMethod]
        public void Validation_WithValidClient_ReturnsTrue()
        {
            Client client = new Client(1, "Durand", "Pierre", "pierre.durand@email.com", "0612345678");

            bool result = IsClientDataValid(client);

            Assert.IsTrue(result);
        }

        [TestMethod]
        [DataRow("test@test.com", true)]
        [DataRow("test.test@test.co.uk", true)]
        [DataRow("test-test@test.fr", true)]
        [DataRow("test@test", false)]
        [DataRow("test.com", false)]
        [DataRow("@test.com", false)]
        public void Validation_EmailFormat_ReturnsExpected(string email, bool expectedResult)
        {
            Client client = new Client(1, "Nom", "Prénom", email, "0612345678");

            bool actual = IsClientDataValid(client);

            Assert.AreEqual(expectedResult, actual);
        }

        [TestMethod]
        [DataRow("0612345678", true)]
        [DataRow("+33612345678", true)]
        [DataRow("0123456789", true)]
        [DataRow("06-12-34-56-78", false)]
        [DataRow("612345678", false)]
        [DataRow("abcdefghij", false)]
        public void Validation_TelephoneFormat_ReturnsExpected(string telephone, bool expectedResult)
        {
            Client client = new Client(1, "Nom", "Prénom", "test@test.com", telephone);

            bool actual = IsClientDataValid(client);

            Assert.AreEqual(expectedResult, actual);
        }

        [TestMethod]
        public void Validation_WithMissingName_ReturnsFalse()
        {
            Client client = new Client(1, " ", "Prénom", "test@test.com", "0612345678");

            bool result = IsClientDataValid(client);

            Assert.IsFalse(result);
        }

        private bool IsClientDataValid(Client client)
        {
            if (client == null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(client.Nomclient) ||
                string.IsNullOrWhiteSpace(client.Prenomclient) ||
                string.IsNullOrWhiteSpace(client.Mailclient) ||
                string.IsNullOrWhiteSpace(client.Telclient))
            {
                return false;
            }

            if (!IsEmailFormatValid(client.Mailclient))
            {
                return false;
            }

            if (!IsTelephoneFormatValid(client.Telclient))
            {
                return false;
            }

            return true;
        }

        private bool IsEmailFormatValid(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private bool IsTelephoneFormatValid(string telephone)
        {
            return Regex.IsMatch(telephone, @"^(\+33|0)[1-9](\d{2}){4}$");
        }
    }
}