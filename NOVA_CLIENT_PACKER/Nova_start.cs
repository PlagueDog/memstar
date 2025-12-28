// $WinConsoleEnabled=true;
// Console::enable(true);

//LOGGING
// $console::logmode=2;
// $console::printlevel=2;

IDFNT_CONSOLE = 00159995, "console.pft";
//IDFNT_IF_MR_36B = 00159994, "if_mr_36b.pft"; //Used with TGA image replacing to display icons
//IDFNT_IF_MR_18B = 00159993, "if_mr_18b.pft";

//Load shell.vol here incase Nova.vol has assets that must replace assets in shell.vol
newObject( shellVol, SimVolume, "shell.vol" );
IDPAL_LOGO = 00180001, "logo.ppl";
        
$pref::windowBorder = 0;
//Delay this so to allow mem.dll to hook into the OpenGL context of the simcanvas

//Create some empty functions so we don't spam the console on startup
function Nova::guiOpen(){}
function NovaLoadingGui::onOpen(){}
function NovaLoadingGui::onClose(){}
function modloaderInitERRGui::onOpen(){}
function modloaderInitERRGui::onClose(){}

function __clientBegin()
{   
    ##Uncomment to disable OpenGL surface manipulation
    //deleteFunctions("OpenGL::shif*");
    //deleteFunctions("OpenGL::scal*");
    //deleteFunctions("OpenGL::offs*");
    //deleteFunctions("OpenGL::flip*");
    //deleteFunctions("OpenGL::unflip*");
    if ($cargv1 != "-s")
    {
        
        IDSTR_DEFAULT_IRC_SERVER         = 00131095, "irc.libera.chat";
        IDSTR_DEFAULT_IRC_PORT           = 00131096, "6665";
        
        if(isFile("defaultPrefs.cs"))
        {
            exec("defaultPrefs.cs");
            exec("playerPrefs.cs");
            if(!Nova::findInDefaultPrefs("$pref::packetRate"))
            {
                $pref::packetRate = 2000;
            }
			if(!Nova::findInDefaultPrefs("$pref::packetSize"))
            {
                $pref::packetRate = 980;
            }
            if(!Nova::findInDefaultPrefs("$pref::GWC::SIM_FS_DEVICE"))
            {
				if(isGfxDevice(simcanvas,OpenGL))
				{
                    $pref::GWC::SIM_FS_DEVICE = "OpenGL";
				}
				else if(isGfxDevice(simcanvas,Glide))
				{
                    $pref::GWC::SIM_FS_DEVICE = "Glide";
				}
                    $pref::GWC::SIM_FS_MODE = "Default";
                    $pref::GWC::SIM_FS_HEIGHT = "480";
                    $pref::GWC::SIM_FS_WIDTH = "640";
                    $pref::GWC::SIM_IS_FULLSCREEN = "True";
            }
            else if($pref::GWC::SIM_FS_MODE == "Upscaled")
            {
                OpenGL::windowedFullscreen(true);
                setSplash640x480(width,$pref::GWC::SIM_FS_WIDTH);
                setSplash640x480(height,$pref::GWC::SIM_FS_HEIGHT);
            }
        }
        else
        {
            $pref::GWC::SIM_FS_MODE = "Default";
            $pref::GWC::SIM_FS_HEIGHT = "480";
            $pref::GWC::SIM_FS_WIDTH = "640";
            $pref::GWC::SIM_IS_FULLSCREEN = "True";
			if(isGfxDevice(simcanvas,OpenGL))
			{
                $pref::GWC::SIM_FS_DEVICE = "OpenGL";
				setFullscreenDevice( simCanvas, OpenGL );
			}
			else if(isGfxDevice(simcanvas,Glide))
			{
                $pref::GWC::SIM_FS_DEVICE = "Glide";
				setFullscreenDevice( simCanvas, Glide );
			}
            $pref::packetRate = 2000;
            $pref::packetSize = 980;
        }
        unlockWindowSize(simcanvas);
        
        if($pref::GWC::SIM_FS_MODE == "Upscaled" && $pref::GWC::SIM_IS_FULLSCREEN && $pref::GWC::SIM_FS_DEVICE == "OpenGL")
        {
            Nova::disableWindowed();
			enabledWindowBorder();
            IDBMP_LOGO = 00160001, "logoWide.bmp";
            setWindowSize(simcanvas, 854,480);
            function goToShellRes(){}
            function goFullWhenBoth640x480(){}
        }
        else
        {
            IDBMP_LOGO = 00160001, "logo.bmp";
            setWindowSize(simcanvas, 640,480);
        }
        guiLoadContentCtrl(simcanvas, "NovaLoading.gui");
    
        $pref::GWC::SIM_WIN_HEIGHT = "480";
        $pref::GWC::SIM_WIN_WIDTH = "640";
        $pref::GWC::ResolutionIndex = 0;
        
        $Opengl::Active = false;
        
        //Set the main window for Windows dialog window windows... wait what?
        setmainwindow(simcanvas);
        setWindowTitle(simcanvas, "Starsiege [Nova]");
        
        setCursor( simCanvas, "cursor.bmp" );
        cursorOn(simcanvas);
    }

    //The ESCSDelegate-LOOPBACK is created in mem.dll
    //Use deleteObject(cDel); then uncomment one of the transports you need
    # create a client net delegate
    # NOTE: only one of the ESCSDelegates should be created for the client, but
    # you can change which transport it uses (the last two parameters are transport
    # type and port #)
    
    # setup UDP transport only
    # newObject( cDel, ESCSDelegate, false, "IP", 0);
    
    # setup other transports
    # newObject( cDel, ESCSDelegate, false, "COM1", 0 );
    
    # newObject( cDel, ESCSDelegate, false, "LOOPBACK", 0);

    //Clear the function
    deleteFunctions("__clientBegin");
    
    //Kick off the rest of the client init
    schedule("__initNovaClient();",0);
}
schedule("__clientBegin();",0.1);

