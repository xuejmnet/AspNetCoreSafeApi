using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreSafeApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AspNetCoreSafeApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController: RsaBaseController
    {
        [AllowAnonymous]
        public IActionResult Test()
        {
            return Ok();
        }


        public IActionResult Test1()
        {
            var appid = Request.HttpContext.User.Claims.FirstOrDefault(o=>o.Type== "appid").Value;
            var appname = Request.HttpContext.User.Claims.FirstOrDefault(o=>o.Type== "appname").Value;

            return Ok($"appid:{appid},appname:{appname}");
        }

        public IActionResult Test2(TestModel request)
        {
            return Ok(JsonConvert.SerializeObject(request));
        }
        public IActionResult Test3(TestModel request)
        {
            var x = 0;
            var a = 1 / x;
            return Ok("ok");
        }
    }

}
