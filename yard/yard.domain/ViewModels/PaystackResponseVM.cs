using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yard.domain.ViewModels
{
    public class Data
    {
        public string authorization_url { get; set; }
        public string access_code { get; set; }
        public string reference { get; set; }
    }

    public class PaystackResponseVM
    {
        public bool status { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }
}
