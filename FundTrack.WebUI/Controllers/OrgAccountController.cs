﻿using FundTrack.BLL.Abstract;
using FundTrack.Infrastructure.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundTrack.WebUI.Controllers
{
    [Route("api/[controller]")]
    public class OrgAccountController:Controller
    {
        private readonly IOrganizationAccountService _orgAccountService;
        public OrgAccountController(IOrganizationAccountService orgAccountService)
        {
            this._orgAccountService = orgAccountService;
        }

        [HttpGet("[action]/{id}")]
        public JsonResult Get(int id)
        {
            return Json(this._orgAccountService.GetOrganizationAccountById(id));
        }
        [Authorize(Roles ="admin")]
        [HttpPost("[action]")]
        public JsonResult ReadAll([FromBody] UserInfoViewModel model)
        {
            return Json(this._orgAccountService.GetAccountsByOrganizationId(model.orgId));
        }
        [HttpPost("[action]")]
        public JsonResult Create([FromBody]OrgAccountViewModel model)
        {
            this._orgAccountService.CreateOrganizationAccount(model);
            return Json(model);
        }
        [HttpPut("[action]")]
        public JsonResult Edit([FromBody]OrgAccountViewModel model)
        {
            return Json(this._orgAccountService.UpdateOrganizationAccount(model));
        }
        [Authorize]
        [HttpDelete("[action]/{id}")]
        public StatusCodeResult Delete(int id)
        {
            this._orgAccountService.DeleteOrganizationAccount(id);
            return StatusCode(200);
        }
    }
}