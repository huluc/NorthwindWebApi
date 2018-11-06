using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace NorthwindWebApi.Filters
{
    public class AppExceptionFilter : ExceptionFilterAttribute
    {/*
        Fatih — Eğitmen  · bir yıl önce 
        https://www.udemy.com/aspnet-web-api-ile-yazilimcilarin-bagimsizligi/learn/v4/t/lecture/6700660?start=0
        Ders 36
        Bu soruda aslında cevabı saklı.

        MyValidationException diye kendi exception sınıfını oluştur ve MyValidationException hatası fırlat yok yeni exception yazmıyım diyorsan  FluentValidation'ın veya .NET'in bir validation sınıfını kullanabilirsin. Daha sonra ExceptionFilter yaz api'de (Ki mecburen bir exception filterin olmalı :) ) Eğer gelen hata MyValidationException tipinde bir exception ise nerden geldiğini biliyorsundur artık orada istediğin şekilde HTTP Durum kodunu göndür. 

        Exception lar çok önemlidir ister api ister farklı bir şey yazın elinizden geldiğince  hata çıkmasın diye Try-Catch  koymamaya çalışın. Buna en büyük örneği .NET'in kendisidir. Örneğin Bir dosyayı bulamadığında Direk FileNotFoundException fırlatır mesela. Bizim işimiz fırlayan hataları ortak bir yerde handle ederek ekranda projemize/sayfalarımıza uygun şekilde hataları göstermektir. 
      */
        //Bu attribute u kullanan her yerde bir exception fırlatıldığında bu action tetiklenecektir.
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            //Logging
            Logger.Logging(actionExecutedContext.Exception.Message, actionExecutedContext.Exception.StackTrace);
            //Giving Response
            MyErrorResponse result = new MyErrorResponse();
            result.ErrorAction = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
            result.ErrorController = actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName;
            result.ErrorMessage = actionExecutedContext.Exception.ToString();

            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, result);
        }
    }
}