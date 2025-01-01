using System.Diagnostics;
using System.Text;
using TNT.ArgumentParser;
using TNT.Commons;

namespace TNT.Updater;

public class Arguments : ArgumentParser.ArgumentParser
{
  private const string APPLICATION = "a";
  private const string APP_ID = "i";
  private const string APP_PASSWORD = "p";
  private const string SILENT_MODE = "s";
  private const string ENDPOINT = "e";
  private const string WRITE_TO_CONSOLE = "c";

  private FlagArgument writeToConsoleArgument = new FlagArgument(WRITE_TO_CONSOLE, "Forces usage to be written to console");
  private FileArgument executableArgument = new FileArgument(APPLICATION, "Application to update", true, true);
  private GuidArgument applicationIdArgument = new GuidArgument(APP_ID, "Application ID", true);
  private StringArgument applicationPasswordArgument = new StringArgument(APP_PASSWORD, "Application password", true);
  private FlagArgument isSilentModeArgument = new FlagArgument(SILENT_MODE, "Hides dialog when latest version is installed");
  private UriArgument baseUriArgument = new UriArgument(ENDPOINT, "Base Uri to the service", true);

  public bool WriteToConsole => writeToConsoleArgument.Value;
  public string Executable => executableArgument.Value!;
  public Guid ApplicationId => applicationIdArgument.Value ?? Guid.Empty;
  public string ApplicationPassword => applicationPasswordArgument.Value!;
  public bool IsSilentMode => isSilentModeArgument.Value;
  public Uri BaseUri => baseUriArgument.Value!;

  public string CompanyName { get; private set; } = string.Empty;
  public string ProductName { get; private set; } = string.Empty;
  public Version FileVersion { get; private set; } = Version.Parse("0.0.0");
  public string? ProcessName => Path.GetFileNameWithoutExtension(Executable);

  public Arguments() : base()
  {
    Add(writeToConsoleArgument);
    Add(executableArgument);
    Add(applicationIdArgument);
    Add(applicationPasswordArgument);
    Add(isSilentModeArgument);
    Add(baseUriArgument);
  }

  public string Usage(Exception ex)
  {
    var sb = new StringBuilder(ex.Message);
    sb.AppendLine();
    sb.AppendLine();
    sb.AppendLine(base.Usage());
    return sb.ToString();
  }

  public override bool Parse(string[] args, bool swallowException = true)
  {
    base.Parse(args, false);

    string executable = Executable ?? throw new ArgumentException();
    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(executable);
    CompanyName = fvi.CompanyName ?? string.Empty;
    ProductName = fvi.ProductName ?? string.Empty;
    FileVersion = fvi.FileVersion?.let(it => Version.Parse(it)) ?? Version.Parse("0.0.0");

    return true;
  }
}
