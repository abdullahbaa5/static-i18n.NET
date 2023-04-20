using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace static_i18n.NET
{
    public class Process
    {
        private readonly Configuration _configuration;
        private readonly Translate _translate;

        public Process(Configuration configuration, Translate translate)
        {
            _configuration = configuration;
            _translate = translate; 
        }

        public async Task<string> ProcessLocaleAsync(string rawHtml, string locale)
        {
            return await _translate.TranslateString(rawHtml, locale);
        }

        public async Task<Dictionary<string, string>> ProcessAsync(string rawHtml)
        {
            Dictionary<string, string> results = new Dictionary<string, string>();

            foreach (string locale in _configuration.Locales)
            {
                results.Add(locale, await ProcessLocaleAsync(rawHtml, locale));
            }

            return results;
        }

        public async Task ProcessFileAsync(string file)
        {
            _configuration.LocaleFile = _configuration.LocaleFile ?? file;
            string html = await File.ReadAllTextAsync(file);
            var results = await ProcessAsync(html);

            if (_configuration.OutputDir != null)
            {
                foreach (var locale in _configuration.Locales)
                {
                    string output = GetOutput(file, locale);
                    await File.WriteAllTextAsync(output, results[locale]);
                }
            }
        }

        public async Task ProcessDirAsync()
        {
            var pattern = _configuration.Files ?? "*.html";
            var files = GetFilesRecursively(_configuration.BaseDir, pattern)
                .Where(f => !ShouldExcludeFile(f));

            int maxConcurrentTasks = 5; // add this to Configuration todo
            var semaphore = new SemaphoreSlim(maxConcurrentTasks);

            var tasks = files.Select(async file =>
            {
                await semaphore.WaitAsync();
                try
                {
                    await ProcessFileAsync(file);
                }
                finally
                {
                    semaphore.Release();
                }
            }).ToArray();

            await Task.WhenAll(tasks);
        }

        private IEnumerable<string> GetFilesRecursively(string directoryPath, string pattern)
        {
            var files = Directory.EnumerateFiles(directoryPath, pattern, SearchOption.TopDirectoryOnly);
            var directories = Directory.EnumerateDirectories(directoryPath);

            foreach (var directory in directories)
            {
                files = files.Concat(GetFilesRecursively(directory, pattern));
            }

            return files;
        }

        private bool ShouldExcludeFile(string file)
        {
            return _configuration.Exclude.Contains(file);
        }

        // needs rework too sleepy for this right now
        private string GetOutput(string file, string locale)
        {
            if (!Directory.Exists(_configuration.OutputDir)) Directory.CreateDirectory(_configuration.OutputDir);

            string localeOutputFolder = Path.Combine(_configuration.OutputDir, locale);
            if (!Directory.Exists(localeOutputFolder)) Directory.CreateDirectory(localeOutputFolder);

            CloneBaseDirInOutputLocale(_configuration.BaseDir, localeOutputFolder);

            return Path.Combine(_configuration.OutputDir, locale, file);

            // todo implement outputDefailt and outputOther
        }

        private void CloneBaseDirInOutputLocale(string baseDir, string outputLocale)
        {
            Directory.CreateDirectory(Path.Combine(outputLocale, baseDir));

            foreach (string directory in Directory.GetDirectories(baseDir)) {
                string path = Path.Combine(outputLocale, directory);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                CloneBaseDirInOutputLocale(Path.Combine(baseDir, directory), path);
            }
        }
    }
}
