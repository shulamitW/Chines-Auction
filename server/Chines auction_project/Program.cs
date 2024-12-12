using Chines_auction_project.BLL;
using Chines_auction_project.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
using System.Text;
//using Chines_auction_project.Modells;
using Microsoft.OpenApi.Models;
using Chines_auction_project.Modells;
using Chines_auction_project.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();

// Add services to the container.
//ConectionSring="Data Source=srv1\pupils;Initial Catalog=Order2024;Integrated Security=True;Trust Server Certificate=True;"

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddSwaggerGen();


var tkConf = builder.Configuration.GetSection("Jwt");
var TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidIssuer = tkConf["Issuer"],
    ValidateAudience = true,
    ValidAudience = tkConf["Audience"],
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tkConf["Key"])),
    ValidateLifetime = true,
    //ClockSkew = TimeSpan.FromSeconds(30),
    //RequireExpirationTime = true,
};

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = TokenValidationParameters;
    });

builder.Services.AddMvc();
builder.Services.AddControllers();
builder.Services.AddRazorPages();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
    c.OperationFilter<SecurityRequirementsOperationFilter>();
});



//builder.Services.AddSwaggerGen(options =>
//{
//    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
//    {
//        Name = "Authorization",
//        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
//        Description = "Please enter your token with this format: ''Bearer YOUR_TOKEN''",
//        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
//        BearerFormat = "JWT",
//        Scheme = "bearer"
//    });
//    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Name = "Bearer",
//                In = ParameterLocation.Header,
//                Reference = new OpenApiReference
//                {
//                    Id = "Bearer",
//                    Type = ReferenceType.SecurityScheme
//                }
//            },
//            new List<string>()
//        }
//    });
//});


// Add Authentication and JwtBearer
//builder.Services
//    .AddAuthentication(options =>
//    {
//        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    })
//    .AddJwtBearer(options =>
//    {
//        options.SaveToken = true;//not in video
//        options.RequireHttpsMetadata = false;//not in video
//        options.TokenValidationParameters = new TokenValidationParameters()
//        {
//            ValidateIssuer = true,
//            ValidateLifetime=true,
//            ValidateAudience = true,
//            ValidIssuer = builder.Configuration["JWT:Issuer"],
//            ValidAudience = builder.Configuration["JWT:Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
//        };
//    });


//home
//DESKTOP - 6IB58BT
//builder.Services.AddDbContext<AuctionContex>(c => c.UseSqlServer("Data Source=DESKTOP-6IB58BT;Initial Catalog=Chines auction_project_325461697;Integrated Security=True;Trust Server Certificate=True;")); //home:)
// home hadassa
//builder.Services.AddDbContext<AuctionContext>(c => c.UseSqlServer("Data Source = LAPTOP-EVTT8MP2; Initial Catalog = Chines_Auction_; Integrated Security = True;Trust Server Certificate=True"));
builder.Services.AddDbContext<AuctionContex>(c => c.UseSqlServer("Data Source = LAPTOP-EVTT8MP2; Initial Catalog = Chines_Auction_final; Integrated Security = True;Trust Server Certificate=True")); //home:)

//school
//builder.Services.AddDbContext<AuctionContex>(c => c.UseSqlServer("Data Source=srv2\\pupils;Initial Catalog=Chines auction_project_325461697;Integrated Security=True;Trust Server Certificate=True;"));//school
builder.Services.AddScoped<IDonorService, DonorService>();
builder.Services.AddScoped<IDonorDal, DonorDal>();
builder.Services.AddScoped<IPresentDal, PresentDal>();
builder.Services.AddScoped<IPresentService, PresentService>();
builder.Services.AddScoped<ICategoryDal, CategoryDal>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserDal, UserDal>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPurchaseDal, PurchaseDal>();
builder.Services.AddScoped<IPurchaseService, PurchaseService>();
builder.Services.AddScoped<IUserDal, UserDal>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRaffleService, RaffleService>();
builder.Services.AddScoped<IRaffleDal, RaffleDal>();
////////////builder.Services.AddCors();
////builder.Services.AddCors(options =>
////{
////    options.AddPolicy("CorsPolicy", builder =>
////    {
////        builder.WithOrigins("http://localhost:3000", "development web site")
////            .AllowAnyHeader()
////            .AllowAnyMethod();
////    });
////});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();
app.UseRouting();
//app.UseAuthentication();
// Configure the HTTP request pipeline.
app.UseCors(options => options
    .WithOrigins("http://localhost:3000") // Replace with your frontend origin
    .AllowAnyMethod() // Adjust based on your API requirements
    .AllowAnyHeader()); // Adjust based on your API requirements

app.UseStaticFiles(); // Serve static files from wwwroot


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapRazorPages();
});

app.MapControllers();
app.Run();



//app.UseAuthorization();

//app.UseWhen(
//    context =>
//         !context.Request.Path.StartsWithSegments("/Purchase/GetPurchaseById") &&
//         !context.Request.Path.StartsWithSegments("/Purchase/GetShoppingCartById/34") &&
//         !context.Request.Path.StartsWithSegments("/Purchase/Buy") &&
//         !context.Request.Path.StartsWithSegments("/Purchase/AddToCart") &&
//         !context.Request.Path.StartsWithSegments("/Purchase/RemoveFromCart") &&
//         !context.Request.Path.StartsWithSegments("/Purchase/GetPurchaseByPresent") &&
//         !context.Request.Path.StartsWithSegments("/Purchase/SortByNumOfPurchases") &&
//         !context.Request.Path.StartsWithSegments("/User/Register") &&
//         !context.Request.Path.StartsWithSegments("/User/Login") &&
//         !context.Request.Path.StartsWithSegments("/Present/SearchByNumOfPurcheses") &&
//         !context.Request.Path.StartsWithSegments("/Present/SearchByDonor") &&
//         !context.Request.Path.StartsWithSegments("/Present/SearchByName") &&
//         !context.Request.Path.StartsWithSegments("/Present/GetPresents"),
//    orderApp =>

//    {
//        orderApp.Use(async (context, next) =>
//        {
//            if (context.Request.Headers.ContainsKey("Authorization"))
//            {
//                var authorizationHeader = context.Request.Headers["Authorization"].ToString();
//                if (authorizationHeader.StartsWith("Bearer "))
//                {
//                    context.Request.Headers["Authorization"] = authorizationHeader.Substring("Bearer ".Length);
//                }
//            }

//            await next();
//        });
//        // orderApp.UseMiddleware<Chines_auction_project.Middleware.AuthenticationMiddleware>();
//    });


//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//    endpoints.MapRazorPages();
//});

