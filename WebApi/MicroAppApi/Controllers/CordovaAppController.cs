using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.Options;
using MicroAppApi.Settings;
using Microsoft.AspNetCore.Hosting;

using MicroAppApi.Models;

namespace MicroAppApi.Controllers
{
    [Route("api/[controller]")]
    public class CordovaAppController : Controller
    {

        // private readonly ProgramSettings _settings;
        // public CordovaAppController(IOptions<ProgramSettings> serviceSettings)
        // {
        //     _settings = serviceSettings.Value;
        // }
        private IConfiguration _configuration;
        private string projectRootFolder;
        private IHostingEnvironment _hostingEnvironment;
        public CordovaAppController(IHostingEnvironment env, IConfiguration Configuration)
        {
            _hostingEnvironment = env;
            _configuration = Configuration;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<ClientAppFileModel> Get()
        {
            ProgramSettings ps = new ProgramSettings();
            // _configuration.GetSection("ProgramSettings").Bind(ps);
            string path = Path.Combine(_hostingEnvironment.ContentRootPath, "ClientApp/React");

            System.Diagnostics.Debug.WriteLine(path);
            string[] entries = Directory.GetFileSystemEntries(path, "*", SearchOption.AllDirectories);

List<ClientAppFileModel> files = new List<ClientAppFileModel>();
            foreach (var elem in entries)
            {
                files.Add(new ClientAppFileModel(elem, _hostingEnvironment.ContentRootPath));
                System.Diagnostics.Debug.WriteLine(elem);
            }
            return files;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            //This function should return an object that contains a list of files in the 
            //  client app's dir with their paths and hashes.

            // ProgramSettings ps = new ProgramSettings();
            // _configuration.GetSection("ProgramSettings").Bind(ps);
            //             string path = ps.ClientAppRelativePath;
            string path = Path.Combine(_hostingEnvironment.ContentRootPath, "ClientApp/React");
            string[] entries = Directory.GetFileSystemEntries(path, "*", SearchOption.AllDirectories);

            foreach (var elem in entries)
            {
                System.Diagnostics.Debug.WriteLine(elem);
            }

            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
