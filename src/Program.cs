using Microsoft.Extensions.DependencyInjection;
using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Help;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Threading.Tasks;

namespace DnClassDiagram
{
	/// <summary>
	/// </summary>
	partial class Program
	{
		/// <summary>
		/// The service provider
		/// </summary>
		private static IServiceProvider ServiceProvider = BuildServices();

		/// <summary>
		/// Builds the services.
		/// </summary>
		/// <returns></returns>
		private static IServiceProvider BuildServices()
		{
			var services = new ServiceCollection();
			//services.AddSingleton<HostService>();
			return services.BuildServiceProvider(true);
		}

		/// <summary>
		/// Entry point.
		/// </summary>
		/// <param name="args">The arguments.</param>
		/// <returns></returns>
		private async static Task<int> Main(string[] args)
		{
			Console.OutputEncoding = System.Text.Encoding.UTF8;

			var app = new RootCommand
			{
				TreatUnmatchedTokensAsErrors = true,
			};

			app.NewCommand
			(
				"export",
				"exports class diagram to output folder",
				usage: EXPORT_USAGE,
				options: new Option[]
				{
					new Option<string>(new[]{"-i","--inputfile"}, "input assembly path"){ IsRequired = true },
					new Option<string>(new[]{"-o", "--outputfile"}, "output file path"){ IsRequired = true },
					new Option<string>(new[]{"-d", "--dotexepath"}, "dot.exe file path"){ IsRequired = false },
				},
				handler: CommandHandler.Create<IConsole, string, string, string>(DoExport)
			);

			var returnValue = await new CommandLineBuilder(app)
				.UseDefaults()
				.UseHelpBuilder(context => new MyHelpBuilder(context.Console))
				.Build()
				.Parse(args)
				.InvokeAsync();

			Console.WriteLine("\n\n😊 Done.");
			return returnValue;
		}

		/// <summary>
		/// Write to console with color.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <param name="format">The format.</param>
		/// <param name="arguments">The arguments.</param>
		private static void ColorConsoleWriteLine(ConsoleColor color, string format, params object[] arguments)
		{
			Console.ForegroundColor = color;
			try
			{
				if (arguments != null && arguments.Length > 0)
				{
					Console.WriteLine(format, arguments);
				}
				else
				{
					Console.WriteLine(format);
				}
			}
			finally
			{
				Console.ResetColor();
			}
		}

		/// <summary>
		/// The hr
		/// </summary>
		private static readonly string HR = new string('-', 80);

		internal class MyHelpBuilder : HelpBuilder
		{
			public MyHelpBuilder(IConsole console)
				: base(console)
			{
			}

			public override void Write(ICommand command)
			{
				base.Write(command);

				if (command is CommandEx rc && !string.IsNullOrEmpty(rc.Usage))
				{
					var outs = base.Console.Out;

					outs.Write("Usage:");
					outs.Write(string.Format(rc.Usage, rc.Name));
				}
			}
		}
	}
}
