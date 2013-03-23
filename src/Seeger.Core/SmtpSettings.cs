using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger
{
    public class SmtpSettings
    {
        private GlobalSettingManager _manager;
        private const string _keyPrefix = "Seeger.Smtp.";

        public SmtpSettings(GlobalSettingManager manager)
        {
            _manager = manager;
        }

        public string SenderName
        {
            get
            {
                return _manager.GetValue(_keyPrefix + "SenderName");
            }
            set
            {
                _manager.SetValue(_keyPrefix + "SenderName", value);
            }
        }

        public string SenderEmail
        {
            get
            {
                return _manager.GetValue(_keyPrefix + "SenderEmail");
            }
            set
            {
                _manager.SetValue(_keyPrefix + "SenderEmail", value);
            }
        }

        public string Server
        {
            get
            {
                return _manager.GetValue(_keyPrefix + "Server");
            }
            set
            {
                _manager.SetValue(_keyPrefix + "Server", value);
            }
        }

        public int Port
        {
            get
            {
                return _manager.TryGetValue<int>(_keyPrefix + "Port", 25);
            }
            set
            {
                _manager.SetValue(_keyPrefix + "Port", value.ToString());
            }
        }

        public bool EnableSsl
        {
            get
            {
                return _manager.TryGetValue<bool>(_keyPrefix + "EnableSsl", false);
            }
            set
            {
                _manager.SetValue(_keyPrefix + "EnableSsl", value.ToString());
            }
        }

        public string AccountName
        {
            get
            {
                return _manager.GetValue(_keyPrefix + "AccountName");
            }
            set
            {
                _manager.SetValue(_keyPrefix + "AccountName", value);
            }
        }

        public string Password
        {
            get
            {
                return _manager.GetValue(_keyPrefix + "Password");
            }
            set
            {
                _manager.SetValue(_keyPrefix + "Password", value);
            }
        }
    }
}
