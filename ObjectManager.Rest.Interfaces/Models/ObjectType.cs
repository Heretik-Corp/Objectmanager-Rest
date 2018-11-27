namespace ObjectManager.Rest.Interfaces
{
    public class ObjectType
    {
        public ObjectType(int artifactTypeId) => this.ArtifactTypeId = artifactTypeId;
        public int ArtifactTypeId { get; set; }
    }
}