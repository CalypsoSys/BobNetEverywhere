namespace bobweb
{
    public interface IItemStoragePathProvider
    {
        /// <summary>
        /// Gets the root folder used for sample item persistence.
        /// </summary>
        string GetDataPath();
    }
}
