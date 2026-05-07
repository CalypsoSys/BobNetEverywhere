using System.Reflection;
using bobweb.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace bobweb.tests;

public class DefaultControllerTests
{
    [Fact]
    public void DefaultControllerUsesApiItemsRoute()
    {
        var route = typeof(DefaultController).GetCustomAttribute<RouteAttribute>();

        Assert.NotNull(route);
        Assert.Equal("api/items", route.Template);
    }

    [Fact]
    public void DefaultControllerDoesNotExposeRootGetRoute()
    {
        MethodInfo[] rootGetMethods = typeof(DefaultController)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Where(method => method.GetCustomAttribute<HttpGetAttribute>()?.Template == null)
            .ToArray();

        Assert.Empty(rootGetMethods);
    }
}
