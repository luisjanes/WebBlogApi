using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebBlog;
using WebBlog.Data;
using WebBlog.Services;

var builder = WebApplication.CreateBuilder(args);
var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options=>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

builder.Services.AddDbContext<BlogDataContext>();
builder.Services.AddTransient<TokenService>(); //Sempre vai gerar um novo
//builder.Services.AddSingleton(); //1 por app!
//builder.Services.AddScoped(); //Vai durar por requisição

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
