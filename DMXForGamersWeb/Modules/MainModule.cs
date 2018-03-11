using Nancy;
using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXForGamers.Web
{
    public class MainModule : NancyModule
    {
        public MainModule()
        {
            //https://github.com/NancyFx/Nancy/wiki/Model-binding

            Get["/"] = _ =>
            {
                this.BindTo(new object());
                return View["index.html"];
            };
        }
    }
}
