namespace SmallApiToolkit.Core.Abstractions
{
    public interface IFile
    {
        string FileName { get; }
        string ContentType { get; }
        byte[] Data { get; }
    }
}
