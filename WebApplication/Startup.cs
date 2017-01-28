using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FileLibrary;
using System.IO;

namespace WebApplication
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                if (context.Request.Query["filename"].Count != 0)
                {
                    try
                    {
                        FileModel fm = FileService.FileMetadata(context.Request.Query["filename"]);
                        await context.Response.WriteAsync("<html>");
                        await context.Response.WriteAsync("<head><style>");
                        await context.Response.WriteAsync("table {font - family: arial, sans - serif;border - collapse: collapse;width: 100 %;}");
                        await context.Response.WriteAsync("td, th {border: 1px solid #dddddd;text - align: left;padding: 8px;}");
                        await context.Response.WriteAsync("tr: nth - child(even) {ackground - color: #dddddd;}");
                        await context.Response.WriteAsync("</style></head>");
                        await context.Response.WriteAsync("<table>");
                        await context.Response.WriteAsync("<tr><th>Full Name</th><th>Name</th><th>Extension</th><th>Length</th><th>Creation Time</th></tr>");
                        await context.Response.WriteAsync("<tr><td>" + fm.FullName + "</td><td>" + fm.Name + "</td><td>" + fm.Extension + "</td><td>" + fm.Length + "</td><td>" + fm.CreationTime + "</td></tr>");
                        await context.Response.WriteAsync("</table>");
                        await context.Response.WriteAsync("</body></html>");
                    }
                    catch (UnauthorizedAccessException)
                    {
                        await context.Response.WriteAsync("Access to file is denied.");
                    }
                    catch (FileNotFoundException)
                    {
                        await context.Response.WriteAsync("File does not exist.");
                    }
                }
                else
                {
                    // Nowa metoda
                    Tree tree = new Tree(new string[] { Directory.GetCurrentDirectory() });
                    tree.StartBuildTree();
                    List<Tree.DataTree> list = tree.ReturnDataTree();
                    await context.Response.WriteAsync("<html>");
                    await context.Response.WriteAsync("<head><style>");
                    await context.Response.WriteAsync("table {font - family: arial, sans - serif;border - collapse: collapse;width: 100 %;}");
                    await context.Response.WriteAsync("td, th {border: 1px solid #dddddd;text - align: left;padding: 8px;}");
                    await context.Response.WriteAsync("tr: nth - child(even) {ackground - color: #dddddd;}");
                    await context.Response.WriteAsync("</style></head>");
                    await context.Response.WriteAsync("<table>");
                    await context.Response.WriteAsync("<tr><th>Name</th><th>Full Name</th><th>Extension</th><th>Length</th><th>Creation Time</th></tr>");
                    for (int i = 0; i < list.Count; i++)
                    {
                        await context.Response.WriteAsync("<tr><td style=\"padding-left:" + (list[i].DataWithIndent.Count(f => f == ' ') * 4 + 8).ToString() + "px;\">" + list[i].DataWithIndent + "</td><td>" + list[i].DataFile.FullName + "</td><td>" + list[i].DataFile.Extension + "</td><td>" + list[i].DataFile.Length.ToString() + "</td><td>" + list[i].DataFile.CreationTime + "</td></tr>");
                    }
                    await context.Response.WriteAsync("</table>");
                    await context.Response.WriteAsync("</body></html>");
                }

                /* Stara metoda
                Tree tree = new Tree(new string[] { Directory.GetCurrentDirectory() });
                tree.StartBuildTree();
                await context.Response.WriteAsync(tree.PrintTreeDataInWeb());
                */
            });
        }
    }
}
