using WebBlog.Data;
using WebBlog.Services;

var builder = WebApplication.CreateBuilder(args);
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


app.MapControllers();
app.Run();
