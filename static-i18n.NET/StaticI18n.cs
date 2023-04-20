using I18Next.Net.Backends;
using I18Next.Net.Extensions.Builder;
using I18Next.Net;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System;

namespace static_i18.NET
{
    public class StaticI18n
    {
        private Configuration _configuration;
        private II18Next _i18nService;

        private Process _process;
        private Translate _translate;

        public static async Task<StaticI18n> CreateAsync(Configuration configuration)
        {
            StaticI18n myClass = new StaticI18n(configuration);
            
            myClass.Init();
            await myClass.LoadAllLocales();

            return myClass;
        }

        public async Task GenerateToOutputFolder()
        {
            await _process.ProcessDirAsync();
        }

        private StaticI18n(Configuration configuration)
        {
            _configuration = configuration;
        }

        private void Init()
        {
            var services = new ServiceCollection();
            var i18nBuilder = new I18NextBuilder(services);

            i18nBuilder
            .UseDefaultNamespace("translation")
            .UseDefaultLanguage(_configuration.Locale)
            .UseDefaultLocaleFileFormat(_configuration.FileFormat);
            
            switch (_configuration.FileFormat)
            {
                case "json":
                    i18nBuilder.AddBackend(new JsonFileBackend(_configuration.LocalesPath));
                    break;

                case "xml":
                    i18nBuilder.AddBackend(new XmlFileBackend(_configuration.LocalesPath));
                    break;

                case "ini":
                    i18nBuilder.AddBackend(new IniFileBackend(_configuration.LocalesPath));
                    break;

                default:
                    throw new Exception("Unknown File Format. Allowed File Format: json, xml, and ini");
            }

            i18nBuilder.Build();

            var serviceProvider = services.BuildServiceProvider();
            _i18nService = serviceProvider.GetRequiredService<II18Next>();

            _translate = new Translate(_configuration, _i18nService);
            _process = new Process(_configuration, _translate);
        }

        private async Task LoadAllLocales()
        {
            foreach (string locale in _configuration.Locales)
            {
                await _i18nService.Backend.LoadNamespaceAsync(locale, "translation");
            }
        }

        public II18Next GetI18nService()
        {
            return _i18nService;
        }
    }
}
