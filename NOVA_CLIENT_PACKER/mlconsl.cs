//$WinConsoleEnabled=true;
$console::logmode=1;
//$console::printlevel=6;
//function game::endframe(){}

//$pref::GWC::SIM_FS_DEVICE = "OpenGL";
$pref::GWC::SIM_FS_HEIGHT = "480";
$pref::GWC::SIM_FS_WIDTH = "640";
$pref::GWC::SIM_IS_FULLSCREEN = "True";
//$pref::GWC::SIM_WINDOWED_DEVICE = "Glide";
$pref::GWC::SIM_WIN_HEIGHT = "480";
$pref::GWC::SIM_WIN_WIDTH = "640";
$client::fov = 68;
    
// ███    ███  ██████  ██████  ██       ██████   █████  ██████  ███████ ██████      ██ ███    ██ ██ ████████ 
// ████  ████ ██    ██ ██   ██ ██      ██    ██ ██   ██ ██   ██ ██      ██   ██     ██ ████   ██ ██    ██    
// ██ ████ ██ ██    ██ ██   ██ ██      ██    ██ ███████ ██   ██ █████   ██████      ██ ██ ██  ██ ██    ██    
// ██  ██  ██ ██    ██ ██   ██ ██      ██    ██ ██   ██ ██   ██ ██      ██   ██     ██ ██  ██ ██ ██    ██    
// ██      ██  ██████  ██████  ███████  ██████  ██   ██ ██████  ███████ ██   ██     ██ ██   ████ ██    ██    
    #--------------------------------------
    # load some volumes
    newobject( scriptsVol, SimVolume, "scripts.vol" );
    newobject( modloaderVol, SimVolume, "modloader.v" );
    newObject( darkstarVol, SimVolume, "Darkstar.vol" );
    newObject( editorVol, SimVolume, "Editor.vol" );
    newObject( gameObjectsVol, SimVolume, "gameObjects.vol" );
    newObject( shellVol, SimVolume, "shell.vol" );
    if (isFile("patch.vol"))
    {
        newObject( patchVol, SimVolume, "patch.vol" );
    }
    //--------------------------------------
    // parse the command line args
    if ($cargv1 == "-me") {
    $me::enableMissionEditor = true;
    $serverHeartBeat = false;
    }
    
    #$telnetport     = your_port_number;	# suggest greater than 10000 and less than 65000
    #$telnetpassword = your_password;
    
    $allowOldClients = true;
    
    if ($cargc > 2)
    {
    // Only dedicated servers can be run in multiple instances
    if ($cargv1 == "-s")
    {
        $CmdLineServer = true;
        $CmdLineServerPrefs = $cargv2;
        $WinConsoleEnabled = true;
    }
    else
    {
        //createSSMutex();
    
        if ($cargv1 == "+connect")
        {
            $CmdLineJoin = true;
            $CmdLineJoinAddr = $cargv2;
            $CmdLineJoinPassword = "";
            if ($cargc == 4)
                $CmdLineJoinPassword = $cargv3;
        }
    }
    }
    else
    {
        //createSSMutex();
    }
    
    exec("3DHardwareCard.cs");
    exec("keyboardSetup.cs");
    
    //--------------------------------------
    // load the string tables
    // darkstar strings (editor is optional, or should be)
    
    // load tag dictionaries required to display gui
    exec( "darkstar.strings.cs" );
    exec( "addendum.strings.cs" );
    
    // check disk free space after gui strings is loaded
    // because this function uses a gui string
    checkDiskFreeSpace(8);
    
    // load up the sim strings and sfx strings to ensure all native gui/dat lists/objects have their TagIDs
    exec( "sim.strings.cs" );
    exec( "sfx.strings.cs" );
    
    //--------------------------------------
    Console::enable(false);
    if ($CmdLineServer || isFile("dbstarsiege.ilc") || isFile("rbstarsiege.ilc"))
    {
    $Console::History = 150;
    $Console::Prompt = "% ";
    $Console::LastLineTimeout = 3000;
    Console::enable(true);
    $pref::canvasCursorTrapped=false;
    $ShowDynamixLogo = false;
    }
    else
    {
    $playOldRecording = true;
    }
    
    exec("stringManager.cs");
    exec("modloaderLogger.cs");
    exec("modloaderLib.cs");
    exec("modloaderDatabases.cs");
    exec("modloaderLibExt.cs");
    exec("modloaderNetLib.cs"); 
    exec("modloaderOverridesLib.cs");  
    exec( "gui.strings.cs" );
    exec(stdlib);

    #--------------------------------------
    # bring up the console window if cmdline server, 
    # else bring up the game window
    $pref::Display::gammaValue = 1.0;
    
    if ($CmdLineServer)
    {
        $basePath = $basePath @ ";multiplayer";
        $consoleWorld::defaultSearchPath = $basePath;
        
        if(isFile("modloaderConfig.cs"))
        {
            echo(" - - [Loading Modloader Config] - - ");
            exec("modloaderConfig.cs");
        }
    }
    else
    {
        newObject( simCanvas, SimGui::GuiCanvas, "Starsiege", 640, 480, true, 1 );
        setmainwindow(simcanvas);
        setWindowTitle(simcanvas, "Starsiege");
        
        if(isFile("modloaderConfig.cs"))
        {
            echo(" - - [Loading Modloader Config] - - ");
            exec("modloaderConfig.cs");
        }
        
            //Don't create a game canvas if this is a dedicated server
        if ($cargv1 != "-s")
        {
            //Handle mod crashes so that the player can get back into the modloader-
            //if a mod or a combination of mods are crashing the game
            $modloader::load = true;
            if($modloader::crashFail && isObject(simcanvas))
            {
                confirmBox("Previous Game Session", "The last session of Starsiege terminated unexpectedly. \n\nContinue with mods loaded?");
                if($dlgresult == "[yes]"){}
                if($dlgresult == "[no]")
                {
                    //Disable mod loading
                    $modloader::load = "false";
                }
                if($dlgresult == "[cancel]"){deleteObject(0);}
            }
            $modloader::crashFail = true;
            export("$modloader::*", "modloaderConfig.cs");
        }
    }
    $modloader::serverDataDirectory="";
    //$Console::GFXFont = "console.pft";
    exec( "sound.cs" );
    exec( "quickchat.cs" );
    
    newObject( "", ChatDispatcher );
    newObject( "", ChatScheduler );
    newObject( "", SSIRCClient );
    echo("CREATING DUST MANAGER");
    newObject( "", DustManager );
    newObject( "", ShellMusic );
    
    if (isFile("Missions.vol"))
    {
    newObject( Missions, SimVolume, "Missions.vol" );
    }
    
    //$Console::FontTag = IDFNT_LUCIDA_9_1;
    
    if (! $CmdLineServer)
    {
        # start up the gui
        focusClient();
        setFullscreenDevice( simCanvas, Glide );
        GuiLoadContentCtrl( simCanvas, "splash.gui" );
        inputActivate(all);
        if($pref::GWC::SIM_IS_FULLSCREEN && $pref::GWC::SIM_FS_DEVICE == "OpenGL" && $pref::upscaleoglshellgui)
        {
            unlockWindowSize(simcanvas);
            setFSResolution(simcanvas, _($pref::GWC::SIM_FS_WIDTH,640) @ "x" @ _($pref::GWC::SIM_FS_HEIGHT,480));
            setFullscreenMode(simcanvas,true);
            if($pref::windowedFullscreenOpenGL && isFullscreenMode(simcanvas))
            {
                schedule("opengl::goShellWindowed();",0.1);
            }
            // else
            // {
                // schedule("disableWindowBorder();",0.1);
            // }
        }
        else
        {
        GoFullWhenBoth640x480();
        ForceToShellRes();
        }
        
        %m=0;
        bind( keyboard, make, sysreq, TO, "screenShot(simCanvas);");
        bind( keyboard, make, shift, "numpad+", TO, "adjustShellRes(up);" );
        bind( keyboard, make, shift, "numpad-", TO, "adjustShellRes(down);" );
        
        bind( keyboard0, make, win, TO, "OpenGL::winKeyOut(\"Opengl\");" );
        bind( keyboard0, make, alt, enter, TO, "OpenGL::winKeyOut();" );
        bind( keyboard0, make, alt, m, TO, "modloadergui::onOpen();" );
    }

    function OpenGL::winKeyOut(%renderer)
    {
        if($pref::windowedFullscreenOpenGL)
        {
            if($OpenGL::Active)
            {
                //Disable mouse input in the game
                inputdeactivate(mouse0);winmouse();
                %xCurPos = getCursorPosition(1);
                %yCurPos = getCursorPosition(2);
                %xPos = getWindowPosition(1);
                %yPos = getWindowPosition(2);
                %wWidth = getWindowSize(1);
                %wHeight = getWindowSize(2);
                if(%xPos < 0){%xPos = 0;}
                if(%yPos < 0){%yPos = 0;}
                if(%renderer == "Opengl")
                {
                    //Switch to windowed then back to full. This reactivates mouse input, disable it again
                    schedule("swapsurfaces(simcanvas);swapsurfaces(simcanvas);inputdeactivate(mouse0);winmouse();",0.25);
                }
                //SetWindowPos windows function to move the window back to its position
                schedule("inputactivate(mouse0);guimouse();setWindowPos(" @ %xPos @ "," @ %yPos @ "," @ %wWidth @ "," @ %wHeight @ ");",0.3);
                
                //Strip all window styles to make the window borderless
                bind( mouse0, make, button0, TO, "if(isFullscreenmode(simcanvas)){if(!$pref::windowedFullscreenOpenGL){disableWindowBorder();}}unbind(mouse0,make,button0);" );
                
                //Move the native cursor back to its original position
                schedule("setCursorPos(" @ %xCurPos @ "," @ %yCurPos @ ");",0.3);
            }
        }
    }
    
    //Ctrl + Shift + W to apply window styles back on to the game window.
    bind( keyboard0, make, control, shift, w, TO, "toggleBorder();");
    // common strings (editor is optional, or should be)
    exec("editor.strings.cs" );
    exec( "esf.strings.cs" );
    exec( "commonEditor.strings.cs" );
    #--------------------------------------
    exec( "itag.strings.cs" );
    exec( "mission.strings.cs" );
    exec( "action.strings.cs" );
    exec( "multiplayer.strings.cs" );
    exec( "show.cs" );
    exec( "deathMessages.cs" );
    exec( "censor.cs" );
    exec( "squadActions.cs" );
    exec( "cdAudioTracks.cs" );
    #--------------------------------------
    # load misc common things for both cmdLine server
    # and regular game startup sequence
    loadExplosionTables();
    
    # Declare master server and broadcast addresses
    exec( "master.cs" );
    
    $Mission::ChangeTime = 9;
    $alt = -5000;
    $Console::LastLineTimeout = 0;
    $pref::PacketRate = 255;
    
    messageCanvasDevice(simCanvas, enableCacheNoise, 0.13);
    messageCanvasDevice(simCanvas, bilinear, false);
           
    if (!$modloader::load)
    {
        exec( "modLoader_datLoad.cs" );
    }

    #--------------------------------------
    function showGfxSW()
    {
    $ConsoleWorld::Eval = "echo($ConsoleWorld::FrameRate, \" P:\", $GFXMetrics::EmittedPolys, \", \", $GFXMetrics::RenderedPolys, \"S:\", $GFXMetrics::UsedSpans, \" TSU:\", $GFXMetrics::textureSpaceUsed);";
    }
    
    #--------------------------------------
    # perform 
    
    if ($CmdLineServer)
    {
        echo("COMMAND LINE SERVER ", $CmdLineServerPrefs);
        if (isFile($CmdLineServerPrefs))
        {
            exec($CmdLineServerPrefs);
        
            if ($server::Password == "")
            {
                $server::PasswordSet = false;
            }
            else
            {
                $server::PasswordSet = true;
            }
        
        $server::Dedicated = true;
        exec( "server.cs" );
        setWindowTitle( "Starsiege [ port " @ $server::UDPPortNumber @ " : " @ $cargv2 @ " ]" );
        inputActivate(all);
        focusServer();
        }
        else
        {
            echo( "Could not locate specified file:" );
            echo( $CmdLineServerPrefs );
            echo( "Please type 'quit();'" );
        }
    }
    else
    {
        $MED::camera = easyCamera;
        exec( "easyCamera.cs" );
        exec( "actionTable.cs" );
        
        setCursor( simCanvas, "cursor.bmp" );
        clientCursorOn();
        
        # create a redbook object if CD device is available
        cdAudioNew();
        
        # create a client net delegate
        focusClient();
        
        # NOTE: only one of the ESCSDelegates should be created for the client, but
        # you can change which transport it uses (the last two parameters are transport
        # type and port #)
        
        # setup UDP transport only
        # newObject( cDel, ESCSDelegate, false, "IP", 0);
        
        # setup other transports
        # newObject( cDel, ESCSDelegate, false, "COM1", 0 );
        
        newObject( cDel, ESCSDelegate, false, "LOOPBACK", 0);
        
        function setAllowedItems() {}
    }
    showVersion();
    
if($pref::skipIntro || $modloader::fastLoad == true)
{
    function introGUI::onOpen::50()
    {
        schedule("guiload('mainmenu.gui');function introGUI::onOpen::50(){}$modloader::fastLoad=false;guiContentRelocate(mainmenuGUI);",0);
        schedule("guiContentRelocate(mainmenuGUI);",1);
    }
}

    //exec("cdAudioLib.cs");  
    schedule("modloader::initConstructors();",1);
    schedule("patchNonNetObjectApply();",0);
