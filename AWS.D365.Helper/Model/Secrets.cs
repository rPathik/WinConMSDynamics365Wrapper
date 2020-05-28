using System;
using System.Collections.Generic;
using System.Text;

namespace AWS.D365.Helper.Model
{
   public class Secrets
    {
        public string clientId { get; set; }

        public string clientSecret { get; set; }

        public string aadInstance { get; set; }

        public string tenantID { get; set; }

        public string serviceUrl { get; set; }


    }
}
