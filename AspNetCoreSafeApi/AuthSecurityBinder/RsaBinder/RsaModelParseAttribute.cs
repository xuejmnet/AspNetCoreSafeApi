using System;
using AspNetCoreSafeApi.AuthSecurityBinder.RsaBinder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AuthSecurity.AspNetCore.NettyClientProvider.AuthSecurityBinder.RsaBinder
{
/*
* @Author: xjm
* @Description:
* @Date: Thursday, 19 November 2020 15:10:22
* @Email: 326308290@qq.com
*/
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class RsaModelParseAttribute : Attribute, IBinderTypeProviderMetadata, IBindingSourceMetadata, IModelNameProvider
    {
        private readonly ModelBinderAttribute modelBinderAttribute = new ModelBinderAttribute() { BinderType = typeof(EncryptBodyModelBinder) };

        public BindingSource BindingSource => modelBinderAttribute.BindingSource;

        public string Name => modelBinderAttribute.Name;

        public Type BinderType => modelBinderAttribute.BinderType;
    }
}