$WinConsoleEnabled=true; //Uncomment to enable the external console window
//$console::logmode=1; //Uncomment to enable console logging to console.log
//$console::printlevel=3; //Uncomment to enable full console verboose


IDFNT_CONSOLE = 00159995, "console.pft";
//Create game canvas prior to loading mem.dll so OpenGL calls are handled properly
if ($cargv1 != "-s")
{
    newObject( simCanvas, SimGui::Canvas, "Starsiege", 640, 480, true_quitOnSimcanvasDestroy, 1);
    unlockWindowSize(simcanvas);
}
newobject(NovaVol, simVolume, "Nova.vol");
$pref::GWC::SIM_WIN_HEIGHT = "480";
$pref::GWC::SIM_WIN_WIDTH = "640";
$client::fov = 68;

//Disable the client init into 640x480 for now to prevent resolution thrashing
//schedule("disableSplash640x480(true);",0.25);
$Opengl::Active = false;
OpenGL::WindowLoseFocusMinimize(false);
function __initNovaClient()
{
    # create a client net delegate
    # NOTE: only one of the ESCSDelegates should be created for the client, but
    # you can change which transport it uses (the last two parameters are transport
    # type and port #)
    
    # setup UDP transport only
    # newObject( cDel, ESCSDelegate, false, "IP", 0);
    
    # setup other transports
    # newObject( cDel, ESCSDelegate, false, "COM1", 0 );
    
    newObject( cDel, ESCSDelegate, false, "LOOPBACK", 0);
    
    function setAllowedItems() {}
    exec("repath.cs");
    
    if (! $CmdLineServer)
    {
        if(isFile("defaultPrefs.cs"))
        {
           exec("defaultPrefs.cs");
           if($pref::GWC::SIM_FS_MODE == "Upscaled")
           {
                setSplash640x480(width,$pref::GWC::SIM_FS_WIDTH);
                setSplash640x480(height,$pref::GWC::SIM_FS_HEIGHT);
           }
        }
        else
        {
            setFullscreenDevice(simcanvas, "OpenGL");
            $pref::GWC::SIM_FS_MODE = "Default";
            $pref::GWC::SIM_FS_HEIGHT = "480";
            $pref::GWC::SIM_FS_WIDTH = "640";
            $pref::GWC::SIM_IS_FULLSCREEN = "True";
            $pref::packetrate = 255;
        }
    }
    #--------------------------------------
    # load some volumes
    newObject( shellVol, SimVolume, "shell.vol" );
    if(isFile("Nova.vol"))
    {
        deleteObject(NovaVol);
        newobject( NovaVol, SimVolume, "Nova.vol" );
    }
    newObject( darkstarVol, SimVolume, "Darkstar.vol" );
    newObject( editorVol, SimVolume, "Editor.vol" );
    newObject( gameObjectsVol, SimVolume, "gameObjects.vol" );
    if (isFile("patch.vol"))
    {
        newObject( patchVol, SimVolume, "patch.vol" );
    }
    
    //Load more patch volumes if they exist and are labled as patch#.vol where # can be a number up to 32 to differentiate the file from patch volumes
    for(%i=0;%i<32;%i++)
    {
        if (isFile("patch" @ %i @ ".vol"))
        {
            newObject( "patchVol" @ %i, SimVolume, "patch" @ %i @ ".vol" );
        }
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
    
    exec("keyboardSetup.cs");
    
    //--------------------------------------
    // load the string tables
    // darkstar strings (editor is optional, or should be)
    
    // load tag dictionaries required to display gui
    exec( "darkstar.strings.cs" );
    exec( "addendum.strings.cs" );
    
    // check disk free space after gui strings are loaded
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
    
    function StringM::explode(%arg1,%arg2,%arg3)
    {
        String::Explode(%arg1,%arg2,%arg3);
    }
    function strAlignR(%arg1, %arg2)
    {
        String::Right(%arg2, %arg1);
    }
    exec("Nova_Logger.cs");
    exec("Nova_Lib.cs");
    exec("Nova_Databases.cs");
    exec("Nova_LibExt.cs");
    exec("Nova_NetLib.cs"); 
    exec("Nova_OverridesLib.cs");
    // No CD patch for both SSP and Complete
    exec("Nova_nocd.cs");
    schedule("console::enable(true);",2);
    IDGUI_MODLOADERINITERR = 00100004, "modloaderInitERR.gui";
    function splashGUI::onOpen::modloaderFunc()
    {
        guiload("intro.gui");
    }
  
    exec( "gui.strings.cs" );
    exec(stdlib);

    #--------------------------------------
    # bring up the console window if cmdline server, 
    # else bring up the game window
    $pref::Display::gammaValue = 1.0;
    
        focusClient();
        if(!CmdLineServer)
        {
            # start up the gui
            setFullscreenDevice( simCanvas, _($pref::GWC::SIM_FS_DEVICE,Glide) );
            setmainwindow(simcanvas);
            setWindowTitle(simcanvas, "Starsiege");
        }
        
        if(isFile("Nova.vol"))
        {
            if(isFile("Nova_Config.cs"))
            {
                echo(" - - [Loading Nova Config] - - ");
                exec("Nova_Config.cs");
            }
        }
        
        //Don't create a game canvas if this is a dedicated server
        if ($cargv1 != "-s")
        {
            setCursor( simCanvas, "cursor.bmp" );
            cursorOn(simcanvas);
            guimouse();
            if(isFile("Nova.vol"))
            {
                $modloader::load = true;
                if($modloader::crashFail)
                {
                    guiload("modloaderInitERR.gui");
                    inputActivate(all);
                    break;
                }
                else
                {
                    inputActivate(all);
                    $modloader::crashFail = true;
                    export("$modloader::*", "Nova_Config.cs");
                    __resumeClientInit();
                }
            }
        }
        if($CmdLineServer || !$modloader::crashFail)
        {
            schedule("__resumeClientInit();",1);
        }
    ###        //Handle mod crashes so that the player can get back into the modloader-
    ###        //if a mod or a combination of mods are crashing the game
    ###        if(isFile("modloader.vol"))
    ###        {
    ###            $modloader::load = true;
    ###            if($modloader::crashFail && isObject(simcanvas))
    ###            {
    ###                confirmBox("Previous Game Session", "The last session of Starsiege terminated unexpectedly. \n\nContinue with mods loaded?");
    ###                if($dlgresult == "[yes]"){}
    ###                if($dlgresult == "[no]")
    ###                {
    ###                    //Disable mod loading
    ###                    $modloader::load = "false";
    ###                }
    ###                if($dlgresult == "[cancel]"){deleteObject(0);}
    ###            }
    ###            $modloader::crashFail = true;
    ###            export("$modloader::*", "modloaderConfig.cs");
    ###        }
    ###    }
    ###}
    $modloader::serverDataDirectory="";
}

function mlERRButtonYES::onAction()
{
    GuiLoadContentCtrl( simCanvas, "splash.gui" );
}

function mlERRButtonNO::onAction()
{
    GuiLoadContentCtrl( simCanvas, "splash.gui" );
}

function mlERRButtonQUIT::onAction()
{
    $modloader::crashFail = true;
    export("$modloader::*","Nova_Config.cs");
    schedule("quitGame();",0.1);
}

function __resumeClientInit()
{
    if(!CmdLineServer)
    {
        setCursor( simCanvas, "cursor.bmp" );
    }
    exec( "sound.cs" );
    exec( "quickchat.cs" );
    
    newObject( "", ChatDispatcher );
    newObject( "", ChatScheduler );
    if(!CmdLineServer)
    {
        newObject( "", SSIRCClient );
        echo("CREATING DUST MANAGER");
        newObject( "", DustManager );
        newObject( "", ShellMusic );
    }
    
    //The host.gui does not handle the selection and loading of missions from volumes since loose files take priority over packed files
    //
    //if (isFile("Missions.vol"))
    //{
    //    newObject( Missions, SimVolume, "Missions.vol" );
    //}
    
    
    //If it is NOT a headless client
    if (! $CmdLineServer)
    {
        //if(isFile("defaultPrefs.cs"))
        //{
        //    exec("defaultPrefs.cs");
        //}
        
        if(strlen($pref::GWC::SIM_FS_MODE) > 0)
        {
            schedule("modloader::OpenGL::Mode($pref::GWC::SIM_FS_MODE);",2);
        }
        
        %m=0;
        bind( keyboard, make, sysreq, TO, "screenShot(simCanvas);");
        if(isFile("Nova.vol"))
        {
            bind( keyboard, make, shift, "numpad+", TO, "adjustShellRes(up);" );
            bind( keyboard, make, shift, "numpad-", TO, "adjustShellRes(down);" );
            bind( keyboard0, make, alt, m, TO, "modloadergui::onOpen();" );

            if($pref::GWC::SIM_FS_MODE == "Windowed" || $pref::DisableOGLMinimize)
            {
                OpenGL::WindowLoseFocusMinimize("false");
            }
        }
        else
        {
            bind( keyboard, make, shift, "numpad+", TO, "nextres(simcanvas);" );
            bind( keyboard, make, shift, "numpad-", TO, "prevres(simcanvas);" );
        }
    }

    
    //Ctrl + Shift + W to apply window styles back on to the game window.
    if (! $CmdLineServer)
    {
        bind( keyboard0, make, control, shift, w, TO, "toggleBorder();");
    }
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
    
    $zz_MM_firstload = true;
    if (!$CmdLineServer)
    {
        if($pref::GWC::SIM_IS_FULLSCREEN)
        {
            setFullscreenMode(simcanvas,true);
        }
        else
        {
            setFullscreenMode(simcanvas,false);
        }
        
        if($pref::GWC::SIM_FS_MODE == "Upscaled" || $pref::GWC::SIM_FS_MODE == "Windowed")
        {
            if($pref::GWC::SIM_FS_MODE == "Upscaled")
            {
                    //If the OGL Minimize is disabled we need re-create the game window by switching to windowed then back to fullscreen
                    modloader::RecreateGameWindow();
            }
            GuiLoadContentCtrl( simCanvas, "splash.gui" );
        }
        else
        {
            GuiLoadContentCtrl( simCanvas, "splash.gui" );  
        }
    }
    
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
           
    if (!$modloader::load)
    {
        exec( "Nova_datLoad.cs" );
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
        echo("COMMAND LINE SERVER: EXECUTING CONFIG -> [ ", $CmdLineServerPrefs, " ]");
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
        
        clientCursorOn();
        
        # create a redbook object if CD device is available
        cdAudioNew();
        focusClient();
        
    }
    showVersion();
    $muteModloaderOverlay = true;
    schedule("modloader::initConstructors();",0);
    schedule("modloader::toggleCockpitFadein();",0);
}
__initNovaClient();