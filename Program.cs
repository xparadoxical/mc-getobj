using System.Diagnostics;

if (args.Length is not 1 || string.IsNullOrWhiteSpace(args[0]))
{
	Console.WriteLine("Usage: mc-getobj <obj url>");
	return;
}

var arg = args[0]; //ex.: https://resources.download.minecraft.net/66/6615ab7f668384cf98b0cb034ce8e4c52eaf86b2
if (!arg.StartsWith("https://resources.download.minecraft.net/"))
{
	Console.WriteLine("That's not a valid Minecraft resource URL.");
	return;
}

var uri = new Uri(arg);

var pathParts = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
(var dirName, var file) = (pathParts[0], pathParts[1]);

string mcObjDir = Path.Combine(Environment.GetEnvironmentVariable("appdata"), ".minecraft", "assets", "objects");

string dir = Path.Combine(mcObjDir, dirName);
if (!Directory.Exists(dir))
	Directory.CreateDirectory(dir);

Console.WriteLine("Running curl...");
Console.WriteLine();

var curl = Process.Start(new ProcessStartInfo(@"C:\Windows\System32\curl.exe", $"-O {arg}")
{
	WorkingDirectory = dir,
	UseShellExecute = false,
	RedirectStandardOutput = true,
	RedirectStandardError = true,
	CreateNoWindow = true
})!;

var stdout = curl.StandardOutput.ReadToEnd();
var stderr = curl.StandardError.ReadToEnd();
curl.WaitForExit();
Console.Write(stdout);
Console.Write(stderr);
