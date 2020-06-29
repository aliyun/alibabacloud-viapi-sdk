// This file is auto-generated, don't edit it. Thanks.
/**
 * For viapi
 */
package client

import (
	"fmt"
	"io"
	"io/ioutil"
	"net/http"
	"net/url"
	"os"
	"regexp"
	"strings"

	"github.com/alibabacloud-go/tea/tea"
)

/**
 * Judge whether origin starts with sub, if yes, return true, or return false
 * @param origin, sub
 * @return the judge result
 */
func StartsWith(origin *string, sub *string) (_result *bool) {
	return tea.Bool(strings.HasPrefix(tea.StringValue(origin), tea.StringValue(sub)))
}

/**
 * If whether origin contains regrex, return string, or return ''
 * @param origin, regrex
 * @return string
 */
func Match(origin *string, regrex *string) (_result *string) {
	flysnowRegexp := regexp.MustCompile(tea.StringValue(regrex))
	params := flysnowRegexp.FindStringSubmatch(tea.StringValue(origin))
	if len(params) > 0 {
		return tea.String(params[0])
	} else {
		return tea.String("")
	}
}

func Decode(origin *string, decodeType *string) (_result *string) {
	res, _ := url.QueryUnescape(tea.StringValue(origin))
	return tea.String(res)
}

func GetNameFromUrl(urlStr *string) (_result *string) {
	u, err := url.Parse(tea.StringValue(urlStr))
	if err != nil {
		return tea.String("")
	}
	return tea.String(u.Path)
}

func GetNameFromPath(pathStr *string) (_result *string) {
	fs, err := os.Stat(tea.StringValue(pathStr))
	if err != nil {
		return tea.String("")
	}
	return tea.String(fs.Name())
}

func SubStringAfterLast(path *string, sub *string) (_result *string) {
	strs := strings.Split(tea.StringValue(path), tea.StringValue(sub))
	return tea.String(strs[len(strs)-1])
}

func GetStreamFromNet(filepath *string) (_result io.Reader, _err error) {
	resp, _err := http.Get(tea.StringValue(filepath))
	if _err != nil {
		return nil, _err
	}

	if resp.StatusCode < 200 || resp.StatusCode > 300 {
		byt, _err := ioutil.ReadAll(resp.Body)
		if _err != nil {
			return nil, _err
		}
		return nil, fmt.Errorf("Get %s failed, message: %s", tea.StringValue(filepath), string(byt))
	}

	return resp.Body, nil
}

func GetStreamFromPath(filepath *string) (_result io.Reader, _err error) {
	_result, _err = os.Open(tea.StringValue(filepath))
	return _result, _err
}

func Close(body io.Reader) {
	real, ok := body.(io.ReadCloser)
	if ok {
		real.Close()
	}
}
