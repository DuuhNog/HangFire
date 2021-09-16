using Hangfire;
using HangFireAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HangFireAPI.Controller
{
    [Route("api/[controller]")]

    public class EnvioController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] EnvioDTO envio)
        {
            try
            {
                BackgroundJob.Enqueue(() => MeuPrimeiroJobfireandforget());
                return Ok();
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        public async Task MeuPrimeiroJobfireandforget()
        {
            await Task.Run(() =>
            {
                Console.WriteLine("Bem vindo ao hangfire");
            });
        }
    }
}
