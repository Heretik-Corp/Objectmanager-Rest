namespace ObjectManager.Rest.Tests.Integration.Common.TestFixtures
{
    public class ApplicationInstallContext
    {
        public string Name { get; private set; }
        public string Path { get; private set; }
        public ApplicationInstallContext() { }

        public static ApplicationInstallContext FieldTestPath
        {
            get
            {
                return new ApplicationInstallContext
                {
                    Name = "Object Manger Field Tests",
                    Path = @"..\..\..\applications\Field_Tests.rap"
                };
            }
        }
    }
}