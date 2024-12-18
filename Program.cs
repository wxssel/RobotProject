using RobotProject.Components;
using SimpleMqtt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

var sqlConnectionString = configuration["SQL:ConnectionString"];
var userName = configuration["MQTT:UserName"];
var password = configuration["MQTT:Password"];

builder.Services.AddSingleton<IUserRepository, SqlUserRespository>(o => new SqlUserRespository(sqlConnectionString));

// Create a simple MQTT client with the necessary connection details
var simpleMqttClient = new SimpleMqttClient(new()
{
    Host = "1c3baa4edf4346dd98626b5dc5865638.s1.eu.hivemq.cloud", // maak eventueel een account aan bij hivemq als dit problemen geeft.
            Port = 8883,
            CleanStart = false, // <--- false, haalt al gebufferde meldingen ook op.
            ClientId = "web_app", // Dit clientid moet uniek zijn binnen de broker
            TimeoutInMs = 5_000, // Standaard time-out bij het maken van een verbinding (5 seconden)
            UserName = userName,
            Password = password
});

// Register the Simple MQTT client as an object in the dependency injection container
builder.Services.AddSingleton(simpleMqttClient); 

// Configure a MQTT Message Processing Service (that runs continuously in the background)
builder.Services.AddHostedService<MqttMessageProcessingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
