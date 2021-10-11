using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace AspNetCoreSafeApi.SecurityAuthorization.RsaChecker
{
    public static class AuthSecurityRsaExtension
    {
        public static AuthenticationBuilder AddAuthSecurityRsa(this AuthenticationBuilder builder)
            => builder.AddAuthSecurityRsa(AuthSecurityRsaDefaults.AuthenticationScheme, _ => { });

        public static AuthenticationBuilder AddAuthSecurityRsa(this AuthenticationBuilder builder, Action<AuthSecurityRsaOptions> configureOptions)
            => builder.AddAuthSecurityRsa(AuthSecurityRsaDefaults.AuthenticationScheme, configureOptions);

        public static AuthenticationBuilder AddAuthSecurityRsa(this AuthenticationBuilder builder, string authenticationScheme, Action<AuthSecurityRsaOptions> configureOptions)
            => builder.AddAuthSecurityRsa(authenticationScheme, displayName: null, configureOptions: configureOptions);

        public static AuthenticationBuilder AddAuthSecurityRsa(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<AuthSecurityRsaOptions> configureOptions)
        {
            return builder.AddScheme<AuthSecurityRsaOptions, AuthSecurityRsaAuthenticationHandler>(authenticationScheme, displayName, configureOptions);
        }
    }
}
