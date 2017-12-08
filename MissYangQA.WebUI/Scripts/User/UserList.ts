/// <reference path="../jquery.d.ts" />
/// <reference path="../base.ts" />
'use strict';
namespace MissYangQA {
    class UserListModel {
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
                UserListModel.GetList();
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
                if (contentH - viewH - scrollTop <= 100 && !UserListModel.PageSetting.IsLoading && UserListModel.PageSetting.OldScrollTop < scrollTop) {
                    UserListModel.GetList();
                }
                UserListModel.PageSetting.OldScrollTop = scrollTop;
            });
        }
        /**
         * 获得列表信息
         */
        private static GetList() {
            if (UserListModel.PageSetting.PageCount >= UserListModel.PageSetting.PageIndex) {
                UserListModel.PageSetting.IsLoading = true;
                let url: string = "api/User/GetAllUserInfo";
                let data = {
                    PageIndex: UserListModel.PageSetting.PageIndex,
                    PageSize: UserListModel.PageSetting.PageSize,
                };
                let SFun = function (resM: Object) {
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
                let FFun = function (xhr: XMLHttpRequest, resM: Object) {
                    common.ShowMessageBox(resM["Message"])
                };
                let CFun = function (resM: Object) {
                };
                common.SendGetAjax(url, data, SFun, FFun, CFun);
            }
        }
    }
    /*页面加载完毕事件*/
    MDMa.AddEvent(window, "load", function (e: Event) {
        let pageM: UserListModel = new UserListModel();
    });
}