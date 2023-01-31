using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineQuizSystem.CommonCode
{
    public static class ExtensionMethods
    {
        public static string PagerPageLink(this HtmlHelper helper, int pageId, System.Web.Routing.RequestContext reqContext  , PagerHelper pager)
        {

            if (pager.IsForSearchPage)
            {
                return PagerPageLinkForSearch(helper, pageId, pageId.ToString(), reqContext, pager);
            }
            UrlHelper urlHelper = new UrlHelper(reqContext);

            //string url = "#";
            //---prevent javascipt event default by adding '!' symbole with hash
            string url = "#!";
            if (!pager.AjaxEnabled)
            {
                url = urlHelper.Content(string.Format("~/{0}", URL_Helper.GetURLForPageNumber(pageId, reqContext)));
            }
            return string.Format("<a class='page-link' href=\"{0}\" {2} rel='{3}'>{1}</a>", url, pageId, pager.GetAjaxRelatedAttributes(), pageId);
        }


        public static string PagerPageLink(this HtmlHelper helper, int pageId, System.Web.Routing.RequestContext reqContext, string linkText  , PagerHelper pager)
        {
            if (pager.IsForSearchPage)
            {
                return PagerPageLinkForSearch(helper, pageId, linkText, reqContext, pager);
            }
            UrlHelper urlHelper = new UrlHelper(reqContext);
            //string url = "#!";
            //---prevent javascipt event default by adding '!' symbole with hash
            string url = "#!";
            if (!pager.AjaxEnabled)
                url = urlHelper.Content(string.Format("~/{0}", URL_Helper.GetURLForPageNumber(pageId, reqContext)));
            return string.Format("<a class='page-link'  href=\"{0}\" {2} rel='{3}'>{1}</a>", url, linkText, pager.GetAjaxRelatedAttributes(), pageId );
        }

        public static string PagerPageLinkForSearch(this HtmlHelper helper, int pageNo, string linkText, System.Web.Routing.RequestContext reqContext, PagerHelper pager)
        {

            UrlHelper urlHelper = new UrlHelper(reqContext);

            string url = urlHelper.Content(string.Format("~/{0}", URL_Helper.GetURLForPageNumber(pageNo, reqContext)));

            string refUrl = null;
            if (HttpContext.Current.Request.HttpMethod == @"GET")
            {
                refUrl = HttpContext.Current.Request.RawUrl;
            }
            else
            {
                if (HttpContext.Current.Request.UrlReferrer != null)
                    refUrl = HttpContext.Current.Request.UrlReferrer.ToString();
                if (refUrl == null) refUrl = HttpContext.Current.Request.RawUrl;
            }

            string refUrlParams = refUrl.Substring(refUrl.IndexOf('?') + 1);
            var refQryStrings = HttpUtility.ParseQueryString(refUrlParams);
            var refKeyword = HttpContext.Current.Server.UrlEncode(refQryStrings[@"Keyword"]);

            string keywordTxt = string.IsNullOrEmpty(refKeyword) ? "" : string.Format("?keyword={0}", refKeyword);

            url = url + keywordTxt;

            return string.Format("<a href=\"{0}\" rel='{2}'>{1}</a>", url, linkText, pageNo);
        }
    }
}