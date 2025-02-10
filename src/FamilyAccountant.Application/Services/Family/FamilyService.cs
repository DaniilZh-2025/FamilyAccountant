using AutoMapper;
using FamilyAccountant.Application.DbConnection;
using FamilyAccountant.Application.Services.Family.Models;
using FamilyAccountant.Domain.Exceptions;
using FamilyAccountant.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FamilyAccountant.Application.Services.Family;

public class FamilyService(
    ICurrentUserService currentUserService,
    IFamilyRepository familyRepository,
    IUserRepository userRepository,
    IUnitOfWorkFactory unitOfWorkFactory,
    IMapper mapper,
    ILogger<FamilyService> logger)
{
    public async Task<FamilyDto> Create()
    {
        var currentUser = await currentUserService.GetCurrentUser();

        if (currentUser.FamilyId != null)
            throw new BusinessException("You already in family");

        using var unitOfWork = unitOfWorkFactory.Create();
        try
        {
            unitOfWork.BeginTransaction();

            var familyId = await familyRepository.Create(unitOfWork.Transaction);
            await userRepository.UpdateFamily(currentUser.UserId, familyId, true, unitOfWork.Transaction);

            unitOfWork.Commit();

            var family = await familyRepository.FindBy(familyId);
            
            logger.LogInformation($"Created family with ID: {family!.FamilyLink} by {currentUser.UserId}");
            return mapper.Map<FamilyDto>(family);
        }
        catch
        {
            unitOfWork.Rollback();
            throw;
        }
    }
    
    public async Task<IEnumerable<FamilyMemberDto>> GetMembers()
    {
        var currentUser = await currentUserService.GetCurrentUser();
        
        if (currentUser.FamilyId == null)
            throw new BusinessException("You are not in any family");

        var members = await familyRepository.GetMembers(currentUser.FamilyId!.Value);

        return mapper.Map<List<FamilyMemberDto>>(members);
    }
    
    public async Task<IEnumerable<FamilyMemberDto>> AddMember(UserDto userDto)
    {
        var currentUser = await currentUserService.GetCurrentUser();
        
        if (currentUser.FamilyId == null)
            throw new BusinessException("You are not in any family");
        
        if (!currentUser.IsAdmin!.Value) 
            throw new PermissionsException();

        var user = await userRepository.FindBy(userDto.UserLogin)
                   ?? throw new NotFound($"User with login {userDto.UserLogin} not found");
        
        if (user.FamilyId != null)
            throw new BusinessException("The user already has a family. To join your family he must first leave his current family.");

        await userRepository.UpdateFamily(user.Id, currentUser.FamilyId.Value, false);

        var members = await familyRepository.GetMembers(currentUser.FamilyId!.Value);
        
        logger.LogInformation($"User {currentUser.UserLogin} has join {userDto.UserLogin} to family {user.FamilyId}.");

        return mapper.Map<List<FamilyMemberDto>>(members);
    }
    
    public async Task<IEnumerable<FamilyMemberDto>> RemoveMember(string userLogin)
    {
        var currentUser = await currentUserService.GetCurrentUser();
        
        if (currentUser.FamilyId == null)
            throw new BusinessException("You are not in any family");

        if (!currentUser.IsAdmin!.Value) 
            throw new PermissionsException();
        
        var user = await userRepository.FindBy(userLogin)
                   ?? throw new NotFound($"User with login {userLogin} not found");
        
        if (user.FamilyId != currentUser.FamilyId)
            throw new BusinessException("User is not in your family");
        
        if (user.Id == currentUser.UserId)
            throw new BusinessException("You cannot remove yourself.");
        
        await userRepository.RemoveFamily(user.Id);

        var members = await familyRepository.GetMembers(currentUser.FamilyId!.Value);
        
        logger.LogInformation($"User {currentUser.UserLogin} has remove {userLogin} from family {user.FamilyId}.");

        return mapper.Map<List<FamilyMemberDto>>(members);
    }
}