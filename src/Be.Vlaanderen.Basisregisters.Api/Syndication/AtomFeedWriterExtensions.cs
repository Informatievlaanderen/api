namespace Be.Vlaanderen.Basisregisters.Api.Syndication
{
    using Microsoft.SyndicationFeed;
    using Microsoft.SyndicationFeed.Atom;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class AtomFeedWriterExtensions
    {
        /// <summary>
        /// Write default meta data for an atom feed
        /// </summary>
        /// <param name="atomFeedWriter">the AtomFeedWriter</param>
        /// <param name="id">the id of the feed</param>
        /// <param name="title">the title of the feed</param>
        /// <param name="version">the version of the feed</param>
        /// <param name="selfUri">the self referencing uri of the feed</param>
        /// <param name="relatedUrls">optional related urls</param>
        /// <returns></returns>
        [Obsolete("Use the extension method with the AtomFeedConfiguration parameter.")]
        public static async Task WriteDefaultMetadata(
            this AtomFeedWriter atomFeedWriter,
            string id,
            string title,
            string version,
            Uri selfUri,
            params string[] relatedUrls)
        {
            await atomFeedWriter.WriteDefaultMetadata(
                new AtomFeedConfiguration(
                    id,
                    title,
                    "Basisregisters Vlaanderen stelt u in staat om alles te weten te komen rond: de Belgische gemeenten; de Belgische postcodes; de Vlaamse straatnamen; de Vlaamse adressen; de Vlaamse gebouwen en gebouweenheden; de Vlaamse percelen; de Vlaamse organisaties en organen; de Vlaamse dienstverlening.",
                    "Basisregisters Vlaanderen",
                    "https://basisregisters.vlaanderen.be",
                    version,
                    $"Copyright (c) 2017-{DateTime.Now.Year}, Informatie Vlaanderen",
                    DateTimeOffset.Now,
                    new SyndicationPerson("agentschap Informatie Vlaanderen", "informatie.vlaanderen@vlaanderen.be",
                        AtomContributorTypes.Author),
                    new SyndicationLink(selfUri, AtomLinkTypes.Self),
                    new List<SyndicationLink>(),
                    relatedUrls.Select(x => new SyndicationLink(new Uri(x), AtomLinkTypes.Related)).ToList()));
        }

        /// <summary>
        /// Write default meta data for an atom feed
        /// </summary>
        /// <param name="atomFeedWriter">the AtomFeedWriter</param>
        /// <param name="atomFeedConfiguration">the configuration of the atom feed</param>
        /// <returns></returns>
        public static async Task WriteDefaultMetadata(
            this AtomFeedWriter atomFeedWriter,
            AtomFeedConfiguration atomFeedConfiguration)
        {
            await atomFeedWriter.WriteId(atomFeedConfiguration.Id);
            await atomFeedWriter.WriteTitle(atomFeedConfiguration.Title);
            await atomFeedWriter.WriteSubtitle(atomFeedConfiguration.Subtitle);
            await atomFeedWriter.WriteGenerator(atomFeedConfiguration.GeneratorTitle, atomFeedConfiguration.GeneratorUri, atomFeedConfiguration.GeneratorVersion);
            await atomFeedWriter.WriteRights(atomFeedConfiguration.Rights);
            await atomFeedWriter.WriteUpdated(atomFeedConfiguration.Updated);
            await atomFeedWriter.Write(atomFeedConfiguration.Author);
            await atomFeedWriter.Write(atomFeedConfiguration.SelfUri);

            foreach (var alternateUri in atomFeedConfiguration.AlternateUris)
                await atomFeedWriter.Write(alternateUri);

            foreach (var relatedUri in atomFeedConfiguration.RelatedUris)
                await atomFeedWriter.Write(relatedUri);
        }
    }
}
