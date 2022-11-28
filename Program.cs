using EFBulkActivities_V2.DataBaseContext;
using EFBulkActivities_V2.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<DataBaseDataContext>(
    o => o.UseSqlServer(builder.Configuration.GetConnectionString("Database")));


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();




//Migration 

// Add-Migration InitialMigration

// Update - Database

#region OrdinaryUpdate
app.MapPut("Update-CPGA", async (int ClassID, DataBaseDataContext dbContext) =>
{
    var Uniclass = await dbContext
        .Set<UniversityClass>()
        .Include(c => c.Students)
        .FirstOrDefaultAsync(c => c.ID == ClassID);

    if (Uniclass is null)
    {
        return Results.NotFound(
            $"The Class with Id '{ClassID}' was not found.");
    }

    foreach (var student in Uniclass.Students)
    {
        student.CPGA += 0.1m;
    }

    await dbContext.SaveChangesAsync();

    return Results.NoContent();
});

#endregion


#region BulkUpdate

app.MapPut("Update-CPGA-v2", async (int ClassID, DataBaseDataContext dbContext) =>
{
    var Uniclass = await dbContext
        .Set<UniversityClass>()
        .FirstOrDefaultAsync(c => c.ID == ClassID);

    if (Uniclass is null)
    {
        return Results.NotFound(
            $"The Class with Id '{ClassID}' was not found.");
    }

    await dbContext.Set<Student>()
        .Where(e => e.ClassID == Uniclass.ID)
        .ExecuteUpdateAsync(s => s.SetProperty(
            e => e.CPGA,
            e => e.CPGA + 0.1m));

    return Results.NoContent();
});


#endregion


#region BulkDelete


app.MapDelete("Delete-student", async (
    int ClassID,
    DataBaseDataContext dbContext) =>
{
    var UniClass = await dbContext
        .Set<UniversityClass>()
        .FirstOrDefaultAsync(c => c.ID == ClassID);

    if (UniClass is null)
    {
        return Results.NotFound(
            $"The Class with Id '{ClassID}' was not found.");
    }

    await dbContext.Set<Student>()
        .Where(e => e.ID == UniClass.ID)
        .ExecuteDeleteAsync();

    return Results.NoContent();
});

#endregion