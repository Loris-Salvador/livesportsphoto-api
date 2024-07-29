using Application.Repositories;
using Google.Api;
using Google.Cloud.Firestore;
using Infrastructure.Profiles;
using Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuration FirestoreDb
//LOCAL
//var filepath = "./livesportsphoto-1bd95-firebase-adminsdk-cd6sc-f2dc58e360.json";

//RENDER
var filepath = "/etc/secrets/livesportsphoto-1bd95-firebase-adminsdk-cd6sc-f2dc58e360.json";
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filepath);


//Dependency injection 

//Firebase database
//LOCAL
//var projectId = builder.Configuration["firebaseProjectId"];

//RENDER
var projectId = Environment.GetEnvironmentVariable("FIREBASE_PROJECT_ID"); //space???


var firestoreDb = FirestoreDb.Create(projectId);
builder.Services.AddSingleton(firestoreDb);

//Repositories
builder.Services.AddScoped<IAlbumRepository, FirestoreAlbumRepository>();
builder.Services.AddScoped<ISectionRepository, FireStoreSectionRepository>();


//AutoMapper
builder.Services.AddAutoMapper(typeof(SectionProfile));


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

app.Urls.Add("http://*:80");

app.Run();
