/// <reference path="../jquery.d.ts" />
/// <reference path="../base.ts" />
'use strict';
namespace MissYangQA {
    class ProblemListPage {
        /*页面数据*/
        private static PageData = {
            params: MTMa.GetURLParams()
        }
        /**
         * 构造方法
         */
        constructor() {
            if (common.IsLogin(true)) {
                if (!MTMa.IsNullOrUndefinedOrEmpty(ProblemListPage.PageData.params["PaperID"])) {
                    this.BindEvent();
                    ProblemListPage.GetList();
                    let BtnAdd = MDMa.$("BtnAdd") as HTMLAnchorElement;
                    BtnAdd.href = "/Problem/ProblemDetails?PaperID=" + ProblemListPage.PageData.params["PaperID"];
                }
                else {
                    window.history.back();
                }
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
            let url: string = "api/Problem/GetProblemInfoByPaperID";
            let data = {
                PaperID: ProblemListPage.PageData.params["PaperID"],
            };
            let SFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                let MainList = MDMa.$("MainList");
                if (!MTMa.IsNullOrUndefined(MainList)) {
                    let listM = resM["Data"];
                    for (let i = 0; i < listM.length; i++) {
                        let ListItem = document.createElement("a");
                        ListItem.href = "/Problem/ProblemDetails?PaperID=" + data.PaperID + "&ID=" + listM[i]["ID"];
                        ListItem.classList.add("list-group-item");
                        let ContentP = document.createElement("p");
                        ContentP.textContent = listM[i]["Contents"];
                        ListItem.appendChild(ContentP);
                        let Score = document.createElement("span");
                        Score.textContent = "分值:" + listM[i]["Score"] + "分";
                        ListItem.appendChild(Score);
                        for (var j = 0; j < listM[i]["Answers"].length; j++) {
                            let Answer = document.createElement("span");
                            Answer.textContent = listM[i]["Answers"][j]["Contents"];
                            if (listM[i]["Answers"][j]["IsCorrect"]) {
                                Answer.classList.add("Correct");
                            }
                            ListItem.appendChild(Answer);
                        }
                        let RightIco = document.createElement("i");
                        MDMa.AddClass(RightIco, "list-right glyphicon glyphicon-chevron-right");
                        ListItem.appendChild(RightIco);
                        MainList.appendChild(ListItem);
                    };
                }
            }
            let FFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                common.ShowMessageBox(resM["Message"])
            };
            let CFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
            };
            common.SendGetAjax(url, data, SFun, FFun, CFun);
        }
    }
    /*页面加载完毕事件*/
    MDMa.AddEvent(window, "load", function (e: Event) {
        let pageM: ProblemListPage = new ProblemListPage();
    });
}