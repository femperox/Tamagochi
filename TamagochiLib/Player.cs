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
                        TamagochiStateHandler VoicedHandler)
        {
            

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

            tamagochi.Get();
            
        }

        public void ThrowEX(string e) => throw new Exception(e);

        public void Feed(string food)
        { 
            if (tamagochi == null) throw new Exception("You have no tamagochi");
            tamagochi.Feed(food);
        }

        public void Play(string game)
        {
            if (tamagochi == null) throw new Exception("You have no tamagochi");
            tamagochi.Play(game);
        }

        public void Wash(string soap)
        {
            if (tamagochi == null) throw new Exception("You have no tamagochi");
            tamagochi.Wash(soap);
        }

        public void Walk(string place)
        {
            if (tamagochi == null) throw new Exception("You have no tamagochi");
            tamagochi.Walk(place);
        }

        public void Talk()
        {
            if (tamagochi == null) throw new Exception("You have no tamagochi");
            tamagochi.Voice();
        }

        public void Dead()
        {
            if (tamagochi == null) throw new Exception("You have no tamagochi");
            tamagochi.Death();
            tamagochi = null;
        }

        public void Ageup()
        {
            if (tamagochi == null) throw new Exception("You have no tamagochi");
            tamagochi.AgeUp();
        }
    }
}
