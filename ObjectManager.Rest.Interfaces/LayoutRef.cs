namespace ObjectManager.Rest.Interfaces
{
    public class LayoutRef
    {
        public LayoutRef(string name, int artifactId)
        {
            this.Name = name;
            this.ArtifactId = artifactId;
        }

        public int ArtifactId { get; }
        public string Name { get; }
    }
}