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

            // Carregar as variáveis do arquivo .env
            // Encontre o diretório da solução, procurando o arquivo .sln no caminho
            // Obtém o diretório do assembly atual
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var assemblyDirectory = Path.GetDirectoryName(assemblyLocation);

            // Navega até a raiz da solução
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
            // Substituir variáveis de ambiente corretamente
            var dbHost = Env.GetString("DB_HOST");
            var dbUser = Env.GetString("POSTGRES_USER");
            var dbPassword = Env.GetString("POSTGRES_PASSWORD");

            var connectionString = $"Host={dbHost};Database=BancoDigitalDB;Username={dbUser};Password={dbPassword};Port=5432";

            // Adicionar o DbContext com a conexão corrigida
            builder.Services.AddDbContext<BancoContext>(options =>
                options.UseNpgsql(connectionString));

            builder.Services.AddScoped<IContaRepository, ContaRepository>();
            builder.Services.AddScoped<IContaService, ContaService>();
            builder.Services.AddScoped<ITransacaoRepository, TransacaoRepository>();
            builder.Services.AddScoped<ITransacaoService, TransacaoService>();
            builder.Services.AddScoped<ICPFValidatorService, CPFValidatorService>();

            // Adicionando o HttpClient
            builder.Services.AddHttpClient<ICPFValidatorService, CPFValidatorService>();

            // Configuração do Health Check
            builder.Services.AddHealthChecks();

            var app = builder.Build();

            // Aplicar migrações no banco de dados ao iniciar
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BancoContext>();
                dbContext.Database.Migrate(); // Aplica as migrações
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
