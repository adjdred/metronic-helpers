using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MetronicHelpers
{
    [RestrictChildren("portlet-head", "portlet-body", "portlet-foot","form")]
    [HtmlTargetElement("portlet")]
    public class PortletTagHelper : TagHelper
    {
        [HtmlAttributeName("id")]
        public string Id { get; set; }

        [HtmlAttributeName("m-type")]
        public PortletType Type { get; set; }

        [HtmlAttributeName("m-style")]
        public PortletStyle Style { get; set; }
        private string GetStyleClass()
        {
            switch (Style)
            {
                case PortletStyle.Default:
                    return string.Empty;
                case PortletStyle.Solid:
                    return $"kt-portlet-solid kt-bg-{Type.ToString()}".ToLower();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.Add("class", $"kt-portlet {GetStyleClass()}");

            if (!string.IsNullOrEmpty(Id))
                output.Attributes.Add("id", Id);
        }
    }
    [RestrictChildren("a")]
    [HtmlTargetElement("portlet-head", ParentTag = "portlet")]
    public class PortletHeadTagHelper : TagHelper
    {
        [HtmlAttributeName("m-title")]
        public string Title { get; set; }
        [HtmlAttributeName("m-sub-title")]
        public string SubTitle { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(Title))
                return;

            output.TagName = "div";
            output.Attributes.Add("class", "kt-portlet__head");

            var labelTag = new TagBuilder("div");
            labelTag.AddCssClass("kt-portlet__head-label");

            var titleTag = new TagBuilder("h3");
            titleTag.AddCssClass("kt-portlet__head-title");
            titleTag.InnerHtml.Append(Title);

            if (!string.IsNullOrEmpty(SubTitle))
            {
                var subTitleTag = new TagBuilder("small");
                subTitleTag.InnerHtml.Append(SubTitle);
                titleTag.InnerHtml.AppendHtml(subTitleTag);
            }

            labelTag.InnerHtml.AppendHtml(titleTag);
            output.Content.AppendHtml(labelTag);

            var toolbarContent = output.GetChildContentAsync().Result;

            if (toolbarContent == null) return;

            var toolbar = new TagBuilder("div");
            toolbar.AddCssClass("kt-portlet__head-toolbar");

            var actions = new TagBuilder("div");
            actions.AddCssClass("kt-portlet__head-actions");
            actions.InnerHtml.AppendHtml(toolbarContent);

            toolbar.InnerHtml.AppendHtml(actions);

            output.Content.AppendHtml(toolbar);
        }
    }

    [HtmlTargetElement("portlet-body", ParentTag = "portlet")]
    public class PanelBodyTagHelper : TagHelper
    {
        public override  void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.Add("class", "kt-portlet__body");
        }
    }

    [HtmlTargetElement("portlet-foot", ParentTag = "portlet")]
    public class PanelFootTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.Add("class", "kt-portlet__foot");
        }
    }

    public enum PortletType
    {
        Primary,
        Secondary,
        Success,
        Danger,
        Warning,
        Info,
        Light,
        Dark
    }

    public enum PortletStyle
    {
        Default,
        Solid
    }
}
