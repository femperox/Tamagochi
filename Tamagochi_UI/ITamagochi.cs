using System;
using System.Collections.Generic;
using System.Text;

namespace TamagochiLib
{
    interface ITamagochi
    {
        // покормить
        void Feed(string food);

        // играть
        void Play(string game);

        // помыть и тип мыла
        void Wash(string soap);

        // выгулять
        void Walk(string place);

      
    }
}
