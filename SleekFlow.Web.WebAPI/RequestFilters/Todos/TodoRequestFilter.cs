using AutoFilterer.Attributes;
using AutoFilterer.Enums;
using Microsoft.AspNetCore.Mvc;
using SleekFlow.Web.Todos;
using System.ComponentModel;

namespace SleekFlow.Web.WebAPI.Filters.Todos
{
    public class TodoRequestFilter : TodoFilter
    {
        /// <summary>
        /// id equal to...
        /// </summary>
        [FromQuery(Name = "id")]
        public override long? Id { get => base.Id; set => base.Id = value; }

        /// <summary>
        /// name contains...
        /// </summary>
        [FromQuery(Name = "name")]
        public override string? Name { get => base.Name; set => base.Name = value; }

        /// <summary>
        /// description contains... e.g. desc1
        /// </summary>
        [FromQuery(Name = "desc")]
        public override string? Description { get => base.Description; set => base.Description = value; }

        /// <summary>
        /// due date from... e.g. 2022-10-25T12:51:06.557Z
        /// </summary>
        [FromQuery(Name = "ddf")]
        public override DateTime? DueDateFrom { get => base.DueDateFrom; set => base.DueDateFrom = value; }

        /// <summary>
        /// due date to... e.g. 2022-10-25T14:55:02.557Z
        /// </summary>
        [FromQuery(Name = "ddt")]
        public override DateTime? DueDateTo { get => base.DueDateFrom; set => base.DueDateFrom = value; }

        /// <summary>
        /// status equal to... could be A or I or D
        /// </summary>
        [FromQuery(Name = "s")]
        public override string? Status { get => base.Status; set => base.Status = value; }
    }
}
