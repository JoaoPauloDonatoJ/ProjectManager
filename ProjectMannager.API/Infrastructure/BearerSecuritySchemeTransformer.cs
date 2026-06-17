using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace ProjectMannager.API.Infrastructure;

// 1. Transformer para registrar o esquema global do Token JWT
public sealed class BearerSecuritySchemeTransformer(IAuthenticationSchemeProvider authProvider)
    : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken ct)
    {
        var schemes = await authProvider.GetAllSchemesAsync();

        if (schemes.Any(s => s.Name == "Bearer"))
        {
            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

            document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "Insira o token JWT gerado no login."
            };
        }
        await Task.CompletedTask;
    }
}

// 2. Transformer inteligente para mapear o [Authorize] por endpoint automaticamente
public sealed class AuthOperationTransformer : IOpenApiOperationTransformer
{
    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        // O .NET 10 fornece acesso direto aos metadados do endpoint atual
        var metadata = context.Description.ActionDescriptor.EndpointMetadata;

        var hasAuthorize = metadata.Any(m => m is AuthorizeAttribute);
        var hasAllowAnonymous = metadata.Any(m => m is AllowAnonymousAttribute);

        // Se exigir autenticação e NÃO for explicitamente pública (AllowAnonymous)
        if (hasAuthorize && !hasAllowAnonymous)
        {
            operation.Security ??= [];
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                // Vincula o endpoint ao esquema "Bearer" registrado globalmente
                [new OpenApiSecuritySchemeReference("Bearer", context.Document)] = []
            });
        }

        return Task.CompletedTask;
    }
}
