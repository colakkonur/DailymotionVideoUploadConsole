using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailymotionVideoUploadConsole
{
    public class Error
    {
        public string more_info { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public string type { get; set; }
        public ErrorData error_data { get; set; }
    }

    public class ErrorData
    {
        public string reason { get; set; }
    }

    public class Root
    {
        public Error error { get; set; }
    }
}
