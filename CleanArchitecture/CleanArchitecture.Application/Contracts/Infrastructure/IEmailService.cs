using CleanArchitecture.Application.models;

namespace CleanArchitecture.Application.Contracts.Infrastructure;

public interface IEmailService
{
    Task<bool> SendEmail(Email email);
} 