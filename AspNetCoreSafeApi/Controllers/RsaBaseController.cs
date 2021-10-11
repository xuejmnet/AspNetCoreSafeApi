using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreSafeApi.SecurityAuthorization.RsaChecker;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreSafeApi.Controllers
{
    [Authorize(AuthenticationSchemes =AuthSecurityRsaDefaults.AuthenticationScheme )]
    public class RsaBaseController : ControllerBase
    {
    }
}
