using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

using CeramicSpeed.Core.Helpers;
using CeramicSpeed.Core.Services;

using UCommerce.Api;
using UCommerce.EntitiesV2;
using UCommerce.Runtime;
using Umbraco.Core.Logging;
using Umbraco.Web.WebApi;
using WPVML_Processor.Controller.ApiModels;
using WPVML_Processor.Controller.ViewModels;
using WPVML_Processor.Models;
using WPVML_Processor.Services.RavenDB;

namespace WPVML_Processor.Controller
{
    public class MiningController : UmbracoApiController
    {
        private readonly IDataMiningRepository _repository;
        private readonly IVariantService _variantService;

        public MiningController(IDataMiningRepository repository, IVariantService variantService)
        {
            _repository = repository;
            _variantService = variantService;
        }

        [HttpPost]
        public IHttpActionResult CreateNewSession(CreateSession session)
        {
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, "Model invalid");
            }

            if (!session.UserBrowserCookieId.HasValue)
            {
                session.UserBrowserCookieId = Guid.NewGuid();
            }


            try
            {
                var result = _repository.CreateSession(session);
                return Ok(new SessionViewModel()
                {
                    UserBrowserCookieId = session.UserBrowserCookieId.Value,
                    SessionId = result
                });
            }
            catch (Exception e)
            {
                LogHelper.Error<MiningController>($"CreateNewSession failed - session: '{session}'", e);
                return Content(HttpStatusCode.Conflict, "Something went wrong while creating new session for personalization");
            }

        }

        [HttpPost]
        public IHttpActionResult AddPageVisit(AddPageVisit page)
        {
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, "Model invalid");
            }

            try
            {
                _repository.AddPageVisit(page);
                return Ok();
            }
            catch (Exception e)
            {
                LogHelper.Error<MiningController>($"AddPageVisit failed - page: '{page}'", e);
                return Content(HttpStatusCode.Conflict, "Something went wrong while creating new session for personalization");
            }

        }

        [HttpPost]
        public IHttpActionResult UpdateProductsForUser(UpdateUserProducts model)
        {
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, "Model invalid");
            }

            var order = OrderHelper.GetOrder(model.OrderGuid);
            var productList = new List<UCommerceProduct>();

            SiteContext.Current.CatalogContext.CurrentCatalog.PriceGroup =
                PriceGroup.FirstOrDefault(x => x.PriceGroupId == 19);

            var currentPriceGroup = SiteContext.Current.CatalogContext.CurrentCatalog.PriceGroup;

            foreach (var line in order.OrderLines)
            {
                var product = CatalogLibrary.GetProduct(line.Sku);
                var priceGroupPrice = product.PriceGroupPrices.Where(x => x.PriceGroup.PriceGroupId == 19).FirstOrDefault();
                var price = _variantService.GetLowestVariantPrice(product);
                if (priceGroupPrice.Price.HasValue)
                {
                    price = priceGroupPrice.Price.Value;
                }
                productList.Add(new UCommerceProduct() { Price = price, SKU = line.Sku, Quantity = line.Quantity });
            }

            _repository.UpdateUserProducts(productList, model.SessionId);

            return Ok();
        }
    }
}
