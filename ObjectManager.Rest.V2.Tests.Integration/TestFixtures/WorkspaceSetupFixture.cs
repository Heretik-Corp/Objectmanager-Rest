using Relativity.API;
using Relativity.Test.Helpers;
using System;
using Xunit;

namespace ObjectManager.Rest.V2.Tests.Integration
{
    [CollectionDefinition(CollectionName)]
    public class WorkspaceSetupFixture : ICollectionFixture<WorkspaceSetupFixture>, IDisposable
    {
        public const string CollectionName = "Workspace Collection";
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
            this.WorkspaceId = ObjectManager.Rest.Tests.Integration.Common.TestFixtures.WorkspaceSetupFixture.SetupEnvironment(helper, _workspaceName);
        }

        public void Dispose()
        {
            ObjectManager.Rest.Tests.Integration.Common.TestFixtures.WorkspaceSetupFixture.TearDown(this.WorkspaceId, this.Helper);
        }
    }
}
