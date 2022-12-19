using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using _4thtry.Context;
namespace _4thtry
{
    public class WebRoleProvider : RoleProvider
    {
        db_testEntities dbObj = new db_testEntities();
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            User usertb = new User();
            usertb = dbObj.Users.Where(x =>x.Name== username).SingleOrDefault();
            if (usertb.Name =="admin"&& usertb.Email == "admin@admin.com")
            {
               // var result = "admin";
                string[] result = { "admin" };
                return result;
            }
            else if (usertb != null)
            {
                string[] result = { "customer" };
                return result;
            }
            else
            {
                string[] result = { "visitor" };
                return result;
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }
        /*99 111 112 121 114 105 103 104 116 32 99 114 101 100 105 116 32 116 111 32 82 97 110 105 115 104*/
        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}