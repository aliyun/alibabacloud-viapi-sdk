// This file is auto-generated, don't edit it
import OSS, * as $OSS from '@alicloud/oss-client';
import Util from '@alicloud/tea-util';
import RPC, * as $RPC from '@alicloud/rpc-client';
import OSSUtil, * as $OSSUtil from '@alicloud/oss-util';
import viapiutils20200401, * as $viapiutils20200401 from '@alicloud/viapiutils20200401';
import ViapiTool from '@alicloud/viapi-tool';
import * as $tea from '@alicloud/tea-typescript';
import { Readable } from 'stream';


export default class Client {

  static async upload(accessKeyId: string, accessKeySecret: string, filePath: string): Promise<string> {
    let ins: Readable = null;
    try {
      let viConfig = new $RPC.Config({
        accessKeyId: accessKeyId,
        accessKeySecret: accessKeySecret,
        type: "access_key",
        endpoint: "viapiutils.cn-shanghai.aliyuncs.com",
        regionId: "cn-shanghai",
      });
      let viclient = new viapiutils20200401(viConfig);
      let viRequest = new $viapiutils20200401.GetOssStsTokenRequest({});
      let viResponse = await viclient.getOssStsToken(viRequest);
      if (Util.isUnset($tea.toMap(viResponse)) || Util.isUnset($tea.toMap(viResponse.data))) {
        throw $tea.newError({
          code: "InvalidResponse",
          message: "GetOssStsToken gets a invalid response",
          data: viResponse,
        });
      }

      let fileName = "";
      if (ViapiTool.startsWith(filePath, "https://") || ViapiTool.startsWith(filePath, "http://")) {
        filePath = ViapiTool.decode(filePath, "UTF-8");
        fileName = ViapiTool.match(filePath, "\\w+.(jpg|gif|png|jpeg|bmp|mov|mp4|avi)");
        if (Util.empty(fileName)) {
          fileName = ViapiTool.getNameFromUrl(filePath);
          fileName = ViapiTool.subStringAfterLast(fileName, "/");
        }

        ins = await ViapiTool.getStreamFromNet(filePath);
      } else {
        ins = await ViapiTool.getStreamFromPath(filePath);
        fileName = ViapiTool.getNameFromPath(filePath);
      }

      let ossConfig = new $OSS.Config({
        accessKeyId: viResponse.data.accessKeyId,
        accessKeySecret: viResponse.data.accessKeySecret,
        securityToken: viResponse.data.securityToken,
        type: "sts",
        endpoint: "oss-cn-shanghai.aliyuncs.com",
        regionId: "cn-shanghai",
      });
      let ossClient = new OSS(ossConfig);
      let objectName = `${accessKeyId}/${Util.getNonce()}${fileName}`;
      let uploadRequest = new $OSS.PutObjectRequest({
        bucketName: "viapi-customer-temp",
        body: ins,
        objectName: objectName,
      });
      let ossRuntime = new $OSSUtil.RuntimeOptions({});
      await ossClient.putObject(uploadRequest, ossRuntime);
      return `http://viapi-customer-temp.oss-cn-shanghai.aliyuncs.com/${objectName}`;
    } finally {
      if (!Util.isUnset(ins)) {
        ViapiTool.close(ins);
      }

    }
  }

}
