using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnlineElection.ModelView
{
    public static class Helper
    {
        public static ResultView GetJson(this IHtmlHelper html, string json)
        {
            return JsonConvert.DeserializeObject<ResultView>(json);
        }
    }
}
