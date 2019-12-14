using System.Collections.Generic;
using WPVML_Processor.Controller.ApiModels;
using WPVML_Processor.Models;

namespace WPVML_Processor.Services.RavenDB
{
    public interface IDataMiningRepository
    {
        string CreateSession(CreateSession session);

        void AddPageVisit(AddPageVisit page);

        void UpdateUserProducts(List<UCommerceProduct> products, string sessionId);
    }
}