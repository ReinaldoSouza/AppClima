using System;
using System.Collections.Generic;
using System.Text;

namespace AppClima.Global
{
    public class Constantes
    {
        //declarando a constante que será atribuida a API
        //0 = key que o site fornece após a criação da conta do desenvolvedor
        //1 = latitude / 2 = longitude
        public const string URL_DARK_SKY = "https://api.darksky.net/forecast/{0}/{1},{2}?lang=pt";
        public const string SECRET_KEY = "6394d6869024518c9eada5f6ee916b83";
    }
}
