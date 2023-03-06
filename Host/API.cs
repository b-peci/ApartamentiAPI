using System.Text;
using Application.Features.Users.Command;
using Host.EndPointMethods;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.Google;
namespace Host;

public static class API
{
    public static void ConfigureAPI(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseHttpsRedirection();
        app.UseCors("CorsPolicy");
    }
    
    public static void AddMapping(this WebApplication app)
    {
        #region User Endpoint

        #region GET
        app.MapGet("/User/IsEmailTaken/{email}", UserEndPoint.IsEmailTaken);
        app.MapGet("/User/UserBasicInfo", UserEndPoint.BasicInfo);
        #endregion

        #region POST
        app.MapPost("/user/checkCredentials",UserEndPoint.CheckUserCredentials);
        app.MapPost("/User/AddUser", UserEndPoint.AddUser);
        app.MapPost("/User/AddUserFromOAUTH", UserEndPoint.AddUserFromOAuth);
        app.MapPost("/User/VerifyEmail", UserEndPoint.VerifyEmail);
        app.MapPost("/User/UpdateUserBasicInfo", UserEndPoint.UpdateUserBasicInfo);
        #endregion
       
        #endregion

        #region Posts Endpoint

        #region GET
        app.MapGet("/Posts/GetPostsThumbnail/{pageNumber}", PostEndPoints.GetPostsThumbnail);
        app.MapGet("/Posts/GetFilteredPosts", PostEndPoints.GetFilteredPosts);
        app.MapGet("/Posts/GetUserPostCount", PostEndPoints.GetUserPostCount);
        app.MapGet("/Posts/GetPostTimeInteracted", PostEndPoints.GetPostsInteractedPosts);
        app.MapGet("/Posts/GetUserPosts/{pageNumber}", PostEndPoints.GetUserPosts);
        app.MapGet("/Posts/GetPostToEditById/{postId}", PostEndPoints.GetPostToEditById);
        app.MapGet("/Posts/IsUserCreator/{postId}", PostEndPoints.IsUserCreator);
        app.MapGet("/Posts/GetPostById/{postId}", PostEndPoints.GetPostById);
        #endregion

        #region POST
        app.MapPost("/Posts/AddPost", PostEndPoints.AddPost);
        app.MapPost("/Posts/UpdatePost", PostEndPoints.UpdatePost);
        #endregion

        #region DELETE
        app.MapDelete("/Posts/DeletePost/{postId}", PostEndPoints.DeletePost);
        #endregion
      
        #endregion

        #region Chat Endpoints

        #region GET Methods
        app.MapGet("/Chat/UserChats", ChatEndPoints.GetChats);
        app.MapGet("/Chat/UserChats/{secondUserId}", ChatEndPoints.GetChatContent);
        #endregion

        #region POST Methods
        app.MapPost("/Chat/SendMessage", ChatEndPoints.SendMessage);
        #endregion


        #endregion

        #region City Endpoints
        app.MapGet("/city/getCities/{country}", CityEndpoints.GetCities);
        #endregion
        
        #region Country Endpoints
        app.MapGet("/country/getCountries", CountryEndpoints.GetCountries);
        #endregion
    } 
    
    public static void AddAuthServices(this IServiceCollection service, IConfiguration Configuration)
    {
        service
            .AddAuthorization();
        service.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            // .AddGoogle(googleOptions =>
            // {
            //     googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
            //     googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            // })
            .AddJwtBearer(jwt =>
        {
           
            var key = Encoding.UTF8.GetBytes(Configuration["JWT:Key"]);
            jwt.SaveToken = true;
            jwt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidIssuer = Configuration["JWT:Issuer"],
                ValidAudience = Configuration["JWT:Audience"],
                ValidateIssuer = true,
                ValidateAudience = false,
                RequireExpirationTime = true,
                ValidateLifetime = true
            };
            jwt.Events = new JwtBearerEvents();
            jwt.Events.OnMessageReceived = context =>
            {
                if (context.Request.Cookies.ContainsKey("X-Access-Token"))
                {
                    context.Token = context.Request.Cookies["X-Access-Token"];
                }

                return Task.CompletedTask;
            };
        });
    }

    public static void AddCors(this IServiceCollection service, IConfiguration Configuration)
    {
        service.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                configure => configure
                    .WithOrigins(Configuration["Links:UI"])
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithExposedHeaders("Content-Disposition")
            );
        });
    }
    


    
}