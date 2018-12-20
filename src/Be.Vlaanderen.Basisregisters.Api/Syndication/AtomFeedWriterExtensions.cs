namespace Be.Vlaanderen.Basisregisters.Api.Syndication
{
    using Microsoft.SyndicationFeed;
    using Microsoft.SyndicationFeed.Atom;
    using System;
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
        public static async Task WriteDefaultMetadata(
            this AtomFeedWriter atomFeedWriter,
            string id,
            string title,
            string version,
            Uri selfUri,
            params string[] relatedUrls)
        {
            await atomFeedWriter.WriteId(id);
            await atomFeedWriter.WriteTitle(title);
            await atomFeedWriter.WriteSubtitle("Basisregisters Vlaanderen stelt u in staat om alles te weten te komen rond: de Belgische gemeenten; de Belgische postcodes; de Vlaamse straatnamen; de Vlaamse adressen; de Vlaamse gebouwen en gebouweenheden; de Vlaamse percelen; de Vlaamse organisaties en organen; de Vlaamse dienstverlening.");
            await atomFeedWriter.WriteGenerator("Basisregisters Vlaanderen", "https://basisregisters.vlaanderen.be", version);
            await atomFeedWriter.WriteRights($"Copyright (c) 2017-{DateTime.Now.Year}, Informatie Vlaanderen");
            await atomFeedWriter.WriteUpdated(DateTimeOffset.UtcNow);
            await atomFeedWriter.Write(new SyndicationPerson("agentschap Informatie Vlaanderen", "informatie.vlaanderen@vlaanderen.be", AtomContributorTypes.Author));
            await atomFeedWriter.Write(new SyndicationLink(selfUri, AtomLinkTypes.Self));

            foreach (var relatedUrl in relatedUrls)
                await atomFeedWriter.Write(new SyndicationLink(new Uri(relatedUrl), AtomLinkTypes.Related));
        }
    }
}
