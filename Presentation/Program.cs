using Application.Repositories;
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
const string filepath = "./livesportsphoto-1bd95-firebase-adminsdk-cd6sc-f2dc58e360.json";
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filepath);


//Dependency injection 

//Firebase database
var projectId = builder.Configuration["firebaseProjectId"]; //user secrets

var firestoreDb = FirestoreDb.Create(projectId);
builder.Services.AddSingleton(firestoreDb);

//Repositories
builder.Services.AddScoped<IUserRepository, FireStoreUserRepository>();
builder.Services.AddScoped<ISectionRepository, FireStoreSectionRepository>();


//AutoMapper
builder.Services.AddAutoMapper(typeof(SectionProfile));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAllOrigins",
        configurePolicy: policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseCors("AllowAllOrigins");

app.MapControllers();

app.Run();
