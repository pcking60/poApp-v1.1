using AutoMapper;
using Microsoft.AspNet.Identity;
using PostOffice.Common;
using PostOffice.Model.Models;
using PostOffice.Service;
using PostOffice.Web.App_Start;
using PostOffice.Web.Infrastructure.Core;
using PostOffice.Web.Infrastructure.Extensions;
using PostOffice.Web.Models;
using PostOfiice.DAta.Exceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;

namespace PostOffice.Web.Api
{
    //[Authorize]
    [RoutePrefix("api/applicationUser")]
    public class ApplicationUserController : ApiControllerBase
    {
        private ApplicationUserManager _userManager;
        private IApplicationGroupService _appGroupService;
        private ITransactionDetailService _transactionDetailService;
        private IPOService _poService;
        private IApplicationRoleService _appRoleService;
        private IApplicationUserService _userService;

        public ApplicationUserController(
            IApplicationGroupService appGroupService,
            IApplicationRoleService appRoleService,
            ApplicationUserManager userManager,
            IPOService poService,
            IApplicationUserService userService,
            ITransactionDetailService transactionDetailService,
            IErrorService errorService)
            : base(errorService)
        {
            _appRoleService = appRoleService;
            _appGroupService = appGroupService;
            _userManager = userManager;
            _transactionDetailService = transactionDetailService;
            _poService = poService;
            _userService = userService;
        }

        [Route("getlistpaging")]
        [HttpGet]
        [Authorize(Roles = "ViewUser")]
        public HttpResponseMessage GetListPaging(HttpRequestMessage request, int page, int pageSize, string filter = null)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int totalRow = 0;
                IEnumerable<ApplicationUser> model = null;

                //check user group
                bool isManager = false;
                bool isAdministrator = false;
                isAdministrator = _userService.CheckRole(User.Identity.Name, "Administrator");
                isManager = _userService.CheckRole(User.Identity.Name, "Manager");

                if (isAdministrator)
                {
                    model = _userManager.Users;
                }
                else
                {
                    if (!isAdministrator && isManager)
                    {
                        model = _userService.GetAllByPOID(_userService.getPoId(User.Identity.Name));
                    }
                    else
                    {
                        var res = Request.CreateResponse(HttpStatusCode.Moved);
                        res.Headers.Location = new Uri("10.97.52.125:88/admin");
                        return res;
                    }
                }                
                IEnumerable<ApplicationUserViewModel> modelVm = Mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<ApplicationUserViewModel>>(model);

                foreach (var item in modelVm) {
                    var po = _poService.GetByID(item.POID);
                    item.POName = po.Name;
                }
                PaginationSet<ApplicationUserViewModel> pagedSet = new PaginationSet<ApplicationUserViewModel>()
                {
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize),
                    Items = modelVm
                };

                response = request.CreateResponse(HttpStatusCode.OK, pagedSet);

