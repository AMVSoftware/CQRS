using System;
// ReSharper disable CheckNamespace


namespace AMV.CQRS
{
    /// <summary>
    /// Abstraction over Server.MapPath() for testabily and looser coupling.
    /// Use <see cref="ServerPathService"/> for production use;
    /// use <see cref="StubServerPathService"/> for testing
    /// </summary>
    public interface IServerPathService
    {
        String MapPath(String relativePath);
    }
}