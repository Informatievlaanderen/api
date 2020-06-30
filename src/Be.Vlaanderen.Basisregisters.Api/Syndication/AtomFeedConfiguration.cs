namespace Be.Vlaanderen.Basisregisters.Api.Syndication
{
    using System;
    using System.Collections.Generic;
    using Microsoft.SyndicationFeed;

    /// <summary>
    /// The configuration class of an Atom feed.
    /// </summary>
    public class AtomFeedConfiguration
    {
        /// <summary>
        /// The Id of the feed
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// The title of the feed
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// The subtitle of the feed
        /// </summary>
        public string Subtitle { get; }

        /// <summary>
        /// The generator title of the feed
        /// </summary>
        public string GeneratorTitle { get; }

        /// <summary>
        /// The generator uri of the feed
        /// </summary>
        public string GeneratorUri { get; }

        /// <summary>
        /// The version of the feed
        /// </summary>
        public string GeneratorVersion { get; }

        /// <summary>
        /// The rights of the feed (copyright)
        /// </summary>
        public string Rights { get; }

        /// <summary>
        /// The timestamp the feed was last updated on
        /// </summary>
        public DateTimeOffset Updated { get; }

        /// <summary>
        /// The author of the feed
        /// </summary>
        public SyndicationPerson Author { get; }

        /// <summary>
        /// The Self Uri of the feed
        /// </summary>
        public SyndicationLink SelfUri { get; }

        /// <summary>
        /// The alternate uris of the feed
        /// </summary>
        public List<SyndicationLink> AlternateUris { get; }

        /// <summary>
        /// The related uris of the feed
        /// </summary>
        public List<SyndicationLink> RelatedUris { get; }

        /// <summary>
        /// The constructor of an atom feed configuration
        /// </summary>
        /// <param name="id">the id of the feed</param>
        /// <param name="title">the title of the feed</param>
        /// <param name="subtitle">the subtitle of the feed</param>
        /// <param name="generatorTitle">the generator title of the feed</param>
        /// <param name="generatorUri">the generator uri of the feed</param>
        /// <param name="generatorVersion">the generator version of the feed</param>
        /// <param name="rights">the rights of the feed (copyright)</param>
        /// <param name="updated">the timestamp the feed was last updated on</param>
        /// <param name="author">the author of the feed</param>
        /// <param name="selfUri">the self uri of the feed</param>
        /// <param name="alternateUris">the alternate uris of the feed</param>
        /// <param name="relatedUris">the related uris of the feed</param>
        public AtomFeedConfiguration(
            string id,
            string title,
            string subtitle,
            string generatorTitle,
            string generatorUri,
            string generatorVersion,
            string rights,
            DateTimeOffset updated,
            SyndicationPerson author,
            SyndicationLink selfUri,
            List<SyndicationLink> alternateUris,
            List<SyndicationLink> relatedUris)
        {
            Id = id;
            Title = title;
            Subtitle = subtitle;
            GeneratorTitle = generatorTitle;
            GeneratorUri = generatorUri;
            GeneratorVersion = generatorVersion;
            Rights = rights;
            Updated = updated;
            Author = author;
            SelfUri = selfUri;
            AlternateUris = alternateUris;
            RelatedUris = relatedUris;
        }
    }
}
