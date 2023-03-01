using Application.GlobalDtos;
using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Users.Command;

public class UpdateUserBasicInfoCommand : IRequest<GlobalResponseDto<string>>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Guid UserId { get; set; }

    public UpdateUserBasicInfoCommand(string firstName, string lastName, Guid userId)
    {
        FirstName = firstName;
        LastName = lastName;
        UserId = userId;
    }
}

public class UpdateUserBasicInfoCommandHandler : IRequestHandler<UpdateUserBasicInfoCommand, GlobalResponseDto<string>>
{
    private readonly IUserRepository _repository;

    public UpdateUserBasicInfoCommandHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<GlobalResponseDto<string>> Handle(UpdateUserBasicInfoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            (bool isSuccess, Exception? ex) = await _repository.UpdateUserBasicInfo(request.UserId, request.FirstName, request.LastName);
            return new GlobalResponseDto<string>( isSuccess ? "User added" : ex!.Message, isSuccess);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new GlobalResponseDto<string>("Could not add user", isSuccess: false);
        }
    }
}