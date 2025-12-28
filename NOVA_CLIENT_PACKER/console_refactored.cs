$WinConsoleEnabled = true;
//This contains the standard console.cs script as well as a modified-
//console.cs script specifically for use with the modloader

//If there is no modLoader.vol this variable will be set to false
$HAVE_THE_MODLOADER = true;
if(!isFile("modLoader.vol"))
{
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
    }
    else
    {
        createSSMutex();
    
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
    createSSMutex();
    }
    
    exec("3DHardwareCard.cs");
    exec("keyboardSetup.cs");
    
    //--------------------------------------
    // load the string tables
    // darkstar strings (editor is optional, or should be)
    exec("editor.strings.cs" );
    
    // load tag dictionaries required to display gui
    exec( "darkstar.strings.cs" );
    exec( "gui.strings.cs" );
    exec( "addendum.strings.cs" );
    
    // check disk free space after gui strings is loaded
    // because this function uses a gui string
    checkDiskFreeSpace(8);
    
    // common strings (editor is optional, or should be)
    exec( "esf.strings.cs" );
    exec( "commonEditor.strings.cs" );
    
    // earthsiege specific strings
    exec( "mission.strings.cs" );
    exec( "sim.strings.cs" );
    exec( "itag.strings.cs" );
    exec( "sfx.strings.cs" );
    exec( "action.strings.cs" );
    exec( "multiplayer.strings.cs" );
    exec( "show.cs" );
    exec( "deathMessages.cs" );
    exec( "censor.cs" );
    exec( "squadActions.cs" );
    exec( "cdAudioTracks.cs" );
    
    // No CD patch
    schedule("exec('NoCD_override.cs');",1);
    
    //--------------------------------------
    //checkDisk();   
    getCDRomDrive();   
    // Linux Wine Fix
    // checkForFile("smackw32.dll", "shell.vol", "dinput.dll", "ddraw.dll", "dsound.dll", "dinput.dll");
    checkDirectXVersion();
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
    verifyCDRomInDrive();
    $playOldRecording = true;
    }
    
    
    exec(repath);
    exec(stdlib);
    
    #--------------------------------------
    # bring up the console window if cmdline server, 
    # else bring up the game window
    
    $pref::Display::gammaValue = 1.0;
    
    if ($CmdLineServer)
    {
    $basePath = $basePath @ ";multiplayer";
    $consoleWorld::defaultSearchPath = $basePath;
    
    $WinConsoleEnabled = true;
    }
    else
    {
    newObject( simCanvas, SimGui::GuiCanvas, "Starsiege", 640, 480, true, 1 );
    $Console::GFXFont = "console.pft";
    exec( "sound.cs" );
    exec( "quickchat.cs" );
    }
    
    #--------------------------------------
    # load some volumes
    if (isFile("patch.vol"))
    {
    newObject( patchVol, SimVolume, "patch.vol" );
    }
    newObject( darkstarVol, SimVolume, "Darkstar.vol" );
    newObject( editorVol, SimVolume, "Editor.vol" );
    newObject( gameObjectsVol, SimVolume, "gameObjects.vol" );
    newObject( shellVol, SimVolume, "shell.vol" );
    
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
    
    $Console::FontTag = IDFNT_LUCIDA_9_1;
    
    if (! $CmdLineServer)
    {
    # start up the gui
    focusClient();
    setFullscreenDevice( simCanvas, Glide );
    GuiLoadContentCtrl( simCanvas, "splash.gui" );
    inputActivate(all);
    GoFullWhenBoth640x480();
    ForceToShellRes();
    
    bind( keyboard, make, sysreq, TO, "screenShot(simCanvas);");
    bind( keyboard, make, shift, "numpad+", TO, "nextRes(simCanvas);" );
    bind( keyboard, make, shift, "numpad-", TO, "prevRes(simCanvas);" );
    }
    
    #--------------------------------------
    # load misc common things for both cmdLine server
    # and regular game startup sequence
    loadExplosionTables();
    
    # Declare master server and broadcast addresses
    exec( "master.cs" );
    
    $Mission::ChangeTime = 9;
    $alt = 3;
    $Console::LastLineTimeout = 0;
    $pref::PacketRate = 10;
    
    messageCanvasDevice(simCanvas, enableCacheNoise, 0.13);
    exec( "datLoad.cs" );
    
    
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
    
    //Stop the execution here if we don't have a modLoader.vol
    $HAVE_THE_MODLOADER = false;
}

