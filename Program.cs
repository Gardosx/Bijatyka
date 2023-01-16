using System.Reflection.Metadata.Ecma335;
using System.IO;
using System.Security.Cryptography;
using System;
using System.Globalization;

namespace Bijatyka
{

    internal class Program
    {
        #region Struktury
        public struct Istota
        {
            public string imie;
            public string profesja;
            public double zdrowie;
            public double atak;
            public double wiedza;
        }
        #endregion

        #region Funkcje

        public static void WczytywaniePliku(ref Istota kreatura, string nazwaFolderu = "", int nazwaPliku = 0, bool wypisz = false, bool NadanieImienia = false)
        {
            string ścieżka = Path.Combine(nazwaFolderu, nazwaPliku + ".txt");

            string[] lines = File.ReadAllLines(ścieżka); // Load

            kreatura.imie = lines[0];
            kreatura.profesja = lines[1];
            kreatura.zdrowie = double.Parse(lines[2]);
            kreatura.atak = double.Parse(lines[3]);
            kreatura.wiedza = double.Parse(lines[4]);

            if (NadanieImienia)
            {
                Console.WriteLine("{0,8} {1,10} {2,19} {3,18} {4,21}", nazwaPliku + ". ", kreatura.profesja, kreatura.zdrowie, kreatura.atak, kreatura.wiedza);
                return;

            }
            if (wypisz)
            {
                Console.WriteLine("{0,8} {1,10} {2,19} {3,18} {4,21} {5,19}", nazwaPliku + ". ", kreatura.imie, kreatura.profesja, kreatura.zdrowie, kreatura.atak, kreatura.wiedza);
            }
        }
        public static bool CzyWyjść(string wprowadzenie)
        {
            wprowadzenie = wprowadzenie.ToLower();
            wprowadzenie = wprowadzenie.Replace(" ", "").Replace("ź", "z");
            return (wprowadzenie == "wyjdz");
        }
        public static void KolorCzcionki(string kolor = "biały")
        {

            switch (kolor)
            {

                case "czerwony":

                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                case "zielony":

                    Console.ForegroundColor = ConsoleColor.Green;
                    break;

                case "niebieski":

                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;

                case "biały":

                    Console.ForegroundColor = ConsoleColor.White;
                    break;


            }

            ConsoleColor foregroundColor = Console.ForegroundColor;

            Console.ForegroundColor = foregroundColor;



        }
        public static void KolorLinijki(string wiadomość, string kolor = "czerwony")
        {

            ConsoleColor foregroundColor = Console.ForegroundColor;

            switch (kolor)
            {

                case "czerwony":

                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                case "zielony":

                    Console.ForegroundColor = ConsoleColor.Green;
                    break;

                case "niebieski":

                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;

                case "biały":

                    Console.ForegroundColor = ConsoleColor.White;
                    break;


            }
            Console.Write($"{wiadomość}");
            Console.ForegroundColor = foregroundColor;

        }



        public static double ZróbDouble(string wprowadzenie)
        {
            double liczba = 0.0;

            try
            {
                wprowadzenie = wprowadzenie.Replace(" ", "").Replace(".", ",");
                liczba = double.Parse(wprowadzenie);
            }
            catch (Exception)
            {
                liczba = -2.0;
            }

            return liczba;
        }
        public static int ZróbInt(string wprowadzenie)
        {
            int liczba = 0;
            double tempLiczba = 0.0;

            try
            {
                wprowadzenie = wprowadzenie.Replace(" ", "").Replace(".", ",");
                tempLiczba = double.Parse(wprowadzenie);
                liczba = (int)tempLiczba;
                if (tempLiczba - liczba >= 0.5) liczba++;
                if (tempLiczba - liczba <= -0.5) liczba--;
            }
            catch (Exception)
            {
                liczba = -2;
            }

            return liczba;
        }
        public static void CzyszczenieEkranu()
        {
            Console.Clear();
            Thread.Sleep(100);
        }
        public static int LiczbaPlików(string nazwaFolderu, string rozszerzenie = "*.txt")
        {
            return Directory.GetFiles(nazwaFolderu, rozszerzenie).Length;
        }