                return response;
            });
        }

        [Route("getuserinfo/{userName}")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage getUserByUsername(HttpRequestMessage request, string userName)
        {
            return CreateHttpResponse(request, () =>
            {
                var base64EncodedBytes = System.Convert.FromBase64String(userName);
                userName = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
                ApplicationUser user = _userService.getByUserName(userName);
                decimal? totalEarn = _transactionDetailService.GetTotalEarnMoneyByUsername(userName);
                var response = Mapper.Map<ApplicationUser, ApplicationUserViewModel>(user);
                response.TotalEarn = totalEarn;
                var responseData = request.CreateResponse(HttpStatusCode.OK, response);
                return responseData;
            });
        }

        [Route("getuserbypoid/{id}")]
        [HttpGet]
        public HttpResponseMessage GetAllParentID(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {                
                var model = _userService.GetAllByPOID(id);
                var responseData = Mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<ApplicationUserViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("detail/{id}")]
        [HttpGet]
        [Authorize(Roles = "ViewUser")]
        public HttpResponseMessage Details(HttpRequestMessage request, string id)
        {
            if (String.IsNullOrEmpty(id) || id == "undefined")
            {
                id = User.Identity.GetUserId();
            }

            var user = _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return request.CreateErrorResponse(HttpStatusCode.NoContent, "Không có dữ liệu");
            }
            else
            {
                var pwd = _userManager.PasswordHasher.ToString();               
                
                var applicationUserViewModel = Mapper.Map<ApplicationUser, ApplicationUserViewModel>(user.Result);
                
                var listGroup = _appGroupService.GetListGroupByUserId(applicationUserViewModel.Id);
                applicationUserViewModel.Groups = Mapper.Map<IEnumerable<ApplicationGroup>, IEnumerable<ApplicationGroupViewModel>>(listGroup);
                return request.CreateResponse(HttpStatusCode.OK, applicationUserViewModel);
            }
        }

        [Route("userinfo")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage UserInfo(HttpRequestMessage request)
        {
            var id = _userService.getByUserName(User.Identity.Name).Id;
            if (string.IsNullOrEmpty(id))
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(id) + " không có giá trị.");
            }
            var user = _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return request.CreateErrorResponse(HttpStatusCode.NoContent, "Không có dữ liệu");
            }
            else
            {
                var applicationUserViewModel = Mapper.Map<ApplicationUser, ApplicationUserViewModel>(user.Result);
                var listGroup = _appGroupService.GetListGroupByUserId(applicationUserViewModel.Id);
                applicationUserViewModel.Groups = Mapper.Map<IEnumerable<ApplicationGroup>, IEnumerable<ApplicationGroupViewModel>>(listGroup);                
                return request.CreateResponse(HttpStatusCode.OK, applicationUserViewModel);
            }
        }


        [HttpPost]
        [Route("add")]
        [Authorize(Roles = "AddUser")]
        public async Task<HttpResponseMessage> Create(HttpRequestMessage request, ApplicationUserViewModel applicationUserViewModel)
        {
            if (ModelState.IsValid)
            {
                var newAppUser = new ApplicationUser();
                newAppUser.CreatedDate = DateTime.Now;
                newAppUser.CreatedBy = User.Identity.Name;
                newAppUser.UpdateUser(applicationUserViewModel);
                try
                {
                    newAppUser.Id = Guid.NewGuid().ToString();
                    var result = await _userManager.CreateAsync(newAppUser, applicationUserViewModel.PasswordHash);
                    if (result.Succeeded)
                    {
                        var listAppUserGroup = new List<ApplicationUserGroup>();
                        foreach (var group in applicationUserViewModel.Groups)
                        {
                            listAppUserGroup.Add(new ApplicationUserGroup()
                            {
                                GroupId = group.ID,
                                UserId = newAppUser.Id
                            });
                            //add role to user
                            var listRole = _appRoleService.GetListRoleByGroupId(group.ID);
                            foreach (var role in listRole)
                            {
                                await _userManager.RemoveFromRoleAsync(newAppUser.Id, role.Name);
                                await _userManager.AddToRoleAsync(newAppUser.Id, role.Name);
                            }
                        }
                        _appGroupService.AddUserToGroups(listAppUserGroup, newAppUser.Id);
                        _appGroupService.Save();

                        return request.CreateResponse(HttpStatusCode.OK, applicationUserViewModel);
                    }
                    else
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Join(",", result.Errors));
                }
                catch (NameDuplicatedException dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
                catch (Exception ex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                }
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [HttpPut]
        [Route("update")]
        [Authorize(Roles = "UpdateUser")]
        public async Task<HttpResponseMessage> Update(HttpRequestMessage request, ApplicationUserViewModel applicationUserViewModel)
        {
            if (ModelState.IsValid)
            {
                if (applicationUserViewModel.PasswordHash != null)
                {
                    applicationUserViewModel.PasswordHash = _userManager.PasswordHasher.HashPassword(applicationUserViewModel.PasswordHash);
                }                
                var appUser = await _userManager.FindByIdAsync(applicationUserViewModel.Id);
                try
                {
                    appUser.UpdateUser(applicationUserViewModel);
                    var result = await _userManager.UpdateAsync(appUser);
                    if (result.Succeeded)
                    {
                        var listAppUserGroup = new List<ApplicationUserGroup>();
                        foreach (var group in applicationUserViewModel.Groups)
                        {
                            listAppUserGroup.Add(new ApplicationUserGroup()
                            {
                                GroupId = group.ID,
                                UserId = applicationUserViewModel.Id
                            });
                            //add role to user
                            var listRole = _appRoleService.GetListRoleByGroupId(group.ID);
                            foreach (var role in listRole)
                            {
                                await _userManager.RemoveFromRoleAsync(appUser.Id, role.Name);
                                await _userManager.AddToRoleAsync(appUser.Id, role.Name);
                            }
                        }
                        _appGroupService.AddUserToGroups(listAppUserGroup, applicationUserViewModel.Id);
                        _appGroupService.Save();
                        return request.CreateResponse(HttpStatusCode.OK, applicationUserViewModel);
                    }
                    else
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Join(",", result.Errors));
                }
                catch (NameDuplicatedException dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [HttpPut]
        [Route("updateInfo")]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> UpdateInfo(HttpRequestMessage request, ApplicationUserViewModel applicationUserViewModel)
        {
            if (ModelState.IsValid)
            {
                if(!string.IsNullOrEmpty(applicationUserViewModel.PasswordHash))
                {
                    applicationUserViewModel.PasswordHash = _userManager.PasswordHasher.HashPassword(applicationUserViewModel.PasswordHash);
                }                
                var appUser = await _userManager.FindByIdAsync(applicationUserViewModel.Id);
                try
                {
                    appUser.UpdateUser(applicationUserViewModel);
                    var result = await _userManager.UpdateAsync(appUser);                   
                    return request.CreateResponse(HttpStatusCode.OK, applicationUserViewModel);                   
                }
                catch (Exception dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [HttpPut]
        [Route("ChangePassword")]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> ChangePassword(HttpRequestMessage request, ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState.Values)
                {
                    foreach (var item1 in item.Errors)
                    {
                        if(item1.ErrorMessage.Like("%6 characters%"))
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest,"Mật khẩu ít nhất 6 ký tự");
                    }
                }
               
            }

            IdentityResult result = await _userManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    if(error.Like("Incorrect%"))
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, "Mật khẩu cũ không đúng!");
                }                                
            }

            return request.CreateErrorResponse(HttpStatusCode.OK, result.Succeeded.ToString());            
        }        

        [HttpDelete]
        [Route("delete")]
        [Authorize(Roles = "DeleteUser")]
        public async Task<HttpResponseMessage> Delete(HttpRequestMessage request, string id)
        {
            var appUser = await _userManager.FindByIdAsync(id);
            var result = await _userManager.DeleteAsync(appUser);
            if (result.Succeeded)
                return request.CreateResponse(HttpStatusCode.OK, id);
            else
                return request.CreateErrorResponse(HttpStatusCode.OK, string.Join(",", result.Errors));
        }
    }
}