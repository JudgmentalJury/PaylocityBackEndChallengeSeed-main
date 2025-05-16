using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApiTests.IntegrationTests
{
    public  class PayrollIntegrationTests : IntegrationTest
    {
        [Fact]
        public async Task WhenAskedForSalaryOfEmployee_ShouldReturnCorrectBiWeeklySalary()
        {
            var response = await HttpClient.GetAsync($"/api/v1/payroll/biweekly/3");
            Debug.WriteLine(response);
            await response.ShouldReturn(HttpStatusCode.OK, 4567.19m);
        }

        [Fact]
        public async Task WhenAskedForSalaryOfEmployee_ShouldReturnCorrectMonthlySalary()
        {
            var response = await HttpClient.GetAsync($"/api/v1/payroll/monthly/3");
            Debug.WriteLine(response);
            await response.ShouldReturn(HttpStatusCode.OK, 9895.57m);
        }
    }
}
