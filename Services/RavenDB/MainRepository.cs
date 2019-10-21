using Raven.Client.Documents.Session;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Raven.Client.Documents;
using Raven.Client.Documents.Linq;

using WPVML_Processor.Controller.ApiModels;
using WPVML_Processor.Models;
using WPVML_Processor.Services.OpenWeather;

namespace WPVML_Processor.Services.RavenDB
{
    public class MainRepository : IMainRepository
    {
        //private IDocumentStore store;
        private Handler weatherHandler = new Handler();
        //private static ConcurrentQueue<AddPageVisit> pageQueue;
        //private Thread queueThread;

        public MainRepository()
        {
            /*
            store = ;
            weatherHandler = new Handler();
            /*
            pageQueue = new ConcurrentQueue<AddPageVisit>();
            queueThread = new Thread(AddPageToDb);
            queueThread.Start();
            */
        }
        public void TestStore()
        {
            using (IDocumentSession documentSession = DocumentStoreSingleton.Instance.GetStore("CeramicSpeedPersonalizationModule").OpenSession())  // Open a session for a default 'Database'
            {
                User user = new User()
                {
                    BrowserCookieId = Guid.NewGuid()
                };
                documentSession.Store(user);

                Session session = new Session()
                {
                    Location = new Location()
                    {
                        Latitude = 50,
                        Longitude = 10
                    },
                    User = user.Id,
                    PageVisits = new List<PageVisit>()
                    {
                        new PageVisit()
                        {
                            CountOrder = 1,
                            DocumentTypeAlias = "homePage",
                            NodeId = 1234,
                            TimeSpent = 20
                        },
                        new PageVisit()
                        {
                            CountOrder = 2,
                            DocumentTypeAlias = "shopPage",
                            NodeId = 1235,
                            TimeSpent = 30,
                        }
                    }
                };

                documentSession.Store(session);                             // Assign an 'Id' and collection (Products)
                // and start tracking an entity

                documentSession.SaveChanges();                              // Send to the Server
                // one request processed in one transaction
                Console.WriteLine("Product created");
            }
        }

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
                    newUser = new User() { BrowserCookieId = session.UserBrowserCookieId };
                    documentSession.Store(newUser);
                }

                Session newSession = new Session()
                {
                    Location = session.Location,
                    User = newUser.Id
                    //Weather = weatherHandler.GetWeatherForLocation(session.Location.Latitude, session.Location.Longitude)
                };

                documentSession.Store(newSession);
                documentSession.SaveChanges();
                return newSession.Id;

            }
        }

        public void AddPageVisit(AddPageVisit page)
        {
            //pageQueue.Enqueue(page);
            
            /*
            Thread th = new Thread(() =>
            {
                */
                using (IDocumentSession documentSession =
                    DocumentStoreSingleton.Instance.GetStore("CeramicSpeedPersonalizationModule").OpenSession())
                {
                    var session = documentSession.Load<Session>(page.SessionId);
                    session.PageVisits.Add(new PageVisit()
                    {
                        CountOrder = session.PageVisits.Any() ? session.PageVisits.Last().CountOrder + 1 : 1,
                        DocumentTypeAlias = page.DocumentTypeAlias,
                        NodeId = page.NodeId,
                        TimeSpent = page.TimeSpent
                    });
                    documentSession.Store(session);
                    documentSession.SaveChanges();
                }
                /*
        });
            th.Start();
            */
        }

        /*
        private static void AddPageToDb()
        {
            while (pageQueue.Any())
                {
                    AddPageVisit page;
                    pageQueue.TryDequeue(out page);
                    if (page != null)
                    {
                        using (IDocumentSession documentSession =
                            DocumentStoreSingleton.Instance.GetStore("CeramicSpeedPersonalizationModule").OpenSession())
                        {
                            var session = documentSession.Load<Session>(page.SessionId);
                            session.PageVisits.Add(new PageVisit()
                            {
                                CountOrder = session.PageVisits.Any() ? session.PageVisits.Last().CountOrder + 1 : 1,
                                DocumentTypeAlias = page.DocumentTypeAlias,
                                NodeId = page.NodeId,
                                TimeSpent = page.TimeSpent
                            });
                            documentSession.Store(session);
                            documentSession.SaveChanges();
                        }    
                    }
                }
        }
        */

    }
}
