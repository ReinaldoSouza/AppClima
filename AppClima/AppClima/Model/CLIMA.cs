using System;
using System.Collections.Generic;
using System.Text;

namespace AppClima.Model
{
    public class CLIMA
    {
        public string timezone { get; set; }

        public ATUAL currently { get; set; }
        public HORADIARIA hourly { get; set; }
        public DIA daily { get; set; }

    }

    public class ATUAL
    {
        public string summary { get; set; }
        public string icon { get; set; }
        public string temperature { get; set; }
        public string apparentTemperature { get; set; }
        public string precipProbability { get; set; }
        public string humidity { get; set; }
    }

    public class HORADIARIA
    {
        public string summary { get; set; }
        public string icon { get; set; }
        public TEMPERATURAHORA[] data { get; set; }
        //public string hora { get; set; }
    }

    public class TEMPERATURAHORA
    {
        public string summary { get; set; }
        public string icon { get; set; }
        public string precipProbability { get; set; }
        public string temperature { get; set; }
        public string hora { get; set; }
    }

    public class DIA
    {
        public string summary { get; set; }
        public string icon { get; set; }
        public TEMPERATURASEMANA[] data { get; set; }
    }

    public class TEMPERATURASEMANA
{
        public string summary { get; set; }
        public string icon { get; set; }
        public string precipProbability { get; set; }
        public string temperatureHigh { get; set; }
        public string temperatureLow { get; set; }
        public string diaSemana { get; set; }
    }

}
