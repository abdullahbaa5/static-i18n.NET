# static-i18n.net

#### This C# .NET project is similar to the NodeJS static-i18n package (https://www.npmjs.com/package/static-i18n).

**Simple Usage:**

```
Configuration config = new Configuration();
StaticI18n myClass = await StaticI18n.CreateAsync(config);
await myClass.GenerateToOutputFolder();
```

You can easily configure the class by specifying settings such as the directories for your HTML files and locales, as well as a list of available locales. Once you have configured the class, you can use the GenerateToOutputFolder() function to generate localized files and save them to your desired output folder.

**Locales Files Formats:**
- JSON
- XML
- INI

Most of the features and configuration variables have been implemented, but some are still in progress.

**Files and their HTML elements are processed asynchronously.**
Currently, five jobs are being handled simultaneously. A configuration option is being developed to allow for easy modification of the number of concurrent jobs without requiring any code changes.

**Thanks to:**
@DarkLiKally for the .NET port of I18Next.
