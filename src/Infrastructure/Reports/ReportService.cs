using Razor.Templating.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Entities.Orders;

namespace Infrastructure.Reports;
internal sealed class ReportService : IReportService
{
    public async Task<string> GenerateHtmlFromRazorPage<T>(string pageName, T model)
    {
        var html = await RazorTemplateEngine
            .RenderAsync($"~/{pageName}.cshtml", model);

        return html;
    }
}
