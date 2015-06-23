using System.Web.Mvc;


namespace AMV.CQRS
{
    public static class MvcHtmlStringExtensions
    {
        public static MvcHtmlString Concat(this MvcHtmlString self, MvcHtmlString other)
        {
            return MvcHtmlString.Create(string.Format("{0}{1}", self, other));
        }

        public static MvcHtmlString Concat(this MvcHtmlString self, string other)
        {
            return MvcHtmlString.Create(string.Format("{0}{1}", self, other));
        }

        public static MvcHtmlString ConcatIf(this MvcHtmlString self, bool predicate, MvcHtmlString mvcHtmlString)
        {
            return predicate ? self.Concat(mvcHtmlString) : self;
        }

        public static MvcHtmlString ConcatIf(this MvcHtmlString self, bool predicate, string other)
        {
            return predicate ? self.Concat(other) : self;
        }
    }
}