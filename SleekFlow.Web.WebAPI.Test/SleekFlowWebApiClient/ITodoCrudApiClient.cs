using Refit;
using SleekFlow.Web.Todos;
using SleekFlow.Web.WebAPI.DTOs.Login;
using SleekFlow.Web.WebAPI.Filters.Todos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleekFlow.Web.WebAPI.Test.SleekFlowWebApiClient
{
    [Headers("Authorization: Bearer")]
    partial interface ISleekFlowWebApiClient
    {
        [Post("/Todo")]
        Task<Todo> CreateTodo(CreateTodoDto login);

        [Get("/Todo")]
        Task<IEnumerable<Todo>> SearchTodo(TodoRequestFilter filter);

        [Put("/Todo")]
        Task<Todo> UpdateTodo(Todo todo);

        [Delete("/Todo/{id}")]
        Task<long> DeleteTodo(long id);
    }
}
