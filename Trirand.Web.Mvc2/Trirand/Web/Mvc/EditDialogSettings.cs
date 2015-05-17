namespace Trirand.Web.Mvc
{
    using System;
    using System.Runtime.CompilerServices;

    public class EditDialogSettings
    {
        public EditDialogSettings()
        {
            bool flag2;
            string str;
            string str2;
            this.TopOffset = this.LeftOffset = 0;
            this.Width = this.Height = 300;
            this.Modal = this.CloseAfterEditing = false;
            this.ReloadAfterSubmit = flag2 = true;
            this.Resizable = this.Draggable = flag2;
            this.LoadingMessageText = str = "";
            this.CancelText = str2 = str;
            this.Caption = this.SubmitText = str2;
        }

        public string CancelText { get; set; }

        public string Caption { get; set; }

        public bool CloseAfterEditing { get; set; }

        public bool Draggable { get; set; }

        public int Height { get; set; }

        public int LeftOffset { get; set; }

        public string LoadingMessageText { get; set; }

        public bool Modal { get; set; }

        public bool ReloadAfterSubmit { get; set; }

        public bool Resizable { get; set; }

        public string SubmitText { get; set; }

        public int TopOffset { get; set; }

        public int Width { get; set; }
    }
}

