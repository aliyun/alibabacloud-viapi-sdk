## Install

```xml
<dependency>
  <groupId>com.aliyun</groupId>
  <artifactId>viapi-utils</artifactId>
  <version>1.0.0</version>
</dependency>
<dependency>
  <groupId>com.aliyun</groupId>
  <artifactId>aliyun-java-sdk-viapiutils</artifactId>
  <version>1.0.0</version>
</dependency>

```

间接依赖，按需添加

```xml

    <dependency>
      <groupId>com.aliyun</groupId>
      <artifactId>aliyun-java-sdk-core</artifactId>
      <version>4.4.8</version>
    </dependency>
    <dependency>
      <groupId>com.aliyun.oss</groupId>
      <artifactId>aliyun-sdk-oss</artifactId>
      <version>3.4.2</version>
    </dependency>

    <dependency>
      <groupId>org.apache.commons</groupId>
      <artifactId>commons-lang3</artifactId>
      <version>3.9</version>
    </dependency>
    
```

## Usage
```java
public static void testUploadFile() throws ClientException, IOException {
        String accessKey = "xxx";
        String accessKeySecret = "xxx";
        //String file = /home/admin/file/1.jpg  或者本地上传
        String file = "https://fuss10.elemecdn.com/5/32/c17416d77817f2507d7fbdf15ef22jpeg.jpeg";
        FileUtils fileUtils = FileUtils.getInstance(accessKey,accessKeySecret);
        String ossurl = fileUtils.upload(file);
        System.out.println(ossurl);
    }
```