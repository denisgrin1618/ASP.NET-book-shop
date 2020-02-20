using System;
using System.Text;
using System.Web.Mvc;
using BookStore.WebUI.Models;

namespace BookStore.WebUI.HtmlHelpers
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(  this HtmlHelper html,
                                                PagingInfo pagingInfo,
                                                Func<int, string> pageUrl)
        {

          
            StringBuilder result = new StringBuilder();
            TagBuilder Nav = new TagBuilder("nav");

            //Первая страница
            TagBuilder ul = new TagBuilder("ul");
            ul.AddCssClass("pagination");
                TagBuilder liStart = new TagBuilder("li");
                    TagBuilder aStart = new TagBuilder("a");
                    aStart.MergeAttribute("href", pageUrl(1));
                        TagBuilder span = new TagBuilder("span");
                        span.MergeAttribute("aria-hidden", "true");
                        span.InnerHtml += "&laquo";
                    aStart.InnerHtml += span.ToString();
                liStart.InnerHtml += aStart.ToString();
            ul.InnerHtml += liStart.ToString();

            //все остальные страницы
            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                TagBuilder li = new TagBuilder("li");
                if (i == pagingInfo.CurrentPage)
                    li.AddCssClass("active");

                TagBuilder a = new TagBuilder("a");
                a.MergeAttribute("href", pageUrl(i));                
                a.InnerHtml = i.ToString();

                li.InnerHtml += a.ToString();
                ul.InnerHtml += li.ToString();
            }

            //последняя страница
            TagBuilder liEnd = new TagBuilder("li");
                TagBuilder aEnd = new TagBuilder("a");
                aEnd.MergeAttribute("href", pageUrl(pagingInfo.TotalPages));
                    TagBuilder spanEnd = new TagBuilder("span");
                    spanEnd.MergeAttribute("aria-hidden", "true");
                    spanEnd.InnerHtml += "»";
                aEnd.InnerHtml += spanEnd.ToString();
            liEnd.InnerHtml += aEnd.ToString();
            ul.InnerHtml += liEnd.ToString();

            Nav.InnerHtml += ul.ToString();
            result.Append(Nav.ToString());

            return MvcHtmlString.Create(result.ToString());
        }
    }
}