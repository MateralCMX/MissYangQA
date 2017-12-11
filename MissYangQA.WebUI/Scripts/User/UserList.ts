/// <reference path="../jquery.d.ts" />
/// <reference path="../base.ts" />
'use strict';
namespace MissYangQA {
    class UserListPage {
        /*页面设置*/
        private static PageSetting = {
            IsLoading: false,
            OldScrollTop: 0,
            PageIndex: 1,
            PageSize: 10,
            PageCount: 99
        }
        /**
         * 构造方法
         */
        constructor() {
            if (common.IsLogin(true)) {
                this.BindEvent();
                UserListPage.GetList();
            }
        }
        /**
         * 绑定事件
         */
        private BindEvent() {
            /*下拉加载*/
            MDMa.AddEvent(window, "scroll", function (e) {
                let viewH = common.GetClientHeight();//可见高度
                let contentH = Math.max(document.body.scrollHeight, document.documentElement.scrollHeight);//内容高度
                let scrollTop = common.GetScrollTop();//滚动条位置
                if (contentH - viewH - scrollTop <= 100 && !UserListPage.PageSetting.IsLoading && UserListPage.PageSetting.OldScrollTop < scrollTop) {
                    UserListPage.GetList();
                }
                UserListPage.PageSetting.OldScrollTop = scrollTop;
            });
        }
        /**
         * 获得列表信息
         */
        private static GetList() {
            if (UserListPage.PageSetting.PageCount >= UserListPage.PageSetting.PageIndex) {
                UserListPage.PageSetting.IsLoading = true;
                let url: string = "api/User/GetAllUserInfo";
                let data = {
                    PageIndex: UserListPage.PageSetting.PageIndex,
                    PageSize: UserListPage.PageSetting.PageSize,
                };
                let SFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                    let MainList = MDMa.$("MainList");
                    if (!MTMa.IsNullOrUndefined(MainList)) {
                        MainList.innerHTML = "";
                        for (var i = 0; i < resM["Data"].length; i++) {
                            let ListItem = document.createElement("a");
                            ListItem.href = "/User/UserDetailed?ID=" + resM["Data"][i]["ID"];
                            ListItem.classList.add("list-group-item");
                            let TextContent = document.createTextNode(resM["Data"][i]["UserName"]);
                            let RightIco = document.createElement("i");
                            MDMa.AddClass(RightIco, "list-right glyphicon glyphicon-chevron-right");
                            ListItem.appendChild(TextContent);
                            ListItem.appendChild(RightIco);
                            MainList.appendChild(ListItem);
                        }
                    }
                };
                let FFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                    common.ShowMessageBox(resM["Message"])
                };
                let CFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                };
                common.SendGetAjax(url, data, SFun, FFun, CFun);
            }
        }
    }
    /*页面加载完毕事件*/
    MDMa.AddEvent(window, "load", function (e: Event) {
        let pageM: UserListPage = new UserListPage();
    });
}