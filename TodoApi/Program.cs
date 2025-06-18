using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TodoApi;
//////////////////////////////////////////////////

// הוסף את שירותי CORS


// הוסף שירותים אחרים
// builder.Services.AddControllers();

// var app = builder.Build();

// // אפשר CORS
// app.UseCors("AllowAllOrigins");

// app.UseRouting();

// app.UseAuthorization();

// app.MapControllers();

// app.Run();

///////////////////////////////////////////////////////

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDbContext<ToDoDbContext>();

// if (builder.Environment.IsDevelopment())
// {
//     builder.Services.AddCors(options =>
//     {

//         options.AddDefaultPolicy(
//             policy =>
//             {
//                 policy.AllowAnyOrigin()
//                     .AllowAnyHeader()
//                     .AllowAnyMethod();
//             });
//     });
// }
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy  =>
                      {
                          policy.WithOrigins("https://example.com",
                                              "https://www.contoso.com");
                      });
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ToDo API",
        Description = "An ASP.NET Core Web API for managing ToDo items",
        TermsOfService = new Uri("http://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("http://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGet("/items",async (ToDoDbContext db)=> await db.Items.ToListAsync());

app.MapPost("/items", async (Item item, ToDoDbContext Db) =>
{
    var todoItem = new Item
    {
        IsComplete = item.IsComplete,
        Name = item.Name
    };
    Db.Items.Add(todoItem);
    await Db.SaveChangesAsync();
    return Results.Created($"/items/{todoItem.Id}", todoItem);
});

app.MapPut("/items/{id}", async (int Id, ToDoDbContext Db) =>
{
    var todo = await Db.Items.FindAsync(Id);
    if (todo is null) return Results.NotFound();
    todo.IsComplete = true;
    await Db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/items/{id}", async (int Id, ToDoDbContext Db) =>
{
    if (await Db.Items.FindAsync(Id) is Item todo)
    {
        Db.Items.Remove(todo);
        await Db.SaveChangesAsync();
        return Results.Ok(todo);
    }
    return Results.NotFound();
});

//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseSwagger(options =>
{
    options.SerializeAsV2 = true;
});

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "App Is Running!");
app.Run();
