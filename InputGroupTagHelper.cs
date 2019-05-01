using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MetronicHelpers
{
    [RestrictChildren("input-group-prepend", "input-group-append", "input")]
    [HtmlTargetElement("input-group")]
    public class InputGroupTagHelper : TagHelper
    {

        public override async void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "input-group");

            var inputGroupContext = new InputGroupContext();
            context.Items.Add(typeof(InputGroupTagHelper), inputGroupContext);

            await output.GetChildContentAsync();

            if (inputGroupContext.Prepend != null)
            {
                var prependTag = new TagBuilder("div");
                prependTag.AddCssClass("input-group-prepend");

                prependTag.InnerHtml.AppendHtml(inputGroupContext.Prepend);
                output.PreContent.AppendHtml(prependTag);
            }


            if (inputGroupContext.Append != null)
            {
                var appendTag = new TagBuilder("div");
                appendTag.AddCssClass("input-group-append");

                appendTag.InnerHtml.AppendHtml(inputGroupContext.Append);
                output.PostContent.AppendHtml(appendTag);
            }
        }
    }


    [HtmlTargetElement("input-group-prepend", ParentTag = "input-group")]
    public class InputGroupPrependTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var inputGroupContext = (InputGroupContext)context.Items[typeof(InputGroupTagHelper)];
            inputGroupContext.Prepend = output;
            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("input-group-append", ParentTag = "input-group")]
    public class InputGroupAppendTagHelper : TagHelper
    {

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var inputGroupContext = (InputGroupContext)context.Items[typeof(InputGroupTagHelper)];
            inputGroupContext.Append = output;
            output.SuppressOutput();
        }
    }

    public class InputGroupContext
    {
        public IHtmlContent Prepend { get; set; }
        public IHtmlContent Append { get; set; }
    }
}
