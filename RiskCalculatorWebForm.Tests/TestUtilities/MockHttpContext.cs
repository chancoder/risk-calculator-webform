using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.SessionState;
using Moq;
using Microsoft.AspNetCore.Http;
using HttpContext = System.Web.HttpContext;
using HttpSessionStateBase = System.Web.SessionState.HttpSessionState;


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
            var mockRequest = new Mock<System.Web.HttpRequest>();
            var mockResponse = new Mock<System.Web.HttpResponse>();
            var mockSession = new Mock<HttpSessionState>();
            var mockServer = new Mock<HttpServerUtility>();

            // Setup request
            mockRequest.Setup(r => r.QueryString).Returns(new NameValueCollection());
            mockRequest.Setup(r => r.Form).Returns(new NameValueCollection());
            mockRequest.Setup(r => r.Cookies).Returns(new HttpCookieCollection());
            mockRequest.Setup(r => r.Url).Returns(new Uri("http://localhost:12345/Test.aspx"));

            // Setup response
            mockResponse.Setup(r => r.Cookies).Returns(new HttpCookieCollection());

            // Setup session
            mockSession.Setup(s => s.SessionID).Returns("TestSessionId12345");

            // Setup context
            mockContext.Setup(c => c.Request).Returns(mockRequest.Object);
            mockContext.Setup(c => c.Response).Returns(mockResponse.Object);
            mockContext.Setup(c => c.Session).Returns(mockSession.Object);
            mockContext.Setup(c => c.Server).Returns(mockServer.Object);

            return mockContext.Object;
        }

        public static HttpContext CreateMockHttpContextWithSession(Dictionary<string, object> sessionData = null)
        {
            var mockContext = new Mock<HttpContext>();
            var mockRequest = new Mock<System.Web.HttpRequest>();
            var mockResponse = new Mock<System.Web.HttpResponse>();
            var mockSession = new Mock<HttpSessionState>();
            var mockServer = new Mock<HttpServerUtility>();

            // Setup request
            mockRequest.Setup(r => r.QueryString).Returns(new NameValueCollection());
            mockRequest.Setup(r => r.Form).Returns(new NameValueCollection());
            mockRequest.Setup(r => r.Cookies).Returns(new HttpCookieCollection());
            mockRequest.Setup(r => r.Url).Returns(new Uri("http://localhost:12345/Test.aspx"));

            // Setup response
            mockResponse.Setup(r => r.Cookies).Returns(new HttpCookieCollection());

            // Setup context
            mockContext.Setup(c => c.Request).Returns(mockRequest.Object);
            mockContext.Setup(c => c.Response).Returns(mockResponse.Object);
            mockContext.Setup(c => c.Server).Returns(mockServer.Object);

            // Setup session with data
            if (sessionData != null)
            {
                foreach (var item in sessionData)
                {
                    mockSession.Setup(s => s[item.Key]).Returns(item.Value);
                }
            }

            mockSession.Setup(s => s.SessionID).Returns("TestSessionId12345");
            mockContext.Setup(c => c.Session).Returns(mockSession.Object);

            return mockContext.Object;
        }
    }
}
