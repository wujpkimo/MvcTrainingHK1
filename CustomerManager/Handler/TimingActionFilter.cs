using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CustomerManager.Handler
{
    public class TimingActionFilter : ActionFilterAttribute
    {
        //Action開始執行
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            GetTimer(filterContext, "action").Start();
            base.OnActionExecuting(filterContext);
        }

        //Action執行結束
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            GetTimer(filterContext, "action").Stop();
            base.OnActionExecuted(filterContext);
        }

        //Action Result開始
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            GetTimer(filterContext, "render").Start();
            base.OnResultExecuting(filterContext);
        }

        //Action Result結束
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var renderTimer = GetTimer(filterContext, "render");
            renderTimer.Stop();
            var actionTimer = GetTimer(filterContext, "action");
            //if (actionTimer.ElapsedMilliseconds >= 100 || renderTimer.ElapsedMilliseconds >= 100)
            //{
            Debug.WriteLine("Monitor(" + filterContext.RouteData.Values["controller"] + ")", String.Format(
                    "【{0}】-【{1}】,Execute:{2}ms,Render:{3}ms",
                    filterContext.RouteData.Values["controller"],
                    filterContext.RouteData.Values["action"],
                    actionTimer.ElapsedMilliseconds,
                    renderTimer.ElapsedMilliseconds
                ));
            //}
            base.OnResultExecuted(filterContext);
        }

        private Stopwatch GetTimer(ControllerContext context, string name)
        {
            string key = "__timer__" + name;
            if (context.HttpContext.Items.Contains(key))
            {
                return (Stopwatch)context.HttpContext.Items[key];
            }

            var result = new Stopwatch();
            context.HttpContext.Items[key] = result;
            return result;
        }
    }
}