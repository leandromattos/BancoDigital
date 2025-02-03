using BancoDigitalAPI.Repositories;
using BancoDigitalAPI.Services;
using BancoDigitalAPI.Swagger.Examples;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using DotNetEnv;
using BancoDigitalAPI.Validators;
using FluentValidation;
using System.Reflection;

namespace BancoDigitalAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Carregar as vari�veis do arquivo .env
            // Encontre o diret�rio da solu��o, procurando o arquivo .sln no caminho
            // Obt�m o diret�rio do assembly atual
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var assemblyDirectory = Path.GetDirectoryName(assemblyLocation);

            // Navega at� a raiz da solu��o
            var solutionPath = Path.GetFullPath(Path.Combine(assemblyDirectory, @"..\..\..\.."));

            var envPath = Path.Combine(solutionPath, "BancoDigitalAPI", ".env");

            Env.Load(envPath);

            builder.Services.AddControllers();
            builder.Services.AddValidatorsFromAssemblyContaining<RelatorioRequestValidator>();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BancoDigitalAPI", Version = "v1" });
                c.EnableAnnotations();
                c.ExampleFilters();
            });

            // Registrar o filtro de exemplos
            builder.Services.AddSwaggerExamplesFromAssemblyOf<CriarContaDTOExample>();

            // Adicionar o DbContext
            // Substituir vari�veis de ambiente corretamente
            var dbHost = Env.GetString("DB_HOST");
            var dbUser = Env.GetString("POSTGRES_USER");
            var dbPassword = Env.GetString("POSTGRES_PASSWORD");

            var connectionString = $"Host={dbHost};Database=BancoDigitalDB;Username={dbUser};Password={dbPassword};Port=5432";

            // Adicionar o DbContext com a conex�o corrigida
            builder.Services.AddDbContext<BancoContext>(options =>
                options.UseNpgsql(connectionString));

            builder.Services.AddScoped<IContaRepository, ContaRepository>();
            builder.Services.AddScoped<IContaService, ContaService>();
            builder.Services.AddScoped<ITransacaoRepository, TransacaoRepository>();
            builder.Services.AddScoped<ITransacaoService, TransacaoService>();
            builder.Services.AddScoped<ICPFValidatorService, CPFValidatorService>();

            // Adicionando o HttpClient
            builder.Services.AddHttpClient<ICPFValidatorService, CPFValidatorService>();

            // Configura��o do Health Check
            builder.Services.AddHealthChecks();

            var app = builder.Build();

            // Aplicar migra��es no banco de dados ao iniciar
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BancoContext>();
                dbContext.Database.Migrate(); // Aplica as migra��es
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();
            //app.UseAuthorization();

            // Mapeando o endpoint de health check
            app.MapHealthChecks("/health"); // A URL do health check

            app.MapControllers();

            app.Run();
        }
    }
}
