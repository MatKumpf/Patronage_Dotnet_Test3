using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FileLibrary;
using System.IO;
using WebApplication.Models;
using System.Text;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public ActionResult Index(string filename)
        {
            if (filename != null)
            {
                ViewData["Path"] = filename.Replace("\\", "\\\\");
                return View();
            }
            else
            {
                ViewData["Path"] = "null";
                return View();
            }
        }

        public ActionResult ReturnJsonTree()
        {
            Tree tree = new Tree(new string[] { Directory.GetCurrentDirectory() });
            tree.StartBuildTree();
            List<FileLibrary.DataTreeModel> list = Tree.ReturnDataTree(tree);
            return Json(list);
        }

        public ActionResult ReturnJsonFile(string path)
        {
            FileModel fm = FileService.FileMetadata(path);
            List<FileModel> fmlist = new List<FileModel>();
            fmlist.Add(fm);
            return Json(fmlist);
        }
    }
}
