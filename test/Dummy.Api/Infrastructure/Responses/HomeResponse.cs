namespace Dummy.Api.Infrastructure.Responses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Net;
    using System.Runtime.Serialization;
    using Swashbuckle.AspNetCore.Filters;

    [DataContract(Name = "Home", Namespace = "")]
    public class HomeResponse
    {
        /// <summary>
        /// Hypermedia links
        /// </summary>
        [DataMember(Name = "Links", Order = 1)]
        public List<Link> Links { get; set; } = new List<Link>();

        [DataMember(Name = "Pi")]
        [Description("Commas")]
        public double[] Pies { get; set; } = { Math.PI, Math.PI };

        public HomeResponse()
        {
            Links.AddRange(new[]
            {
                new Link("/example-aggregates", Link.Relations.ExampleAggregates, WebRequestMethods.Http.Get),
                new Link("/example-aggregates", Link.Relations.ExampleAggregates, WebRequestMethods.Http.Post)
            });
        }
    }

    public class HomeResponseExamples : IExamplesProvider<HomeResponse>
    {
        public HomeResponse GetExamples() => new HomeResponse();
    }
}
