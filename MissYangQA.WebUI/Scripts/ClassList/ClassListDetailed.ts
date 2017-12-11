///// <reference path="../jquery.d.ts" />
///// <reference path="../../lib/m-tools/m-tools.ts" />
///// <reference path="../base.ts" />
//module BackManage {
//    import MDMa = Materal.DOMManager;
//    import MTMa = Materal.ToolManager;
//    import Common = BackManage.Common;
//    class ClassListDetailedModel {
//        /*页面数据*/
//        private static PageData = {
//            params: MTMa.GetURLParams(),
//            url: ""
//        }
//        /*页面设置*/
//        private static PageSetting = {
//            UpdatePasswordState: false
//        }
//        /**
//         * 构造方法
//         */
//        constructor() {
//            if (Common.IsLogin(true)) {
//                Common.Init();
//                this.BindEvent();
//                this.BindMode();
//            }
//        }
//        /**
//         * 绑定事件
//         */
//        private BindEvent() {
//            MDMa.AddEvent("InputName", "invalid", function (e: Event) {
//                Common.InputInvalidEvent(e, {
//                    required: "班级名不能为空"
//                });
//            });
//            MDMa.AddEvent("InputRank", "invalid", function (e: Event) {
//                let inputElement = e.target as HTMLInputElement;
//                Common.InputInvalidEvent(e, {
//                    required: "位序不能为空",
//                    min: "位序不能小于" + inputElement.min
//                });
//            });
//            MDMa.AddEvent("BtnSave", "click", this.BtnSaveEvent_Click);
//        }
//        /**
//         * 绑定模式
//         */
//        private BindMode() {
//            let BtnSave = MDMa.$("BtnSave") as HTMLButtonElement;
//            if (!MTMa.IsNullOrUndefinedOrEmpty(ClassListDetailedModel.PageData.params["ID"])) {
//                this.GetClassListViewInfoByID();
//                ClassListDetailedModel.PageData.url = "api/ClassList/EditClassListInfo";
//                BtnSave.classList.add("glyphicon-floppy-disk");
//                Common.SetTitle("修改班级");
//            }
//            else {
//                ClassListDetailedModel.PageData.url = "api/ClassList/AddClassListInfo";
//                BtnSave.classList.add("glyphicon-plus");
//                Common.SetTitle("添加班级");
//            }
//        }
//        /**
//         * 获得对象信息
//         */
//        private GetClassListViewInfoByID() {
//            let url: string = "api/ClassList/GetClassListViewInfoByID";
//            let data = {
//                ID: ClassListDetailedModel.PageData.params["ID"]
//            }
//            let SFun = function (resM: Object) {
//                if (resM["ResultType"] == 0) {
//                    let InputName = MDMa.$("InputName") as HTMLInputElement;
//                    InputName.value = resM["Data"]["Name"];
//                    let InputRank = MDMa.$("InputRank") as HTMLInputElement;
//                    InputRank.value = resM["Data"]["Rank"];
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
//            let data = ClassListDetailedModel.GetInputData();
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
//                Common.SendAjax(ClassListDetailedModel.PageData.url, data, SFun, EFun, CFun);
//            }
//        }
//        /**
//         * 获得输入数据
//         */
//        private static GetInputData(): Object {
//            let data = null;
//            if (document.forms["InputForm"].checkValidity()) {
//                data = {
//                    ID: ClassListDetailedModel.PageData.params["ID"],
//                    Name: (MDMa.$("InputName") as HTMLInputElement).value,
//                    Rank: (MDMa.$("InputRank") as HTMLInputElement).value,
//                }
//            }
//            return data;
//        }
//    }
//    /*页面加载完毕事件*/
//    MDMa.AddEvent(window, "load", function (e: Event) {
//        let pageM: ClassListDetailedModel = new ClassListDetailedModel();
//    });
//}