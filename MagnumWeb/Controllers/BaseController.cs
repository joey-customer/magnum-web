using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MagnumWeb.Models;
using Magnum.Api.NoSql;
using Magnum.Api.Factories;
using Magnum.Api.Businesses.Registrations;
using Magnum.Api.Models;
using Microsoft.AspNetCore.Http;

namespace MagnumWeb.Controllers
{
    public class BaseController : Controller
    {
        private INoSqlContext context = null;
      
        public void SetNoSqlContext(INoSqlContext context)
        {
            this.context = context;
        }

        public INoSqlContext GetNoSqlContext()
        {
            return context;
        }

        protected INoSqlContext GetNoSqlContext(string provider, string host, string key, string user, string password)
        {
            INoSqlContext ctx = null;
            if (provider.Equals("firebase"))
            {
                ctx = new FirebaseNoSqlContext();
                ctx.Authenticate(host, key, user, password);
            }

            return ctx;
        }
    }
}
