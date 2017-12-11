///// <reference path="../jquery.d.ts" />
///// <reference path="../../lib/m-tools/m-tools.ts" />
///// <reference path="../base.ts" />
//module BackManage {
//    import MDMa = Materal.DOMManager;
//    import MTMa = Materal.ToolManager;
//    import Common = BackManage.Common;
//    class PaperListModel {
//        /*页面设置*/
//        private static PageSetting = {
//            IsLoading: false,
//            OldScrollTop: 0,
//            PageIndex: 1,
//            PageSize: 10,
//            PageCount: 99
//        }
//        /**
//         * 构造方法
//         */
//        constructor() {
//            if (Common.IsLogin(true)) {
//                Common.Init();
//                this.BindEvent();
//                this.GetAllPaperStateInfo();
//            }
//        }
//        /**
//         * 绑定事件
//         */
//        private BindEvent() {
//            MDMa.AddEvent("BtnSearch", "click", this.BtnSearchEvent_Click);
//            /*下拉加载*/
//            MDMa.AddEvent(window, "scroll", function (e) {
//                var viewH = Common.GetClientHeight();//可见高度
//                var contentH = Math.max(document.body.scrollHeight, document.documentElement.scrollHeight);//内容高度
//                var scrollTop = Common.GetScrollTop();//滚动条位置
//                if (contentH - viewH - scrollTop <= 100 && !PaperListModel.PageSetting.IsLoading && PaperListModel.PageSetting.OldScrollTop < scrollTop) {
//                    PaperListModel.GetList();
//                }
//                PaperListModel.PageSetting.OldScrollTop = scrollTop;
//            });
//        }
//        /**
//         * 查询按钮单击事件
//         * @param e
//         */
//        private BtnSearchEvent_Click(e: MouseEvent) {
//            PaperListModel.PageSetting.PageIndex = 1;
//            PaperListModel.PageSetting.PageCount = 99;
//            let MainList = MDMa.$("MainList");
//            MainList.innerHTML = "";
//            PaperListModel.GetList();
//            $('#SearchModal').modal('toggle');
//        }
//        /**
//         * 获得所有试题状态信息
//         */
//        private GetAllPaperStateInfo() {
//            let url: string = "api/Paper/GetAllPaperStateInfo";
//            let data = {
//            }
//            let SFun = function (resM: Object) {
//                if (resM["ResultType"] == 0) {
//                    let SearchState = MDMa.$("SearchState") as HTMLSelectElement;
//                    if (!MTMa.IsNullOrUndefined(SearchState)) {
//                        SearchState.innerHTML = "";
//                        for (var i = 0; i < resM["Data"].length; i++) {
//                            let option = new Option(resM["Data"][i]["EnumName"], resM["Data"][i]["EnumValue"]);
//                            SearchState.options.add(option);
//                        }
//                    }
//                    PaperListModel.GetAllClassListStateInfo();
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
//         * 获得所有班级信息
//         */
//        private static GetAllClassListStateInfo() {
//            let url: string = "api/ClassList/GetAllClassListInfo";
//            let data = {
//            }
//            let SFun = function (resM: Object) {
//                if (resM["ResultType"] == 0) {
//                    let SearchClassList = MDMa.$("SearchClassList") as HTMLSelectElement;
//                    if (!MTMa.IsNullOrUndefined(SearchClassList)) {
//                        SearchClassList.innerHTML = "";
//                        let listM = resM["Data"];
//                        Materal.ArrayManager.ArrayInsert(listM, {
//                            ID: "null",
//                            Name: "所有班级",
//                            Rank: 0
//                        }, 0);
//                        for (var i = 0; i < listM.length; i++) {
//                            let option = new Option(listM[i]["Name"], listM[i]["ID"]);
//                            SearchClassList.options.add(option);
//                        }
//                        PaperListModel.GetList();
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
//        /**
//         * 获得列表信息
//         */
//        private static GetList() {
//            if (PaperListModel.PageSetting.PageCount >= PaperListModel.PageSetting.PageIndex) {
//                PaperListModel.PageSetting.IsLoading = true;
//                let url: string = "api/Paper/GetPaperInfoByWhere";
//                let data = {
//                    Title: (MDMa.$("SearchTitle") as HTMLInputElement).value,
//                    ClassListID: (MDMa.$("SearchClassList") as HTMLInputElement).value,
//                    State: (MDMa.$("SearchState") as HTMLInputElement).value,
//                    PageIndex: PaperListModel.PageSetting.PageIndex,
//                    PageSize: PaperListModel.PageSetting.PageSize,
//                };
//                let SFun = function (resM: Object) {
//                    if (resM["ResultType"] == 0) {
//                        let MainList = MDMa.$("MainList");
//                        if (!MTMa.IsNullOrUndefined(MainList)) {
//                            let listM = resM["Data"];
//                            for (var i = 0; i < listM.length; i++) {
//                                let ListItem = document.createElement("a");
//                                ListItem.href = "/Paper/PaperDetailed?ID=" + listM[i]["ID"];
//                                ListItem.classList.add("list-group-item");
//                                let titleSpan = document.createElement("span");
//                                titleSpan.classList.add("Title");
//                                titleSpan.textContent = listM[i]["Title"];
//                                let classSpan = document.createElement("span");
//                                classSpan.textContent = listM[i]["ClassListName"];
//                                let dateSpan = document.createElement("span");
//                                dateSpan.textContent = listM[i]["StartTimeStr"] + " 至 " + listM[i]["EndTimeStr"];
//                                let RightIco = document.createElement("i");
//                                MDMa.AddClass(RightIco, "list-right glyphicon glyphicon-chevron-right");
//                                ListItem.appendChild(titleSpan);
//                                ListItem.appendChild(classSpan);
//                                ListItem.appendChild(dateSpan);
//                                ListItem.appendChild(RightIco);
//                                MainList.appendChild(ListItem);
//                            }
//                        }
//                        PaperListModel.PageSetting.PageCount = parseInt(resM["PagingInfo"]["PagingCount"]);
//                        PaperListModel.PageSetting.PageIndex++;
//                    }
//                    else {
//                        Common.ShowMessageBox(resM["Message"])
//                    }
//                };
//                let EFun = function (xhr: XMLHttpRequest, resM: Object) {
//                };
//                let CFun = function (resM: Object) {
//                    PaperListModel.PageSetting.IsLoading = false;
//                };
//                Common.SendAjax(url, data, SFun, EFun, CFun);
//            }
//        }
//    }
//    /*页面加载完毕事件*/
//    MDMa.AddEvent(window, "load", function (e: Event) {
//        let pageM: PaperListModel = new PaperListModel();
//    });
//}