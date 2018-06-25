using System;
using System.Collections.Generic;
$if$ ($targetframeworkversion$ >= 3.5)using System.Linq;
$endif$using System.Text;

namespace $safeprojectname$
{

using imbACE.Core.application;
using imbACE.Services.application;
using $safeprojectname$.console;


public class $safeprojectname$Application : aceConsoleApplication<$safeprojectname$Console>
{

    public static void Main(string[] args)
    {
        var application = new $safeprojectname$Application();

        application.StartApplication(args);

        // here you may place code that has to be performed after user called application closing
        // ...
    }

    
    public override void setAboutInformation()
    {
    // Insert proper information here
    appAboutInfo = new aceApplicationInfo(); // parametarless constructor takes values from Assembly attributes 
    appAboutInfo.license = "Licensed under GNU General Public License v3.0";
    appAboutInfo.welcomeMessage = "imbACE Console Tool";
    
    }

}
