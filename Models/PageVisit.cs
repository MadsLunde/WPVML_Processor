using System;
using System.Collections.Generic;
using System.Text;

namespace WPVML_Processor.Models
{
    public class PageVisit
    {
        public int CountOrder { get; set; }
        public int NodeId { get; set; }
        public string DocumentTypeAlias { get; set; }
        public int TimeSpent { get; set; }
    }
}
