using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;


namespace TNT.Services.ApplicationDeployer
{
	class Program
	{
		static Parameters m_Parameters = null;

		static void Main(string[] args)
		{
			m_Parameters = new Parameters();

			if (!m_Parameters.ParseArgs(args))
			{
				return;
			}

			FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(m_Parameters.AppPath);

			Console.WriteLine();
			Console.WriteLine("{0,20}: {1}", "Application Path", m_Parameters.AppPath);
			Console.WriteLine("{0,20}: {1}", "URL Path", m_Parameters.URLPath);
			Console.WriteLine("{0,20}: {1}", "FTP Path", m_Parameters.FTPPath);
			Console.WriteLine("{0,20}: {1}", "Application ID", m_Parameters.AppID);
			Console.WriteLine("{0,20}: {1}", "Application Name", fvi.ProductName);
			Console.WriteLine("{0,20}: {1}", "Application Version", fvi.FileVersion);
			Console.WriteLine();

			if (!m_Parameters.RunUnattended)
			{
				Console.Write("Continue? ");

				ConsoleKeyInfo cki = Console.ReadKey();
				Console.WriteLine();

				if (cki.Key != ConsoleKey.Y)
				{
					return;
				}
			}

			try
			{
				Console.WriteLine();
				Console.Write("Uploading {0} to {1} ... ", m_Parameters.AppPath, m_Parameters.FTPPath);
				UploadFile(m_Parameters.Host, m_Parameters.AppPath, m_Parameters.FTPUser, m_Parameters.FTPPassword);
				Console.WriteLine("Done!");

				Console.WriteLine();
				Console.Write("Updating entry for {0} ... ", fvi.ProductName.Trim());
				SQLServer.AddUpdateApplication(m_Parameters.AppID.ToString(), fvi.ProductName, fvi.FileVersion, m_Parameters.URLPath);
				Console.WriteLine("Done!");
			}
			catch (Exception ex)
			{
				Console.WriteLine("Failed!");
				Console.WriteLine();
				Console.WriteLine(ex.Message);
				Console.WriteLine();
			}
		}

		/// <summary>
		/// Upload File to Specified FTP Url with username and password and Upload Directory if need to upload in sub folders
		/// </summary>
		/// <param name="FtpUrl">Base FtpUrl of FTP Server</param>
		/// <param name="fileName">Local Filename to Upload</param>
		/// <param name="userName">Username of FTP Server</param>
		/// <param name="password">Password of FTP Server</param>
		/// <param name="UploadDirectory">[Optional]Specify sub Folder if any</param>
		/// <returns>Status String from Server</returns>
		public static string UploadFile(string FtpUrl, string fileName, string userName, string password, string UploadDirectory = "")
		{
			string PureFileName = new FileInfo(fileName).Name;
			String uploadUrl = String.Format("ftp://{0}{1}/{2}", FtpUrl, UploadDirectory, PureFileName);
			FtpWebRequest req = (FtpWebRequest)FtpWebRequest.Create(uploadUrl);
			req.Proxy = null;
			req.Method = WebRequestMethods.Ftp.UploadFile;
			req.Credentials = new NetworkCredential(userName, password);
			req.UseBinary = true;
			req.UsePassive = true;
			byte[] data = File.ReadAllBytes(fileName);
			req.ContentLength = data.Length;
			Stream stream = req.GetRequestStream();
			stream.Write(data, 0, data.Length);
			stream.Close();
			FtpWebResponse res = (FtpWebResponse)req.GetResponse();
			return res.StatusDescription;
		}

		static void FTP()
		{
			string site = string.Concat("ftp://", m_Parameters.Host, Path.GetFileName(m_Parameters.AppPath));

			// Get the object used to communicate with the server.
			FtpWebRequest request = (FtpWebRequest)WebRequest.Create(site);
			request.Method = WebRequestMethods.Ftp.UploadFile;


			// This example assumes the FTP site uses anonymous logon.
			request.Credentials = new NetworkCredential(m_Parameters.FTPUser, m_Parameters.FTPPassword);

			// Copy the contents of the file to the request stream.
			StreamReader sourceStream = new StreamReader(m_Parameters.AppPath);
			byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
			sourceStream.Close();
			request.ContentLength = fileContents.Length;

			Stream requestStream = request.GetRequestStream();
			requestStream.Write(fileContents, 0, fileContents.Length);
			requestStream.Close();

			FtpWebResponse response = (FtpWebResponse)request.GetResponse();

			response.Close();
		}
	}
}
