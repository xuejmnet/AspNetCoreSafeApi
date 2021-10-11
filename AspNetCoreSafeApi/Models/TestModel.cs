using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AuthSecurity.AspNetCore.NettyClientProvider.AuthSecurityBinder.RsaBinder;

namespace AspNetCoreSafeApi.Models
{
    [RsaModelParse]
    public class TestModel
    {
        [Display(Name = "id"),Required(ErrorMessage = "{0}不能为空")]
        public string Id { get; set; }
    }
}
