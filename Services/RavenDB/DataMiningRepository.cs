using System.Collections.Generic;
using System.Linq;
using WPVML_Processor.Controller.ApiModels;
using WPVML_Processor.Models;
using WPVML_Processor.Services.OpenWeather;
using Raven.Client;

using Umbraco.Core;

namespace WPVML_Processor.Services.RavenDB
{
    public class DataMiningRepository : IDataMiningRepository
    {
        private Handler weatherHandler = new Handler();

        public string CreateSession(CreateSession session)
        {
            using (IDocumentSession documentSession = DocumentStoreSingleton.Instance.GetStore("CeramicSpeedPersonalizationModule").OpenSession())
            {
                User user = documentSession
                    .Query<User>()
                    .Where(x => x.BrowserCookieId == session.UserBrowserCookieId)
                    .FirstOrDefault();
                User newUser = null;
                if (user != null)
                {
                    newUser = user;
                }
                else
                {
                    newUser = new User() { BrowserCookieId = session.UserBrowserCookieId.Value };
                    documentSession.Store(newUser);
                }

                Weather weather = null;
                if (session.Location != null)
                {
                    weather = weatherHandler.GetWeatherForLocation(session.Location.Latitude, session.Location.Longitude);
                }


                Session newSession = new Session()
                {
                    Location = session.Location,
                    User = newUser.Id,
                    Weather = weather,
                    DateTime = session.DateTime
                };

                documentSession.Store(newSession);
                documentSession.SaveChanges();
                return newSession.Id;

            }
        }

        public void AddPageVisit(AddPageVisit page)
        {
            using (IDocumentSession documentSession =
                    DocumentStoreSingleton.Instance.GetStore("CeramicSpeedPersonalizationModule").OpenSession())
            {
                var session = documentSession.Load<Session>(page.SessionId);

                var newPage = new PageVisit()
                {
                    CountOrder = session.PageVisits != null && session.PageVisits.Any()
                        ? session.PageVisits.Last().CountOrder + 1
                        : 1,
                    DocumentTypeAlias = page.DocumentTypeAlias,
                    NodeId = page.NodeId,
                    TimeSpent = page.TimeSpent
                };

                if (session.PageVisits != null)
                {
                    session.PageVisits.Add(newPage);
                }
                else
                {
                    session.PageVisits = new List<PageVisit>() { newPage };
                }

                documentSession.Store(session);
                documentSession.SaveChanges();

            }

        }

        public void UpdateUserProducts(List<UCommerceProduct> products, string sessionId)
        {
            using (IDocumentSession documentSession =
                DocumentStoreSingleton.Instance.GetStore("CeramicSpeedPersonalizationModule").OpenSession())
            {
                var session = documentSession.Include<Session>(x => x.User).Load<Session>(sessionId);
                if (session != null)
                {
                    var user = documentSession.Load<User>(session.User);

                    foreach (var product in products)
                    {
                        var p = user.BoughtProducts.Where(x => x.SKU == product.SKU).FirstOrDefault();
                        if (p != null)
                        {
                            p.Quantity += product.Quantity;
                        }
                        else
                        {
                            user.BoughtProducts.Add(product);
                        }
                    }

                    session.MoneySpent += products.Sum(x => x.Price);

                    documentSession.Store(session);
                    documentSession.Store(user);
                    documentSession.SaveChanges();
                }
            }
        }
    }
}
