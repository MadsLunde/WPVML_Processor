using System;
using System.Collections.Generic;
using System.Text;

using WPVML_Processor.Models;

namespace WPVML_Processor.Controller.ApiModels
{
    public class CreateSession
    {
        public Guid UserBrowserCookieId { get; set; }
        public Location Location { get; set; }
        public DateTime DateTime { get; set; }
    }
}
