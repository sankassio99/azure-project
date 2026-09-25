using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalAssistant.Features.Expenses.CreateExpense;
using PersonalAssistant.Features.Expenses.DeleteExpense;
using PersonalAssistant.Features.Expenses.ListExpenses;
using PersonalAssistant.Features.Expenses.UpdateExpense;
using PersonalAssistant.Infrastructure.Data;
using PersonalAssistant.Infrastructure.Validation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PersonalAssistantDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("PersonalAssistantDb")));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
        if (exceptionFeature?.Error is ValidationException validationException)
        {
            var errors = validationException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new ValidationProblemDetails(errors));
            return;
        }

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    });
});

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PersonalAssistantDbContext>();
    dbContext.Database.Migrate();
}

var expenses = app.MapGroup("/api/expenses");
expenses.MapListExpenses();
expenses.MapCreateExpense();
expenses.MapUpdateExpense();
expenses.MapDeleteExpense();

app.Run();
