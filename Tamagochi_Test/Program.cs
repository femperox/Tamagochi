using System;
using TamagochiLib;
using System.Threading;


namespace Tamagochi_Test
{
    class Program
    {
        static ConsoleColor color = ConsoleColor.White;
        static ConsoleColor colorInfo = ConsoleColor.Yellow;
        static Timer statsWatcherTimer;

        static Player<Tamagochi> player = new Player<Tamagochi>();

        static void Main(string[] args)
        {
            bool run = true;
            bool watchStats = false;



            while (run)
            {
                TimerCallback statsWatcherTimerCallback = new TimerCallback(WatchTamaStats);

                if (!watchStats)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("1. Get pet \t 2. Feed pet  \t 3. Walk pet");
                    Console.WriteLine("4. Wash pet\t 5. Play with\t 6. Talk with pet");
                    Console.WriteLine("7. Death...\t 8. Watch Stats\t 9. End game");
                    Console.WriteLine("Enter number:");

                    statsWatcherTimer?.Dispose();
                }
                else
                {
                    watchStats = false;
                    statsWatcherTimer = new Timer(statsWatcherTimerCallback, null, 0, 1000);
                    Console.Clear();
                }
                Console.ForegroundColor = color;

                try
                {
                    int command = Convert.ToInt32(Console.ReadLine());

                    switch (command)
                    {
                        case 1: GetTama(player); break;
                        case 2: FeedTama(player); break;
                        case 3: WalkTama(player); break;
                        case 4: WashTama(player); break;
                        case 5: PlayTama(player); break;
                        case 6: TalkTama(player); break;
                        case 7: DeathTama(player); break;
                        case 8: { watchStats = true; WatchTamaStats(null); break; }
                        case 9: run = false; continue;
                        case 0: { watchStats = false; Console.Clear(); break; }
                    }
                    if (player.CheckAlive()) player.Ageup();
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }

                
            }
        
        }

        private static void GetTama(Player<Tamagochi> player)
        {
            if (player.CheckAlive()) Misc.ThrowEX("You have tamagochi");

            

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Choose animal: 1.Cat 2.Dog");
            Console.ForegroundColor = color;
            int animal = Convert.ToInt32(Console.ReadLine());

            TamagochiType tamagochiType = (TamagochiType)animal;

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Enter name: ");
            Console.ForegroundColor = color;
            string name = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Choose gender: 1. Male 2.Female");
            Console.ForegroundColor = color;
            int gender = Convert.ToInt32(Console.ReadLine());

            Console.ForegroundColor = colorInfo;
            player.Get(tamagochiType, name, gender,
                       (o, e) => Console.WriteLine(e.Mes),
                       (o, e) => Console.WriteLine(e.Mes),
                       (o, e) => Console.WriteLine(e.Mes),
                       (o, e) => Console.WriteLine(e.Mes),
                       (o, e) => Console.WriteLine(e.Mes),
                       (o, e) => Console.WriteLine(e.Mes),
                       (o, e) => Console.WriteLine(e.Mes),
                       (o, e) => Console.WriteLine(e.Mes));

            
             TimerCallback AliveTimerCallback = new TimerCallback(player.TimerCheckAlive);
             player.CheckAliveTimer = new Timer(AliveTimerCallback, null, 0, player.CheckAliveTime);

        }

        private static void FeedTama(Player<Tamagochi> player)
        {
            if (!player.CheckAlive()) { Console.WriteLine(player.CheckAlive()); Misc.ThrowEX("You have no tamagochi"); }

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("What do you want to feed it with?");
            Console.ForegroundColor = color;
            string food = Console.ReadLine();

            Console.ForegroundColor = colorInfo;
            player.Feed(food);
        }

        private static void WalkTama(Player<Tamagochi> player)
        {
            if (!player.CheckAlive()) Misc.ThrowEX("You have no tamagochi");

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Where do you want to walk it?");
            Console.ForegroundColor = color;
            string s = Console.ReadLine();

            Console.ForegroundColor = colorInfo;
            player.Walk(s);
        }

        private static void WashTama(Player<Tamagochi> player)
        {
            if (!player.CheckAlive()) Misc.ThrowEX("You have no tamagochi");

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Whith what do you want to wash it?");
            Console.ForegroundColor = color;
            string s = Console.ReadLine();

            Console.ForegroundColor = colorInfo;
            player.Walk(s);
        }

        private static void PlayTama(Player<Tamagochi> player)
        {
            if (!player.CheckAlive()) Misc.ThrowEX("You have no tamagochi");

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("In what game do you want to play with it?");
            Console.ForegroundColor = color;
            string s = Console.ReadLine();

            Console.ForegroundColor = colorInfo;
            player.Walk(s);
        }

        private static void TalkTama(Player<Tamagochi> player)
        {
            if (!player.CheckAlive()) Misc.ThrowEX("You have no tamagochi");

            Console.ForegroundColor = colorInfo;
            player.Talk();
        }

        private static void DeathTama(Player<Tamagochi> player)
        {
            if (!player.CheckAlive()) Misc.ThrowEX("You have no tamagochi");

            Console.ForegroundColor = colorInfo;
            player.Dead();
        }

        private static void WatchTamaStats(object o)
        {
           
            Console.Clear();
            Console.WriteLine("To stop watching stats press \"0\" key");
            if (player.CheckAlive()) player.GetStats();
            
        }
    }
}
