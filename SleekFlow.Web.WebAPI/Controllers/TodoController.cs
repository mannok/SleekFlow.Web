using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SleekFlow.Web.Todos;
using SleekFlow.Web.WebAPI.Filters.Todos;
using System.Collections;

namespace SleekFlow.Web.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoController : Controller
    {
        private readonly IMapper mapper;
        private readonly TodoService todoService;

        public TodoController(IMapper mapper, TodoService todoService)
        {
            this.mapper = mapper;
            this.todoService = todoService;
        }

        /// <summary>
        /// search todos
        /// </summary>
        /// <param name="requestFilter"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<Todo>> Get([FromQuery] TodoRequestFilter requestFilter)
        {
            var result = await todoService.GetTodos(requestFilter);

            return result;
        }

        /// <summary>
        /// create todo
        /// </summary>
        /// <param name="createTodo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Todo> Post([FromBody] CreateTodoDto createTodo)
        {
            var creator = mapper.Map<Todo>(createTodo);

            var result = await todoService.CreateTodo(creator);

            return result;
        }

        /// <summary>
        /// update todo
        /// </summary>
        /// <param name="updateTodo"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<Todo> Put([FromBody] Todo updateTodo)
        {
            var result = await todoService.UpdateTodo(updateTodo);

            return result;
        }

        /// <summary>
        /// delete todo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<long> Delete(long id)
        {
            var result = await todoService.DeleteTodo(id);

            return result;
        }
    }
}