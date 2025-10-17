using Microsoft.EntityFrameworkCore;
using IotApi.Models;
using IotApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add PostgreSQL database context
builder.Services.AddEntityFrameworkNpgsql()
    .AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// Add health checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>();

// Register application services
builder.Services.AddScoped<ITimbreService, TimbreService>();
builder.Services.AddScoped<IModelConfigService, ModelConfigService>();
builder.Services.AddScoped<IConfigService, ConfigService>();
builder.Services.AddScoped<ISysParamsService, SysParamsService>();
builder.Services.AddScoped<ISecurityService, SecurityService>();

// Register newly implemented services
builder.Services.AddScoped<IAgentMcpAccessPointService, AgentMcpAccessPointService>();
builder.Services.AddScoped<IAgentChatAudioService, AgentChatAudioService>();
builder.Services.AddScoped<IAgentTemplateService, AgentTemplateService>();
builder.Services.AddScoped<IAgentPluginMappingService, AgentPluginMappingService>();
builder.Services.AddScoped<ISmsService, SmsService>();
builder.Services.AddScoped<IOtaService, OtaService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Seed initial data
//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//    await DataSeeder.SeedDataAsync(context);
//}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();