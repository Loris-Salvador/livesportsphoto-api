using Application;
using Application.Repositories;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuration FirestoreDb
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"livesportsphoto-1bd95-firebase-adminsdk-cd6sc-f2dc58e360.json");

var projectId = builder.Configuration["firebaseProjectId"];
var firestoreDb = FirestoreDb.Create(projectId);
builder.Services.AddSingleton(firestoreDb);

//Repo
builder.Services.AddScoped<IAlbumRepository, FirestoreAlbumRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
