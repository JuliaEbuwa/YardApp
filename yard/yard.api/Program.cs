using Microsoft.AspNetCore.Identity;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Web.Http.Cors;
using Serilog;
using yard.api.middlewares;
using yard.application;
using yard.application.Services.Implementation;
using yard.application.Services.Interface;
using yard.domain;
using yard.domain.Context;
using yard.domain.Models;
using yard.infrastructure;
using yard.infrastructure.Repositories.Interface;
using yard.infrastructure.Repositories.Implementation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
builder.Services.AddScoped<UserManager<AppUser>>();



builder.Services.AddDbContext<ApplicationContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("YardAppDb")));

var identityConfig = builder.Services.AddIdentity<AppUser, IdentityRole<int>>(options =>
{
	options.User.RequireUniqueEmail = true;
	options.Password.RequireDigit = true;
	options.Password.RequireUppercase = true;
	options.Password.RequireLowercase = true;
	options.Password.RequiredLength = 8;
	options.SignIn.RequireConfirmedEmail = true;
});
identityConfig = new IdentityBuilder(identityConfig.UserType, typeof(IdentityRole<int>), builder.Services);
identityConfig.AddEntityFrameworkStores<ApplicationContext>()
	.AddDefaultTokenProviders();

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("Admin", policy => policy.RequireRole("admin"));
});

builder.Services.Configure<DataProtectionTokenProviderOptions>
	(options => options.TokenLifespan = TimeSpan.FromMinutes(15));

var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.IncludeErrorDetails = true;
		options.Authority = "https://localhost:7278";
		options.SaveToken = true;
		options.RequireHttpsMetadata = false;
		options.TokenValidationParameters = new TokenValidationParameters
		{
			//ValidTypes = new[] { "at+jwt" },
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = jwtIssuer,
			ValidAudience = jwtIssuer,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
		};
	});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//builder.Services.AddScoped<IIdentityUserService, IdentityUserService>();

builder.Services.InjectInfraServices();
builder.Services.InjectApplicationServices();


//Paystack
builder.Services.AddHttpClient("Paystack", client =>
{
    client.BaseAddress = new Uri($"https://api.paystack.co/");
    client.DefaultRequestHeaders.Add("Authorization", "Bearer sk_test_9a6113b409e7b8b98016e4a5d52dcb373a558d7f");
    //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
    client.DefaultRequestHeaders
        .Accept
        .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
});

//Add Email Configuration
var emailConfig = builder.Configuration
	.GetSection("EmailConfiguration")
	.Get<EmailConfiguration>();

builder.Services.AddSingleton(emailConfig);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.InjectInfraServices();
builder.Services.InjectApplicationServices();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen(opt =>
{
	opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
	opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Please enter token",
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		BearerFormat = "JWT",
		Scheme = "bearer"
	});

	opt.AddSecurityRequirement(new OpenApiSecurityRequirement
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
			new string[] { }
		}
	});
});

builder.Services.AddCors(options =>
   {
	   options.AddDefaultPolicy(
		   builder =>
		   {
			   builder
					// .WithOrigins("http://127.0.0.1:5173")
					.AllowAnyOrigin()
				   	.AllowAnyHeader()
				   	.AllowAnyMethod();
		   });
   });

builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

builder.Host.UseSerilog((context, configuration) =>
	configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

// Retrieve an instance of the DbContext from the application's services
// and use to seed hotels
using (var serviceScope = app.Services.CreateScope())
{
	var services = serviceScope.ServiceProvider;

	try
	{
		var context = services.GetRequiredService<ApplicationContext>();
		await DbInitializer.SeedHotels(context);
	}
	catch (Exception ex)
	{
		Console.WriteLine("An error occurred while retrieving DbContext: " + ex.Message);
	}
}


app.UseCors();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();


app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();