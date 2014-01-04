using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeger.Globalization;
using Seeger.Web;
using Seeger.Data;
using Seeger.Templates;
using Seeger.Data.Mapping;

namespace Seeger.Security
{
    [Class]
    public class User
    {
        private string _skinName;
        private Skin _skin;

        [EntityKey, HiloId]
        public virtual int Id { get; protected set; }
        
        public virtual string UserName { get; set; }

        public virtual string Nick { get; set; }
        
        public virtual string Password { get; protected set; }
        
        public virtual string Email { get; set; }

        public virtual int FailedPasswordAttemptCount { get; set; }

        public virtual DateTime? LastFailedPasswordAttemptTime { get; set; }

        public virtual DateTime? LastLoginTime { get; set; }

        public virtual string LastLoginIP { get; set; }

        public virtual bool IsSuperAdmin { get; protected set; }
        
        public virtual IList<Role> Roles { get; protected set; }

        [NotMapped]
        public virtual Skin Skin
        {
            get
            {
                if (_skin == null && !String.IsNullOrEmpty(_skinName))
                {
                    _skin = AdminSkins.Find(_skinName);
                }

                return _skin;
            }
            set
            {
                Require.NotNull(value, "value");
                _skin = value;
                _skinName = value.Name;
            }
        }

        public virtual string Language { get; set; }

        public User()
        {
            Email = String.Empty;
            Roles = new List<Role>();
        }

        public virtual void UpdatePassword(string newPassword)
        {
            Require.NotNullOrEmpty(newPassword, "newPassword");

            Password = AuthenticationService.HashPassword(newPassword);
        }

        public virtual bool HasPermission(string pluginName, string groupName, string permissionName)
        {
            return IsSuperAdmin || (Roles.Count > 0 && Roles.Any(it => it.HasPermission(pluginName, groupName, permissionName)));
        }

        public override string ToString()
        {
            return UserName;
        }
    }
}
