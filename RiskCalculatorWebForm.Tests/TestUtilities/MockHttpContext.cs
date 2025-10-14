using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;


namespace RiskCalculatorWebForm.Tests.TestUtilities
{
    /// <summary>
    /// Utility class for creating mock HttpContext objects for unit testing
    /// </summary>
    public static class MockHttpContext
    {
        public static HttpContext CreateMockHttpContext()
        {
            var mockContext = new Mock<HttpContext>();
            var mockRequest = new Mock<HttpRequest>();
            var mockResponse = new Mock<HttpResponse>();
            var mockSession = new Mock<ISession>();
            var mockCookies = new Mock<IRequestCookieCollection>();
            var mockResponseCookies = new Mock<IResponseCookies>();

            // Setup request
            mockRequest.Setup(r => r.Query).Returns(new QueryCollection());
            mockRequest.Setup(r => r.Form).Returns(new FormCollection(new Dictionary<string, StringValues>()));
            mockRequest.Setup(r => r.Cookies).Returns(mockCookies.Object);
            mockRequest.Setup(r => r.Scheme).Returns("http");
            mockRequest.Setup(r => r.Host).Returns(new HostString("localhost:12345"));
            mockRequest.Setup(r => r.Path).Returns("/Test.aspx");

            // Setup response
            mockResponse.Setup(r => r.Cookies).Returns(mockResponseCookies.Object);

            // Setup session
            mockSession.Setup(s => s.Id).Returns("TestSessionId12345");

            // Setup context
            mockContext.Setup(c => c.Request).Returns(mockRequest.Object);
            mockContext.Setup(c => c.Response).Returns(mockResponse.Object);
            mockContext.Setup(c => c.Session).Returns(mockSession.Object);

            return mockContext.Object;
        }

        public static HttpContext CreateMockHttpContextWithSession(Dictionary<string, object> sessionData = null)
        {
            var mockContext = new Mock<HttpContext>();
            var mockSession = new Mock<ISession>();

            // Setup session with data
            if (sessionData != null)
            {
                foreach (var item in sessionData)
                {
                    byte[] value = System.Text.Encoding.UTF8.GetBytes(item.Value?.ToString() ?? "");
                    mockSession.Setup(s => s.TryGetValue(item.Key, out value)).Returns(true);
                }
            }

            mockSession.Setup(s => s.Id).Returns("TestSessionId12345");
            mockContext.Setup(c => c.Session).Returns(mockSession.Object);

            return mockContext.Object;
        }
    }
}
