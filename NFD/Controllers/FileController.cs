using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using NFD.BLL.Common;
using NFD.Entities.Common;

namespace NFD.Controllers
{
    public class FileController : Controller
    {


        public ActionResult ShowFiles(int hid, PubEnum.AttaType type)
        {
            var model= AttaManager.GetAttasByHt_d(hid, type);
            return View(model);
        }


        [OutputCache(Duration = 0)]
        public JsonResult Upload()
        {
            HttpPostedFileBase file = Request.Files["uploadFile"];
            if (file != null)
            {
                string dir = "/Content/File/Temp";
                string root = Server.MapPath("~" + dir);
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
                file.SaveAs(root + "/" + file.FileName);
                return Json(new { isSuccess = true, path = dir + "/" + file.FileName });
            }
            return Json(new { isSuccess = false });
        }

        public void DelFile(string path)
        {
            System.IO.File.Delete(Server.MapPath("~" + path));
        }

        [HttpPost]
        public JsonResult RenameFile(string oldPath, string newPath)
        {
            var extension = System.IO.Path.GetExtension(oldPath);
            var dir = System.IO.Path.GetDirectoryName(oldPath);
            string fileName = newPath + "" + extension;
            newPath = Server.MapPath("~" + dir) + @"\" + fileName;
            oldPath = Server.MapPath(oldPath);
            string webPath = dir.Replace(@"\", @"/") + "/" + fileName;
            COM.Utility.FileHelper.Move(oldPath, newPath);
            return Json(new { fileName = fileName, webPath = webPath, path = newPath });
        }

    }
}
