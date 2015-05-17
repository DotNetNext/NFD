namespace Trirand.Web.Mvc
{
    using System;
    using System.Runtime.CompilerServices;

    public class SearchDialogSettings
    {
        public SearchDialogSettings()
        {
            bool flag;
            this.TopOffset = this.LeftOffset = 0;
            this.Width = this.Height = 300;
            this.ValidateInput = flag = false;
            this.Modal = this.MultipleSearch = flag;
            this.Draggable = true;
            this.FindButtonText = this.ResetButtonText = "";
        }

        public bool Draggable { get; set; }

        public string FindButtonText { get; set; }

        public int Height { get; set; }

        public int LeftOffset { get; set; }

        public bool Modal { get; set; }

        public bool MultipleSearch { get; set; }

        public string ResetButtonText { get; set; }

        public bool Resizable { get; set; }

        public int TopOffset { get; set; }

        public bool ValidateInput { get; set; }

        public int Width { get; set; }
    }
}

