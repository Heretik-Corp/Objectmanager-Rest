using ObjectManager.Rest.Tests.Integration.Common.TestFixtures;
using Xunit;

namespace ObjectManager.Rest.V1.Tests.Integration
{
    [CollectionDefinition(CollectionName)]
    public class WorkspaceSetupFixture : WorkspaceSetupFixtureHelper, ICollectionFixture<WorkspaceSetupFixture>
    {
    }
}
