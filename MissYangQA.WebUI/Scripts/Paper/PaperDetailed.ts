/// <reference path="../jquery.d.ts" />
/// <reference path="../base.ts" />
'use strict';
namespace MissYangQA {
    class PaperDetailedPage {
        /*页面数据*/
        private static PageData = {
            params: MTMa.GetURLParams(),
            url: ""
        }
        /**
         * 构造方法
         */
        constructor() {
            if (common.IsLogin(true)) {
                this.BindEvent();
                PaperDetailedPage.BindMode();
            }
        }
        /**
         * 绑定事件
         */
        private BindEvent() {
            MDMa.AddEvent("InputTitle", "invalid", function (e: Event) {
                let setting: InvalidOptionsModel = new InvalidOptionsModel();
                setting.Required = "试题标题不能为空";
                common.InputInvalidEvent_Invalid(e, setting);
            });
            MDMa.AddEvent("InputStartDate", "invalid", function (e: Event) {
                let setting: InvalidOptionsModel = new InvalidOptionsModel();
                setting.Required = "开始时间不能为空";
                common.InputInvalidEvent_Invalid(e, setting);
            });
            MDMa.AddEvent("InputEndDate", "invalid", function (e: Event) {
                let setting: InvalidOptionsModel = new InvalidOptionsModel();
                setting.Required = "结束时间不能为空";
                common.InputInvalidEvent_Invalid(e, setting);
            });
            MDMa.AddEvent("BtnSave", "click", this.BtnSaveEvent_Click);
            MDMa.AddEvent("BtnDelete", "click", this.BtnDeleteEvent_Click);
        }
        /**
         * 绑定模式
         */
        private static BindMode() {
            let BtnSave = MDMa.$("BtnSave") as HTMLButtonElement;
            let LinkQuestion = MDMa.$("LinkQuestion") as HTMLAnchorElement;
            if (!MTMa.IsNullOrUndefinedOrEmpty(PaperDetailedPage.PageData.params["ID"])) {
                PaperDetailedPage.GetPaperViewInfoByID();
                PaperDetailedPage.PageData.url = "api/Paper/EditPaperInfo";
                BtnSave.classList.add("glyphicon-floppy-disk");
                let TopTools = BtnSave.parentElement;
                let deleteClassListBtn = document.createElement("button");
                MDMa.AddClass(deleteClassListBtn, "btn btn-danger glyphicon glyphicon-remove");
                deleteClassListBtn.type = "button";
                deleteClassListBtn.dataset.toggle = "modal";
                deleteClassListBtn.dataset.target = "#DeleteModal";
                TopTools.appendChild(deleteClassListBtn);
                common.SetTitle("修改试题");
                LinkQuestion.href = "/Question/QuestionList?ID=" + PaperDetailedPage.PageData.params["ID"];
            }
            else {
                PaperDetailedPage.PageData.url = "api/Paper/AddPaperInfo";
                BtnSave.classList.add("glyphicon-plus");
                common.SetTitle("添加试题");
                LinkQuestion.setAttribute("style", "display:none;");
                let QuestionCountGroup = MDMa.$("QuestionCountGroup") as HTMLDivElement;
                QuestionCountGroup.setAttribute("style", "display:none;");
                let SumScoreGroup = MDMa.$("SumScoreGroup") as HTMLDivElement;
                SumScoreGroup.setAttribute("style", "display:none;");
            }
        }
        /**
         * 获得对象信息
         */
        private static GetPaperViewInfoByID() {
            let url: string = "api/Paper/GetPaperInfoByID";
            let data = {
                ID: PaperDetailedPage.PageData.params["ID"]
            }
            let SFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                let InputTitle = MDMa.$("InputTitle") as HTMLInputElement;
                InputTitle.value = resM["Data"]["Title"];
                let InputStartDate = MDMa.$("InputStartDate") as HTMLInputElement;
                let StartTime = new Date(resM["Data"]["StartTime"]);
                InputStartDate.value = MTMa.DateTimeFormat(StartTime, "yyyy-MM-dd");
                let InputEndDate = MDMa.$("InputEndDate") as HTMLInputElement;
                let EndTime = new Date(resM["Data"]["EndTime"]);
                InputEndDate.value = MTMa.DateTimeFormat(EndTime, "yyyy-MM-dd");
                let InputQuestionCount = MDMa.$("InputQuestionCount") as HTMLInputElement;
                InputQuestionCount.value = resM["Data"]["QuestionCount"];
                let InputSumScore = MDMa.$("InputSumScore") as HTMLInputElement;
                InputSumScore.value = resM["Data"]["SumScore"];
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
            let data = PaperDetailedPage.GetInputData();
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
                common.SendPostAjax(PaperDetailedPage.PageData.url, data, SFun, FFun, CFun);
            }
        }
        /**
         * 获得输入数据
         */
        private static GetInputData(): Object {
            let data = null;
            if (document.forms["InputForm"].checkValidity()) {
                data = {
                    ID: PaperDetailedPage.PageData.params["ID"],
                    Title: (MDMa.$("InputTitle") as HTMLInputElement).value,
                    StartTime: (MDMa.$("InputStartDate") as HTMLInputElement).value,
                    EndTime: (MDMa.$("InputEndDate") as HTMLInputElement).value
                }
            }
            return data;
        }
        /**
         * 删除按钮事件
         * @param e
         */
        private BtnDeleteEvent_Click(e: MouseEvent) {
            let url = "api/Paper/DeletePaperInfo";
            let data = {
                ID: PaperDetailedPage.PageData.params["ID"]
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
        let pageM: PaperDetailedPage = new PaperDetailedPage();
    });
}