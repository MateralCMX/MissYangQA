/// <reference path="../jquery.d.ts" />
/// <reference path="../base.ts" />
'use strict';
namespace MissYangQA {
    class PaperListModel {
        /*页面设置*/
        private static PageSetting = {
            IsLoading: false,
            OldScrollTop: 0,
            PageIndex: 1,
            PageSize: 10,
            PageCount: 99,
            SearchKey:"PaperListSearchKey"
        }
        /**
         * 构造方法
         */
        constructor() {
            if (common.IsLogin(true)) {
                this.BindEvent();
                common.BindSearchInfo(PaperListModel.PageSetting.SearchKey);
                PaperListModel.GetList();
            }
        }
        /**
         * 绑定事件
         */
        private BindEvent() {
            MDMa.AddEvent("BtnSearch", "click", this.BtnSearchEvent_Click);
            /*下拉加载*/
            MDMa.AddEvent(window, "scroll", function (e) {
                let viewH = common.GetClientHeight();//可见高度
                let contentH = Math.max(document.body.scrollHeight, document.documentElement.scrollHeight);//内容高度
                let scrollTop = common.GetScrollTop();//滚动条位置
                if (contentH - viewH - scrollTop <= 100 && !PaperListModel.PageSetting.IsLoading && PaperListModel.PageSetting.OldScrollTop < scrollTop) {
                    PaperListModel.GetList();
                }
                PaperListModel.PageSetting.OldScrollTop = scrollTop;
            });
        }
        /**
         * 查询按钮单击事件
         * @param e
         */
        private BtnSearchEvent_Click(e: MouseEvent) {
            PaperListModel.PageSetting.PageIndex = 1;
            PaperListModel.PageSetting.PageCount = 99;
            let MainList = MDMa.$("MainList");
            MainList.innerHTML = "";
            PaperListModel.GetList();
            $('#SearchModal').modal('toggle');
        }
        /**
         * 获得列表信息
         */
        private static GetList() {
            if (PaperListModel.PageSetting.PageCount >= PaperListModel.PageSetting.PageIndex) {
                PaperListModel.PageSetting.IsLoading = true;
                let url: string = "api/Paper/GetPaperInfoByWhere";
                let data = {
                    Title: (MDMa.$("SearchTitle") as HTMLInputElement).value,
                    IsEnable: (MDMa.$("SearchIsEnable") as HTMLInputElement).value,
                    PageIndex: PaperListModel.PageSetting.PageIndex,
                    PageSize: PaperListModel.PageSetting.PageSize,
                };
                let SFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                    common.SaveSearchInfo(PaperListModel.PageSetting.SearchKey, data);
                    let MainList = MDMa.$("MainList");
                    if (!MTMa.IsNullOrUndefined(MainList)) {
                        let listM = resM["Data"];
                        for (let i = 0; i < listM.length; i++) {
                            let ListItem = document.createElement("a");
                            ListItem.href = "/Paper/PaperDetails?ID=" + listM[i]["ID"];
                            ListItem.classList.add("list-group-item");
                            let titleSpan = document.createElement("span");
                            titleSpan.classList.add("Title");
                            titleSpan.textContent = listM[i]["Title"];
                            ListItem.appendChild(titleSpan);
                            let classSpan = document.createElement("span");
                            classSpan.textContent = listM[i]["ClassListName"];
                            ListItem.appendChild(classSpan);
                            let enableSpan = document.createElement("span");
                            enableSpan.textContent = "启用状态：" + (listM[i]["IsEnable"] ? "启用" : "禁用");
                            ListItem.appendChild(enableSpan);
                            let questionCountSpan = document.createElement("span");
                            questionCountSpan.textContent = "问题总数：" + listM[i]["ProblemCount"];
                            ListItem.appendChild(questionCountSpan);
                            let scoreCountSpan = document.createElement("span");
                            scoreCountSpan.textContent = "总分：" + listM[i]["ProblemCount"];
                            ListItem.appendChild(scoreCountSpan);
                            let RightIco = document.createElement("i");
                            MDMa.AddClass(RightIco, "list-right glyphicon glyphicon-chevron-right");
                            ListItem.appendChild(RightIco);
                            MainList.appendChild(ListItem);
                        }
                    }
                    PaperListModel.PageSetting.PageCount = parseInt(resM["PagingInfo"]["PagingCount"]);
                    PaperListModel.PageSetting.PageIndex++;
                };
                let FFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                    common.ShowMessageBox(resM["Message"])
                };
                let CFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                    PaperListModel.PageSetting.IsLoading = false;
                };
                common.SendGetAjax(url, data, SFun, FFun, CFun);
            }
        }
    }
    /*页面加载完毕事件*/
    MDMa.AddEvent(window, "load", function (e: Event) {
        let pageM: PaperListModel = new PaperListModel();
    });
}