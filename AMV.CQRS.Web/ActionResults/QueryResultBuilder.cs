using System;
using System.Collections;
using System.Web.Mvc;
using AutoMapper;


namespace AMV.CQRS
{
    public class QueryResultBuilder<TResult>
    {
        private readonly IQuery<TResult> query;
        private readonly ViewDataDictionary viewData;
        private readonly TempDataDictionary tempData;
        private readonly ViewEngineCollection viewEngineCollection;
        private readonly IMediator mediator;

        private Type destinationType;
        private bool doJson;
        private bool jsonAllowGet;

        public QueryResultBuilder(IQuery<TResult> query, ViewDataDictionary viewData, TempDataDictionary tempData, ViewEngineCollection viewEngineCollection)
        {
            this.query = query;
            this.viewData = viewData;
            this.tempData = tempData;
            this.viewEngineCollection = viewEngineCollection;
            mediator = DependencyResolver.Current.GetService<IMediator>();
        }


        public QueryResultBuilder<TResult> MapTo<TDest>()
        {
            destinationType = typeof(TDest);
            return this;
        }


        public QueryResultBuilder<TResult> DoJson(bool allowGet = false)
        {
            this.doJson = true;
            this.jsonAllowGet = allowGet;
            return this;
        }


        public static implicit operator ActionResult(QueryResultBuilder<TResult> processorBuilder)
        {
            return processorBuilder.Build();
        }


        public virtual ActionResult Build()
        {
            var result = mediator.Request(query);
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
