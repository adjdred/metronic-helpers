using System;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MetronicHelpers
{
    [HtmlTargetElement("alert", Attributes = "message,alert-style,alert-type")]
    public class AlertTagHelper : TagHelper
    {
        [HtmlAttributeName("m-icon")]
        public string Icon { get; set; }

        [HtmlAttributeName("m-header")]
        public string Header { get; set; }

        [HtmlAttributeName("m-message")]
        public string Message { get; set; }

        [HtmlAttributeName("m-type")]
        public AlertType Type { get; set; }

        [HtmlAttributeName("m-style")]
        public AlertStyle Style { get; set; }
        
        [HtmlAttributeName("m-visible")]
        public bool? Visible { get; set; }

        private string GetStyleClass()
        {
            switch (Style)
            {
                case AlertStyle.Default:
                    return $"alert-{Type.ToString()}".ToLower();
                case AlertStyle.Outline:
                    return $"alert-outline-{Type.ToString()}".ToLower();
                case AlertStyle.Solid:
                    return $"alert-solid-{Type.ToString()}".ToLower();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(Message) || Visible.HasValue && !Visible.Value)
                return;

            var cssClass = $"alert {GetStyleClass()}";
            output.TagName = "div";

            output.Attributes.Add("class", cssClass);
            output.Attributes.Add("role", "alert");

            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(Icon))
            {
                sb.AppendFormat("<div class=\"alert-icon\"><i class='fa fa-{0}'></i></div>", Icon);
            }

            if (!string.IsNullOrEmpty(Header))
            {
                Header = $"<h4 class=\"alert-heading\">{ System.Net.WebUtility.HtmlEncode(Message)}</h4>";
            }

            sb.AppendFormat("<div class=\"alert-text\">{0}{1}</div>", Header, System.Net.WebUtility.HtmlEncode(Message));

            var actionContent = output.GetChildContentAsync().Result;

            if (actionContent !=null)
            {
                sb.Append(actionContent.GetContent());
            }

            output.Content.SetHtmlContent(sb.ToString());
        }
    }
    public enum AlertType
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

    public enum AlertStyle
    {
        Default,
        Outline,
        Solid
    }
}
