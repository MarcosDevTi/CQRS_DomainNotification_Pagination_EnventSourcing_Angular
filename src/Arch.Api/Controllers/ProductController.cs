﻿using Arch.Domain.Core.DomainNotifications;
using Arch.Infra.Shared.Cqrs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Arch.Cqrs.Client.Query.Product.Queries;

namespace Arch.Api.Controllers
{
    [RoutePrefix("api/products")]
    public class ProductController : BaseController
    {
        private readonly IProcessor _processor;
        public ProductController(IDomainNotification notifications, IProcessor processor) : base(notifications)
        {
            _processor = processor;
        }

        [HttpGet, Route("v1/public/list")]
        public HttpResponseMessage ProductsDropDownList(GetProductsDropDownList getProductsDw)
        {
            var result = _processor.Get(getProductsDw);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}