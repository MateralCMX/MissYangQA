/// <reference path="../jquery.d.ts" />
/// <reference path="../base.ts" />
'use strict';
namespace MissYangQA {
    class LoginPage {
        /**
         * 构造方法
         */
        constructor() {
            this.BindEvent();
        }
        /**
         * 绑定事件
         */
        private BindEvent() {
            MDMa.AddEvent("BtnLogin", "click", this.BtnLoginEvent_Click);
            MDMa.AddEvent("BtnShowPassword", "click", common.BtnShowPasswordEvent_Click);
            MDMa.AddEvent("InputUserName", "invalid", function (e: Event) {
                let setting: InvalidOptionsModel = new InvalidOptionsModel();
                setting.Required = "用户名不能为空";
                common.InputInvalidEvent_Invalid(e, setting);
            });
            MDMa.AddEvent("InputPassword", "invalid", function (e: Event) {
                let setting: InvalidOptionsModel = new InvalidOptionsModel();
                setting.Required = "密码不能为空";
                common.InputInvalidEvent_Invalid(e, setting);
            });
        }
        /**
         * 登录按钮单击事件
         * @param e
         */
        private BtnLoginEvent_Click(e: MouseEvent) {
            common.ClearErrorMessage();
            let data = LoginPage.GetInputData();
            if (!MTMa.IsNullOrUndefined(data)) {
                let BtnElement = e.target as HTMLButtonElement;
                BtnElement.textContent = "登录中......";
                BtnElement.disabled = true;
                let url: string = "api/User/Login";
                let SFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                    common.SaveLoginUserInfo(resM["Data"]);
                    let params = MTMa.GetURLParams();
                    if (MTMa.IsNullOrUndefinedOrEmpty(params["from"])) {
                        common.GoToPage("Index");
                    }
                    else {
                        params["from"] = decodeURIComponent(params["from"]);
                        while (params["from"][0] == '/') {
                            params["from"] = (params["from"] as string).substr(1);
                        }
                        window.location.href = common.ApplicationSettingM.DomainName + params["from"];
                    }
                };
                let FFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                    common.ShowMessageBox(resM["Message"])
                };
                let CFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                    BtnElement.textContent = "登录";
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
                data = {
                    UserName: (MDMa.$("InputUserName") as HTMLInputElement).value,
                    Password: (MDMa.$("InputPassword") as HTMLInputElement).value,
                    IsEncrypted: false
                }
            }
            return data;
        }
    }
    MDMa.AddEvent(window, "load", function (e: Event) {
        const pageM: LoginPage = new LoginPage();
    });
}