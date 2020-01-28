# [10.3.0](https://github.com/informatievlaanderen/api/compare/v10.2.0...v10.3.0) (2020-01-28)


### Features

* update swagger ([766d5c5](https://github.com/informatievlaanderen/api/commit/766d5c5))

# [10.2.0](https://github.com/informatievlaanderen/api/compare/v10.1.0...v10.2.0) (2020-01-15)


### Bug Fixes

* dont use endpoint routing yet, break in a next release ([00061b1](https://github.com/informatievlaanderen/api/commit/00061b1))
* swagger wants a parameterless ctor ([1c61c47](https://github.com/informatievlaanderen/api/commit/1c61c47))


### Features

* upgrade swagger and also fix api to get registries working ([f6499ca](https://github.com/informatievlaanderen/api/commit/f6499ca))

# [10.1.0](https://github.com/informatievlaanderen/api/compare/v10.0.1...v10.1.0) (2020-01-13)


### Bug Fixes

* prefer 3 digits for nuget dependencies ([fe6aef9](https://github.com/informatievlaanderen/api/commit/fe6aef9))
* pushed logic to aws-distributed-mutex ([9170b4b](https://github.com/informatievlaanderen/api/commit/9170b4b))


### Features

* allow lock to be disabled with configuration ([9d8b893](https://github.com/informatievlaanderen/api/commit/9d8b893))

## [10.0.1](https://github.com/informatievlaanderen/api/compare/v10.0.0...v10.0.1) (2020-01-02)


### Bug Fixes

* turn distributedlock configuration into a func ([7dc7380](https://github.com/informatievlaanderen/api/commit/7dc7380))

# [10.0.0](https://github.com/informatievlaanderen/api/compare/v9.1.1...v10.0.0) (2020-01-02)


### Code Refactoring

* change runwithlock chaining ([082a0c4](https://github.com/informatievlaanderen/api/commit/082a0c4))


### BREAKING CHANGES

* RunWithLock now chains and expects UseDefaultForApi before

## [9.1.1](https://github.com/informatievlaanderen/api/compare/v9.1.0...v9.1.1) (2020-01-02)


### Bug Fixes

* use updated distributed lock ([b111bb0](https://github.com/informatievlaanderen/api/commit/b111bb0))

# [9.1.0](https://github.com/informatievlaanderen/api/compare/v9.0.1...v9.1.0) (2020-01-02)


### Features

* add RunWithLock to support single instance apis ([4431a34](https://github.com/informatievlaanderen/api/commit/4431a34))

## [9.0.1](https://github.com/informatievlaanderen/api/compare/v9.0.0...v9.0.1) (2019-12-20)


### Bug Fixes

* add Microsoft.AspNetCore.Mvc.NewtonsoftJson to template ([687bd2f](https://github.com/informatievlaanderen/api/commit/687bd2f))

# [9.0.0](https://github.com/informatievlaanderen/api/compare/v8.0.0...v9.0.0) (2019-12-17)


### Build System

* move to FAKE 5 ([0317515](https://github.com/informatievlaanderen/api/commit/0317515))


### Features

* upgrade to netcoreapp31 ([42eef11](https://github.com/informatievlaanderen/api/commit/42eef11))


### BREAKING CHANGES

* Upgrade to .NET Core 3.1
* Upgrade to .NET Core 3.1

# [8.0.0](https://github.com/informatievlaanderen/api/compare/v7.10.0...v8.0.0) (2019-11-25)


### Code Refactoring

* change totalitems from int to long ([#11](https://github.com/informatievlaanderen/api/issues/11)) ([ebb2fbb](https://github.com/informatievlaanderen/api/commit/ebb2fbb))


### BREAKING CHANGES

* Totalitems is a long instead of an int.

# [7.10.0](https://github.com/informatievlaanderen/api/compare/v7.9.0...v7.10.0) (2019-11-21)


### Features

* add count func parameter to pagination ([b6b69f8](https://github.com/informatievlaanderen/api/commit/b6b69f8))

# [7.9.0](https://github.com/informatievlaanderen/api/compare/v7.8.1...v7.9.0) (2019-11-08)


### Features

* add additional hooks ([03556fd](https://github.com/informatievlaanderen/api/commit/03556fd))

## [7.8.1](https://github.com/informatievlaanderen/api/compare/v7.8.0...v7.8.1) (2019-10-24)


### Bug Fixes

* integrate swagger fix to work in edge and ie ([5cdbce9](https://github.com/informatievlaanderen/api/commit/5cdbce9))

# [7.8.0](https://github.com/informatievlaanderen/api/compare/v7.7.0...v7.8.0) (2019-10-22)


### Features

* allow custom sorting to be specified in swagger ([512df90](https://github.com/informatievlaanderen/api/commit/512df90))

# [7.7.0](https://github.com/informatievlaanderen/api/compare/v7.6.2...v7.7.0) (2019-10-21)


### Features

* provide swagger middleware hooks ([3277aba](https://github.com/informatievlaanderen/api/commit/3277aba))

## [7.6.2](https://github.com/informatievlaanderen/api/compare/v7.6.1...v7.6.2) (2019-10-21)


### Bug Fixes

* add details for problemdetails ([d4a4bda](https://github.com/informatievlaanderen/api/commit/d4a4bda))

## [7.6.1](https://github.com/informatievlaanderen/api/compare/v7.6.0...v7.6.1) (2019-10-01)


### Bug Fixes

* update swagger ([24812cc](https://github.com/informatievlaanderen/api/commit/24812cc))

# [7.6.0](https://github.com/informatievlaanderen/api/compare/v7.5.0...v7.6.0) (2019-10-01)


### Features

* add servers option for swagger ([0182d10](https://github.com/informatievlaanderen/api/commit/0182d10))

# [7.5.0](https://github.com/informatievlaanderen/api/compare/v7.4.2...v7.5.0) (2019-10-01)


### Features

* update swagger dependency ([26a23fc](https://github.com/informatievlaanderen/api/commit/26a23fc))

## [7.4.2](https://github.com/informatievlaanderen/api/compare/v7.4.1...v7.4.2) (2019-09-17)


### Bug Fixes

* include pipeline in error responses too ([a4bfb69](https://github.com/informatievlaanderen/api/commit/a4bfb69))

## [7.4.1](https://github.com/informatievlaanderen/api/compare/v7.4.0...v7.4.1) (2019-09-03)


### Bug Fixes

* update problemdetails for xml response GR-829 ([36eaafb](https://github.com/informatievlaanderen/api/commit/36eaafb))

# [7.4.0](https://github.com/informatievlaanderen/api/compare/v7.3.4...v7.4.0) (2019-08-30)


### Features

* add assembly getversiontext helper ([9d707c4](https://github.com/informatievlaanderen/api/commit/9d707c4))

## [7.3.4](https://github.com/informatievlaanderen/api/compare/v7.3.3...v7.3.4) (2019-08-27)


### Bug Fixes

* make datadog tracing check more for nulls ([b109592](https://github.com/informatievlaanderen/api/commit/b109592))

## [7.3.3](https://github.com/informatievlaanderen/api/compare/v7.3.2...v7.3.3) (2019-08-26)


### Bug Fixes

* use fixed datadog tracing ([02e31ac](https://github.com/informatievlaanderen/api/commit/02e31ac))

## [7.3.2](https://github.com/informatievlaanderen/api/compare/v7.3.1...v7.3.2) (2019-08-26)


### Bug Fixes

* make localization available earlier ([82dc05a](https://github.com/informatievlaanderen/api/commit/82dc05a))

## [7.3.1](https://github.com/informatievlaanderen/api/compare/v7.3.0...v7.3.1) (2019-08-26)


### Bug Fixes

* pass in new swagger parameters ([5689cc7](https://github.com/informatievlaanderen/api/commit/5689cc7))

# [7.3.0](https://github.com/informatievlaanderen/api/compare/v7.2.2...v7.3.0) (2019-08-22)


### Features

* bump to .net 2.2.6 ([1e0a760](https://github.com/informatievlaanderen/api/commit/1e0a760))
* bump to .net 2.2.6 ([3e84778](https://github.com/informatievlaanderen/api/commit/3e84778))

## [7.2.2](https://github.com/informatievlaanderen/api/compare/v7.2.1...v7.2.2) (2019-08-20)


### Bug Fixes

* add spanid ([da97801](https://github.com/informatievlaanderen/api/commit/da97801))

## [7.2.1](https://github.com/informatievlaanderen/api/compare/v7.2.0...v7.2.1) (2019-08-20)


### Bug Fixes

* add parentspanid ([f7e1170](https://github.com/informatievlaanderen/api/commit/f7e1170))

# [7.2.0](https://github.com/informatievlaanderen/api/compare/v7.1.0...v7.2.0) (2019-08-20)


### Features

* add support for parent span id ([4ee1d91](https://github.com/informatievlaanderen/api/commit/4ee1d91))

# [7.1.0](https://github.com/informatievlaanderen/api/compare/v7.0.3...v7.1.0) (2019-08-12)


### Features

* span exposes traceid ([5472ced](https://github.com/informatievlaanderen/api/commit/5472ced))

## [7.0.3](https://github.com/informatievlaanderen/api/compare/v7.0.2...v7.0.3) (2019-07-17)


### Bug Fixes

* do not hardcode logging to console ([cfb280a](https://github.com/informatievlaanderen/api/commit/cfb280a))

## [7.0.2](https://github.com/informatievlaanderen/api/compare/v7.0.1...v7.0.2) (2019-07-05)


### Bug Fixes

* correct total item count with limit = 0 ([a79a71f](https://github.com/informatievlaanderen/api/commit/a79a71f))

## [7.0.1](https://github.com/informatievlaanderen/api/compare/v7.0.0...v7.0.1) (2019-04-30)


### Bug Fixes

* correct nuget dependencies ([b05cab3](https://github.com/informatievlaanderen/api/commit/b05cab3))

# [7.0.0](https://github.com/informatievlaanderen/api/compare/v6.1.0...v7.0.0) (2019-04-29)


### Features

* all errors now return problemdetails ([f28c57c](https://github.com/informatievlaanderen/api/commit/f28c57c))


### BREAKING CHANGES

* BasicApiProblem moved to ProblemDetails. IExceptionHandler.GetApiProblemFor now
takes in a HttpContext.

# [6.1.0](https://github.com/informatievlaanderen/api/compare/v6.0.5...v6.1.0) (2019-04-26)


### Features

* add middleware configuration options ([bdfcff0](https://github.com/informatievlaanderen/api/commit/bdfcff0))

## [6.0.5](https://github.com/informatievlaanderen/api/compare/v6.0.4...v6.0.5) (2019-04-26)


### Bug Fixes

* post changelog to confluence ([d7a38b6](https://github.com/informatievlaanderen/api/commit/d7a38b6))

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
