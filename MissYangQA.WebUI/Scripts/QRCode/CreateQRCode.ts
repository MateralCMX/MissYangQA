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
                this.GetAllClassInfo();
                this.GetAllEnablePaperInfo();
            }
        }
        /**
         * 绑定事件
         */
        private BindEvent() {
            MDMa.AddEvent("BtnCreateQRCode", "click", this.BtnCreateQRCodeEvent_Click);
        }
        /**
         * 创建二维码
         * @param e
         */
        private BtnCreateQRCodeEvent_Click(e: MouseEvent) {
            let QRCode = MDMa.$("QRCode") as HTMLDivElement;
            QRCode.innerHTML = "";
            let data = {
                ClassID: (MDMa.$("SelectClass") as HTMLSelectElement).value,
                PaperID: (MDMa.$("SelectPaper") as HTMLSelectElement).value
            }
            let src = common.ApplicationSettingM.DomainName + "api/ValidateCode/GetQRImage?ClassID=" + data.ClassID + "&PaperID=" + data.PaperID;
            let img = document.createElement("img");
            img.src = src;
            QRCode.appendChild(img);
        }
        /**
         * 获得所有班级信息
         */
        private GetAllClassInfo() {
            let url: string = "api/Class/GetAllClassInfo";
            let data = {}
            let SFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                ClassListPage.PageData.Data = resM["Data"];
                let SelectClass = MDMa.$("SelectClass") as HTMLSelectElement;
                if (!MTMa.IsNullOrUndefined(SelectClass)) {
                    SelectClass.innerHTML = "";
                    for (let i = 0; i < resM["Data"].length; i++) {
                        let option = new Option(resM["Data"][i]["Name"], resM["Data"][i]["ID"]);
                        SelectClass.options.add(option);
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
         * 获得所有启用的试题信息
         */
        private GetAllEnablePaperInfo() {
            let url: string = "api/Paper/GetAllEnablePaperInfo";
            let data = {}
            let SFun = function (resM: Object, xhr: XMLHttpRequest, state: number) {
                ClassListPage.PageData.Data = resM["Data"];
                let SelectPaper = MDMa.$("SelectPaper") as HTMLSelectElement;
                if (!MTMa.IsNullOrUndefined(SelectPaper)) {
                    SelectPaper.innerHTML = "";
                    for (let i = 0; i < resM["Data"].length; i++) {
                        let option = new Option(resM["Data"][i]["Title"], resM["Data"][i]["ID"]);
                        SelectPaper.options.add(option);
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
    }
    /*页面加载完毕事件*/
    MDMa.AddEvent(window, "load", function (e: Event) {
        let pageM: ClassListPage = new ClassListPage();
    });
}