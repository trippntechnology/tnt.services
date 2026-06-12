using System.Diagnostics;
using System.Text;
using TNT.ArgumentParser;
using TNT.Commons;

namespace TNT.Updater;

/// <summary>
/// Represents the command-line arguments for the updater application.
/// </summary>
public class Arguments : ArgumentParser.ArgumentParser
{
    /// <summary>
    /// Argument key for the application executable.
    /// </summary>
    private const string APPLICATION = "a";
    /// <summary>
    /// Argument key for the application ID.
    /// </summary>
    private const string APP_ID = "i";
    /// <summary>
    /// Argument key for the application password.
    /// </summary>
    private const string APP_PASSWORD = "p";
    /// <summary>
    /// Argument key for silent mode.
    /// </summary>
    private const string SILENT_MODE = "s";
    /// <summary>
    /// Argument key for the service endpoint.
    /// </summary>
    private const string ENDPOINT = "e";

    /// <summary>
    /// Argument for the application executable file.
    /// </summary>
    private FileArgument executableArgument = new FileArgument(APPLICATION, "Application to update", true, true);
    /// <summary>
    /// Argument for the application ID.
    /// </summary>
    private GuidArgument applicationIdArgument = new GuidArgument(APP_ID, "Application ID", true);
    /// <summary>
    /// Argument for the application password.
    /// </summary>
    private StringArgument applicationPasswordArgument = new StringArgument(APP_PASSWORD, "Application password", true);
    /// <summary>
    /// Argument for silent mode.
    /// </summary>
    private FlagArgument isSilentModeArgument = new FlagArgument(SILENT_MODE, "Hides dialog when latest version is installed");
    /// <summary>
    /// Argument for the base URI to the service.
    /// </summary>
    private UriArgument baseUriArgument = new UriArgument(ENDPOINT, "Base Uri to the service", true);

    /// <summary>
    /// Gets the executable file path.
    /// </summary>
    public string Executable => executableArgument.Value!;
    /// <summary>
    /// Gets the application ID.
    /// </summary>
    public Guid ApplicationId => applicationIdArgument.Value ?? Guid.Empty;
    /// <summary>
    /// Gets the application password.
    /// </summary>
    public string ApplicationPassword => applicationPasswordArgument.Value!;
    /// <summary>
    /// Gets a value indicating whether silent mode is enabled.
    /// </summary>
    public bool IsSilentMode => isSilentModeArgument.Value;
    /// <summary>
    /// Gets the base URI to the service.
    /// </summary>
    public Uri BaseUri => baseUriArgument.Value!;

    /// <summary>
    /// Gets the company name from the executable file version info.
    /// </summary>
    public string CompanyName { get; private set; } = string.Empty;
    /// <summary>
    /// Gets the product name from the executable file version info.
    /// </summary>
    public string ProductName { get; private set; } = string.Empty;
    /// <summary>
    /// Gets the file version from the executable file version info.
    /// </summary>
    public Version FileVersion { get; private set; } = Version.Parse("0.0.0");
    /// <summary>
    /// Gets the process name from the executable file path.
    /// </summary>
    public string? ProcessName => Path.GetFileNameWithoutExtension(Executable);

    /// <summary>
    /// Initializes a new instance of the <see cref="Arguments"/> class.
    /// </summary>
    public Arguments() : base()
    {
        Add(executableArgument);
        Add(applicationIdArgument);
        Add(applicationPasswordArgument);
        Add(isSilentModeArgument);
        Add(baseUriArgument);
    }

    /// <summary>
    /// Returns the usage string for the arguments, including the exception message.
    /// </summary>
    /// <param name="ex">The exception to include in the usage output.</param>
    /// <returns>The usage string.</returns>
    public string Usage(Exception ex)
    {
        var sb = new StringBuilder(ex.Message);
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine(base.Usage());
        return sb.ToString();
    }

    /// <summary>
    /// Parses the command-line arguments and extracts file version info.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    /// <param name="swallowException">Whether to swallow exceptions during parsing.</param>
    /// <returns>True if parsing is successful; otherwise, false.</returns>
    public override bool Parse(string[] args, bool swallowException = true)
    {
        base.Parse(args, false);

        string executable = Executable ?? throw new ArgumentException();
        FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(executable);
        CompanyName = fvi.CompanyName ?? string.Empty;
        ProductName = fvi.ProductName ?? string.Empty;
        FileVersion = fvi.FileVersion?.Let(it => Version.Parse(it)) ?? Version.Parse("0.0.0");

        return true;
    }
}
