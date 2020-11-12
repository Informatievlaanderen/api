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
