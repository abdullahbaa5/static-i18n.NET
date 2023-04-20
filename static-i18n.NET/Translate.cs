using HtmlAgilityPack;
using I18Next.Net;
using I18Next.Net.Plugins;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace static_i18n.NET
{
    public class Translate
    {
        private readonly Configuration _configuration;
        private readonly II18Next _i18nService;
        public Translate(Configuration configuration, II18Next i18n)
        {
            _configuration = configuration;
            _i18nService = i18n;
        }

        private async Task TranslateElem(HtmlNode elem, string locale)
        {
            string key, attr;
            if (_configuration.UseAttr && (attr = _configuration.Selector) != null)
            {
                key = elem.GetAttributeValue(attr, "");
            }
            else
            {
                key = elem.InnerText;
            }

            if (_configuration.RemoveAttr)
            {
                elem.Attributes.Remove(_configuration.Selector);
            }

            if (string.IsNullOrEmpty(key)) return;

            string trans = await _i18nService.Translator.TranslateAsync(locale, key, new Dictionary<string, object>(), new TranslationOptions() { DefaultNamespace = "translation" });

            if (_configuration.Replace)
            {
                var newElem = HtmlNode.CreateNode(trans);
                elem.ParentNode.ReplaceChild(newElem, elem);
            }
            else
            {
                if (_configuration.AllowHtml)
                {
                    elem.InnerHtml = trans;
                }
                else
                {
                    elem.InnerHtml = HtmlDocument.HtmlEncode(trans);
                }
            }
        }

        private async Task TranslateAttributes(HtmlNode elem, string locale)
        {
            bool shouldAttrInterpolate = false;

            HtmlAttribute attrInterpolate = elem.Attributes[_configuration.AttrInterpolateSelector];
            if (attrInterpolate != null)
            {
                shouldAttrInterpolate = true;
            }

            Dictionary<string, string> attrsToAdd = new Dictionary<string, string>();

            foreach (var attribute in elem.Attributes)
            {
                string key = attribute.Value;
                if (string.IsNullOrEmpty(key) || attribute.Name == _configuration.AttrSelector) continue;

                if (attribute.Name.EndsWith(_configuration.AttrSuffix))
                {
                    string attr = attribute.Name[..^_configuration.AttrSuffix.Length];

                    if (shouldAttrInterpolate)
                    {
                        string pattern = @"\{\{(.+?)\}\}";
                        MatchCollection matches = Regex.Matches(key, pattern);
                        List<string> extractedStrings = matches.Cast<Match>()
                                       .Select(m => m.Groups[1].Value)
                                       .ToList();

                        foreach (string extractedString in extractedStrings)
                        {
                            string trans = await _i18nService.Translator.TranslateAsync(locale, extractedString, new Dictionary<string, object>(), new TranslationOptions() { DefaultNamespace = "translation" });
                            key = key.Replace(("{{" + extractedString + "}}"), trans);
                        }

                        attrsToAdd.Add(attr, key);
                    }
                    else
                    {
                        key = await _i18nService.Translator.TranslateAsync(locale, key, new Dictionary<string, object>(), new TranslationOptions() { DefaultNamespace = "translation" });
                        attrsToAdd.Add(attr, key);
                    }
                }
            }

            foreach (var attrToAdd in attrsToAdd)
            {
                elem.SetAttributeValue(attrToAdd.Key, attrToAdd.Value);

                if (_configuration.RemoveAttr)
                {
                    elem.Attributes.Remove($"{attrToAdd.Key}{_configuration.AttrSuffix}");
                }
            }

            attrsToAdd = null;

            if (_configuration.RemoveAttr)
            {
                elem.Attributes.Remove(_configuration.AttrInterpolateSelector);
                elem.Attributes.Remove(_configuration.AttrSelector);
            }
        }

        public async Task<string> TranslateString(string html, string locale)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            int maxConcurrentTasks = 5; // add this to Configuration todo
            var semaphore = new SemaphoreSlim(maxConcurrentTasks);

            var elems = doc.DocumentNode.SelectNodes($"//*[@{_configuration.Selector}]") ?? Enumerable.Empty<HtmlNode>(); ;
            var elemsTasks = elems.Select(async elem =>
            {
                await semaphore.WaitAsync();
                try
                {
                    await TranslateElem(elem, locale);
                }
                finally
                {
                    semaphore.Release();
                }
            }).ToArray();

            var elemsAttr = doc.DocumentNode.SelectNodes($"//*[@{_configuration.AttrSelector}]") ?? Enumerable.Empty<HtmlNode>(); ;
            var elemsAttrTasks = elemsAttr.Select(async elem =>
            {
                await semaphore.WaitAsync();
                try
                {
                    await TranslateAttributes(elem, locale);
                }
                finally
                {
                    semaphore.Release();
                }
            }).ToArray();

            var allTasks = elemsTasks.Concat(elemsAttrTasks).ToArray();
            await Task.WhenAll(allTasks);

            return doc.DocumentNode.OuterHtml;
        }
    }
}
