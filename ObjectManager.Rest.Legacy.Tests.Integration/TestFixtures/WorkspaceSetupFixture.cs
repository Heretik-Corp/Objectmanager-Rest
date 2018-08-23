using ObjectManager.Rest.Tests.Integration.Common.TestFixtures;
using Xunit;

namespace ObjectManager.Rest.Legacy.Tests.Integration.TestFixtures
{
    [CollectionDefinition(CollectionName)]
    public class WorkspaceSetupFixture : WorkspaceSetupFixtureHelper, ICollectionFixture<WorkspaceSetupFixture>
    {

    }
}
