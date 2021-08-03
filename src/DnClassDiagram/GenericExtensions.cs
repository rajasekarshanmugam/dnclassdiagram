using System.Collections.Generic;
using System.CommandLine;
using System.Runtime.CompilerServices;

namespace System
{
	/// <summary>
	/// </summary>
	public static class GenericExtensions
	{
		/// <summary>
		/// for each helper
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">The list.</param>
		/// <param name="callback">The callback.</param>
		public static void ForEach<T>(this IEnumerable<T> list, Action<T> callback)
		{
			if (list != null && callback != null)
			{
				foreach (var item in list)
				{
					callback.Invoke(item);
				}
			}
		}

		/// <summary>
		/// for each helper
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">The list.</param>
		/// <param name="callback">The callback.</param>
		public static void ForEach<T>(this IEnumerable<T> list, Action<T, int> callback)
		{
			if (list != null && callback != null)
			{
				var index = 0;
				foreach (var item in list)
				{
					callback.Invoke(item, index);
				}
			}
		}

		/// <summary>
		/// Writes the line to console interface.
		/// </summary>
		/// <param name="iconsole">The iconsole.</param>
		/// <param name="message">The message.</param>
		public static void WriteLine(this IConsole iconsole, string message)
		{
			iconsole.Out.Write(message);
			iconsole.Out.Write(Environment.NewLine);
		}

		/// <summary>
		/// Coalesces the specified strings.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="source2">The source2.</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string Coalesce(this string source, string source2)
			=> !source.IsNullOrEmpty() ? source : source2;

		/// <summary>
		/// Coalesces the specified strings.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="source2">The source2.</param>
		/// <param name="source3">The source3.</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string Coalesce(this string source, string source2, string source3)
			=> !source.IsNullOrEmpty() ? source : !source2.IsNullOrEmpty() ? source2 : source3;

		/// <summary>
		/// Determines whether [is null or empty].
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns><c>true</c> if [is null or empty] [the specified source]; otherwise, <c>false</c>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNullOrEmpty(this string source) => string.IsNullOrEmpty(source);

		/// <summary>
		/// Determines whether [is null or white space].
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns>
		/// <c>true</c> if [is null or white space] [the specified source]; otherwise, <c>false</c>.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNullOrWhiteSpace(this string source) => string.IsNullOrWhiteSpace(source);
	}
}
