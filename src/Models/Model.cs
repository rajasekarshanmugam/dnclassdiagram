using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DnClassDiagram.Models
{
	public record DNClassInfo(string Name)
	{
		public IEnumerable<DNPropertyInfo> Properties { get; set; }

		public static DNClassInfo FromType(Type t)
		{
			return new DNClassInfo(t.Name)
			{
				Properties = t.GetProperties().Select(p => DNPropertyInfo.FromProperty(p))
			};
		}
	}

	public record DNPropertyInfo(string Name, string Type, bool CanRead, bool CanWrite)
	{
		public static DNPropertyInfo FromProperty(PropertyInfo t)
		{
			return new DNPropertyInfo(t.Name, t.PropertyType.Name, t.CanRead, t.CanWrite);
		}
	}
}
