/// <reference path="../jquery.d.ts" />
/// <reference path="../base.ts" />
'use strict';
namespace MissYangQA {
    class ClassDetailsPage {
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
            MDMa.AddEvent("InputName", "invalid", function (e: Event) {
                let setting: InvalidOptionsModel = new InvalidOptionsModel();
                setting.Required = "班级名不能为空";
                common.InputInvalidEvent_Invalid(e, setting);
            });
            MDMa.AddEvent("BtnSave", "click", this.BtnSaveEvent_Click);
            MDMa.AddEvent("BtnDelete", "click", this.BtnDeleteEvent_Click);
        }
        /**
         * 绑定模式
         */
        private BindMode() {
            let BtnSave = MDMa.$("BtnSave") as HTMLButtonElement;
            if (!MTMa.IsNullOrUndefinedOrEmpty(ClassDetailsPage.PageData.params["ID"])) {
                this.GetClassViewInfoByID();
                ClassDetailsPage.PageData.url = "api/Class/EditClassInfo";
                BtnSave.classList.add("glyphicon-floppy-disk");
                let TopTools = BtnSave.parentElement;
                let deleteClassBtn = document.createElement("button");
                MDMa.AddClass(deleteClassBtn, "btn btn-danger glyphicon glyphicon-trash");
                deleteClassBtn.type = "button";
                deleteClassBtn.dataset.toggle = "modal";
                deleteClassBtn.dataset.target = "#DeleteModal";
                TopTools.appendChild(deleteClassBtn);
                common.SetTitle("修改班级");
            }
            else {
                ClassDetailsPage.PageData.url = "api/Class/AddClassInfo";
                BtnSave.classList.add("glyphicon-plus");
                common.SetTitle("添加班级");
            }
        }
        /**
         * 获得对象信息
         */
        private GetClassViewInfoByID() {
            let url: string = "api/Class/GetClassInfoByID";
            let data = {
                ID: ClassDetailsPage.PageData.params["ID"]
            }
            let SFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                let InputName = MDMa.$("InputName") as HTMLInputElement;
                InputName.value = resM["Data"]["Name"];
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
            let data = ClassDetailsPage.GetInputData();
            if (!MTMa.IsNullOrUndefined(data)) {
                let BtnElement = e.target as HTMLButtonElement;
                BtnElement.disabled = true;
                let SFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                    if (resM["ResultType"] == 0) {
                        window.history.back();
                    }
                    else {
                        common.ShowMessageBox(resM["Message"])
                    }
                };
                let FFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                };
                let CFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                    BtnElement.disabled = false
                };
                common.SendPostAjax(ClassDetailsPage.PageData.url, data, SFun, FFun, CFun);
            }
        }
        /**
         * 获得输入数据
         */
        private static GetInputData(): Object {
            let data = null;
            if (document.forms["InputForm"].checkValidity()) {
                data = {
                    ID: ClassDetailsPage.PageData.params["ID"],
                    Name: (MDMa.$("InputName") as HTMLInputElement).value,
                }
            }
            return data;
        }
        /**
         * 删除按钮事件
         * @param e
         */
        private BtnDeleteEvent_Click(e: MouseEvent) {
            let url = "api/Class/DeleteClassInfo";
            let data = {
                ID: ClassDetailsPage.PageData.params["ID"]
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
        let pageM: ClassDetailsPage = new ClassDetailsPage();
    });
}