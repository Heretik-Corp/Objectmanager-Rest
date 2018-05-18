namespace ObjectManager.Rest.Interfaces
{
    public class LayoutRef
    {
        public LayoutRef() { }
        //public LayoutRef(string name) => this.Name = name;
        public LayoutRef(int artifactId) => this.ArtifactId = artifactId;

        public int ArtifactId { get; set; }
        //public string Name { get; set; }
    }
}