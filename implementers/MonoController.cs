using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using splendor.net5.core.commons;
using splendor.net5.core.implementers;
using splendor.net5.mvc.contracts;

namespace splendor.net5.mvc.implementers
{
    public abstract class MonoController<E, K, TO, S> : Controller
        where E : class, new()
        where TO : TObject
        where S : Service<E, K, TO>
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly IActionContextAccessor _actionContextAccessor;
        protected readonly S _service;
        protected readonly IMVCTracer _tracer;

        public MonoController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _actionContextAccessor = _serviceProvider.GetRequiredService<IActionContextAccessor>();
            _service = _serviceProvider.GetRequiredService<S>();
            _tracer = _serviceProvider.GetRequiredService<DefaultMVCTracer>();
        }

        public MonoController(IServiceProvider serviceProvider, Type typeTracer)
        {
            _serviceProvider = serviceProvider;
            _actionContextAccessor = _serviceProvider.GetRequiredService<IActionContextAccessor>();
            _service = _serviceProvider.GetRequiredService<S>();
            _tracer = _serviceProvider.GetRequiredService(typeTracer) as IMVCTracer;
        }

        [HttpGet]
        #pragma warning disable CS1998
        public virtual async Task<IActionResult> List()
        {
            return View($"_List{_actionContextAccessor.ActionContext.RouteData.Values["controller"]}");
        }

        [HttpPost]
        public virtual async Task<IActionResult> Page(DPagination pagination)
        {
            using (_service)
            {
                return Json(await _service.Page(pagination));
            }
        }

        [HttpGet]
        public virtual async Task<IActionResult> Get(K id)
        {
            using (_service)
            {
                return Json(await _service.GetTO(id));
            }
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {
            using (_service)
            {
                return Json(await _service.AllTO());
            }
        }

        [HttpGet]
        #pragma warning disable CS1998
        public virtual async Task<IActionResult> Add(bool? modal = false)
        {
            ViewData["isModal"] = modal;
            return View($"_Add{_actionContextAccessor.ActionContext.RouteData.Values["controller"]}");
        }

        [HttpPost]
        public virtual async Task<IActionResult> Add(TO to)
        {
            using (_service)
            {
                return Json(await _service.AddTO(to));
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> AddReturn(TO to)
        {
            using (_service)
            {
                return Json(await _service.AddTO(to, true));
            }
        }

        [HttpGet]
        #pragma warning disable CS1998
        public virtual async Task<IActionResult> Edit(K id, bool? modal = false)
        {
            using (_service)
            {
                ReplyTO<E, TO> reply = await _service.GetTO(id);
                if (reply.Success)
                {
                    ViewData["isModal"] = modal;
                    return View($"_Edit{_actionContextAccessor.ActionContext.RouteData.Values["controller"]}", reply);
                }
                return Json(reply);
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> Edit(K id, TO to)
        {
            using (_service)
            {
                return Json(await _service.EditTO(id, to));
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> Remove(K id, TO to)
        {
            using (_service)
            {
                return Json(await _service.RemoveTO(id, to));
            }
        }

        [HttpGet]
        public virtual async Task<IActionResult> PrintList(DPagination pagination)
        {
            using (_service)
            {
                ReplyTO<E, TO> reply = await _service.PageTO(pagination);
                reply.Trace = _tracer.Trace(User);
                return PartialView($"Print{_actionContextAccessor.ActionContext.RouteData.Values["controller"]}", reply);
            }
        }

        [HttpGet]
        public virtual async Task<IActionResult> PrintTO(K id)
        {
            ReplyTO<E, TO> reply = await _service.GetTO(id);
            if (reply.Success)
            {
                reply.Trace = _tracer.Trace(User);
                return PartialView($"PrintTO{_actionContextAccessor.ActionContext.RouteData.Values["controller"]}", reply);
            }
            return Json(reply);
        }
    }
}