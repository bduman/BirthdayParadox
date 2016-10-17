using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace ConsoleApplication1
{
    class Birthday
    {
        private int year, month, day;
        private string city;

        public Birthday(int day,int month,int year,string city)
        {
            this.setCity(city);
            this.setDay(day);
            this.setMonth(month);
            this.setYear(year);
        }

        public void setCity(string city)
        {
            this.city = city;
        }
        public string getCity()
        {
            return this.city;
        }
        public void setYear(int year)
        {
            this.year = year;
        }
        public int getYear()
        {
            return this.year;
        }
        public void setMonth(int month)
        {
            this.month = month;
        }
        public int getMonth()
        {
            return this.month;
        }
        public void setDay(int day)
        {
            this.day = day;
        }
        public int getDay()
        {
            return this.day;
        }

    }

    class Paradox
    {

        private int[] nDeğerleri = { 100, 250, 1000 };
        private int[] aylarinGunleri = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        private string[] aylarinAdlari = { "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık" };
        private string[] sehirlerinAdlari = { "Paris", "Roma", "New York" };

        static Random rand = new Random();
        
        // b seçeneği 
        private int[][] cakismalar = new int[12][];
        private int n;

        // c ve d seçeneği
        // ilk index = şehir, ikinci index = ay, üçüncü index = gün
        private int[,][] sehirBazliCakismalar = new int[3, 12][];
        private int[,] deneySonuclari;

        public Paradox(int n)
        {
            this.n = n;
            Birthday doğum;

            this.generateCakismalarDizisi();

            for (int i = 0; i < this.n; i++)
            {
                doğum = this.randomBirthday();
                this.cakismalar[doğum.getMonth()][doğum.getDay()]++;
            }

            this.cakismaTakvimiBas();
        }

        public Paradox(bool sehir)
        {
            Birthday randBirthday;
            // ilk index => n. deney
            // ikinci index => nDeğerlerinin temsili alanı
            this.deneySonuclari = new int[10, this.nDeğerleri.Length];
            // 2 boyutlu dizinin tüm elemanlarına sıfır atar
            Array.Clear(deneySonuclari, 0, deneySonuclari.Length);


            // nDeğerleri dizisinin elemanı kadar dönecek
            for(int i=0;i < this.nDeğerleri.Length;i++) 
            {
                // 10 deneylik döngü
                for(int j=0;j<10;j++) 
                {
                    Console.WriteLine("\n n={0} | {1}. Deney Sonuçları | Takvim",this.nDeğerleri[i],j+1);          
                    // sehirBazliCakismalar dizisini sıfırlıcaz döngü tekrar başlayacak
                    this.generateSehirBazliCakismalar();


                    // 1 deneydeki n sayısı kadar kişi oluşturma döngüsü
                    for (int k=0;k<nDeğerleri[i];k++)
                    {
                        randBirthday = this.randomBirthday();
                        this.sehirBazliCakismalar[this.getKeyFromCity(randBirthday.getCity()), randBirthday.getMonth()][randBirthday.getDay()]++;
                    }

                    // sehirBazliCakismalar dizisini buraya bastırıcaz - devam et butonu olacak                        
                    if (sehir == false)
                        this.sehirsizTakvimBas();
                    else
                        this.sehirliTakvimBas();
                    
                    Console.WriteLine("\n\nDevam etmek için herangi bir tuşa basın");
                    Console.ReadKey();
                    Console.Clear();
                    
                    // sehirBazliCakismalar dizisindeki sayıları deneySonuclari na ekleme yapicaz
                    if (sehir == false)
                        this.setCakismalarSehirsiz(j, i);
                    else
                        this.setCakismalarSehirli(j, i);
                                        
                }

            }

            // deneySonuclari değişkeninde bulunan istatistiksel bilgilerini olduğu 11x3 lük tablo basılacak
            if (sehir == true)
                this.istatikselTabloBas(true);
            else
                this.istatikselTabloBas();
            
        }

        private void setCakismalarSehirsiz(int deneyNumarasi, int n)
        {
            int toplam;
            // aylari temsil eder
            for(int j=0;j<12;j++)
            {
                // günleri temsil eder
                for(int k=0;k < this.aylarinGunleri[j];k++)
                {
                    toplam = 0;
                    // şehirleri temsil eder 
                    for(int i=0;i<this.sehirlerinAdlari.Length;i++)
                    {
                        // toplam kişi sayısını elde ederiz
                        if(this.sehirBazliCakismalar[i, j][k] > -1)
                            toplam += (this.sehirBazliCakismalar[i, j][k] + 1);
                    }

                    // toplam çakışma sayısını deneySonuclari dizisine atarız
                    if(toplam > 1)
                        this.deneySonuclari[deneyNumarasi, n] += (toplam-1);
                }
            }

        }

        private void setCakismalarSehirli(int deneyNumarasi, int n)
        {
            // şehirlerin de çakışması gerektiği için 
            // her şehrin çakışmasını kendi içinde değerleniriyoruz
            
            // şehirleri temsil eder
            for(int i=0;i<this.sehirlerinAdlari.Length;i++)
            {
                // ayları temsil eder
                for(int j=0;j<12;j++)
                {
                    // günleri temsil eder
                    for (int k = 0; k < this.aylarinGunleri[j]; k++)
                    {
                        // aynı şehir aynı ay ve aynı gün doğanların,
                        // çakışma sayılarını direkt olarak deneySonuclari dizisine aktarıyoruz
                        if (this.sehirBazliCakismalar[i, j][k] > 0)
                        {
                            this.deneySonuclari[deneyNumarasi, n] += this.sehirBazliCakismalar[i, j][k];
                        }
                    }
                }
            }
        }

        private int getKeyFromCity(string currentCity)
        {
            for (int i = 0; i < this.sehirlerinAdlari.Length; i++)
                if (sehirlerinAdlari[i] == currentCity)
                    return i;

            return 0;
        }

        private void generateCakismalarDizisi()
        {
            // aylari temsil eder
            for (int i = 0; i < 12; i++)
            {
                this.cakismalar[i] = new int[this.aylarinGunleri[i]];
                for (int j = 0; j < this.aylarinGunleri[i]; j++)
                    this.cakismalar[i][j] = -1; // -1 => 0 çakışmaya denk gelmektedir !
            }
        }

        private void generateSehirBazliCakismalar()
        {
            // şehirleri temsil eder
            for(int i=0;i < 3; i++)
            {
                // ayları temsil eder
                for(int j=0;j<12; j++)
                {
                    this.sehirBazliCakismalar[i,j] = new int[this.aylarinGunleri[j]];
                    // günleri temsil eder
                    for (int k=0;k < this.aylarinGunleri[j]; k++)
                    {
                        this.sehirBazliCakismalar[i, j][k] = -1; // -1 => 0 çakışmaya denk gelmektedir !
                    }
                }
            }
        }

        private Birthday randomBirthday()
        {
            int rastgeleAy = rand.Next(0,12);
            int rastgeleGun = rand.Next(0, this.aylarinGunleri[rastgeleAy]);
            int rastgeleSehir = rand.Next(0, 3);
            int rastgeleYil = rand.Next(1970, 2015);

            return new Birthday(rastgeleGun, rastgeleAy, rastgeleYil, this.sehirlerinAdlari[rastgeleSehir]);
        }

        // b seçeneğini bastırmak için kullandık
        private void cakismaTakvimiBas()
        {
            
            Console.WriteLine("\n\n{0,5}{1,5}{2,5}{3,5}{4,5}{5,5}", "T","A","K","V","İ","M");
            Console.WriteLine("    {0}","--------------------------");
            for (int i=0;i<this.cakismalar.GetLength(0);i++)
            {
                Console.WriteLine("\n   " + aylarinAdlari[i]);
                Console.WriteLine("   ---------");
                for (int j=0;j < this.cakismalar[i].Length; j++)
                { 

                    if (cakismalar[i][j] == -1 || cakismalar[i][j] == 0)
                        Console.Write("{0} ");
                    else
                        Console.Write("{" + (cakismalar[i][j] - 1) + "} ");
                    // console sınırları için
                    if (j == 14)
                        Console.Write("\n");
                        
                }
            }
        }
        // c seçeneği bastırmak için kullandık
        private void sehirsizTakvimBas()
        {

            int toplam;
            for (int j = 0; j < 12; j++)
            {
                Console.WriteLine("\n\n   " + aylarinAdlari[j]);
                Console.WriteLine("   ---------");
                for (int k = 0; k < this.aylarinGunleri[j]; k++)
                {

                    // şehirlerin çakışmalarını toplar
                    toplam = 0;

                    for (int i = 0; i < 3; i++)
                        if(sehirBazliCakismalar[i, j][k] != -1)
                            toplam += (sehirBazliCakismalar[i, j][k]+1);

                    // günlere göre çakışma basar
                    if(toplam == 0)
                        Console.Write("{0} ");
                    else
                        Console.Write("{" + (toplam-1) + "} ");

                    // console sınırları için
                    if (k == 14)
                        Console.Write("\n");
                }
            }

        }

        // d seçeneği bastırmak için kullandık
        private void sehirliTakvimBas()
        {
            // şehirleri temsil eder
            for(int i=0;i<3;i++)
            {
                Console.WriteLine("\n\n  [" + sehirlerinAdlari[i] + "]");
                Console.WriteLine(" ___________");
                // ayları temsil eder
                for (int j=0;j<12;j++)
                {
                    Console.WriteLine("\n\n   " + aylarinAdlari[j]);
                    Console.WriteLine("   ---------");

                    // günleri temsil eder
                    for (int k=0;k < this.aylarinGunleri[j]; k++)
                    {
                        // günlere göre çakışma basar
                        if (sehirBazliCakismalar[i, j][k] == -1 || sehirBazliCakismalar[i, j][k] == 0)
                            Console.Write("{0} ");
                        else
                            Console.Write("{" + sehirBazliCakismalar[i, j][k] + "} ");

                        // console sınırları için
                        if (k == 14)
                            Console.Write("\n");
                    }
                }
            }

        }

        private void istatikselTabloBas(bool sehir = false)
        {
            double[] toplamlar = new double[deneySonuclari.GetLength(1)];
            Array.Clear(toplamlar, 0, toplamlar.Length);

            Console.WriteLine("\n{0,10}", (sehir == true) ? "[Şehir Çakışmaları Dahil]" : "[Şehir Çakışmaları Dahil Değil]");
            Console.WriteLine("\n{0,10} {1,8} {2,6} {3,7}", "Deney No", "n=100", "n=250", "n=1000");
            Console.WriteLine("{0,10} {1,9} {2,6} {3,6}", "--------", "------", "------", "------");
            for (int i = 0; i < deneySonuclari.GetLength(0); i++)
            {
                Console.Write("\n{0,5}   ",i+1);
                for(int j=0;j<deneySonuclari.GetLength(1);j++)
                {
                    Console.Write("{0,8}",deneySonuclari[i,j]);
                    toplamlar[j] += deneySonuclari[i, j];
                }
            }
            Console.WriteLine("\n{0,9} {1,9} {2,6} {3,6}", "-------", "------", "------", "------");
            Console.WriteLine("{0,10} {1,6} {2,7}  {3,6}", "Ortalama", toplamlar[0]/10, toplamlar[1]/10, toplamlar[2]/10);
        }


    }

    class Program
    {

        static public int sayiAl(string mesaj, int altSinir, int ustSinir)
        {
            int sayi;
            string line;
            do
            {
                Console.Write(mesaj);
                line = Console.ReadLine();
                sayi = (int.TryParse(line, out sayi)) ? Convert.ToInt32(sayi) : 0;

            } while (sayi < altSinir || sayi > ustSinir);

            return sayi;
        }

        static void menuGoster()
        {
            Console.WriteLine("\n > Birthday Paradox Simulator <\n");

            Console.WriteLine("[1] Gireceğiniz n değerindeki sayıya göre bir deney gerçekleştirir. Takvim basar.\n");
            Console.WriteLine("[2] n=100,250,1000 sırasıyla n değerleri için 10ar deney yapar. Takvim ve istatistik basar.\n");
            Console.WriteLine("[3] n=100,250,1000 sırasıyla n değerleri için 10ar deney yapar. Şehir filtreli takvim ve istatistik basar.\n");
            Console.WriteLine("[4] Programı sonlandır.\n");
            Console.WriteLine("[Program belli aşamalarda console temizliği yapar]\n");
        }

        static int menuSec()
        {
            menuGoster();
            return sayiAl("Menüden işlemini yapmak istediğiniz sayıyı giriniz:", 1, 4);
        }

        static void menuyeDonmek()
        {
            Console.WriteLine("\n\nMenüye dönmek için herangi bir tuşa basın.");
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            // b seçeneği için new Paradox(n); int n kullanıcıdan alınan n değeri
            // c seçeneği için new Paradox(false); false -> şehirler dahil değil
            // d seçeneği için new Paradox(true); true -> şehirler dahil
            int secim;
            
            do
            {
                Console.Clear();
                secim = menuSec();
                Paradox paradox;

                switch (secim)
                {
                    case 1:
                        int n = sayiAl("Deneyin uygulanacağı kişi sayısını girin:",1, Int32.MaxValue);
                        paradox = new Paradox(n);
                        menuyeDonmek();
                        break;
                    case 2:
                        paradox = new Paradox(false);
                        menuyeDonmek();
                        break;
                    case 3:
                        paradox = new Paradox(true);
                        menuyeDonmek();
                        break;
                }
            } while (secim != 4);

        }
    }
}
