using System;
using System.Collections.Generic;
$if$ ($targetframeworkversion$ >= 3.5)using System.Linq;
$endif$using System.Text;

namespace $safeprojectname$
{

using imbACE.Core.application;
using imbACE.Services.application;
using $rootnamespace$.console;


public class $fileinputname$Application : aceConsoleApplication<$fileinputname$Console>
{

    public static void Main(string[] args)
    {
        var application = new $fileinputname$Application();

        application.StartApplication(args);

        // here you may place code that has to be performed after user called application closing
        // ...
    }

    
    public override void setAboutInformation()
    {
    // Insert proper information here
    appAboutInfo = new aceApplicationInfo
    {
        software = "$app_software$", // $safeprojectname$ Application",
        copyright = "$app_copyright$", // "Copyright © $registeredorganization$ $year$",
        organization = "$registeredorganization$",
        author = " - [author ]-",
        license = "$app_license$", //Licensed under GNU General Public License v3.0",
        applicationVersion = "0.1v",
        welcomeMessage =  "This is $safeprojectname$ console application."
        };
    }
}

}
