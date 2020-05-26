package client

import (
	"testing"

	"github.com/alibabacloud-go/tea/tea"
	"github.com/alibabacloud-go/tea/utils"
)

func Test_StartsWith(t *testing.T) {
	ok := StartsWith(tea.String("viapi-test"), tea.String("viapi"))
	utils.AssertEqual(t, tea.BoolValue(ok), true)
}

func Test_Match(t *testing.T) {
	reg := tea.String("\\w+.(jpg|gif|png|jpeg|bmp|mov|mp4|avi)")
	str := Match(tea.String("test.avi"), reg)
	utils.AssertEqual(t, tea.StringValue(str), "test.avi")

	str = Match(tea.String("test.awm"), reg)
	utils.AssertEqual(t, tea.StringValue(str), "")
}

func Test_Decode(t *testing.T) {
	str := Decode(tea.String("viapi+test"), tea.String("UrlDecode"))
	utils.AssertEqual(t, tea.StringValue(str), "viapi test")
}

func Test_GetNameFromUrl(t *testing.T) {
	name := GetNameFromUrl(tea.String("https://viapi.com/viapi.txt"))
	utils.AssertEqual(t, tea.StringValue(name), "/viapi.txt")

	name = GetNameFromUrl(tea.String("https://viapi.com\viapitxt"))
	utils.AssertEqual(t, tea.StringValue(name), "")
}

func Test_GetNameFromPath(t *testing.T) {
	name := GetNameFromPath(tea.String("./client_test.go"))
	utils.AssertEqual(t, tea.StringValue(name), "client_test.go")

	name = GetNameFromPath(tea.String("./clienttest.go"))
	utils.AssertEqual(t, tea.StringValue(name), "")
}

func Test_SubStringAfterLast(t *testing.T) {
	name := SubStringAfterLast(tea.String("/viapi.txt"), tea.String("/"))
	utils.AssertEqual(t, tea.StringValue(name), "viapi.txt")
}

func Test_GetStreamFromNet(t *testing.T) {
	res, err := GetStreamFromNet(tea.String("http://img.qctm.com/file/a/news/191217/art61593/U9VDoz_6.jpg"))
	utils.AssertNil(t, err)
	Close(res)

	_, err = GetStreamFromNet(tea.String("http://img.qctm/a/news/191217/art61593/U9VDoz_6.jpg"))
	utils.AssertContains(t, err.Error(), "Get http://img.qctm/a/news/191217/art61593/U9VDoz_6.jpg: dial tcp:")
}

func Test_GetStreamFromPath(t *testing.T) {
	res, err := GetStreamFromPath(tea.String("./client_test.go"))
	utils.AssertNil(t, err)
	Close(res)
}
