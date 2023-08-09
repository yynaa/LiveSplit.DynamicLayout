using LiveSplit.Model;
using System;

namespace LiveSplit.UI.Components
{
    public class DynamicLayoutFactory : IComponentFactory
    {
        public string ComponentName => "Dynamic Layout";

        public string Description => "Adds a new window that you can capture through OBS.";

        public ComponentCategory Category => ComponentCategory.Other;

        public IComponent Create(LiveSplitState state) => new DynamicLayout(state);

        public string UpdateName => ComponentName;

        public string UpdateURL => "";

        public string XMLURL => UpdateURL + "";

        public Version Version => Version.Parse("1.1.0");
    }
}
