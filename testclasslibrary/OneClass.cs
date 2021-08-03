using System;

namespace OneClass
{
	public class TheOnlyClass
	{
		public TheOnlyClass()
		{
		}

		public string PublicString { get; set; }
		public string PublicGetOnlyString { get; }
		public string PublicSetOnlyString { set {; } }
	}
}
