using CommandLine;
using System.Collections.Generic;

namespace static_i18.NET.ConsoleApp
{
    public class CommandLineOptions
    {
        [Option("selector", HelpText = "The selector to look for elements to translate.")]
        public string Selector { get; set; }

        [Option("attr-selector", HelpText = "The selector to look for elements with attributes to translate.")]
        public string AttrSelector { get; set; }

        [Option("interpolate-selector", HelpText = "The selector that should be applied to elements to indicate that interpolation should be performed.")]
        public string InterpolateSelector { get; set; }

        [Option("attr-interpolate-selector", HelpText = "The selector that should be applied to elements to indicate that interpolation should be performed for the custom attributes.")]
        public string AttrInterpolateSelector { get; set; }

        [Option("attr-suffix", HelpText = "Suffix for attr to translate. value-t will be translated and mapped to value.")]
        public string AttrSuffix { get; set; }

        [Option("attr-interpolate-suffix ", HelpText = "Suffix for attr to interpolate the translations.")]
        public string AttrInterpolateSuffix { get; set; }

        [Option("file-format", HelpText = "Enables JSON, XML or INI mode for HtmlAgilityPack.")]
        public string FileFormat { get; set; }

        [Option("use-attr", HelpText = "If false, the element text is always used as the key, even if the attribute value is not empty.")]
        public bool UseAttr { get; set; } = true;

        [Option("replace", HelpText = "If true, the element is replaced by the translation.")]
        public bool Replace { get; set; } = false;

        [Option("locales", Separator = ',', HelpText = "The list of locales to be generated. All locales need to have an existing resource file.")]
        public IEnumerable<string> Locales { get; set; }

        [Option("exclude", Separator = ',', HelpText = "Files to exclude. Can contain regex (uses normal test) or string (uses startsWith).")]
        public IEnumerable<string> Exclude { get; set; }

        [Option("locale", HelpText = "The default locale.")]
        public string Locale { get; set; }

        [Option("files", HelpText = "The files to translate, relative to the base directory.")]
        public string Files { get; set; }

        [Option("base-dir", Required = true, HelpText = "The base directory to look for translations.")]
        public string BaseDir { get; set; }

        [Option("allow-html", HelpText = "Allow the usage of HTML in translation.")]
        public bool AllowHtml { get; set; } = false;

        [Option("remove-attr", HelpText = "When true, removes the attribute used to translate (e.g. data-t will be removed)")]
        public bool RemoveAttr { get; set; } = true;    

        [Option("output-dir", HelpText = "The directory to output generated files.")]
        public string OutputDir { get; set; }

        [Option("output-default", HelpText = "The name for the default locale output files, relative to outputDir.")]
        public string OutputDefault { get; set; }

        [Option("output-other", HelpText = "The name for the other locales output files, relative to outputDir.")]
        public string OutputOther { get; set; }

        [Option("locales-path", HelpText = "The directory of the translations, where each file should be named LOCALE_NAME.json.")]
        public string LocalesPath { get; set; }
    }
}
