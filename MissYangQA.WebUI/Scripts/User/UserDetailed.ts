/// <reference path="../jquery.d.ts" />
/// <reference path="../base.ts" />
'use strict';
namespace MissYangQA {
    class UserDetailedPage {
        /*页面数据*/
        private static PageData = {
            params: MTMa.GetURLParams(),
            url: ""
        }
        /*页面设置*/
        private static PageSetting = {
            UpdatePasswordState: false
        }
        /**
         * 构造方法
         */
        constructor() {
            if (common.IsLogin(true)) {
                this.BindEvent();
                this.BindMode();
            }
        }
        /**
         * 绑定事件
         */
        private BindEvent() {
            MDMa.AddEvent("InputUserName", "invalid", function (e: Event) {
                let setting: InvalidOptionsModel = new InvalidOptionsModel();
                setting.Required = "用户名不能为空";
                common.InputInvalidEvent_Invalid(e, setting);
            });
            MDMa.AddEvent("BtnSave", "click", this.BtnSaveEvent_Click);
            MDMa.AddEvent("BtnDelete", "click", this.BtnDeleteEvent_Click)
        }
        /**
         * 绑定模式
         */
        private BindMode() {
            let BtnSave = MDMa.$("BtnSave") as HTMLButtonElement;
            if (!MTMa.IsNullOrUndefinedOrEmpty(UserDetailedPage.PageData.params["ID"])) {
                this.GetUserViewInfoByID();
                UserDetailedPage.PageData.url = "api/User/EditUserInfo";
                BtnSave.classList.add("glyphicon-floppy-disk");
                let TopTools = BtnSave.parentElement;
                let changePasswordA = document.createElement("a");
                MDMa.AddClass(changePasswordA, "btn btn-warning glyphicon glyphicon-pencil");
                changePasswordA.href = "/User/ChangePassword?ID=" + UserDetailedPage.PageData.params["ID"];
                TopTools.appendChild(changePasswordA);
                let deleteUserBtn = document.createElement("button");
                MDMa.AddClass(deleteUserBtn, "btn btn-danger glyphicon glyphicon-remove");
                deleteUserBtn.type = "button";
                deleteUserBtn.dataset.toggle = "modal";
                deleteUserBtn.dataset.target = "#DeleteModal";
                TopTools.appendChild(deleteUserBtn);
                common.SetTitle("修改用户");
            }
            else {
                UserDetailedPage.PageData.url = "api/User/AddUserInfo";
                BtnSave.classList.add("glyphicon-plus");
                common.SetTitle("添加用户");
            }
        }
        /**
         * 获得对象信息
         */
        private GetUserViewInfoByID() {
            let url: string = "api/User/GetUserViewInfoByID";
            let data = {
                ID: UserDetailedPage.PageData.params["ID"]
            }
            let SFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                let InputUserName = MDMa.$("InputUserName") as HTMLInputElement;
                InputUserName.value = resM["Data"]["UserName"];
            };
            let FFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                common.ShowMessageBox(resM["Message"])
            };
            let CFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
            };
            common.SendGetAjax(url, data, SFun, FFun, CFun);
        }
        /**
         * 保存按钮事件
         * @param e
         */
        private BtnSaveEvent_Click(e: MouseEvent) {
            common.ClearErrorMessage();
            let data = UserDetailedPage.GetInputData();
            if (!MTMa.IsNullOrUndefined(data)) {
                let BtnElement = e.target as HTMLButtonElement;
                BtnElement.disabled = true;
                let SFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                    window.history.back();
                };
                let FFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                    common.ShowMessageBox(resM["Message"])
                };
                let CFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                    BtnElement.disabled = false
                };
                common.SendPostAjax(UserDetailedPage.PageData.url, data, SFun, FFun, CFun);
            }
        }
        /**
         * 获得输入数据
         */
        private static GetInputData(): Object {
            let data = null;
            if (document.forms["InputForm"].checkValidity()) {
                data = {
                    ID: UserDetailedPage.PageData.params["ID"],
                    UserName: (MDMa.$("InputUserName") as HTMLInputElement).value,
                }
            }
            return data;
        }
        /**
         * 删除按钮事件
         * @param e
         */
        private BtnDeleteEvent_Click(e: MouseEvent) {
            let url = "api/User/DeleteUserInfo";
            let data = {
                ID: UserDetailedPage.PageData.params["ID"]
            }
            let BtnElement = e.target as HTMLButtonElement;
            BtnElement.disabled = true;
            let SFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                window.history.back();
            };
            let FFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                common.ShowMessageBox(resM["Message"])
            };
            let CFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                BtnElement.disabled = false
            };
            common.SendPostAjax(url, data, SFun, FFun, CFun);
        }
    }
    /*页面加载完毕事件*/
    MDMa.AddEvent(window, "load", function (e: Event) {
        let pageM: UserDetailedPage = new UserDetailedPage();
    });
}