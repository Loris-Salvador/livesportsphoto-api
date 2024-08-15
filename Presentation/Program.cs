using Application.Repositories;
using Google.Cloud.Firestore;
using Infrastructure.Profiles;
using Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (builder.Environment.IsDevelopment())
{
    //add secrets which are environment variable in prod/staging
    builder.Configuration.AddUserSecrets<Program>();

    //need to set environment variable (how google firebase works)
    var filepath = builder.Configuration["FIREBASE_CREDENTIALS_FILE_PATH"];
    Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filepath);
}
else
{
    builder.Configuration.AddEnvironmentVariables();
}

//Dependency injection
var projectId = builder.Configuration["FIREBASE_PROJECT_ID"];

var firestoreDb = FirestoreDb.Create(projectId);
builder.Services.AddSingleton(firestoreDb);

//Repositories
builder.Services.AddScoped<IUserRepository, FireStoreUserRepository>();
builder.Services.AddScoped<ISectionRepository, FireStoreSectionRepository>();


//AutoMapper
builder.Services.AddAutoMapper(typeof(SectionProfile));


builder.Services.AddControllersWithViews();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Localhost",
        policy =>
        {
            policy.WithOrigins("http://127.0.0.1:5500")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    options.AddPolicy(name: "AllowOrigin",
        policy =>
        {
            policy.WithOrigins("https://www.livesportsphoto.be")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    options.AddPolicy(name: "AllowAllOrigin",
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("Localhost");
}
else if (app.Environment.IsProduction())
{
    app.UseCors("AllowOrigin");
}
else if(app.Environment.IsStaging())
{
    app.UseCors("AllowAllOrigin");
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.MapControllers();

app.Run();