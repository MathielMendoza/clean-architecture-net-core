﻿using AutoMapper;
using CleanArchitecture.Application.Contracts.Infrastructure;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.models;
using CleanArchitecture.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Features.Streamers.Commands;

public class StreamerCommandHandler : IRequestHandler<StreamerCommand, int>
{
    private readonly IStreamerRepository _streamerRepository;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly ILogger<StreamerCommandHandler> _logger;

    public StreamerCommandHandler(IStreamerRepository streamerRepository, IMapper mapper, IEmailService emailService,
        ILogger<StreamerCommandHandler> logger)
    {
        _streamerRepository = streamerRepository;
        _mapper = mapper;
        _emailService = emailService;
        _logger = logger;
    }


    public async Task<int> Handle(StreamerCommand request, CancellationToken cancellationToken)
    {
        var streamerEntity = _mapper.Map<Streamer>(request);
        var newStreamer = await _streamerRepository.AddAsync(streamerEntity);

        _logger.LogInformation($"Streamer {newStreamer.Id} fue creado exitosamente");

        await SendEmail(newStreamer);

        return newStreamer.Id;
    }

    private async Task SendEmail(Streamer streamer)
    {
        var email = new Email()
        {
            To = "gerbo0906@gmail.com",
            Body = "La compania de streamer se creo corectamente",
            Subject = "Mensaje de alerta"
        };

        try
        {
            await _emailService.SendEmail(email);
        }
        catch (Exception e)
        {
            _logger.LogError($"Errores enviando el email de {streamer.Id}");
        }
    }
}