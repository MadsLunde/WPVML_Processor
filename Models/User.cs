using System;
using System.Collections.Generic;
using System.Text;

namespace WPVML_Processor.Models
{
    public class User
    {
        public string Id { get; set; }
        public Guid BrowserCookieId { get; set; }
        public List<UCommerceProduct> BoughtProducts { get; set; }
    }
}
