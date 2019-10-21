using Raven.Client.Documents;
using System;
using System.Collections.Generic;
using System.Text;

namespace WPVML_Processor.Services.RavenDB
{
    public sealed class DocumentStoreSingleton
    {
        private Dictionary<string, IDocumentStore> documentStoreDictionary;
        private static DocumentStoreSingleton instance = null;
        private IDocumentStore store;
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
                store = new DocumentStore
                {
                    Urls = new[] // URL to the Server,
                    {
                        // or list of URLs 
                        "http://192.168.10.14:8080" // to all Cluster Servers (Nodes)
                    },
                    Database = databaseName, // Default database that DocumentStore will interact with
                    Conventions = { } // DocumentStore customizationss
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