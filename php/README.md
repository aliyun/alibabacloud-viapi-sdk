English | [简体中文](README-CN.md)

![](https://aliyunsdk-pages.alicdn.com/icons/AlibabaCloud.svg)

## Alibaba Cloud Viapi Utils Library for PHP

## Installation

### Composer

```bash
composer require alibabacloud/viapi-utils
```

## Demo

```php
<?php
namespace demo;

require __DIR__ . '/vendor/autoload.php';

use AlibabaCloud\SDK\ViapiUtils\ViapiUtils;
use AlibabaCloud\Tea\Exception\TeaUnableRetryError;

try {
    $url = ViapiUtils::upload("<Access-Key-Id>", "<Access-Key-Secret>", "<File-Path>");
    var_dump($url);
} catch (TeaUnableRetryError $e) {
    var_dump($e->getMessage());
    var_dump($e->getErrorInfo());
    var_dump($e->getLastException());
    var_dump($e->getLastRequest());
}
```

## Issues

[Opening an Issue](https://github.com/aliyun/alibabacloud-sdk/issues/new), Issues not conforming to the guidelines may be closed immediately.

## Changelog

Detailed changes for each release are documented in the [release notes](./ChangeLog.txt).

## References

* [Latest Release](https://github.com/aliyun/alibabacloud-sdk)

## License

[Apache-2.0](http://www.apache.org/licenses/LICENSE-2.0)

Copyright (c) 2009-present, Alibaba Cloud All rights reserved.
