using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SleekFlow.Web.Todos;
using SleekFlow.Web.WebAPI.Filters.Todos;
using System.Collections;

namespace SleekFlow.Web.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly TodoService todoService;

        public TodoController(IMapper mapper, TodoService todoService)
        {
            this.mapper = mapper;
            this.todoService = todoService;
        }

        /// <summary>
        /// get todo
        /// </summary>
        /// <param name="id">todo id, e.g. 4</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "user, admin")]
        public async Task<Todo?> Get(long id)
        {
            var result = (await todoService.GetTodos(new TodoRequestFilter { Id = id })).SingleOrDefault();

            return result;
        }

        /// <summary>
        /// search todos
        /// </summary>
        /// <param name="requestFilter"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<Todo>> Search([FromQuery] TodoRequestFilter requestFilter)
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<long> Delete(long id)
        {
            var result = await todoService.DeleteTodo(id);

            return result;
        }
    }
}