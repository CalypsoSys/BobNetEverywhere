using System.Text.RegularExpressions;
using bobdomain;

namespace bobdomain.tests;

public class GraphDataTests
{
    [Fact]
    public void GetRandomColorReturnsCssHexColor()
    {
        string color = GraphData.GetRandomColor();

        Assert.Matches(new Regex("^#[0-9A-F]{6}$"), color);
    }

    [Fact]
    public void BuildGraphDataIncludesRequestedChartShape()
    {
        object graphData = new GraphData().BuildGraphData("bar");

        Assert.NotNull(graphData);
        Assert.NotNull(graphData.GetType().GetProperty("labels"));
        Assert.NotNull(graphData.GetType().GetProperty("datasets"));
    }
}
