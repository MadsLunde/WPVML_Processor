using WPVML_Processor.Controller.ApiModels;

namespace WPVML_Processor.Services.RavenDB
{
    public interface IMainRepository
    {
        void TestStore();

        string CreateSession(CreateSession session);

        void AddPageVisit(AddPageVisit page);
    }
}