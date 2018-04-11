using DMXForGamers.Models;
using Nancy;
using Nancy.ModelBinding;
using Nancy.ViewEngines.Razor;
using Newtonsoft.Json;
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

            Get["/events/{id}/enable"] = parameters =>
            {
                var eventID = (string)(parameters.id);

                var data = Main.Instance;

                var foundEvent = data.Events.SingleOrDefault(x => String.Compare(x.EventID, eventID, true) == 0);

                if(foundEvent != null)
                {
                    foundEvent.EventOn.Execute(eventID);
                }

                return @"{ ""success"":true }";
            };

            Get["/events/{id}/disable"] = parameters =>
            {
                var eventID = (string)(parameters.id);

                var data = Main.Instance;

                var foundEvent = data.Events.SingleOrDefault(x => String.Compare(x.EventID, eventID, true) == 0);

                if (foundEvent != null)
                {
                    foundEvent.EventOff.Execute(eventID);
                }

                return @"{ ""success"":true }";
            };
        }
    }
}
