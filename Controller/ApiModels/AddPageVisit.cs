using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WPVML_Processor.Controller.ApiModels
{
    public class AddPageVisit
    {
        [Required]
        public string SessionId { get; set; }
        [Required]
        public int NodeId { get; set; }
        [Required]
        public string DocumentTypeAlias { get; set; }
        public int TimeSpent { get; set; }
    }
}
