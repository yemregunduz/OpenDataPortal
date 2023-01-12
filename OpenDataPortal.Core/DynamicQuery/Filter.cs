using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDataPortal.Core.DynamicQuery
{
    public class Filter
    {
        public string Field { get; set; }
        public string Operator { get; set; }
        public string? Value { get; set; }
        public string? Logic { get; set; }
        public IEnumerable<Filter>? Filters { get; set; }
    }
}
