using System;
using System.Collections.Generic;
using System.Text;

namespace TamagochiLib
{
    public class Cat: Tamagochi
    {
        public Cat(string name, int gender): base(name, gender)
        {
            _deathAge = 6;

        }


    }

    public class Dog: Tamagochi
    {
        public Dog(string name, int gender) :base(name, gender)
        {
            _deathAge = 12; 
        }
    }
}
