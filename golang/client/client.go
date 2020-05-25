// This file is auto-generated, don't edit it. Thanks.
package client

import (
	"io"

	viapiutils20200401 "github.com/alibabacloud-go/Viapiutils-20200401/client"
	oss "github.com/alibabacloud-go/tea-oss-sdk/client"
	ossutil "github.com/alibabacloud-go/tea-oss-utils/service"
	rpc "github.com/alibabacloud-go/tea-rpc/client"
	util "github.com/alibabacloud-go/tea-utils/service"
	"github.com/alibabacloud-go/tea/tea"
	viapitool "github.com/alibabacloud-go/viapi-tool/client"
)

func Upload(accessKeyId *string, accessKeySecret *string, filePath *string) (_result *string, _err error) {
	var ins io.Reader
	defer func() {
		if !tea.BoolValue(util.IsUnset(ins)) {
			viapitool.Close(ins)
		}

	}()
	tryRes, tryErr := func() (*string, error) {
		viConfig := &rpc.Config{
			AccessKeyId:     accessKeyId,
			AccessKeySecret: accessKeySecret,
			Type:            tea.String("access_key"),
			Endpoint:        tea.String("viapiutils.cn-shanghai.aliyuncs.com"),
			RegionId:        tea.String("cn-shanghai"),
		}
		viclient, _err := viapiutils20200401.NewClient(viConfig)
		if _err != nil {
			return _result, _err
		}

		viRequest := &viapiutils20200401.GetOssStsTokenRequest{}
		viResponse, _err := viclient.GetOssStsToken(viRequest)
		if _err != nil {
			return _result, _err
		}

		if tea.BoolValue(util.IsUnset(tea.ToMap(viResponse))) || tea.BoolValue(util.IsUnset(tea.ToMap(viResponse.Data))) {
			_err = tea.NewSDKError(map[string]interface{}{
				"code":    "InvalidResponse",
				"message": "GetOssStsToken gets a invalid response",
				"data":    viResponse,
			})
			return _result, _err
		}

		fileName := tea.String("")
		if tea.BoolValue(viapitool.StartsWith(filePath, tea.String("https://"))) || tea.BoolValue(viapitool.StartsWith(filePath, tea.String("http://"))) {
			filePath = viapitool.Decode(filePath, tea.String("UTF-8"))
			fileName = viapitool.Match(filePath, tea.String("\\w+.(jpg|gif|png|jpeg|bmp|mov|mp4|avi)"))
			if tea.BoolValue(util.Empty(fileName)) {
				fileName = viapitool.GetNameFromUrl(filePath)
				fileName = viapitool.SubStringAfterLast(fileName, tea.String("/"))
			}

			ins, _err = viapitool.GetStreamFromNet(filePath)
			if _err != nil {
				return _result, _err
			}

		} else {
			ins, _err = viapitool.GetStreamFromPath(filePath)
			if _err != nil {
				return _result, _err
			}

			fileName = viapitool.GetNameFromPath(filePath)
		}

		ossConfig := &oss.Config{
			AccessKeyId:     viResponse.Data.AccessKeyId,
			AccessKeySecret: viResponse.Data.AccessKeySecret,
			SecurityToken:   viResponse.Data.SecurityToken,
			Type:            tea.String("sts"),
			Endpoint:        tea.String("oss-cn-shanghai.aliyuncs.com"),
			RegionId:        tea.String("cn-shanghai"),
		}
		ossClient, _err := oss.NewClient(ossConfig)
		if _err != nil {
			return _result, _err
		}

		objectName := tea.String(tea.StringValue(accessKeyId) + "/" + tea.StringValue(util.GetNonce()) + tea.StringValue(fileName))
		uploadRequest := &oss.PutObjectRequest{
			BucketName: tea.String("viapi-customer-temp"),
			Body:       ins,
			ObjectName: objectName,
		}
		ossRuntime := &ossutil.RuntimeOptions{}
		_, _err = ossClient.PutObject(uploadRequest, ossRuntime)
		if _err != nil {
			return _result, _err
		}
		_result = tea.String("http://viapi-customer-temp.oss-cn-shanghai.aliyuncs.com/" + tea.StringValue(objectName))
		return _result, _err
	}()
	_result = tryRes
	_err = tryErr
	if _err != nil {
		return _result, _err
	}
	return _result, _err
}
