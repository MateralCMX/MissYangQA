/// <reference path="../jquery.d.ts" />
/// <reference path="../base.ts" />
'use strict';
namespace MissYangQA {
    class ClassListPage {
        private static PageData = {
            Data: []
        };
        private static PageSetting = {
            ChangeRanking: false
        };
        /**
         * 构造方法
         */
        constructor() {
            if (common.IsLogin(true)) {
                this.BindEvent();
                ClassListPage.GetList();
            }
        }
        /**
         * 绑定事件
         */
        private BindEvent() {
        }
        /**
         * 获得列表信息
         */
        private static GetList() {
            let url: string = "api/ClassList/GetAllClassListInfo";
            let data = {}
            let SFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                ClassListPage.PageData.Data = resM["Data"];
                let MainList = MDMa.$("MainList");
                if (!MTMa.IsNullOrUndefined(MainList)) {
                    MainList.innerHTML = "";
                    for (var i = 0; i < resM["Data"].length; i++) {
                        let listItem = document.createElement("li");
                        listItem.dataset.id = resM["Data"][i]["ID"];
                        listItem.classList.add("list-group-item");
                        MDMa.AddEvent(listItem, "click", ClassListPage.ClassItemEvent_Click);
                        let textContent = document.createTextNode(resM["Data"][i]["Rank"] + "：" + resM["Data"][i]["Name"]);
                        let btnGroup = document.createElement("div");
                        MDMa.AddClass(btnGroup, "upDownBtnGroup btn-group btn-group-xs");
                        btnGroup.setAttribute("role", "group");
                        btnGroup.dataset.index = i.toString();
                        let upBtn = document.createElement("button");
                        MDMa.AddClass(upBtn, "btn btn-info glyphicon glyphicon-arrow-up");
                        btnGroup.appendChild(upBtn);
                        upBtn.dataset.target = "-1";
                        MDMa.AddEvent(upBtn, "click", ClassListPage.BtnChangeClassListRankEvent_Click);
                        let downBtn = document.createElement("button");
                        MDMa.AddClass(downBtn, "btn btn-info glyphicon glyphicon-arrow-down");
                        btnGroup.appendChild(downBtn);
                        downBtn.dataset.target = "1";
                        MDMa.AddEvent(downBtn, "click", ClassListPage.BtnChangeClassListRankEvent_Click);
                        let RightIco = document.createElement("i");
                        MDMa.AddClass(RightIco, "list-right glyphicon glyphicon-chevron-right");
                        listItem.appendChild(textContent);
                        listItem.appendChild(btnGroup);
                        listItem.appendChild(RightIco);
                        MainList.appendChild(listItem);
                    }
                }
            };
            let FFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                common.ShowMessageBox(resM["Message"])
            };
            let CFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
            };
            common.SendGetAjax(url, data, SFun, FFun, CFun);
        }
        /**
         * 班级单项单击事件
         * @param e
         */
        private static ClassItemEvent_Click(e: MouseEvent) {
            let classItem = e.target as HTMLElement;
            while (classItem.tagName != "LI") {
                classItem = classItem.parentElement;
            }
            window.location.href = "/ClassList/ClassListDetailed?ID=" + classItem.dataset.id;
        }
        /**
         * 调换班级位序单击事件
         * @param e
         */
        private static BtnChangeClassListRankEvent_Click(e: MouseEvent) {
            let btnElement = e.target as HTMLButtonElement;
            let btnGroup = btnElement.parentElement as HTMLDivElement;
            let index = parseInt(btnGroup.dataset.index);
            let targetIndex = index + parseInt(btnElement.dataset.target);
            if (targetIndex >= 0 && targetIndex < ClassListPage.PageData.Data.length) {
                if (!ClassListPage.PageSetting.ChangeRanking) {
                    ClassListPage.PageSetting.ChangeRanking = true;
                    let data = {
                        ClassListID: ClassListPage.PageData.Data[index].ID,
                        TargetClassListID: ClassListPage.PageData.Data[targetIndex].ID,
                    }
                    let url: string = "api/ClassList/ChangeClassListRank";
                    let SFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                        ClassListPage.GetList();
                    };
                    let FFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                        common.ShowMessageBox(resM["Message"])
                    };
                    let CFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                        ClassListPage.PageSetting.ChangeRanking = false;
                    };
                    common.SendPostAjax(url, data, SFun, FFun, CFun);
                }
            }
            event.stopPropagation();
        }
    }
    /*页面加载完毕事件*/
    MDMa.AddEvent(window, "load", function (e: Event) {
        let pageM: ClassListPage = new ClassListPage();
    });
}