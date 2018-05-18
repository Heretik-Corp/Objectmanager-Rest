using Relativity.API;
using Relativity.Test.Helpers;
using System;
using Xunit;

namespace ObjectManager.Rest.V2.Tests.Integration
{
    [CollectionDefinition(Rest.Tests.Integration.Common.TestFixtures.WorkspaceSetupFixtureHelper.CollectionName)]
    public class WorkspaceSetupFixture : ICollectionFixture<WorkspaceSetupFixture>, IDisposable
    {
        private readonly string _workspaceName = "Integration test";
        public int WorkspaceId { get; private set; }
        public IHelper Helper { get; private set; }

        public WorkspaceSetupFixture()
        {
            Setup();
        }

        protected virtual void Setup()
        {
            var helper = new TestHelper();
            this.Helper = helper;
            this.WorkspaceId = Rest.Tests.Integration.Common.TestFixtures.
                WorkspaceSetupFixtureHelper
                .SetupEnvironment(helper, _workspaceName);
        }

        public void Dispose()
        {
            Rest.Tests.Integration.Common.TestFixtures.WorkspaceSetupFixtureHelper.TearDown(this.WorkspaceId, this.Helper);
        }
    }
}
