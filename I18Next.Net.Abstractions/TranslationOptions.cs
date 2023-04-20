using System;

namespace I18Next.Net;

public class TranslationOptions
{
    private string _defaultNamespace;

    public string DefaultNamespace
    {
        get => _defaultNamespace;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(nameof(value));

            _defaultNamespace = value;
        }
    }

    private string _localeFileFormat;
    public string LocaleFileFormat
    {
        get => _localeFileFormat;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(nameof(value));

            _localeFileFormat = value;
        }
    }

    public string[] FallbackLanguages { get; set; }

    public string[] FallbackNamespaces { get; set; }
}
