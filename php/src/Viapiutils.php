<?php

// This file is auto-generated, don't edit it. Thanks.

namespace AlibabaCloud\SDK\Viapiutils;

use AlibabaCloud\SDK\OSS\OSS;
use AlibabaCloud\SDK\OSS\OSS\PutObjectRequest;
use AlibabaCloud\SDK\ViapiTool\ViapiTool;
use AlibabaCloud\SDK\Viapiutils\V20200401\Models\GetOssStsTokenRequest;
use AlibabaCloud\SDK\Viapiutils\V20200401\Viapiutils as AlibabaCloudSDKViapiutilsV20200401Viapiutils;
use AlibabaCloud\Tea\Exception\TeaError;
use AlibabaCloud\Tea\OSSUtils\OSSUtils\RuntimeOptions;
use AlibabaCloud\Tea\Rpc\Rpc\Config;
use AlibabaCloud\Tea\Utils\Utils;

class Viapiutils
{
    /**
     * @param string $accessKeyId
     * @param string $accessKeySecret
     * @param string $filePath
     *
     * @throws \Exception
     *
     * @return string
     */
    public static function upload($accessKeyId, $accessKeySecret, $filePath)
    {
        $ins = null;

        try {
            $viConfig = new Config([
                'accessKeyId'     => $accessKeyId,
                'accessKeySecret' => $accessKeySecret,
                'type'            => 'access_key',
                'endpoint'        => 'viapiutils.cn-shanghai.aliyuncs.com',
                'regionId'        => 'cn-shanghai',
            ]);
            $viclient   = new AlibabaCloudSDKViapiutilsV20200401Viapiutils($viConfig);
            $viRequest  = new GetOssStsTokenRequest([]);
            $viResponse = $viclient->getOssStsToken($viRequest);
            if (Utils::isUnset($viResponse) || Utils::isUnset($viResponse->data->toMap())) {
                throw new TeaError([
                    'code'    => 'InvalidResponse',
                    'message' => 'GetOssStsToken gets a invalid response',
                    'data'    => $viResponse,
                ]);
            }
            $fileName = '';
            if (ViapiTool::startsWith($filePath, 'https://') || ViapiTool::startsWith($filePath, 'http://')) {
                $filePath = ViapiTool::decode($filePath, 'UTF-8');
                $fileName = ViapiTool::match($filePath, '\\w+.(jpg|gif|png|jpeg|bmp|mov|mp4|avi)');
                if (Utils::empty_($fileName)) {
                    $fileName = ViapiTool::getNameFromUrl($filePath);
                    $fileName = ViapiTool::subStringAfterLast($fileName, '/');
                }
                $ins = ViapiTool::getStreamFromNet($filePath);
            } else {
                $ins      = ViapiTool::getStreamFromPath($filePath);
                $fileName = ViapiTool::getNameFromPath($filePath);
            }
            $ossConfig = new \AlibabaCloud\SDK\OSS\OSS\Config([
                'accessKeyId'     => $viResponse->data->accessKeyId,
                'accessKeySecret' => $viResponse->data->accessKeySecret,
                'securityToken'   => $viResponse->data->securityToken,
                'type'            => 'sts',
                'endpoint'        => 'oss-cn-shanghai.aliyuncs.com',
                'regionId'        => 'cn-shanghai',
            ]);
            $ossClient     = new OSS($ossConfig);
            $objectName    = '' . $accessKeyId . '/' . Utils::getNonce() . '' . $fileName . '';
            $uploadRequest = new PutObjectRequest([
                'bucketName' => 'viapi-customer-temp',
                'body'       => $ins,
                'objectName' => $objectName,
            ]);
            $ossRuntime = new RuntimeOptions([]);
            $ossClient->putObject($uploadRequest, $ossRuntime);

            return 'http://viapi-customer-temp.oss-cn-shanghai.aliyuncs.com/' . $objectName . '';
        } finally {
            if (!Utils::isUnset($ins)) {
                ViapiTool::close($ins);
            }
        }
    }
}
