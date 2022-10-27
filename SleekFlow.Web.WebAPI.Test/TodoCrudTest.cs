using NUnit.Framework;
using Refit;
using SleekFlow.Web.Todos;
using SleekFlow.Web.WebAPI.Filters.Todos;
using SleekFlow.Web.WebAPI.Test.SleekFlowWebApiClient;

namespace SleekFlow.Web.WebAPI.Test
{
    internal class TodoCrudTest : WebApiTestBase
    {
        private string todoName;

        [OneTimeSetUp]
        public override void OneTimeSetUp()
        {
            todoName = $"test-todo-{DateTime.Now.Ticks}";

            base.OneTimeSetUp();
        }

        [OneTimeTearDown]
        public override void OneTimeTearDown()
        {
            base.OneTimeTearDown();
        }

        public override void TearDown()
        {
            Login("admin").Wait();
            CleanupTestTodo().Wait();   // comment this line if you want to see created record in db

            base.TearDown();
        }


        public async Task CleanupTestTodo()
        {
            var todos = (await apiClient.SearchTodo(new TodoRequestFilter { Name = todoName })).ToArray();
            await Task.WhenAll(todos.Select(todo => apiClient.DeleteTodo(todo.Id)));
        }

        [Test]
        public async Task TestCreateAndSearchTodo()
        {
            await Login("admin");

            var createdTodo = await apiClient.CreateTodo(new CreateTodoDto
            {
                Name = todoName,
                Description = $"desc-test",
                DueDate = DateTime.Now.AddDays(1),
                Status = "A"
            });

            Assert.That(createdTodo.Name, Is.EqualTo(todoName), "todo creation failed");

            var searchedTodo = (await apiClient.SearchTodo(new TodoRequestFilter { Id = createdTodo.Id })).SingleOrDefault();

            Assert.That(searchedTodo?.Name, Is.EqualTo(todoName), "search or create todo failed");
        }

        [Test]
        public async Task TestUpdateTodo()
        {
            await Login("admin");

            var before = await apiClient.CreateTodo(new CreateTodoDto
            {
                Name = todoName,
                Description = $"desc-test",
                DueDate = DateTime.Now.AddDays(1),
                Status = "A"
            });

            before.Description = "updated description";

            await apiClient.UpdateTodo(before);

            var after = (await apiClient.SearchTodo(new TodoRequestFilter { Id = before.Id })).SingleOrDefault();

            Assert.That(after?.Description, Is.EqualTo("updated description"), "updaed todo failed");
        }

        [Test]
        public async Task TestDeleteTodo()
        {
            await Login("admin");

            var before = await apiClient.CreateTodo(new CreateTodoDto
            {
                Name = todoName,
                Description = $"desc-test",
                DueDate = DateTime.Now.AddDays(1),
                Status = "A"
            });

            await apiClient.DeleteTodo(before.Id);

            var after = (await apiClient.SearchTodo(new TodoRequestFilter { Id = before.Id })).FirstOrDefault();

            Assert.That(after, Is.Null, "delete todo failed");
        }
    }
}