using Raven.Client;
using System;
using System.Collections.Generic;
using System.Text;

using Raven.Client.Document;
using System.Configuration;

namespace WPVML_Processor.Services.RavenDB
{
    public sealed class DocumentStoreSingleton
    {
        private Dictionary<string, IDocumentStore> documentStoreDictionary;
        private static DocumentStoreSingleton instance = null;
        private IDocumentStore store;
        private string databaseUrl = ConfigurationManager.AppSettings.Get("WPVML_Processor.DatabaseUrl");
        private DocumentStoreSingleton()
        {
            documentStoreDictionary = new Dictionary<string, IDocumentStore>();
        }

        public IDocumentStore GetStore(string databaseName)
        {
            if (documentStoreDictionary.ContainsKey(databaseName))
            {
                return documentStoreDictionary[databaseName];
            }
            else
            {
                store = new DocumentStore()
                {
                    Url = databaseUrl,
                    DefaultDatabase = databaseName
                };

                store.Initialize();                 // Each DocumentStore needs to be initialized before use.
                // This process establishes the connection with the Server
                // and downloads various configurations
                // e.g. cluster topology or client configuration
                documentStoreDictionary.Add(databaseName, store);
                return store;
            }
            
        }
        public static DocumentStoreSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DocumentStoreSingleton();
                }

                return instance;
            }
        }
    }
}