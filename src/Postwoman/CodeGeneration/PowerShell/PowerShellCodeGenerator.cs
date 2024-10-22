using Postwoman.Models.PwRequest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Postwoman.CodeGeneration.PowerShell;

public class PowerShellCodeGenerator : ICodeGenerator
{

    public string Generate(CollectionViewModel collection, RequestViewModel request)
    {
        var code = new StringBuilder();

        var isBasicAuth = request.Authorization.Type == "Basic";
        if (isBasicAuth)
        {
            code.AppendLine("$credential = Get-Credential");
            code.AppendLine("$authHeader = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes((\"{0}:{1}\" -f $credential.UserName, $credential.GetNetworkCredential().Password)))");
            code.AppendLine();
        }

        var variables = VariableCompiler.Compile(collection.SelectedEnvironment.VariableGroup);
        var isBearerAuth = request.Authorization.Type == "Bearer";
        if (isBearerAuth)
        {
            var bearerToken = VariableReplacer.Replace(request.Authorization.BearerToken, variables);
            code.AppendLine($"$bearerToken = \"{Escape(bearerToken)}\"");
            code.AppendLine();
        }

        var hasHeaders = collection.Headers.Count > 0 || request.Headers.Count > 0;
        var isApiKeyAuth = request.Authorization.Type == "ApiKey";
        if (hasHeaders || isBasicAuth || isBearerAuth || isApiKeyAuth)
        {
            code.AppendLine("$headers = @{");
            if (isBasicAuth)
            {
                code.AppendLine("    \"Authorization\" = (\"Basic {0}\" -f $authHeader)");
            }
            if (isBearerAuth)
            {
                code.AppendLine("    \"Authorization\" = (\"Bearer {0}\" -f $bearerToken)");
            }
            if (isApiKeyAuth)
            {
                var headerName = VariableReplacer.Replace(request.Authorization.ApiKeyHeaderName, variables);
                var headerValue = VariableReplacer.Replace(request.Authorization.ApiKeyValue, variables);
                code.AppendLine($"    \"{Escape(headerName)}\" = \"{Escape(headerValue)}\"");
            }
            foreach (var header in GetHeaders(collection, request))
            {
                var headerName = VariableReplacer.Replace(header.Name, variables);
                var headerValue = VariableReplacer.Replace(header.Value, variables);
                code.AppendLine($"    \"{Escape(headerName)}\" = \"{Escape(headerValue)}\"");
            }
            code.AppendLine("}");
            code.AppendLine();
        }

        code.Append("Invoke-RestMethod");
        code.Append($" -Method {request.Method}");
        code.Append($" -Uri \"{UrlTools.GetFullUrl(collection, request)}\"");

        if (hasHeaders || isBasicAuth)
        {
            code.Append(" -Headers $headers");
        }

        if (request.Body != null)
        {
            var contentType = ContentTypeCalculator.Analyze(request.Body);
            if (contentType == "application/json")
            {
                code.Append($" -ContentType \"{contentType}\"");
            }

            var body = VariableReplacer.Replace(request.Body, variables);
            if (!string.IsNullOrWhiteSpace(body))
            {
                code.Append($" -Body @'{Environment.NewLine}{body}{Environment.NewLine}'@");
            }
        }
        return code.ToString();
    }

    private static List<RequestHeaderViewModel> GetHeaders(CollectionViewModel collection, RequestViewModel request)
    {
        var result = new List<RequestHeaderViewModel>();
        result.AddRange(collection.Headers);
        result.AddRange(request.Headers);
        return result;
    }

    private static string Escape(string text)
    {
        return text?.Replace("\"", "`\"") ?? string.Empty;
    }

}
