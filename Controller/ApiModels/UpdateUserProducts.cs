using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPVML_Processor.Controller.ApiModels
{
    public class UpdateUserProducts
    {
        [Required]
        public string OrderGuid { get; set; }
        [Required]
        public string SessionId { get; set; }
    }
}
