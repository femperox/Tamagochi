using System;
using System.Collections.Generic;
using System.Text;

namespace TamagochiLib
{
    public abstract class Tamagochi : ITamagochi
    {
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

        protected int _deathAge = 100; // когда пора умирать

        protected int _hungerTime = 1000; // время голода
        protected int _funTime = 1000; // время ментал хелфа
        protected int _cleanTime = 1000; // время чистки

        public Tamagochi(string name, int gender)
        {
            this.name = name ?? this.name;
            this.gender = (Gender)gender;
           
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
        protected virtual void onDead(TamagochiEventArgs e) => CallEvent(e, Dead);
        protected virtual void onVoiced(TamagochiEventArgs e) => CallEvent(e, Voiced);

        public virtual void Feed(string food)
        {
            onFed(new TamagochiEventArgs($"You fed {name} {food}"));
        }

        public virtual void Play(string game)
        {
            ChangeFun(10);
            onPlayed(new TamagochiEventArgs($"You played {game} with {name}"));
        }

        public virtual void Wash(string soap)
        {
            onWashed(new TamagochiEventArgs($"You washed {name} with {soap}"));
        }

        public virtual void Walk(string place)
        {
            ChangeFun(10);
            ChangeHunger(-5);
            onWalked(new TamagochiEventArgs($"You walked with {name} at {place}"));
        }

        // подача голоса
        public void Voice(string phrase=null)
        {
            onVoiced(new TamagochiEventArgs($"{name}: {phrase ?? "..."}"));
        }

        internal virtual void Get()
        {
            onGot(new TamagochiEventArgs($"You got new pet!\nType: {this.GetType()}\nName: {name}\nGender: {gender}"));
        }

        public void Death()
        {
            onDead(new TamagochiEventArgs($"Your pet {name} died at the age of {age}"));
        }

    

        // Старение
        protected internal void AgeUp()
        {
            if (age < _deathAge) age++;
            else Death();
        }

        // Изменение характеристик
        protected internal void ChangeHealth(int value) => health += value;
        protected internal void ChangeHunger(int value) => hunger += value;
        protected internal void ChangeFun(int value) => fun += value;
        protected internal void ChangeClean(int value) => clean += value;

    
    }
}
