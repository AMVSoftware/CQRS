using System;
using System.Web;
// ReSharper disable CheckNamespace


namespace AMV.CQRS
{
    /// <summary>
    /// Wrapper for Server.MapPath()
    /// </summary>
    public class ServerPathService : IServerPathService
    {
        public String MapPath(String relativePath)
        { 
            return HttpContext.Current.Server.MapPath(relativePath);
        }
    }
}