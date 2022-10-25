using AutoFilterer.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleekFlow.Web.Todos
{
    public class TodoService
    {
        public async Task<IEnumerable<Todo>> GetTodos(TodoFilter todoFilter)
        {
            using var ctx = new DAL.DbContexts.SleekFlowWebDbContext();

            return await ctx.Todos.ApplyFilter(todoFilter).ToArrayAsync();
        }

        public async Task<Todo> CreateTodo(Todo todo)
        {
            using var ctx = new DAL.DbContexts.SleekFlowWebDbContext();

            await ctx.Todos.AddAsync(todo);
            await ctx.SaveChangesAsync();

            return todo;
        }

        public async Task<Todo> UpdateTodo(Todo todo)
        {
            using var ctx = new DAL.DbContexts.SleekFlowWebDbContext();

            ctx.Todos.Attach(todo);
            ctx.Entry(todo).State = EntityState.Modified;
            await ctx.SaveChangesAsync();

            return todo;
        }

        public async Task<long> DeleteTodo(long id)
        {
            using var ctx = new DAL.DbContexts.SleekFlowWebDbContext();

            var todo = ctx.Todos.Single(x => x.Id == id);
            ctx.Todos.Remove(todo);
            await ctx.SaveChangesAsync();

            return id;
        }
    }
}
