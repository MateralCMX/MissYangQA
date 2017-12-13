/// <reference path="../jquery.d.ts" />
/// <reference path="../base.ts" />
'use strict';
namespace MissYangQA {
    class ExamDetailsPage {
        /*页面数据*/
        private static PageData = {
            params: MTMa.GetURLParams(),
            AnswersNo: ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"]
        }
        /**
         * 构造方法
         */
        constructor() {
            if (!MTMa.IsNullOrUndefinedOrEmpty(ExamDetailsPage.PageData.params["ClassID"]) &&
                !MTMa.IsNullOrUndefinedOrEmpty(ExamDetailsPage.PageData.params["PaperID"]) &&
                !MTMa.IsNullOrUndefinedOrEmpty(ExamDetailsPage.PageData.params["StudentName"])
            ) {
                this.BindEvent();
                this.GetExamInfoByPaperID();
            }
            else {
                window.history.back();
            }
        }
        /**
         * 绑定事件
         */
        private BindEvent() {
            MDMa.AddEvent("BtnSubmit", "click", this.BtnSubmitEvent_Click);
        }
        /**
         * 根据试题ID获得试卷信息
         */
        private GetExamInfoByPaperID() {
            let url: string = "api/Paper/GetExamInfoByPaperID";
            let data = {
                PaperID: ExamDetailsPage.PageData.params["PaperID"]
            }
            let SFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                let PaperInfo = resM["Data"];
                common.SetTitle(PaperInfo.Title);
                let Problems = PaperInfo["Problems"] as Array<any>;
                let PaperDiv = MDMa.$("PaperDiv") as HTMLDivElement;
                PaperDiv.innerHTML = "";
                for (var i = 0; i < Problems.length; i++) {
                    let ProblemItem = document.createElement("div");
                    MDMa.AddClass(ProblemItem, "panel panel-default");
                    ProblemItem.dataset.id = Problems[i]["ID"];
                    let ProblemHeading = document.createElement("div");
                    MDMa.AddClass(ProblemHeading, "panel-heading");
                    let ProblemNumber = document.createElement("span");
                    ProblemNumber.textContent = (i + 1).toString() + ".";
                    ProblemHeading.appendChild(ProblemNumber);
                    let ProblemType = document.createTextNode(Problems[i]["ProblemTypeStr"]);
                    ProblemHeading.appendChild(ProblemType);
                    let ProblemScore = document.createElement("span");
                    MDMa.AddClass(ProblemScore, "Score");
                    ProblemScore.textContent = "(" + Problems[i]["Score"] + "分)";
                    ProblemHeading.appendChild(ProblemScore);
                    ProblemItem.appendChild(ProblemHeading)
                    let ProblemBody = document.createElement("div");
                    MDMa.AddClass(ProblemBody, "panel-body");
                    ProblemBody.textContent = Problems[i]["Contents"];
                    ProblemItem.appendChild(ProblemBody);
                    let AnswersList = document.createElement("div");
                    MDMa.AddClass(AnswersList, "list-group");
                    AnswersList.dataset.type = Problems[i]["ProblemType"];
                    let Answers = Problems[i]["Answers"] as Array<any>;
                    for (var j = 0; j < Answers.length; j++) {
                        let AnswerBtn = document.createElement("button");
                        MDMa.AddClass(AnswerBtn, "list-group-item");
                        AnswerBtn.dataset.id = Answers[j]["ID"];
                        AnswerBtn.textContent = ExamDetailsPage.PageData.AnswersNo[j] + " : " + Answers[j]["Contents"];
                        MDMa.AddEvent(AnswerBtn, "click", ExamDetailsPage.BtnAnswerEvent_Click);
                        AnswersList.appendChild(AnswerBtn);
                    }
                    ProblemItem.appendChild(AnswersList);
                    PaperDiv.appendChild(ProblemItem);
                }
                let BtnOpenSubmitWindow = MDMa.$("BtnOpenSubmitWindow") as HTMLButtonElement;
                BtnOpenSubmitWindow.disabled = false;
            };
            let FFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                common.ShowMessageBox(resM["Message"])
            };
            let CFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
            };
            common.SendGetAjax(url, data, SFun, FFun, CFun);
        }
        /**
         * 答案单击事件
         * @param e
         */
        private static BtnAnswerEvent_Click(e: MouseEvent) {
            let btnElement = e.target as HTMLButtonElement;
            let answerList = btnElement.parentElement as HTMLDivElement;
            let ProblemType: number = parseInt(answerList.dataset.type);
            switch (ProblemType) {
                case 1://单选题
                    ExamDetailsPage.SelectAnswerByRadio(btnElement);
                    break;
                case 2://多选题
                    ExamDetailsPage.SelectMultipleByRadio(btnElement);
                    break;
                case 3://问答题
                    break;
                default://其他
                    break;
            }
        }
        /**
         * 选择单选题
         * @param btnElement
         */
        private static SelectAnswerByRadio(btnElement: HTMLButtonElement) {
            let answerList = btnElement.parentElement as HTMLDivElement;
            let panelElement = answerList.parentElement as HTMLDivElement;
            MDMa.RemoveClass(panelElement, "panel-default panel-info");
            if (MDMa.HasClass(btnElement, "active")) {
                MDMa.RemoveClass(btnElement, "active");
                MDMa.AddClass(panelElement, "panel-default");
            }
            else {
                for (var i = 0; i < answerList.children.length; i++) {
                    MDMa.RemoveClass(answerList.children[i], "active");
                }
                MDMa.AddClass(btnElement, "active");
                MDMa.AddClass(panelElement, "panel-info");
            }
        }
        /**
         * 选择多选题
         * @param btnElement
         */
        private static SelectMultipleByRadio(btnElement: HTMLButtonElement) {
            let answerList = btnElement.parentElement as HTMLDivElement;
            let panelElement = answerList.parentElement as HTMLDivElement;
            MDMa.RemoveClass(panelElement, "panel-default panel-info");
            if (MDMa.HasClass(btnElement, "active")) {
                MDMa.RemoveClass(btnElement, "active");
            }
            else {
                MDMa.AddClass(btnElement, "active");
            }
            let activeCount = answerList.getElementsByClassName("active").length;
            if (activeCount > 1) {
                MDMa.AddClass(panelElement, "panel-info");
            }
            else {
                MDMa.AddClass(panelElement, "panel-default");
            }
        }
        /**
         * 提交按钮单击事件
         * @param e
         */
        private BtnSubmitEvent_Click(e: MouseEvent) {
            let data = ExamDetailsPage.GetAnswerSheetInfo();
            if (!MTMa.IsNullOrUndefined(data)) {
                let url: string = "api/AnswerSheet/SubmitAnswerSheet";
                let SFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                    common.GoToPage("AnswerSheetDetails", ["ID=" + resM["Data"]]);
                };
                let FFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                    common.ShowMessageBox(resM["Message"])
                };
                let CFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                };
                common.SendPostAjax(url, data, SFun, FFun, CFun);
            }
            else {
                $('#SubmitModal').modal('toggle');
                common.ShowMessageBox("请至少完成一题");
            }
        }
        /**
         * 获得答题卡信息
         */
        private static GetAnswerSheetInfo() {
            let PaperDiv = MDMa.$("PaperDiv");
            let ProblemList = PaperDiv.children;
            let data = {
                PaperID: ExamDetailsPage.PageData.params["PaperID"],
                ClassID: ExamDetailsPage.PageData.params["ClassID"],
                StudentName: ExamDetailsPage.PageData.params["StudentName"],
                Answers:[]
            };
            for (var i = 0; i < ProblemList.length; i++) {
                let Problem = ProblemList[i] as HTMLDivElement;
                let AnswerList = Problem.lastChild as HTMLDivElement;
                for (var j = 0; j < AnswerList.children.length; j++) {
                    let Answer = AnswerList.children[j] as HTMLButtonElement;
                    if (MDMa.HasClass(Answer, "active")) {
                        data.Answers.push(Answer.dataset.id);
                    }
                }
            }
            if (data.Answers.length == 0) {
                data = null;
            }
            return data;
        }
    }
    /*页面加载完毕事件*/
    MDMa.AddEvent(window, "load", function (e: Event) {
        let pageM: ExamDetailsPage = new ExamDetailsPage();
    });
}