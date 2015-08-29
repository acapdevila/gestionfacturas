using System.Collections.Generic;
using System.Web.Mvc.Html;
using System.Text.RegularExpressions;
using System.Linq.Expressions;

namespace System.Web.Mvc
{
    //www.jarrettmeyer.com/post/2995732471/nested-collection-models-in-asp-net-mvc-3
    public static class HtmlHelpers
    {
        public static IHtmlString LinkToRemoveNestedForm(this HtmlHelper helper, string linkText, string container, string deleteElement)
        {

            var js = string.Format("javascript:removeNestedForm(this,'{0}','{1}');return false;", container, deleteElement);

            var tb = new TagBuilder("a");

            tb.Attributes.Add("href", "#");

            tb.Attributes.Add("onclick", js);

            tb.InnerHtml = linkText;

            var tag = tb.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(tag);

        }

        public static MvcHtmlString LinkToAddNestedForm<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string linkText, string containerElement, string counterElement, string focusPropertyName, object[] argValues = null, string cssClass = null) where TProperty : IEnumerable<object>
        {
            // a fake index to replace with a real index
            long ticks = DateTime.UtcNow.Ticks;
            // pull the name and type from the passed in expression
            string collectionProperty = ExpressionHelper.GetExpressionText(expression);

            var nestedObject = Activator.CreateInstance(typeof(TProperty).GetGenericArguments()[0], argValues);

            // save the field prefix name so we can reset it when we're doing
            string oldPrefix = htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix;
            // if the prefix isn't empty, then prepare to append to it by appending another delimiter
            if (!string.IsNullOrEmpty(oldPrefix))
            {
                htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix += ".";
            }
            // append the collection name and our fake index to the prefix name before rendering
            htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix += string.Format("{0}[{1}]", collectionProperty, ticks);

            var focusId = string.Format("#{0}_{1}", htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix.Replace(".", "_").Replace("[", "_").Replace("]", "_"), focusPropertyName);
          
            string partial = htmlHelper.EditorFor(x => nestedObject).ToHtmlString();


            // done rendering, reset prefix to old name
            htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix = oldPrefix;



            // strip out the fake name injected in (our name was all in the prefix)
            partial = Regex.Replace(partial, @"[\._]?nestedObject", "");



            // encode the output for javascript since we're dumping it in a JS string
            partial = HttpUtility.JavaScriptStringEncode(partial);



            // create the link to render
            var js = string.Format("javascript:addNestedForm('{0}','{1}','{2}','{3}','{4}');return false;", containerElement, counterElement, ticks, partial,focusId);
            var a = new TagBuilder("a");
            a.Attributes.Add("href", "javascript:void(0)");
            a.Attributes.Add("onclick", js);
            if (cssClass != null)
            {
                a.AddCssClass(cssClass);
            }
            a.InnerHtml = linkText;



            return MvcHtmlString.Create(a.ToString(TagRenderMode.Normal));
        }

    }  
}