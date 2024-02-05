//namespace TestBankProject
//{
//    public class Tests
//    {
//        [SetUp]
//        public void Setup()
//        {
//            // Minden teszteset előtt lefut
//            // Alapállapot beállítására használjuk
//            // Neve nem számít, csak [SetUp] annotációval kell ellátni.
//        }

//        [TearDown]
//        public void TearDown() 
//        { 
//            // Minden teszteset után fut le
//            // Ideiglenes fájlok eltávolítására, adatbázis visszaállítására, stb. használjuk
//        }

//        // Sikeres teszteset
//        [Test]
//        public void Test1()
//        {
//            int osszeg = 2 + 3;
//            Assert.That(osszeg, Is.EqualTo(5));
//        }

//        // Sikertelen teszteset
//        [Test]
//        public void Test2()
//        {
//            int osszeg = 2 + 3;
//            Assert.That(osszeg, Is.EqualTo(6));
//        }

//        // Nincs ellátva [Test] annotációval -> nem teszteset
//        public void Test3()
//        {
//            Assert.Pass();
//        }

//        // Nincs assert -> sikeresen fut le
//        [Test]
//        public void Test4()
//        {
//        }
//    }
//}