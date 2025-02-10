namespace FamilyAccountant.Domain.Exceptions;

public class UserAlreadyExists(string userName) : Exception($"User {userName} already exists");