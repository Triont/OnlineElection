#pragma checksum "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Election\Result.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "48772d65e211f1124d71493f5b210d4ddf99f76c"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Election_Result), @"mvc.1.0.view", @"/Views/Election/Result.cshtml")]
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
#line 2 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\_ViewImports.cshtml"
using OnlineElection.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Election\Result.cshtml"
using OnlineElection.ModelView;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"48772d65e211f1124d71493f5b210d4ddf99f76c", @"/Views/Election/Result.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8c96bc6760a3e74cf247cde4a95df60a520311f9", @"/Views/_ViewImports.cshtml")]
    public class Views_Election_Result : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ResultView>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("<div>\r\n\r\n  \r\n    \r\n    <div style=\"display: inline-block\">\r\n\r\n\r\n");
#nullable restore
#line 16 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Election\Result.cshtml"
         for(int i=0;i<@Model.Names.Count;i++)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <div style=\"display: inline-block\">\r\n\r\n                    ");
#nullable restore
#line 20 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Election\Result.cshtml"
               Write(Model.Names[i]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" \r\n                    ");
#nullable restore
#line 21 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Election\Result.cshtml"
               Write(Model.Votes[i]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" votes and\r\n");
#nullable restore
#line 22 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Election\Result.cshtml"
                     if(@Double.IsNaN(Model.Percents[0]))
                    {                      
                   

#line default
#line hidden
#nullable disable
            WriteLiteral("0%");
#nullable restore
#line 24 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Election\Result.cshtml"
                                  
                  
                    }
                    else
                    {                   
                   

#line default
#line hidden
#nullable disable
            WriteLiteral(" ");
#nullable restore
#line 29 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Election\Result.cshtml"
                     Write(Model.Percents[i]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" %");
#nullable restore
#line 29 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Election\Result.cshtml"
                                                     
                  
                    

                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("                    \r\n                 \r\n\r\n                    </div>\r\n                 <br/>\r\n");
#nullable restore
#line 39 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Election\Result.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("       </div>\r\n       <br/>\r\n       <div>\r\n");
#nullable restore
#line 43 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Election\Result.cshtml"
          if(Model.Active)
         {
             

#line default
#line hidden
#nullable disable
#nullable restore
#line 45 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Election\Result.cshtml"
              if(Double.IsNaN( Model.Percents[0]))
             {

#line default
#line hidden
#nullable disable
            WriteLiteral("                  <p>   ");
#nullable restore
#line 47 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Election\Result.cshtml"
                   Write(Model.Names[0]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" is winning with ");
#nullable restore
#line 47 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Election\Result.cshtml"
                                                   Write(Model.Votes[0]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" and 0 % now</p>\r\n");
#nullable restore
#line 48 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Election\Result.cshtml"
             }
             else
             {

#line default
#line hidden
#nullable disable
            WriteLiteral("          <p>   ");
#nullable restore
#line 51 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Election\Result.cshtml"
           Write(Model.Names[0]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" is winning with ");
#nullable restore
#line 51 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Election\Result.cshtml"
                                           Write(Model.Votes[0]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" and ");
#nullable restore
#line 51 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Election\Result.cshtml"
                                                               Write(Model.Percents[0]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" % now</p>\r\n");
#nullable restore
#line 52 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Election\Result.cshtml"
          }

#line default
#line hidden
#nullable disable
#nullable restore
#line 52 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Election\Result.cshtml"
           
         }
         else
         {
             if(Double.IsNaN(Model.Percents[0]))
             {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <p>  Winner is ");
#nullable restore
#line 58 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Election\Result.cshtml"
                      Write(Model.Names[0]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" with ");
#nullable restore
#line 58 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Election\Result.cshtml"
                                           Write(Model.Votes[0]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" votes and 0 %</p>\r\n");
#nullable restore
#line 59 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Election\Result.cshtml"
            }
            else
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                  <p>  Winner is ");
#nullable restore
#line 62 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Election\Result.cshtml"
                            Write(Model.Names[0]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" with ");
#nullable restore
#line 62 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Election\Result.cshtml"
                                                 Write(Model.Votes[0]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" votes and ");
#nullable restore
#line 62 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Election\Result.cshtml"
                                                                           Write(Model.Percents[0]);

#line default
#line hidden
#nullable disable
            WriteLiteral("%</p>\r\n");
#nullable restore
#line 63 "C:\Users\user\source\repos\OnlineElection_02\OnlineElection\Views\Election\Result.cshtml"
        
            }
         }

#line default
#line hidden
#nullable disable
            WriteLiteral("          \r\n       </div>\r\n\r\n        \r\n      \r\n\r\n</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ResultView> Html { get; private set; }
    }
}
#pragma warning restore 1591
