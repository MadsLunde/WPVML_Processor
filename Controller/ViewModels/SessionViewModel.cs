﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WPVML_Processor.Controller.ViewModels
{
    public class SessionViewModel
    {
        public string SessionId { get; set; }
        public Guid UserBrowserCookieId { get; set; }
    }
}
