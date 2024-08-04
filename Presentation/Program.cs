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

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();

    var filepath = builder.Configuration["FIREBASE_CREDENTIALS_FILE_PATH"];
    Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filepath);
}

builder.Configuration.AddEnvironmentVariables();

//Dependency injection 
//Firebase database
var projectId = builder.Configuration["FIREBASE_PROJECT_ID"]; //user secrets

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.Run();