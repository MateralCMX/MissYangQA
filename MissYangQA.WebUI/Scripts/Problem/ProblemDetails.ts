/// <reference path="../jquery.d.ts" />
/// <reference path="../base.ts" />
'use strict';
namespace MissYangQA {
    class ProblemDetailsPage {
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
                ProblemDetailsPage.BindMode();
            }
        }
        /**
         * 绑定事件
         */
        private BindEvent() {
            MDMa.AddEvent("InputContents", "invalid", function (e: Event) {
                let setting: InvalidOptionsModel = new InvalidOptionsModel();
                setting.Required = "问题不能为空";
                common.InputInvalidEvent_Invalid(e, setting);
            });
            MDMa.AddEvent("InputScore", "invalid", function (e: Event) {
                let setting: InvalidOptionsModel = new InvalidOptionsModel();
                setting.Required = "分值不能为空";
                setting.Min = "分值不能小于0";
                common.InputInvalidEvent_Invalid(e, setting);
            });
            MDMa.AddEvent("BtnSave", "click", this.BtnSaveEvent_Click);
            MDMa.AddEvent("BtnDelete", "click", this.BtnDeleteEvent_Click);
            MDMa.AddEvent("BtnAddAnswer", "click", this.BtnAddAnswersEvent_Click);
        }
        /**
         * 绑定模式
         */
        private static BindMode() {
            let BtnSave = MDMa.$("BtnSave") as HTMLButtonElement;
            if (!MTMa.IsNullOrUndefinedOrEmpty(ProblemDetailsPage.PageData.params["ID"])) {
                ProblemDetailsPage.GetProblemViewInfoByID();
                ProblemDetailsPage.PageData.url = "api/Problem/EditProblemInfo";
                BtnSave.classList.add("glyphicon-floppy-disk");
                let TopTools = BtnSave.parentElement;
                let deleteClassListBtn = document.createElement("button");
                MDMa.AddClass(deleteClassListBtn, "btn btn-danger glyphicon glyphicon-trash");
                deleteClassListBtn.type = "button";
                deleteClassListBtn.dataset.toggle = "modal";
                deleteClassListBtn.dataset.target = "#DeleteModal";
                TopTools.appendChild(deleteClassListBtn);
                common.SetTitle("修改问题");
            }
            else {
                ProblemDetailsPage.PageData.url = "api/Problem/AddProblemInfo";
                BtnSave.classList.add("glyphicon-plus");
                common.SetTitle("添加问题");
            }
        }
        /**
         * 获得对象信息
         */
        private static GetProblemViewInfoByID() {
            let url: string = "api/Problem/GetProblemInfoByID";
            let data = {
                ID: ProblemDetailsPage.PageData.params["ID"]
            }
            let SFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                let InputContents = MDMa.$("InputContents") as HTMLInputElement;
                InputContents.value = resM["Data"]["Contents"];
                let InputScore = MDMa.$("InputScore") as HTMLInputElement;
                InputScore.value = resM["Data"]["Score"];
                let Answers = resM["Data"]["Answers"] as Array<any>;
                for (var i = 0; i < Answers.length; i++) {
                    ProblemDetailsPage.AddAnswersItem(Answers[i]);
                }
                //let InputProblemCount = MDMa.$("InputProblemCount") as HTMLInputElement;
                //InputProblemCount.value = resM["Data"]["ProblemCount"];
                //let InputSumScore = MDMa.$("InputSumScore") as HTMLInputElement;
                //InputSumScore.value = resM["Data"]["SumScore"];
            };
            let FFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                common.ShowMessageBox(resM["Message"])
            };
            let CFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
            };
            common.SendGetAjax(url, data, SFun, FFun, CFun);
        }
        /**
         * 添加一个答案组
         * @param answerM
         */
        private static AddAnswersItem(answerM: any = null) {
            if (answerM == null) {
                answerM = {
                    ID: null,
                    IsCorrect: false,
                    Contents:"",
                };
            }
            let Answers = MDMa.$("AnswersList") as HTMLDivElement;
            let AnswerItem = document.createElement("div");
            AnswerItem.dataset.id = answerM["ID"];
            MDMa.AddClass(AnswerItem, "form-group input-group");
            let AnswerAddon = document.createElement("span");
            MDMa.AddClass(AnswerAddon, "input-group-addon");
            AnswerItem.appendChild(AnswerAddon);
            let AnswerCheckbox = document.createElement("input");
            AnswerCheckbox.type = "checkbox";
            AnswerCheckbox.name = "AnswerIsCorrect"
            AnswerCheckbox.checked = answerM["IsCorrect"];
            AnswerAddon.appendChild(AnswerCheckbox);
            let AnswerContent = document.createElement("input");
            AnswerContent.type = "text";
            AnswerContent.value = answerM["Contents"];
            AnswerContent.required = true;
            MDMa.AddEvent(AnswerContent, "invalid", function (e: Event) {
                let setting: InvalidOptionsModel = new InvalidOptionsModel();
                setting.Required = "答案不能为空";
                common.InputInvalidEvent_Invalid(e, setting);
            });
            MDMa.AddClass(AnswerContent, "form-control");
            AnswerItem.appendChild(AnswerContent);
            let AnswerGroupBtn = document.createElement("span");
            MDMa.AddClass(AnswerGroupBtn, "input-group-btn");
            AnswerItem.appendChild(AnswerGroupBtn);
            let AnswerRemoveBtn = document.createElement("button");
            MDMa.AddClass(AnswerRemoveBtn, "btn btn-danger glyphicon glyphicon-trash");
            AnswerRemoveBtn.type = "button";
            AnswerGroupBtn.appendChild(AnswerRemoveBtn);
            MDMa.AddEvent(AnswerGroupBtn, "click", ProblemDetailsPage.BtnRemoveAnswersEvent_Click);
            Answers.appendChild(AnswerItem);
        }
        /**
         * 添加一个答案组
         * @param e
         */
        private BtnAddAnswersEvent_Click(e: MouseEvent) {
            ProblemDetailsPage.AddAnswersItem();
        }
        /**
         * 移除一个答案组
         * @param e
         */
        private static BtnRemoveAnswersEvent_Click(e: MouseEvent) {
            let removeBtn = e.target as HTMLButtonElement;
            let item = removeBtn.parentElement.parentElement;
            item.parentElement.removeChild(item);
        }
        /**
         * 保存按钮事件
         * @param e
         */
        private BtnSaveEvent_Click(e: MouseEvent) {
            common.ClearErrorMessage();
            let data = ProblemDetailsPage.GetInputData();
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
                common.SendPostAjax(ProblemDetailsPage.PageData.url, data, SFun, FFun, CFun);
            }
        }
        /**
         * 获得输入数据
         */
        private static GetInputData(): Object {
            let data = null;
            if (document.forms["InputForm"].checkValidity()) {
                data = {
                    ID: ProblemDetailsPage.PageData.params["ID"],
                    Contents: (MDMa.$("InputContents") as HTMLInputElement).value,
                    Score: (MDMa.$("InputScore") as HTMLInputElement).value,
                    FK_Paper: ProblemDetailsPage.PageData.params["PaperID"],
                    Answers:[]
                }
                let AnswersList = MDMa.$("AnswersList") as HTMLDivElement;
                if (!MTMa.IsNullOrUndefined(AnswersList) && AnswersList.children.length > 0) {
                    let AnswersItems = AnswersList.children;
                    for (var i = 0; i < AnswersItems.length; i++) {
                        data["Answers"].push({
                            ID: (AnswersItems[i] as HTMLDivElement).dataset.id,
                            IsCorrect: (AnswersItems[i].firstChild.firstChild as HTMLInputElement).checked,
                            Contents: (AnswersItems[i].children[1] as HTMLInputElement).value,
                        });
                    }
                }
                else {
                    common.ShowMessageBox("答案不能为空");
                    data = null;
                }
            }
            return data;
        }
        /**
         * 删除按钮事件
         * @param e
         */
        private BtnDeleteEvent_Click(e: MouseEvent) {
            let url = "api/Problem/DeleteProblemInfo";
            let data = {
                ID: ProblemDetailsPage.PageData.params["ID"]
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
        let pageM: ProblemDetailsPage = new ProblemDetailsPage();
    });
}