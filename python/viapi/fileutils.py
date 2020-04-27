from aliyunsdkviapiutils.request.v20200401 import GetOssStsTokenRequest
from aliyunsdkcore.client import AcsClient
import json
import oss2
import urllib.request as url_request
import certifi
import uuid


class FileUtils:
    def __init__(self, access_key, access_secret):
        self.access_key = access_key
        self.access_secret = access_secret
        self.client = AcsClient(access_key, access_secret, "cn-shanghai")

    def get_oss_url(self, file_path, suffix, is_local):

        request = GetOssStsTokenRequest.GetOssStsTokenRequest()
        request.set_endpoint("viapiutils.cn-shanghai.aliyuncs.com")

        response = self.client.do_action_with_exception(request)
        response_data = json.loads(response,encoding="UTF-8").get('Data')
        token = response_data.get('SecurityToken')
        ak_key = response_data.get('AccessKeyId')
        ak_sec = response_data.get('AccessKeySecret')

        auth = oss2.StsAuth(ak_key, ak_sec , token)

        bucket = oss2.Bucket(auth, 'http://oss-cn-shanghai.aliyuncs.com', 'viapi-customer-temp')
        oss_key = self.access_key + "/" + str(uuid.uuid4()) + "." + suffix
        oss_url = 'http://viapi-customer-temp.oss-cn-shanghai.aliyuncs.com/' + oss_key
        if is_local:
            with open(file_path,"rb") as image:

                bucket.put_object(oss_key, image)
                return oss_url
        else:
            with url_request.urlopen(file_path, cafile=certifi.where()) as image:
                bucket.put_object(oss_key, image.read())
                return oss_url
