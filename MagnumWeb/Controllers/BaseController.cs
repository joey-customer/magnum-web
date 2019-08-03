using Microsoft.AspNetCore.Mvc;
using Magnum.Api.NoSql;

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
