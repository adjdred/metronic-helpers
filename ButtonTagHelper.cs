using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MetronicHelpers
{
    [HtmlTargetElement("a")]
    public class ButtonTagHelper : TagHelper
    {
        [HtmlAttributeName("m-tooltip")]
        public string Tooltip { get; set; }

        [HtmlAttributeName("m-tooltip-placement")]
        public string TooltipPlacement
        {
            get => string.IsNullOrEmpty(_tooltipPlacement) ? "top" : _tooltipPlacement;
            set => _tooltipPlacement = value;
        }

        private string _tooltipPlacement;

        [HtmlAttributeName("m-icon")]
        public string Icon { get; set; }

        [HtmlAttributeName("m-icon-size")]
        public ButtonIconSize? IconSize { get; set; }

        [HtmlAttributeName("m-type")]
        public ButtonType Type { get; set; }

        [HtmlAttributeName("m-style")]
        public ButtonStyle Style { get; set; }
        private string GetStyleClass()
        {
            switch (Style)
            {
                case ButtonStyle.Solid:
                    return $"btn btn-{Type.ToString()}".ToLower();
                case ButtonStyle.Outline:
                    return $"btn btn-outline-{Type.ToString()}".ToLower();
                default:
                    return string.Empty;
            }
        }

        private string GetIconClass()
        {
            if(string.IsNullOrEmpty(Icon))
                return string.Empty;

            switch (IconSize)
            {
                case ButtonIconSize.Small:
                    return "btn-icon btn-icon-sm";
                case ButtonIconSize.Medium:
                    return "btn-icon btn-icon-md";
                case ButtonIconSize.Large:
                    return "btn-icon btn-icon-lg";
                default:
                    return string.Empty;
            }
        }

        public override  void Process(TagHelperContext context, TagHelperOutput output)
        {
            var currentClass = output.Attributes.ContainsName("class") ? output.Attributes["class"].Value : string.Empty;
            output.Attributes.SetAttribute("class", $"{GetStyleClass()} {GetIconClass()} {currentClass}");

            if (!string.IsNullOrEmpty(Icon) && string.IsNullOrEmpty(Tooltip))
                throw new Exception("Icons must have a tooltip");

            if (!string.IsNullOrEmpty(Tooltip))
            {
                output.Attributes.SetAttribute("data-toggle","tooltip");
                output.Attributes.SetAttribute("data-placement", TooltipPlacement);
                output.Attributes.SetAttribute("title", Tooltip);
            }

            if (!string.IsNullOrEmpty(Icon))
            {
                var iconTag = new TagBuilder("i");
                iconTag.AddCssClass($"fa fa-{Icon}");

                output.PreContent.AppendHtml(iconTag);
            }
        }
    }

    public enum ButtonIconSize
    {
        Medium,
        Small,
        Large
    }

    public enum ButtonType
    {
        Blank,
        Primary,
        Secondary,
        Success,
        Danger,
        Warning,
        Info
    }

    public enum ButtonStyle
    {
        Blank,
        Solid,
        Outline
    }
}
