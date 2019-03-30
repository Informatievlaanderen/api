namespace Be.Vlaanderen.Basisregisters.Api.Localization
{
    using System;
    using Microsoft.Extensions.Localization;

    public static class IStringLocalizerExtensions
    {
        public static string GetString(
            this IStringLocalizer target,
            Func<string> model)
            => target.GetString(model());

        public static string GetString(
            this IStringLocalizer target,
            Func<string> model,
            params object[] formatArguments)
            => target.GetString(model(), formatArguments);

        public static string GetString<T>(
            this IStringLocalizer<T> target,
            Func<T, string> model) where T : new()
            => target.GetString(model(new T()));

        public static string GetString<T>(
            this IStringLocalizer<T> target,
            Func<T, string> model,
            params object[] formatArguments) where T : new()
            => target.GetString(model(new T()), formatArguments);
    }
}
