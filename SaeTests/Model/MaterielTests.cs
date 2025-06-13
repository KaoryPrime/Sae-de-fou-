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
    public class MaterielTests
    {
        [TestMethod()]
        public void EqualsTest_WithSameProperties_ReturnsTrue()
        {
            Materiel materiel1 = new Materiel(1, 1, 1, "REF01", "Nacelle", "desc", 150.0m, "RAS");
            Materiel materiel2 = new Materiel(1, 1, 1, "REF01", "Nacelle", "desc", 150.0m, "RAS");

            Assert.IsTrue(materiel1.Equals(materiel2));
        }

        [TestMethod()]
        public void EqualsTest_WithDifferentProperties_ReturnsFalse()
        {
            Materiel materiel1 = new Materiel(1, 1, 1, "REF01", "Nacelle", "desc", 150.0m, "RAS");
            Materiel materiel2 = new Materiel(2, 1, 1, "REF02", "Pelle", "desc2", 200.0m, "OK");

            Assert.IsFalse(materiel1.Equals(materiel2));
        }

        [TestMethod()]
        public void GetHashCodeTest_ForEqualObjects_ReturnsSameHashCode()
        {
            Materiel materiel1 = new Materiel(1, 1, 1, "REF01", "Nacelle", "desc", 150.0m, "RAS");
            Materiel materiel2 = new Materiel(1, 1, 1, "REF01", "Nacelle", "desc", 150.0m, "RAS");

            Assert.AreEqual(materiel1.GetHashCode(), materiel2.GetHashCode());
        }

        [TestMethod]
        [DataRow("Nacelle articulée", "pack://application:,,,/img/Nacelle.jpg")]
        [DataRow("Bétonnière 160L", "pack://application:,,,/img/betoniere.jpg")]
        [DataRow("Meuleuse 125mm", "pack://application:,,,/img/meuleuse.jpg")]
        public void ImagePath_KnownMaterialName_ReturnsCorrectPath(string nomMateriel, string expectedPath)
        {
            Materiel materiel = new Materiel();
            materiel.Nommateriel = nomMateriel;

            string actualPath = materiel.ImagePath;

            Assert.AreEqual(expectedPath, actualPath);
        }

        [TestMethod]
        public void ImagePath_UnknownMaterialName_ReturnsDefaultPath()
        {
            Materiel materiel = new Materiel();
            materiel.Nommateriel = "MaterielInconnu";
            string expectedPath = "pack://application:,,,/img/aucun.png";

            string actualPath = materiel.ImagePath;

            Assert.AreEqual(expectedPath, actualPath);
        }

        [TestMethod]
        public void ImagePath_NullMaterialName_ReturnsNull()
        {
            Materiel materiel = new Materiel();
            materiel.Nommateriel = null;

            string actualPath = materiel.ImagePath;

            Assert.IsNull(actualPath);
        }
    }
}