using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger
{
    public class TaskQueueSettings
    {
        private const string _keyPrefix = "Seeger.TaskQueue.";
        private GlobalSettingManager _manager;

        public int IntervalInMinutes
        {
            get
            {
                return _manager.TryGetValue<int>(_keyPrefix + "IntervalInMinutes", 3);
            }
            set
            {
                Require.That(value > 0, "Interval should be at least 1 minue.");

                _manager.SetValue(_keyPrefix + "IntervalInMinutes", value.ToString());
            }
        }

        public bool Enabled
        {
            get
            {
                return _manager.TryGetValue<bool>(_keyPrefix + "Enabled", true);
            }
            set
            {
                _manager.SetValue(_keyPrefix + "Enabled", value.ToString());
            }
        }

        public TaskQueueSettings(GlobalSettingManager manager)
        {
            _manager = manager;
        }
    }
}
