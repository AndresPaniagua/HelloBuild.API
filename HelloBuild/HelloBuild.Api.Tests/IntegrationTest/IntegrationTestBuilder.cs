using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;

namespace HelloBuild.Api.Tests.IntegrationTest
{
    public class IntegrationTestBuilder : IDisposable
    {
        protected HttpClient TestClient;
        private bool Disposed;

        protected IntegrationTestBuilder()
        {
            BootstrapTestingSuite();
        }

        protected void BootstrapTestingSuite()
        {
            Disposed = false;
            WebApplicationFactory<Startup> appFactory = new();
            TestClient = appFactory.CreateClient();
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
            {
                return;
            }

            if (disposing)
            {
                TestClient.Dispose();
            }

            Disposed = true;
        }
    }
}
