using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace TNT.Services.Service.Controllers
{
  public class BaseController : Controller
  {
    const string TEMP_FILE_PATH = "wwwroot/uploads";

    protected string GetVersion(byte[] package)
    {
      string version = String.Empty;
      string fileName = String.Empty;

      try
      {
        if (!Directory.Exists(TEMP_FILE_PATH))
        {
          Directory.CreateDirectory(TEMP_FILE_PATH);
        }

        fileName = Path.Combine(TEMP_FILE_PATH, Guid.NewGuid().ToString());
        System.IO.File.WriteAllBytes(fileName, package);
        version = FileVersionInfo.GetVersionInfo(fileName)?.FileVersion ?? String.Empty;
      }
      catch (Exception ex)
      {
        System.Diagnostics.Debug.WriteLine(ex.Message);
      }
      finally
      {
        if (!String.IsNullOrEmpty(fileName)) System.IO.File.Delete(fileName);
      }

      return version;
    }
  }
}
