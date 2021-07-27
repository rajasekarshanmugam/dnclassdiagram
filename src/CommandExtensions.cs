using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;

namespace System.CommandLine
{
	/// <summary>
	/// </summary>
	/// <seealso cref="System.CommandLine.Command"/>
	public class CommandEx : Command
	{
		/// <summary>
		/// Gets or sets the usage.
		/// </summary>
		/// <value>The usage.</value>
		public string Usage { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="CommandEx"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="description">The description.</param>
		public CommandEx(string name, string description = null)
			: base(name, description)
		{
		}
	}

	/// <summary>
	/// </summary>
	public static class CommandExtensions
	{
		/// <summary>
		/// Creates new command.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <param name="name">The name.</param>
		/// <param name="description">The description.</param>
		/// <param name="usage">The usage.</param>
		/// <param name="options">The options.</param>
		/// <param name="handler">The handler.</param>
		/// <param name="validator">The validator.</param>
		/// <returns></returns>
		public static Command NewCommand(this Command command,
			string name,
			string description = null,
			string usage = null,
			IEnumerable<Option> options = null,
			ICommandHandler handler = null,
			ValidateSymbol<CommandResult> validator = null)
		{
			var newCommand = new CommandEx(name, description: description)
			{
				Usage = usage
			};

			if (options != null)
			{
				foreach (var option in options)
				{
					newCommand.AddOption(option);
				}
			}
			newCommand.Handler = handler;
			command.AddCommand(newCommand);

			if (validator != null)
			{
				newCommand.AddValidator(validator);
			}
			return newCommand;
		}
	}
}
