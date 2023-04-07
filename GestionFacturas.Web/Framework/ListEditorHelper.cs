using System.Linq.Expressions;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GestionFacturas.Web.Framework
{
    //www.jarrettmeyer.com/post/2995732471/nested-collection-models-in-asp-net-mvc-3
    public static class HtmlHelpers
    {
        public static IHtmlContent LinkToRemoveNestedForm<TModel>(this IHtmlHelper<TModel> helper, string linkText, string container, string deleteElement)
        {
            var js = $"javascript:removeNestedForm(this,'{container}','{deleteElement}');return false;";

            var tb = new TagBuilder("a");

            tb.Attributes.Add("href", "#");

            tb.Attributes.Add("onclick", js);

            tb.InnerHtml.AppendHtml(linkText);
            
            return new HtmlContentBuilder().AppendHtml(tb);
        }
        
        public static IHtmlContent LinkToAddNestedForm<TModel, TProperty>(
            this IHtmlHelper<TModel> htmlHelper, 
            Expression<Func<TModel, TProperty>> expression, 
            string linkText, 
            string containerElement, 
            string counterElement, 
            string focusPropertyName, 
            object[] argValues = null, 
            string cssClass = null) where TProperty : IEnumerable<object>
        {
            // a fake index to replace with a real index
            long ticks = DateTime.UtcNow.Ticks;


            var expresionProvider = htmlHelper.ViewContext.HttpContext.RequestServices
                .GetService(typeof(ModelExpressionProvider)) as ModelExpressionProvider;
            // pull the name and type from the passed in expression
            string collectionProperty = expresionProvider.GetExpressionText(expression);

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

            var templateName = nestedObject.GetType().Name;

            var editor = htmlHelper.EditorFor(x => nestedObject, templateName);
            
            string partial = GetString(editor);


            // done rendering, reset prefix to old name
            htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix = oldPrefix;



            // strip out the fake name injected in (our name was all in the prefix)
            partial = Regex.Replace(partial, @"[\._]?nestedObject", "");



            // encode the output for javascript since we're dumping it in a JS string
            partial = HttpUtility.JavaScriptStringEncode(partial);
            
            // create the link to render
            var js = string.Format("javascript:addNestedForm('{0}','{1}','{2}','{3}','{4}');return false;", containerElement, counterElement, ticks, partial, focusId);
            var a = new TagBuilder("a");
            a.Attributes.Add("id", "añadir_" + templateName);
            a.Attributes.Add("href", "javascript:void(0)");
            a.Attributes.Add("onclick", js);
            if (cssClass != null)
            {
                a.AddCssClass(cssClass);
            }
            a.InnerHtml.AppendHtml("<i class='fa fa-plus'></i> " +linkText);

            
            return new HtmlContentBuilder().AppendHtml(a);
        }

        private static string GetString(IHtmlContent content)
        {
            using (var writer = new System.IO.StringWriter())
            {
                content.WriteTo(writer, HtmlEncoder.Default);
                return writer.ToString();
            }
        }

    }

  
}