using AutoFilterer.Extensions;
using Microsoft.EntityFrameworkCore;
using SleekFlow.Web.DAL.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleekFlow.Web.Todos
{
    public class TodoService : ITodoService
    {
        private readonly SleekFlowWebDbContext sleekFlowWebDbContext;

        public TodoService(SleekFlowWebDbContext sleekFlowWebDbContext)
        {
            this.sleekFlowWebDbContext = sleekFlowWebDbContext;
        }

        public async Task<IEnumerable<Todo>> GetTodos(TodoFilter todoFilter)
        {
            return await sleekFlowWebDbContext.Todos.ApplyFilter(todoFilter).ToArrayAsync();
        }

        public async Task<Todo> CreateTodo(Todo todo)
        {
            await sleekFlowWebDbContext.Todos.AddAsync(todo);
            await sleekFlowWebDbContext.SaveChangesAsync();

            return todo;
        }

        public async Task<Todo> UpdateTodo(Todo todo)
        {
            sleekFlowWebDbContext.Todos.Attach(todo);
            sleekFlowWebDbContext.Entry(todo).State = EntityState.Modified;
            await sleekFlowWebDbContext.SaveChangesAsync();

            return todo;
        }

        public async Task<long> DeleteTodo(long id)
        {
            var todo = sleekFlowWebDbContext.Todos.Single(x => x.Id == id);
            sleekFlowWebDbContext.Todos.Remove(todo);
            await sleekFlowWebDbContext.SaveChangesAsync();

            return id;
        }
    }
}
