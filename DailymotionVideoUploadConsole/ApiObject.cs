using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailymotionVideoUploadConsole
{
    internal class ApiObject
    {
        public string upload_url{ get; set; }
        public string progress_url { get; set; }
        public string url { get; set; }
        public string id { get; set; }
        public string published { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string channel { get; set; }
        public string tags { get; set; }
        public string is_created_for_kids { get; set; }

        
    }
}
