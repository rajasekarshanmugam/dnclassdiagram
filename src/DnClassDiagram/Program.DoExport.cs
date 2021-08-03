using RazorLight;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
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
		/// <param name="namespace">The namespace.</param>
		/// <param name="nameregex">The nameregex.</param>
		/// <returns></returns>
		/// <exception cref="System.Exception">dot.exe failed with an error</exception>
		/// <exception cref="Exception">target file already exists, to prevent accidental overwrite, delete it manually - File={outputxlsx}</exception>
		private static async Task<int> DoExport(IConsole console,
			string inputfile, string outputfile,
			string dotexepath = null,
			string @namespace = null,
			string nameregex = null
		)
		{
			var returnValue = -1;
			try
			{
				inputfile = Path.GetFullPath(inputfile);
				console.WriteLine($"opening assembly - {inputfile}");

				outputfile = Path.GetFullPath(outputfile);

				// assume in path
				dotexepath ??= "dot.exe";

				var source = Assembly.LoadFrom(inputfile);
				var types = (IEnumerable<Type>)source.GetExportedTypes();

				// ns filter
				if (!@namespace.IsNullOrEmpty())
				{
					types = types.Where(t => t.Namespace == @namespace);
				}

				if (!nameregex.IsNullOrEmpty())
				{
					var regex = new Regex(nameregex, RegexOptions.IgnoreCase | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
					types = types.Where(t => regex.IsMatch(t.FullName));
				}

				var classes = types
					.Select(t => DnClassDiagram.Models.DNClassInfo.FromType(t));
				var model = new DnClassDiagram.Models.Model
				{
					Classes = classes.ToDictionary(c => c.FullName, c => c)
				};

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
				if (dotCommand.ExitCode < 0)
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