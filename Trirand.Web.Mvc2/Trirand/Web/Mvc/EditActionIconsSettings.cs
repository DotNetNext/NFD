namespace Trirand.Web.Mvc
{
    using System;
    using System.Runtime.CompilerServices;

    public class EditActionIconsSettings
    {
        public EditActionIconsSettings()
        {
            this.ShowEditIcon = true;
            this.ShowDeleteIcon = true;
            this.SaveOnEnterKeyPress = false;
        }

        public bool SaveOnEnterKeyPress { get; set; }

        public bool ShowDeleteIcon { get; set; }

        public bool ShowEditIcon { get; set; }
    }
}

