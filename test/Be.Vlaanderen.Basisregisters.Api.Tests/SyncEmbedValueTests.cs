namespace Be.Vlaanderen.Basisregisters.Api.Tests
{
    using System;
    using FluentAssertions;
    using Search.Filtering;
    using Xunit;


    public class When_parsing_an_empty_embed_string
    {
        private SyncEmbedValue _sut;

        public When_parsing_an_empty_embed_string()
        {
            _sut = SyncEmbedValue.Parse("");
        }

        [Fact]
        public void Then_event_is_not_set()
        {
            _sut.Event
                .Should().BeFalse();
        }

        [Fact]
        public void Then_object_is_not_set()
        {
            _sut.Object
                .Should().BeFalse();
        }
    }

    public class When_parsing_an_embed_string_containing_event
    {
        [Theory]
        [InlineData("event")]
        [InlineData("EVENT")]
        [InlineData("evEnT")]
        [InlineData("event, object")]
        [InlineData("object,event")]
        [InlineData("event,event,event,event,event")]
        public void Then_event_is_set(string value)
        {
            SyncEmbedValue.Parse(value)
                .Event
                .Should().BeTrue();
        }
    }

    public class When_parsing_an_embed_string_containing_object
    {
        [Theory]
        [InlineData("object")]
        [InlineData("OBJECT")]
        [InlineData("ObJEct")]
        [InlineData("event, object")]
        [InlineData("object,event")]
        [InlineData("object,object,object,object,object,object")]
        public void Then_object_is_set(string value)
        {
            SyncEmbedValue.Parse(value)
                .Object
                .Should().BeTrue();
        }
    }

    public class When_parsing_an_invalid_embed_string
    {
        [Theory]
        [InlineData("object,")]
        [InlineData(",event")]
        [InlineData("obj")]
        [InlineData("123")]
        [InlineData("object,123")]
        [InlineData("123,event")]
        public void Then_a_validation_exception_is_thrown(string value)
        {
            Action parse = () => SyncEmbedValue.Parse(value);

            parse
                .Should()
                .Throw<SyncEmbedValue.InvalidOptionException>();
        }
    }
}
