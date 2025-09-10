using Manager.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterServices(builder.Configuration);

var JWTSetting = builder.Configuration.GetSection("JWTSetting");

//JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(opt =>
{
    opt.SaveToken = true;
    opt.RequireHttpsMetadata = false;
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = JWTSetting["ValidAudience"],
        ValidIssuer = JWTSetting["ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTSetting["SecurityKey"]))
        
    };
    opt.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = ctx =>
        {
            Console.WriteLine($"JWT fail: {ctx.Exception.Message}");
            Console.WriteLine($"Stack:  {ctx.Exception.StackTrace}");
            Console.WriteLine($"JWT fail: {ctx.Request.Headers.Authorization.ToString()}");
            return Task.CompletedTask;
        }
        //OnChallenge = ctx =>
        //{
        //    Console.WriteLine($"JWT challenge: {ctx.Error} {ctx.ErrorDescription}");
        //    return Task.CompletedTask;
        //},
        //OnMessageReceived = ctx =>
        //{
        //    Console.WriteLine("AUTH header: " +
        //        ctx.Request.Headers.Authorization.ToString());
        //    var auth = ctx.Request.Headers.Authorization.ToString();
        //    if (auth.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        //    {
        //        ctx.Token = auth.Substring("Bearer ".Length).Trim(); // тепер ctx.Token не пустий
        //    }

        //    Console.WriteLine("Parsed token: " + ctx.Token);
        //    return Task.CompletedTask;
        //},
    };
    opt.IncludeErrorDetails = true;
});


builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization Example : 'Bearer qwErtY8zyW1abcdefGHI",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement(){
        {
            new OpenApiSecurityScheme{
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "Bearer",
                Name="Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

