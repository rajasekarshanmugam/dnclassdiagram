using RazorLight;
using System;
using System.CommandLine;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DnClassDiagram
{
	/// <summary>
	/// </summary>
	partial class Program
	{
		/// <summary>
		/// Does the export.
		/// </summary>
		/// <param name="console">The console.</param>
		/// <param name="inputfile">The inputfile.</param>
		/// <param name="outputfile">The outputfile.</param>
		/// <param name="dotexepath">The dotexepath.</param>
		/// <returns></returns>
		/// <exception cref="Exception">target file already exists, to prevent accidental overwrite, delete it manually - File={outputxlsx}</exception>
		private static async Task<int> DoExport(IConsole console, string inputfile, string outputfile, string dotexepath = null)
		{
			var returnValue = -1;
			try
			{
				inputfile = Path.GetFullPath(inputfile);
				console.WriteLine($"opening assembly - {inputfile}");

				// assume in path
				dotexepath ??= "dot.exe";

				var source = Assembly.LoadFrom(inputfile);
				var model = source.GetTypes()
					.Select(t => DnClassDiagram.Models.DNClassInfo.FromType(t));

				var currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
				var templatesDirectory = Path.Combine(currentDirectory, "Templates");

				var engine = new RazorLightEngineBuilder()
					.UseFileSystemProject(templatesDirectory)
					.SetOperatingAssembly(typeof(Program).Assembly)
					.UseMemoryCachingProvider()
					.Build();

				var dotoutputfile = $"{outputfile}.dot";
				var templatePath = "Default/index.cshtml";
				var result = await engine.CompileRenderAsync(templatePath, model);
				await File.WriteAllTextAsync(dotoutputfile, result);

				var dotCommand = Process.Start(dotexepath, $"-v -Tsvg -o\"{outputfile}\" \"{dotoutputfile}\"");
				dotCommand.WaitForExit();
				if (dotCommand.ExitCode != 0)
				{
					throw new Exception($"dot.exe failed with an error");
				}

				Process.Start(new ProcessStartInfo
				{
					FileName = outputfile,
					UseShellExecute = true,
					Verb = "open",
				});
			}
			catch (AggregateException ae)
			{
				ae.Handle((hre) =>
				{
					ColorConsoleWriteLine(ConsoleColor.Red, $"{HR}\nData : {hre.Data}\nHelpLink : {hre.HelpLink}\nHResult : {hre.HResult}\nInnerException : {hre.InnerException}\nMessage : {hre.Message}\nSource : {hre.Source}\nStackTrace : {hre.StackTrace}");
					return true;
				});
			}
			catch (Exception e)
			{
				ColorConsoleWriteLine(ConsoleColor.Red, $"{HR}\nData : {e.Data}\nHelpLink : {e.HelpLink}\nHResult : {e.HResult}\nInnerException : {e.InnerException}\nMessage : {e.Message}\nSource : {e.Source}\nStackTrace : {e.StackTrace}");
			}
			return returnValue;
		}

		/// <summary>
		/// The convert usage
		/// </summary>
		private const string EXPORT_USAGE =
@"
---- exports class diagram to output file
dnclassdiagram.exe export -i .\bin\output\assembly.dll -o .\classdiagram.png"
;
	}
}