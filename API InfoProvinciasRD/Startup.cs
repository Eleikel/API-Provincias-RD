using API_InfoProvinciasRD.Data;
using API_InfoProvinciasRD.Provincias_Mapper;
using API_InfoProvinciasRD.Repository.IConfiguration;
using API_InfoProvinciasRD.Repository.IRepositories;
using API_InfoProvinciasRD.Repository.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace API_InfoProvinciasRD
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(Options => Options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Agregar dependencia del token
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });


            services.AddAutoMapper(typeof(ProvinciasMappers));

            //Documentacion de el API
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("ApiRegion", new OpenApiInfo { Title = "API Region", Version = "v1" });
                options.SwaggerDoc("ApiProvincia", new OpenApiInfo { Title = "API Provincias", Version = "v1" });
                options.SwaggerDoc("ApiUser", new OpenApiInfo { Title = "API User Authorization", Version = "v1" });


                // Hacer que los comentarios sirvan como detalles en la documentacion del API en Swagger
                //Comentarios XML
                var archivoXmlComentarios = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var rutaApiComentarios = Path.Combine(AppContext.BaseDirectory, archivoXmlComentarios);
                options.IncludeXmlComments(rutaApiComentarios);

                //AUTENTICACION EN LA DOCUMENTACION
                //ESTO ES PARA PODER USAR EL TOKEN DEL USUARIO Y VALIDARLO PARA INGRESAR A METODOS NO AUTORIZADOS
                //Primero definir el esquema de seguridad
                options.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description = "Autenticación JWT (Bearer)",
                        Type = SecuritySchemeType.Http,
                        Scheme = "Bearer"
                    });


                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                        }, new List<string>()
                    }
                });

            });

            services.AddControllers();


            // DAR SOPORTE PARA CORS
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/ApiRegion/swagger.json", "API Region");
                options.SwaggerEndpoint("/swagger/ApiProvincia/swagger.json", "API Provincia");
                options.SwaggerEndpoint("/swagger/ApiUser/swagger.json", "API User (Authorization)");

                options.RoutePrefix = "";
            });

            app.UseRouting();


            // Estos dos son para la Autenticacion y autorizacion 
            app.UseAuthentication();
            app.UseAuthorization();



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // DAR SOPORTE PARA CORS
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        }
    }
}
