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


        public List<LOOKUPAvailableRole> GetAllRoles()
        {
            using (DemoDBEntities db = new DemoDBEntities())
            {
                var roles = db.LOOKUPRoles.Select(x => new LOOKUPAvailableRole
                {
                    LOOKUPRoleID = x.LOOKUPRoleID,
                    RoleName = x.RoleName,
                    RoleDescription = x.RoleDescription
                }).ToList();

                return roles;
            }
        }


        public int GetUserID(string loginName)
        {
            using (DemoDBEntities db = new DemoDBEntities())
            {
                var user = db.SYSUsers.Where(x => x.LoginName.Equals(loginName));
                if(user.Any())
                {
                    return user.FirstOrDefault().SYSUserID;

                }
                return 0;
            }
        }


        public List<UserProfileView> GetAllUserProfiles()
        {
            List<UserProfileView> profiles = new List<UserProfileView>();
            using (DemoDBEntities db = new DemoDBEntities())
            {
                UserProfileView UPV;
                var users = db.SYSUsers.ToList();
                foreach(SYSUser s in db.SYSUsers)
                {
                    UPV = new UserProfileView
                    {
                        SYSUserID = s.SYSUserID,
                        LoginName = s.LoginName,
                        Password = s.PasswordEncryptedText
                    };

                    var SUP = db.SYSUserProfiles.Find(s.SYSUserID);
                    if(SUP!=null)
                    {
                        UPV.FirstName = SUP.FirstName;
                        UPV.LastName = SUP.LastName;
                        UPV.Gender = SUP.Gender;
                    }

                    var SUR = db.SYSUserRoles.Where(x => x.SYSUserID.Equals(s.SYSUserID));
                    if(SUR.Any())
                    {
                        var userRole = SUR.FirstOrDefault();
                        UPV.LOOKUPRoleID = userRole.LOOKUPRoleID;
                        UPV.RoleName = userRole.LOOKUPRole.RoleName;
                        UPV.IsRoleActive = userRole.IsActive;
                    }
                    profiles.Add(UPV);
                }
            }
            return profiles;
        }

        //gets all user profiles and roles.
        public UserDataView GetUserDataView(string loginName)
        {
            UserDataView UDV = new UserDataView();
            List<UserProfileView> profiles = GetAllUserProfiles();
            List<LOOKUPAvailableRole> roles = GetAllRoles();

            int? userAssignedRoleID = 0, userID = 0;
            string userGender = string.Empty;

            userID = GetUserID(loginName);

            using (DemoDBEntities db = new DemoDBEntities())
            {
                userAssignedRoleID = db.SYSUserRoles.Where(x => x.SYSUserID == userID)?.FirstOrDefault().LOOKUPRoleID;
                userGender = db.SYSUserProfiles.Where(x => x.SYSUserID == userID)?.FirstOrDefault().Gender;
            }
            List<Gender> genders = new List<Gender>
            {
                new Gender { Text = "Male", Value = "M" },
                new Gender { Text = "Female", Value = "F" }
            };

            UDV.UserProfile = profiles;
            UDV.UserRoles = new UserRoles { SelectedRoleID = userAssignedRoleID, UserRoleList = roles };
            UDV.UserGender = new UserGender { SelectedGender = userGender, Gender = genders };

            return UDV;
        }
    }
}