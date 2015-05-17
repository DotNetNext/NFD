namespace Trirand.Web.Mvc
{
    using System;
    using System.Collections;
    using System.Web.Script.Serialization;

    internal class JsonViewRowDialog
    {
        private JQGrid _grid;
        private JavaScriptSerializer _json = new JavaScriptSerializer();
        private Hashtable _jsonValues = new Hashtable();

        public JsonViewRowDialog(JQGrid grid)
        {
            this._grid = grid;
        }

        public string Process()
        {
            ViewRowDialogSettings viewRowDialogSettings = this._grid.ViewRowDialogSettings;
            if (viewRowDialogSettings.TopOffset != 0)
            {
                this._jsonValues["top"] = viewRowDialogSettings.TopOffset;
            }
            if (viewRowDialogSettings.LeftOffset != 0)
            {
                this._jsonValues["left"] = viewRowDialogSettings.LeftOffset;
            }
            if (viewRowDialogSettings.Width != 300)
            {
                this._jsonValues["width"] = viewRowDialogSettings.Width;
            }
            if (viewRowDialogSettings.Height != 300)
            {
                this._jsonValues["height"] = viewRowDialogSettings.Height;
            }
            if (viewRowDialogSettings.Modal)
            {
                this._jsonValues["modal"] = true;
            }
            if (!viewRowDialogSettings.Draggable)
            {
                this._jsonValues["drag"] = false;
            }
            if (!string.IsNullOrEmpty(viewRowDialogSettings.Caption))
            {
                this._jsonValues["editCaption"] = viewRowDialogSettings.Caption;
            }
            if (!string.IsNullOrEmpty(viewRowDialogSettings.SubmitText))
            {
                this._jsonValues["bSubmit"] = viewRowDialogSettings.SubmitText;
            }
            if (!string.IsNullOrEmpty(viewRowDialogSettings.CancelText))
            {
                this._jsonValues["bCancel"] = viewRowDialogSettings.CancelText;
            }
            if (!viewRowDialogSettings.Resizable)
            {
                this._jsonValues["resize"] = false;
            }
            return new JavaScriptSerializer().Serialize(this._jsonValues);
        }
    }
}

