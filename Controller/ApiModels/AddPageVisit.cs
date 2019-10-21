using System;
using System.Collections.Generic;
using System.Text;

namespace WPVML_Processor.Controller.ApiModels
{
    public class AddPageVisit
    {
        public string SessionId { get; set; }
        public int NodeId { get; set; }
        public string DocumentTypeAlias { get; set; }
        public int TimeSpent { get; set; }
    }
}
