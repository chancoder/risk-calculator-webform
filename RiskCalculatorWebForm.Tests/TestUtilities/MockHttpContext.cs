using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;


namespace RiskCalculatorWebForm.Tests.TestUtilities
{
    /// <summary>
    /// A simple in-memory ISession implementation for unit testing
    /// </summary>
    public class MockSession : ISession
    {
        private readonly Dictionary<string, byte[]> _store = new Dictionary<string, byte[]>();

        public bool IsAvailable => true;
        public string Id => "MockSessionId";
        public IEnumerable<string> Keys => _store.Keys;

        public void Clear() => _store.Clear();

        public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public void Remove(string key) => _store.Remove(key);

        public void Set(string key, byte[] value) => _store[key] = value;

        public bool TryGetValue(string key, out byte[] value) => _store.TryGetValue(key, out value);
    }
    /// <summary>
    /// Utility class for creating mock HttpContext objects for unit testing
    /// </summary>
    public static class MockHttpContext
    {
        public static HttpContext CreateMockHttpContext()
        {
            var context = new DefaultHttpContext();

            // Setup request
            context.Request.Method = "GET";
            context.Request.Scheme = "http";
            context.Request.Host = new HostString("localhost", 12345);
            context.Request.Path = "/Test.aspx";
            context.Request.QueryString = QueryString.Empty;

            // Setup session
            var session = new MockSession();
            session.SetString("SessionID", "TestSessionId12345");
            context.Features.Set<ISessionFeature>(new SessionFeature { Session = session });

            return context;
        }

        public static HttpContext CreateMockHttpContextWithSession(Dictionary<string, object> sessionData = null)
        {
            var context = new DefaultHttpContext();

            var session = new MockSession();
            session.SetString("SessionID", "TestSessionId12345");

            // Setup session with data
            if (sessionData != null)
            {
                foreach (var item in sessionData)
                {
                    if (item.Value is string strVal)
                        session.SetString(item.Key, strVal);
                    else
                        session.SetString(item.Key, System.Text.Json.JsonSerializer.Serialize(item.Value));
                }
            }

            context.Features.Set<ISessionFeature>(new SessionFeature { Session = session });

            return context;
        }

        // Minimal ISessionFeature implementation
        private class SessionFeature : ISessionFeature
        {
            public ISession Session { get; set; }
        }
    }
}
