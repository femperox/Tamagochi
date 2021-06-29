using System;
using System.Collections.Generic;
using System.Text;

namespace TamagochiLib
{
    public delegate void TamagochiStateHandler(object sender, TamagochiEventArgs e);

    public class TamagochiEventArgs
    {
        public string Mes { get; private set;}
        public string Food { get; private set; }
        public string Game { get; private set; }
        public string Soap { get; private set; }
        public string Place { get; private set; }

        public TamagochiEventArgs( string _mes, 
                                   string _food = null, string _game = null,
                                   string _soap = null, string _place = null)
        {
            Mes = _mes;
            Food = _food;
            Game = _game;
            Soap = _soap;
            Place = _place;
        }


    }

    
}
