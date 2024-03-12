# [21.0.0](https://github.com/informatievlaanderen/api/compare/v20.1.0...v21.0.0) (2024-03-12)


### Features

* upgrade to net8 ([263f2e5](https://github.com/informatievlaanderen/api/commit/263f2e530d0a651f45cdcf97e934991c58d780d2))


### BREAKING CHANGES

* upgrade to net8

# [20.1.0](https://github.com/informatievlaanderen/api/compare/v20.0.1...v20.1.0) (2023-09-13)


### Features

* add application ld accepttype ([e4ae3a5](https://github.com/informatievlaanderen/api/commit/e4ae3a5a40205dc36c25d5fe8014c6f67cb94c23))

## [20.0.1](https://github.com/informatievlaanderen/api/compare/v20.0.0...v20.0.1) (2023-04-25)


### Bug Fixes

* ForbiddenResponseExamples detail ([adff57e](https://github.com/informatievlaanderen/api/commit/adff57e19ca93b17e3febae98bfc2214ffc2270d))

# [20.0.0](https://github.com/informatievlaanderen/api/compare/v19.1.0...v20.0.0) (2023-03-11)


### Performance Improvements

* refactor pagination to gain performance GAWR-4574 ([06e593e](https://github.com/informatievlaanderen/api/commit/06e593e24226096de98589a84122021854ffdc00))


### BREAKING CHANGES

* Removed `HasNextPage` property from PaginationInfo and replaced it by a method.
Side effect: when the collection EXACTLY hits the end with limit,
then HasNextPage will return true while before it was false.

# [19.1.0](https://github.com/informatievlaanderen/api/compare/v19.0.1...v19.1.0) (2023-03-01)


### Features

* v2 response examples ([80fc5c1](https://github.com/informatievlaanderen/api/commit/80fc5c18ea437b98d82ac36e9bd3cb9ccaa8e669))

## [19.0.1](https://github.com/informatievlaanderen/api/compare/v19.0.0...v19.0.1) (2023-01-12)


### Bug Fixes

* replace datadog.tracing.autofac by microsoft ([a0deac7](https://github.com/informatievlaanderen/api/commit/a0deac7a05c6b78fbc780ce7719b252ab360e685))

# [19.0.0](https://github.com/informatievlaanderen/api/compare/v18.3.2...v19.0.0) (2023-01-11)


### Features

* problem details base uri versioning ([751e715](https://github.com/informatievlaanderen/api/commit/751e715b25fa6a8f366904ef568bf0547eeb7f48))


### BREAKING CHANGES

* IExceptionHandler GetApiProblemFor has an additional HttpContext parameter.

## [18.3.2](https://github.com/informatievlaanderen/api/compare/v18.3.1...v18.3.2) (2023-01-01)


### Bug Fixes

* bump DependencyInjection dependency ([7c63da4](https://github.com/informatievlaanderen/api/commit/7c63da4fb614214f23f5dab4f38839b0f0a3592a))
* use Microsoft for datadog toggles ([c0eb64a](https://github.com/informatievlaanderen/api/commit/c0eb64adcd2a7423d827bfb51db8f45c4cad8c8b))

## [18.3.1](https://github.com/informatievlaanderen/api/compare/v18.3.0...v18.3.1) (2022-12-28)


### Bug Fixes

* add nuget to dependabot ([3254665](https://github.com/informatievlaanderen/api/commit/3254665aff569b4091d1a1d7948e1c04a9578d23))
* add parameter default from base ([e6e4b71](https://github.com/informatievlaanderen/api/commit/e6e4b71d0282d982e7dfe752bc53b9b366cf6ac2))
* bump dependencies ([0a82335](https://github.com/informatievlaanderen/api/commit/0a823357b10a5b167f476c94077ef6a61fe49e87))
* don't throw general exceptions ([af85de5](https://github.com/informatievlaanderen/api/commit/af85de5097d6239b5dc07e28983acc7bc4b7396d))
* empty methods ([9534cd8](https://github.com/informatievlaanderen/api/commit/9534cd82d4e5d713b5854d172148f6ddae6b1909))
* field to property ([6dee558](https://github.com/informatievlaanderen/api/commit/6dee558140605d4c541968905d8552ddc72fd1fe))
* implement Dispose pattern correctly ([e6f118d](https://github.com/informatievlaanderen/api/commit/e6f118df09ac27dc3cbdec8e25c089fc243f7b16))
* implement Serializable correctly ([f175345](https://github.com/informatievlaanderen/api/commit/f175345089c0a8bc94382be567be79cbb5bbe754))
* make Attribute class name end with Attribute ([088f741](https://github.com/informatievlaanderen/api/commit/088f741025e879009680c37855743fafe4431967))
* make UnhandledException public ([e59728e](https://github.com/informatievlaanderen/api/commit/e59728e9bbdf3adad1724f5af5ed7640c01e8bec))
* make utility classes static ([16f0325](https://github.com/informatievlaanderen/api/commit/16f0325f8de83876b537995a9d019621e8c995be))
* merge if's ([1c9b623](https://github.com/informatievlaanderen/api/commit/1c9b62394e5c4f9fbfe0485aac1d64886b1042f3))
* remove redundant bool literals ([71d9cb8](https://github.com/informatievlaanderen/api/commit/71d9cb836b6668ef53fff8a4256586bbcbab3224))
* rename EmbedOptions to EmbedOptions ([e1a01fd](https://github.com/informatievlaanderen/api/commit/e1a01fd3d49424ca25aac9de574791e8b029a54e))
* rename parameter ([353c22f](https://github.com/informatievlaanderen/api/commit/353c22f5194ad027eb14611202ac3baf7ef47971))
* seal private classes ([0201ce0](https://github.com/informatievlaanderen/api/commit/0201ce0f63a5d3429331f7c365f8e45c7e2bf3f1))
* use VBR_SONAR_TOKEN ([22e8c8c](https://github.com/informatievlaanderen/api/commit/22e8c8cae0e56141c89feb6cbda04fe0c25fa987))

# [18.3.0](https://github.com/informatievlaanderen/api/compare/v18.2.1...v18.3.0) (2022-11-02)


### Features

* add actionmodelconventions ([36bbdfb](https://github.com/informatievlaanderen/api/commit/36bbdfbe258628a6a5a7a45bdf258b2d2274d1bb))

## [18.2.1](https://github.com/informatievlaanderen/api/compare/v18.2.0...v18.2.1) (2022-09-19)


### Bug Fixes

* make exceptions serializable ([e37060b](https://github.com/informatievlaanderen/api/commit/e37060b9d21eee1a9b85abfd421e3c7991f1f8c9))

# [18.2.0](https://github.com/informatievlaanderen/api/compare/v18.1.1...v18.2.0) (2022-09-16)


### Features

* add actionmodelconventions ([3a29a1a](https://github.com/informatievlaanderen/api/commit/3a29a1acda8cea5610479f89aa1e0c7888163f4b))

## [18.1.1](https://github.com/informatievlaanderen/api/compare/v18.1.0...v18.1.1) (2022-07-27)


### Bug Fixes

* bump dependencies ([087c362](https://github.com/informatievlaanderen/api/commit/087c362b2d0cb9ca243c68c5d48bd20b5b34ecac))

# [18.1.0](https://github.com/informatievlaanderen/api/compare/v18.0.5...v18.1.0) (2022-06-17)


### Features

* add startup helper snapshot store ([06f252b](https://github.com/informatievlaanderen/api/commit/06f252b83d0f200cd4669f54b56f6f58b0ede4b8))

## [18.0.5](https://github.com/informatievlaanderen/api/compare/v18.0.4...v18.0.5) (2022-04-29)


### Bug Fixes

* run sonar end when release version != none ([0f50c5c](https://github.com/informatievlaanderen/api/commit/0f50c5c118d707b803290300e38ef14b9cdb9aeb))

## [18.0.4](https://github.com/informatievlaanderen/api/compare/v18.0.3...v18.0.4) (2022-04-27)


### Bug Fixes

* redirect sonar to /dev/null ([90f37ad](https://github.com/informatievlaanderen/api/commit/90f37ad3b01da6eac79a7add339c5fd5d096a3da))

## [18.0.3](https://github.com/informatievlaanderen/api/compare/v18.0.2...v18.0.3) (2022-04-12)


### Bug Fixes

* bump problemdetails middleware ([3017431](https://github.com/informatievlaanderen/api/commit/301743171ce5fa1dfa54766f5429f093d71e9691))

## [18.0.2](https://github.com/informatievlaanderen/api/compare/v18.0.1...v18.0.2) (2022-03-30)


### Bug Fixes

* bump swagger to 4.0.2 ([93e1caa](https://github.com/informatievlaanderen/api/commit/93e1caa15f308d57c78a9f3f24627ae4678af461))

## [18.0.1](https://github.com/informatievlaanderen/api/compare/v18.0.0...v18.0.1) (2022-03-30)


### Bug Fixes

* bump swagger ([7ffd9f9](https://github.com/informatievlaanderen/api/commit/7ffd9f9dce55cb3e3b6431211a976a63422237ef))

# [18.0.0](https://github.com/informatievlaanderen/api/compare/v17.3.1...v18.0.0) (2022-03-25)


### Features

* move to dotnet 6.0.3 ([ab1570e](https://github.com/informatievlaanderen/api/commit/ab1570ef4e3d426020a1d7943a20e0c163ed9849))


### BREAKING CHANGES

* move to dotnet 6.0.3

## [17.3.1](https://github.com/informatievlaanderen/api/compare/v17.3.0...v17.3.1) (2022-03-18)


### Bug Fixes

* rename conflictResponseExample file ([39af16b](https://github.com/informatievlaanderen/api/commit/39af16b9db67e25b9928aa661eaf7152fe1d8815))

# [17.3.0](https://github.com/informatievlaanderen/api/compare/v17.2.1...v17.3.0) (2022-03-16)


### Features

* add ConflictResponseExamples ([d514c94](https://github.com/informatievlaanderen/api/commit/d514c944648f036bde30c15a0d5ee4fdc77e37ef))

## [17.2.1](https://github.com/informatievlaanderen/api/compare/v17.2.0...v17.2.1) (2022-03-11)


### Bug Fixes

* change PreconditionFailedResponseExamples ([42bc855](https://github.com/informatievlaanderen/api/commit/42bc855404816e694e1e41569d7ddd80f033a7cc))

# [17.2.0](https://github.com/informatievlaanderen/api/compare/v17.1.2...v17.2.0) (2022-03-11)


### Features

* add NoContentWithETagResult ([1ccbe9a](https://github.com/informatievlaanderen/api/commit/1ccbe9a04b2e32f4932d10a9fa32842ade60fdf3))

## [17.1.2](https://github.com/informatievlaanderen/api/compare/v17.1.1...v17.1.2) (2022-03-08)


### Bug Fixes

* build for toomanyrequests ([458a2c1](https://github.com/informatievlaanderen/api/commit/458a2c15c8e7125f65c032d2fbdd82fae328af4b))

## [17.1.1](https://github.com/informatievlaanderen/api/compare/v17.1.0...v17.1.1) (2022-03-08)


### Bug Fixes

* add option and pass it to datadog ([422e0e3](https://github.com/informatievlaanderen/api/commit/422e0e3c5c5d7b1f63fcb282d30a609618642cde))

# [17.1.0](https://github.com/informatievlaanderen/api/compare/v17.0.2...v17.1.0) (2022-03-08)


### Features

* upgrade datadog tracing ([1029a45](https://github.com/informatievlaanderen/api/commit/1029a45d974be9c6065661117c991cd15e46f978))

## [17.0.2](https://github.com/informatievlaanderen/api/compare/v17.0.1...v17.0.2) (2022-03-07)


### Bug Fixes

* etag should have double quotes ([e642c64](https://github.com/informatievlaanderen/api/commit/e642c64916fe13ac17f720363b17b861cb32b484))

## [17.0.1](https://github.com/informatievlaanderen/api/compare/v17.0.0...v17.0.1) (2022-02-28)


### Bug Fixes

* style to trigger build ([59a2f90](https://github.com/informatievlaanderen/api/commit/59a2f900f632861c055babb65cf9b0a41b7ebbdd))

# [17.0.0](https://github.com/informatievlaanderen/api/compare/v16.1.1...v17.0.0) (2022-02-25)


### Bug Fixes

* remove unneeded header name ([3b8a943](https://github.com/informatievlaanderen/api/commit/3b8a94340b31cfa03d0119434a3f3c3083b6e307))


### Features

* add OkWithLastObservedPositionAsETagResult ([56dea3c](https://github.com/informatievlaanderen/api/commit/56dea3ca96005265ed1a417b210565deba3ef2ce))


### BREAKING CHANGES

* removed ApiResults related to CRAB

## [16.1.1](https://github.com/informatievlaanderen/api/compare/v16.1.0...v16.1.1) (2022-02-25)


### Bug Fixes

* throw when healthcheck result not healthy ([c906a1a](https://github.com/informatievlaanderen/api/commit/c906a1a4fbd50666a790c2aa1faaf54d013ac8cc))

# [16.1.0](https://github.com/informatievlaanderen/api/compare/v16.0.2...v16.1.0) (2022-02-24)


### Features

* add polly retry to checkdatabases ([6746a45](https://github.com/informatievlaanderen/api/commit/6746a4565468343c9dda8502d1ebf0bf889aa422))

## [16.0.2](https://github.com/informatievlaanderen/api/compare/v16.0.1...v16.0.2) (2022-02-14)

## [16.0.1](https://github.com/informatievlaanderen/api/compare/v16.0.0...v16.0.1) (2022-02-11)


### Bug Fixes

* bump problemdetails ([563fcd0](https://github.com/informatievlaanderen/api/commit/563fcd0c680b0a8a9ba91b2ee08a6e1ab3eafdb4))

# [16.0.0](https://github.com/informatievlaanderen/api/compare/v15.5.6...v16.0.0) (2022-02-11)


### Bug Fixes

* bump problem details dependency ([151a9fe](https://github.com/informatievlaanderen/api/commit/151a9fea51f336f15009f346a02731e27fab8308))


### Features

* add json error action filter ([aaa07f5](https://github.com/informatievlaanderen/api/commit/aaa07f58cf6fe7283bb497c0c96a3ff494a7f236))


### BREAKING CHANGES

* bump

## [15.5.6](https://github.com/informatievlaanderen/api/compare/v15.5.5...v15.5.6) (2022-02-07)


### Bug Fixes

* too much logging ([7333606](https://github.com/informatievlaanderen/api/commit/7333606d41edfc7ba17bdc1e478a3c656f6d6f32))

## [15.5.5](https://github.com/informatievlaanderen/api/compare/v15.5.4...v15.5.5) (2022-02-04)


### Bug Fixes

* `NotAcceptableResponseExamples` uses correct provider ([322ad02](https://github.com/informatievlaanderen/api/commit/322ad02406568537969a9d5632f9a84659afcb44))
* commit 322ad ([9d373fd](https://github.com/informatievlaanderen/api/commit/9d373fd47f7614ebe894ce23f77f558f35c8e4df))

## [15.5.4](https://github.com/informatievlaanderen/api/compare/v15.5.3...v15.5.4) (2022-02-04)


### Bug Fixes

* gawr-614 remove body from 406 exception ([91af944](https://github.com/informatievlaanderen/api/commit/91af944d936555ebca04143fd7f50c5aed903936))

## [15.5.3](https://github.com/informatievlaanderen/api/compare/v15.5.2...v15.5.3) (2021-12-01)


### Bug Fixes

* bump problemjson (await next) ([2bd84e0](https://github.com/informatievlaanderen/api/commit/2bd84e086e4cccf3b8dfeab9cfb912cd204c06ee))

## [15.5.2](https://github.com/informatievlaanderen/api/compare/v15.5.1...v15.5.2) (2021-11-30)


### Bug Fixes

* bump problemjson again ([4668704](https://github.com/informatievlaanderen/api/commit/46687041010490e0a02192494e612b9bae68e31d))

## [15.5.1](https://github.com/informatievlaanderen/api/compare/v15.5.0...v15.5.1) (2021-11-30)


### Bug Fixes

* bump problemjson package ([46dcba4](https://github.com/informatievlaanderen/api/commit/46dcba4978100b42a826e6c02f47de11c75bc372))

# [15.5.0](https://github.com/informatievlaanderen/api/compare/v15.4.0...v15.5.0) (2021-11-29)


### Features

* use problemjson middleware ([2ec755d](https://github.com/informatievlaanderen/api/commit/2ec755d81fa8d2588e70e58ad1a82b1372fb3202))

# [15.4.0](https://github.com/informatievlaanderen/api/compare/v15.3.1...v15.4.0) (2021-11-26)


### Features

* use problemjson middleware ([37a1a44](https://github.com/informatievlaanderen/api/commit/37a1a44ccb69681bff426e0311e6116ee5081eb4))

## [15.3.1](https://github.com/informatievlaanderen/api/compare/v15.3.0...v15.3.1) (2021-11-24)


### Bug Fixes

* bump swagger ([e1697e7](https://github.com/informatievlaanderen/api/commit/e1697e7809dda74c5041da80b1155f89816ac77e))

# [15.3.0](https://github.com/informatievlaanderen/api/compare/v15.2.1...v15.3.0) (2021-11-23)


### Features

* add AcceptType ([ce65534](https://github.com/informatievlaanderen/api/commit/ce65534e7944cd4212334866b1788a852a64f91b))
* add PlainStringJsonConverter ([796691c](https://github.com/informatievlaanderen/api/commit/796691cb432bc0652b6a1eb0670b188c8b36856a))

## [15.2.1](https://github.com/informatievlaanderen/api/compare/v15.2.0...v15.2.1) (2021-11-22)


### Bug Fixes

* removed obsolete tag from ContentWithETagResult ([ae5b206](https://github.com/informatievlaanderen/api/commit/ae5b20628dbdad38eac9d9891838e1124a237bfc))

# [15.2.0](https://github.com/informatievlaanderen/api/compare/v15.1.1...v15.2.0) (2021-11-22)


### Features

* add CreatedWithLastObservedPositionAsETagResult ([15af7b7](https://github.com/informatievlaanderen/api/commit/15af7b7648073b81f121a5986080941b13a5ce04))

## [15.1.1](https://github.com/informatievlaanderen/api/compare/v15.1.0...v15.1.1) (2021-11-15)


### Bug Fixes

* delete unnecessary using ([d70e348](https://github.com/informatievlaanderen/api/commit/d70e3482f92ac85109aea759655b023307149abb))

# [15.1.0](https://github.com/informatievlaanderen/api/compare/v15.0.12...v15.1.0) (2021-11-15)


### Features

* extend search query ([45c3acc](https://github.com/informatievlaanderen/api/commit/45c3accb56498640a0856a1c7aad1869955dd95f))

## [15.0.12](https://github.com/informatievlaanderen/api/compare/v15.0.11...v15.0.12) (2021-10-01)


### Bug Fixes

* gawr-622 api documentation ([fa9c7dd](https://github.com/informatievlaanderen/api/commit/fa9c7dddd844a86c8be508cd0f82bd50894994c0))

## [15.0.11](https://github.com/informatievlaanderen/api/compare/v15.0.10...v15.0.11) (2021-09-17)


### Bug Fixes

* gawr-721 update ProblemDetail version ([9f34e71](https://github.com/informatievlaanderen/api/commit/9f34e713a99144e5ff3a3f56739523665d569311))

## [15.0.10](https://github.com/informatievlaanderen/api/compare/v15.0.9...v15.0.10) (2021-06-16)


### Bug Fixes

* added string null check for ruleset ([9a1a5a5](https://github.com/informatievlaanderen/api/commit/9a1a5a5f3d10be4c5c44e1e9e6f59a6a2329f935))

## [15.0.9](https://github.com/informatievlaanderen/api/compare/v15.0.8...v15.0.9) (2021-05-31)


### Bug Fixes

* bump swagger packages ([88e25f4](https://github.com/informatievlaanderen/api/commit/88e25f4202094b33991a9c560caa3d0873481e18))

## [15.0.8](https://github.com/informatievlaanderen/api/compare/v15.0.7...v15.0.8) (2021-05-31)


### Bug Fixes

* update swagger version ([3d32fdc](https://github.com/informatievlaanderen/api/commit/3d32fdc426de9f709c8ed57bbff23715ae4b4179))

## [15.0.7](https://github.com/informatievlaanderen/api/compare/v15.0.6...v15.0.7) (2021-05-31)


### Bug Fixes

* update fluent validation ([4952008](https://github.com/informatievlaanderen/api/commit/49520080324b4d19b3f9dfef77a9bc08216d13a0))

## [15.0.6](https://github.com/informatievlaanderen/api/compare/v15.0.5...v15.0.6) (2021-05-31)


### Bug Fixes

* update swagger ([ba3510a](https://github.com/informatievlaanderen/api/commit/ba3510aaf61a481851c357b6ba276b0bd6ffef81))

## [15.0.5](https://github.com/informatievlaanderen/api/compare/v15.0.4...v15.0.5) (2021-05-31)


### Bug Fixes

* update build-pipeline ([cf9f13a](https://github.com/informatievlaanderen/api/commit/cf9f13af9ab38ad5f277e0c7e68e0c056e755b97))

## [15.0.4](https://github.com/informatievlaanderen/api/compare/v15.0.3...v15.0.4) (2021-05-31)


### Bug Fixes

* pin swagger to correct major version ([cdb9fb6](https://github.com/informatievlaanderen/api/commit/cdb9fb6b19ae6559e904d65fd87af2212a1a269c))

## [15.0.3](https://github.com/informatievlaanderen/api/compare/v15.0.2...v15.0.3) (2021-05-28)


### Bug Fixes

* move to 5.0.6 ([ca5d6ce](https://github.com/informatievlaanderen/api/commit/ca5d6ce96fab47ac7540bac05d52d157cacd0583))

## [15.0.2](https://github.com/informatievlaanderen/api/compare/v15.0.1...v15.0.2) (2021-05-26)


### Bug Fixes

*  version bump Be.Vlaanderen.Basisregisters.AspNetCore.Swagger to 3.7.24 ([7a357d7](https://github.com/informatievlaanderen/api/commit/7a357d7480893a154d9fe323cb9b9e3d7c85fc28))

## [15.0.1](https://github.com/informatievlaanderen/api/compare/v15.0.0...v15.0.1) (2021-03-19)


### Bug Fixes

* error message quotes around parameter value ([cc563a5](https://github.com/informatievlaanderen/api/commit/cc563a5afafabaaaa813131d86db67ff247891fe))

# [15.0.0](https://github.com/informatievlaanderen/api/compare/v14.2.0...v15.0.0) (2021-03-16)


### Bug Fixes

* change embed value to sync embed value and used dutch error message GRAR-1891 ([ffc0da1](https://github.com/informatievlaanderen/api/commit/ffc0da1281e14fe05eed7c3fb9b236402fbb6542))


### BREAKING CHANGES

* renamed EmbedValue to SyncEmbedValue

# [14.2.0](https://github.com/informatievlaanderen/api/compare/v14.1.2...v14.2.0) (2021-03-10)


### Features

* add extract archive in an transaction in isolation ([e70c565](https://github.com/informatievlaanderen/api/commit/e70c5655663679dc141326f5d03e0e5488b5f14e))

## [14.1.2](https://github.com/informatievlaanderen/api/compare/v14.1.1...v14.1.2) (2021-02-15)


### Bug Fixes

* use problem details helper for validation details GRAR-1814 ([bbc4219](https://github.com/informatievlaanderen/api/commit/bbc421978c3124e1d38659ee3f6ef5b5133f11e7))

## [14.1.1](https://github.com/informatievlaanderen/api/compare/v14.1.0...v14.1.1) (2021-02-11)


### Bug Fixes

* add forbidden example ([12a7386](https://github.com/informatievlaanderen/api/commit/12a7386a4e4836cde895f55018472aec52380837))

# [14.1.0](https://github.com/informatievlaanderen/api/compare/v14.0.0...v14.1.0) (2021-02-11)


### Features

* add unauthorized example ([bfb74ed](https://github.com/informatievlaanderen/api/commit/bfb74ed4f23b1d425d51d805f24576d45d4038e3))

# [14.0.0](https://github.com/informatievlaanderen/api/compare/v13.1.1...v14.0.0) (2021-02-10)


### Bug Fixes

* remove extension method GRAR-1814 ([0a1fcc2](https://github.com/informatievlaanderen/api/commit/0a1fcc2561af19d56da13f61015e90c90738f500))
* remove hardcoded ProblemDetailsHelper instances GRAR-1814 ([c33a653](https://github.com/informatievlaanderen/api/commit/c33a6538a130cdd54db355e2c33078710a6528c6))


### BREAKING CHANGES

* CHANGE
modified the public constructor to use a ProblemDetailsHelper
* CHANGE
remove extension method that hides the use of ProblemDetailsHelper in
favor of explicitly getting a helper and using it

## [13.1.1](https://github.com/informatievlaanderen/api/compare/v13.1.0...v13.1.1) (2021-02-08)


### Bug Fixes

* fix trailing character GRAR-1814 ([4fb8c21](https://github.com/informatievlaanderen/api/commit/4fb8c215bd533f1a6c2a7cc0181cbcecb3f59ebf))

# [13.1.0](https://github.com/informatievlaanderen/api/compare/v13.0.2...v13.1.0) (2021-02-05)


### Features

* add rewriting a exception from problemdetails GRAR-1814 ([40af6c3](https://github.com/informatievlaanderen/api/commit/40af6c3495d357596263d89f72ab9f9b68da6dcc))

## [13.0.2](https://github.com/informatievlaanderen/api/compare/v13.0.1...v13.0.2) (2021-02-02)


### Bug Fixes

* move to 5.0.2 ([e2a6576](https://github.com/informatievlaanderen/api/commit/e2a65769719314704d00a6d0f094c8ba261c8593))

## [13.0.1](https://github.com/informatievlaanderen/api/compare/v13.0.0...v13.0.1) (2021-01-29)


### Bug Fixes

* update problem details dependencies GRAR-170 ([c2cf6f8](https://github.com/informatievlaanderen/api/commit/c2cf6f8d2a95f042365f5d86756a685ad672e15d))

# [13.0.0](https://github.com/informatievlaanderen/api/compare/v12.2.3...v13.0.0) (2021-01-28)


### Features

* add namespace override for problem details GRAR-170 ([d0e567b](https://github.com/informatievlaanderen/api/commit/d0e567b4904ea2444818d8116841303dfd7f0e50))


### BREAKING CHANGES

* CHANGES
- ProblemDetailHelper is turned into an instance class
- ApiProblemDetailsExceptionMapping.Map signature is extended with ProblemDetailsHelper

## [12.2.3](https://github.com/informatievlaanderen/api/compare/v12.2.2...v12.2.3) (2021-01-28)


### Bug Fixes

* update problem details middleware GRAR-170 ([7c60d72](https://github.com/informatievlaanderen/api/commit/7c60d7272d66cfa8b378ca7c98d488172a68f606))

## [12.2.2](https://github.com/informatievlaanderen/api/compare/v12.2.1...v12.2.2) (2021-01-27)


### Bug Fixes

* change wrong argument type GRAR-170 ([c35091c](https://github.com/informatievlaanderen/api/commit/c35091c7537ac6585d33e0a611182b7d1ae084e3))

## [12.2.1](https://github.com/informatievlaanderen/api/compare/v12.2.0...v12.2.1) (2021-01-27)


### Bug Fixes

* inject startup options in ProblemDetailException mapping GRAR-170 ([70dd1e4](https://github.com/informatievlaanderen/api/commit/70dd1e4a78e84149bbe1bf783f1699def31fe9c1))

# [12.2.0](https://github.com/informatievlaanderen/api/compare/v12.1.4...v12.2.0) (2021-01-27)


### Bug Fixes

* always assign status code 500 as a default for ApiExceptions ([a405392](https://github.com/informatievlaanderen/api/commit/a4053928e10e1e6bf0e185b7e4561f0bf44a9f1e))


### Features

* add mapping mapping of ProblemDetailsException GRAR-170 ([57e22f3](https://github.com/informatievlaanderen/api/commit/57e22f3a405719102e58081048141e404b417646))

## [12.1.4](https://github.com/informatievlaanderen/api/compare/v12.1.3...v12.1.4) (2021-01-13)


### Bug Fixes

* include all actions in swagger doc ([523263c](https://github.com/informatievlaanderen/api/commit/523263cbd2f8d57cc6001ba86f7a95fb93d0db65))

## [12.1.3](https://github.com/informatievlaanderen/api/compare/v12.1.2...v12.1.3) (2020-12-24)


### Bug Fixes

* get versioned problemdetail base uri GRAR-400 ([9cd4f31](https://github.com/informatievlaanderen/api/commit/9cd4f316955c4d9dc481f0f0b13e5f92fb000396))

## [12.1.2](https://github.com/informatievlaanderen/api/compare/v12.1.1...v12.1.2) (2020-12-24)


### Bug Fixes

* correct error uri ([97c5561](https://github.com/informatievlaanderen/api/commit/97c55613180d4494aaf1ba8ad5edde6ef86b6815))
* correct error validation uri ([51af928](https://github.com/informatievlaanderen/api/commit/51af92866765823483da462059ff8b447d37f6fc))
* correct problemdetails instance url GRAR-400 ([764eb6f](https://github.com/informatievlaanderen/api/commit/764eb6f94781fa76ad0704617e5f2d9631b0743a))

## [12.1.1](https://github.com/informatievlaanderen/api/compare/v12.1.0...v12.1.1) (2020-12-18)


### Bug Fixes

* move to 5.0.1 ([ad28fa8](https://github.com/informatievlaanderen/api/commit/ad28fa821078bac18d7ff5c2558d7cf2b1dc61ae))

# [12.1.0](https://github.com/informatievlaanderen/api/compare/v12.0.3...v12.1.0) (2020-12-15)


### Bug Fixes

* remove double closing brace ([90e485b](https://github.com/informatievlaanderen/api/commit/90e485b569e3346caf283d2639dc30488d3dec3d))


### Features

* extend httprequest with check for html accept header ([ba8247b](https://github.com/informatievlaanderen/api/commit/ba8247bc80a556704f0e0964b01189a4b7803eab))

## [12.0.3](https://github.com/informatievlaanderen/api/compare/v12.0.2...v12.0.3) (2020-11-19)


### Bug Fixes

* update eventhandling ([9773632](https://github.com/informatievlaanderen/api/commit/97736329835bf42dc619b627693e75ebde5179ad))

## [12.0.2](https://github.com/informatievlaanderen/api/compare/v12.0.1...v12.0.2) (2020-11-18)


### Bug Fixes

* remove set-env usage in gh-actions ([4954260](https://github.com/informatievlaanderen/api/commit/4954260b7907bd67950b0518196d12558c054473))

## [12.0.1](https://github.com/informatievlaanderen/api/compare/v12.0.0...v12.0.1) (2020-11-16)


### Bug Fixes

* add NotModified response to LastObeservedPosition ([c78e5b1](https://github.com/informatievlaanderen/api/commit/c78e5b1159a25761d43f252b321d70e2c5cc1e59))

# [12.0.0](https://github.com/informatievlaanderen/api/compare/v11.8.2...v12.0.0) (2020-11-12)


### Bug Fixes

* removed deprecated function ([26ab7ba](https://github.com/informatievlaanderen/api/commit/26ab7bafe0748f08ff47872ed65ae1077b8a581c))
* update swashbuckle dependcy ([58603b4](https://github.com/informatievlaanderen/api/commit/58603b48fc0cee57f02715dfbe9ce0488efbb380))


### BREAKING CHANGES

* CHANGES
* JsonConverter property is removed from the attribute

## [11.8.2](https://github.com/informatievlaanderen/api/compare/v11.8.1...v11.8.2) (2020-09-21)


### Bug Fixes

* move to 3.1.8 ([c45c343](https://github.com/informatievlaanderen/api/commit/c45c343e6904201bab82c75df53a97e033735ffe))

## [11.8.1](https://github.com/informatievlaanderen/api/compare/v11.8.0...v11.8.1) (2020-09-02)


### Bug Fixes

* bump swagger to fix utf docs ([d9842cb](https://github.com/informatievlaanderen/api/commit/d9842cb13bffaf8f7ca4b06ffb10236bb16a0f44))
* bump swagger to fix utf docs ([681eeb1](https://github.com/informatievlaanderen/api/commit/681eeb138648e574c23c705831d83e17e6cc172c))

# [11.8.0](https://github.com/informatievlaanderen/api/compare/v11.7.5...v11.8.0) (2020-08-20)


### Bug Fixes

* phase out e-tag usuage ([198ffea](https://github.com/informatievlaanderen/api/commit/198ffea3243fe96d120485f77a5c70e9e924a4e2))


### Features

* add lastObservedPosition headers ([cd23e9d](https://github.com/informatievlaanderen/api/commit/cd23e9d948a962840a81656b460c1e472a3aa598))

## [11.7.5](https://github.com/informatievlaanderen/api/compare/v11.7.4...v11.7.5) (2020-07-18)


### Bug Fixes

* move to 3.1.6 ([f9e9f9c](https://github.com/informatievlaanderen/api/commit/f9e9f9c1893cc8b9324f3b1076e786b86fc25953))

## [11.7.4](https://github.com/informatievlaanderen/api/compare/v11.7.3...v11.7.4) (2020-07-14)


### Bug Fixes

* make embed value parse stricter GRAR-1465 ([7e18a97](https://github.com/informatievlaanderen/api/commit/7e18a97b609e410e98b01b420667e409bcedf474))

## [11.7.3](https://github.com/informatievlaanderen/api/compare/v11.7.2...v11.7.3) (2020-07-13)


### Bug Fixes

* throw validation exception on invalid parse GRAR-1465 ([010a78d](https://github.com/informatievlaanderen/api/commit/010a78d755618577c448c9e8626ddc722949f0f1))

## [11.7.2](https://github.com/informatievlaanderen/api/compare/v11.7.1...v11.7.2) (2020-07-13)


### Bug Fixes

* expose exception so it can be caught GRAR-1465 ([fad8bfd](https://github.com/informatievlaanderen/api/commit/fad8bfd68d048cd0b14d759771ae9726dd881f35))

## [11.7.1](https://github.com/informatievlaanderen/api/compare/v11.7.0...v11.7.1) (2020-07-13)


### Bug Fixes

* support deserialising for object as well GRAR-1465 ([985a816](https://github.com/informatievlaanderen/api/commit/985a81663310853c3b57121c1af5adda4ac8f65b))

# [11.7.0](https://github.com/informatievlaanderen/api/compare/v11.6.0...v11.7.0) (2020-07-13)


### Features

* add embed value object GRAR-1465 ([5994885](https://github.com/informatievlaanderen/api/commit/59948850b8a4631bdb049ba1bf27080434e2e3b2))

# [11.6.0](https://github.com/informatievlaanderen/api/compare/v11.5.1...v11.6.0) (2020-07-09)


### Features

* allow configuration of problemdetails ([329ec63](https://github.com/informatievlaanderen/api/commit/329ec63a1edfa9f2102b2d6788cb764998227fc3))

## [11.5.1](https://github.com/informatievlaanderen/api/compare/v11.5.0...v11.5.1) (2020-07-02)


### Bug Fixes

* update streamstore ([7470403](https://github.com/informatievlaanderen/api/commit/747040332d12ffda6288c91a968fe3a2cb1ed111))

# [11.5.0](https://github.com/informatievlaanderen/api/compare/v11.4.2...v11.5.0) (2020-06-30)


### Features

* add atomfeedwriter write metadata with alternate uris ([4081dfd](https://github.com/informatievlaanderen/api/commit/4081dfd7fd25cd1fbdd20258df85d5b24a2e8be1))

## [11.4.2](https://github.com/informatievlaanderen/api/compare/v11.4.1...v11.4.2) (2020-06-25)


### Bug Fixes

* update to openapi 1.2.2 ([7b22d5c](https://github.com/informatievlaanderen/api/commit/7b22d5c4fdcfb7bb4dde2f8268dbdaeff1f70b6c))

## [11.4.1](https://github.com/informatievlaanderen/api/compare/v11.4.0...v11.4.1) (2020-06-22)


### Bug Fixes

* configure baseurls for all exceptions GRAR-1358 GRAR-1357 ([baaa571](https://github.com/informatievlaanderen/api/commit/baaa5715ee7dd44bd236f2067fdff145b8b610de))

# [11.4.0](https://github.com/informatievlaanderen/api/compare/v11.3.0...v11.4.0) (2020-06-22)


### Features

* allow baseurl configuration in problemdetails GRAR-1358 GRAR-1357 ([f52e517](https://github.com/informatievlaanderen/api/commit/f52e5176beef6ea4d9f8c9a41debaa6e183976d1))

# [11.3.0](https://github.com/informatievlaanderen/api/compare/v11.2.6...v11.3.0) (2020-06-22)


### Features

* add support for more frame options directives ([6d01b3f](https://github.com/informatievlaanderen/api/commit/6d01b3f742356e9a421a9c5aab0b44c670f19adf))

## [11.2.6](https://github.com/informatievlaanderen/api/compare/v11.2.5...v11.2.6) (2020-06-22)


### Bug Fixes

* make sure docs work again GRAR-1375 ([f25bfed](https://github.com/informatievlaanderen/api/commit/f25bfed26de528c1edbf0f276954f64985b3a3e9))

## [11.2.5](https://github.com/informatievlaanderen/api/compare/v11.2.4...v11.2.5) (2020-06-19)


### Bug Fixes

* move to 3.1.5 ([d246377](https://github.com/informatievlaanderen/api/commit/d246377c9e2c758d52bb7d9175d1a87164856f5f))

## [11.2.4](https://github.com/informatievlaanderen/api/compare/v11.2.3...v11.2.4) (2020-05-29)


### Bug Fixes

* update dependencies GRAR-752 ([0f0d701](https://github.com/informatievlaanderen/api/commit/0f0d7012013a45cf96f64d634369653d63cb3446))

## [11.2.3](https://github.com/informatievlaanderen/api/compare/v11.2.2...v11.2.3) (2020-05-18)


### Bug Fixes

* move to 3.1.4 ([2dc5fb8](https://github.com/informatievlaanderen/api/commit/2dc5fb8869c22493dd94d35c425539b73874e207))

## [11.2.2](https://github.com/informatievlaanderen/api/compare/v11.2.1...v11.2.2) (2020-05-14)


### Bug Fixes

* remove dotnet 3.1.4 references ([e513827](https://github.com/informatievlaanderen/api/commit/e513827))

## [11.2.1](https://github.com/informatievlaanderen/api/compare/v11.2.0...v11.2.1) (2020-05-13)


### Bug Fixes

* move to GH-actions ([e3473db](https://github.com/informatievlaanderen/api/commit/e3473db))

# [11.2.0](https://github.com/informatievlaanderen/api/compare/v11.1.9...v11.2.0) (2020-05-12)


### Features

* add configuration of cors ([0d75b1f](https://github.com/informatievlaanderen/api/commit/0d75b1f))

## [11.1.9](https://github.com/informatievlaanderen/api/compare/v11.1.8...v11.1.9) (2020-05-07)


### Bug Fixes

* exclude docs from traces ([3bacc9c](https://github.com/informatievlaanderen/api/commit/3bacc9c))

## [11.1.8](https://github.com/informatievlaanderen/api/compare/v11.1.7...v11.1.8) (2020-05-07)


### Bug Fixes

* ignore health from tracing ([e4dc3b0](https://github.com/informatievlaanderen/api/commit/e4dc3b0))

## [11.1.7](https://github.com/informatievlaanderen/api/compare/v11.1.6...v11.1.7) (2020-04-15)


### Bug Fixes

* update swashbuckle ([c2ea728](https://github.com/informatievlaanderen/api/commit/c2ea728))

## [11.1.6](https://github.com/informatievlaanderen/api/compare/v11.1.5...v11.1.6) (2020-04-03)


### Bug Fixes

* use correct build user GR-1214 ([7e9cc25](https://github.com/informatievlaanderen/api/commit/7e9cc25))

## [11.1.5](https://github.com/informatievlaanderen/api/compare/v11.1.4...v11.1.5) (2020-04-03)


### Bug Fixes

* update swagger version GR-1214 ([ca93cb4](https://github.com/informatievlaanderen/api/commit/ca93cb4))

## [11.1.4](https://github.com/informatievlaanderen/api/compare/v11.1.3...v11.1.4) (2020-03-12)


### Bug Fixes

* bump problem details contract name fix ([02d4982](https://github.com/informatievlaanderen/api/commit/02d4982))

## [11.1.3](https://github.com/informatievlaanderen/api/compare/v11.1.2...v11.1.3) (2020-03-03)


### Bug Fixes

* bump netcore to 3.1.2 ([0945b6d](https://github.com/informatievlaanderen/api/commit/0945b6d))

## [11.1.2](https://github.com/informatievlaanderen/api/compare/v11.1.1...v11.1.2) (2020-02-26)


### Bug Fixes

* update swagger to version with updated json formatter ([b5841b2](https://github.com/informatievlaanderen/api/commit/b5841b2))

## [11.1.1](https://github.com/informatievlaanderen/api/compare/v11.1.0...v11.1.1) (2020-02-26)


### Bug Fixes

* update mvc json formatter ([06a9a72](https://github.com/informatievlaanderen/api/commit/06a9a72))
* update swagger dependency ([721a9e9](https://github.com/informatievlaanderen/api/commit/721a9e9))

# [11.1.0](https://github.com/informatievlaanderen/api/compare/v11.0.0...v11.1.0) (2020-02-24)


### Features

* add support for format filters ([7409def](https://github.com/informatievlaanderen/api/commit/7409def))
* make action nullable ([27be08a](https://github.com/informatievlaanderen/api/commit/27be08a))
* make formatter mappings configurable ([31228a7](https://github.com/informatievlaanderen/api/commit/31228a7))

# [11.0.0](https://github.com/informatievlaanderen/api/compare/v10.8.0...v11.0.0) (2020-02-21)


### Features

* add HasNextPage instead of TotalItems for paging ([e70e171](https://github.com/informatievlaanderen/api/commit/e70e171))


### BREAKING CHANGES

* add HasNextPage instead of TotalItems, TotalPages

# [10.8.0](https://github.com/informatievlaanderen/api/compare/v10.7.11...v10.8.0) (2020-02-17)


### Features

* determine json property order based on datamember ([9aef1ab](https://github.com/informatievlaanderen/api/commit/9aef1ab))

## [10.7.11](https://github.com/informatievlaanderen/api/compare/v10.7.10...v10.7.11) (2020-02-11)


### Bug Fixes

* make sure to ignore the xml proxy for json ([bfaf635](https://github.com/informatievlaanderen/api/commit/bfaf635))

## [10.7.10](https://github.com/informatievlaanderen/api/compare/v10.7.9...v10.7.10) (2020-02-11)


### Bug Fixes

* only support EN culture for now ([55c6226](https://github.com/informatievlaanderen/api/commit/55c6226))
* update problemdetails ([3d1161e](https://github.com/informatievlaanderen/api/commit/3d1161e))

## [10.7.9](https://github.com/informatievlaanderen/api/compare/v10.7.8...v10.7.9) (2020-02-06)


### Bug Fixes

* update problemdetails to fix error examples ([8a11877](https://github.com/informatievlaanderen/api/commit/8a11877))

## [10.7.8](https://github.com/informatievlaanderen/api/compare/v10.7.7...v10.7.8) (2020-02-05)


### Bug Fixes

* update badrequest example with validation errors ([ee099f7](https://github.com/informatievlaanderen/api/commit/ee099f7))
* update problem details ([63ed119](https://github.com/informatievlaanderen/api/commit/63ed119))

## [10.7.7](https://github.com/informatievlaanderen/api/compare/v10.7.6...v10.7.7) (2020-02-04)


### Bug Fixes

* correct instance uri for examples ([9a9b77c](https://github.com/informatievlaanderen/api/commit/9a9b77c))

## [10.7.6](https://github.com/informatievlaanderen/api/compare/v10.7.5...v10.7.6) (2020-02-04)


### Bug Fixes

* upgrade problem details ([acb3c4d](https://github.com/informatievlaanderen/api/commit/acb3c4d))

## [10.7.5](https://github.com/informatievlaanderen/api/compare/v10.7.4...v10.7.5) (2020-02-04)


### Bug Fixes

* update problem details ([#18](https://github.com/informatievlaanderen/api/issues/18)) ([74ccb94](https://github.com/informatievlaanderen/api/commit/74ccb94))

## [10.7.4](https://github.com/informatievlaanderen/api/compare/v10.7.3...v10.7.4) (2020-02-04)


### Bug Fixes

* change prefix for problem details instance GR-940 ([bd58e46](https://github.com/informatievlaanderen/api/commit/bd58e46))
* change prefix for problem details instance GR-940 ([dca63ab](https://github.com/informatievlaanderen/api/commit/dca63ab))

## [10.7.3](https://github.com/informatievlaanderen/api/compare/v10.7.2...v10.7.3) (2020-02-03)


### Bug Fixes

* add problemdetail instance to statuscode ([251345a](https://github.com/informatievlaanderen/api/commit/251345a))

## [10.7.2](https://github.com/informatievlaanderen/api/compare/v10.7.1...v10.7.2) (2020-02-03)


### Bug Fixes

* add type to problemdetails ([6e6ad92](https://github.com/informatievlaanderen/api/commit/6e6ad92))

## [10.7.1](https://github.com/informatievlaanderen/api/compare/v10.7.0...v10.7.1) (2020-02-03)


### Bug Fixes

* update swagger ([592400c](https://github.com/informatievlaanderen/api/commit/592400c))

# [10.7.0](https://github.com/informatievlaanderen/api/compare/v10.6.1...v10.7.0) (2020-02-03)


### Features

* add AfterMvcCore hook ([5d01b56](https://github.com/informatievlaanderen/api/commit/5d01b56))

## [10.6.1](https://github.com/informatievlaanderen/api/compare/v10.6.0...v10.6.1) (2020-02-03)


### Bug Fixes

* update dependencies ([d16a998](https://github.com/informatievlaanderen/api/commit/d16a998))

# [10.6.0](https://github.com/informatievlaanderen/api/compare/v10.5.0...v10.6.0) (2020-02-01)


### Features

* upgrade netcoreapp31 and dependencies ([909a123](https://github.com/informatievlaanderen/api/commit/909a123))

# [10.5.0](https://github.com/informatievlaanderen/api/compare/v10.4.0...v10.5.0) (2020-01-31)


### Features

* dont depend on netcoreapp22 8 via destructurama ([4d8665b](https://github.com/informatievlaanderen/api/commit/4d8665b))

# [10.4.0](https://github.com/informatievlaanderen/api/compare/v10.3.0...v10.4.0) (2020-01-31)


### Features

* prevent depending on netcoreapp22 8 ([54bc476](https://github.com/informatievlaanderen/api/commit/54bc476))

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
