using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NorthwindWebApi.Controllers
{
    public class OrderController : ApiController
    {
        //API geliştiricisi, token authentication yöntemini kullanarak Apiyi kullanmak isteyen clientlara denetleme sağlamakta ve bu şekilde kontrollü bir açılım uygulamaktadır.
        //Denetlemede doğru kullanıcı adı ve şifreyi giren kullanıcıya server tarafında belirli bir süreliğine bir adet token oluşturulur. Kullanıcı bu tokenı kullanarak Web API’nin nimetlerinden faydalanabilmektedir.
        [HttpGet]
        [Authorize]
        public List<string> List()
        {
            List<string> orders = new List<string>();

            orders.Add("Elma");
            orders.Add("Armut");
            orders.Add("Erik");

            return orders;
        }
    }
}
