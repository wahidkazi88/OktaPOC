using Microsoft.OpenApi.Models;
using Okta.AspNetCore;
using OktaOpeIDConnectDemo.Models;
using OktaOpeIDConnectDemo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c=>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Okta Authorization header using Bearer scheme. Example:\"Authorization: Bearer <token>\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Name ="Bearer",
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Reference =new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });

});
builder.Services.Configure<OktaTokenSettings>(builder.Configuration.GetSection("Okta"));
var oktaSettings = builder.Configuration.GetSection("Okta").Get<OktaTokenSettings>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
    options.DefaultChallengeScheme = OktaDefaults.ApiAuthenticationScheme;
    options.DefaultSignInScheme = OktaDefaults.ApiAuthenticationScheme;
}).
AddOktaWebApi(new OktaWebApiOptions
{
    OktaDomain = oktaSettings.OktaDomain,
    AuthorizationServerId = oktaSettings.AuthorizationServerId,
    Audience = oktaSettings.Audience
});

builder.Services.AddAuthorization();
builder.Services.AddSingleton<ITokenService, TokenService>();

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
