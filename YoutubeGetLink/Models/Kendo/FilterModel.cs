using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoutubeGetLink.Models.Kendo
{
    public class FilterItem
    {
        public string Value { get; set; }
        public string Operator { get; set; }
        public string Field { get; set; }
        public string IgnoreCase { get; set; }
    }

    public class Filter
    {
        public List<FilterItem> Filters { get; set; }
        public string Logic { get; set; }
    }

    public class FilterModel
    {
        public Filter Filter { get; set; }
    }

}
