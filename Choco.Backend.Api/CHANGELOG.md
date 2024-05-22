# Change Log

All notable changes to this project will be documented in this file. See [versionize](https://github.com/versionize/versionize) for commit guidelines.

<a name="3.3.0"></a>
## [3.3.0](https://www.github.com/choco-manager/Backend/releases/tag/v3.3.0) (2024-05-22)

### Features

* add endpoint to get order by id ([03e1755](https://www.github.com/choco-manager/Backend/commit/03e17557b561b0912c45ae64ec584944aae7f4cb))

<a name="3.2.0"></a>
## [3.2.0](https://www.github.com/choco-manager/Backend/releases/tag/v3.2.0) (2024-05-22)

### Features

* add endpoint to create order ([11ef3b3](https://www.github.com/choco-manager/Backend/commit/11ef3b3a6aad89268620f96cb3e9be536e12a0dc))

<a name="3.1.0"></a>
## [3.1.0](https://www.github.com/choco-manager/Backend/releases/tag/v3.1.0) (2024-05-22)

### Features

* add endpoint to get all orders in paged format ([04a3f5d](https://www.github.com/choco-manager/Backend/commit/04a3f5d0832f5440f56af76bdb2f6aff5b010b88))

### Bug Fixes

* replace `List<ProductDto>` with more extensible `ListOfProducts` ([fd29ef4](https://www.github.com/choco-manager/Backend/commit/fd29ef43b02d0a3ed8af5e4bfa9389b4549d60dc))

<a name="3.0.0"></a>
## [3.0.0](https://www.github.com/choco-manager/Backend/releases/tag/v3.0.0) (2024-05-22)

### Features

* add automatic database migration on startup ([e6d3808](https://www.github.com/choco-manager/Backend/commit/e6d3808efee603141b8969fc54b133a15422d98e))
* add automatic registration of use cases in DI ([9eff33c](https://www.github.com/choco-manager/Backend/commit/9eff33c1c9949d47b16ad0395c7e9bc3dd5b66d2))
* add common interface for all use cases ([4e2fa92](https://www.github.com/choco-manager/Backend/commit/4e2fa92801e5d51899a36abcc915f9bcd2907b93))
* add common model for requests of "find by id" kind ([f5a3fa8](https://www.github.com/choco-manager/Backend/commit/f5a3fa8efe1b6b73c77637f178d4c96b1762cf64))
* add common paged request ([42cd044](https://www.github.com/choco-manager/Backend/commit/42cd0447ef659d37e2f0808221b2c3ee9c020845))
* add connection to database ([9e3bf0b](https://www.github.com/choco-manager/Backend/commit/9e3bf0b8a42fe86f275b3e5681ece18ae08c4d58))
* add converting result to status code ([4a85577](https://www.github.com/choco-manager/Backend/commit/4a85577f103e6a0bdba868d553cfbebbbed3ca17))
* add database connection string to config ([962f1e0](https://www.github.com/choco-manager/Backend/commit/962f1e0527b92f1148f5c1b04f5b10d81f5b65fd))
* add dummy register endpoint ([a8b288d](https://www.github.com/choco-manager/Backend/commit/a8b288d06c0ac7467b092504ef615b408e6df47a))
* add endpoint for stocktaking products ([58287e2](https://www.github.com/choco-manager/Backend/commit/58287e28b60082a13c73db8cd0a8a0dbfabbfd48))
* add endpoint to create new product ([513c6ea](https://www.github.com/choco-manager/Backend/commit/513c6ea3a3df80504691dc04d65c46d6a957a4cc))
* add endpoint to force delete products ([38b4806](https://www.github.com/choco-manager/Backend/commit/38b4806d35517f58240fdc10d4958c8ccbb88442))
* add endpoint to get all product tags ([a5a64f6](https://www.github.com/choco-manager/Backend/commit/a5a64f64e43847c16a03bdacc22bec7853cd46c6))
* add endpoint to get all products ([e4f098b](https://www.github.com/choco-manager/Backend/commit/e4f098bea37a7e6e6b06d99d3dec2051fd4096bb))
* add endpoint to get product by id ([c92c919](https://www.github.com/choco-manager/Backend/commit/c92c91999f85dad16d7dca13f1905b0b85b3d8c8))
* add endpoint to log out current user ([23225ab](https://www.github.com/choco-manager/Backend/commit/23225ab751ca75fb92b798254114f9faa882bc9f))
* add endpoint to request token for password restoration ([1e677b5](https://www.github.com/choco-manager/Backend/commit/1e677b52e1bb01244e525616d5db6a3a330696e5))
* add endpoint to restore deleted products ([1c02239](https://www.github.com/choco-manager/Backend/commit/1c02239fb8d34a3d3d79c2cdb098073d53abf5aa))
* add endpoint to restore password ([d4dc951](https://www.github.com/choco-manager/Backend/commit/d4dc95118ca895c3f27309042bd2a068bccde2f3))
* add endpoint to softly delete products ([5ca884a](https://www.github.com/choco-manager/Backend/commit/5ca884abc70037706e5ce0dec6c6eb8da4a4da1b))
* add endpoint to update product ([41eef7a](https://www.github.com/choco-manager/Backend/commit/41eef7acf35e11316bb75f1c27b493003348676e))
* add interface for use cases, that returns PagedResult<T> ([052c79c](https://www.github.com/choco-manager/Backend/commit/052c79c1ef11c313361c0ab3ea0860bec7f274e2))
* add logging exception ([d5fec7d](https://www.github.com/choco-manager/Backend/commit/d5fec7d3049ae77ee01303324d33f43fe9ee7740))
* add login endpoint ([0b0c8c7](https://www.github.com/choco-manager/Backend/commit/0b0c8c7949a765ed2674973dadecbb168a84e951))
* add mapper for product model ([741b5e1](https://www.github.com/choco-manager/Backend/commit/741b5e1e745b21093debf06bae9a013e0bbfaf63))
* add ping endpoint ([ab13360](https://www.github.com/choco-manager/Backend/commit/ab1336082dc0547fac0f1ec366836ad0da723dd9))
* add post-processor, that will catch all occured exceptions ([1589218](https://www.github.com/choco-manager/Backend/commit/1589218868ee8122d1cb542d67c5c148b903074b))
* add refresh token endpoint ([7530603](https://www.github.com/choco-manager/Backend/commit/7530603570ecc03f2774feeec5c786c31199fff6))
* add registering use cases ([2fb4ea7](https://www.github.com/choco-manager/Backend/commit/2fb4ea75683917815cfd3fab67806487ef1fd65c))
* add saving history of prices ([1d239f4](https://www.github.com/choco-manager/Backend/commit/1d239f4d1eec30b377a007ba8d5f15c0ceeabac0))
* add swagger support ([a73ee74](https://www.github.com/choco-manager/Backend/commit/a73ee74507d95c86f0be78e6e3aea83b49cc22ac))
* add template for validating token (not working correctly yet) ([1818b73](https://www.github.com/choco-manager/Backend/commit/1818b736a623ef3d858535d145d2e6dd2bb1d40b))
* add token validation ([3ff2edb](https://www.github.com/choco-manager/Backend/commit/3ff2edb3d74734f64bbea5841ffb59fe4ac9c44b))
* add whoami endpoint ([9d45156](https://www.github.com/choco-manager/Backend/commit/9d45156042488f87369cbf817f49cdcccdb441c4))
* automatically register swagger tags ([6b01cc7](https://www.github.com/choco-manager/Backend/commit/6b01cc7a060566e3543d65be9cb1363a6de8fb95))
* configure authentication & authorization ([4a99cc3](https://www.github.com/choco-manager/Backend/commit/4a99cc3753ebbefb522d224575b83325989d8811))
* enable using serilog ([c2020c9](https://www.github.com/choco-manager/Backend/commit/c2020c9b3ba30a5d4eeccc4e3551e5b397f8c606))
* replace useless extensions with own code ([442b32c](https://www.github.com/choco-manager/Backend/commit/442b32c4be5324b329d0228b4187de5cb7cd94a3))

### Bug Fixes

* add url to instantly launch swagger on startup ([c7de30d](https://www.github.com/choco-manager/Backend/commit/c7de30df1328cdf70f90d287ebfda66c84bd3fe8))
* building routes rules ([ed6b0d1](https://www.github.com/choco-manager/Backend/commit/ed6b0d166c4df20d9ff2725eed361cf5a57d3f5b))
* call on already revoked token ([8e3f33d](https://www.github.com/choco-manager/Backend/commit/8e3f33dd849f4c6138f65b25e6eb9fe95bdf376d))
* checking if token is valid ([fee167c](https://www.github.com/choco-manager/Backend/commit/fee167c0653d61107bf254a0b46f0a436bcc87cf))
* checking token's validity condition ([63e1e4b](https://www.github.com/choco-manager/Backend/commit/63e1e4b5ae77cb10f0a3a104561a3df9de0f01c7))
* code, that automatically registers use cases ([9cca4e8](https://www.github.com/choco-manager/Backend/commit/9cca4e8df4231327d1ddef8d0235e60c8cfdf1fa))
* condition, so this processor won't be run, if handler throws an exception ([acf3c15](https://www.github.com/choco-manager/Backend/commit/acf3c15f656ad9b42d3a1e8b089534b66b682893))
* di ([537afd3](https://www.github.com/choco-manager/Backend/commit/537afd3005e66673442e2a28fbee612c171ff115))
* imports ([f34e50b](https://www.github.com/choco-manager/Backend/commit/f34e50be555abcc880ecf06ecde27f8bba0b52be))
* make cancellation token optional ([86108f5](https://www.github.com/choco-manager/Backend/commit/86108f584bb9ac48ab804aa24ac4b7a584ea500e))
* move passing fcm token to body ([35ac364](https://www.github.com/choco-manager/Backend/commit/35ac3642913c486b487084a7a1d48c5ca1a28cf4))
* passing db context options ([6c6c287](https://www.github.com/choco-manager/Backend/commit/6c6c2877dabc627fb9d8f3df3894ba1572030ac9))
* ping endpoint return type ([db0ed16](https://www.github.com/choco-manager/Backend/commit/db0ed16d07e26f87283d6e3f6ed4ee8a8f6ff3c3))
* strange shit with login endpoint ([673b61f](https://www.github.com/choco-manager/Backend/commit/673b61fe1e4f1debdc242b43c258d7612995260b))
* update status codes for some endpoints ([4d70427](https://www.github.com/choco-manager/Backend/commit/4d70427a5ae5b4779427bd686f7932f7e3d41b10))

