using NorthwindWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace NorthwindWebApi.Filters
{
    public class MyActionAttribute :ActionFilterAttribute
    {
        //Attribute kullanılan yerdeki actionlar çalışmadan önce 

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            using (NorthwindDbContext db = new NorthwindDbContext())
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in actionContext.ActionArguments)
                {
                    sb.Append($"{item.Key}={item.Value.ToString()},");
                }
                db.Logs.Add(new Log()
                {
                    IsBefore = true,
                    LogCaption = $"{actionContext.ControllerContext.ControllerDescriptor.ControllerName}-{actionContext.ActionDescriptor.ActionName}",
                    Time = DateTime.Now,
                    LogDetail = sb.ToString()
                });
                db.SaveChanges();
            }
            //https://www.strathweb.com/2012/10/clean-up-your-web-api-controllers-with-model-validation-and-null-check-filters/
        
            base.OnActionExecuting(actionContext);
        }   
        //Attribute kullanılan yerdeki actionlar çalıştıktan sonra
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            using (NorthwindDbContext db = new NorthwindDbContext())
            {
                db.Logs.Add(new Log()
                {
                    IsBefore = false,
                    LogCaption = $"{actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName}-{actionExecutedContext.ActionContext.ActionDescriptor.ActionName}",
                    Time = DateTime.Now,
                    LogDetail = (actionExecutedContext.Response.Content as ObjectContent).ObjectType.FullName
                });
                db.SaveChanges();
            }
            base.OnActionExecuted(actionExecutedContext);
        }
    }

    //https://www.strathweb.com/2012/10/clean-up-your-web-api-controllers-with-model-validation-and-null-check-filters/
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if(!actionContext.ModelState.IsValid)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
            }
        }
    }

    //https://www.strathweb.com/2012/10/clean-up-your-web-api-controllers-with-model-validation-and-null-check-filters/
    public class CheckModelForNullAttribute : ActionFilterAttribute //İNCELE!!!
    {
        //Dictionary türünden parametre alıp object türden değer döndüren metodu temsil eder.
        private readonly Func<Dictionary<string, object>, bool> _validate;
        //We accept a single input paramter when the argument is declared – a Func to provide a plumbing mechanism for custom logic.By default we simply check – if the argument is null we invalidate the request.
        public CheckModelForNullAttribute() : this(arguments => arguments.ContainsValue(null))
        {

        }
        public CheckModelForNullAttribute(Func<Dictionary<string, object>, bool> checkCondition)
        {
            _validate = checkCondition;
        }
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if(_validate(actionContext.ActionArguments))
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The argument can not be null");
            }
            base.OnActionExecuting(actionContext);
        }
    }
 
}