// ███    ███  ██████  ██████  ██       ██████   █████  ██████  ███████ ██████      ██ ███    ██ ██ ████████ 
// ████  ████ ██    ██ ██   ██ ██      ██    ██ ██   ██ ██   ██ ██      ██   ██     ██ ████   ██ ██    ██    
// ██ ████ ██ ██    ██ ██   ██ ██      ██    ██ ███████ ██   ██ █████   ██████      ██ ██ ██  ██ ██    ██    
// ██  ██  ██ ██    ██ ██   ██ ██      ██    ██ ██   ██ ██   ██ ██      ██   ██     ██ ██  ██ ██ ██    ██    
// ██      ██  ██████  ██████  ███████  ██████  ██   ██ ██████  ███████ ██   ██     ██ ██   ████ ██    ██    

if($HAVE_THE_MODLOADER)
{   
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
    }
    else
    {
        createSSMutex();
    
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
    createSSMutex();
    }
    
    exec("3DHardwareCard.cs");
    exec("keyboardSetup.cs");
    
    //--------------------------------------
    // load the string tables
    // darkstar strings (editor is optional, or should be)
    exec("editor.strings.cs" );
    
    // load tag dictionaries required to display gui
    exec( "darkstar.strings.cs" );
    exec( "gui.strings.cs" );
    exec( "addendum.strings.cs" );
    
    // check disk free space after gui strings is loaded
    // because this function uses a gui string
    checkDiskFreeSpace(8);
    
    // common strings (editor is optional, or should be)
    exec( "esf.strings.cs" );
    exec( "commonEditor.strings.cs" );
    
    // earthsiege specific strings
    exec( "mission.strings.cs" );
    exec( "sim.strings.cs" );
    exec( "itag.strings.cs" );
    exec( "sfx.strings.cs" );
    exec( "action.strings.cs" );
    exec( "multiplayer.strings.cs" );
    exec( "show.cs" );
    exec( "deathMessages.cs" );
    exec( "censor.cs" );
    exec( "squadActions.cs" );
    exec( "cdAudioTracks.cs" );
    
    // No CD patch
    schedule("exec('NoCD_override.cs');",1);
    
    //--------------------------------------
    //checkDisk();   
    getCDRomDrive();   
    // Linux Wine Fix
    // checkForFile("smackw32.dll", "shell.vol", "dinput.dll", "ddraw.dll", "dsound.dll", "dinput.dll");
    //checkDirectXVersion();
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
    verifyCDRomInDrive();
    $playOldRecording = true;
    }
    
    
    exec(repath);
    exec(stdlib);
    
    #--------------------------------------
    # bring up the console window if cmdline server, 
    # else bring up the game window
    
    $pref::Display::gammaValue = 1.0;
    
    if ($CmdLineServer)
    {
    $basePath = $basePath @ ";multiplayer";
    $consoleWorld::defaultSearchPath = $basePath;
    
    $WinConsoleEnabled = true;
    }
    else
    {
    newObject( simCanvas, SimGui::GuiCanvas, "Starsiege", 640, 480, true, 1 );
    setmainwindow(simcanvas);
    $Console::GFXFont = "console.pft";
    exec( "sound.cs" );
    exec( "quickchat.cs" );
    }
    
    #--------------------------------------
    # load some volumes
    if (isFile("patch.vol"))
    {
    newObject( patchVol, SimVolume, "patch.vol" );
    }
    newObject( darkstarVol, SimVolume, "Darkstar.vol" );
    newObject( editorVol, SimVolume, "Editor.vol" );
    newObject( gameObjectsVol, SimVolume, "gameObjects.vol" );
    newObject( shellVol, SimVolume, "shell.vol" );
    
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
    
    $Console::FontTag = IDFNT_LUCIDA_9_1;
    
    if (! $CmdLineServer)
    {
    # start up the gui
    focusClient();
    setFullscreenDevice( simCanvas, Glide );
    GuiLoadContentCtrl( simCanvas, "splash.gui" );
    inputActivate(all);
    GoFullWhenBoth640x480();
    ForceToShellRes();

    exec("modloaderLib.cs");
    if(isFile("modloaderConfig.cs"))
    {
        echo(" - - [Loading Modloader Config] - - ");
        exec("modloaderConfig.cs");
    }
    //Don't create a game canvas if this is a dedicated server
    if ($cargv1 != "-s")
    {
        //Gui canvas must be created early in order to handle the crash dialog window
        newObject( simCanvas, SimGui::GuiCanvas, "Starsiege", 640, 480, true, 1 );
        setMainWindow(simcanvas);
        
        //Handle mod crashes so that the player can get back into the modloader-
        //if a mod or a combination of mods are crashing the game
        $modloader::load = true;
        if($modloader::crashFail)
        {
            confirmBox("Previous Game Session", "The last session of Starsiege terminated unexpectedly. \n\nContinue with mods loaded?");
            if($dlgresult == "[yes]"){}
            if($dlgresult == "[no]"){$modloader::load = "false";}
            if($dlgresult == "[cancel]"){deleteObject(0);}
        }
    }
    
    %m=0;
    if($modloader::load != "false")
    {
        while(strlen($modloader::mod[%m++,fileName]) != 0)
        {
            if($modloader::mod[%m,enabled] == true)
            {
                if(strAlignR(4, $modloader::mod[%m,fileName]) == ".vol" || strAlignR(4, $modloader::mod[%m,fileName]) == ".mlv")
                {
                    newObject($modloader::mod[%m,fileName], simVolume, "mods\\" @ $modloader::mod[%m,fileName]);
                    echo("MODLOADER: Loaded [mods\\" @ $modloader::mod[%m,fileName] @ "]");
                
                    if(strAlignR(4, $modloader::mod[%m,fileName]) == ".mlv")
                    {
                        repath::append("mods\\" @ strAlign(strlen($modloader::mod[%m,fileName])-4,l,$modloader::mod[%m,fileName]));
                        exec("mod_" @ strAlign(strlen($modloader::mod[%m,fileName])-4,l,$modloader::mod[%m,fileName]) @ ".cs");
                        echo("MODLOADER: Executed: [mods\\" @ $modloader::mod[%m,fileName] @ "]");
                    }
                }
                if(strAlignR(3, $modloader::mod[%m,fileName]) == ".cs")
                {
                    repath::append("mods\\" @ strAlign(strlen($modloader::mod[%m,fileName])-4,l,$modloader::mod[%m,fileName]));
                    exec("mods\\" @ $modloader::mod[%m,fileName]);
                    echo("MODLOADER: Executed [mods\\" @ $modloader::mod[%m,fileName] @ "]");
                }
            }
        }
    }
    bind( keyboard, make, sysreq, TO, "screenShot(simCanvas);");
    bind( keyboard, make, shift, "numpad+", TO, "nextRes(simCanvas);" );
    bind( keyboard, make, shift, "numpad-", TO, "prevRes(simCanvas);" );
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
    $pref::PacketRate = -1; //Max the packet rate
    
    messageCanvasDevice(simCanvas, enableCacheNoise, 0.13);
    exec( "datLoad.cs" );
    
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
}