        public static Istota WybórBohatera(string nickGracza)
        {

            Istota bohater = new Istota();

            Console.WriteLine("\n Dostępne profesje bohaterów:\n");

            Console.WriteLine("  =================================================================================");
            KolorCzcionki("zielony");
            Console.WriteLine("{0,7} {1,10} {2,20} {3,20} {4,20}", "Lp.", "Profesja", "Zdrowie", "Atak", "Wiedza");
            KolorCzcionki("biały");
            Console.WriteLine("  =================================================================================");

            for (int i = 1; i <= LiczbaPlików("Profesje"); i++)
            {
                Istota profesja = new Istota();

                WczytywaniePliku(ref profesja, "Profesje", i, true, true);
            }

            Console.WriteLine("  =================================================================================");

            int wybor = 0;
            int licznik = 0;

            do
            {

                if (licznik > 0)
                    KolorLinijki("\n Podano niepoprawną wartość.\n");

                Console.Write("\n Wybieram profesję nr: ");

                KolorCzcionki("niebieski");
                wybor = ZróbInt(Console.ReadLine());
                KolorCzcionki("");

                if (wybor == 1 || wybor == 2)
                    break;

                licznik++;

            } while (wybor != 1 || wybor != 2);


            string[] lines = File.ReadAllLines("./profesje/" + wybor + ".txt");

            bohater.profesja = lines[1];
            bohater.zdrowie = ZróbDouble(lines[2]);
            bohater.atak = ZróbDouble(lines[3]);
            bohater.wiedza = ZróbDouble(lines[4]);

            string output = "";

            output += $"{nickGracza}\n{bohater.profesja}\n{bohater.zdrowie}\n{bohater.atak}\n{bohater.wiedza}";//bohater.imie + "\n" + bohater.profesja + "\n" + bohater.zdrowie + "\n" + bohater.atak + "\n" + bohater.wiedza;

            File.WriteAllText("./StatystykiGracza/1.txt", output); //zapis wybranej profesji przez gracza do pliku 1.txt

            return bohater;
        }
        public static void WyświetlStatystyki()
        {

            string wyjsc = "";
            do
            {

                string[] lines = File.ReadAllLines("./StatystykiGracza/1.txt"); // Load

                string imie = lines[0];
                string profesja = lines[1];
                double zdrowie = ZróbDouble(lines[2]);
                double atak = ZróbDouble(lines[3]);
                double wiedza = ZróbDouble(lines[4]);

                Console.WriteLine("\tImię: " + imie + "\n\tProfesja: " + profesja + "\n\tZdrowie: " + zdrowie + "\n\tAtak: " + atak + "\n\tWiedza: " + wiedza);

                Console.Write("\n\n Aby wyjsc do menu wpisz (Y): ");
                wyjsc = Console.ReadLine();


            } while (wyjsc.ToLower() != "y");

        }
        public static void NoweUmiejętności()
        {
            string[] lines = File.ReadAllLines("./StatystykiGracza/1.txt");

            string imie = lines[0];
            string profesja = lines[1];
            double zdrowie = ZróbDouble(lines[2]);
            double atak = ZróbDouble(lines[3]);
            double wiedza = ZróbDouble(lines[4]);



            if (profesja == "Wojownik")
            {
                Console.WriteLine("\n  1. Ćwiczenie na kukłach [+10 Atak]\n");

            }
            else
            {
                Console.WriteLine("\n  1. Doskonalenie władania różdżką [+10 Atak]\n");
            }


            Console.WriteLine("  2. Nauka z mistrzem Holem [+10 Wiedza]\n");

            Console.Write("\n\n Wybierz opcję: ");

            int wybor = ZróbInt(Console.ReadLine());

            string output = "";

            if (wybor == 1)
                output += $"{imie}\n{profesja}\n{zdrowie}\n{(atak + 10)}\n{wiedza}";

            else if (wybor == 2)
                output += $"{imie}\n{profesja}\n{zdrowie}\n{atak}\n{(wiedza + 10)}";

            File.WriteAllText("./StatystykiGracza/1.txt", output);
        }
        public static void Arena()
        {
            Console.WriteLine("  ======================================================================================================");
            KolorCzcionki("zielony");
            Console.WriteLine("{0,7} {1,10} {2,20} {3,20} {4,20} {5,20}", "Lp.", "Imię", "Profesja", "Zdrowie", "Atak", "Wiedza");
            KolorCzcionki();
            Console.WriteLine("  ======================================================================================================");

            int ileWojów = LiczbaPlików("ZawodnicyArena");

            Istota[] wojowie = new Istota[ileWojów];

            for (int i = 1; i <= ileWojów; i++)
            {
                Thread.Sleep(300);
                WczytywaniePliku(ref wojowie[i - 1], "ZawodnicyArena", i, true);
            }

            Console.WriteLine("  ======================================================================================================");

            Istota bohater = new Istota();
            WczytywaniePliku(ref bohater, "StatystykiGracza", 1);

            Thread.Sleep(500);

            Console.Write("\n\n Dostępne opcje:\n\n\n\t 1.Wyzwij przeciwnika na pojedynek\n\t 2.Opuść arenę\n");

            KolorLinijki("\n\t W dowolonym momencie wpisz \"", "niebieski");

            KolorLinijki("wyjdź");

            KolorLinijki("\", aby wrócić do menu głównego.\n\n", "niebieski");


            int licznik = 0;
            int wybor = 0;

            do
            {
                if (licznik > 0)
                    KolorLinijki("\n Podano niepoprawną wartość.\n");

                Console.Write("\n Wybieram opcję nr: ");

                KolorCzcionki("niebieski");
                string? wprowadzono = Console.ReadLine();
                KolorCzcionki();

                if (CzyWyjść(wprowadzono))
                    return;

                wybor = ZróbInt(wprowadzono);
                licznik++;

            } while (wybor != 1 && wybor != 2);

            int WybranyPrzeciwnik = 0;
            licznik = 0;

            if (wybor == 1)
            {
                do
                {
                    if (licznik > 0)
                        KolorLinijki("\n Podano niepoprawną wartość.\n");

                    Console.Write("\n Wskaż zawodnika z listy: ");

                    KolorCzcionki("niebieski");
                    string? wprowadzono = Console.ReadLine();
                    KolorCzcionki();

                    if (CzyWyjść(wprowadzono))
                        return;

                    WybranyPrzeciwnik = ZróbInt(wprowadzono) - 1;

                    licznik++;

                } while (WybranyPrzeciwnik < 0 || WybranyPrzeciwnik > LiczbaPlików("ZawodnicyArena") - 1);

                if (WybranyPrzeciwnik >= 0 && WybranyPrzeciwnik <= LiczbaPlików("ZawodnicyArena") - 1)
                {
                    CzyszczenieEkranu();

                    Console.Write("\n  Rzuciłeś wyzwanie " + wojowie[WybranyPrzeciwnik].imie + "\n\n");

                    int max = 50;
                    Random rng = new Random();

                    int i = 1;
                    do
                    {
                        int nbr = rng.Next() % (max + 1);

                        Thread.Sleep(1000);
                        Console.WriteLine("\n  Runda " + i);

                        double ObrZadanePrzeciw = bohater.atak * 5 + bohater.wiedza * 0.5 + nbr;
                        wojowie[WybranyPrzeciwnik].zdrowie -= ObrZadanePrzeciw;

                        Thread.Sleep(1000);

                        Console.WriteLine("\n\n\tObrazenia zadane przeciwnikowi: " + ObrZadanePrzeciw);

                        nbr = rng.Next() % (max + 1);

                        double ObrOtrzymOdPrzeciw = wojowie[WybranyPrzeciwnik].atak * 5 + wojowie[WybranyPrzeciwnik].wiedza * 0.5 + nbr;
                        bohater.zdrowie -= ObrOtrzymOdPrzeciw;

                        Thread.Sleep(1000);

                        Console.Write("\tObrazenia otrzymane od przeciwnika: ");
                        KolorLinijki("" + ObrOtrzymOdPrzeciw);

                        Thread.Sleep(1000);

                        if (bohater.zdrowie > 0 || wojowie[WybranyPrzeciwnik].zdrowie > 0)
                        {
                            Console.WriteLine("\n\n\n  Bilans po " + i + " rundzie:");

                            Thread.Sleep(1000);

                            Console.Write("\n\n\tTwoje zdrowie: ");
                            KolorLinijki("" + bohater.zdrowie, "zielony");

                            Thread.Sleep(1000);
                            Console.WriteLine("\n\tZdrowie przeciwnika: " + wojowie[WybranyPrzeciwnik].zdrowie);
                        }

                        if ((bohater.zdrowie <= 0 || wojowie[WybranyPrzeciwnik].zdrowie <= 0)) break;

                        Thread.Sleep(1000);

                        KolorLinijki("\n\n  Wcisnij dowolny klawisz, aby przejść do następnej rundy: ", "niebieski");

                        Console.ReadKey();

                        i++;

                        Thread.Sleep(500);
                        Console.Clear();



                    } while (bohater.zdrowie > 0 && wojowie[WybranyPrzeciwnik].zdrowie > 0);

                    double roznica = Math.Abs(bohater.zdrowie - wojowie[WybranyPrzeciwnik].zdrowie);

                    if (bohater.zdrowie > wojowie[WybranyPrzeciwnik].zdrowie)
                        KolorLinijki("\n\n\n\tWygrywasz z " + wojowie[WybranyPrzeciwnik].imie + " o " + roznica + " punkty! W nagrodę otrzumujesz 100 PD", "zielony");

                    else
                        KolorLinijki("\n\n\n\tPrzegrywasz z " + wojowie[WybranyPrzeciwnik].imie + " o " + roznica + " punkty! Tym samym tracisz 100 PD");

                    KolorLinijki("\n\n\n  Wcisnij dowolny klawisz, aby opuścić arenę ", "niebieski");

                    Console.ReadKey();
                }
                else if (wybor == 2)
                    return;

            }
        }

