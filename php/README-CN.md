[English](README.md) | 简体中文

![](https://aliyunsdk-pages.alicdn.com/icons/AlibabaCloud.svg)

## Alibaba Cloud Viapi Utils Library for PHP

## 安装

### Composer

```bash
composer require alibabacloud/viapi-utils
```

## 使用示例

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

## 问题

[提交 Issue](https://github.com/aliyun/alibabacloud-sdk/issues/new)，不符合指南的问题可能会立即关闭。

## 发行说明

每个版本的详细更改记录在[发行说明](./ChangeLog.txt)中。

## 相关

* [最新源码](https://github.com/aliyun/alibabacloud-sdk)

## 许可证

[Apache-2.0](http://www.apache.org/licenses/LICENSE-2.0)

Copyright (c) 2009-present, Alibaba Cloud All rights reserved.
