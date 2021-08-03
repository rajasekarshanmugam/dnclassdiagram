namespace ThreeClasses
{
	public class Class1
	{
		public string PublicString { get; set; }
		public Class2 Class2Instance { get; }
		public Class3 Class3Instance { get; }
	}

	public class Class2

	{
		public string PublicString { get; set; }
		public Class1 Class2Instance { get; }
		public Class3 Class3Instance { get; }
	}

	public class Class3

	{
		public string PublicString { get; set; }
		public Class1 Class1Instance { get; }
		public Class2 Class2Instance { get; }
	}
}
