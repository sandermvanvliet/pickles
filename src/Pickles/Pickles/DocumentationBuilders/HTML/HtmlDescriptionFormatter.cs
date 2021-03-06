﻿#region License

/*
    Copyright [2011] [Jeffrey Cameron]

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

#endregion

using System;
using System.Text.RegularExpressions;
using System.Xml.Linq;

using PicklesDoc.Pickles.Extensions;

namespace PicklesDoc.Pickles.DocumentationBuilders.HTML
{
    public class HtmlDescriptionFormatter
    {
      private readonly MarkdownProvider markdown;

        private readonly XNamespace xmlns;

        public HtmlDescriptionFormatter(MarkdownProvider markdown)
        {
            this.markdown = markdown;
            this.xmlns = HtmlNamespace.Xhtml;
        }

        public XElement Format(string descriptionText)
        {
            if (String.IsNullOrEmpty(descriptionText)) return null;

            string markdownResult = "<div>" + this.markdown.Transform(descriptionText) + "</div>";

            var regex = new Regex(@"<p>(.*?)<\/p>", RegexOptions.Singleline);
            var markdownResultWithNewlinesFormatted = regex
                .Replace(
                    markdownResult,
                    m => m.Value
                        .Replace(
                            m.Groups[1].Value,
                            m.Groups[1].Value.Replace("\r\n", "\n").Replace("\n", "<br />\n")));

            XElement descriptionElements = XElement.Parse(markdownResultWithNewlinesFormatted);
            descriptionElements.SetAttributeValue("class", "description");

            descriptionElements.MoveToNamespace(this.xmlns);

            return descriptionElements;
        }
    }
}