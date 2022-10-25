using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace SleekFlow.Web.Todos
{
    [AutoMap(typeof(Todo), ReverseMap = true)]
    public class CreateTodoDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        [StringLength(1)]
        [Required]
        public string Status { get; set; }
    }
}
