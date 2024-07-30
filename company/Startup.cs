using company.Controllers;
using company.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace company
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddScoped<BoardDAO>(provider => new BoardDAO(connectionString));
            services.AddScoped<BoardService>();
            // services.AddScoped<BoardController>(); // Controller는 여기서 추가할 필요 없음

            services.AddScoped<QnaDAO>(provider => new QnaDAO(connectionString));
            services.AddScoped<QnaService>();
            // services.AddScoped<QnaController>(); // Controller는 여기서 추가할 필요 없음

            services.AddScoped<DataroomDAO>(provider => new DataroomDAO(connectionString));
            services.AddScoped<DataroomService>();
            // services.AddScoped<DataroomController>(); // Controller는 여기서 추가할 필요 없음

            services.AddScoped<ProductDAO>(provider => new ProductDAO(connectionString));
            services.AddScoped<ProductService>();
            // services.AddScoped<ProductController>(); // Controller는 여기서 추가할 필요 없음

            services.AddScoped<MemberDAO>(provider => new MemberDAO(connectionString));
            services.AddScoped<MemberService>();
            // services.AddScoped<MemberController>(); // Controller는 여기서 추가할 필요 없음

            services.AddControllers();
            services.AddTransient<EmailService>();
            services.AddScoped<EmailService>();

            services.AddControllers().AddNewtonsoftJson();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Company API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Company API v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
