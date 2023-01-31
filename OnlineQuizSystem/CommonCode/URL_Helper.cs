using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace OnlineQuizSystem.CommonCode
{
    public static class URL_Helper
    {
        public static string GetURLForPageNumber(int pageNumber, RequestContext reqContext)
        {
            string url = HttpContext.Current.Request.Url.AbsolutePath;
            string currentController = GetControllerAndActionFromURL(url)["Controller"];
            string currentAction = GetControllerAndActionFromURL(url)["Action"];
            RouteData rData = null;
            string retUrl = "";


            foreach (Route route in RouteTable.Routes)
            {
                if (route.RouteHandler is StopRoutingHandler)
                    continue;//This rout is Ignored
                HttpContextWrapper req = new HttpContextWrapper(HttpContext.Current);

                rData = route.GetRouteData(req);
                if (rData == null)
                    continue;

                string controller = rData.Values["controller"].ToString().ToLower();
                string action = rData.Values["action"].ToString().ToLower();
                string page = rData.Values.ContainsKey("pageId") ? rData.Values["pageId"].ToString() : pageNumber.ToString();
                if (!rData.Values.ContainsKey("pageId"))
                {
                    rData.Values.Add("pageId", page);
                }
                else
                {
                    rData.Values["pageId"] = pageNumber;
                }

                if (controller == currentController && action == currentAction)
                {
                    retUrl = route.Url;
                    if (!retUrl.Contains("{pageId}"))
                        retUrl = retUrl + "/{pageId}";
                    string[] strs = retUrl.Split("/".ToCharArray());

                    foreach (string str in strs)
                    {
                        object strWith = rData.Values[str.Replace("{", "").Replace("}", "")];
                        strWith = strWith ?? str;
                        retUrl = retUrl.Replace(str, strWith.ToString());
                    }
                    break;
                    //It is ok to return now we found the route date we need
                }
            }

            return reqContext.HttpContext.Request.QueryString.Count > 0 ? string.Format("{0}?{1}", retUrl, reqContext.HttpContext.Request.QueryString.ToString()) : retUrl;
        }

        private static Dictionary<string, string> GetControllerAndActionFromURL(string strRequestedUrl)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            //string strRequestedUrl = HttpContext.Current.Request.Url.AbsolutePath.Replace(HttpContext.Current.Request.ApplicationPath + "/", "").ToLower();

            int reqUrlSplitedCount = strRequestedUrl.Split("/".ToCharArray()).Count();
            foreach (Route route in RouteTable.Routes)
            {
                if (route.RouteHandler is StopRoutingHandler)
                    continue;//This rout is Ignored
                HttpContextWrapper req = new HttpContextWrapper(HttpContext.Current);

                RouteData rDate = route.GetRouteData(req);
                if (rDate == null)
                    continue;
                string controller = rDate.Values["controller"].ToString().ToLower();
                string action = rDate.Values["action"].ToString().ToLower();
                string page = rDate.Values.ContainsKey("pageId") ? rDate.Values["pageId"].ToString() : "1";
                //TODO: We had to verify this
                string routeUrl = route.Url.Replace("{controller}", controller).Replace("{action}", action);

                // if (routeUrl.Contains(strRequestedUrl))
                {
                    result.Add("Controller", controller);
                    result.Add("Action", action);
                    break;
                }
            }
            return result;
        }

    }
}