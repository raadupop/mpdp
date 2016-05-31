﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Mpdp.Api.Models;
using Mpdp.Data.Infrastructure;
using Mpdp.Data.Repositories;
using Mpdp.Entities;
using Mpdp.Services.Abstract;
using Mpdp.Services.Utilities;

namespace Mpdp.Api.Controllers
{
  public class AccountController : ApiBaseController
  {
    private readonly IMembershipService _membershipService;
    private readonly IEntityBaseRepository<UserProfile> _userProfileRepository;
    private readonly IEntityBaseRepository<User> _userRepository;

    public AccountController(IMembershipService membershipService, IEntityBaseRepository<User> userRepository, IEntityBaseRepository<UserProfile> userProfileRepository, IEntityBaseRepository<Error> errorsRepository, IUnitOfWork unitOfWork) : base(errorsRepository, unitOfWork)
    {
      _membershipService = membershipService;
      _userProfileRepository = userProfileRepository;
      _userRepository = userRepository;
    }

    [AllowAnonymous]
    [HttpPost]
    public HttpResponseMessage Login(HttpRequestMessage request, LoginViewModel user)
    {
      return CreateHttpResponse(request, () =>
      {
        HttpResponseMessage response = null;

        if (ModelState.IsValid)
        {
          MembershipContext _userContext = _membershipService.ValidateUser(user.Username, user.Password);

          if (_userContext.User != null)
          {
            response = request.CreateResponse(HttpStatusCode.OK, new { success = true, userId = _userContext.User.UserProfile.Id });
          }
          else
          {
            response = request.CreateResponse(HttpStatusCode.BadRequest, new { success = false });
          }
        }
        else
          response = request.CreateResponse(HttpStatusCode.BadRequest, new { success = false });

        return response;
      });
    }

    [AllowAnonymous]
    [HttpPost]
    public HttpResponseMessage Register(HttpRequestMessage request, RegistrationViewModel user)
    {
      return CreateHttpResponse(request, () =>
     {
       HttpResponseMessage response = null;

       if (!ModelState.IsValid)
       {
         response = request.CreateResponse(HttpStatusCode.BadRequest, new { success = false });
       }
       else
       {
         User _user = _membershipService.CreateUser(user.Username, user.Email, user.Password, new[] { 2 });

         if (_user != null)
         {
           var _userProfile = new UserProfile() {Name = user.Name, User = _user};
           _userProfileRepository.Add(_userProfile);
           _unitOfWork.Commit();

           _user.UserProfile = _userProfile;
           _userRepository.Edit(_user);
           _unitOfWork.Commit();

           response = request.CreateResponse(HttpStatusCode.OK, new { success = true });
         }
         else
         {
           response = request.CreateResponse(HttpStatusCode.BadRequest, new { success = false });
         }
       }

       return response;
     });

    }
  }
}
