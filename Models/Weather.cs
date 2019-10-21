using System;
using System.Collections.Generic;
using System.Text;

namespace WPVML_Processor.Models
{
    public class Weather
    {
        public double Temperature { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }
        public double WindSpeed { get; set; }
        public int WindDegrees { get; set; }
        public int WindGust { get; set; }
        public bool Rain { get; set; }
        public bool Snow { get; set; }
        public int Cloudiness { get; set; }
    }
}
