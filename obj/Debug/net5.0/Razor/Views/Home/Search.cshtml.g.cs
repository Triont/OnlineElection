#pragma checksum "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Home\Search.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0a218c504c6637e6c0b95667e210a08e7d6e49fb"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Search), @"mvc.1.0.view", @"/Views/Home/Search.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\_ViewImports.cshtml"
using OnlineElection;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Home\Search.cshtml"
using OnlineElection.ModelView;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Home\Search.cshtml"
using OnlineElection.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0a218c504c6637e6c0b95667e210a08e7d6e49fb", @"/Views/Home/Search.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8c96bc6760a3e74cf247cde4a95df60a520311f9", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Search : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<Election>>
    {
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n\r\n");
            WriteLiteral("<div class=\"text-center\">\r\n    <h1 class=\"display-4\">Welcome</h1>\r\n    <p>Learn about <a href=\"https://docs.microsoft.com/aspnet/core\">building Web apps with ASP.NET Core</a>.</p>\r\n</div>\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "0a218c504c6637e6c0b95667e210a08e7d6e49fb3542", async() => {
                WriteLiteral("\r\n\r\n<div>\r\n");
#nullable restore
#line 21 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Home\Search.cshtml"
     if( TempData["Name"]!=null)
    {

#line default
#line hidden
#nullable disable
                WriteLiteral("         <p>");
#nullable restore
#line 23 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Home\Search.cshtml"
       Write(TempData["Name"]);

#line default
#line hidden
#nullable disable
                WriteLiteral("</p>\r\n");
#nullable restore
#line 24 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Home\Search.cshtml"

    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 26 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Home\Search.cshtml"
     if(User.Identity.IsAuthenticated)
    {

#line default
#line hidden
#nullable disable
                WriteLiteral("        <a href=\"/Logout\"> Log out</a>\r\n");
#nullable restore
#line 29 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Home\Search.cshtml"
        
    }

#line default
#line hidden
#nullable disable
                WriteLiteral("  \r\n</div>\r\n\r\n<div class =\"table\">\r\n<div align=\"center\">\r\n");
#nullable restore
#line 36 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Home\Search.cshtml"
     if(Model!=null)
    {

#line default
#line hidden
#nullable disable
                WriteLiteral(@"         <table class=""table table-striped"">
              <thead>
    <tr>
    
      <th scope=""col"">Id</th>
      <th scope=""col"">Name</th>
       <th scope=""col"">Vote</th>
        <th scope=""col"">Result</th>
  
    </tr>
  </thead>
        
");
#nullable restore
#line 50 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Home\Search.cshtml"
         foreach(var i in Model)
        {
          

#line default
#line hidden
#nullable disable
                WriteLiteral("               <tr>\r\n                   \r\n                   <td> ");
#nullable restore
#line 55 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Home\Search.cshtml"
                   Write(i.Id);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                   <td>");
#nullable restore
#line 56 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Home\Search.cshtml"
                  Write(i.Name);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                 \r\n");
#nullable restore
#line 58 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Home\Search.cshtml"
                       if(@i.Status=="Archived")
                      {

#line default
#line hidden
#nullable disable
                WriteLiteral("                         <td>\r\n                         \r\n                        Archived\r\n                          \r\n                       </td>   \r\n");
#nullable restore
#line 65 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Home\Search.cshtml"
                      }
                      else
                      {

#line default
#line hidden
#nullable disable
                WriteLiteral("                       <td>\r\n                           <a");
                BeginWriteAttribute("href", " href=\"", 1613, "\"", 1640, 2);
                WriteAttributeValue("", 1620, "/Election/Show/", 1620, 15, true);
#nullable restore
#line 69 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Home\Search.cshtml"
WriteAttributeValue("", 1635, i.Id, 1635, 5, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(">\r\n                        \r\n                           Vote!</a>\r\n                       </td>\r\n");
#nullable restore
#line 73 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Home\Search.cshtml"
                            }

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                            <td>\r\n                               <a");
                BeginWriteAttribute("href", " href=\"", 1838, "\"", 1867, 2);
                WriteAttributeValue("", 1845, "/Election/Result/", 1845, 17, true);
#nullable restore
#line 76 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Home\Search.cshtml"
WriteAttributeValue("", 1862, i.Id, 1862, 5, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(">\r\n                                 Result\r\n                                </a>\r\n                            </td>\r\n                    \r\n                \r\n               </tr>\r\n");
#nullable restore
#line 83 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Home\Search.cshtml"

           
                
        }

#line default
#line hidden
#nullable disable
                WriteLiteral("           </table>\r\n");
#nullable restore
#line 88 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Home\Search.cshtml"
    }

#line default
#line hidden
#nullable disable
                WriteLiteral("    </div>\r\n\r\n</div>\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<Election>> Html { get; private set; }
    }
}
#pragma warning restore 1591
