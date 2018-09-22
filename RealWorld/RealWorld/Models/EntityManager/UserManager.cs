using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealWorld.Models.DB;
using RealWorld.Models.ViewModel;

namespace RealWorld.Models.EntityManager
{
    public class UserManager
    {
        // The AddUserAccount() is a method that inserts data to the database.

        public void AddUserAccount(UserSignUpView user)
        {
            using (DemoDBEntities db = new DemoDBEntities())
            {
                SYSUser SU = new SYSUser
                {
                    LoginName = user.LoginName,
                    PasswordEncryptedText = user.Password,
                    RowCreatedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1,
                    RowModifiedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1,
                    RowCreatedDateTime = DateTime.Now,
                    RowModifiedDateTime = DateTime.Now
                };
                db.SYSUsers.Add(SU);
                db.SaveChanges();

                SYSUserProfile SUP = new SYSUserProfile
                {
                    SYSUserID = SU.SYSUserID,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Gender = user.Gender,
                    RowCreatedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1,
                    RowModifiedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1,
                    RowCreatedDateTime = DateTime.Now,
                    RowModifiedDateTime = DateTime.Now
                };
                db.SYSUserProfiles.Add(SUP);
                db.SaveChanges();

                if(user.LOOKUPRoleID>0)
                {
                    SYSUserRole SUR = new SYSUserRole
                    {
                        LOOKUPRoleID = user.LOOKUPRoleID,
                        SYSUserID = user.SYSUserID,
                        IsActive = true,
                        RowCreatedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1,
                        RowModifiedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1,
                        RowCreatedDateTime = DateTime.Now,
                        RowModifiedDateTime = DateTime.Now
                    };
                    db.SYSUserRoles.Add(SUR);
                    db.SaveChanges();
                }
            }
        }

        //The IsLoginNameExist() checks the database for an existing data using LINQ syntax.

        public bool IsLoginNameExist(string loginName)
        {
            using (DemoDBEntities db = new DemoDBEntities())
            {
                return db.SYSUsers.Where(x => x.LoginName.Equals(loginName)).Any();
            }
        }

        //Gets the corresponding password from the database for a particular user using LINQ query.

        public string GetUserPassword(string loginName)
        {
            using (DemoDBEntities db = new DemoDBEntities())
            {
                var user = db.SYSUsers.Where(x => x.LoginName.ToLower().Equals(loginName));
                if(user.Any())
                {
                    return user.FirstOrDefault().PasswordEncryptedText;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        //checks for the existing records in the “SYSUser” table and then validates if the corresponding
        //user has roles assigned to it.
        public bool IsUserInRole(string loginName, string roleName)
        {
            using (DemoDBEntities db = new DemoDBEntities())
            {
                SYSUser SU = db.SYSUsers.Where(x => x.LoginName.ToLower().Equals(loginName))?.FirstOrDefault();
                if(SU!=null)
                {
                    var roles = from q in db.SYSUserRoles
                                join r in db.LOOKUPRoles on q.LOOKUPRoleID equals r.LOOKUPRoleID
                                where r.RoleName.Equals(roleName) && q.SYSUserID.Equals(SU.SYSUserID)
                                select r.RoleName;

                    if(roles!=null)
                    {
                        return roles.Any();
                    }
                }
                
            }
            return false;
        }
    }
}