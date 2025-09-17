using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.SessionState;
using Moq;

namespace RiskCalculatorWebForm.Tests.TestUtilities
{
    /// <summary>
    /// Utility class for creating mock HttpContext objects for unit testing
    /// </summary>
    public static class MockHttpContext
    {
        public static HttpContextBase CreateMockHttpContext()
        {
            var mockContext = new Mock<HttpContextBase>();
            var mockRequest = new Mock<HttpRequestBase>();
            var mockResponse = new Mock<HttpResponseBase>();
            var mockSession = new Mock<HttpSessionStateBase>();
            var mockServer = new Mock<HttpServerUtilityBase>();

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

        public static HttpContextBase CreateMockHttpContextWithSession(Dictionary<string, object> sessionData = null)
        {
            var mockContext = CreateMockHttpContext();
            var mockSession = new Mock<HttpSessionStateBase>();
            
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
