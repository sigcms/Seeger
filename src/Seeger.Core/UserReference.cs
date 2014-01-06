using Seeger.Data.Mapping;
using Seeger.ComponentModel;
using Seeger.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger
{
    [Component]
    public class UserReference
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Nick { get; set; }

        public UserReference()
        {
        }

        public UserReference(int id, string userName, string nick)
        {
            Id = id;
            UserName = userName;
            Nick = nick;
        }

        public static UserReference From(User user)
        {
            return new UserReference
            {
                Id = user.Id,
                UserName = user.UserName,
                Nick = user.Nick
            };
        }

        public static UserReference System()
        {
            return new UserReference(0, "System", "System");
        }
    }
}
