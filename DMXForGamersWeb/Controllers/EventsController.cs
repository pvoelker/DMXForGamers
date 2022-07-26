using DMXForGamers.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace DMXForGamers.Web.Controllers
{
    [Route("events")]
    public class EventsController : Controller
    {
        private readonly ILogger<EventsController> _logger;

        public EventsController(ILogger<EventsController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{eventID}/enable")]
        public ActionResult EnableEvent(string eventID)
        {
            try
            {
                var data = Main.Instance;

                var foundEvent = data.Events.SingleOrDefault(x => string.Compare(x.EventID, eventID, true) == 0);

                if (foundEvent != null)
                {
                    foundEvent.EventOn.Execute(eventID);
                }

                return Ok(@"{ ""success"":true }");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while enabling event ID '{eventID}'");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{eventID}/disable")]
        public ActionResult DisableEvent(string eventID)
        {
            try
            {
                var data = Main.Instance;

                var foundEvent = data.Events.SingleOrDefault(x => string.Compare(x.EventID, eventID, true) == 0);

                if (foundEvent != null)
                {
                    foundEvent.EventOff.Execute(eventID);
                }

                return Ok(@"{ ""success"":true }");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while disabling event ID '{eventID}'");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
