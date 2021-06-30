using System;
using System.Collections.Generic;
using System.Text;


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
        public void Get(TamagochiType tamagochiType, string name, int gender,
                        TamagochiStateHandler FedHandler, TamagochiStateHandler PlayedHandler,
                        TamagochiStateHandler WashedHandler, TamagochiStateHandler WalkedHandler,
                        TamagochiStateHandler GotHandler, TamagochiStateHandler DeadHandler,
                        TamagochiStateHandler VoicedHandler, TamagochiStateHandler StatsHandler)
        {
            // если у пользователя уже есть тамагочи
            if (tamagochi != null) Misc.ThrowEX("You already have one!");

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
            
        }



        public void Feed(string food)
        { 
            if (tamagochi == null) Misc.ThrowEX("You have no tamagochi");
            tamagochi.Feed(food);
            tamagochi.GetStats();
        }

        public void Play(string game)
        {
            if (tamagochi == null) Misc.ThrowEX("You have no tamagochi");
            tamagochi.Play(game);
            tamagochi.GetStats();
        }

        public void Wash(string soap)
        {
            if (tamagochi == null) Misc.ThrowEX("You have no tamagochi");
            tamagochi.Wash(soap);
            tamagochi.GetStats();
        }

        public void Walk(string place)
        {
            if (tamagochi == null) Misc.ThrowEX("You have no tamagochi");
            tamagochi.Walk(place);
            tamagochi.GetStats();
        }

        public void Talk()
        {
            if (tamagochi == null) Misc.ThrowEX("You have no tamagochi");
            tamagochi.Voice();
            GetCauseOfDeath();
 

        }

        public void Dead(string causeOfDeath=null)
        {
            if (tamagochi == null) Misc.ThrowEX("You have no tamagochi");
            
            tamagochi.Death(causeOfDeath);
            tamagochi = null;

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
            if ((tamagochi == null) || (tamagochi._isAlive)) return false;
            else return true;
        }

    }
}
