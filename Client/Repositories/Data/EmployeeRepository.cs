using API.Models;
using API.ViewModel;
using Client.Base.Urls;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client.Repositories.Data
{
    public class EmployeeRepository : GeneralRepository<Employee, string>
    {
        private readonly Address address;
        private readonly string request;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly HttpClient httpClient;
        public EmployeeRepository(Address address, string request = "Employees/") : base(address, request)
        {
            this.address = address;
            this.request = request;
            _contextAccessor = new HttpContextAccessor();
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(address.Link)
            };
        }
        public async Task<List<RegisterVMClient>> GetRegister()
        {
            List<RegisterVMClient> entities = new List<RegisterVMClient>();

            using (var response = await httpClient.GetAsync(request+"AllRegisteredData"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entities = JsonConvert.DeserializeObject<List<RegisterVMClient>>(apiResponse);
            }
            return entities;
        }
    }
}