        #endregion
        static void Main(string[] args)
        {
            Console.Write("\n  ====== GRA BIJATYKA ====== ");

            Console.Write("\n\n Podaj swoje imię, aby zacząć: ");

            KolorCzcionki("niebieski");
            string ImięGracza = Console.ReadLine();

            KolorCzcionki();

            Console.Clear();

            Istota bohater = WybórBohatera(ImięGracza);

            bool zostan = true;

            do
            {

                CzyszczenieEkranu(); ;

                KolorCzcionki("zielony");
                Console.WriteLine("\n Dostępne opcje \n");
                KolorCzcionki();
                Console.WriteLine("\n  1. Zobacz statystyki i ekwipunek bohatera\n");
                Console.WriteLine("  2. Posiądź nowe umiejętności\n");
                Console.Write("  3. Odwiedź sklep ");
                KolorLinijki("(ZAMKNIĘTE)\n\n");
                Console.WriteLine("  4. Udaj się na polowanie\n");
                Console.WriteLine("  5. Zawalcz na arenie\n");


                Console.Write("\n\n Wybieram opcję nr: ");

                KolorCzcionki("niebieski");
                int wybor = ZróbInt(Console.ReadLine());
                KolorCzcionki();

                CzyszczenieEkranu();

                switch (wybor)
                {
                    case 1:


                        Console.WriteLine("\n Twoje statystyki: \n\n");

                        WyświetlStatystyki();

                        break;
                    case 2:

                        Console.WriteLine("\n Dostępne opcje: ");

                        NoweUmiejętności();

                        break;

                    case 3:

                        Console.WriteLine("\nWitaj w sklepie! Rozejrzyj się, !");
                        break;

                    case 4:
                        Console.WriteLine("\nZwierzęta i potwory w pobliżu: ");
                        break;

                    case 5:
                        Console.WriteLine("\n Czołem " + ImięGracza + "! Co powiesz na szybką walkę? Rzuć okiem na naszych zawodników:\n\n");
                        Thread.Sleep(500);
                        Arena();

                        break;
                    default:
                        break;
                }



            } while (zostan);


        }
    }
}



