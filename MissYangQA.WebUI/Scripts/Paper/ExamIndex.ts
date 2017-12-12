/// <reference path="../jquery.d.ts" />
/// <reference path="../base.ts" />
'use strict';
namespace MissYangQA {
    class ExamIndexPage {
        /*页面数据*/
        private static PageData = {
            params: MTMa.GetURLParams(),
        }
        /**
         * 构造方法
         */
        constructor() {
            this.BindEvent();
            this.GetAllClassInfo();
        }
        /**
         * 绑定事件
         */
        private BindEvent() {
            MDMa.AddEvent("InputStudentName", "invalid", function (e: Event) {
                let setting: InvalidOptionsModel = new InvalidOptionsModel();
                setting.Required = "姓名不能为空";
                common.InputInvalidEvent_Invalid(e, setting);
            });
            MDMa.AddEvent("BtnSubmit", "click", this.BtnSubmitEvent_Click);
        }
        /**
         * 提交按钮事件
         * @param e
         */
        private BtnSubmitEvent_Click(e: MouseEvent) {
            common.ClearErrorMessage();
            let data = ExamIndexPage.GetInputData();
            if (!MTMa.IsNullOrUndefined(data)) {
                common.GoToPage("ExamDetails", [
                    "ClassID=" + data["ClassID"],
                    "PaperID=" + data["PaperID"],
                    "StudentName=" + encodeURIComponent(data["StudentName"])
                ]);
            }
        }
        /**
         * 获得输入数据
         */
        private static GetInputData(): Object {
            let data = null;
            if (document.forms["InputForm"].checkValidity()) {
                data = {
                    StudentName: (MDMa.$("InputStudentName") as HTMLInputElement).value,
                    ClassID: (MDMa.$("SelectClass") as HTMLSelectElement).value,
                    PaperID: (MDMa.$("SelectPaper") as HTMLSelectElement).value,
                }
            }
            return data;
        }
        /**
         * 获得所有班级信息
         */
        private GetAllClassInfo() {
            let url: string = "api/Class/GetAllClassInfo";
            let data = {}
            let SFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                let SelectClass = MDMa.$("SelectClass") as HTMLSelectElement;
                if (!MTMa.IsNullOrUndefined(SelectClass)) {
                    SelectClass.innerHTML = "";
                    for (let i = 0; i < resM["Data"].length; i++) {
                        let option = new Option(resM["Data"][i]["Name"], resM["Data"][i]["ID"]);
                        SelectClass.options.add(option);
                    }
                }
                ExamIndexPage.GetAllEnablePaperInfo();
            };
            let FFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                common.ShowMessageBox(resM["Message"])
            };
            let CFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
            };
            common.SendGetAjax(url, data, SFun, FFun, CFun);
        }
        /**
         * 获得所有启用的试题信息
         */
        private static GetAllEnablePaperInfo() {
            let url: string = "api/Paper/GetAllEnablePaperInfo";
            let data = {}
            let SFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                let SelectPaper = MDMa.$("SelectPaper") as HTMLSelectElement;
                if (!MTMa.IsNullOrUndefined(SelectPaper)) {
                    SelectPaper.innerHTML = "";
                    for (let i = 0; i < resM["Data"].length; i++) {
                        let option = new Option(resM["Data"][i]["Title"], resM["Data"][i]["ID"]);
                        SelectPaper.options.add(option);
                    }
                }
                ExamIndexPage.BindParams();
            };
            let FFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                common.ShowMessageBox(resM["Message"])
            };
            let CFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
            };
            common.SendGetAjax(url, data, SFun, FFun, CFun);
        }
        /**
         * 绑定参数
         */
        private static BindParams() {
            if (!MTMa.IsNullOrUndefinedOrEmpty(ExamIndexPage.PageData.params["ClassID"])) {
                let SelectClass = MDMa.$("SelectClass") as HTMLSelectElement;
                if (!MTMa.IsNullOrUndefined(SelectClass)) {
                    SelectClass.value = ExamIndexPage.PageData.params["ClassID"];
                }
            }
            if (!MTMa.IsNullOrUndefinedOrEmpty(ExamIndexPage.PageData.params["PaperID"])) {
                let SelectPaper = MDMa.$("SelectPaper") as HTMLSelectElement;
                if (!MTMa.IsNullOrUndefined(SelectPaper)) {
                    SelectPaper.value = ExamIndexPage.PageData.params["PaperID"];
                }
            }
        }
    }
    /*页面加载完毕事件*/
    MDMa.AddEvent(window, "load", function (e: Event) {
        let pageM: ExamIndexPage = new ExamIndexPage();
    });
}