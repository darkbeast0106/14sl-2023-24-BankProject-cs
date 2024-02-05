using BankProject;

namespace TestBankProject
{
    internal class BankTest
    {
        Bank bank;

        [SetUp]
        public void SetUp()
        {
            bank = new Bank();
            bank.UjSzamla("Gipsz Jakab", "1234");
        }

        [Test]
        public void UjSzamla_LetrehozottSzamlaEgyenlegNulla()
        {
            Assert.That(bank.Egyenleg("1234"), Is.EqualTo(0));
        }

        [Test]
        public void UjSzamla_NullNevvel() 
        { 
            Assert.Throws<ArgumentNullException>(() => bank.UjSzamla(null, "4321"));
        }

        [Test]
        public void UjSzamla_UresNevvel()
        {
            Assert.Throws<ArgumentException>(() => bank.UjSzamla("", "4321"));
        }

        [Test]
        public void UjSzamla_NullSzamlaszammal()
        {
            Assert.Throws<ArgumentNullException>(() => bank.UjSzamla("Gipsz Jakab", null));
        }

        [Test]
        public void UjSzamla_UresSzamlaszammal()
        {
            Assert.Throws<ArgumentException>(() => bank.UjSzamla("Gipsz Jakab", ""));
        }

        [Test]
        public void UjSzamla_LetezoSzamlaszammal()
        {
            Assert.Throws<ArgumentException>(() => bank.UjSzamla("Teszt Elek", "1234"));
        }

        [Test]
        public void UjSzamla_LetezoNevvel()
        {
            Assert.DoesNotThrow(() => bank.UjSzamla("Gipsz Jakab", "4321"));
        }

        [Test]
        public void Egyenleg_NemletezoSzamlaszammal()
        {
            Assert.Throws<HibasSzamlaszamException>(() => bank.Egyenleg("4321"));
        }

        [Test]
        public void EgyenlegFeltolt_OsszegNulla()
        {
            Assert.Throws<ArgumentException>(() => bank.EgyenlegFeltolt("1234", 0));
        }

        [Test]
        public void EgyenlegFeltolt_EgyenlegMegvaltozik()
        {
            bank.EgyenlegFeltolt("1234", 10000);
            Assert.That(bank.Egyenleg("1234"), Is.EqualTo(10000));
        }

        [Test]
        public void EgyenlegFeltolt_EgyenlegHozzaadodik()
        {
            bank.EgyenlegFeltolt("1234", 10000);
            Assert.That(bank.Egyenleg("1234"), Is.EqualTo(10000));
            bank.EgyenlegFeltolt("1234", 20000);
            Assert.That(bank.Egyenleg("1234"), Is.EqualTo(30000));
        }

        [Test]
        public void EgyenlegFeltolt_JoSzamlaraToltodik()
        {
            bank.UjSzamla("Gipsz Jakab", "4321");
            bank.UjSzamla("Teszt Elek", "5678");
            bank.EgyenlegFeltolt("1234", 10000);
            bank.EgyenlegFeltolt("4321", 50000);
            bank.EgyenlegFeltolt("5678", 20000);
            Assert.That(bank.Egyenleg("1234"), Is.EqualTo(10000));
            Assert.That(bank.Egyenleg("4321"), Is.EqualTo(50000));
            Assert.That(bank.Egyenleg("5678"), Is.EqualTo(20000));
        }

        private void Utal_Setup()
        {
            bank.UjSzamla("Teszt Elek", "5678");
            bank.UjSzamla("Gipsz Jakab", "4321");
            bank.EgyenlegFeltolt("1234", 50000);
            bank.EgyenlegFeltolt("5678", 20000);
            bank.EgyenlegFeltolt("4321", 70000);
            Assert.That(bank.Egyenleg("1234"), Is.EqualTo(50000));
            Assert.That(bank.Egyenleg("5678"), Is.EqualTo(20000));
            Assert.That(bank.Egyenleg("4321"), Is.EqualTo(70000));
        }

        [Test]
        public void Utal_EgyenlegekMegvaltoznak()
        {
            Utal_Setup();
            Assert.True(bank.Utal("1234", "5678", 10000));
            Assert.That(bank.Egyenleg("1234"), Is.EqualTo(40000));
            Assert.That(bank.Egyenleg("5678"), Is.EqualTo(30000));
            Assert.That(bank.Egyenleg("4321"), Is.EqualTo(70000));
        }

        [Test]
        public void Utal_ForrasNemLetezik()
        {
            Utal_Setup();
            Assert.Throws<HibasSzamlaszamException>(() => bank.Utal("9876", "5678", 10000));
        }

        [Test]
        public void Utal_CelNemLetezik()
        {
            Utal_Setup();
            Assert.Throws<HibasSzamlaszamException>(() => bank.Utal("1234", "9876", 10000));
        }

        [Test]
        public void Utal_EgyenlegNullaLesz() 
        { 
            Utal_Setup();
            Assert.True(bank.Utal("1234", "5678", 50000));
            Assert.That(bank.Egyenleg("1234"), Is.EqualTo(0));
            Assert.That(bank.Egyenleg("5678"), Is.EqualTo(70000));
            Assert.That(bank.Egyenleg("4321"), Is.EqualTo(70000));
        }

        [Test]
        public void Utal_NagyobbOsszegMintAmiASzamlanVan()
        {
            Utal_Setup();
            Assert.False(bank.Utal("1234", "5678", 50001));
            Assert.That(bank.Egyenleg("1234"), Is.EqualTo(50000));
            Assert.That(bank.Egyenleg("5678"), Is.EqualTo(20000));
            Assert.That(bank.Egyenleg("4321"), Is.EqualTo(70000));
        }
    }
}