function __initNovaClient()
{
	function Nova::addReplacement(%crc, %file)
	{
		//$Nova::HashTable++;
		//$Nova::HashTable[$Nova::HashTable, crc] = %crc;
		//$Nova::HashTable[$Nova::HashTable, crc, file] = %file;
		%file = String::Replace(%file, ".tga", "");
		%file = String::Replace(%file, ".TGA", "");
		dataStore(HashTable, %file, String::toUpper(%crc));
	}
	
	
function Nova::queryReplacementFile(%file)
{
	//%h=1;
	// %file = String::Replace(%file, ".bmp", ".tga");
	%file = String::Replace(%file, ".bmp", "");
	%databaseEntry = dataRetrieve(HashTable, %file);
	if(strlen(%databaseEntry))
	{
		echo(%databaseEntry @ " is assigned to " @ %file @ ".bmp");
	}
	else
	{
		echo("There are no hashes assigned to " @ %file @ ".bmp");
	}
	//while(strlen($Nova::HashTable[%h, crc]))
	//{
	//	if(String::toLower($Nova::HashTable[%h, crc, file]) == String::toLower(%file))
	//	{
	//		echo($Nova::HashTable[%h, crc] @ " is assigned to " @ $Nova::HashTable[%h, crc, file]);
	//		%isAssigned = true;
	//	}
	//	%h++;
	//}
	//if(!%isAssigned)
	//{
	//	echo("There are no hashes assigned to " @ %file);
	//}
}
	
    if ($cargv1 == "-s")
    {
		exec("serverPrefs.cs");
		if ($pref::enforceModloader || $server::enforceNova)
		{
			echo("- - - Enforcing Server-side Modloader - - -");
			Nova::startFileServer($server::UDPPortNumber);
		}
	}
	
    //Get our render devices
	console::mute(true);
    listDevices();
	console::mute(false);
    
    deleteFunctions("__initNovaClient");
    deleteFunctions("setAllowedItems");

    #--------------------------------------
    # load some volumes
    
    //Refresh Nova.vol asset priorities
    deleteObject(NovaVol);
    
    newobject( NovaVol, SimVolume, "Nova.vol" );

    newObject( darkstarVol, SimVolume, "Darkstar.vol" );
    newObject( editorVol, SimVolume, "Editor.vol" );
    newObject( gameObjectsVol, SimVolume, "gameObjects.vol" );

    //Refresh Nova.vol asset priorities
    deleteObject(NovaVol);
    
    newobject( NovaVol, SimVolume, "Nova.vol" );
    
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
		exec($CmdLineServerPrefs);
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
    exec( "gui.strings.cs" );
    
    //Empty all the GUI function calls
    %i=100001;
    while(%i++ < 100036)
    {
        if(*%i != "<INVALID TAG>")
        {
        %gui = String::Replace(*%i,".","");
        eval("function " @ %gui @ "::onOpen(){}");
        eval("function " @ %gui @ "::onClose(){}");
        eval("function " @ %gui @ "::onOpen::handleOpenGL(){}");
        eval("function " @ %gui @ "::onClose::handleOpenGL(){}");
        eval("function " @ %gui @ "::onOpen::NovaFunction(){}");
        eval("function " @ %gui @ "::onClose::NovaFunction(){}");
        eval("function " @ %gui @ "::onOpen::CampaignFunction(){}");
        eval("function " @ %gui @ "::onClose::CampaignFunction(){}");
        eval("function " @ %gui @ "::handleOpenGL(){}");
        eval("function " @ %gui @ "::NovaFunction(){}");
        }
    }
    //--------------------------------------
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
    

    exec("Nova_Strings.cs");
    exec("Nova_OpenGL.cs");
	exec("Nova_Lib.cs");
    //exec("Nova_Logger.cs");
    exec("Nova_Databases.cs");
    if (!$CmdLineServer)
    {
        exec("Nova_LibExt.cs");
    }
    exec("Nova_NetLib.cs"); 
    exec("Nova_OverridesLib.cs");
    exec("Nova_Campaign.cs");
    exec("Nova_PlayerSkins.cs");
	Nova::loadTextureHashes();
	
    // No CD patch for both SSP and Complete
    exec("Nova_nocd.cs");

    IDGUI_MODLOADERINITERR = 00100004, "modloaderInitERR.gui";

    exec("nocd.cs");  
    exec(stdlib);

	function modloaderInitERRGUI::onOpen()
	{
		$engine::testButtonFillColor = 0;
		$engine::testButtonBorderColor = 237;
		$engine::testButtonSelectColor = 237;
		$engine::testButtonFillOpacity = -1;
	}

    #--------------------------------------
    # bring up the console window if cmdline server, 
    # else bring up the game window
    $pref::Display::gammaValue = 1.0;
    
        focusClient();
        
        if(isFile("Nova_Config.cs"))
        {
            echo(" - - [Loading Nova Config] - - ");
            exec("Nova_Config.cs");
        }
        
        //Don't create a game canvas if this is a dedicated server
        if ($cargv1 != "-s")
        {
            setCursor( simCanvas, "cursor.bmp" );
            cursorOn(simcanvas);
            guimouse();
            $modloader::load = true;
            if($modloader::crashFail)
            {
                if($pref::GWC::SIM_FS_MODE == "Upscaled")
                {
                    IDBMP_LOGO = 00160001, "logoWide.bmp";
                    setWindowSize(simcanvas, 854,480);
                }
                else
                {
                    IDBMP_LOGO = 00160001, "logo.bmp";
                    setWindowSize(simcanvas, 640,480);
                }
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
    //}
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
    
    if (isFile("Missions.vol"))
    {
        newObject( Missions, SimVolume, "Missions.vol" );
    }
    
    
    //If it is NOT a headless client
    if (! $CmdLineServer)
    {
        //if(isFile("defaultPrefs.cs"))
        //{
        //    exec("defaultPrefs.cs");
        //}
        
        //if($pref::GWC::SIM_FS_MODE != "Default")
        //{
        //    schedule("modloader::OpenGL::Mode($pref::GWC::SIM_FS_MODE);",2);
        //}

        //else
        //{
            //GoFullWhenBoth640x480();
            //ForceToShellRes();
        //}
        
        %m=0;
        bind( keyboard, make, sysreq, TO, "screenShot(simCanvas);");
        bind( keyboard, make, shift, "numpad+", TO, "adjustShellRes(up);" );
        bind( keyboard, make, shift, "numpad-", TO, "adjustShellRes(down);" );
        bind( keyboard0, make, alt, n, TO, "modloadergui::onOpen();" );
        bind( keyboard0, make, control, shift, d, TO, "Mem::editDefaultPrefs();");
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
        
        // if($pref::GWC::SIM_FS_MODE == "Upscaled" || $pref::GWC::SIM_FS_MODE == "Windowed")
        if($pref::GWC::SIM_FS_MODE == "Upscaled")
        {
            OpenGL::windowedFullscreen(true);
            //IDBMP_LOGO = 00160001,0;
            //if($pref::GWC::SIM_FS_MODE == "Upscaled")
            //{
                    //If the OGL Minimize is disabled we need re-create the game window by switching to windowed then back to fullscreen
                    //modloader::RecreateGameWindow();
            //}
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
    $pref::PacketRate = 4096;
    $pref::PacketSize = 1000;
   
    if (!$CmdLineServer && !$modloader::load)
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
		//Fixes isFile() on Windows machines that are including quotation marks in the launch script
		$CmdLineServerPrefs = String::Replace($CmdLineServerPrefs, "\"", "");
		
        echo("COMMAND LINE SERVER RUNNING CONFIG -> [ ", $CmdLineServerPrefs, " ]");
		
		$pref::fixRecording = 0;
		schedule("Nova::toggleRecordingFix();",0);
				
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
        schedule("focusClient();exec(server);focusServer();",1.25); //Wait for constructors
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
		
        schedule("$pref::PacketRate = 4096;",0.25);
        schedule("$pref::PacketSize = 65535;",0.25);
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
    schedule("Nova::initConstructors();",0);
}


function Engine::EndFrame(){}
function Nova::containCursor(){}
