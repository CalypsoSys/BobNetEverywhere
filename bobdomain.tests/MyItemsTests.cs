using System.Collections;
using bobdomain;

namespace bobdomain.tests;

public class MyItemsTests
{
    [Fact]
    public void SaveItemCreatesDataFile()
    {
        string dataPath = CreateTempDirectory();

        try
        {
            bool saved = MyItems.SaveItem(dataPath, 7, "alpha", "beta");

            Assert.True(saved);
            Assert.Equal("7,alpha,beta", File.ReadAllText(Path.Combine(dataPath, "data", "saved_items.txt")).Trim());
        }
        finally
        {
            Directory.Delete(dataPath, true);
        }
    }

    [Fact]
    public void GetMyItemsIncludesSavedItems()
    {
        string dataPath = CreateTempDirectory();

        try
        {
            MyItems.SaveItem(dataPath, 7, "alpha", "beta");

            IEnumerable items = (IEnumerable)MyItems.GetMyItems(dataPath, 3);

            Assert.Contains(items.Cast<object>(), item => item.GetType().GetProperty("Source")?.GetValue(item)?.ToString() == "User Entered");
        }
        finally
        {
            Directory.Delete(dataPath, true);
        }
    }

    private static string CreateTempDirectory()
    {
        string dataPath = Path.Combine(Path.GetTempPath(), $"bobdomain-tests-{Guid.NewGuid():N}");
        Directory.CreateDirectory(dataPath);
        return dataPath;
    }
}
