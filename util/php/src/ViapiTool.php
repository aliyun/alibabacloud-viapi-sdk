<?php

namespace AlibabaCloud\SDK\ViapiTool;

use GuzzleHttp\Psr7\Stream;

/**
 * For viapi.
 */
class ViapiTool
{
    /**
     * Judge whether origin starts with sub, if yes, return true, or return false.
     *
     * @param string $origin the original string
     * @param string $sub    the prefix string
     *
     * @throws \Exception
     *
     * @return bool judge result
     */
    public static function startsWith($origin, $sub)
    {
        return false !== strpos($origin, $sub);
    }

    /**
     * If whether origin contains regrex, return string, or return ''.
     *
     * @param string $origin the original string
     * @param string $regrex the regular expression
     *
     * @throws \Exception
     *
     * @return string matched string
     */
    public static function match($origin, $regrex)
    {
        return preg_match("/^{$regrex}$/", $origin) ? $origin : '';
    }

    /**
     * Decode origin with decodeType.
     *
     * @param string $origin     the original string
     * @param string $decodeType decode type
     *
     * @throws \Exception
     *
     * @return string decoded string
     */
    public static function decode($origin, $decodeType)
    {
        $text = rawurldecode($origin);

        return iconv(mb_detect_encoding($text, mb_detect_order(), true), strtoupper($decodeType), $text);
    }

    /**
     * Get file name from url.
     *
     * @param string $urlStr the url
     *
     * @throws \Exception
     *
     * @return string file name
     */
    public static function getNameFromUrl($urlStr)
    {
        return parse_url($urlStr, PHP_URL_PATH);
    }

    /**
     * Get file name from  path.
     *
     * @param string $pathStr the path
     *
     * @throws \Exception
     *
     * @return string file name
     */
    public static function getNameFromPath($pathStr)
    {
        return basename($pathStr);
    }

    /**
     * Intercepts the last string according to sub.
     *
     * @param string $path the original string
     * @param string $sub  the cutting string
     *
     * @throws \Exception
     *
     * @return string last string
     */
    public static function subStringAfterLast($path, $sub)
    {
        $tmp = explode($sub, $path);

        return $tmp[\count($tmp) - 1];
    }

    /**
     * Get stream from net.
     *
     * @param string $url the file url
     *
     * @throws \Exception
     *
     * @return Stream stream
     */
    public static function getStreamFromNet($url)
    {
        return new Stream(fopen('data://text/plain;base64,' . base64_encode(\file_get_contents($url)), 'r+'));
    }

    /**
     * Get stream from path.
     *
     * @param string $filepath the file path
     *
     * @throws \Exception
     *
     * @return Stream stream
     */
    public static function getStreamFromPath($filepath)
    {
        return new Stream(fopen($filepath, 'r+'));
    }

    /**
     * Close the body.
     *
     * @param Stream $body the stream that needs to be closed
     *
     * @throws \Exception
     */
    public static function close($body)
    {
        $body->close();
    }
}
