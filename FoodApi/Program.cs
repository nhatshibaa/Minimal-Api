using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<FoodDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/", () => "So Delicious!!!");

app.MapGet("/fooditems", async (FoodDb db) =>
    await db.Foods.ToListAsync());

app.MapGet("/todoitems/complete", async (FoodDb db) =>
    await db.Foods.Where(t => t.IsComplete).ToListAsync());

app.MapGet("/todoitems/{id}", async (int id, FoodDb db) =>
    await db.Foods.FindAsync(id)
        is Food todo
            ? Results.Ok(todo)
            : Results.NotFound());

app.MapPost("/todoitems", async (Food todo, FoodDb db) =>
{
    db.Foods.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{todo.Id}", todo);
});

app.MapPut("/todoitems/{id}", async (int id, Food inputTodo, FoodDb db) =>
{
    var todo = await db.Foods.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async (int id, FoodDb db) =>
{
    if (await db.Foods.FindAsync(id) is Food todo)
    {
        db.Foods.Remove(todo);
        await db.SaveChangesAsync();
        return Results.Ok(todo);
    }

    return Results.NotFound();
});

app.Run();

class Food
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
}

class FoodDb : DbContext
{
    public FoodDb(DbContextOptions<FoodDb> options)
        : base(options) { }

    public DbSet<Food> Foods => Set<Food>();
}