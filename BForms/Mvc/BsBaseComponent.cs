﻿using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using BForms.Models;
using BForms.Renderers;
using System;

namespace BForms.Mvc
{
    public abstract class BsBaseComponent : BsBaseConfigurator, IHtmlString
    {
        #region Properties and Constructor
        internal string template;
        internal BsBaseRenderer renderer;

        internal IDictionary<string, object> htmlAttributes;
        internal Dictionary<string, object> options = new Dictionary<string, object>();


        internal IDictionary<string, object> HtmlAttr
        {
            get
            {
                return this.htmlAttributes;
            }
        }

        internal string TemplatePath
        {
            get
            {
                return this.template;
            }
        }

        public BsBaseComponent(){}

        public BsBaseComponent(ViewContext viewContext) : base(viewContext)
        {

        }
        #endregion

        #region Config
        public BsBaseComponent HtmlAttributes(IDictionary<string, object> htmlAttributes)
        {
            this.htmlAttributes = htmlAttributes;

            return this;
        }
        #endregion

        #region Helpers
        

        internal string RenderModel<T>(T model)
        {
            if (string.IsNullOrEmpty(this.template))
                throw new Exception("Template path is null or empty");

            if (model == null)
                throw new Exception("Model is null");

            if (this.viewContext == null)
                throw new Exception("View context is null");

            return this.viewContext.Controller.BsRenderPartialView(this.template, model);
        }

        internal T GetInstance<T>() where T : BsBaseComponent
        {
            return (T)this;
        }
        #endregion

        #region Options
        public void SetOption<T>(string key, T value)
        {
            if (this.options.ContainsKey(key))
            {
                this.options[key] = value;
            }
            else
            {
                this.options.Add(key,value);
            }
        }

        public void TryGetOption<T>(string key, out T value) where T : class
        {
            if (this.options.ContainsKey(key))
            {
                value = this.options[key] as T;
            }
            else
            {
                value = default(T);
            }
        }
        #endregion

        #region ToString ToHtmlString
        public override string ToString()
        {
            if (this.renderer == null)
                throw new Exception("Renderer is null");

            return this.renderer.Render();
        }

        public virtual string ToHtmlString()
        {
            if (this.viewContext == null)
                throw new Exception("View context is null");

            var writer = new System.IO.StringWriter();
            this.viewContext.Writer.Write(this.ToString());
            return writer.ToString();
        }
        #endregion
    }
}