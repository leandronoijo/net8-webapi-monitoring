using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

static class TodosEndpoints
{
    public static void GenerateEndpoints(WebApplication app) {
        
        app.MapGet("/todos", async (TodoDBContext context) =>
        {
            var result = await context.TodoItems.ToListAsync();
            return Results.Json(result);
        });
    
        app.MapPost("/todos", async ([FromServices] TodoDBContext context, [FromServices] CatFactService catFactService,  [FromBody] TodoItem todoItem) =>
        {
            //get a dad joke
            var catFact = await catFactService.GetRandomCatFact();
            todoItem.Title = $"{todoItem.Title} - {catFact}";
            context.TodoItems.Add(todoItem);
            await context.SaveChangesAsync();
            return Results.Created($"/todos/{todoItem.Id}", todoItem);
        });
        
        app.MapGet("/todos/{id}", async (TodoDBContext context, long id) =>
        {
            var todoItem = await context.TodoItems.FindAsync(id);
            if (todoItem is null)
            {
                return Results.NotFound();
            }
            return Results.Json(todoItem);
        });
        
        app.MapPut("/todos/{id}/toggle", async (TodoDBContext context, long id) =>
        {
            var todoItem = await context.TodoItems.FindAsync(id);
            if (todoItem is null)
            {
                return Results.NotFound();
            }
            todoItem.IsCompleted = !todoItem.IsCompleted;
            await context.SaveChangesAsync();
            return Results.Ok();
        });
     
        app.MapDelete("/todos/{id}", async (TodoDBContext context, long id) =>
        {
            var todoItem = await context.TodoItems.FindAsync(id);
            if (todoItem is null)
            {
                return Results.NotFound();
            }
            context.TodoItems.Remove(todoItem);
            await context.SaveChangesAsync();
            return Results.Ok();
        });
    }
}
