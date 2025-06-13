using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sae.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sae.Model.Tests
{
    [TestClass()]
    public class EmployeTests
    {
        [TestMethod()]
        public void EqualsTest_WithSameProperties_ReturnsTrue()
        {
            Employe employe1 = new Employe(1, 2, "Martin", "Paul", "pmartin", "pass123");
            Employe employe2 = new Employe(1, 2, "Martin", "Paul", "pmartin", "pass123");

            Assert.IsTrue(employe1.Equals(employe2));
        }

        [TestMethod()]
        public void EqualsTest_WithDifferentId_ReturnsFalse()
        {
            Employe employe1 = new Employe(1, 2, "Martin", "Paul", "pmartin", "pass123");
            Employe employe2 = new Employe(99, 2, "Martin", "Paul", "pmartin", "pass123");

            Assert.IsFalse(employe1.Equals(employe2));
        }

        [TestMethod()]
        public void EqualsTest_WithDifferentLogin_ReturnsFalse()
        {
            Employe employe1 = new Employe(1, 2, "Martin", "Paul", "pmartin", "pass123");
            Employe employe2 = new Employe(1, 2, "Martin", "Paul", "ppaul", "pass123");

            Assert.IsFalse(employe1.Equals(employe2));
        }

        [TestMethod()]
        public void GetHashCodeTest_ForEqualObjects_ReturnsSameHashCode()
        {
            Employe employe1 = new Employe(1, 2, "Martin", "Paul", "pmartin", "pass123");
            Employe employe2 = new Employe(1, 2, "Martin", "Paul", "pmartin", "pass123");

            Assert.AreEqual(employe1.GetHashCode(), employe2.GetHashCode());
        }

        [TestMethod]
        public void Validation_WithValidInputs_ReturnsTrue()
        {
            bool result = AreLoginInputsValid("pmartin", "pass123");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Validation_WithEmptyLogin_ReturnsFalse()
        {
            bool result = AreLoginInputsValid("", "pass123");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Validation_WithNullLogin_ReturnsFalse()
        {
            bool result = AreLoginInputsValid(null, "pass123");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Validation_WithEmptyPassword_ReturnsFalse()
        {
            bool result = AreLoginInputsValid("pmartin", "");
            Assert.IsFalse(result);
        }

        private bool AreLoginInputsValid(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                return false;
            }
            return true;
        }
    }
}