using System;
using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using MediatR;

namespace Application.UseCases.User.Login;

public class Handler : IRequestHandler<Request, Response>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    public Handler(IUserRepository userRepository, ITokenService tokenService, IMapper mapper)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _mapper = mapper;
    }
    public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<Domain.Entities.Core.User>(request);
        var userFromDb = await _userRepository.Authenticate(user, cancellationToken);

        if (userFromDb is null)
            return new Response(404, "User not found");

        if(!userFromDb.Password.VerifyPassword(user.Password.Content, userFromDb.Password.Salt))      
            return new Response(403, "Invalid password");  
        
        var token = _tokenService.GenerateToken(userFromDb);
        userFromDb.AssignToken(token); 
        return new Response(200, "Login Successful",[], token);
    }
}