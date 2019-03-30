namespace Be.Vlaanderen.Basisregisters.Api.Localization
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Microsoft.Extensions.Localization;

    public class SharedStringLocalizer<T> : IStringLocalizer
    {
        private readonly IStringLocalizer _primaryStringLocalizer;

        private readonly Lazy<IStringLocalizer<T>> _sharedStringLocalizer
            = new Lazy<IStringLocalizer<T>>(() => GlobalStringLocalizer.Instance.GetLocalizer<T>());

        public SharedStringLocalizer(IStringLocalizer primaryStringLocalizer) => _primaryStringLocalizer = primaryStringLocalizer;

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) => throw new NotSupportedException();

        public IStringLocalizer WithCulture(CultureInfo culture) => throw new NotSupportedException();

        public LocalizedString this[string name]
        {
            get
            {
                var initialTry = _primaryStringLocalizer[name];

                return initialTry.ResourceNotFound
                    ? _sharedStringLocalizer.Value[name]
                    : initialTry;
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var initialTry = _primaryStringLocalizer[name, arguments];

                return initialTry.ResourceNotFound
                    ? _sharedStringLocalizer.Value[name, arguments]
                    : initialTry;
            }
        }
    }
}
