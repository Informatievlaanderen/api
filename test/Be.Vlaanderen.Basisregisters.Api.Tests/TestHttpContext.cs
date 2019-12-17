namespace Be.Vlaanderen.Basisregisters.Api.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security.Claims;
    using System.Text;
    using System.Threading;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.AspNetCore.Http.Internal;
    using Newtonsoft.Json;

    public class TestHttpContext : HttpContext, IDisposable
    {
        private readonly MemoryStream _responseStream;

        public override IFeatureCollection Features { get; }
        public override HttpRequest Request { get; }
        public override HttpResponse Response { get; }
        public override ConnectionInfo Connection { get; }
        public override WebSocketManager WebSockets { get; }
        public override ClaimsPrincipal User { get; set; }
        public override IDictionary<object, object> Items { get; set; }
        public override IServiceProvider RequestServices { get; set; }
        public override CancellationToken RequestAborted { get; set; }
        public override string TraceIdentifier { get; set; }
        public override ISession Session { get; set; }

        public TestHttpContext()
        {
            _responseStream = new MemoryStream();

            // if this new DefaultHttpContext thing ever causes issues, add an Initialize method
            // to this TestHttpContextClass which properly sets up the response with TestHttpContext.
            Response = new DefaultHttpResponse(new DefaultHttpContext()) { Body = _responseStream };
        }

        public string ReadResponseBody() => Encoding.UTF8.GetString(_responseStream.ToArray());

        public T ReadJsonResponseBody<T>() => JsonConvert.DeserializeObject<T>(ReadResponseBody());

        public override void Abort() => throw new NotImplementedException();

        public void Dispose() => _responseStream?.Dispose();
    }
}
