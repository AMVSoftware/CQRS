using System.Collections.Generic;
using System.Web.Mvc;


namespace AMV.CQRS
{
    public interface IDropdownQueryHandler<in TQuery> : IQueryHandler<TQuery, IEnumerable<SelectListItem>> where TQuery : IDropdownQuery { }
}