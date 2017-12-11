///// <reference path="../jquery.d.ts" />
///// <reference path="../../lib/m-tools/m-tools.ts" />
///// <reference path="../base.ts" />
//module BackManage {
//    import MDMa = Materal.DOMManager;
//    import MTMa = Materal.ToolManager;
//    import Common = BackManage.Common;
//    class PaperDetailedModel {
//        /*页面数据*/
//        private static PageData = {
//            params: MTMa.GetURLParams(),
//            url: ""
//        }
//        /**
//         * 构造方法
//         */
//        constructor() {
//            if (Common.IsLogin(true)) {
//                Common.Init();
//                this.BindEvent();
//                PaperDetailedModel.BindMode();
//            }
//        }
//        /**
//         * 绑定事件
//         */
//        private BindEvent() {
//            MDMa.AddEvent("InputTitle", "invalid", function (e: Event) {
//                Common.InputInvalidEvent(e, {
//                    required: "试题名不能为空"
//                });
//            });
//            MDMa.AddEvent("InputStartDate", "invalid", function (e: Event) {
//                let inputElement = e.target as HTMLInputElement;
//                Common.InputInvalidEvent(e, {
//                    required: "开始时间不能为空",
//                });
//            });
//            MDMa.AddEvent("InputEndDate", "invalid", function (e: Event) {
//                let inputElement = e.target as HTMLInputElement;
//                Common.InputInvalidEvent(e, {
//                    required: "结束时间不能为空",
//                });
//            });
//            MDMa.AddEvent("BtnSave", "click", this.BtnSaveEvent_Click);
//        }
//        /**
//         * 绑定模式
//         */
//        private static BindMode() {
//            let BtnSave = MDMa.$("BtnSave") as HTMLButtonElement;
//            let LinkQuestion = MDMa.$("LinkQuestion") as HTMLAnchorElement;
//            if (!MTMa.IsNullOrUndefinedOrEmpty(PaperDetailedModel.PageData.params["ID"])) {
//                PaperDetailedModel.GetPaperViewInfoByID();
//                PaperDetailedModel.PageData.url = "api/Paper/EditPaperInfo";
//                BtnSave.classList.add("glyphicon-floppy-disk");
//                Common.SetTitle("修改试题");
//                LinkQuestion.href = "/Question/QuestionList?ID=" + PaperDetailedModel.PageData.params["ID"];
//            }
//            else {
//                PaperDetailedModel.PageData.url = "api/Paper/AddPaperInfo";
//                BtnSave.classList.add("glyphicon-plus");
//                Common.SetTitle("添加试题");
//                LinkQuestion.setAttribute("style", "display:none;");
//            }
//        }
//        /**
//         * 获得对象信息
//         */
//        private static GetPaperViewInfoByID() {
//            let url: string = "api/Paper/GetPaperViewInfoByID";
//            let data = {
//                ID: PaperDetailedModel.PageData.params["ID"]
//            }
//            let SFun = function (resM: Object) {
//                if (resM["ResultType"] == 0) {
//                    let InputTitle = MDMa.$("InputTitle") as HTMLInputElement;
//                    InputTitle.value = resM["Data"]["Title"];
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
//            let data = PaperDetailedModel.GetInputData();
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
//                Common.SendAjax(PaperDetailedModel.PageData.url, data, SFun, EFun, CFun);
//            }
//        }
//        /**
//         * 获得输入数据
//         */
//        private static GetInputData(): Object {
//            let data = null;
//            if (document.forms["InputForm"].checkValidity()) {
//                data = {
//                    ID: PaperDetailedModel.PageData.params["ID"],
//                    Title: (MDMa.$("InputTitle") as HTMLInputElement).value,
//                    StartTime: (MDMa.$("InputStartDate") as HTMLInputElement).value,
//                    EndTime: (MDMa.$("InputEndDate") as HTMLInputElement).value,
//                    FK_ClassList: (MDMa.$("InputClass") as HTMLSelectElement).value,
//                }
//            }
//            return data;
//        }
//    }
//    /*页面加载完毕事件*/
//    MDMa.AddEvent(window, "load", function (e: Event) {
//        let pageM: PaperDetailedModel = new PaperDetailedModel();
//    });
//}