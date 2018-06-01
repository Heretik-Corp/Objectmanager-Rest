using System;
using ObjectManager.Rest.Tests.Integration.Common.TestFixtures;
using Relativity.API;
using Relativity.Test.Helpers;
using Xunit;

namespace ObjectManager.Rest.Legacy.Tests.Integration.TestFixtures
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
            this.WorkspaceId = WorkspaceSetupFixtureHelper.SetupEnvironment(helper, _workspaceName);
        }

        public void Dispose()
        {
            WorkspaceSetupFixtureHelper.TearDown(this.WorkspaceId, this.Helper);
        }
    }
}
