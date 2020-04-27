## Install
- pip install oss2
- pip install aliyun-python-sdk-viapiutils
- pip install aliyun-python-sdk-core
- pip install viapi-utils

## Useage
```python
from viapi.fileutils import FileUtils
file_utils = FileUtils("your own accessKey","your own accessSecret")
oss_url = file_utils.get_oss_url("http://xxx.jpeg","jpg",False)
print(oss_url)
oss_url = file_utils.get_oss_url("/home/xxx.mp4","mp4",True)
print(oss_url)
```