using NUnit.Framework;
using Refit;
using SleekFlow.Web.Todos;
using SleekFlow.Web.WebAPI.Filters.Todos;
using SleekFlow.Web.WebAPI.Test.SleekFlowWebApiClient;

namespace SleekFlow.Web.WebAPI.Test
{
    public class TodoCrudTest
    {
        private string? token;
        private ISleekFlowWebApiClient apiClient;
        private string todoName;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            apiClient = RestService.For<ISleekFlowWebApiClient>(Common.Configuration["ApiBase"], new RefitSettings()
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(token)
            });

            todoName = $"test-todo-{DateTime.Now.Ticks}";

            Login("admin").Wait();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
        }

        private async Task Login(string username)
        {
            token = await Common.GetToken(username);
        }

        [Test, Order(1)]
        public async Task CreateTodo()
        {
            var todo = await apiClient.CreateTodo(new CreateTodoDto
            {
                Name = todoName,
                Description = $"desc-test",
                DueDate = DateTime.Now.AddDays(1),
                Status = "A"
            });

            Assert.That(todo.Name, Is.EqualTo(todoName), "todo creation failed");
        }

        [Test, Order(2)]
        public async Task SearchTodo()
        {
            var todos = await apiClient.SearchTodo(new TodoRequestFilter
            {
                Name = todoName
            });

            Assert.That(todos.All(x => x.Name == todoName), Is.True, "search todo failed");
        }

        [Test, Order(3)]
        public async Task UpdateTodo()
        {
            var before = (await apiClient.SearchTodo(new TodoRequestFilter { Name = todoName })).First();

            before.Description = "updated description";

            await apiClient.UpdateTodo(before);

            var after = (await apiClient.SearchTodo(new TodoRequestFilter { Id = before.Id })).FirstOrDefault();

            Assert.That(after?.Description, Is.EqualTo("updated description"), "updaed todo failed");
        }

        [Test, Order(4)]
        public async Task DeleteTodo()
        {
            var before = (await apiClient.SearchTodo(new TodoRequestFilter { Name = todoName })).First();

            await apiClient.DeleteTodo(before.Id);

            var after = (await apiClient.SearchTodo(new TodoRequestFilter { Id = before.Id })).FirstOrDefault();

            Assert.That(after, Is.Null, "delete todo failed");
        }
    }
}