using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebsiteKinhDoanhDoGoCuongThai.Startup))]
namespace WebsiteKinhDoanhDoGoCuongThai
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
