using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DnClassDiagram.Models
{
	public record Model
	{
		public Dictionary<string, DNClassInfo> Classes { get; set; }
	}

	public record DNClassInfo(string Name)
	{
		public string Id { get; set; }
		public string FullName { get; set; }
		public Type Type { get; set; }

		public IEnumerable<DNPropertyInfo> Properties { get; set; }

		public static DNClassInfo FromType(Type t)
		{
			return new DNClassInfo(t.Name)
			{
				Id = t.FullName.Replace(".", "_").ToLowerInvariant(),
				Type = t,
				FullName = t.FullName,
				Properties = t.GetProperties().Select(p => DNPropertyInfo.FromProperty(p))
			};
		}
	}

	public record DNPropertyInfo(string Id, string Name, string Type, string RuntimeType, string RuntimeTypeId, bool CanRead, bool CanWrite)
	{
		public static DNPropertyInfo FromProperty(PropertyInfo t)
		{
			return new DNPropertyInfo(t.Name.ToLowerInvariant(), 
				t.Name, t.PropertyType.Name, 
				t.PropertyType.FullName, t.PropertyType.FullName.Replace(".","_").ToLowerInvariant(),
				t.CanRead, t.CanWrite);
		}
	}
}
