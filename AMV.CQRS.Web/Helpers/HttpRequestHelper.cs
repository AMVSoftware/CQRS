using System;
using System.Web;

namespace AMV.CQRS
{
    public static class HttpRequestHelper
    {
        /// <summary>
        /// Checks if request made is http POST
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool IsPost(this HttpRequestBase request)
        {
            return request.HttpMethod.Equals("POST", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}