using System;
using System.Collections.Generic;

namespace static_i18n.NET
{
    public class Configuration
    {
        private string selector = "data-t";
        public string Selector
        {
            get
            {
                return selector;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value)) selector = value;
            }
        }

        private string attrSelector = "data-attr-t";
        public string AttrSelector
        {
            get
            {
                return attrSelector;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value)) attrSelector = value;
            }
        }

        private string interpolateSelector = "data-t-interpolate";
        public string InterpolateSelector
        {
            get
            {
                return interpolateSelector;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value)) interpolateSelector = value;
            }
        }

        private string attrInterpolateSelector = "data-attr-t-interpolate";
        public string AttrInterpolateSelector
        {
            get
            {
                return attrInterpolateSelector;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value)) attrInterpolateSelector = value;
            }
        }

        private string locale = "en";
        public string Locale
        {
            get
            {
                return locale;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value)) locale = value;
            }
        }

        private string files = "*.html";
        public string Files
        {
            get
            {
                return files;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value)) files = value;
            }
        }

        private string baseDir = "data";
        public string BaseDir
        {
            get
            {
                return baseDir;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value)) baseDir = value;
            }
        }

        private string outputDir = "output";
        public string OutputDir
        {
            get
            {
                return outputDir;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value)) outputDir = value;
            }
        }

        private string attrSuffix = "-t";
        public string AttrSuffix
        {
            get
            {
                return attrSuffix;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value)) attrSuffix = value;
            }
        }

        private string attrInterpolateSuffix = "-t-interpolate";
        public string AttrInterpolateSuffix
        {
            get
            {
                return attrInterpolateSuffix;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value)) attrInterpolateSuffix = value;
            }
        }

        private LocaleFileType fileFormat = LocaleFileType.json;
        public string FileFormat
        {
            get
            {
                return fileFormat.ToString();
            }
            set
            {
                if (value == null)
                {
                    fileFormat = LocaleFileType.json;
                    return;
                }

                if (!Enum.TryParse<LocaleFileType>(value, true, out LocaleFileType result))
                {
                    throw new Exception("Allowed Locale File Format: json, xml, ini");
                }

                fileFormat = result;
            }
        }

        private string localeFile = "__lng__.__fmt__";
        public string LocaleFile
        {
            get
            {
                return localeFile;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value)) localeFile = value;
            }
        }

        private string outputDefault = "__file__";
        public string OutputDefault
        {
            get
            {
                return outputDefault;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value)) outputDefault = value;
            }
        }

        private string outputOther = "__lng__/__file__";
        public string OutputOther
        {
            get
            {
                return outputOther;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value)) outputOther = value;
            }
        }

        private string localesPath = "locales";
        public string LocalesPath
        {
            get
            {
                return localesPath;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value)) localesPath = value;
            }
        }

        private string nsSeparator = ":";
        public string NsSeparator
        {
            get
            {
                return nsSeparator;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value)) nsSeparator = value;
            }
        }

        private string encoding = "utf8";
        public string Encoding
        {
            get
            {
                return encoding;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value)) encoding = value;
            }
        }

        private List<string> locales = new List<string> { "en" };
        public List<string> Locales
        {
            get
            {
                return locales;
            }
            set
            {
                if (value != null && value.Count > 0) locales = value;
            }
        }

        public bool UseAttr { get; set; } = true;
        public bool Replace { get; set; } = false;
        public bool RemoveAttr { get; set; } = true;
        public bool AllowHtml { get; set; } = false;
        public List<string> Exclude { get; set; } = new List<string>();
        public Dictionary<string, Dictionary<string, string>> OutputOverride { get; set; } = new();
        public bool TranslateConditionalComments { get; set; } = false;
    }

    public enum LocaleFileType
    {
        json,
        xml,
        ini,
    }
}
