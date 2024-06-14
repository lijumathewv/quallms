﻿using QualLMS.Domain.APIModels;
using QualLMS.Domain.Models;
using QualvationLibrary;
using static QualvationLibrary.ServiceResponse;


namespace QualLMS.Domain.Contracts
{
    public interface IUserAccount
    {
        Task<GeneralResponses> CreateAccount(UserRegister user);
        Task<LoginResponses> LoginAccount(Login user);
        Task<ResponsesWithData> AllUsers(Guid OrganizationId);
    }
}
