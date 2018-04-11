﻿using DMXForGamers.Models;
using Nancy;
using Nancy.ErrorHandling;
using Nancy.ViewEngines;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DMXForGamers.Web
{
    // https://blog.terribledev.io/custom-error-pages-in-nancy/
    public class CustomStatusCode : IStatusCodeHandler
    {
        private static IEnumerable<int> _checks = new List<int>();

        public static IEnumerable<int> Checks { get { return _checks; } }

        private IViewRenderer viewRenderer;

        public CustomStatusCode(IViewRenderer viewRenderer)
        {
            this.viewRenderer = viewRenderer;
        }

        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            return (_checks.Any(x => x == (int)statusCode));
        }

        public static void AddCode(int code)
        {
            AddCode(new List<int>() { code });
        }
        public static void AddCode(IEnumerable<int> code)
        {
            _checks = _checks.Union(code);
        }

        public static void RemoveCode(int code)
        {
            RemoveCode(new List<int>() { code });
        }
        public static void RemoveCode(IEnumerable<int> code)
        {
            _checks = _checks.Except(code);
        }

        public static void Disable()
        {
            _checks = new List<int>();
        }

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            try
            {
                var response = viewRenderer.RenderView(context, "Codes/" + (int)statusCode + ".cshtml", Main.Instance);
                response.StatusCode = statusCode;
                context.Response = response;
            }
            catch (Exception)
            {
                RemoveCode((int)statusCode);
                context.Response.StatusCode = statusCode;
            }
        }
    }
}