using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;
using jwtMinimalAPi.Model;
using jwtMinimalAPi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen( options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
       BearerFormat = "JWT",
       In = ParameterLocation.Header,
       Name = "Authorization",
       Description ="Bearer Authentication with JWT token",
       Type =  SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
       {
          new OpenApiSecurityScheme
          {
              Reference = new OpenApiReference
              {
                  Id = "Bearer",
                  Type = ReferenceType.SecurityScheme
              }
          },

            new List <string >()
        }

    }) ;

});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))


    };
});

builder.Services.AddAuthorization();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton <IMovieService, MovieService>();
builder.Services .AddSingleton <IUserService, UserService>();
var app = builder.Build();
app.UseSwagger();

app.UseAuthorization();
app.UseAuthentication();
app.MapGet("/", () => "Hello World!");
app.MapPost("/login", (UserLogin user, IUserService service) => Login(user, service));
app.MapPost("/create", (Movie movie, IMovieService service) => Create(movie, service));
app.MapGet("/get", (int id, IMovieService service) => Get(id, service));
app.MapGet("/list",(IMovieService service) =>List(service));
app.MapPut("/update", (Movie newmovie, IMovieService service) => Update(newmovie, service));
app.MapDelete("/delete", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
(int id, IMovieService service) => Delete(id,service ));
IResult Login(UserLogin user, IUserService service)
{
    var tokenString = "";
    if (!string.IsNullOrEmpty(user.UserName )&& !string.IsNullOrEmpty(user.Password))
    {
        var loggedInUser = service.Get(user);
        if (loggedInUser == null)
        {
            return Results.NotFound("User not found");
        }
        var claims = new[]
        {
            new Claim (ClaimTypes.NameIdentifier, loggedInUser.Username ),
            new Claim (ClaimTypes.Email, loggedInUser.EmailAddress ),
            new Claim(ClaimTypes.GivenName ,loggedInUser.GivenName ),
            new Claim(ClaimTypes.Surname ,loggedInUser .Surname ),
            new Claim (ClaimTypes.Role ,loggedInUser.Role )

        };
        var tokens = new JwtSecurityToken(

            issuer: builder.Configuration["Jwt:Issuer"],
            audience: builder.Configuration["jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            notBefore: DateTime.UtcNow,
            signingCredentials : new SigningCredentials 
            (new SymmetricSecurityKey (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"])),SecurityAlgorithms.HmacSha256)


            ) ;
        tokenString = new JwtSecurityTokenHandler().WriteToken (tokens);
     
        
    }

    return Results.Ok(tokenString);
}
[Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme,Roles ="Admin")]
IResult Create(Movie movie, IMovieService service)
{
    var result = service.Create (movie);
    return Results.Ok (result );
}
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "user, Admin")]
IResult Get(int id, IMovieService service)
{
    var movie = service.Get (id);
    if (movie == null)
    {
        return Results.NotFound("Movie not found");
    }
    return Results.Ok (movie);  
}
IResult List(IMovieService service)
{
    var list = service.List ();
    return Results.Ok (list);
}
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
IResult Update(Movie movie, IMovieService service)
{
    var update = service.Update (movie);
    if(update == null)
    {
        return Results.NotFound("Movie not found");
    }
    return Results.Ok (update);
}

IResult Delete(int id, IMovieService service)
{
    var result = service.Delete (id);
    if(!result)
    {
        return Results.BadRequest("Something went wrong");
    }
    return Results.Ok(result);
}
app.UseSwaggerUI();
app.Run();
