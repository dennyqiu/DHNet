using DHNet.Components.Extensions.Html;
using DHNet.Components.Security;
using DHNet.Data.Core;
using DHNet.Objects;
using DHNet.Resources;
using DHNet.Resources.Permission;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DHNet.Services
{
    public class RoleService : BaseService, IRoleService
    {
        public RoleService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public virtual void SeedPermissions(RoleView view)
        {
            JsTreeNode root = new JsTreeNode(Titles.All);
            view.Permissions.Nodes.Add(root);

            IEnumerable<Permission> permissions = GetAllPermissions();
            foreach (IGrouping<String, Permission> area in permissions.GroupBy(permission => permission.Area))
            {
                JsTreeNode areaNode = new JsTreeNode(area.Key);
                foreach (IGrouping<String, Permission> controller in area.GroupBy(permission => permission.Controller))
                {
                    JsTreeNode controllerNode = new JsTreeNode(controller.Key);
                    foreach (Permission permission in controller)
                        controllerNode.Nodes.Add(new JsTreeNode(permission.Id, permission.Action));

                    if (areaNode.Title == null)
                        root.Nodes.Add(controllerNode);
                    else
                        areaNode.Nodes.Add(controllerNode);
                }

                if (areaNode.Title != null)
                    root.Nodes.Add(areaNode);
            }
        }
        private IEnumerable<Permission> GetAllPermissions()
        {
            return UnitOfWork
                .Select<Permission>()
                .ToArray()
                .Select(permission => new Permission
                {
                    Id = permission.Id,
                    Area = ResourceProvider.GetPermissionAreaTitle(permission.Area),
                    Controller = ResourceProvider.GetPermissionControllerTitle(permission.Area, permission.Controller),
                    Action = ResourceProvider.GetPermissionActionTitle(permission.Area, permission.Controller, permission.Action)
                })
                .OrderBy(permission => permission.Area ?? permission.Controller)
                .ThenBy(permission => permission.Controller)
                .ThenBy(permission => permission.Action);
        }

        public IQueryable<RoleView> GetViews()
        {
            return UnitOfWork
                .Select<Role>()
                .To<RoleView>()
                .OrderByDescending(role => role.Id);
        }
        public RoleView GetView(Int32 id)
        {
            RoleView role = UnitOfWork.GetAs<Role, RoleView>(id);
            if (role != null)
            {
                role.Permissions.SelectedIds = UnitOfWork
                    .Select<RolePermission>()
                    .Where(rolePermission => rolePermission.RoleId == role.Id)
                    .Select(rolePermission => rolePermission.PermissionId)
                    .ToList();

                SeedPermissions(role);
            }

            return role;
        }

        public void Create(RoleView view)
        {
            Role role = UnitOfWork.To<Role>(view);
            foreach (Int32 permissionId in view.Permissions.SelectedIds)
                role.Permissions.Add(new RolePermission { RoleId = role.Id, PermissionId = permissionId });

            UnitOfWork.Insert(role);
            UnitOfWork.Commit();
        }
        public void Edit(RoleView view)
        {
            Role role = UnitOfWork.Get<Role>(view.Id);
            EditRolePermissions(role, view);
            EditRole(role, view);

            UnitOfWork.Commit();

            Authorization.Provider.Refresh();
        }
        public void Delete(Int32 id)
        {
            RemoveRoleFromAccounts(id);
            DeleteRolePermissions(id);
            DeleteRole(id);

            UnitOfWork.Commit();

            Authorization.Provider.Refresh();
        }

        private void EditRole(Role role, RoleView view)
        {
            role.Title = view.Title;

            UnitOfWork.Update(role);
        }
        private void EditRolePermissions(Role role, RoleView view)
        {
            List<Int32> selectedPermissions = view.Permissions.SelectedIds.ToList();

            foreach (RolePermission rolePermission in role.Permissions.ToArray())
                if (!selectedPermissions.Remove(rolePermission.PermissionId))
                    UnitOfWork.Delete(rolePermission);

            foreach (Int32 permissionId in selectedPermissions)
                UnitOfWork.Insert(new RolePermission { RoleId = role.Id, PermissionId = permissionId });
        }

        private void DeleteRole(Int32 id)
        {
            UnitOfWork.Delete<Role>(id);
        }
        private void DeleteRolePermissions(Int32 roleId)
        {
            IQueryable<RolePermission> rolePermissions = UnitOfWork
                .Select<RolePermission>()
                .Where(rolePermission => rolePermission.RoleId == roleId);

            foreach (RolePermission rolePermission in rolePermissions)
                UnitOfWork.Delete(rolePermission);
        }
        private void RemoveRoleFromAccounts(Int32 roleId)
        {
            IQueryable<Account> accountsWithRole = UnitOfWork
                .Select<Account>()
                .Where(account => account.RoleId == roleId);

            foreach (Account account in accountsWithRole)
            {
                account.RoleId = null;
                UnitOfWork.Update(account);
            }
        }
    }
}
