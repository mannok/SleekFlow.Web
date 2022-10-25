using AutoFilterer.Attributes;
using AutoFilterer.Enums;
using AutoFilterer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SleekFlow.Web.Todos
{
    public class TodoFilter : PaginationFilterBase
    {
        public virtual long? Id { get; set; }
        [StringFilterOptions(StringFilterOption.Contains)]
        public virtual string? Name { get; set; }
        [StringFilterOptions(StringFilterOption.Contains)]
        public virtual string? Description { get; set; }
        [OperatorComparison(OperatorType.GreaterThanOrEqual)]
        public virtual DateTime? DueDateFrom { get; set; }
        [OperatorComparison(OperatorType.LessThanOrEqual)]
        public virtual DateTime? DueDateTo { get; set; }
        public virtual string? Status { get; set; }
    }
}
