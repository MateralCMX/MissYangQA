/// <reference path="../lib/m-tools/m-tools.ts" />
'use strict';
import MDMa = MateralTools.DOMManager;
import MTMa = MateralTools.ToolManager;
import MLDMa = MateralTools.LocalDataManager;
namespace MissYangQA {
    class Common {
        /**
         * 构造方法
         */
        constructor() {
            ApplicationSettingModel.IsDebug = true;
            this.AddMessageBoxArticle();
            this.BindFooterInfo();
            this.BindBackBtn();
        }
        /*应用程序配置对象*/
        public ApplicationSettingM: ApplicationSettingModel = new ApplicationSettingModel();
        /**
         * 保存登录用户信息
         * @param data
         */
        public SaveLoginUserInfo(data): void {
            if (data != null) {
                let saveData: LoginUserModel = new LoginUserModel(data["ID"], data["Token"]);
                MLDMa.SetLocalData(this.ApplicationSettingM.SaveUserKey, saveData, true);
            }
            else {
                this.RemoveLoginUserInfo();
            }
        }
        /**
         * 移除登录用户信息
         */
        public RemoveLoginUserInfo(): void {
            MLDMa.RemoveLocalData(this.ApplicationSettingM.SaveUserKey);
        }
        /**
         * 获得登录用户信息
         */
        public GetLoginUserInfo(): LoginUserModel {
            return MLDMa.GetLocalData(this.ApplicationSettingM.SaveUserKey, true) as LoginUserModel;
        }
        /**
         * 显示密码按钮单击事件
         * @param e
         */
        public BtnShowPasswordEvent_Click(e: MouseEvent) {
            let btnElement = e.target as HTMLButtonElement;
            let targetID = btnElement.dataset.target;
            if (!MTMa.IsNullOrUndefinedOrEmpty(targetID)) {
                let inputElement = MDMa.$(targetID) as HTMLInputElement;
                if (!MTMa.IsNullOrUndefined(inputElement)) {
                    if (inputElement.type == "text") {
                        inputElement.type = "password";
                        btnElement.classList.remove("glyphicon-eye-close");
                        btnElement.classList.add("glyphicon-eye-open");
                    }
                    else {
                        inputElement.type = "text";
                        btnElement.classList.remove("glyphicon-eye-open");
                        btnElement.classList.add("glyphicon-eye-close");
                    }
                }
            }
        }
        /**
         * 输入框验证事件
         * @param e 事件对象
         * @param setting 错误提示设置对象
         */
        public InputInvalidEvent_Invalid(e: Event, setting: InvalidOptionsModel) {
            let options: InvalidOptionsModel = MTMa.IsNullOrUndefined(setting) ? new InvalidOptionsModel() : setting;
            let inputElement = e.target as HTMLInputElement;
            if (inputElement.validity.valueMissing) {
                common.SetInputErrorMessage(inputElement, options.Required);
            }
            else if (inputElement.validity.patternMismatch) {
                common.SetInputErrorMessage(inputElement, options.Pattern);
            }
            else if (inputElement.validity.rangeUnderflow) {
                common.SetInputErrorMessage(inputElement, options.Min);
            }
            else {
                common.SetInputErrorMessage(inputElement, options.Defult);
            }
        }
        /**
         * 设置错误信息
         * @param inputElement 输入元素
         * @param message 错误信息
         */
        public SetInputErrorMessage(inputElement: HTMLElement, message: string) {
            let formGroupElement = inputElement.parentElement as HTMLElement;
            while (!MDMa.HasClass(formGroupElement, "form-group")) {
                formGroupElement = formGroupElement.parentElement;
            }
            formGroupElement.classList.add("has-error");
            let messageSpan = formGroupElement.lastElementChild;
            if (MDMa.HasClass(messageSpan, "help-block")) {
                messageSpan.textContent = message;
            }
        }
        /**
         * 清空错误信息
         */
        public ClearErrorMessage() {
            let ErrorMessages = document.getElementsByClassName("has-error");
            for (let i = ErrorMessages.length - 1; i >= 0; i--) {
                let messageSpan = ErrorMessages[i].lastElementChild;
                if (MDMa.HasClass(messageSpan, "help-block")) {
                    messageSpan.textContent = "";
                }
                ErrorMessages[i].classList.remove("has-error");
            }
        }
        /**
         * 跳转页面
         * @param act 标识
         */
        public GoToPage(act: string, params: string[] = null, isTop: boolean = false) {
            let url = this.ApplicationSettingM.DomainName;
            switch (act) {
                case "Login":
                    url += "Home/Login";
                    break;
                case "Index":
                    url += "Home/Index";
                    break;
                default:
                    url += "Home/Login";
                    break;
            }
            if (!MTMa.IsNullOrUndefined(params) && params.length > 0) {
                url += "?" + params.join("&");
            }
            window.location.href = url;
        }
        /**
         * 发送GetAjax请求
         * @param url 请求地址
         * @param data 请求参数
         * @param SFun 成功方法
         * @param SFun 失败方法
         * @param CFun 都执行方法
         * @param EFun 错误方法
         */
        public SendGetAjax(url: string, data: Object, SFun: Function, FFun: Function, CFun: Function, EFun: Function = null) {
            this.SendAjax(url, data, SFun, FFun, CFun, EFun, "get", "json");
        }
        /**
         * 发送PostAjax请求
         * @param url 请求地址
         * @param data 请求参数
         * @param SFun 成功方法
         * @param SFun 失败方法
         * @param CFun 都执行方法
         * @param EFun 错误方法
         * @param dataType 数据类型
         */
        public SendPostAjax(url: string, data: Object, SFun: Function, FFun: Function, CFun: Function, EFun: Function = null, dataType: string = "json") {
            this.SendAjax(url, data, SFun, FFun, CFun, EFun, "post", dataType);
        }
        /**
         * 发送Ajax请求
         * @param url 请求地址
         * @param data 请求参数
         * @param SFun 成功方法
         * @param SFun 失败方法
         * @param CFun 都执行方法
         * @param EFun 错误方法
         * @param type 请求类型
         * @param dataType 数据类型
         */
        public SendAjax(url: string, data: Object, SFun: Function, FFun: Function, CFun: Function, EFun: Function = null, type: string = "post", dataType: string = "json") {
            url = this.ApplicationSettingM.DomainName + url;
            let LoginUserM: LoginUserModel = this.GetLoginUserInfo();
            if (LoginUserM != null) {
                data["LoginUserID"] = LoginUserM.ID;
                data["LoginUserToken"] = LoginUserM.Token;
            }
            if (EFun == null) {
                EFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                    common.ShowMessageBox(resM["Message"] == null ? resM["Message"] : "应用程序出错了");
                }
            }
            let SuccessFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                if (resM["ResultType"] == 0) {
                    SFun(resM, xhr, state);
                }
                else if (resM["ResultType"] == 1) {
                    FFun(resM, xhr, state);
                }
                else {
                    EFun(resM, xhr, state);
                }
            }
            let ErrorFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                if (xhr.status == 401) {
                    this.GoToPage("Login", null, true);
                }
                if (EFun) {
                    EFun(resM, xhr, state);
                }
            }
            let config = new MateralTools.HttpConfigModel(url, type, data, dataType, SuccessFun, ErrorFun, CFun);
            MateralTools.HttpManager.Send(config);
        }
        /**
         * 显示消息窗体
         * @param message 消息
         * @param title 标题
         */
        public ShowMessageBox(message: string, title: string = "提示"): void {
            let MessageBoxTitle = MDMa.$("MessageBoxTitle") as HTMLDivElement;
            let MessageBoxBody = MDMa.$("MessageBoxBody") as HTMLDivElement;
            if (!MTMa.IsNullOrUndefined(MessageBoxTitle)) {
                MessageBoxTitle.textContent = title;
            }
            if (!MTMa.IsNullOrUndefined(MessageBoxBody)) {
                MessageBoxBody.textContent = message;
            }
            $('#MessageBox').modal('toggle');
        }
        /**
         * 添加信息提示窗体
         */
        private AddMessageBoxArticle() {
            let element = MDMa.$("MessageBox") as HTMLElement;
            if (MTMa.IsNullOrUndefined(element)) {
                element = document.createElement("article") as HTMLElement;
                element.id = "MessageBox";
                document.body.appendChild(element);
            }
            else {
                element.innerHTML = "";
            }
            MDMa.AddClass(element, "modal fade");
            element.setAttribute("tabindex", "-1");
            element.setAttribute("role", "dialog");
            element.setAttribute("aria-labelledby", "MessageBoxLabel");
            let modalDialog = document.createElement("div");
            MDMa.AddClass(modalDialog, "modal-dialog");
            modalDialog.setAttribute("role", "document");
            element.appendChild(modalDialog);
            let modalContent = document.createElement("div");
            MDMa.AddClass(modalContent, "modal-content");
            modalDialog.appendChild(modalContent);
            let modalHeader = document.createElement("div");
            MDMa.AddClass(modalHeader, "modal-header");
            modalContent.appendChild(modalHeader);
            let btnClose = document.createElement("button");
            MDMa.AddClass(btnClose, "close");
            btnClose.type = "button";
            btnClose.dataset.dismiss = "modal";
            btnClose.setAttribute("aria-label", "Close");
            let btnCloseSpan = document.createElement("span");
            btnCloseSpan.setAttribute("aria-hidden", "true");
            btnCloseSpan.innerHTML = "&times;";
            btnClose.appendChild(btnCloseSpan);
            modalHeader.appendChild(btnClose);
            let modalTitle = document.createElement("h4");
            MDMa.AddClass(modalTitle, "modal-title");
            modalTitle.id = "MessageBoxTitle";
            modalTitle.innerHTML = "提示";
            modalHeader.appendChild(modalTitle);
            let modalBody = document.createElement("div");
            MDMa.AddClass(modalBody, "modal-body");
            modalBody.id = "MessageBoxBody";
            modalContent.appendChild(modalBody);
            let modalFooter = document.createElement("div");
            MDMa.AddClass(modalFooter, "modal-footer");
            modalContent.appendChild(modalFooter);
            let btnOK = document.createElement("button");
            btnOK.type = "button";
            MDMa.AddClass(btnOK, "btn btn-default");
            btnOK.dataset.dismiss = "modal";
            btnOK.innerHTML = "确定";
            modalFooter.appendChild(btnOK);
            
        }
        /**
         * 绑定标准页面脚部
         */
        private BindFooterInfo() {
            let footers = document.getElementsByClassName("DefultFooter");
            for (var i = 0; i < footers.length; i++) {
                let ContentP = document.createElement("p");
                ContentP.textContent = "欢迎使用：Miss Yang Q&A System";
                let KeepA = document.createElement("a");
                KeepA.href = "";
                KeepA.target = "_blank";
                KeepA.textContent = "";
                let CopyrightP = document.createElement("p");
                CopyrightP.textContent = "技术支持：小杨老师家的猴子 ©2017 Materal";
                footers[i].appendChild(ContentP);
                footers[i].appendChild(CopyrightP);
            }
        }
        /**
         * 绑定后退按钮
         */
        private BindBackBtn() {
            let BtnBacks = document.getElementsByClassName("btn-back");
            for (var i = 0; i < BtnBacks.length; i++) {
                MDMa.AddEvent(BtnBacks[i], "click", function (e: MouseEvent) {
                    window.history.back();
                });
            }
        }
        /**
         * 检测登录
         * @param gotoLogin 没登录是否跳转登录
         */
        public IsLogin(gotoLogin: boolean = false) {
            let userInfo = common.GetLoginUserInfo();
            if (MTMa.IsNullOrUndefined(userInfo)) {
                if (gotoLogin) {
                    common.GoToPage("Login", ["from=" + encodeURIComponent(window.location.pathname + window.location.search)], true);
                    return false;
                }
                else {
                    return false;
                }
            }
            else {
                return true;
            }
        }
        /**
         * 设置标题
         * @param headerTitle 头部标题
         * @param pageTitle 页面标题
         */
        public SetTitle(headerTitle: string, pageTitle: string = null) {
            if (MTMa.IsNullOrUndefinedOrEmpty(pageTitle)) {
                pageTitle = headerTitle;
            }
            let HeaderTitle = MDMa.$("HeaderTitle") as HTMLElement;
            if (!MTMa.IsNullOrUndefined(HeaderTitle)) {
                HeaderTitle.textContent = headerTitle;
            }
            let PageTitle = MDMa.$("PageTitle") as HTMLElement;
            if (!MTMa.IsNullOrUndefined(PageTitle)) {
                PageTitle.textContent = pageTitle;
            }
        }
        /**
         * 获取滚动条位置
         */
        public GetScrollTop() {
            var scrollTop = 0;
            if (document.documentElement && document.documentElement.scrollTop) {
                scrollTop = document.documentElement.scrollTop;
            }
            else if (document.body) {
                scrollTop = document.body.scrollTop;
            }
            return scrollTop;
        }
        /**
         * 获取可见高度
         */
        public GetClientHeight() {
            var clientHeight = 0;
            if (document.body.clientHeight && document.documentElement.clientHeight) {
                var clientHeight = (document.body.clientHeight < document.documentElement.clientHeight) ? document.body.clientHeight : document.documentElement.clientHeight;
            }
            else {
                var clientHeight = (document.body.clientHeight > document.documentElement.clientHeight) ? document.body.clientHeight : document.documentElement.clientHeight;
            }
            return clientHeight;
        }
    }
    /**
     * 应用程序设置模型
     */
    class ApplicationSettingModel {
        /*调试模型*/
        public static IsDebug: boolean = false;
        /*域名*/
        private _domainName: string = "http://localhost:49420/";
        /*调试域名*/
        private _deBugDomainName: string = "http://localhost:49420/";
        /*保存用户的值*/
        public SaveUserKey: string = "LOGINUSERKEY";
        /*服务器域名*/
        public get DomainName() {
            if (!ApplicationSettingModel.IsDebug) {
                return this._domainName;
            }
            else {
                return this._deBugDomainName;
            }
        }
    }
    /**
     * 验证模型
     */
    export class InvalidOptionsModel {
        /*必填*/
        public Required: string = "输入不能为空";
        /*正则*/
        public Pattern: string = "输入未能通过验证";
        /*过小*/
        public Min: string = "输入过小";
        /*过大*/
        public Max: string = "输入过大";
        /*默认*/
        public Defult: string = "输入有误";
    }
    /**
     * 登录用户模型
     */
    export class LoginUserModel {
        /*用户ID*/
        public ID: string;
        /*用户Token*/
        public Token: string;
        /**
         * 构造函数
         * @param id 用户唯一标识
         * @param token 用户Token
         */
        constructor(id: string, token: string) {
            this.ID = id;
            this.Token = token;
        }
    }
    /**
     * 公共处理对象
     */
    export const common: Common = new Common();
}