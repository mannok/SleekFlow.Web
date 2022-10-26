namespace SleekFlow.Web.Todos
{
    public interface ITodoService
    {
        Task<Todo> CreateTodo(Todo todo);
        Task<long> DeleteTodo(long id);
        Task<IEnumerable<Todo>> GetTodos(TodoFilter todoFilter);
        Task<Todo> UpdateTodo(Todo todo);
    }
}