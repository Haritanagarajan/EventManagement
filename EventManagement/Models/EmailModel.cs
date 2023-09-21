using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventManagement.Models
{
    public class EmailModel
    {

        public string To { get; set; }

        public string Subject { get; set; }
        public string Body { get; set; }
        public HttpPostedFileBase Attachment { get; set; }
        public string Email { get; set; } = "20bsca121harishmithak@skacas.ac.in";
        public string Password { get; set; } = "welcome123";
    }
}

