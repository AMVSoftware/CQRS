using System.Collections.Generic;
using System.Web.Mvc;


namespace AMV.CQRS
{
    public interface IDropdownQuery : IQuery<IEnumerable<SelectListItem>> { }
}