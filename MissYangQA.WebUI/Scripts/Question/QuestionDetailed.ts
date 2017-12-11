///// <reference path="../jquery.d.ts" />
///// <reference path="../../lib/m-tools/m-tools.ts" />
///// <reference path="../base.ts" />
//module BackManage {
//    import MDMa = Materal.DOMManager;
//    import MTMa = Materal.ToolManager;
//    import Common = BackManage.Common;
//    class QuestionDetailedModel {
//        /*页面数据*/
//        private static PageData = {
//            params: MTMa.GetURLParams(),
//            url: "",
//            AnswerList: [],
//        }
//        /**
//         * 构造方法
//         */
//        constructor() {
//            if (Common.IsLogin(true)) {
//                Common.Init();
//                this.BindEvent();
//                QuestionDetailedModel.BindMode();
//            }
//        }
//        /**
//         * 绑定事件
//         */
//        private BindEvent() {
//            MDMa.AddEvent("InputQuestion", "invalid", function (e: Event) {
//                Common.InputInvalidEvent(e, {
//                    required: "问题描述不能为空"
//                });
//            });
//            MDMa.AddEvent("InputScore", "invalid", function (e: Event) {
//                let inputElement = e.target as HTMLInputElement;
//                Common.InputInvalidEvent(e, {
//                    required: "分值不能为空",
//                    min: "分值不能小于0"
//                });
//            });
//            MDMa.AddEvent("BtnSave", "click", this.BtnSaveEvent_Click);
//            MDMa.AddEvent("BtnAddAnswer", "click", this.BtnAddAnswerEvent_Click);
//        }
//        /**
//         * 绑定模式
//         */
//        private static BindMode() {
//            let BtnSave = MDMa.$("BtnSave") as HTMLButtonElement;
//            if (!MTMa.IsNullOrUndefinedOrEmpty(QuestionDetailedModel.PageData.params["ID"])) {
//                QuestionDetailedModel.GetQuestionViewInfoByID();
//                QuestionDetailedModel.PageData.url = "api/Question/EditQuestionInfo";
//                BtnSave.classList.add("glyphicon-floppy-disk");
//                Common.SetTitle("修改问题");
//            }
//            else {
//                QuestionDetailedModel.PageData.url = "api/Question/AddQuestionInfo";
//                BtnSave.classList.add("glyphicon-plus");
//                Common.SetTitle("添加问题");
//            }
//        }
//        /**
//         * 获得对象信息
//         */
//        private static GetQuestionViewInfoByID() {
//            let url: string = "api/Question/GetQuestionViewInfoByID";
//            let data = {
//                ID: QuestionDetailedModel.PageData.params["ID"]
//            }
//            let SFun = function (resM: Object) {
//                if (resM["ResultType"] == 0) {
//                    let InputTitle = MDMa.$("InputTitle") as HTMLInputElement;
//                    InputTitle.value = resM["Data"]["Title"];
//                    let InputClass = MDMa.$("InputClass") as HTMLInputElement;
//                    InputClass.value = resM["Data"]["FK_ClassList"];
//                    let InputStartDate = MDMa.$("InputStartDate") as HTMLInputElement;
//                    InputStartDate.value = resM["Data"]["StartTimeStr"];
//                    let InputEndDate = MDMa.$("InputEndDate") as HTMLInputElement;
//                    InputEndDate.value = resM["Data"]["EndTimeStr"];
//                }
//                else {
//                    Common.ShowMessageBox(resM["Message"])
//                }
//            };
//            let EFun = function (xhr: XMLHttpRequest, resM: Object) {
//            };
//            let CFun = function (resM: Object) {
//            };
//            Common.SendAjax(url, data, SFun, EFun, CFun);
//        }
//        /**
//         * 保存按钮事件
//         * @param e
//         */
//        private BtnSaveEvent_Click(e: MouseEvent) {
//            Common.ClearErrorMessage();
//            let data = QuestionDetailedModel.GetInputData();
//            if (!MTMa.IsNullOrUndefined(data)) {
//                let BtnElement = e.target as HTMLButtonElement;
//                BtnElement.disabled = true;
//                let SFun = function (resM: Object) {
//                    if (resM["ResultType"] == 0) {
//                        window.history.back();
//                    }
//                    else {
//                        Common.ShowMessageBox(resM["Message"])
//                    }
//                };
//                let EFun = function (xhr: XMLHttpRequest, resM: Object) {
//                };
//                let CFun = function (resM: Object) {
//                    BtnElement.disabled = false
//                };
//                Common.SendAjax(QuestionDetailedModel.PageData.url, data, SFun, EFun, CFun);
//            }
//        }
//        /**
//         * 获得输入数据
//         */
//        private static GetInputData(): Object {
//            let data = null;
//            if (document.forms["InputForm"].checkValidity()) {
//                data = {
//                    ID: QuestionDetailedModel.PageData.params["ID"],
//                    Contents: (MDMa.$("InputQuestion") as HTMLTextAreaElement).value,
//                    Score: (MDMa.$("InputScore") as HTMLInputElement).value,
//                    AnswerList: QuestionDetailedModel.PageData.AnswerList
//                }
//                if (data["AnswerList"]["length"] == 0) {
//                    data = null;
//                    let BtnAddAnswer = MDMa.$("BtnAddAnswer") as HTMLElement;
//                    Common.SetInputErrorMessage(BtnAddAnswer, "答案列表不能为空");
//                }
//            }
//            return data;
//        }
//        private GetAnswerItem(model) {
//            let AnswerList = MDMa.$("AnswerList") as HTMLDivElement;
//            let answerGroup = document.createElement("div");
//            MDMa.AddClass(answerGroup, "form-group input-group");
//            let answerContent = document.createElement("input");
//            MDMa.AddClass(answerContent, "form-control");
//            answerContent.placeholder = "请输入正确答案";
//            answerContent.type = "text";
//            let answerBtnGroup = document.createElement("span");
//            MDMa.AddClass(answerBtnGroup, "input-group-btn");
//            let answerBtnSetRight = document.createElement("button");
//            MDMa.AddClass(answerBtnSetRight, "btn btn-success");
//            answerBtnSetRight.type = "button";
//            answerBtnSetRight.textContent = "√";
//            let answerBtnRemove = document.createElement("button");
//            MDMa.AddClass(answerBtnRemove, "btn btn-danger");
//            answerBtnRemove.type = "button";
//            answerBtnRemove.textContent = "×";
//            answerBtnGroup.appendChild(answerBtnSetRight);
//            answerBtnGroup.appendChild(answerBtnRemove);
//            answerGroup.appendChild(answerContent);
//            answerGroup.appendChild(answerBtnGroup);
//            AnswerList.appendChild(answerGroup);
//        }
//        /**
//         * 添加答案按钮单击事件
//         * @param e
//         */
//        private BtnAddAnswerEvent_Click(e: MouseEvent) {
//        }
//    }
//    /*页面加载完毕事件*/
//    MDMa.AddEvent(window, "load", function (e: Event) {
//        let pageM: QuestionDetailedModel = new QuestionDetailedModel();
//    });
//}