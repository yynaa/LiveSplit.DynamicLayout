using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.UI.Components
{
    public partial class DynamicLayoutSettings : UserControl
    {
        public LayoutMode Mode { get; set; }

        public DynamicLayoutSettings()
        {
            //InitializeComponent();
        }

        private void DynamicLayoutSettings_Load(object sender, EventArgs e)
        {

        }

        public void SetSettings(XmlNode node)
        {
            
        }

        public XmlNode GetSettings(XmlDocument document)
        {
            var parent = document.CreateElement("Settings");
            CreateSettingsNode(document, parent);
            return parent;
        }

        public int GetSettingsHashCode()
        {
            return CreateSettingsNode(null, null);
        }

        private int CreateSettingsNode(XmlDocument document, XmlElement parent)
        {
            return SettingsHelper.CreateSetting(document, parent, "Version", "1");
        }
    }
}
