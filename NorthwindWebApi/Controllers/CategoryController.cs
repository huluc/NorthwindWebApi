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
    [AppExceptionFilter]
    public class CategoryController : ApiController
    {
        NorthwindDbContext db = new NorthwindDbContext();
        [Authorize]
        public HttpResponseMessage Get()
        {
            //throw new Exception("This is throwed manually.");
            var user = UserInfoMethod.CurrentUser();
            if (user.Role=="Admin")
            {
                var categories = db.Categories.ToList();
                if (categories != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, categories);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Categories are not found.");
                }

            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Categories can be listed by Admin!");
            }

            //Request.CreateResponse(HttpStatusCode.Accepted);
        }
        [CheckModelForNull]
        public HttpResponseMessage Post(Category category)
        {
            using (NorthwindDbContext db = new NorthwindDbContext())
            {
                db.Categories.Add(category);
                if (db.SaveChanges() > 0)
                    return Request.CreateResponse(HttpStatusCode.Created, "Category is added succesfully!");
                else
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Category can not be added!");
            }
        }
    }
}
