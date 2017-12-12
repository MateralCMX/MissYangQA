/// <reference path="../jquery.d.ts" />
/// <reference path="../base.ts" />
'use strict';
namespace MissYangQA {
    class UserDetailsPage {
        /*页面数据*/
        private static PageData = {
            params: MTMa.GetURLParams(),
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
            MDMa.AddEvent("InputOldPassword", "invalid", function (e: Event) {
                let setting: InvalidOptionsModel = new InvalidOptionsModel();
                setting.Required = "旧密码不能为空";
                common.InputInvalidEvent_Invalid(e, setting);
            });
            MDMa.AddEvent("InputNewPassword", "invalid", function (e: Event) {
                let setting: InvalidOptionsModel = new InvalidOptionsModel();
                setting.Required = "请输入新密码";
                common.InputInvalidEvent_Invalid(e, setting);
            });
            MDMa.AddEvent("InputNewPassword2", "invalid", function (e: Event) {
                let setting: InvalidOptionsModel = new InvalidOptionsModel();
                setting.Required = "请再次输入新密码";
                common.InputInvalidEvent_Invalid(e, setting);
            });
            MDMa.AddEvent("BtnSave", "click", this.BtnSaveEvent_Click);
        }
        /**
         * 绑定模式
         */
        private BindMode() {
            let BtnSave = MDMa.$("BtnSave") as HTMLButtonElement;
            if (MTMa.IsNullOrUndefinedOrEmpty(UserDetailsPage.PageData.params["ID"])) {
                window.history.back();
            }
        }
        /**
         * 保存按钮事件
         * @param e
         */
        private BtnSaveEvent_Click(e: MouseEvent) {
            common.ClearErrorMessage();
            let data = UserDetailsPage.GetInputData();
            if (!MTMa.IsNullOrUndefined(data)) {
                let url = "api/User/ChangePassword"
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
        /**
         * 获得输入数据
         */
        private static GetInputData(): Object {
            let data = null;
            if (document.forms["InputForm"].checkValidity()) {
                let InputNewPassword = (MDMa.$("InputNewPassword") as HTMLInputElement);
                let InputNewPassword2 = (MDMa.$("InputNewPassword2") as HTMLInputElement);
                data = {
                    ID: UserDetailsPage.PageData.params["ID"],
                    OldPassword: (MDMa.$("InputOldPassword") as HTMLInputElement).value,
                    NewPassword: InputNewPassword.value,
                    NewPassword2: InputNewPassword2.value,
                }
                if (data["NewPassword"] != data["NewPassword2"]) {
                    common.SetInputErrorMessage(InputNewPassword, "两次输入的密码不一样");
                    common.SetInputErrorMessage(InputNewPassword2, "两次输入的密码不一样");
                    data = null;
                }
            }
            return data;
        }
    }
    /*页面加载完毕事件*/
    MDMa.AddEvent(window, "load", function (e: Event) {
        let pageM: UserDetailsPage = new UserDetailsPage();
    });
}