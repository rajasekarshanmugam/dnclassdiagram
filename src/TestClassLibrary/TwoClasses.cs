namespace TwoClasses
{
	public class Class1
	{
		public string PublicString { get; set; }
		public Class2 Class2Instance { get; }
	}

	public class Class2

	{
		public string PublicString { get; set; }
		public Class1 Class2Instance { get; }
	}
}
