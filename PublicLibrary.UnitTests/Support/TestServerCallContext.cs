using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;

namespace PublicLibrary.UnitTests.Support
{
    // Minimal ServerCallContext implementation for unit tests
    public class TestServerCallContext : ServerCallContext
    {
        public static ServerCallContext Create()
        {
            return new TestServerCallContext();
        }

        protected override string MethodCore => "test";
        protected override string HostCore => "localhost";
        protected override string PeerCore => "peer";
        protected override DateTime DeadlineCore => DateTime.UtcNow.AddMinutes(1);
        protected override Metadata RequestHeadersCore => new Metadata();
        protected override CancellationToken CancellationTokenCore => CancellationToken.None;
        protected override Metadata ResponseTrailersCore => new Metadata();
        protected override Status StatusCore { get; set; }
        protected override WriteOptions WriteOptionsCore { get; set; }
        protected override AuthContext AuthContextCore => new AuthContext("test", new Dictionary<string, List<Grpc.Core.AuthProperty>>());

        protected override ContextPropagationToken CreatePropagationTokenCore(ContextPropagationOptions options) => throw new NotSupportedException();
        protected override Task WriteResponseHeadersAsyncCore(Metadata responseHeaders) => Task.CompletedTask;
    }
}
