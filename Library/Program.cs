﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Library
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseKestrel()
                   .UseStartup<Startup>()
                   .UseIISIntegration()
                   .Build();
    }
}
