using DigitalShowcaseAPIServer.Data.APIs;
using DigitalShowcaseAPIServer.Data.Contexts;
using DigitalShowcaseAPIServer.Data.Interfaces;
using DigitalShowcaseAPIServer.Data.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options => // Controller API
{
    options.OutputFormatters.RemoveType<StringOutputFormatter>(); // to format everything with json output formatter, including simple strings
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type?.FullName?.ToString());
    options.UseInlineDefinitionsForEnums();

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "DigitalShowcase",
        Description = "",
        //TermsOfService = new Uri(""),
        Contact = new OpenApiContact
        {
            Name = "Evgeny 'Riketta' Chugaev",
            Email = "riketta@outlook.com",
            Url = new Uri("https://riketta.com/"),
        },
        License = new OpenApiLicense
        {
            Name = "_",
            //Url = new Uri(""),
        },
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

//builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => // TODO: use Identity Framework?
//{
//    //options.ClaimsIdentity;
//    options.User.RequireUniqueEmail = false;

//    options.Password.RequiredLength = 8;
//    options.Password.RequireDigit = false;
//    options.Password.RequireLowercase = false;
//    options.Password.RequireUppercase = false;
//    options.Password.RequireNonAlphanumeric = false;
//}).AddEntityFrameworkStores<DigitalShowcaseContext>().AddDefaultTokenProviders();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => // TODO: replace cookie schema with smth else to support non-browsers?
{
    options.Cookie.Name = "Session-Token";
    options.Cookie.HttpOnly = false;
    options.Cookie.SameSite = SameSiteMode.Strict; // Lax can be also ok if Get requests does not change backend state, but for API Server Strict looks fine
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    //options.LoginPath = "/api/users/login";
    options.ExpireTimeSpan = TimeSpan.FromHours(6);
    options.SlidingExpiration = true;
    //options.SessionStore

    options.Events.OnRedirectToAccessDenied = options.Events.OnRedirectToLogin = c =>
    {
        c.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.FromResult<object>(null!);
    };
});
builder.Services.AddAuthorization(builder =>
{
    builder.AddPolicy("ReadOnly", p =>
    {
        p.RequireAuthenticatedUser()
            .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
            .RequireRole(Roles.Guest.ToString())
            .RequireRole(Roles.User.ToString());
    });

    builder.AddPolicy("Admin", p =>
    {
        p.RequireAuthenticatedUser()
            .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
            .RequireRole(Roles.MasterAdmin.ToString())
            .RequireRole(Roles.Admin.ToString());
            //.RequireClaim(ClaimTypes.Role, new[] { Roles.MasterAdmin.ToString(), Roles.Admin.ToString() });
    });
});


#region DBs
builder.Services.AddDbContext<DigitalShowcaseContext>(options =>
{
    var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    var dbPath = Path.Join(path, "DigitalShowcase.db");
    options.UseSqlite($"Data Source={dbPath}");
});
#endregion

#region APIs
builder.Services.AddSingleton<IPasswordService<User>, PasswordService<User>>().AddOptions<PasswordServiceOptions>().Configure(options =>
{
    options.Pepper = builder.Configuration["Pepper"] ?? "P3pper"; // builder.Configuration.GetValue<string>("")
});
builder.Services.AddScoped<IDigitalShowcaseService, DigitalShowcaseService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ILotService, LotService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFileService, FileService>().AddOptions<FileServiceOptions>().Configure(options =>
{
    options.ContentRoot = builder.Configuration[WebHostDefaults.ContentRootKey]!;
    options.StaticFolder = builder.Configuration["StaticFolder"]!;
    options.UploadsFolder = builder.Configuration["UploadFolder"]!;
});
builder.Services.AddHttpContextAccessor(); // TODO: get rid of this?
#endregion

var app = builder.Build();
// Reject HTTP: HTTPS only!
//app.UseHttpsRedirection();
//app.UseHsts(); // can't be used effectively for Web APIs because HSTS is generally a browser only instruction
app.Use(async (context, next) => {
    if (!context.Request.IsHttps)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync("HTTPS required!");
    }
    else
        await next(context);
});

//app.UseStaticFiles(); // serve files from wwwroot
app.UseStaticFiles(new StaticFileOptions // to use saved image
{
    // builder.Configuration["StaticFolder"]; builder.Environment.ContentRootPath
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, builder.Configuration["StaticFolder"]!)),
    RequestPath = '/' + builder.Configuration["StaticFolder"],
});
app.UseCookiePolicy(new CookiePolicyOptions() { MinimumSameSitePolicy = SameSiteMode.Strict, });

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
};

app.MapControllers(); // Controller API

// Endpoints (Minimal API)
//app.MapCategoryEndpoints();
//app.MapLotEndpoints();

app.Run();