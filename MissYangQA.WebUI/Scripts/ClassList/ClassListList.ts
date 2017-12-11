///// <reference path="../jquery.d.ts" />
///// <reference path="../../lib/m-tools/m-tools.ts" />
///// <reference path="../base.ts" />
//module BackManage {
//    import MDMa = Materal.DOMManager;
//    import MTMa = Materal.ToolManager;
//    import Common = BackManage.Common;
//    class ClassListListModel {
//        /**
//         * 构造方法
//         */
//        constructor() {
//            if (Common.IsLogin(true)) {
//                Common.Init();
//                this.BindEvent();
//                ClassListListModel.GetList();
//            }
//        }
//        /**
//         * 绑定事件
//         */
//        private BindEvent() {
//        }
//        /**
//         * 获得列表信息
//         */
//        private static GetList() {
//            let url: string = "api/ClassList/GetAllClassListInfo";
//            let data = {
//            }
//            let SFun = function (resM: Object) {
//                if (resM["ResultType"] == 0) {
//                    let MainList = MDMa.$("MainList");
//                    if (!MTMa.IsNullOrUndefined(MainList)) {
//                        MainList.innerHTML = "";
//                        for (var i = 0; i < resM["Data"].length; i++) {
//                            let ListItem = document.createElement("a");
//                            ListItem.href = "/ClassList/ClassListDetailed?ID=" + resM["Data"][i]["ID"];
//                            ListItem.classList.add("list-group-item");
//                            let TextContent = document.createTextNode(resM["Data"][i]["Rank"]+"："+resM["Data"][i]["Name"]);
//                            let RightIco = document.createElement("i");
//                            MDMa.AddClass(RightIco, "list-right glyphicon glyphicon-chevron-right");
//                            ListItem.appendChild(TextContent);
//                            ListItem.appendChild(RightIco);
//                            MainList.appendChild(ListItem);
//                        }
//                    }
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
//    }
//    /*页面加载完毕事件*/
//    MDMa.AddEvent(window, "load", function (e: Event) {
//        let pageM: ClassListListModel = new ClassListListModel();
//    });
//}