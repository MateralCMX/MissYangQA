/// <reference path="../jquery.d.ts" />
/// <reference path="../base.ts" />
'use strict';
namespace MissYangQA {
    class IndexPage {
        /**
         * 构造方法
         */
        constructor() {
            if (common.IsLogin(true)) {
                this.BindEvent();
            }
        }
        /**
         * 绑定事件
         */
        private BindEvent() {
            MDMa.AddEvent("BtnLogOut", "click", this.BtnLogOutEvent_Click);
        }
        /**
         * 登出按钮单击事件
         * @param e
         */
        private BtnLogOutEvent_Click(e: MouseEvent) {
            common.RemoveLoginUserInfo();
            common.GoToPage("Login");
        }
    }
    /*页面加载完毕事件*/
    MDMa.AddEvent(window, "load", function (e: Event) {
        let pageM: IndexPage = new IndexPage();
    });
}