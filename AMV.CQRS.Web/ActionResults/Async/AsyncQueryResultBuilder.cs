using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;


namespace AMV.CQRS
{
    public class AsyncQueryResultBuilder<TResult>
    {
        private readonly IAsyncQuery<TResult> query;
        private readonly ViewDataDictionary viewData;
        private readonly TempDataDictionary tempData;
        private readonly ViewEngineCollection viewEngineCollection;
        private readonly IMediator mediator;

        private Type destinationType;
        private bool doJson;
        private bool jsonAllowGet;

        public AsyncQueryResultBuilder(IAsyncQuery<TResult> query, ViewDataDictionary viewData, TempDataDictionary tempData, ViewEngineCollection viewEngineCollection)
        {
            this.query = query;
            this.viewData = viewData;
            this.tempData = tempData;
            this.viewEngineCollection = viewEngineCollection;
            mediator = DependencyResolver.Current.GetService<IMediator>();
        }


        public AsyncQueryResultBuilder<TResult> MapTo<TDest>()
        {
            destinationType = typeof(TDest);
            return this;
        }


        public AsyncQueryResultBuilder<TResult> DoJson(bool allowGet = false)
        {
            this.doJson = true;
            this.jsonAllowGet = allowGet;
            return this;
        }


        public static implicit operator Task<ActionResult>(AsyncQueryResultBuilder<TResult> processorBuilder)
        {
            return processorBuilder.Build();
        }


        public virtual async Task<ActionResult> Build()
        {
            var result = await mediator.RequestAsync(query);
            Object resultingModel = result;

            if (destinationType != null)
            {
                resultingModel = Mapper.Map(result, typeof(TResult), destinationType);
            }

            if (doJson)
            {
                var jsonResult = new CustomJsonResult()
                {
                    Data = resultingModel,
                    MaxJsonLength = Int32.MaxValue,
                };
                if (jsonAllowGet)
                {
                    jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                }

                return jsonResult;
            }

            viewData.Model = resultingModel;

            return new ViewResult
            {
                ViewName = null,
                MasterName = null,
                ViewData = viewData,
                TempData = tempData,
                ViewEngineCollection = viewEngineCollection
            };
        }
    }
}
