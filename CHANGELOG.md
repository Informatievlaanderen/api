## [6.0.4](https://github.com/informatievlaanderen/api/compare/v6.0.3...v6.0.4) (2019-04-26)

## [6.0.3](https://github.com/informatievlaanderen/api/compare/v6.0.2...v6.0.3) (2019-04-24)

## [6.0.2](https://github.com/informatievlaanderen/api/compare/v6.0.1...v6.0.2) (2019-04-24)


### Bug Fixes

* change the PagedQueryable.Items type ([4705acf](https://github.com/informatievlaanderen/api/commit/4705acf))
* items of PagedQueryable alwas are AsyncEnumerable ([6e8c531](https://github.com/informatievlaanderen/api/commit/6e8c531))
* revert using IAsynQueryable<T> ([beef789](https://github.com/informatievlaanderen/api/commit/beef789))
* set number of pages to 1 for limit 0 ([7a2c1a2](https://github.com/informatievlaanderen/api/commit/7a2c1a2))

## [6.0.1](https://github.com/informatievlaanderen/api/compare/v6.0.0...v6.0.1) (2019-04-23)


### Bug Fixes

* don't query items when page size is zero ([2e6cb1d](https://github.com/informatievlaanderen/api/commit/2e6cb1d))

# [6.0.0](https://github.com/informatievlaanderen/api/compare/v5.5.0...v6.0.0) (2019-04-19)


### Features

* switch to sockets as kestrel transport instead of libuv ([0e03a7a](https://github.com/informatievlaanderen/api/commit/0e03a7a))


### BREAKING CHANGES

* Kestrel is running on sockets instead of libuv, be aware of this in case you have a
specific use case to need libuv

# [5.5.0](https://github.com/informatievlaanderen/api/compare/v5.4.0...v5.5.0) (2019-04-19)


### Features

* support http/2 ([b6133cc](https://github.com/informatievlaanderen/api/commit/b6133cc))

# [5.4.0](https://github.com/informatievlaanderen/api/compare/v5.3.1...v5.4.0) (2019-04-19)


### Features

* add brotli compression ([051d686](https://github.com/informatievlaanderen/api/commit/051d686))

## [5.3.1](https://github.com/informatievlaanderen/api/compare/v5.3.0...v5.3.1) (2019-04-19)

# [5.3.0](https://github.com/informatievlaanderen/api/compare/v5.2.0...v5.3.0) (2019-04-19)


### Features

* add default datadog setup ([e1e46d6](https://github.com/informatievlaanderen/api/commit/e1e46d6))

# [5.2.0](https://github.com/informatievlaanderen/api/compare/v5.1.1...v5.2.0) (2019-04-18)


### Features

* add traceid to logcontext ([026ce2e](https://github.com/informatievlaanderen/api/commit/026ce2e))

## [5.1.1](https://github.com/informatievlaanderen/api/compare/v5.1.0...v5.1.1) (2019-04-18)


### Bug Fixes

* properly register tracesource factory ([2e6eb04](https://github.com/informatievlaanderen/api/commit/2e6eb04))

# [5.1.0](https://github.com/informatievlaanderen/api/compare/v5.0.0...v5.1.0) (2019-04-18)


### Features

* add extra debug logging for traceagent ([27defb0](https://github.com/informatievlaanderen/api/commit/27defb0))

# [5.0.0](https://github.com/informatievlaanderen/api/compare/v4.0.1...v5.0.0) (2019-04-18)


### Bug Fixes

* trace id has to be a long ([2844da9](https://github.com/informatievlaanderen/api/commit/2844da9))


### BREAKING CHANGES

* Trace Id has to be a long instead of a string

## [4.0.1](https://github.com/informatievlaanderen/api/compare/v4.0.0...v4.0.1) (2019-04-18)


### Bug Fixes

* properly register datadog with autofac ([cdff518](https://github.com/informatievlaanderen/api/commit/cdff518))

# [4.0.0](https://github.com/informatievlaanderen/api/compare/v3.1.0...v4.0.0) (2019-04-17)


### Features

* provide your own trace id ([8b6ea1e](https://github.com/informatievlaanderen/api/commit/8b6ea1e))


### BREAKING CHANGES

* UseDataDogTracing now expects a function to return a TraceSource per request

# [3.1.0](https://github.com/informatievlaanderen/api/compare/v3.0.0...v3.1.0) (2019-04-17)


### Features

* remove datadog profiler ([8b7955c](https://github.com/informatievlaanderen/api/commit/8b7955c))

# [3.0.0](https://github.com/informatievlaanderen/api/compare/v2.3.0...v3.0.0) (2019-04-17)


### Features

* programdefaults uses an options object for configuration ([00bd633](https://github.com/informatievlaanderen/api/commit/00bd633)), closes [#5](https://github.com/informatievlaanderen/api/issues/5)


### BREAKING CHANGES

* Change ProgramDefault to use the new options object.

# [2.3.0](https://github.com/informatievlaanderen/api/compare/v2.2.1...v2.3.0) (2019-04-01)


### Features

* add a check for databases health ([c6b6b9d](https://github.com/informatievlaanderen/api/commit/c6b6b9d))

## [2.2.1](https://github.com/informatievlaanderen/api/compare/v2.2.0...v2.2.1) (2019-04-01)


### Bug Fixes

* add more details to healtcheck ([b53a546](https://github.com/informatievlaanderen/api/commit/b53a546))

# [2.2.0](https://github.com/informatievlaanderen/api/compare/v2.1.0...v2.2.0) (2019-04-01)


### Features

* add healtchecks ([71a6090](https://github.com/informatievlaanderen/api/commit/71a6090))

# [2.1.0](https://github.com/informatievlaanderen/api/compare/v2.0.2...v2.1.0) (2019-03-30)


### Features

* add hooks for authorization and localization options ([d96bdc6](https://github.com/informatievlaanderen/api/commit/d96bdc6))

## [2.0.2](https://github.com/informatievlaanderen/api/compare/v2.0.1...v2.0.2) (2019-03-30)


### Bug Fixes

* properly resolve RequestLocalizationOptions ([2f418e4](https://github.com/informatievlaanderen/api/commit/2f418e4))

## [2.0.1](https://github.com/informatievlaanderen/api/compare/v2.0.0...v2.0.1) (2019-03-30)


### Bug Fixes

* still provide an overload without sharedresources fallback ([be08d4c](https://github.com/informatievlaanderen/api/commit/be08d4c))

# [2.0.0](https://github.com/informatievlaanderen/api/compare/v1.11.0...v2.0.0) (2019-03-30)


### Features

* add localization, redesign ConfigureDefaultForApi ([fc511b2](https://github.com/informatievlaanderen/api/commit/fc511b2))


### BREAKING CHANGES

* ConfigureDefaultOptions has a new signature, using an options object and having an
extra generic parameter for localization.

# [1.11.0](https://github.com/informatievlaanderen/api/compare/v1.10.1...v1.11.0) (2019-03-21)


### Features

* add validation api problem ([6de612c](https://github.com/informatievlaanderen/api/commit/6de612c))

## [1.10.1](https://github.com/informatievlaanderen/api/compare/v1.10.0...v1.10.1) (2019-03-18)


### Bug Fixes

* allow empty cors settings ([4342027](https://github.com/informatievlaanderen/api/commit/4342027))

# [1.10.0](https://github.com/informatievlaanderen/api/compare/v1.9.0...v1.10.0) (2019-03-18)


### Features

* allow configuration of mvccore options and cors headers ([a296537](https://github.com/informatievlaanderen/api/commit/a296537))

# [1.9.0](https://github.com/informatievlaanderen/api/compare/v1.8.4...v1.9.0) (2019-03-15)


### Features

* add fluent validation library ([b52b5d1](https://github.com/informatievlaanderen/api/commit/b52b5d1))

## [1.8.4](https://github.com/informatievlaanderen/api/compare/v1.8.3...v1.8.4) (2019-02-26)

## [1.8.3](https://github.com/informatievlaanderen/api/compare/v1.8.2...v1.8.3) (2019-02-25)

## [1.8.2](https://github.com/informatievlaanderen/api/compare/v1.8.1...v1.8.2) (2019-02-25)


### Bug Fixes

* remove unneeded parameter on CreateFileCallbackResult ([1a41ce2](https://github.com/informatievlaanderen/api/commit/1a41ce2))

## [1.8.1](https://github.com/informatievlaanderen/api/compare/v1.8.0...v1.8.1) (2019-02-25)

# [1.8.0](https://github.com/informatievlaanderen/api/compare/v1.7.0...v1.8.0) (2019-02-25)


### Features

* add extract infrastructure ([e4ff812](https://github.com/informatievlaanderen/api/commit/e4ff812))

# [1.7.0](https://github.com/informatievlaanderen/api/compare/v1.6.0...v1.7.0) (2019-02-24)


### Features

* provide default exception handler for easier usage ([14cb620](https://github.com/informatievlaanderen/api/commit/14cb620))

# [1.6.0](https://github.com/informatievlaanderen/api/compare/v1.5.1...v1.6.0) (2019-02-24)


### Features

* allow passing in custom exception handlers ([3d4aa78](https://github.com/informatievlaanderen/api/commit/3d4aa78))

## [1.5.1](https://github.com/informatievlaanderen/api/compare/v1.5.0...v1.5.1) (2019-02-18)


### Bug Fixes

* update Be.Vlaanderen.Basisregisters.AggregateSource dependency ([#3](https://github.com/informatievlaanderen/api/issues/3)) ([fe09359](https://github.com/informatievlaanderen/api/commit/fe09359))

# [1.5.0](https://github.com/informatievlaanderen/api/compare/v1.4.3...v1.5.0) (2019-02-07)


### Features

* support nullable Rfc3339SerializableDateTimeOffset in converter ([c9d056a](https://github.com/informatievlaanderen/api/commit/c9d056a))

## [1.4.3](https://github.com/informatievlaanderen/api/compare/v1.4.2...v1.4.3) (2019-02-06)


### Bug Fixes

* add nuget references to dependencies ([224cfd4](https://github.com/informatievlaanderen/api/commit/224cfd4))

## [1.4.2](https://github.com/informatievlaanderen/api/compare/v1.4.1...v1.4.2) (2019-01-24)


### Bug Fixes

* clarify detail for internal server error contains details about the error ([a91b1ef](https://github.com/informatievlaanderen/api/commit/a91b1ef))

## [1.4.1](https://github.com/informatievlaanderen/api/compare/v1.4.0...v1.4.1) (2019-01-24)


### Bug Fixes

* not modified 304 must not contain a body ([df0368e](https://github.com/informatievlaanderen/api/commit/df0368e))

# [1.4.0](https://github.com/informatievlaanderen/api/compare/v1.3.1...v1.4.0) (2019-01-22)


### Features

* add 406 Not Acceptable response examples ([77eadb0](https://github.com/informatievlaanderen/api/commit/77eadb0))

## [1.3.1](https://github.com/informatievlaanderen/api/compare/v1.3.0...v1.3.1) (2019-01-10)

# [1.3.0](https://github.com/informatievlaanderen/api/compare/v1.2.0...v1.3.0) (2019-01-10)


### Features

* allow customisation of header and poweredby name ([139cdb2](https://github.com/informatievlaanderen/api/commit/139cdb2))

# [1.2.0](https://github.com/informatievlaanderen/api/compare/v1.1.1...v1.2.0) (2019-01-10)


### Features

* add middleware hooks ([6f1d3f3](https://github.com/informatievlaanderen/api/commit/6f1d3f3))

## [1.1.1](https://github.com/informatievlaanderen/api/compare/v1.1.0...v1.1.1) (2019-01-08)


### Bug Fixes

* define keep-alive timeout to be higher than traefik timeout ([29ea1de](https://github.com/informatievlaanderen/api/commit/29ea1de))

# [1.1.0](https://github.com/informatievlaanderen/api/compare/v1.0.2...v1.1.0) (2018-12-30)


### Features

* pull in ApiController and make it participate in ApiExplorer ([1434c66](https://github.com/informatievlaanderen/api/commit/1434c66))

## [1.0.2](https://github.com/informatievlaanderen/api/compare/v1.0.1...v1.0.2) (2018-12-30)

## [1.0.1](https://github.com/informatievlaanderen/api/compare/v1.0.0...v1.0.1) (2018-12-30)


### Bug Fixes

* pull in upstream fix for an encoding issue in api documentation ([b19b852](https://github.com/informatievlaanderen/api/commit/b19b852))

# 1.0.0 (2018-12-20)


### Features

* open source with MIT license as 'agentschap Informatie Vlaanderen' ([18e71d8](https://github.com/informatievlaanderen/api/commit/18e71d8))
