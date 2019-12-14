using System;
using System.Collections.Generic;
using System.Text;

namespace WPVML_Processor.Models
{
    public class Session
    {
        public string Id { get; set; }
        public string User { get; set; }
        public decimal MoneySpent { get; set; }
        public Location Location{ get; set; }
        public Weather Weather { get; set; }
        public List<PageVisit> PageVisits { get; set; }
        public DateTime DateTime { get; set; }
    }
}
