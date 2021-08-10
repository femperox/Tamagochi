using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;


namespace TamagochiLib
{ 
    public enum TamagochiType
    {
        Cat =1,
        Dog
    }


    public class Player<T> where T : Tamagochi
    {
        T tamagochi = null;
        public int CheckAliveTime = 10000;

        public int Cash = 500;

        private DateTime TimeStarted { get; set; } // время старта игры
        private DateTime TimeTamaGot { get; set; } // время получения тамагочи
        private DateTime TimeTamaDied { get; set; } // время смерти тамагочи

        public Timer CheckAliveTimer; // таймер на проверку "жив ли?"

        public Player()
        {
            TimeStarted = DateTime.Now; 
        }

        public void Get(int animal, string name, int gender,
                        TamagochiStateHandler FedHandler, TamagochiStateHandler PlayedHandler,
                        TamagochiStateHandler WashedHandler, TamagochiStateHandler WalkedHandler,
                        TamagochiStateHandler GotHandler, TamagochiStateHandler DeadHandler,
                        TamagochiStateHandler VoicedHandler, TamagochiStateHandler StatsHandler)
        {
            // если у пользователя уже есть тамагочи
            if (tamagochi != null) Misc.ThrowEX("You already have one!");

            TamagochiType tamagochiType = (TamagochiType)animal;
          
            switch (tamagochiType)
            {
                case TamagochiType.Cat: tamagochi = new Cat(name, gender) as T; break;
                case TamagochiType.Dog: tamagochi = new Dog(name, gender) as T; break;
            }

            if (tamagochi == null) throw new Exception("Can't create tamagochi");

            // установка обработчиков событий
            tamagochi.Fed += FedHandler;
            tamagochi.Played += PlayedHandler;
            tamagochi.Washed += WashedHandler;
            tamagochi.Walked += WalkedHandler;
            tamagochi.Got += GotHandler;
            tamagochi.Dead += DeadHandler;
            tamagochi.Voiced += VoicedHandler;
            tamagochi.GotStats += StatsHandler;

            tamagochi.Get();

            TimeTamaGot = DateTime.Now;

            // таймеры на состояние
            TimerCallback StatsTimerCallback = new TimerCallback(tamagochi.ChangeStat);
            Timer hungerTimer = new Timer(StatsTimerCallback, new Stats { stat = 1, value = tamagochi._hungerMinus }, 0, tamagochi._hungerTime);
            Timer funTimer = new Timer(StatsTimerCallback, new Stats { stat = 3, value = tamagochi._funMinus }, 0, tamagochi._funTime);
            Timer cleanTimer = new Timer(StatsTimerCallback, new Stats { stat = 4, value = tamagochi._cleanMinus }, 0, tamagochi._cleanTime);

        }


        public void Feed(string food)
        { 
            if (tamagochi == null) Misc.ThrowEX("You have no tamagochi");
            tamagochi.Feed(food);
            GetCauseOfDeath();
            GetStats();
        }

        public void Play(string game)
        {
            if (tamagochi == null) Misc.ThrowEX("You have no tamagochi");
            tamagochi.Play(game);
            GetCauseOfDeath();
            GetStats();
        }

        public void Wash(string soap)
        {
            if (tamagochi == null) Misc.ThrowEX("You have no tamagochi");
            tamagochi.Wash(soap);
            GetCauseOfDeath();
            GetStats();
        }

        public void Walk(string place)
        {
            if (tamagochi == null) Misc.ThrowEX("You have no tamagochi");
            tamagochi.Walk(place);
            GetCauseOfDeath();
            GetStats();
        }

        public void Talk()
        {
            if (tamagochi == null) Misc.ThrowEX("You have no tamagochi");
            tamagochi.Voice();
            GetCauseOfDeath();
            GetStats();

        }

        public void Dead(string causeOfDeath=null)
        {
            if (tamagochi == null) Misc.ThrowEX("You have no tamagochi");
            
            tamagochi.Death(causeOfDeath);
            tamagochi = null;

            TimeTamaDied = DateTime.Now;
        }

        public void Ageup()
        {
            if (tamagochi == null) Misc.ThrowEX("You have no tamagochi");
            if (tamagochi.AgeUp()) Dead();
    
        }

        public void GetCauseOfDeath()
        {
            string causeOfDeath = tamagochi.CauseOfDeath();
            if (causeOfDeath != null) Dead(causeOfDeath);
        }

        public bool CheckAlive()
        {
            if ((tamagochi == null) || (!tamagochi._isAlive)) return false;
            else return true;
        }

        public void TimerCheckAlive(object o)
        {
            if (tamagochi != null)
            {
                if (!CheckAlive()) GetCauseOfDeath();

                // Тестовое взросление 
                Ageup();
            }
            else CheckAliveTimer.Dispose();
        }

        public void GetStats()
        {
            if (tamagochi == null) Misc.ThrowEX("You have no tamagochi");
            tamagochi.GetStats();
        }
        
    }
}
