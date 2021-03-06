﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AngularWebApp.Infrastructure.DI;
using AngularWebApp.Infrastructure.Web.Authentication.Models;
using AngularWebApp.Infrastructure.Web.Authentication.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AngularWebApp.Infrastructure.Web.Authentication.Services
{
    public class RolesService: IApplicationService
    {
        public const string AdministratorRoleName = "Administrators";

        private readonly ILogger<UserRolesService> log;
        private readonly RoleManager<ApplicationRole> roleManager;

        public RolesService(ILogger<UserRolesService> _log,
            RoleManager<ApplicationRole> _roleManager)
        {
            log = _log;
            roleManager = _roleManager;

            EnsureAdministratorRole();
        }

        public IEnumerable<GetRoleOutputDto> GetAll()
        {
            return roleManager.Roles
                .Select(t => new GetRoleOutputDto() { Id = t.Id, Name = t.Name })
                .OrderBy(t => t.Name);
        }

        public GetRoleOutputDto GetOne(string roleId)
        {
            var entity = roleManager.Roles.FirstOrDefault(t => t.Id == roleId);
            if (entity == null)
                return null;

            return new GetRoleOutputDto() { Id = entity.Id, Name = entity.Name };
        }

        public GetRoleOutputDto GetOneByName(string roleName)
        {
            var entity = roleManager.Roles.FirstOrDefault(t => t.Name == roleName);
            if (entity == null)
                return null;

            return new GetRoleOutputDto() { Id = entity.Id, Name = entity.Name };
        }

        public async Task<string> Add(AddRoleInputDto model)
        {
            log.LogInformation("Adding role {0}", model.Name);

            bool roleExists = await roleManager.RoleExistsAsync(model.Name);
            if (roleExists)
                throw new Exception(String.Format("Role '{0}' already exists!", model.Name));

            var role = new ApplicationRole(model.Name);
            await roleManager.CreateAsync(role);

            return role.Id;
        }

        public async Task Edit(EditRoleInputDto model)
        {
            log.LogInformation("Editing role {0} - new name = {1}", model.Id, model.Name);

            var role = await roleManager.FindByIdAsync(model.Id);
            if (role == null)
                throw new Exception(String.Format("Role ID '{0}' not found!", model.Id));

            role.Name = model.Name;
            await roleManager.UpdateAsync(role);
        }

        public async Task Delete(string roleId)
        {
            log.LogInformation("Deleting role {0}", roleId);

            var role = await roleManager.FindByIdAsync(roleId);
            if (role != null)
                await roleManager.DeleteAsync(role);
        }

        public async Task<List<Claim>> GetClaimsForRole(string userRole)
        {
            List<Claim> claims = new List<Claim>();

            var role = await roleManager.FindByNameAsync(userRole);
            if (role != null)
            {
                var roleClaims = await roleManager.GetClaimsAsync(role);
                if (roleClaims != null)
                {
                    foreach (Claim roleClaim in roleClaims)
                        claims.Add(roleClaim);
                }
            }

            return claims;
        }

        #region Private Helpers
        private async void EnsureAdministratorRole()
        {
            // Ensure the Administrator role exists
            var adminRole = GetOneByName(AdministratorRoleName);
            if (adminRole == null)
                await Add(new AddRoleInputDto() { Name = AdministratorRoleName });
        }
        #endregion
    }
}
