using NorthwindWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace NorthwindWebApi
{
    internal class UserInfoMethod
    {
        internal static UserInfoModel CurrentUser()
        {
            //Kullanıcı değerierini SimpleAuthorizationServerProvider da bir claims listesine yüklemiştik. Buradaki değerleri o listeden alacağız.
            var claims = ClaimsPrincipal.Current.Identities.First().Claims;
            var result = new UserInfoModel()
            {
                UserName = claims.First(x => x.Type == "UserName").Value,
                Role = claims.First(x => x.Type == "Role").Value,
                Lang = claims.First(x => x.Type == "Lang").Value
            };
            return result;
        }
    }
}