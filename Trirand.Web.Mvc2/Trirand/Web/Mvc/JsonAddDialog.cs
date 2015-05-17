namespace Trirand.Web.Mvc
{
    using System;
    using System.Collections;
    using System.Web.Script.Serialization;

    internal class JsonAddDialog
    {
        private JQGrid _grid;
        private Hashtable _jsonValues = new Hashtable();

        public JsonAddDialog(JQGrid grid)
        {
            this._grid = grid;
        }

        public string Process()
        {
            AddDialogSettings addDialogSettings = this._grid.AddDialogSettings;
            if (addDialogSettings.TopOffset != 0)
            {
                this._jsonValues["top"] = addDialogSettings.TopOffset;
            }
            if (addDialogSettings.LeftOffset != 0)
            {
                this._jsonValues["left"] = addDialogSettings.LeftOffset;
            }
            if (addDialogSettings.Width != 300)
            {
                this._jsonValues["width"] = addDialogSettings.Width;
            }
            if (addDialogSettings.Height != 300)
            {
                this._jsonValues["height"] = addDialogSettings.Height;
            }
            if (addDialogSettings.Modal)
            {
                this._jsonValues["modal"] = true;
            }
            if (!addDialogSettings.Draggable)
            {
                this._jsonValues["drag"] = false;
            }
            if (!string.IsNullOrEmpty(addDialogSettings.Caption))
            {
                this._jsonValues["addCaption"] = addDialogSettings.Caption;
            }
            if (!string.IsNullOrEmpty(addDialogSettings.SubmitText))
            {
                this._jsonValues["bSubmit"] = addDialogSettings.SubmitText;
            }
            if (!string.IsNullOrEmpty(addDialogSettings.CancelText))
            {
                this._jsonValues["bCancel"] = addDialogSettings.CancelText;
            }
            if (!string.IsNullOrEmpty(addDialogSettings.LoadingMessageText))
            {
                this._jsonValues["processData"] = addDialogSettings.LoadingMessageText;
            }
            if (addDialogSettings.CloseAfterAdding)
            {
                this._jsonValues["closeAfterAdd"] = addDialogSettings.CloseAfterAdding;
            }
            if (!addDialogSettings.ClearAfterAdding)
            {
                this._jsonValues["clearAfterAdd"] = false;
            }
            if (!addDialogSettings.ReloadAfterSubmit)
            {
                this._jsonValues["reloadAfterSubmit"] = false;
            }
            if (!addDialogSettings.Resizable)
            {
                this._jsonValues["resize"] = false;
            }
            this._jsonValues["recreateForm"] = true;
            string json = new JavaScriptSerializer().Serialize(this._jsonValues);
            ClientSideEvents clientSideEvents = this._grid.ClientSideEvents;
            return JsonUtil.RenderClientSideEvent(JsonUtil.RenderClientSideEvent(JsonUtil.RenderClientSideEvent(JsonUtil.RenderClientSideEvent(JsonUtil.RenderClientSideEvent(json, "beforeShowForm", clientSideEvents.BeforeAddDialogShown), "afterShowForm", clientSideEvents.AfterAddDialogShown), "afterComplete", clientSideEvents.AfterAddDialogRowInserted), "errorTextFormat", "function(data) { return 'Error: ' + data.responseText }"), "editData", string.Format("{{ __RequestVerificationToken: jQuery('input[name=__RequestVerificationToken]').val() }}", this._grid.ID));
        }
    }
}

