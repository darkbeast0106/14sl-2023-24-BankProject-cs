namespace BankProject
{
    /// <summary>
    /// Bank műveleteit végrehajtó osztály.
    /// </summary>
    public class Bank
    {
        private List<Szamla> szamlak = new List<Szamla>();
        /// <summary>
        /// Új számlát nyit a megadott névvel, számlaszámmal, 0 Ft egyenleggel
        /// </summary>
        /// <param name="nev">A számla tulajdonosának neve</param>
        /// <param name="szamlaszam">A számla számlaszáma</param>
        /// <exception cref="ArgumentNullException">A név és a számlaszám nem lehet null</exception>
        /// <exception cref="ArgumentException">A név és a számlaszám nem lehet üres
        /// A számlaszámmal nem létezhet számla
        /// A számlaszám számot, szóközt és kötőjelet tartalmazhat</exception>
        public void UjSzamla(string nev, string szamlaszam)
        {
            if (nev == null)
            {
                throw new ArgumentNullException(nameof(nev));
            }
            if (nev == "")
            {
                throw new ArgumentException("A név nem lehet üres", nameof(nev));
            }
            if (szamlaszam == null)
            {
                throw new ArgumentNullException (nameof(szamlaszam));
            }
            if (szamlaszam == "")
            {
                throw new ArgumentException("A számlaszám nem lehet üres", nameof(szamlaszam));
            }
            int index = 0;
            while (index < szamlak.Count && szamlak[index].Szamlaszam != szamlaszam)
            {
                index++;
            }
            if (index < szamlak.Count)
            {
                throw new ArgumentException("A számlaszámmal már létezik számla", nameof(szamlaszam));
            }

            szamlak.Add(new Szamla(nev, szamlaszam));

            //try
            //{
            //    SzamlaKeres(szamlaszam);
            //    throw new ArgumentException("A számlaszámmal már létezik számla", nameof(szamlaszam));
            //}
            //catch (HibasSzamlaszamException)
            //{
            //    szamlak.Add(new Szamla(nev, szamlaszam));
            //}
        }

        /// <summary>
        /// Lekérdezi az adott számlán lévő pénzösszeget
        /// </summary>
        /// <param name="szamlaszam">A számla számlaszáma, aminek az egyenlegét keressük</param>
        /// <returns>A számlán lévő egyenleg</returns>
        /// <exception cref="ArgumentNullException">A számlaszám nem lehet null</exception>
        /// <exception cref="ArgumentException">A számlaszám számot, szóközt és kötőjelet tartalmazhat</exception>
        /// <exception cref="HibasSzamlaszamException">A megadott számlaszámmal nem létezik számla</exception>
        public ulong Egyenleg(string szamlaszam)
        {
            Szamla szamla = SzamlaKeres(szamlaszam);
            return szamla.Egyenleg;
        }

        private Szamla SzamlaKeres(string szamlaszam)
        {
            if (szamlaszam == null)
            {
                throw new ArgumentNullException(nameof(szamlaszam));
            }
            if (szamlaszam == "")
            {
                throw new ArgumentException("A számlaszám nem lehet üres", nameof(szamlaszam));
            }

            int index = 0;
            while (index < szamlak.Count && szamlak[index].Szamlaszam != szamlaszam)
            {
                index++;
            }
            if (index == szamlak.Count)
            {
                throw new HibasSzamlaszamException(szamlaszam);
            }
            return szamlak[index];
        }

        /// <summary>
        /// Egy létező számlára pénzt helyez
        /// </summary>
        /// <param name="szamlaszam">A számla számlaszáma, amire pénzt helyez</param>
        /// <param name="osszeg">A számlára helyezendő pénzösszeg</param>
        /// <exception cref="ArgumentNullException">A számlaszám nem lehet null</exception>
        /// <exception cref="ArgumentException">Az összeg csak pozitív lehet.
        /// A számlaszám számot, szóközt és kötőjelet tartalmazhat</exception>
        /// <exception cref="HibasSzamlaszamException">A megadott számlaszámmal nem létezik számla</exception>
        public void EgyenlegFeltolt(string szamlaszam, ulong osszeg)
        {
            if (osszeg == 0)
            {
                throw new ArgumentException("Az összeg nem lehet 0", nameof(osszeg));
            }
            Szamla szamla = SzamlaKeres(szamlaszam);
            szamla.Egyenleg += osszeg;
        }

        /// <summary>
        /// Két számla között utal.
        /// Ha nincs elég pénz a forrás számlán, akkor false értékkel tér vissza
        /// </summary>
        /// <param name="honnan">A forrás számla számlaszáma</param>
        /// <param name="hova">A cél számla számlaszáma</param>
        /// <param name="osszeg">Az átutalandó egyenleg</param>
        /// <returns>Az utalás sikeressége</returns>
        /// <exception cref="ArgumentNullException">A forrás és cél számlaszám nem lehet null</exception>
        /// <exception cref="ArgumentException">Az összeg csak pozitív lehet.
        /// A számlaszám számot, szóközt és kötőjelet tartalmazhat</exception>
        /// <exception cref="HibasSzamlaszamException">A megadott számlaszámmal nem létezik számla</exception>
        public bool Utal(string honnan, string hova, ulong osszeg)
        {
            if (osszeg == 0)
            {
                throw new ArgumentException("Az összeg nem lehet 0", nameof(osszeg));
            }
            Szamla forrasSzamla = SzamlaKeres(honnan);
            Szamla celSzamla = SzamlaKeres(hova);

            bool sikeres = false;

            if (forrasSzamla.Egyenleg >= osszeg)
            {
                sikeres = true;
                forrasSzamla.Egyenleg -= osszeg;
                celSzamla.Egyenleg += osszeg;
            }
            return sikeres;
        }

        private class Szamla
        {
            string tulajdonos;
            string szamlaszam;
            ulong egyenleg;

            public Szamla(string tulajdonos, string szamlaszam)
            {
                this.tulajdonos = tulajdonos;
                this.szamlaszam = szamlaszam;
                this.egyenleg = 0;
            }

            public string Tulajdonos { get => tulajdonos; set => tulajdonos = value; }
            public string Szamlaszam { get => szamlaszam; set => szamlaszam = value; }
            public ulong Egyenleg { get => egyenleg; set => egyenleg = value; }
        }
    }
}