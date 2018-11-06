using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NorthwindWebApi.Controllers
{
    [Authorize]
    public class AuthorizationController : ApiController
    {
        //Oturum açan kişinin bilgileri alınacak
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, UserInfoMethod.CurrentUser());
        }
    }
}
