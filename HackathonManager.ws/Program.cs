using HackathonManager.ws.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add CORS
builder.Services.AddAppliactionCors();

// Add app services
string connectionString = builder.Configuration["ConnectionString:DefaultConnection"]
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not found.");

builder.Services.ConfigureJWTOptions();
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddSwaggerWithAuthentication();
builder.Services.AddSqlDefaultServer(connectionString);
builder.Services.AddApplicationIdentity();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

// user app middlewares
app.UseAppMiddlewares();

app.UseHttpsRedirection();

app.UseApplicationCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// DB migration
await app.ApplyMigrationsAsync();

await app.RunAsync();
