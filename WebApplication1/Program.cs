using NSwag.AspNetCore;
using NSwag.Annotations;
using System.Collections;
using Models.Todo;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;

class MyWeb{
    public static void Main(String[] args) {

        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApiDocument(config =>
        {
            config.DocumentName = "TodoAPI";
            config.Title = "TodoAPI v1";
            config.Version = "v1";
        });
        
        WebApplication app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseOpenApi();
            app.UseSwaggerUi(config =>
            {
                config.DocumentTitle = "Todo API";
                config.Path = "/swagger";
                config.DocumentPath = "/swagger/{documentName}/swagger.json";
                config.DocExpansion = "list";
            });
        }


        List<Todo> todos = new List<Todo>(){
            new Todo("Agradecer a Deus por mais um dia!",true),
            new Todo("Dar bom dia pro meu Amor!",true),
            new Todo("Me arrumar pra trabalhar",true),
            new Todo("Trabalhar de manhã",true),
            new Todo("Me alimentar corretamente",true),
            new Todo("Trabalhar a tarde",true),
            new Todo("Me deslocar para a faculdade",true),
            new Todo("Conversar com meu Amor",true),
            new Todo("Estudar",true),
            new Todo("Me deslocar para casa",false),
            new Todo("Conversar com meu Amor",false),
            new Todo("Usar o computador um pouco",false),
            new Todo("Comer alguma coisa leve",false),
            new Todo("Agradecer a Deus por mais um dia",false),
            new Todo("Dormir feliz",false),
        };

        //get todos
        app.MapGet("/tasks",()=> Results.Ok(todos));

        //get todos competas
        app.MapGet("/tasks/completed", () => {
            List<Todo> completas = todos.Where(ta => ta.IsComplete.Equals(true)).ToList();
            if(completas.Count() == 0){
                return Results.BadRequest("Nao foi localizada nenhuma tarefa.");
            }
            return Results.Ok(completas);
        });

        app.MapGet("/tasks/{id}", ([FromRoute] string id) => {
            var todo = todos.FirstOrDefault(t => t.Id.ToString().Equals(id));
            if(todo == null){
                return Results.NotFound("Pagina nao encontrada.");
            }
            return Results.Ok(todo);
        });

        app.MapPost("/tasks", ([FromBody] Todo todo) => {
            var adicionar = todos.FirstOrDefault(t => t.Id.Equals(todo.Id));
            if (adicionar == null) {
                todos.Add(todo);
                return Results.Ok(todo);
            }
            return Results.BadRequest("a todo com o ID " + todo.Id + " já existe.");
        });

        app.MapPut("/tasks/{id}", ([FromRoute] string id, [FromBody] Todo todo) => {
            var alterar = todos.FirstOrDefault(t => t.Id.ToString().Equals(id));
            if (alterar != null){
                alterar.Id = todo.Id;
                alterar.Nome = todo.Nome;
                alterar.IsComplete = todo.IsComplete;
                return Results.Ok(alterar);
            }
            return Results.BadRequest("a todo com o ID " + id + " não foi localizada.");
        });

        app.MapDelete("/tasks/{id}", ([FromRoute] string id) => {
            var deletar = todos.FirstOrDefault(t => t.Id.ToString().Equals(id));
            if(deletar != null){
                todos.Remove(deletar);
                return Results.Ok(deletar);
            };
            return Results.BadRequest("a todo com o ID " + id + " não foi localizada.");
        });

        app.Run();
    }
}