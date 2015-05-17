namespace Trirand.Web.Mvc
{
    using System;
    using System.Runtime.CompilerServices;

    public class DeleteDialogSettings
    {
        public DeleteDialogSettings()
        {
            bool flag;
            string str;
            string str2;
            this.TopOffset = this.LeftOffset = 0;
            this.Width = this.Height = 300;
            this.Modal = false;
            this.ReloadAfterSubmit = flag = true;
            this.Resizable = this.Draggable = flag;
            this.LoadingMessageText = str = "";
            this.CancelText = str2 = str;
            this.Caption = this.SubmitText = str2;
        }

        public string CancelText { get; set; }

        public string Caption { get; set; }

        public string DeleteMessage { get; set; }

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

