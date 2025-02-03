using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BancoDigitalAPI.Services
{
    using DotNetEnv;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class CPFValidatorService : ICPFValidatorService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        private readonly string _apiToken;

        public CPFValidatorService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiUrl = configuration["CPFValidator:ApiUrl"];
            _apiToken = Env.GetString("CPF_VALIDATOR_TOKEN");
        }

        public async Task<CPFValidationResult> ValidarCPFAsync(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return new CPFValidationResult { Valid = false };

            var url = $"{_apiUrl}?token={_apiToken}&value={cpf}&type=cpf";

            var response = await _httpClient.GetStringAsync(url);
            if (string.IsNullOrEmpty(response))
                return new CPFValidationResult { Valid = false };

            var responseObject = JsonConvert.DeserializeObject<CPFValidationResult>(response);
            return responseObject ?? new CPFValidationResult { Valid = false };
        }
    }

    public class CPFValidationResult
    {
        [JsonProperty("valid")]
        public bool Valid { get; set; }

        [JsonProperty("formatted")]
        public string Formatted { get; set; }
    }

}
