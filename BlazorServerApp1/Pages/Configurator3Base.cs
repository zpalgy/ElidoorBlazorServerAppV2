using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorServerApp1.Data;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace BlazorServerApp1.Pages
{
    public class Configurator3Base : ComponentBase
    {
        [Inject]
        public ProtectedSessionStorage ProtectedSessionStore { get; set; }

        protected void OnAfterRenderAsync(EventArgs e)
        {

        }


    }
}
