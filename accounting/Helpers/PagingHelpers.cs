using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace accounting.Helpers
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfo pagingInfo, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            TagBuilder liTag;
            TagBuilder aTag;

            if (pagingInfo.ItemVer < pagingInfo.TotalPages)
            {
                liTag = new TagBuilder("li");
                aTag = new TagBuilder("a");

                aTag.MergeAttribute("href", pageUrl(1));
                aTag.InnerHtml = pagingInfo.ItemIni;
                liTag.InnerHtml += aTag.ToString();
                liTag.AddCssClass("single");

                result.Append(liTag.ToString());

                liTag = new TagBuilder("li");
                aTag = new TagBuilder("a");

                if (pagingInfo.CurrentPage == 1)
                    aTag.MergeAttribute("href", pageUrl(1));
                else
                    aTag.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage - 1));

                aTag.InnerHtml = pagingInfo.ItemAnt;
                aTag.MergeAttribute("title", "Anterior");
                liTag.InnerHtml += aTag.ToString();

                liTag.AddCssClass("single");
                result.Append(liTag.ToString());
            }

            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                liTag = new TagBuilder("li");
                aTag = new TagBuilder("a");

                aTag.MergeAttribute("href", pageUrl(i));
                aTag.InnerHtml = i.ToString();

                if (i == pagingInfo.CurrentPage)
                    liTag.AddCssClass("current");

                liTag.InnerHtml += aTag.ToString();

                if (i == 1 || i == pagingInfo.TotalPages)
                {
                    liTag.AddCssClass("single");
                    result.Append(liTag.ToString());
                }
                else
                {
                    if ((i >= Math.Abs(pagingInfo.CurrentPage - pagingInfo.Intervalo) && i <= Math.Abs(pagingInfo.CurrentPage + pagingInfo.Intervalo))
                        || (pagingInfo.CurrentPage <= 2 && i <= pagingInfo.Intervalo))
                    {

                        liTag.AddCssClass("single");
                        result.Append(liTag.ToString());
                    }
                    else
                    {
                        liTag = new TagBuilder("li");
                        aTag = new TagBuilder("a");

                        aTag.MergeAttribute("href", pageUrl(i));
                        aTag.InnerHtml = pagingInfo.ItemInt;
                        liTag.InnerHtml += aTag.ToString();
                        liTag.AddCssClass("single");
                        result.Append(liTag.ToString());

                        if (i < pagingInfo.CurrentPage)
                            i = Math.Abs(pagingInfo.CurrentPage - pagingInfo.Intervalo) - 1;
                        else
                            i = pagingInfo.TotalPages - 1;
                    }
                }
            }

            if (pagingInfo.ItemVer < pagingInfo.TotalPages)
            {
                liTag = new TagBuilder("li");
                aTag = new TagBuilder("a");

                if (pagingInfo.CurrentPage == pagingInfo.TotalPages)
                    aTag.MergeAttribute("href", pageUrl(pagingInfo.TotalPages));
                else
                    aTag.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage + 1));

                aTag.InnerHtml = pagingInfo.ItemSig;
                aTag.MergeAttribute("title", "Siguiente");
                liTag.InnerHtml += aTag.ToString();
                liTag.AddCssClass("single");
                result.Append(liTag.ToString());

                liTag = new TagBuilder("li");
                aTag = new TagBuilder("a");

                aTag.MergeAttribute("href", pageUrl(pagingInfo.TotalPages));
                aTag.InnerHtml = pagingInfo.ItemFin;
                liTag.InnerHtml += aTag.ToString();
                liTag.AddCssClass("single");
                result.Append(liTag.ToString());
            }

            return MvcHtmlString.Create(result.ToString());
        }
    }
}