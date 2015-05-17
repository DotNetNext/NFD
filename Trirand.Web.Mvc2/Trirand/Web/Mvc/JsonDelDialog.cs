namespace Trirand.Web.Mvc
{
    using System;
    using System.Collections;
    using System.Web.Script.Serialization;

    internal class JsonDelDialog
    {
        private JQGrid _grid;
        private Hashtable _jsonValues = new Hashtable();

        public JsonDelDialog(JQGrid grid)
        {
            this._grid = grid;
        }

        public string Process()
        {
            DeleteDialogSettings deleteDialogSettings = this._grid.DeleteDialogSettings;
            if (deleteDialogSettings.TopOffset != 0)
            {
                this._jsonValues["top"] = deleteDialogSettings.TopOffset;
            }
            if (deleteDialogSettings.LeftOffset != 0)
            {
                this._jsonValues["left"] = deleteDialogSettings.LeftOffset;
            }
            if (deleteDialogSettings.Width != 300)
            {
                this._jsonValues["width"] = deleteDialogSettings.Width;
            }
            if (deleteDialogSettings.Height != 300)
            {
                this._jsonValues["height"] = deleteDialogSettings.Height;
            }
            if (deleteDialogSettings.Modal)
            {
                this._jsonValues["modal"] = true;
            }
            if (!deleteDialogSettings.Draggable)
            {
                this._jsonValues["drag"] = false;
            }
            if (!string.IsNullOrEmpty(deleteDialogSettings.SubmitText))
            {
                this._jsonValues["bSubmit"] = deleteDialogSettings.SubmitText;
            }
            if (!string.IsNullOrEmpty(deleteDialogSettings.CancelText))
            {
                this._jsonValues["bCancel"] = deleteDialogSettings.CancelText;
            }
            if (!string.IsNullOrEmpty(deleteDialogSettings.LoadingMessageText))
            {
                this._jsonValues["processData"] = deleteDialogSettings.LoadingMessageText;
            }
            if (!string.IsNullOrEmpty(deleteDialogSettings.Caption))
            {
                this._jsonValues["caption"] = deleteDialogSettings.Caption;
            }
            if (!string.IsNullOrEmpty(deleteDialogSettings.DeleteMessage))
            {
                this._jsonValues["msg"] = deleteDialogSettings.DeleteMessage;
            }
            if (!deleteDialogSettings.ReloadAfterSubmit)
            {
                this._jsonValues["reloadAfterSubmit"] = false;
            }
            if (!deleteDialogSettings.Resizable)
            {
                this._jsonValues["resize"] = false;
            }
            this._jsonValues["recreateForm"] = true;
            string json = new JavaScriptSerializer().Serialize(this._jsonValues);
            ClientSideEvents clientSideEvents = this._grid.ClientSideEvents;
            return JsonUtil.RenderClientSideEvent(JsonUtil.RenderClientSideEvent(JsonUtil.RenderClientSideEvent(JsonUtil.RenderClientSideEvent(JsonUtil.RenderClientSideEvent(json, "beforeShowForm", clientSideEvents.BeforeDeleteDialogShown), "afterShowForm", clientSideEvents.AfterDeleteDialogShown), "afterComplete", clientSideEvents.AfterDeleteDialogRowDeleted), "errorTextFormat", "function(data) { return 'Error: ' + data.responseText }"), "delData", string.Format("{{ __RequestVerificationToken: jQuery('input[name=__RequestVerificationToken]').val() }}", this._grid.ID));
        }
    }
}

