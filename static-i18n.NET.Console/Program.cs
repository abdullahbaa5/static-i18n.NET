using CommandLine;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace static_i18n.NET.ConsoleApp
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            await Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsedAsync(RunAsync);

            Console.WriteLine($"END Exit code= {Environment.ExitCode}");
        }
        static async Task RunAsync(CommandLineOptions opts)
        {
            var config = new Configuration
            {
                Selector = opts.Selector,
                AttrSelector = opts.AttrSelector,
                AttrInterpolateSelector = opts.AttrInterpolateSelector,
                AttrSuffix = opts.AttrSuffix,
                AttrInterpolateSuffix = opts.AttrInterpolateSuffix,
                UseAttr = opts.UseAttr,
                // NsSeparator
                Replace = opts.Replace,
                Locales = opts.Locales.ToList(),
                // FixPaths
                Locale = opts.Locale,
                Exclude = opts.Exclude.ToList(),
                // Encoding
                Files = opts.Files,
                BaseDir = opts.BaseDir,
                // TranslateConditionalComments
                AllowHtml = opts.AllowHtml,
                RemoveAttr = opts.RemoveAttr,
                OutputDir = opts.OutputDir,
                OutputDefault = opts.OutputDefault,
                OutputOther = opts.OutputOther,
                LocalesPath = opts.LocalesPath,
                // OutputOverride
                // i18n
                FileFormat = opts.FileFormat
            };

            StaticI18n myClass = await StaticI18n.CreateAsync(config);
            await myClass.GenerateToOutputFolder();
        }   
    }
}
