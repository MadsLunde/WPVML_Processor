using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

using WPVML_Processor.Models;

namespace WPVML_Processor.Controller.ApiModels
{
    public class CreateSession
    {

        public Guid? UserBrowserCookieId { get; set; }
        public Location Location { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
    }
}
