using NorthwindWebApi.Filters;
using NorthwindWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NorthwindWebApi.Controllers
{
    [MyAction]
    public class ProductController : ApiController
    {
        NorthwindDbContext db = new NorthwindDbContext();
        public HttpResponseMessage Get()
        {
            var products = db.Products.ToList();
            if (products != null)
            {
                return Request.CreateResponse(HttpStatusCode.Accepted, products);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Products are not found.");
            }

            //Request.CreateResponse(HttpStatusCode.Accepted);
        }
        public HttpResponseMessage Get(int id)
        {
            var product = db.Products.Find(id);
            if (product != null)
            {
                return Request.CreateResponse(HttpStatusCode.Accepted, product);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Product are not found.");
            }

            //Request.CreateResponse(HttpStatusCode.Accepted);
        }
        public HttpResponseMessage Get(string name)
        {
            var product = db.Products.SingleOrDefault(x => x.ProductName == name);
            if (product != null)
            {
                return Request.CreateResponse(HttpStatusCode.Accepted, product);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Product are not found.");
            }

            //Request.CreateResponse(HttpStatusCode.Accepted);
        }

        public HttpResponseMessage Post([FromUri] Product product)
        {
            try
            {
                if(product!=null)
                {
                    db.Products.Add(product);
                    if(db.SaveChanges()>0)
                    {
                        HttpResponseMessage message= Request.CreateResponse(HttpStatusCode.Created, product);
                        message.Headers.Location = new Uri(Request.RequestUri + "/" + product.ProductId);
                        return message;
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Product was not added!");
                    }
                    
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Product was not sent.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);                
            }
          

            //Request.CreateResponse(HttpStatusCode.Accepted);
        }
    }
}
