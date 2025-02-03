namespace BancoDigitalAPI.Services
{
    public interface ICPFValidatorService
    {
        Task<CPFValidationResult> ValidarCPFAsync(string cpf);

    }
}
