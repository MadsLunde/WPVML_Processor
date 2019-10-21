using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using WPVML_Processor.Controller.ApiModels;
using WPVML_Processor.Services.RavenDB;

namespace WPVML_Processor.Controller
{
    public class MiningController : ApiController
    {
        private MainRepository repository;
        public MiningController()
        {
            repository = new MainRepository();
        }

        [HttpPost]
        public IHttpActionResult CreateNewSession(CreateSession session)
        {
            var result = repository.CreateSession(session);
            if (string.IsNullOrWhiteSpace(result))
            {
                return Ok(result);
            }
            else
            {
                return InternalServerError();
            }

        }

        [HttpPost]
        public IHttpActionResult AddPageVisit(AddPageVisit page)
        {
            /*
            Thread th = new Thread(() =>
            {
                Console.WriteLine("MiningController: " + Thread.CurrentThread);
                var repo = new MainRepository();
                repo.AddPageVisit(page);
            });
            th.Start();
            */
            
            repository.AddPageVisit(page);
            return Ok();
        }

        [HttpGet]
        public IHttpActionResult Test()
        {
            return Ok("test ok");
        }
    }
}
