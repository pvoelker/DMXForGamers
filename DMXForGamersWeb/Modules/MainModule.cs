using DMXForGamers.Models;
using Nancy;
using Nancy.ModelBinding;
using Nancy.ViewEngines.Razor;
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
            Get["/"] = _ =>
            {
                //http://www.jhovgaard.com/from-aspnet-mvc-to-nancy-part-1/

                return View["Index", Main.Instance];
            };

            Put["/events/{id}/enable"] = parameters =>
            {
                var dummy = parameters.id;


                var data = Main.Instance;

                var foundEvent = data.Events.SingleOrDefault(x => String.Compare(x.Description, dummy, true) == 0);

                if(foundEvent != null)
                {
                    foundEvent.EventOn.Execute(null);
                }

                return null;
            };
        }
    }
}
