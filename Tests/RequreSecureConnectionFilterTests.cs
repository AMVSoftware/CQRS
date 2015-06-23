using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AMV.CQRS;
using NSubstitute;
using Xunit;

namespace Tests
{
    public class RequreSecureConnectionFilterTests
    {
        private readonly HttpRequestBase request;
        private readonly AuthorizationContext filterContext;


        public RequreSecureConnectionFilterTests()
        {
            var @params = new NameValueCollection();
            var responseHeaders = new NameValueCollection();

            request = Substitute.For<HttpRequestBase>();
            request.Params.Returns(@params);

            var response = Substitute.For<HttpResponseBase>();
            response.Headers.Returns(responseHeaders);

            var context = Substitute.For<HttpContextBase>();
            context.Request.Returns(request);
            context.Response.Returns(response);

            var controller = Substitute.For<ControllerBase>();

            var actionDescriptor = Substitute.For<ActionDescriptor>();
            var controllerContext = new ControllerContext(context, new RouteData(), controller);
            filterContext = new AuthorizationContext(controllerContext, actionDescriptor);
        }


        [Fact]
        public void OnAuthorisation_NoContext_ThrowsException()
        {
            var sut = new RequreSecureConnectionFilter();
            Assert.Throws<ArgumentNullException>(() => sut.OnAuthorization(null));
        }


        [Fact]
        public void OnAuthorisation_LocalRequest_RequestNotRedirected()
        {
            //Arrange
            request.IsLocal.Returns(true);
            var sut = new RequreSecureConnectionFilter();

            // Act
            sut.OnAuthorization(filterContext);

            // Assert - checking if we are not being redirected
            var redirectResult = filterContext.Result as RedirectResult;
            Assert.Null(redirectResult);
        }


        [Fact]
        public void OnAuthorisation_NonLocalRequest_RedirectedToHttps()
        {
            //Arrange
            request.IsLocal.Returns(false);
            var sut = new RequreSecureConnectionFilter();

            // Act && Assert 
            // here we check if controll is passed down to RequireHttpsAttribute code
            // and we are not testing for Microsoft code.
            Assert.Throws<InvalidOperationException>(() => sut.OnAuthorization(filterContext));
        }
    }


}
