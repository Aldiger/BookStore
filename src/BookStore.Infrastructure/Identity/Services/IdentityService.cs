﻿namespace BookStore.Infrastructure.Identity.Services;

using System.Linq;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.Identity;
using Application.Identity.Commands.Common;
using Microsoft.AspNetCore.Identity;

internal class IdentityService : IIdentity
{
    private const string InvalidErrorMessage = "Invalid credentials.";

    private readonly UserManager<User> userManager;
    private readonly IJwtGenerator jwtGenerator;

    public IdentityService(
        UserManager<User> userManager,
        IJwtGenerator jwtGenerator)
    {
        this.userManager = userManager;
        this.jwtGenerator = jwtGenerator;
    }

    public async Task<Result<IUser>> Register(UserRequestModel userRequest)
    {
        var user = new User(userRequest.Email);

        var identityResult = await this.userManager.CreateAsync(
            user,
            userRequest.Password);

        var errors = identityResult.Errors.Select(e => e.Description);

        return identityResult.Succeeded
            ? Result<IUser>.SuccessWith(user)
            : Result<IUser>.Failure(errors);
    }

    public async Task<Result<UserResponseModel>> Login(UserRequestModel userRequest)
    {
        var user = await this.userManager.FindByEmailAsync(userRequest.Email);

        if (user == null)
        {
            return InvalidErrorMessage;
        }

        var passwordValid = await this.userManager.CheckPasswordAsync(
            user,
            userRequest.Password);

        if (!passwordValid)
        {
            return InvalidErrorMessage;
        }

        var token = await this.jwtGenerator.GenerateToken(user);

        return new UserResponseModel(token);
    }
}