namespace Trirand.Web.Mvc
{
    using System;
    using System.Runtime.CompilerServices;

    public class ViewRowDialogSettings
    {
        public ViewRowDialogSettings()
        {
            string str;
            this.TopOffset = this.LeftOffset = 0;
            this.Width = this.Height = 300;
            this.Modal = false;
            this.Resizable = this.Draggable = true;
            this.CancelText = str = "";
            this.Caption = this.SubmitText = str;
        }

        public string CancelText { get; set; }

        public string Caption { get; set; }

        public bool Draggable { get; set; }

        public int Height { get; set; }

        public int LeftOffset { get; set; }

        public bool Modal { get; set; }

        public bool Resizable { get; set; }

        public string SubmitText { get; set; }

        public int TopOffset { get; set; }

        public int Width { get; set; }
    }
}

