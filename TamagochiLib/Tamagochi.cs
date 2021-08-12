using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TamagochiLib
{
    public abstract class Tamagochi : ITamagochi
    {
        // Событие, когда нужно узнать состояние
        protected internal event TamagochiStateHandler GotStats;
        // Событие, когда покормили
        protected internal event TamagochiStateHandler Fed;
        // Событие, когда поиграли
        protected internal event TamagochiStateHandler Played;
        // Событие, когда помыли
        protected internal event TamagochiStateHandler Washed;
        // Событие, когда погуляли
        protected internal event TamagochiStateHandler Walked;
        // Событие, когда родился
        protected internal event TamagochiStateHandler Got;
        // Событие, когда умер
        protected internal event TamagochiStateHandler Dead;
        // Событие, когда подаёт голос
        protected internal event TamagochiStateHandler Voiced;


        enum Gender
        {
            Male =1,
            Female
        }

        public string name = "default"; // имя
        public int age = 0; // возраст
        public int health = 100; // здоровье
        public int hunger = 100; // голод
        public int fun = 100; // атрибут ментал хелфа...
        public int clean = 100; // чистота
        private Gender gender;
        private string type = null;

        protected int _deathAge = 100; // когда пора умирать

        public bool _isAlive = false;

        internal int _hungerTime = 1000; // время голода
        internal int _funTime = 1000; // время ментал хелфа
        internal int _cleanTime = 1000; // время чистки

        internal int _hungerMinus = -5;
        internal int _funMinus = -2;
        internal int _cleanMinus = -1;
        internal int _healthMinus = -3;

        private Stats stats= new Stats();

        public Tamagochi(string name, int gender)
        {
            this.name = name ?? this.name;
            this.gender = (Gender)gender;

            this.type = Misc.GetObjectName(this);
            _isAlive = true;
           
        }

        // Вызов событий
        private void CallEvent(TamagochiEventArgs e, TamagochiStateHandler h)
        {
            if (e != null) h?.Invoke(this, e);
        }

        protected virtual void onFed(TamagochiEventArgs e) => CallEvent(e, Fed);
        protected virtual void onPlayed(TamagochiEventArgs e) => CallEvent(e, Played);
        protected virtual void onWashed(TamagochiEventArgs e) => CallEvent(e, Washed);
        protected virtual void onWalked(TamagochiEventArgs e) => CallEvent(e, Walked);
        protected virtual void onGot(TamagochiEventArgs e) => CallEvent(e, Got);
        protected void onDead(TamagochiEventArgs e) => CallEvent(e, Dead);
        protected virtual void onVoiced(TamagochiEventArgs e) => CallEvent(e, Voiced);
        protected void onGotStats(TamagochiEventArgs e) => CallEvent(e, GotStats);

        public void GetStats()
        {
            string stats = $"\nHealth: {health}\t Age: {age}\n" +
                           $"Hunger: {hunger}\t Fun: {fun}\n" +
                           $"Clean: {clean}";


            onGotStats(new TamagochiEventArgs(stats));
        }

        public virtual void Feed(string food)
        {
            onFed(new TamagochiEventArgs($"You fed {name} {food}"));
        }

        public virtual void Play(string game)
        {
            onPlayed(new TamagochiEventArgs($"You played {game} with {name}"));
        }

        public virtual void Wash(string soap)
        {
            onWashed(new TamagochiEventArgs($"You washed {name} with {soap}"));
        }

        public virtual void Walk(string place)
        {
 
            onWalked(new TamagochiEventArgs($"You walked with {name} at {place}"));
        }

        // подача голоса
        public void Voice(string phrase=null)
        {  
            onVoiced(new TamagochiEventArgs($"{name}: {phrase ?? "..."}"));

        }

        internal virtual void Get()
        {
             onGot(new TamagochiEventArgs($"You got new pet!\nType: {type}\nName: {name}\nGender: {gender}"));
        }

        public void Death(string cause = null)
        {
            if (cause == null) onDead(new TamagochiEventArgs($"Your pet {name} died at the age of {age}"));
            else onDead(new TamagochiEventArgs($"Your pet {name} died at the age of {age} because of {cause}"));
        }

    
        // Старение
        protected internal bool AgeUp()
        {
            if (age < _deathAge) { age++; return false; }
            else { _isAlive = false;  return true; }
        }

        // возвращает причину смерти
        protected internal string CauseOfDeath()
        {
            if (hunger <= 0)  return "hunger";
            if (fun <= 0) return "no attention";
            if ((health <= 0) || (clean <= 0)) return "illnes";
        
            return null;
        }

        // Изменение характеристик
        protected internal void ChangeStat(object o)
        {
            Stats stats = (Stats)o;
 
            switch(stats.stat)
            {
                case 1: { CheckStatsChanged(ref hunger, stats.value); break; }
                case 2: { CheckStatsChanged(ref health, stats.value); ; break; }
                case 3: { CheckStatsChanged(ref fun, stats.value); ; break; }
                case 4: { CheckStatsChanged(ref clean, stats.value); ; break; }
            }
            
            
        }

        // содержание характеристик в пределах [0..100]
        private void CheckStatsChanged(ref int stat, int value)
        {
            stat += value;
            if (stat > 100) stat = 100;
            if (stat < 0) { stat = 0; _isAlive = false; }
        }


    
    }
}
