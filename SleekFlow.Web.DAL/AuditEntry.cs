using Audit.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleekFlow.Web.DAL
{
    public class AuditEntry
    {
        [Key]
        public long Id { get; set; }
        public DateTime AuditDate { get; set; }
        public string UserName { get; set; }
        public string EntityType { get; set; }
        public string AuditAction { get; set; }
        public object Mutations { get; set; }
    }
}
