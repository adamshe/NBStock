using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuandlCS.Types.Json
{
    public class Doc
    {
        public int id { get; set; }
        public string source_code { get; set; }
        public string source_name { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string urlize_name { get; set; }
        public string description { get; set; }
        public string updated_at { get; set; }
        public string frequency { get; set; }
        public string from_date { get; set; }
        public string to_date { get; set; }
        public List<string> column_names { get; set; }
        public bool @private { get; set; }
        public object type { get; set; }
        public string display_url { get; set; }
        public bool premium { get; set; }
    }

    public class Source
    {
        public int id { get; set; }
        public string code { get; set; }
        public int datasets_count { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public string host { get; set; }
        public bool premium { get; set; }
    }

    public class RootObject
    {
        public int total_count { get; set; }
        public int current_page { get; set; }
        public int per_page { get; set; }
        public List<Doc> docs { get; set; }
        public List<Source> sources { get; set; }
    }
}
