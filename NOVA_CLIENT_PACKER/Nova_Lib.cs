$Nova::memCommit = "Mem Commit: e7f18ba5";
$Nova::Version = "Nova: Dev-Nov30-2025";
$Nversion = $version @ " " @ $Nova::Version @ " " @ $Nova::memCommit;

$consoleworld::defaultsearchpath=$basepath;

$_NotInNovaUI=true;
//Create these empty functions to avoid console spammage
function engine::endframe(){}
function game::endframe(){}
function shell::endframe(){}
function world::endframe(){}
function Nova::endframe(){}

function Engine::EndFrame()
{
    game::endframe();
    shell::endframe();
    world::endframe();
    Nova::endframe();
}

//Override the quit function with our own (To properly save config files and crash recovery)
bind(keyboard, make, alt, "F4", TO, "quit();");//Bind Alt F4 to call the quit function
function quit()
{
    //If we are not in the waitroom GUI when exiting the game then reset the server data directory
    if(!isObject(waitroomGUI))
    {
        $modloader::serverDataDirectory = "";
    }
	$pref::OpenGL::use32BitTex = false; //Incompatible with hud scaling
    export("client::*", "playerPrefs.cs");
    export("pref::*", "defaultPrefs.cs");
    $modloader::crashFail  = false;
    export("modloader::*", "Nova_Config.cs");
    Nova::purgeVehicleFiles();
	
	deleteVariables("Net::*");
	deleteVariables("Nova::HashTable*");
	deleteVariables("zzmodloader::*");
	deleteVariables("GLEX*");
	deleteVariables("GL_*");
	schedule("export(\"*\",\"clientDump.txt\");",0.05);
	
    schedule("quitGame();", "0.1");
}

//function SPmainGUI::onOpen::modloaderFunc(){schedule("opengl::goShellWindowed();",0.05);}
//function vehDepotGUI::onOpen::modloaderFunc(){schedule("opengl::goShellWindowed();",0.05);}
//function omniWebGUI::onOpen::modloaderFunc(){schedule("opengl::goShellWindowed();",0.05);}
 
function modloader::pickFile(%file)
{
    modloader::Filelist("NamedGuiSet");
}

    #Operating System Dialog window method
    // openFile("$mod::File", "","");
    // if($dlgResult == "[ok]" && isFile($mod::File))
    // {
        // deleteVariables("$ml*");
        // String::explode($mod::File, "\x5C", ml);
        // %z=-1;
        // while(strlen($ml[%z++]) != 0){}
        // if(String::Contains($ml[%z-1], ".cs") || String::Contains($ml[%z-1], ".vol") || String::Contains($ml[%z-1], ".mlv"))
        // {
            // IDFNT_FONTIS_12_4 = 00151063, "fontis12_6.pft";
            // if(modloader::checkDups($ml[%z-1]))
            // {
                // localMessagebox($ml[%z-1] @ " is already added to the modloader mod list");
                // IDFNT_FONTIS_12_4 = 00151063, "fontis12_4.pft";
                // return false;
            // }
            // if($ml[%z-2] != "mods")
            // {
                // localMessagebox("Mods can only be selected within the mods folder.");
                // IDFNT_FONTIS_12_4 = 00151063, "fontis12_4.pft";
                // return false;
            // }
            // else
            // {
                // modloader::appendMod($ml[%z-1]);
                // IDFNT_FONTIS_12_4 = 00151063, "fontis12_4.pft";
                // return;
            // }
        // }
        // localMessagebox($ml[%z-1] @ " is not a valid file for the Modloader.");
        // IDFNT_FONTIS_12_4 = 00151063, "fontis12_4.pft";
    // }
// }

//Check for duplicate mod entries
function modloader::checkDups(%file)
{
    %f=0;
    while(strlen($modloader::mod[%f++,fileName]) != 0)
    {
        if($modloader::mod[%f,fileName] == %file)
        {
            //Found duplicate file entry return true;
            return true;
        }
    }
    return false;
}

//Add the selected mod to the config
function modloader::appendMod(%file)
{
    if(modloader::checkDups(%file))
    {
        IDFNT_FONTIS_12_4 = 00151063, "fontis12_6.pft";
        localMessagebox(%file @ " is already added to the modloader mod list");
        IDFNT_FONTIS_12_4 = 00151063, "fontis12_4.pft";
        return;
    }
    
    if(!isObject(ModloaderDatabase))
    {
    newObject(ModloaderDatabase, simGroup);
    }
    %m=0;
    while(strlen($modloader::mod[%m++,fileName]) != 0){}
    deleteVariables("mod::*");
    $modloader::mod[%m,fileName] = %file;
    if($modloader::load != "false")
    {
        //Script file. Call exec();
        if(String::Right(%file, 3) == ".cs")
        {
            repath::append("mods\\" @ String::Replace(%file, ".cs", ""));
			addReplacementDirectory("mods\\" @ String::Replace(%file, ".cs", ""));
			Nova::loadTextureHashes(String::Replace(%file, ".cs", ""));
            exec("mods\\" @ %file);
        }

        //Standard volume. Call newObject();
        if(String::Right(%file, 4) == ".vol")
        {
            if(!isObject(%file))
            {
                newObject(%file, simVolume, "mods\\" @ %file);
				repath::append("mods\\" @ String::Replace(%file, ".vol", ""));
				addReplacementDirectory("mods\\" @ String::Replace(%file, ".vol", ""));
				Nova::loadTextureHashes(String::Replace(%file, ".vol", ""));
            }
        }
        
        //Volume setup specifically for modloader. Call newObject(); and exec();
        if(String::Right(%file, 4) == ".mlv")
        {
            if(!isObject(%file))
            {
                newObject(%file, simVolume, "mods\\" @ %file);
            }
            repath::append("mods\\" @ String::Replace(%file, ".mlv", ""));
			addReplacementDirectory("mods\\" @ String::Replace(%file, ".mlv", ""));
			Nova::loadTextureHashes(String::Replace(%file, ".mlv", ""));
            %scriptFile = "mod_" @ String::Replace(%file, ".mlv", ".cs");
            %scriptFile_fb = "mod_" @ String::Replace(%file, ".mlv", ".cs");
            %scriptFile = String::Replace(%scriptFile, "mod_mod_", "mod_");
            exec(%scriptFile);
            exec(%scriptFile_fb);
        }
        if(isObject(%file))
        {
            addtoset(ModloaderDatabase,%file);
        }
    }
    $modloader::mod[%m,modName] =       $mod::modName;
    $modloader::mod[%m,enabled] =       1;
    $modloader::mod[%m,description] =   $mod::description;
    $modloader::mod[%m,version] =       $mod::version;
    $modloader::mod[%m,buttonFunction] =$mod::buttonFunction;
    $modloader::mod[%m,author] =        $mod::author;
    //$modloader::mod[%m,sendToCli] =     true;
    
    // echo($modloader::mod[%m,modName]       );
    // echo($modloader::mod[%m,enabled]       );
    // echo($modloader::mod[%m,description]   );
    // echo($modloader::mod[%m,version]       );
    // echo($modloader::mod[%m,buttonFunction]);
    // echo($modloader::mod[%m,author]        );
    // echo($modloader::mod[%m,sendToCli]     );
    deleteVariables("mod::*");
    $ml::m = %m;
    modloadergui::refreshFilelist();
    modloadergui::updateGui();
    Nova::ButtonClicked();
}

function modloader::removeMod(%mod)
{
    if(String::Right($modloader::mod[%mod,fileName], 4) == ".vol" || String::Right($modloader::mod[%mod,fileName], 4) == ".mlv")
    {
        deleteObject("ModloaderDatabase\\" @ $modloader::mod[%mod,%file]);
        purgeResources();
        flushTextureCache();
        messageCanvasDevice(simcanvas, flushCache);
    }
    repath::remove("mods\\" @ String::Replace($modloader::mod[%mod,fileName], ".mlv", ""));
    $modloader::mod[%mod,fileName] =      "\x00";
    $modloader::mod[%mod,modName] =       "\x00";
    $modloader::mod[%mod,enabled] =       "\x00";
    $modloader::mod[%mod,description] =   "\x00";
    $modloader::mod[%mod,version] =       "\x00";
    $modloader::mod[%mod,buttonFunction] ="\x00";
    $modloader::mod[%mod,author] =        "\x00";
    //$modloader::mod[%mod,sendToCli] =     "\x00";
    while(strlen($modloader::mod[%mod++,fileName]) != 0)
    {
        $modloader::mod[%mod-1,fileName] =      $modloader::mod[%mod,fileName];
        $modloader::mod[%mod-1,modName] =       $modloader::mod[%mod,modName];
        $modloader::mod[%mod-1,enabled] =       $modloader::mod[%mod,enabled];
        $modloader::mod[%mod-1,description] =   $modloader::mod[%mod,description];
        $modloader::mod[%mod-1,version] =       $modloader::mod[%mod,version];
        $modloader::mod[%mod-1,buttonFunction] =$modloader::mod[%mod,buttonFunction];
        $modloader::mod[%mod-1,author] =        $modloader::mod[%mod,author];
        //$modloader::mod[%mod-1,sendToCli] =     $modloader::mod[%mod,sendToCli];
    }
    //Delete the very last mod entry to avoid duplicates
    $modloader::mod[%mod-1,fileName] =      "\x00";
    $modloader::mod[%mod-1,modName] =       "\x00";
    $modloader::mod[%mod-1,enabled] =       "\x00";
    $modloader::mod[%mod-1,description] =   "\x00";
    $modloader::mod[%mod-1,version] =       "\x00";
    $modloader::mod[%mod-1,buttonFunction] ="\x00";
    $modloader::mod[%mod-1,author] =        "\x00";
    //$modloader::mod[%mod-1,sendToCli] =     "\x00";
	
    //control::setActive(increasePri_button,false);
    //control::setActive(decreasePri_button,false);
    //control::setActive(removemod_button, false);
    modloadergui::refreshFilelist();
    if(isObject($currentGUI @ "\\modloaderUI\\mod_description_scroller\\mod_description_scroller_content\\mod_description"))
    {
        deleteobject($currentGUI @ "\\modloaderUI\\mod_description_scroller\\mod_description_scroller_content\\mod_description");
    }
    purgeResources();
    messageCanvasDevice(simcanvas, flushcache);
    modloadergui::updateGui();
    Nova::ButtonClicked();
    modloader::Filelist($currentGUI,noupdate);
 
}
    
function modloader::toggleMod(%mod)
{
    if($modloader::load == "false"){return;}
    if(!isFile("mods\\" @ $modloader::mod[%mod,fileName]))
    {
        control::setValue("mod" @ %mod @ "_checkbutton", $modloader::mod[%mod,disabled]);
        localmessagebox("Modloader is unable to locate this mod file.");
        return;
    }
    if(control::getValue("mod" @ %mod @ "_checkbutton") == false)
    {
        $modloader::mod[%mod,enabled] = 0;
        if(String::Right($modloader::mod[%mod,fileName], 4) == ".vol" || String::Right($modloader::mod[%mod,fileName], 4) == ".mlv") 
        {
            deleteObject("ModloaderDatabase\\" @ $modloader::mod[%mod,fileName]);
            repath::remove("mods\\" @ String::Replace($modloader::mod[%mod,fileName], ".mlv", ""));
            purgeResources();
            flushTextureCache();
            messageCanvasDevice(simcanvas, flushCache);
        }
        purgeResources();
        messageCanvasDevice(simcanvas, flushcache);
    }
    else
    {
        $modloader::mod[%mod,enabled] = 1;
        if(String::Right($modloader::mod[%mod,fileName], 4) == ".vol" || String::Right($modloader::mod[%mod,fileName], 4) == ".mlv") 
        {
            if(!isObject($modloader::mod[%mod,fileName]))
            {
                newObject($modloader::mod[%mod,fileName], simVolume, "mods\\" @ $modloader::mod[%mod,fileName]);
                addtoset(ModloaderDatabase, $modloader::mod[%mod,fileName]);
				
            }
        }
		
	    if(strAlignR(4, $modloader::mod[%m,fileName]) == ".vol")
        {
			repath::append("mods\\" @ strAlign(strlen($modloader::mod[%m,fileName])-4,l,$modloader::mod[%m,fileName]));
			addReplacementDirectory("mods\\" @ String::Replace($modloader::mod[%m,fileName], ".vol", ""));
			Nova::loadTextureHashes(String::Replace($modloader::mod[%m,fileName], ".vol", ""));
		}
					
        if(String::Right($modloader::mod[%mod,fileName], 4) == ".mlv")
        {
            repath::append("mods\\" @ String::Replace($modloader::mod[%mod,fileName], ".mlv", ""));
			addReplacementDirectory("mods\\" @ String::Replace($modloader::mod[%mod,fileName], ".mlv", ""));
			Nova::loadTextureHashes(String::Replace($modloader::mod[%mod,fileName], ".mlv", ""));
            %scriptFile = "mod_" @ String::Replace($modloader::mod[%mod,fileName], ".mlv", ".cs");
            %scriptFile = String::Replace(%scriptFile, "mod_mod_", "mod_");
            exec(%scriptFile);
        }
        if(String::Right($modloader::mod[%mod,fileName], 3) == ".cs")
        {
            repath::append("mods\\" @ String::Replace($modloader::mod[%mod,fileName], ".cs", ""));
			addReplacementDirectory("mods\\" @ String::Replace($modloader::mod[%mod,fileName], ".cs", ""));
			Nova::loadTextureHashes(String::Replace($modloader::mod[%mod,fileName], ".cs", ""));
            exec("mods\\" @ $modloader::mod[%mod,fileName]);
        }
    }
    $modloader::mod[%mod,enabled] = control::getValue("mod" @ %mod @ "_checkbutton");
}
function Nova::initConstructors()
{
    $zzmodloader::haltIDVars = true;
    exec( "Nova_datLoad.cs" );
    //Load the client mod list
    if($modloader::load != "false")
    {
        if(strlen($modloader::mod[1,fileName]) != 0)
        {
            modLoader::Logger::newEntry(info, "<b style=\"color:Orange;\">&#8595;&#8595;&#8595; Loading Mods &#8595;&#8595;&#8595;</b>" );
        }
        $zzmodloader::haltIDVars = false;
        while(strlen($modloader::mod[%m++,fileName]) != 0)
        {
            deleteVariables("zzmodfile*");
            String::explode($modloader::mod[%m,fileName], ".", zzmodfile);
            eval("function player::onAdd::" @ $zzmodfile[0] @ "(){}");
            eval("function player::onRemove::" @ $zzmodfile[0] @ "(){}");
            eval("function vehicle::onAdd::" @ $zzmodfile[0] @ "(){}");
            if(!isFile("mods\\" @ $modloader::mod[%m,fileName]))
            {
                $zzmodloader::missingFiles = $modloader::missingFiles @ "[" @ $modloader::mod[%m,fileName] @ "] ";
                modLoader::Logger::newEntry(warn, "Mod missing: <b> mods\\" @  $modloader::mod[%m,fileName] @ "</b>");
                $modloader::mod[%m,enabled] = 0;
                function introGUI::onClose::modLoaderFunc()
                {
                    localmessagebox("Modloader was unable to find the following mods: " @ $zzmodloader::missingFiles @ ". Those mods have been disabled.");
                    function introGUI:onClose::modLoaderFunc(){}
                }
            }
            if($modloader::mod[%m,enabled] == true)
            {
                if(strAlignR(4, $modloader::mod[%m,fileName]) == ".vol" || strAlignR(4, $modloader::mod[%m,fileName]) == ".mlv")
                {
					//If we have the volume or modVolume already loaded or not...
					if(!isObject("ModloaderDatabase/" @ $modloader::mod[%m,fileName]))
					{
						newObject($modloader::mod[%m,fileName], simVolume, $modloader::mod[%m,fileName]);
						if(isObject($modloader::mod[%m,fileName]))
						{
							modLoader::Logger::newEntry(normal, "Loaded: <b>" @  $modloader::mod[%m,fileName] @ "</b>");
						}
					}
    
	                if(strAlignR(4, $modloader::mod[%m,fileName]) == ".vol")
                    {
						repath::append("mods\\" @ strAlign(strlen($modloader::mod[%m,fileName])-4,l,$modloader::mod[%m,fileName]));
						addReplacementDirectory("mods\\" @ String::Replace($modloader::mod[%m,fileName], ".vol", ""));
						Nova::loadTextureHashes(String::Replace($modloader::mod[%m,fileName], ".vol", ""));
					}
					
                    if(strAlignR(4, $modloader::mod[%m,fileName]) == ".mlv")
                    {
                        $zzmodloader::haltIDVars = false;
						
						//Check to make sure that we don't add duplicate mod directory entries to the basepath
						if(String::findSubStr($basepath, strAlign(strlen($modloader::mod[%m,fileName])-4,l,$modloader::mod[%m,fileName])) == -1)
						{
							repath::append("mods\\" @ strAlign(strlen($modloader::mod[%m,fileName])-4,l,$modloader::mod[%m,fileName]));
							addReplacementDirectory("mods\\" @ String::Replace($modloader::mod[%m,fileName], ".mlv", ""));
							Nova::loadTextureHashes(String::Replace($modloader::mod[%m,fileName], ".mlv", ""));
						}
                        exec("mod_" @ strAlign(strlen($modloader::mod[%m,fileName])-4,l,$modloader::mod[%m,fileName]) @ ".cs");
                        exec(strAlign(strlen($modloader::mod[%m,fileName])-4,l,$modloader::mod[%m,fileName]) @ ".cs");
                        modLoader::Logger::newEntry(normal, "Executing: <b> [mod_]" @  strAlign(strlen($modloader::mod[%m,fileName])-4,l,$modloader::mod[%m,fileName]) @ ".cs" @ "</b>");
                    }
                    if(!isObject(ModloaderDatabase))
                    {
						newobject(ModloaderDatabase,simgroup);
                    }
                    addtoset(ModloaderDatabase,$modloader::mod[%m,fileName]);
                }
                if(strAlignR(3, $modloader::mod[%m,fileName]) == ".cs")
                {
                    $zzmodloader::haltIDVars = false;
                    repath::append("mods\\" @ strAlign(strlen($modloader::mod[%m,fileName])-3,l,$modloader::mod[%m,fileName]));
					addReplacementDirectory("mods\\" @ String::Replace($modloader::mod[%m,fileName], ".cs", ""));
					Nova::loadTextureHashes(String::Replace($modloader::mod[%m,fileName], ".cs", ""));
                    exec("mods\\" @ $modloader::mod[%m,fileName]);
                    modLoader::Logger::newEntry(normal, "Executing: <b> mods\\"@  $modloader::mod[%m,fileName] @ "</b>");
                }
            }
        }
    }
}

//function modloader::initServerMods()
//{
//    //Load the server mod list if we have one. Server mods have priority over client mods.
//    if($modloader::load != "false" && strlen($modloader::serverDataDirectory))
//    {
//        %PATH = String::Replace($modloader::serverDataDirectory, "/", "\\");
//        repath::append(%PATH);
//        if(strlen($servermod[1]))
//        {
//            modLoader::Logger::newEntry(info, "<b style=\"color:Orange;\">&#8595;&#8595;&#8595; Loading Server Mods &#8595;&#8595;&#8595;</b>" );
//        }
//        while(strlen($servermod[%m++]) != 0)
//        {
//            if(strAlignR(4, $servermod[%m]) == ".vol" || strAlignR(4, $servermod[%m]) == ".mlv")
//            {
//                newObject($servermod[%m], simVolume, %PATH @ "\\" @ $servermod[%m]);
//                if(isObject("'" @ $servermod[%m]) @ "'")
//                {
//                    modLoader::Logger::newEntry(normal, "Loaded: <b>" @  %PATH @ "\\" @ $servermod[%m] @ "</b>");
//                }
//
//                if(strAlignR(4, $servermod[%m]) == ".mlv")
//                {
//                    exec("mod_" @ strAlign(strlen($servermod[%m])-4,l,$servermod[%m]) @ ".cs");
//                    exec(strAlign(strlen($servermod[%m])-4,l,$servermod[%m]) @ ".cs");
//                    modLoader::Logger::newEntry(normal, "Executing: <b> [mod_]" @  strAlign(strlen($servermod[%m])-4,l,$servermod[%m]) @ ".cs" @ "</b>");
//                }
//                if(!isObject(ModloaderDatabase_ServerData))
//                {
//                    newobject(ModloaderDatabase_ServerData,simgroup);
//                }
//                addtoset(ModloaderDatabase_ServerData,"'" @ $servermod[%m] @ "'");
//            }
//            if(strAlignR(3, $servermod[%m]) == ".cs")
//            {
//                exec(%PATH @ "\\" @ $servermod[%m]);
//                modLoader::Logger::newEntry(normal, "Executing: <b> " @ %PATH @ "\\" @  $servermod[%m] @ "</b>");
//            }
//            
//        }
//    }
//}
        
 // ██████  ██    ██ ██ 
// ██       ██    ██ ██ 
// ██   ███ ██    ██ ██ 
// ██    ██ ██    ██ ██ 
 // ██████   ██████  ██ 
 
function addmod_button::onAction()
{
    //IDFNT_EDITOR = 00159996, "lucida9_4.pft";
    modloader::Filelist();
    //control::setVisible(FileList_scroller,1);
    //control::setVisible(modlist_scroller,0);
    Nova::ButtonClicked();
}
   
function modloader::Filelist()
{
    deleteVariables("directoryFile*");
    getDirectory("./mods");
    textList::clear("FileList");
    while(strlen($directoryFile[%i++]))
    {
        %fileName = $directoryFile[%i];
        while(strlen($modloader::mod[%ii++,fileName]))
        {
            //Don't show files which have already been added to the modloader
            if($modloader::mod[%ii,fileName] == $directoryFile[%i])
            {
                %dup = true;
            }
        }
        %ii=0;
        if(!%dup)
        {
            //modloader::appendMod('" @ %fileName @ "');modloader::Filelist($currentGUI);
            if(String::findSubStr(%fileName, ".cs") > 0 || String::findSubStr(%fileName, ".vol") > 0 || String::findSubStr(%fileName, ".mlv") > 0 )
            {
                textList::AddLine("FileList", %fileName);
            }
        }
        %dup = false;
    }
}

function updateScrollList(%id)
{
    newobject(scrollUpdater,simgui::control,1,9999);
    addtoset("NamedGuiSet\\" @ %id, "scrollUpdater");
    deleteObject("NamedGuiSet\\" @ %id @ "\\scrollUpdater");
}

function mainmenuGUI::onOpen::modLoaderFunc()
{   
	if($previousGUI == SPMainGUI || $previousGUI == trainingGUI || $previousGUI == splashGUI || $previousGUI == introGUI)
	{
		if(strlen($client::tempPlayerName))
		{
			$client::name = $client::tempPlayerName;
		}
		
		if(strlen($client::tempPlayerSquadName))
		{
			$client::squadName = $client::tempPlayerSquadName;
		}
		export("client::*", "playerPrefs.cs");
	}
	
	$client::tempPlayerName = $client::name;
	$client::tempPlayerSquadName = $client::squadName;
	IDSTR_TRAINEE = 00130681, _($client::name,"Trainee");
	
	Nova::setTotalWeapons($Nova::totalWeapons);
	Nova::setTotalComponents($Nova::totalComponents);
	Nova::setTotalVehicles($Nova::totalVehicles);
    if($previousGUI == SPMainGUI)
    {
        setVehicleDir("mods\\session");
    }
    
    //Don't show the training vehicles
    //repath::remove(training);
    
    if(!$zzmodloader::builtFactoryList)
    {
        $zzmodloader::builtFactoryList = true;
        modloader::buildFactoryList();
    }

    if($modloaderIDerr > 0)
    {
        IDFNT_FONTIS_12_4 = 00151063, "fontis12_6.pft";
        localMessageBox("---- VEHICLE ID CONFLICTS ----\n\nThe current mod list loadout has conflicting vehicle IDs. Refer to the MODLOADERLOG.html for more information.");
        IDFNT_FONTIS_12_4 = 00151063, "fontis12_4.pft";
        $modloaderIDerr=0;
    }
    
    //loadObject(versionText, "versionText.object");
    //schedule("addtoset(\"mainmenuGUI\", versionText);",0);
        
    //Delete TauntsVol.vol duplicates
    while(isObject(tauntsVol))
    {
        deleteObject(tauntsVol);
    }
    purgeResources();
    newObject(TauntsVol, "taunts.vol");
	
}

function slider_fov::onAction()
{
String::Explode($client::fov,".",zzf);control::setText(SLIDER_cockpitFOV_text, *IDSTR_NOVA_FOV @ ":" @ $zzf[0]);
}

function optionsGUI::onOpen::modLoaderFunc()
{
    if($pref::GWC::SIM_FS_DEVICE == "OpenGL"){Control::SetText(IDSTR_FULLSCREEN_MODE, $Nova::OpenGLDevice);}
    if(isGfxDriver(simcanvas,Glide))
    {
        if($pref::GWC::SIM_FS_DEVICE == "Glide"){Control::SetText(IDSTR_FULLSCREEN_MODE, $Nova::GlideDevice);}
        if($pref::GWC::SIM_WINDOWED_DEVICE == "Glide"){Control::SetText(IDSTR_WINDOWED_MODE, $Nova::GlideDevice);}
    }
}

function check_enforcemodloader_check::onAction()
{   
    if($pref::enforceModloader)
    {
        //$pref::enforceModloader = false;
        control::setVisible(enforceModloaderCheckerGreen, false);
    }
    else
    {
        //$pref::enforceModloader = true;
        control::setVisible(enforceModloaderCheckerGreen, true);
    }
    Nova::ButtonClicked();
    playsound(0,"sfx_spot_locking.wav", IDPRF_2D);
}

function loadingGUI::onOpen::modLoaderFunc()
{
    //if($pref::packetSize > 1000)
	//{
	//	schedule("$pref::packetSize = 1000;",0.5);
	//}
	if($pref::packetSize < 100)
	{
		$pref::packetSize = 100;
	}
	
	if($pref::packetRate < 4)
	{
		$pref::packetSize = 4;
	}
    //Set the hud scales
    HudManager::Multiplier("text",      _($pref::hudScale::text,1));
    HudManager::Multiplier("weapons",   _($pref::hudScale::weapons,1));
    HudManager::Multiplier("radar",     _($pref::hudScale::radar,1));
    HudManager::Multiplier("status",    _($pref::hudScale::status,1));
    HudManager::Multiplier("target",    _($pref::hudScale::target,1));
    HudManager::Multiplier("chat",      _($pref::hudScale::chat,1));
    HudManager::Multiplier("orders",    _($pref::hudScale::orders,1));
    HudManager::Multiplier("shields",   _($pref::hudScale::shields,1));
    HudManager::Multiplier("retical",   _($pref::hudScale::retical,1));
    HudManager::Multiplier("internals", _($pref::hudScale::internals,1));
    HudManager::Multiplier("timers",    _($pref::hudScale::timers,1));
    HudManager::Multiplier("targetlos", _($pref::hudScale::targetlos,1));
    
    //Set rendering settings
    Nova::initFarWeather();
    OpenGL::initMipMapping();
    
    // if(String::findsubstr($previousGUI, "loading") == -1)
    // {
        // %pba = connecting;
        // if($previousGUI == "spmainGUI" || $previousGUI == "recplayerGUI" || $previousGUI == "hostGUI")
        // {
            // %pba = loading;
        // }
        // schedule("OpenGL::shiftGUI(-1);guiNewContentCtrl(simcanvas, 'Simgui::TSControl');",0);
        // schedule("if(!isObject(playGUI) && !isObject(waitroomGUI) && !isObject(trainingGUI)){guiload('loading.gui');}",0.08);
        // schedule("control::setVisible('IDPBA_' @ %pba @ '_LEFT',1);",0.085);
        // schedule("control::setVisible('IDPBA_' @ %pba @ '_RIGHT',1);",0.085);
    // }
}

function loadingGUI::onClose::modLoaderFunc()
{
    if(strlen($client::fov) != 0)
    {
        fov($client::fov);
    }
    else
    {
        fov(68);
    }
    if(!isObject(MissionGroup)) //Don't allow all vehicles if we are the server host
    {
        allowVehicle(all,true);
    }
}

function loadingGUI::onOpen::modLoaderFunc()
{
	Nova::externalSetCollMeshColors();
}

function modloadergui::onOpen::SlideOut()
{
    $NovaMainFrame = "NamedGuiSet\\Nova_UI_Background";
    //control::setText("Nova_UI_Background", $Nova::Version);
    if(isObject("NamedGuiSet\\modloaderUI"))
    {
        if(!$zzSlideCloseInterrupt)
        {
            addtoset("NamedGuiSet\\slideControlFrame" @ $zzSliderFrame++, "NamedGuiSet\\NovaBackFrame", $NovaMainFrame);
            //flushTextureCache(simcanvas);
        }
    }
    if(isObject("NamedGuiSet\\slideControlFrame" @ $zzSliderFrame + 1))
    {
        schedule("modloadergui::onOpen::SlideOut();",0.0005);
    }
}

function modloadergui::onOpen::SlideIn()
{
    if(isObject("NamedGuiSet\\modloaderUI"))
    {
        $zzSlideCloseInterrupt = true;
        $zzSliderFrame-=1;
        addtoset("NamedGuiSet\\slideControlFrame" @ $zzSliderFrame, "NamedGuiSet\\NovaBackFrame", $NovaMainFrame);
        //flushTextureCache(simcanvas);
        if(isObject("NamedGuiSet\\slideControlFrame" @ $zzSliderFrame - 1))
        {
            schedule("modloadergui::onOpen::SlideIn();",0.0005);
        }
        else
        {
            guipopdialog(simcanvas,0);
            guipopdialog(simcanvas,0);

            $zzSlideCloseInterrupt = false;
            $zzSliderFrame = 0;
            if(isObject(editorGUI))
            {
                clientCursorOn();
            }
        }
    }
}

function modloader::patchConstructors(){}
function modloader::appendPilotData()
{
    %i=0;
    if($modloader::load == "false")
    {
        return;
    }
    echo("Modloader >> Appending pilot data to campaign.");
    while(strlen($modloader::mod[%i++,fileName]))
    {
        %file = $modloader::mod[%i,fileName];
        if(fileHasPilotData("mods\\" @ %file))
        {
            //Script file. Call exec();
            if(String::Right(%file, 3) == ".cs")
            {
                exec("mods\\" @ %file);
                echo("Modloader: Appending mods\\" @ %file @ " pilot data to campaign.");
            }  
            //Volume setup specifically for modloader. Call newObject(); and exec();
            if(String::Right(%file, 4) == ".mlv")
            {
                //If for some reason this mlv is not loaded then load it
                if(!isObject("ModloaderDatabase\\" @ %file))
                {
                    newObject(%file, simVolume, "mods\\" @ %file);
                }
                %scriptFile = "mod_" @ String::Replace(%file, ".mlv", ".cs");
                %scriptFile_fb = "mod_" @ String::Replace(%file, ".mlv", ".cs");
                %scriptFile = String::Replace(%scriptFile, "mod_mod_", "mod_");
                exec(%scriptFile);
                exec(%scriptFile_fb);
                echo("Modloader: Appending mods\\" @ %file @ " pilot data to campaign.");
            }
            if(isObject(%file))
            {
                addtoset(ModloaderDatabase,%file);
            }
        }
    }
}

//Init the Popup::* functions
newObject(PopupFunctionLoader,MEPopupButton);
deleteObject(PopupFunctionLoader);

//function NovaUI::setHudElementSelection()
//{
//    control::setVisible(chat_sel,       0);
//    control::setVisible(text_sel,       0);
//    control::setVisible(internals_sel,  0);
//    control::setVisible(orders_sel,     0);
//    control::setVisible(radar_sel,      0);
//    control::setVisible(retical_sel,    0);
//    control::setVisible(shields_sel,    0);
//    control::setVisible(status_sel,     0);
//    control::setVisible(weapons_sel,    0);
//    control::setVisible(timers_sel,     0);
//    control::setVisible(config_sel,     0);
//    control::setVisible(all_sel,        0);
//    control::setVisible($Nova::hudSelection @ "_sel", 1);
//}

//function chat::onAction     (){NovaUI::setHudElementSelection();}
//function text::onAction     (){NovaUI::setHudElementSelection();}
//function internals::onAction(){NovaUI::setHudElementSelection();}
//function orders::onAction   (){NovaUI::setHudElementSelection();}
//function radar::onAction    (){NovaUI::setHudElementSelection();}
//function retical::onAction  (){NovaUI::setHudElementSelection();}
//function shields::onAction  (){NovaUI::setHudElementSelection();}
//function status::onAction   (){NovaUI::setHudElementSelection();}
//function weapons::onAction  (){NovaUI::setHudElementSelection();}
//function timers::onAction   (){NovaUI::setHudElementSelection();}
//function config::onAction   (){NovaUI::setHudElementSelection();}
//function all::onAction      (){NovaUI::setHudElementSelection();}

function SetHudElementScale(%int)
{
    %h = $Nova::hudSelection;
         if(%h == chat)       {HudManager::Multiplier("chat",      %int);}
    else if(%h == text)       {HudManager::Multiplier("text",      %int);}
    else if(%h == internals)  {HudManager::Multiplier("internals", %int);}
    else if(%h == orders)     {HudManager::Multiplier("orders",    %int);}
    else if(%h == radar)      {HudManager::Multiplier("radar",     %int);}
    else if(%h == retical)    {HudManager::Multiplier("retical",   %int);}
    else if(%h == shields)    {HudManager::Multiplier("shields",   %int);}
    else if(%h == status)     {HudManager::Multiplier("status",    %int);}
    else if(%h == weapons)    {HudManager::Multiplier("weapons",   %int);}
    else if(%h == timers)     {HudManager::Multiplier("timers",    %int);}
    else if(%h == config)     {HudManager::Multiplier("config",    %int);}
    else if(%h == all)
    {
        //$_zzHudElemSel = Popup::getSelected(HudElementSel);
        HudManager::Multiplier("chat",      %int);
        HudManager::Multiplier("text",      %int);
        HudManager::Multiplier("internals", %int);
        HudManager::Multiplier("orders",    %int);
        HudManager::Multiplier("radar",     %int);
        HudManager::Multiplier("retical",   %int);
        HudManager::Multiplier("shields",   %int);
        HudManager::Multiplier("status",    %int);
        HudManager::Multiplier("weapons",   %int);
        HudManager::Multiplier("timers",    %int);
        HudManager::Multiplier("config",    %int);
    }
    if(isObject(playGUI))
    {
        //Switched from simgui::control to simgui::TScontrol
        //Nova::lastgui(); does not play nice with nameless simgui::controls as the root gui context
        //guinewcontentctrl(simcanvas, simgui::TScontrol);
        //guiload("play.gui");
        schedule("focuscamera(8);",0);
        schedule("focuscamera(player);",0);
        export("pref::*", "defaultPrefs.cs");    
    }
}

//function HudElementSel::onAction()
function HudElementSel::onSelect(%string,%index)
{
    //%HudElem = control::getText(HudElementSel);
    //if(%string == "ChatBox")               { Popup::setSelected(HudElementScale, _($pref::hudScale::chat,1));}
    //else if(%string == "Internals")        { Popup::setSelected(HudElementScale, _($pref::hudScale::internals,1));}
    //else if(%string == "Squad Orders")     { Popup::setSelected(HudElementScale, _($pref::hudScale::order,1));}
    //else if(%string == "Radar")            { Popup::setSelected(HudElementScale, _($pref::hudScale::radar,1));}
    //else if(%string == "Target System")    { Popup::setSelected(HudElementScale, _($pref::hudScale::retical,1));}
    //else if(%string == "Shields")          { Popup::setSelected(HudElementScale, _($pref::hudScale::shields,1));}
    //else if(%string == "Damage Status")    { Popup::setSelected(HudElementScale, _($pref::hudScale::status,1));}
    //else if(%string == "Weapon Display")   { Popup::setSelected(HudElementScale, _($pref::hudScale::weapons,1));}
    //else if(%string == "Sim Timers")       { Popup::setSelected(HudElementScale, _($pref::hudScale::timers,1));}
    //else if(%string == "Hud Pref. Config") { Popup::setSelected(HudElementScale, _($pref::hudScale::config,1));}
    if(%string == *IDSTR_NOVA_HUD_CHATBOX)             	{  $Nova::hudSelection = chat; }
    else if(%string == *IDSTR_NOVA_HUD_CHATBOX_TEXT)    {  $Nova::hudSelection = text; }
    else if(%string == *IDSTR_NOVA_HUD_INTERNALS)      	{  $Nova::hudSelection = internals; }
    else if(%string == *IDSTR_NOVA_HUD_SQUAD_ORDERS)   	{  $Nova::hudSelection = orders; }
    else if(%string == *IDSTR_NOVA_HUD_RADAR)          	{  $Nova::hudSelection = radar; }
    else if(%string == *IDSTR_NOVA_HUD_TARGET_SYS)    	{  $Nova::hudSelection = retical; }
    else if(%string == *IDSTR_NOVA_HUD_SHIELDS)        	{  $Nova::hudSelection = shields; }
    else if(%string == *IDSTR_NOVA_HUD_DAMAGE_STATUS)  	{  $Nova::hudSelection = status; }
    else if(%string == *IDSTR_NOVA_HUD_WEAPON_DISPLAY) 	{  $Nova::hudSelection = weapons; }
    else if(%string == *IDSTR_NOVA_HUD_TIMERS)       	{  $Nova::hudSelection = timers; }
    else if(%string == *IDSTR_NOVA_HUD_PREF_CONFIG) 	{  $Nova::hudSelection = config; }
    else if(%string == *IDSTR_NOVA_HUD_ALL) 			{  $Nova::hudSelection = all; }
}

function modloaderUI::ticker()
{
    String::Explode($client::fov,'.',zzf);control::setText(SLIDER_cockpitFOV_text, *IDSTR_NOVA_FOV @ $zzf[0]);
    fov(_($client::fov,68));
    //Nova::setTerrainDet(trunc($pref::terrainDetail));

    %terrainDetail = floor($pref::terrainDetail);
    if(%terrainDetail >= 142 && %terrainDetail <= 200)
    {
        %terrainDetailRange = *IDSTR_NOVA_UI_VERY_LOW;
    }
    else if(%terrainDetail >= 108 && %terrainDetail < 142)
    {
        %terrainDetailRange = *IDSTR_NOVA_UI_LOW;
    }
    else if(%terrainDetail >= 72 && %terrainDetail < 108)
    {
        %terrainDetailRange = *IDSTR_NOVA_UI_MEDIUM;
    }
    else if(%terrainDetail >= 38 && %terrainDetail < 72)
    {
        %terrainDetailRange = *IDSTR_NOVA_UI_HIGH;
    }
    else if(%terrainDetail < 38)
    {
        %terrainDetailRange = *IDSTR_NOVA_UI_VERY_HIGH;
    }
    
	//Terrain detail and cursor sensitivity texts
    control::setText(SLIDER_terrainDetail_text, *IDSTR_NOVA_UI_TERRAIN_DETAIL @ ": " @ %terrainDetailRange @ " (" @ trunc($pref::terrainDetail) @ ")");
    control::setText(SLIDER_cursorSensitivity_text, *IDSTR_NOVA_UI_CURSOR_SENSITIVITY @ ": " @ _($pref::cursorSensitivity,1));
 
	//Packet texts
    control::setText(SLIDER_packetframe_text, *IDSTR_NOVA_UI_PACKET_FRAME @ ": " @ trunc(_($pref::PacketFrame,32)));
    control::setText(SLIDER_packetrate_text, *IDSTR_NOVA_UI_PACKET_RATE @ ": " @ trunc(_($pref::PacketRate,10)));
    control::setText(SLIDER_packetsize_text, *IDSTR_NOVA_UI_PACKET_SIZE @ ": " @ trunc(_($pref::PacketSize,200)));
	
	//Coll Mesh Texts
	control::setText(LABEL_COLLMESH_URED_label, " R:" @ trunc(_($pref::collMeshColor::undamaged::red,135)));
	control::setText(LABEL_COLLMESH_UGREEN_label, " G:" @ trunc(_($pref::collMeshColor::undamaged::green,135)));
	control::setText(LABEL_COLLMESH_UBLUE_label, " B:" @ trunc(_($pref::collMeshColor::undamaged::blue,135)));
	
	control::setText(LABEL_COLLMESH_DRED_label, " R:" @ trunc(_($pref::collMeshColor::damaged::red,255)));
	control::setText(LABEL_COLLMESH_DGREEN_label, " G:" @ trunc(_($pref::collMeshColor::damaged::green,255)));
	control::setText(LABEL_COLLMESH_DBLUE_label, " B:" @ trunc(_($pref::collMeshColor::damaged::blue,0)));
	
	control::setText(LABEL_COLLMESH_CRED_label, " R:" @ trunc(_($pref::collMeshColor::critical::red,255)));
	control::setText(LABEL_COLLMESH_CGREEN_label, " G:" @ trunc(_($pref::collMeshColor::critical::green,0)));
	control::setText(LABEL_COLLMESH_CBLUE_label, " B:" @ trunc(_($pref::collMeshColor::critical::blue,0)));
	
    if(isObject(getnextobject(squad,0)))
    {
    postAction(getnextobject(squad,0), IDACTION_ZOOM_ADJ, -1);
    }
	
	if($consoleWorld::frameRate > 999)
	{
		$consoleWorld::frameRate = 999;
	}
	else if($consoleWorld::frameRate < 1)
	{
		$consoleWorld::frameRate = 1;
	}
	
    control::setText(fps_green,trunc($consoleWorld::frameRate) @ "FPS");
    control::setText(fps_yellow,trunc($consoleWorld::frameRate) @ "FPS");
    control::setText(fps_red,trunc($consoleWorld::frameRate) @ "FPS");
    if($consoleWorld::frameRate >= 45)
    {
        control::setVisible(fps_green,1);
        control::setVisible(fps_yellow,0);
        control::setVisible(fps_red,0);
    }
    else if($consoleWorld::frameRate < 45 && $consoleWorld::frameRate >= 21)
    {
        control::setVisible(fps_green,0);
        control::setVisible(fps_yellow,1);
        control::setVisible(fps_red,0);
    }
    else
    {
        control::setVisible(fps_green,0);
        control::setVisible(fps_yellow,0);
        control::setVisible(fps_red,1);
    }
    
	control::setText(ram_text, "RAM: " @ client::getMemUsed() @ "MB");
	
    Nova::setCursorSensitivity(_($pref::cursorSensitivity,1));
	
	control::setText(SLIDER_TerrainVisDist_text, *IDSTR_NOVA_UI_CUSTOM_TERRAIN_VIS_DIST @ ": " @ floor($pref::terrainVisDist) @ "km");
	if($pref::customTerrainVisbility)
	{
		if(isObject(8))
		{
			setTerrainVisibility(8, _($pref::terrainVisDist,2000), _($pref::terrainVisDist*0.4725,1750), 0, 0);
		}
	}
}

//function modloadergui::buildHudElementSel()
//{
//    if(isObject("NamedGuiSet\\modloaderUI"))
//    {
//        %p=0;
//        Popup::addLine(HudElementSel, *IDSTR_NOVA_HUD_CHATBOX,       %p++);
//        Popup::addLine(HudElementSel, *IDSTR_NOVA_HUD_CHATBOX_TEXT,  %p++);
//        Popup::addLine(HudElementSel, *IDSTR_NOVA_HUD_INTERNALS,     %p++);
//        Popup::addLine(HudElementSel, *IDSTR_NOVA_HUD_SQUAD_ORDERS,  %p++);
//        Popup::addLine(HudElementSel, *IDSTR_NOVA_HUD_RADAR,         %p++);
//        Popup::addLine(HudElementSel, *IDSTR_NOVA_HUD_TARGET_SYS,    %p++);
//        Popup::addLine(HudElementSel, *IDSTR_NOVA_HUD_SHIELDS,       %p++);
//        Popup::addLine(HudElementSel, *IDSTR_NOVA_HUD_DAMAGE_STATUS, %p++);
//        Popup::addLine(HudElementSel, *IDSTR_NOVA_HUD_WEAPON_DISPLAY,%p++);
//        Popup::addLine(HudElementSel, *IDSTR_NOVA_HUD_TIMERS,        %p++);
//        Popup::addLine(HudElementSel, *IDSTR_NOVA_HUD_PREF_CONFIG,   %p++);
//        Popup::addLine(HudElementSel, *IDSTR_NOVA_HUD_ALL,           %p++);
//        if(!strlen($_zzHudElemSel))
//        {
//            Control::setText(HudElementSel, *IDSTR_NOVA_HUD_ELEMENT_SEL);
//        }
//        if(strlen($_zzHudElemSel))
//        {
//            Popup::setSelected(HudElementSel,$_zzHudElemSel);
//        }
//    }
//}

function Nova::ResetOpenGLCanvas()
{
    $Opengl::scaler::RenderScale=2;
    $Opengl::scaler::RenderShift=-1;
    OpenGL::scaleGUI(2);
    OpenGL::ShiftGUI(-1);
    control::setText(RenderScaleInput, $Opengl::scaler::RenderScale);
    control::setText(RenderVertOffsetInput, $opengl::scaler::Rendershift);
}

function Nova::BuildHudElementList()
{
    if(!isObject("NamedGuiSet\\modloaderUI"))
    {
		return;
	}
    %p=0;
	//Add the lines to the popup
    Popup::addLine(HudElementSel, *IDSTR_NOVA_HUD_CHATBOX,       %p++);
    Popup::addLine(HudElementSel, *IDSTR_NOVA_HUD_CHATBOX_TEXT,  %p++);
    Popup::addLine(HudElementSel, *IDSTR_NOVA_HUD_INTERNALS,     %p++);
    Popup::addLine(HudElementSel, *IDSTR_NOVA_HUD_SQUAD_ORDERS,  %p++);
    Popup::addLine(HudElementSel, *IDSTR_NOVA_HUD_RADAR,         %p++);
    Popup::addLine(HudElementSel, *IDSTR_NOVA_HUD_TARGET_SYS,    %p++);
    Popup::addLine(HudElementSel, *IDSTR_NOVA_HUD_SHIELDS,       %p++);
    Popup::addLine(HudElementSel, *IDSTR_NOVA_HUD_DAMAGE_STATUS, %p++);
    Popup::addLine(HudElementSel, *IDSTR_NOVA_HUD_WEAPON_DISPLAY,%p++);
    Popup::addLine(HudElementSel, *IDSTR_NOVA_HUD_TIMERS,        %p++);
    Popup::addLine(HudElementSel, *IDSTR_NOVA_HUD_PREF_CONFIG,   %p++);
    Popup::addLine(HudElementSel, *IDSTR_NOVA_HUD_ALL,           %p++);
	
	//Set the selected popup value if we have one
		 if($Nova::hudSelection == chat)	  { popup::setSelected(HudElementSel, 1);  }
	else if($Nova::hudSelection == text)	  { popup::setSelected(HudElementSel, 2);  }
	else if($Nova::hudSelection == internals) { popup::setSelected(HudElementSel, 3);  }
	else if($Nova::hudSelection == orders)	  { popup::setSelected(HudElementSel, 4);  }
	else if($Nova::hudSelection == radar)	  { popup::setSelected(HudElementSel, 5);  }
	else if($Nova::hudSelection == retical)	  { popup::setSelected(HudElementSel, 6);  }
	else if($Nova::hudSelection == shields)	  { popup::setSelected(HudElementSel, 7);  }
	else if($Nova::hudSelection == status)	  { popup::setSelected(HudElementSel, 8);  }
	else if($Nova::hudSelection == weapons)	  { popup::setSelected(HudElementSel, 9);  }
	else if($Nova::hudSelection == timers)	  { popup::setSelected(HudElementSel, 10); }
	else if($Nova::hudSelection == config)	  { popup::setSelected(HudElementSel, 11); }
	else if($Nova::hudSelection == all)		  { popup::setSelected(HudElementSel, 12); }
	else								  	  { control::setText(HudElementSel, *IDSTR_NOVA_HUD_ELEMENT_SEL); }
}

function modloadergui::onOpen()
{
	if(isObject(introGUI))
	{
		return;
	}
	if($_campaignPaused)
	{
		return;
	}
	
	if(isObject("NamedGuiSet\\modloaderUI"))
    {
		modloadergui::onClose();
		return;
	}
	
    $_zzNovaUIsettingVertOffset=30;
    if(!isObject("NamedGuiSet\\modloaderUI"))
    {
		$_NotInNovaUI = false;
		appendSearchPath();
		$MEtextList::hlColorIndex = 0xFB;
        $zzInUpscaleConfig = true;
        IDPBA_SCROLL_DEFAULT = 00179998, "scroll.pba";
        IDFNT_EDITOR         = 00159996, "lucida9_3.pft";
        schedule("Nova::BuildHudElementList();",0);
		
        //guipushdialog(simcanvas, "ModloaderUI.dlg");
        guipushdialog(simcanvas, "NovaUI.dlg");
        schedule("UPCTicker();", 0.025);
        $ml:m=1;
        //control::setText(modloader_version,$zzmodloader::version);
        modloadergui::refreshFilelist();
        
        //Disable the special factory vehicles if we are a campaign
        if(isCampaign())
        {
            Control::setVisible(check_showdronevehicles, false);
            Control::setActive(check_showdronevehicles, false);
        }
        
		$hudTitleWin::opacity = 0.7;
		$hudTitleWin::colorIndex = 0;
		
		if(isObject(playGUI))
		{
			$engine::testButtonFillColor = 0;
			$engine::testButtonBorderColor = 241;
			$engine::testButtonSelectColor = 244;
			$engine::testButtonFillOpacity = -1;
			$engine::sliderTabColor = 247;
		}

		else
		{
			$engine::testButtonFillColor = 0;
			//$engine::testButtonBorderColor = 238;
			//$engine::testButtonSelectColor = 237;
			$engine::testButtonBorderColor = 233;
			$engine::testButtonSelectColor = 245;
			$engine::testButtonFillOpacity = -1;
			$engine::sliderTabColor = 236;
		}
	
		//Init our SciptGL render contexts
		//newObject(ScriptGL_Nova1, Simgui::TScontrol, 0, 0, 200, 420);
		//addToSet("NamedGuiSet\\ScriptGL_Container1", ScriptGL_Nova1);
		//postAction("NamedGuiSet\\ScriptGL_Nova1", "Attach", 655);
		//postAction("NamedGuiSet\\ScriptGL_Nova1", "Attach", 655);
		
		//newObject(ScriptGL_Nova2, Simgui::TScontrol, 0, 0, 256, 480);
		//addToSet("NamedGuiSet\\ScriptGL_Container2", ScriptGL_Nova2);
		//postAction("NamedGuiSet\\ScriptGL_Nova2", "Attach", 655);
		
        //Slide out animation
        //modloadergui::onOpen::SlideOut();
        
        if(isObject(playGUI))
        {
            clientcursoron();
        }
        bind( keyboard0, make, escape, TO, "modloadergui::onClose();" );
        bind( keyboard0, make, control, "r", TO, "Nova::ResetOpenGLCanvas();" );
    
        updateScrollList("modloaderUI\\mod_description_scroller\\mod_description_scroller_content");
        updateScrollList("modloaderUI\\modlistWindow\\modlist_scroller\\modlist_scroller_content");
        if(isObject(37))
        {
            control::setActive(removemod_button,0);
            control::setVisible(removemod_button,0);
            control::setActive(addmod_popup,0);
            control::setActive(addmod_button,0);
            control::setVisible(addmod_button,0);
            control::setActive(check_enforcemodloader,0);
            control::setActive(ClearCache_btn, 0);
            control::setVisible(ClearCache_btn, 0);
            while(strlen($modloader::mod[%m++, filename]) != 0)
            {
                control::setActive("mod" @ %m @ "_checkbutton1",0);
                //control::setActive("mod" @ %m @ "_checkbutton2",0);
                control::setVisible("mod" @ %m @ "_checkbutton1",0);
                //control::setVisible("mod" @ %m @ "_checkbutton2",0);
                control::setVisible("mod" @ %m @ "_txt_sendToCli",0);
                control::setVisible("mod" @ %m @ "_remove",0);
            }
        }

    modloader::FileList();
    schedule("NovaUI::initSettingsButtons();", 0);
    }
    else
    {
        sfxAddPair( IDSFX_ROLLOVER, IDPRF_2D, "mous_ovr.wav" );
		guipopdialog(simcanvas,0);
		guipopdialog(simcanvas,0);
    }
}

function modloadergui::onClose()
{
	export("pref::*", "defaultPrefs.cs");
	export("client::*", "playerPrefs.cs");
	//$suspended=0;
    IDFNT_EDITOR         = 00159996, "mefont.pft";
    if(isObject("NamedGuiSet\\modloaderUI"))
    {
		$_NotInNovaUI = true;
        IDPBA_SCROLL_DEFAULT = 00179998, "darkscroll.pba";
        //if(control::getVisible(FileList_scroller))
        //{
        //    control::setVisible(FileList_scroller,0);
        //    control::setVisible(modlist_scroller,1);
        //    return;
        //}
        //if(control::getVisible(cccc))
        //{
        //    control::setVisible(FileList_scroller,0);
        //    control::setVisible(modlist_scroller,1);
        //    control::setVisible(cccc,0);
        //    control::setVisible(cccb,0);
        //    return;
        //}
        unbind( keyboard0, make, escape);
        schedule("flushTextureCache(simcanvas);",0);
        sfxAddPair( IDSFX_ROLLOVER, IDPRF_2D, "mous_ovr.wav" );
        export("$modloader::*", "Nova_Config.cs");
        //Slide animation
        //modloadergui::onOpen::SlideIn();
		guipopdialog(simcanvas,0);
		//guipopdialog(simcanvas,0);
        $zzkb=0;
        control::setVisible(KBs,0);
    }
    unbind( keyboard0, make, escape);
    unbind( keyboard0, make, control, "r");
    deleteFunctions("Nova::ResetOpenGLCanvas");
    $zzInUpscaleConfig = false;
	$MEtextList::hlColorIndex = 0xFE;
	if(isObject(editorGUI))
    {
        clientCursorOn();
    }
	Nova::externalSetCollMeshColors();
}

//Short hand function for nested GUI commands
function Nova::BtnClk()
{
    Nova::ButtonClicked();
}

function Nova::ButtonClicked()
{
    playsound(0,"nova_button.wav", IDPRF_2D);
}

function modloadergui::updateGui()
{
    if(isObject("NamedGuiSet\\modloaderUI\\mod_description_scroller\\mod_description_scroller_content\\mod_description"))
    {
        deleteobject("NamedGuiSet\\modloaderUI\\mod_description_scroller\\mod_description_scroller_content\\mod_description");
    }
    if(strlen($ml:m))
    {
        control::setActive(removemod_button, true);
    }
    %m=0;
    while(strlen($modloader::mod[%m++,fileName]))
    {
        if(strlen($modloader::mod[%m,version]))
        {
            control::setText("mod" @ %m @ "_txt_version", "v" @ $modloader::mod[%m,version]);
            control::setVisible("mod" @ %m @ "_txt_version", 1); 
        }
        else
        {
            control::setVisible("mod" @ %m @ "_txt_version", 0);  
        }
        if($ml::m == %m)
        {
            control::setVisible("mod" @ %m @ "_selected", 1);
            control::setVisible("mod" @ %m @ "_unselected", 0);
        }
        else
        {
            control::setVisible("mod" @ %m @ "_selected", 0);
            control::setVisible("mod" @ %m @ "_unselected", 1);
        }
    }
    if(strlen($modloader::mod[$ml::m,description]) != 0)
    {
        //$mldsc = $modloader::mod[$ml::m,description];
        IDSTR_CO_CHEETAH         		   = 00130001, $modloader::mod[$ml::m,description];
        loadObject("mod_description", "ml_mod_dsc.object");
        addtoset("NamedGuiSet\\modloaderUI\\mod_description_scroller\\mod_description_scroller_content", "mod_description");
        flushtexturecache();
    }
    control::setValue("mod" @ $ml::m @ "_checkbutton", $modloader::mod[$ml::m,enabled]);
    control::setActive(removemod_button, true);
    //control::setActive(increasePri_button,true);
    //control::setActive(decreasePri_button,true);
    updateScrollList("mod_description_scroller\\mod_description_scroller_content");
}

function modloadergui::refreshFilelist()
{
    //Scan the modlist scroll content and delete everything inside it
    while(getNextObject("NamedGuiSet\\modloaderUI\\modlistWindow\\modlist_scroller\\modlist_scroller_content", 0) != 0)
	{
		deleteObject(getNextObject("NamedGuiSet\\modloaderUI\\modlistWindow\\modlist_scroller\\modlist_scroller_content", 0));
	}
    %m=0;
    %v=2;
    while(strlen($modloader::mod[%m++,fileName]) != 0)
    {
        deletefunctions("mod" @ %m @ "_button::onAction");
        deletefunctions("mod" @ %m @ "_altbutton::onAction");
        
        
        newobject("mod" @ %m @ "_button_container", simgui::control, 0, %v, 245, 47);
        
        //newobject("mod" @ %m @ "_buttonBackground", simgui::guilistlabel, 0, 0, 198,32);
        //loadobject("mod" @ %m @ "_selected", "ml_mod_selected.object");
        
        
        //loadobject("mod" @ %m @ "_txt_author", "ml_text_author.object"); DEPRECATED
        SimGui::setFontTags(IDFNT_LUCIDA_7_2, IDFNT_FONT_DEFAULT, IDFNT_FONT_DEFAULT);
        newObject("mod" @ %m @ "_txt_author", SimGui::SimpleText, 4, -3, 245, 15);
        
        //loadobject("mod" @ %m @ "_txt_version", "ml_text_version.object"); DEPRECATED
        SimGui::setFontTags(IDFNT_LUCIDA_7_5, IDFNT_FONT_DEFAULT, IDFNT_FONT_DEFAULT);
        newObject("mod" @ %m @ "_txt_version", SimGui::SimpleText, 4, 31, 60, 13);
        
        //loadobject("mod" @ %m @ "_txt_fileName", "ml_text_fileName.object"); DEPRECATED 
        SimGui::setFontTags(IDFNT_LUCIDA_7_5, IDFNT_FONT_DEFAULT, IDFNT_FONT_DEFAULT);
        newObject("mod" @ %m @ "_txt_fileName", SimGui::SimpleText, 4, 16, 245, 15);
        
        //loadobject("mod" @ %m @ "_txt_modName", "ml_text_modName.object"); DEPRECATED
        SimGui::setFontTags(IDFNT_LUCIDA_8_4, IDFNT_FONT_DEFAULT, IDFNT_FONT_DEFAULT);
        newObject("mod" @ %m @ "_txt_modName", SimGui::SimpleText, 4, 6, 245, 38);
        
        //loadobject("mod" @ %m @ "_txt_active", "ml_text_modActive.object"); //DEPRECATED
        
        //loadobject("mod" @ %m @ "_txt_sendToCli", "ml_text_sendToCli.object"); DEPRECATED
		
        //SimGui::setFontTags(IDFNT_LUCIDA_7_1, IDFNT_FONT_DEFAULT, IDFNT_FONT_DEFAULT);
        //newObject("mod" @ %m @ "_txt_sendToCli", SimGui::SimpleText, 4, 29, 68, 13);
        //control::setText("mod" @ %m @ "_txt_sendToCli", *IDSTR_NOVA_UI_SEND_TO_CLIENTS);
        
        SimGui::setFontTags(IDFNT_FONTIS_12_6, IDFNT_FONTIS_12_1, IDFNT_FONT_DEFAULT);
		
		if(!isObject(37))
        {
			newObject("mod" @ %m @ "_remove", SimGui::TestButton, 222, -4, 16, 18, "", "schedule('modloader::removeMod(" @ %m @ ");',0.05);");
		}
        
        if(!isObject(playGUI))
        {
            loadobject("mod" @ %m @ "_selected", "ml_bmp_selected.object");
            loadobject("mod" @ %m @ "_unselected", "ml_bmp_unselected.object");
        }
        else
        {
            loadobject("mod" @ %m @ "_selected", "ml_bmp_selected_SIM.object");
            loadobject("mod" @ %m @ "_unselected", "ml_bmp_unselected_SIM.object");
        }
        control::setVisible("mod" @ %m @ "_selected",0);
        control::setVisible("mod" @ %m @ "_unselected",1);
        //newobject("mod" @ %m @ "_button", simgui::hudbutton, -1, 0, 200, 47, "", "$ml::m=" @ %m @ ";modloadergui::updateGui();Nova::ButtonClicked();");
        newobject("mod" @ %m @ "_button", simgui::testbutton, 0, 0, 245, 47, "", "$ml::m=" @ %m @ ";modloadergui::updateGui();Nova::ButtonClicked();");
        control::setText("mod" @ %m @ "_button", "");
		
		if(!isObject(37))
        {
			control::setText("mod" @ %m @ "_remove", "X");
		}
        //newobject("mod" @ %m @ "_checkbutton1", MECheck, 120, 33, 11, 11, "modloader::mod" @ %m @ "_enabled", "modloadergui::updateGui();modloader::toggleMod(" @ %m @ ");");//Button mod toggle checkbox
		
        //newobject("mod" @ %m @ "_checkbutton2", MECheck, 74, 33, 11, 11, "modloader::mod" @ %m @ "_sendToCli");//Button mod toggle checkbox
		
        //control::setValue("mod" @ %m @ "_checkbutton1", $modloader::mod[%m,enabled]);
        //control::setValue("mod" @ %m @ "_checkbutton2", $modloader::mod[%m,sendToCli]);
        
        //addtoset("mod" @ %m @ "_button_container","mod" @ %m @ "_buttonBackground");
        //addtoset("mod" @ %m @ "_button_container","mod" @ %m @ "_selected");
        addtoset("mod" @ %m @ "_button_container","mod" @ %m @ "_selected");
        addtoset("mod" @ %m @ "_button_container","mod" @ %m @ "_unselected");
        addtoset("mod" @ %m @ "_button_container","mod" @ %m @ "_txt_author");
        addtoset("mod" @ %m @ "_button_container","mod" @ %m @ "_txt_version");
        addtoset("mod" @ %m @ "_button_container","mod" @ %m @ "_txt_fileName");
        addtoset("mod" @ %m @ "_button_container","mod" @ %m @ "_txt_modName");
        //addtoset("mod" @ %m @ "_button_container","mod" @ %m @ "_txt_active"); DEPRECATED
        //addtoset("mod" @ %m @ "_button_container","mod" @ %m @ "_txt_sendToCli");
        //Now overlay the hud button over the buttonContainer labels
        addtoset("mod" @ %m @ "_button_container","mod" @ %m @ "_button");
        //The checkbox/FunctionButton must overlay the hud button so the user can click them
        //addtoset("mod" @ %m @ "_button_container","mod" @ %m @ "_checkbutton1"); DEPRECATED
        //addtoset("mod" @ %m @ "_button_container","mod" @ %m @ "_checkbutton2");

        if(!isObject(37))
        {
			addToSet("mod" @ %m @ "_button_container","mod" @ %m @ "_remove");
		}
		
        control::setValue("mod" @ %m @ "_checkbutton", $modloader::mod[%m,enabled]);
        if($ml::m == %m)
        {
            control::setVisible("mod" @ %m @ "_selected", 1);
            control::setVisible("mod" @ %m @ "_unselected", 0);
        }
        else
        {
            control::setVisible("mod" @ %m @ "_selected", 0);
            control::setVisible("mod" @ %m @ "_unselected", 1);
        }
        // if(strlen($modloader::mod[%m,buttonFunction]) != 0)
        // {
            // loadobject("mod" @ %m @ "_altbutton", "ml_btn_modAltbutton.object");
            // addtoset("mod" @ %m @ "_button_container","mod" @ %m @ "_altbutton");
            // #Create our action functions
            // eval("function " @ "mod" @ %m @ "_altbutton::onAction(){"@ $modloader::mod[%m,buttonFunction] @ "}");
        // }
        
        //Version var check
        if(strlen($modloader::mod[%m,version]) != 0)
        {
            control::setText("mod" @ %m @ "_txt_version", "v" @ $modloader::mod[%m,version]);
            control::setVisible("mod" @ %m @ "_txt_version", 1);
        }       
        else
        {
            control::setVisible("mod" @ %m @ "_txt_version", 0);
        }
        
        //Author var check
        if(strlen($modloader::mod[%m,author]) != 0)
        {
            control::setText("mod" @ %m @ "_txt_author", "Author: " @ $modloader::mod[%m,author]);
            control::setVisible("mod" @ %m @ "_txt_author", 1);
        }       
        else
        {
            control::setVisible("mod" @ %m @ "_txt_author", 0);
        }
        //Mod name var check
        if(strlen($modloader::mod[%m,modName]) != 0)
        {
            control::setText("mod" @ %m @ "_txt_modName", $modloader::mod[%m,modName]);
            control::setText("mod" @ %m @ "_txt_fileName", $modloader::mod[%m,fileName]);
        }
        else
        {
            control::setText("mod" @ %m @ "_txt_modName", $modloader::mod[%m,fileName]);
            control::setText("mod" @ %m @ "_txt_fileName", $modloader::mod[%m,fileName]);
            control::setVisible("mod" @ %m @ "_txt_fileName",0);
            
        }
        
        //Load priority buttons
        
        if(!isObject(37))
        {
            if(%m > 1)
            {
                SimGui::setFontTags(IDFNT_FONTIS_26_4, IDFNT_FONTIS_26_1, IDFNT_FONT_DEFAULT);
                newObject("mod" @ %m @ "_decreasePri", SimGui::TestButton, 224, 15, 12, 14, "", "modloadergui::adjustLoadPriority(decrease, " @ %m @ ");");
                control::setText("mod" @ %m @ "_decreasePri", "W");
                addtoset("mod" @ %m @ "_button_container","mod" @ %m @ "_decreasePri");
            }
            if(strlen($modloader::mod[%m+1,fileName]))
            {
                SimGui::setFontTags(IDFNT_FONTIS_18_4, IDFNT_FONTIS_18_1, IDFNT_FONT_DEFAULT);
                newObject("mod" @ %m @ "_increasePri", SimGui::TestButton, 224, 30, 12, 14, "", "modloadergui::adjustLoadPriority(increase, " @ %m @ ");");
                control::setText("mod" @ %m @ "_increasePri", "V");
                addtoset("mod" @ %m @ "_button_container","mod" @ %m @ "_increasePri");
            }
        }
        addtoset("NamedGuiSet\\modloaderUI\\modlistWindow\\modlist_scroller\\modlist_scroller_content", "mod" @ %m @ "_button_container");
        %v+=50;
        //control::setText("mod" @ %m @ "_buttonBackground", "");
    }
    updateScrollList("modloaderUI\\modlistWindow\\modlist_scroller\\modlist_scroller_content");
}

function modloader::fileIsLoaded(%filename)
{
    %i=0;
    while(strlen($modloader::mod[%i++,filename]))
    {
        if($modloader::mod[%i,filename] == %filename && $modloader::mod[%i,enabled])
        {
            return true;
        }
    }
    return false;
}

function modloadergui::adjustLoadPriority(%direction, %mod)
{
    if(%direction == increase)
    {
        if(strlen($modloader::mod[%mod+1,fileName]) == 0)
        {
            return false;
        }
        //Store the mod-to-be-swapped to temporary local variables
        %modloader::targetmod[%mod,fileName] =      $modloader::mod[%mod+1,fileName];
        %modloader::targetmod[%mod,modName] =       $modloader::mod[%mod+1,modName];
        %modloader::targetmod[%mod,enabled] =       $modloader::mod[%mod+1,enabled];
        %modloader::targetmod[%mod,description] =   $modloader::mod[%mod+1,description];
        %modloader::targetmod[%mod,version] =       $modloader::mod[%mod+1,version];
        %modloader::targetmod[%mod,buttonFunction] =$modloader::mod[%mod+1,buttonFunction];
        %modloader::targetmod[%mod,author] =        $modloader::mod[%mod+1,author];
        //%modloader::targetmod[%mod,sendToCli] =     $modloader::mod[%mod+1,sendToCli];
        $modloader::mod[%mod+1,fileName] =          "\x00";
        $modloader::mod[%mod+1,modName] =           "\x00";
        $modloader::mod[%mod+1,enabled] =           "\x00";
        $modloader::mod[%mod+1,description] =       "\x00";
        $modloader::mod[%mod+1,version] =           "\x00";
        $modloader::mod[%mod+1,buttonFunction] =    "\x00";
        $modloader::mod[%mod+1,author] =            "\x00";
        //$modloader::mod[%mod+1,sendToCli] =         "\x00";
        
        //Increase the selected mods load priority array
        $modloader::mod[%mod+1,fileName] =      $modloader::mod[%mod,fileName];
        $modloader::mod[%mod+1,modName] =       $modloader::mod[%mod,modName];
        $modloader::mod[%mod+1,enabled] =       $modloader::mod[%mod,enabled];
        $modloader::mod[%mod+1,description] =   $modloader::mod[%mod,description];
        $modloader::mod[%mod+1,version] =       $modloader::mod[%mod,version];
        $modloader::mod[%mod+1,buttonFunction] =$modloader::mod[%mod,buttonFunction];
        $modloader::mod[%mod+1,author] =        $modloader::mod[%mod,author];
        //$modloader::mod[%mod+1,sendToCli] =     $modloader::mod[%mod,sendToCli];
        
        //Set the mod-to-be-swapped array to what the selected mod was
        $modloader::mod[%mod,fileName] =      %modloader::targetmod[%mod,fileName];
        $modloader::mod[%mod,modName] =       %modloader::targetmod[%mod,modName];
        $modloader::mod[%mod,enabled] =       %modloader::targetmod[%mod,enabled];
        $modloader::mod[%mod,description] =   %modloader::targetmod[%mod,description];
        $modloader::mod[%mod,version] =       %modloader::targetmod[%mod,version];
        $modloader::mod[%mod,buttonFunction] =%modloader::targetmod[%mod,buttonFunction];
        $modloader::mod[%mod,author] =        %modloader::targetmod[%mod,author];
        //$modloader::mod[%mod,sendToCli] =     %modloader::targetmod[%mod,sendToCli];
        $ml::m+=1;
    }
    if(%direction == decrease)
    {
        if(strlen($modloader::mod[%mod-1,fileName]) == 0)
        {
            return false;
        }
        //Store the mod-to-be-swapped to temporary local variables
        %modloader::targetmod[%mod,fileName] =      $modloader::mod[%mod-1,fileName];
        %modloader::targetmod[%mod,modName] =       $modloader::mod[%mod-1,modName];
        %modloader::targetmod[%mod,enabled] =       $modloader::mod[%mod-1,enabled];
        %modloader::targetmod[%mod,description] =   $modloader::mod[%mod-1,description];
        %modloader::targetmod[%mod,version] =       $modloader::mod[%mod-1,version];
        %modloader::targetmod[%mod,buttonFunction] =$modloader::mod[%mod-1,buttonFunction];
        %modloader::targetmod[%mod,author] =        $modloader::mod[%mod-1,author];
        //%modloader::targetmod[%mod,sendToCli] =     $modloader::mod[%mod-1,sendToCli];
        $modloader::mod[%mod-1,fileName] =          "\x00";
        $modloader::mod[%mod-1,modName] =           "\x00";
        $modloader::mod[%mod-1,enabled] =           "\x00";
        $modloader::mod[%mod-1,description] =       "\x00";
        $modloader::mod[%mod-1,version] =           "\x00";
        $modloader::mod[%mod-1,buttonFunction] =    "\x00";
        $modloader::mod[%mod-1,author] =            "\x00";
        //$modloader::mod[%mod-1,sendToCli] =         "\x00";
        
        //Increase the selected mods load priority array
        $modloader::mod[%mod-1,fileName] =      $modloader::mod[%mod,fileName];
        $modloader::mod[%mod-1,modName] =       $modloader::mod[%mod,modName];
        $modloader::mod[%mod-1,enabled] =       $modloader::mod[%mod,enabled];
        $modloader::mod[%mod-1,description] =   $modloader::mod[%mod,description];
        $modloader::mod[%mod-1,version] =       $modloader::mod[%mod,version];
        $modloader::mod[%mod-1,buttonFunction] =$modloader::mod[%mod,buttonFunction];
        $modloader::mod[%mod-1,author] =        $modloader::mod[%mod,author];
        //$modloader::mod[%mod-1,sendToCli] =     $modloader::mod[%mod,sendToCli];
        
        //Set the mod-to-be-swapped array to what the selected mod was
        $modloader::mod[%mod,fileName] =      %modloader::targetmod[%mod,fileName];
        $modloader::mod[%mod,modName] =       %modloader::targetmod[%mod,modName];
        $modloader::mod[%mod,enabled] =       %modloader::targetmod[%mod,enabled];
        $modloader::mod[%mod,description] =   %modloader::targetmod[%mod,description];
        $modloader::mod[%mod,version] =       %modloader::targetmod[%mod,version];
        $modloader::mod[%mod,buttonFunction] =%modloader::targetmod[%mod,buttonFunction];
        $modloader::mod[%mod,author] =        %modloader::targetmod[%mod,author];
        //$modloader::mod[%mod,sendToCli] =     %modloader::targetmod[%mod,sendToCli];
        $ml::m-=1;
    }
    modloadergui::refreshFilelist();
    Nova::ButtonClicked();
}

function Nova::unescape(%var)
{
    if(strlen(%var))
    {
        %var = eval("return " @ %var @ ";");
        return %var;
    }
    return false;
}

IDFNT_EDITOR_ALT = 00159995, "mefont.pft";

function NovaUI::addSetting(%name, %text, %font, %buttonType, %var, %function, %helpTag, %extraArg1)
{
	if(isObject(introGUI))
	{
		SimGui::setFontTags(IDFNT_EDITOR_ALT,IDFNT_EDITOR_ALT,IDFNT_EDITOR_ALT);
	}
	else
	{
		SimGui::setFontTags(%font,%font,%font);
	}
    %errSelf = "NovaUI::addSetting: ";
    if(strlen(%function)>80){echo(%errSelf, "%function exceeds 80 characters.");return;}
    else if (%buttonType != check && %buttonType != button && %buttonType != label && %buttonType != textEntry)
    {
        echo(%errSelf, "UNKNOWN %buttonType. (Valid types are check, button, label, slider)");
        return;
    }
    else if(isObject("NamedGuiSet\\NovaUIsettingsInner\\" @ %name @ "_container"))
    {
        echo(%errSelf, %name, " already exists in the NovaUI settings.");
        return;
    }
    if(%buttonType == check) //check types have a fixed width and height
    {
        newObject(%name @ "_container", Simgui::Control, 10, $_zzNovaUIsettingVertOffset+=14, 250, 12);
        newObject(%name @ "_text", Simgui::SimpleText, 14,-4, 245, 16);
        control::setText(%name @ "_text", %text);
        addToSet(%name @ "_container", %name @ "_text");
        newObject(%name @ "_checkContainer", Simgui::Control, 0, 0, 12, 12);
        newObject(%name @ "_check", MEcheck, 0, 0, 11, 11, %var, %function);
        %evalVar = eval("return " @ %var @ ";"); //Unescape the escaped variable
        if(Nova::unescape(%var) == true || Nova::unescape(%var) == 1)
        {
            control::setValue(%name @ "_check", true); //Set the checkbox state
        }
        addToSet(%name @ "_checkContainer", %name @ "_check");
        addToSet(%name @ "_container", %name @ "_checkContainer");
		if(strlen(%helpTag))
		{
			SimGui::setFontTags(IDFNT_FONTIS_10_4,IDFNT_FONTIS_10_2,IDFNT_FONT_DEFAULT);
			newObject(%name @ "_helpButton", SimGui::TestButton, 164, -11, 14, 30, "", "localMessageBox(*" @ %helpTag @ ");");
			control::setText(%name @ "_helpButton", "?");
			addToSet(%name @ "_container", %name @ "_helpButton");
		}
    }
    
    else if(%buttonType == button)
    {
        newObject(%name @ "_container", Simgui::Control, 4, $_zzNovaUIsettingVertOffset+=14, 250, 20);
		SimGui::setFontTags(IDFNT_LUCIDA_8_3,IDFNT_LUCIDA_8_2,IDFNT_FONT_DEFAULT);
        newObject(%name @ "_button", SimGui::TestButton, 4, 0, 164, 20, %var, %function);
        control::setText(%name @ "_button", %text);
        addToSet(%name @ "_container", %name @ "_button");
		if(strlen(%helpTag))
		{
			SimGui::setFontTags(IDFNT_FONTIS_10_4,IDFNT_FONTIS_10_2,IDFNT_FONT_DEFAULT);
			newObject(%name @ "_helpButton", SimGui::TestButton, 170, -7, 14, 30, "", "localMessageBox(*" @ %helpTag @ ");");
			control::setText(%name @ "_helpButton", "?");
			addToSet(%name @ "_container", %name @ "_helpButton");
		}
    }
    
	else if(%buttonType == textEntry)
    {
		newObject(%name @ "_container", Simgui::Control, 0, $_zzNovaUIsettingVertOffset+=28, 250, 28);
		newObject(%name @ "_activeControl", Simgui::ActiveCtrl, 5, -4, 240, 27);
		newObject(%name @ "_text", Simgui::SimpleText, 0,0, 245, 15);
		
		control::setText(%name @ "_text", %text);
		
		addToSet(%name @ "_activeControl", %name @ "_text");
		newObject(%name @ "_container2", Simgui::Control, 10, 11, 160, 16);
		
		IDFNT_EDITOR = 00159996, *IDFNT_HUD_HIGH_RES_ALT;
		IDFNT_EDITOR_HILITE = 00159997, *IDFNT_HUD_HIGH_RES_GRAY_ALT;
		newObject(%name @ "_textEntry", MeTextEdit, 0, 0, 160, 16, %var, %function);
		IDFNT_EDITOR = 00159996, "lucida9_3.pft";
		IDFNT_EDITOR_HILITE = 00159997, "mefonthl.pft";
	
		//newObject(%name @ "_slider", Simgui::GuiSlider, -12, -5, 165, 35, %var, %function); //Simgui::GuiSlider
		addToSet(%name @ "_container2", %name @ "_textEntry");
		addToSet(%name @ "_container",  %name @ "_activeControl", %name @ "_container2");
		if(strlen(%helpTag))
		{
			if(isObject(introGUI))
			{
				SimGui::setFontTags(IDFNT_EDITOR_ALT,IDFNT_EDITOR_ALT,IDFNT_EDITOR_ALT);
			}
			else
			{
				SimGui::setFontTags(IDFNT_FONTIS_10_4,IDFNT_FONTIS_10_2,IDFNT_FONT_DEFAULT);
			}
			newObject(%name @ "_helpButton", SimGui::TestButton, 164, -11, 14, 30, "", "localMessageBox(*" @ %helpTag @ ");");
			control::setText(%name @ "_helpButton", "?");
			addToSet(%name @ "_container", %name @ "_helpButton");
		}
    }
	
    else if(%buttonType == label)
    {
        newObject(%name @ "_container", Simgui::Control, 8, $_zzNovaUIsettingVertOffset+=14, 250, 16);
        newObject(%name @ "_label", SimGui::SimpleText, 0, 0, 245, 25);
        control::setText(%name @ "_label", %text);
        addToSet(%name @ "_container", %name @ "_label");
		if(%var != nofooter)
		{
			loadObject(%name @ "_footer", "label_footer.object");
			addToSet(%name @ "_container", %name @ "_footer");
		}
    }
    if(isObject(%name @ "_container"))
    {
        addToSet("NamedGuiSet\\NovaUIsettingsInnerScrollerContent", %name @ "_container");
        updateScrollList("NovaUIsettingsInnerScrollerContent");
    }
}

//IDFNT_LUCIDA_7_3
function NovaUI::addSliderSetting(%name, %text, %font, %min, %max, %currentValue, %var, %function, %helpTag, %extraArg1)
{
	if(isObject(introGUI))
	{
		SimGui::setFontTags(IDFNT_EDITOR_ALT,IDFNT_EDITOR_ALT,IDFNT_EDITOR_ALT);
	}
	else
	{
		SimGui::setFontTags(%font,%font,%font);
	}
    %errSelf = "NovaUI::addSliderSetting: ";
    if(!isNumeric(%min)){echo(%errSelf, "%min MUST BE NUMERIC.");return;}
    else if(!isNumeric(%max)){echo(%errSelf, "%max MUST BE NUMERIC.");return;}
    else if(strlen(%function)>80){echo(%errSelf, "%function exceeds 80 characters.");return;}
    Simgui::Slider::SetMinMax(%min, %max, %currentValue);
    
	if(strlen(%extraArg1))
	{
		newObject(%name @ "_container", Simgui::Control, 0, $_zzNovaUIsettingVertOffset+=15, 250, 11);
		newObject(%name @ "_activeControl", Simgui::ActiveCtrl, 10, -4, 240, 0);
	}
	else
	{
		newObject(%name @ "_container", Simgui::Control, 0, $_zzNovaUIsettingVertOffset+=25, 250, 22);
		newObject(%name @ "_activeControl", Simgui::ActiveCtrl, 10, -4, 240, 27);
	}
	//if(strlen(%text))
	//{
		newObject(%name @ "_text", Simgui::SimpleText, 0,0, 245, 15);
	//}
    
	if(strlen(%text))
	{
		control::setText(%name @ "_text", %text);
	}
    
	addToSet(%name @ "_activeControl", %name @ "_text");

	if(strlen(%extraArg1))
	{
		newObject(%name @ "_container2", Simgui::Control, 10, 0, 180, 11);
	}
	else
	{
		newObject(%name @ "_container2", Simgui::Control, 10, 11, 180, 11);
	}
    newObject(%name @ "_frame", Simgui::TestButton, 0, 0, 163, 11);
	
    addToSet(%name @ "_container2", %name @ "_frame");
    //newObject(%name @ "_slider", Simgui::Slider, 3, 0, 129, 25, %var, %function); //Simgui::Slider
    newObject(%name @ "_slider", Simgui::Slider, 3, 0, 157, 25, %var, %function); //Simgui::Slider
    //newObject(%name @ "_slider", Simgui::GuiSlider, -12, -5, 165, 35, %var, %function); //Simgui::GuiSlider
    addToSet(%name @ "_container2", %name @ "_slider");
    addToSet(%name @ "_container",  %name @ "_activeControl", %name @ "_container2");
    if(strlen(%helpTag))
	{
		SimGui::setFontTags(IDFNT_FONTIS_10_4,IDFNT_FONTIS_10_2,IDFNT_FONT_DEFAULT);
		newObject(%name @ "_helpButton", SimGui::TestButton, 174, -7, 14, 31, "", "localMessageBox(*" @ %helpTag @ ");");
		control::setText(%name @ "_helpButton", "?");
		addToSet(%name @ "_container", %name @ "_helpButton");
	}
    
    if(isObject(%name @ "_container") && !isObject("NamedGuiSet\\NovaUIsettingsInner\\" @ %name @ "_container"))
    {
        control::setText(%name @ "_frame", "");
        addToSet("NamedGuiSet\\NovaUIsettingsInnerScrollerContent", %name @ "_container");
        updateScrollList("NovaUIsettingsInnerScrollerContent");
    }
}

//function Nova::setTerrainDet(%int)
//{
//    if($previous::pref::terrainDetail == trunc($pref::terrainDetail))
//    {
//        return;
//    }
//    %detail = 200-trunc($pref::terrainDetail);
//    $previous::pref::terrainDetail = %detail;
//    Nova::setTerrainDetail($previous::pref::terrainDetail);
//}

function NovaUI::initSettingsButtons()
{
    if($GUI::NoAutoCreate)
    {
        return;
    }
    
    if(isObject("NamedGuiSet\\modloaderUI"))
    {
        %prependFunc = "Nova::BtnClk();";
		//$_zzNovaUIsettingVertOffset+=10;
		// if(isObject(playGUI))
		// {
			// NovaUI::addSetting(LABEL_GAMEPLAY, *IDSTR_NOVA_UI_LABEL_GAMEPLAY, IDFNT_FONTIS_10_6, label);
		// }
		// else
		// {
			NovaUI::addSetting(LABEL_GAMEPLAY, *IDSTR_NOVA_UI_LABEL_GAMEPLAY, IDFNT_EDITOR_ALT, label);
		// }
		
        $_zzNovaUIsettingVertOffset-=8;

        NovaUI::addSliderSetting(SLIDER_cockpitFOV, *IDSTR_NOVA_FOV, IDFNT_LUCIDA_7_3, 65, 120, _($client::fov,65), '$client::fov');
        $_zzNovaUIsettingVertOffset+=10;

        NovaUI::addSetting(CHECK_FarLook,    		   *IDSTR_NOVA_UI_FAR_LOOK     		   ,IDFNT_LUCIDA_7_3, check, '$pref::farlook',      		  %prependFunc @ 'schedule("Nova::toggleFarLook();",0.1);', IDSTR_NOVA_HELP_FAR_LOOK);
        NovaUI::addSetting(CHECK_ShowDroneVehicles,    *IDSTR_NOVA_UI_SHOW_FACTORY_VEH     ,IDFNT_LUCIDA_7_3, check, '$pref::showDroneVehicles',      %prependFunc @ 'modloader::buildFactoryList();', IDSTR_NOVA_HELP_FACTORY_VEHICLES);
        NovaUI::addSetting(CHECK_CockpitFadeIn,        *IDSTR_NOVA_UI_SPAWN_FADE_IN  	   ,IDFNT_LUCIDA_7_3, check, '$pref::spawnFadeIn',        	  %prependFunc);
        NovaUI::addSetting(CHECK_CockpitShake,         *IDSTR_NOVA_UI_COCKPIT_SHAKE        ,IDFNT_LUCIDA_7_3, check, '$pref::cockpitShake',           %prependFunc @ 'schedule("Nova::toggleCockpitShake();",0.1);');
        NovaUI::addSetting(CHECK_CollisionMesh,        *IDSTR_NOVA_UI_COLLISION_MESH       ,IDFNT_LUCIDA_7_3, check, '$pref::collisionMesh',          %prependFunc @ 'schedule("Nova::toggleCollisionMesh();",0.1);', IDSTR_NOVA_HELP_COLLISION_MESH);
        NovaUI::addSetting(CHECK_SmoothTankAlign,      *IDSTR_NOVA_UI_TANK_ALIGNS          ,IDFNT_LUCIDA_7_3, check, '$pref::smoothTankSurfaceAlign', %prependFunc, IDSTR_NOVA_HELP_SMOOTH_TANK);
        NovaUI::addSetting(CHECK_AutoCenterMapView,    *IDSTR_NOVA_UI_AUTO_CENTER_MAPVIEW  ,IDFNT_LUCIDA_7_3, check, '$pref::autoCenterMapView',      %prependFunc, IDSTR_NOVA_HELP_MAP_CENTER);
        
		if(isObject(playGUI))
		{
			NovaUI::addSetting(BUTTON_FreeCam,   			 *IDSTR_NOVA_UI_FREE_CAM   ,IDFNT_LUCIDA_10_3, button, '', 					%prependFunc @ 'Nova::freeCam();');
			$_zzNovaUIsettingVertOffset+=5;
		}
		
		
		NovaUI::addSetting(LABEL_COLLMESH, *IDSTR_NOVA_UI_LABEL_COLLMESH, IDFNT_EDITOR_ALT, label);
        $_zzNovaUIsettingVertOffset+=1;
		
		NovaUI::addSetting(LABEL_COLLMESH_UNDAMAGED, *IDSTR_NOVA_UI_LABEL_COLLMESH_UNDAMAGED, IDFNT_LUCIDA_7_3, label, nofooter);
		$_zzNovaUIsettingVertOffset+=2;
		
        $_zzNovaUIsettingVertOffset-=5;
		NovaUI::addSetting(LABEL_COLLMESH_URED, " R", IDFNT_LUCIDA_7_3, label, nofooter);
		$_zzNovaUIsettingVertOffset-=11;
		NovaUI::addSliderSetting(SLIDER_collMeshUndamagedRed, "", IDFNT_LUCIDA_7_3, 0, 255, _($pref::collMeshColor::undamaged::red,135), '$pref::collMeshColor::undamaged::red', 	  %prependFunc, "", notext);
		
		$_zzNovaUIsettingVertOffset-=7;
		
		NovaUI::addSetting(LABEL_COLLMESH_UGREEN, " G", IDFNT_LUCIDA_7_3, label, nofooter);
		$_zzNovaUIsettingVertOffset-=11;
		NovaUI::addSliderSetting(SLIDER_collMeshUndamagedGreen, "", IDFNT_LUCIDA_7_3, 0, 255, _($pref::collMeshColor::undamaged::green,135), '$pref::collMeshColor::undamaged::green', 	  %prependFunc, "", notext);
		
		$_zzNovaUIsettingVertOffset-=7;
		
		NovaUI::addSetting(LABEL_COLLMESH_UBLUE, " B", IDFNT_LUCIDA_7_3, label, nofooter);
		$_zzNovaUIsettingVertOffset-=11;
		NovaUI::addSliderSetting(SLIDER_collMeshUndamagedBlue, "", IDFNT_LUCIDA_7_3, 0, 255, _($pref::collMeshColor::undamaged::blue,135), '$pref::collMeshColor::undamaged::blue', 	  %prependFunc, "", notext);
		
        $_zzNovaUIsettingVertOffset-=5;//
		
		NovaUI::addSetting(LABEL_COLLMESH_DAMAGED, *IDSTR_NOVA_UI_LABEL_COLLMESH_DAMAGED, IDFNT_LUCIDA_7_3, label, nofooter);
		$_zzNovaUIsettingVertOffset+=2;
		
        $_zzNovaUIsettingVertOffset-=5;
		NovaUI::addSetting(LABEL_COLLMESH_DRED, " R", IDFNT_LUCIDA_7_3, label, nofooter);
		$_zzNovaUIsettingVertOffset-=11;
		NovaUI::addSliderSetting(SLIDER_collMeshDamagedRed, "", IDFNT_LUCIDA_7_3, 0, 255, _($pref::collMeshColor::damaged::red,255), '$pref::collMeshColor::damaged::red', 	  %prependFunc, "", notext);
		
		$_zzNovaUIsettingVertOffset-=7;
		
		NovaUI::addSetting(LABEL_COLLMESH_DGREEN, " G", IDFNT_LUCIDA_7_3, label, nofooter);
		$_zzNovaUIsettingVertOffset-=11;
		NovaUI::addSliderSetting(SLIDER_collMeshDamagedGreen, "", IDFNT_LUCIDA_7_3, 0, 255, _($pref::collMeshColor::damaged::green,255), '$pref::collMeshColor::damaged::green', 	  %prependFunc, "", notext);
		
		$_zzNovaUIsettingVertOffset-=7;
		
		NovaUI::addSetting(LABEL_COLLMESH_DBLUE, " B", IDFNT_LUCIDA_7_3, label, nofooter);
		$_zzNovaUIsettingVertOffset-=11;
		NovaUI::addSliderSetting(SLIDER_collMeshDamagedBlue, "", IDFNT_LUCIDA_7_3, 0, 255, _($pref::collMeshColor::damaged::blue,0), '$pref::collMeshColor::damaged::blue', 	  %prependFunc, "", notext);
		
		$_zzNovaUIsettingVertOffset-=5;//
		
		NovaUI::addSetting(LABEL_COLLMESH_CRITICAL, *IDSTR_NOVA_UI_LABEL_COLLMESH_CRITICAL, IDFNT_LUCIDA_7_3, label, nofooter);
		$_zzNovaUIsettingVertOffset+=2;
		
        $_zzNovaUIsettingVertOffset-=5;
		NovaUI::addSetting(LABEL_COLLMESH_CRED, " R", IDFNT_LUCIDA_7_3, label, nofooter);
		$_zzNovaUIsettingVertOffset-=11;
		NovaUI::addSliderSetting(SLIDER_collMeshCriticalRed, "", IDFNT_LUCIDA_7_3, 0, 255, _($pref::collMeshColor::critical::red,255), '$pref::collMeshColor::critical::red', 	  %prependFunc, "", notext);
		
		$_zzNovaUIsettingVertOffset-=7;
		
		NovaUI::addSetting(LABEL_COLLMESH_CGREEN, " G", IDFNT_LUCIDA_7_3, label, nofooter);
		$_zzNovaUIsettingVertOffset-=11;
		NovaUI::addSliderSetting(SLIDER_collMeshCriticalGreen, "", IDFNT_LUCIDA_7_3, 0, 255, _($pref::collMeshColor::critical::green,0), '$pref::collMeshColor::critical::green', 	  %prependFunc, "", notext);
		
		$_zzNovaUIsettingVertOffset-=7;
		
		NovaUI::addSetting(LABEL_COLLMESH_CBLUE, " B", IDFNT_LUCIDA_7_3, label, nofooter);
		$_zzNovaUIsettingVertOffset-=11;
		NovaUI::addSliderSetting(SLIDER_collMeshCriticalBlue, "", IDFNT_LUCIDA_7_3, 0, 255, _($pref::collMeshColor::critical::blue,0), '$pref::collMeshColor::critical::blue', 	  %prependFunc, "", notext);
		
		//VISUALS
		NovaUI::addSetting(LABEL_VISUALS, *IDSTR_NOVA_UI_LABEL_VISUALS, IDFNT_EDITOR_ALT, label);
		
        $_zzNovaUIsettingVertOffset-=8;
		
		NovaUI::addSliderSetting(SLIDER_terrainDetail, *IDSTR_NOVA_UI_TERRAIN_DETAIL, IDFNT_LUCIDA_7_3, 20, 200, _($pref::terrainDetail,120), '$pref::terrainDetail', 	  %prependFunc, IDSTR_NOVA_HELP_TERRAIN_DETAIL);
        $_zzNovaUIsettingVertOffset+=10;
		
		NovaUI::addSetting(CHECK_CustomTerrainVis,     *IDSTR_NOVA_UI_CUSTOM_TERRAIN_VIS     ,IDFNT_LUCIDA_7_3, check, '$pref::customTerrainVisbility', %prependFunc @ 'schedule("Nova::setTerrainVisibilities();",0.1);', IDSTR_NOVA_HELP_CUSTOM_TERRAIN_VIS);
		$_zzNovaUIsettingVertOffset-=15;
		
		NovaUI::addSliderSetting(SLIDER_TerrainVisDist,*IDSTR_NOVA_UI_CUSTOM_TERRAIN_VIS_DIST,IDFNT_LUCIDA_7_3, 0, 15000, _($pref::terrainVisDist,2500), '$pref::terrainVisDist', 		%prependFunc @ 'Nova::setTerrainVisibilities();');
		$_zzNovaUIsettingVertOffset+=12;
		
		if($pref::GWC::SIM_FS_MODE == "Upscaled" && !$zzPendingReload && $Opengl::Active)
		{
			NovaUI::addSetting(BUTTON_WindowScale,     *IDSTR_NOVA_UI_WINDOW_SIZE,   IDFNT_FONTIS_10_3, button, '', 						  %prependFunc @ "Nova::updateWindowScale();", IDSTR_NOVA_HELP_WINDOW_SIZE);
			$_zzNovaUIsettingVertOffset+=10;
        }
		
		if(isFile("mods/ScriptGL/RenderContext"))
		{
			NovaUI::addSetting(CHECK_SHELL_SCRIPTGL,   *IDSTR_NOVA_UI_SHELL_SCRIPTGL     	 ,IDFNT_LUCIDA_7_3, check, '$pref::shellScriptGL',      	%prependFunc @ 'schedule("Nova::initScriptGLContext();",0.1);', IDSTR_NOVA_HELP_SHELL_SCRIPTGL);
		}
        NovaUI::addSetting(CHECK_FarWeather,           *IDSTR_NOVA_UI_FAR_WEATHER            ,IDFNT_LUCIDA_7_3, check, '$pref::farWeather',             %prependFunc @ 'schedule("Mem::initFarWeather();",0.1);', IDSTR_NOVA_HELP_FAR_WEATHER);
        NovaUI::addSetting(CHECK_Lensflare,		       *IDSTR_NOVA_UI_LENSFLARE    	      	 ,IDFNT_LUCIDA_7_3, check, '$pref::lensflare',       	    %prependFunc);
        NovaUI::addSetting(CHECK_HiresShadows,         *IDSTR_NOVA_UI_HIRES_SHADOWS          ,IDFNT_LUCIDA_7_3, check, '$pref::hiresShadows',           %prependFunc, IDSTR_NOVA_HELP_HIRES_SHADOWS);
        NovaUI::addSetting(CHECK_NeverDrawShadows,     *IDSTR_NOVA_UI_NO_SHADOWS             ,IDFNT_LUCIDA_7_3, check, '$pref::neverDrawShadows',       %prependFunc @ 'schedule("Nova::determineShadows();",0.1);');
        NovaUI::addSetting(CHECK_GL_NEAREST,           *IDSTR_NOVA_UI_GL_NEAREST             ,IDFNT_LUCIDA_7_3, check, '$pref::OpenGL::GL_NEAREST',     %prependFunc @ 'flushTextureCache();', IDSTR_NOVA_HELP_GLNEAREST);
        NovaUI::addSetting(CHECK_Mipmapping,           *IDSTR_NOVA_UI_NO_MIPMAPPING          ,IDFNT_LUCIDA_7_3, check, '$pref::OpenGL::disableMipMaps', %prependFunc @ 'flushTextureCache();');
        NovaUI::addSetting(CHECK_GridLines,            *IDSTR_NOVA_UI_GRID_LINES             ,IDFNT_LUCIDA_7_3, check, '$pref::disableMapGridLines', 	%prependFunc, IDSTR_NOVA_HELP_GRID_LINES);
        NovaUI::addSetting(CHECK_SmoothTerrain,        *IDSTR_NOVA_UI_SMOOTH_TERRAIN         ,IDFNT_LUCIDA_7_3, check, '$pref::mapviewSmoothTerrain',   %prependFunc @ 'schedule("Nova::toggleMapviewSmoothTerrain();",0.1);', IDSTR_NOVA_HELP_SMOOTH_TERRAIN);
        
		//NETWORKING
			NovaUI::addSetting(LABEL_NETWORKING, *IDSTR_NOVA_UI_LABEL_NETWORKING, IDFNT_EDITOR_ALT, label);
		
		$_zzNovaUIsettingVertOffset+=5;
        NovaUI::addSetting(CHECK_EnforceModloader,       *IDSTR_NOVA_UI_ENFORCE             	   ,IDFNT_LUCIDA_7_3, check, '$pref::enforceModloader',        %prependFunc, IDSTR_NOVA_HELP_ENFORCE_NOVA);
        NovaUI::addSetting(CHECK_DisableServerMOTD,      *IDSTR_NOVA_UI_DISABLE_SERVER_MOTD 	   ,IDFNT_LUCIDA_7_3, check, '$pref::disableMasterServerMOTD', %prependFunc);
		$_zzNovaUIsettingVertOffset-=10;
		//NovaUI::addSliderSetting(SLIDER_PacketFrame, 	 *IDSTR_NOVA_UI_PACKET_FRAME			   ,IDFNT_LUCIDA_7_3, 32, 128, '$pref::packetFrame', 			%prependFunc); //This doesn't seem to do anything
		NovaUI::addSliderSetting(SLIDER_PacketRate, 	 *IDSTR_NOVA_UI_PACKET_RATE				   ,IDFNT_LUCIDA_7_3, 4, 2000, _($pref::packetRate,10), '$pref::packetRate', 			%prependFunc, IDSTR_NOVA_HELP_PACKETRATE);
		NovaUI::addSliderSetting(SLIDER_PacketSize, 	 *IDSTR_NOVA_UI_PACKET_SIZE				   ,IDFNT_LUCIDA_7_3, 100, 1000, _($pref::packetSize,2000), '$pref::packetSize', 			%prependFunc);
		$_zzNovaUIsettingVertOffset+=10;

		//MISC
		NovaUI::addSetting(LABEL_MISCELLANEOUS, *IDSTR_NOVA_UI_LABEL_MISC, IDFNT_EDITOR_ALT, label);
		
        $_zzNovaUIsettingVertOffset-=8;
		
        NovaUI::addSliderSetting(SLIDER_cursorSensitivity, *IDSTR_NOVA_UI_CURSOR_SENSITIVITY, IDFNT_LUCIDA_7_3, 1, 4, _($pref::cursorSensitivity,1),'$pref::cursorSensitivity', "Nova::setCursorSensitivity($pref::cursorSensitivity);");
        $_zzNovaUIsettingVertOffset+=10;
		NovaUI::addSetting(CHECK_IntroSmackers,          *IDSTR_NOVA_UI_INTRO_SMACKERS ,IDFNT_LUCIDA_7_3, check, '$pref::playGameLaunchSmackers', 		%prependFunc @ 'schedule("Nova::determineGameLaunchSmackers();",0.1);');
		NovaUI::addSetting(CHECK_EchoChat,             	 *IDSTR_NOVA_UI_ECHO_CHAT      ,IDFNT_LUCIDA_7_3, check, '$pref::echoPlayerMessages', 			%prependFunc);
		NovaUI::addSetting(CHECK_NotifyText,             *IDSTR_NOVA_UI_NOTIFY_TEXT    ,IDFNT_LUCIDA_7_3, check, '$pref::novaNotifyText', 				%prependFunc);
        NovaUI::addSetting(CHECK_ShowFPS,                *IDSTR_NOVA_UI_SHOW_FPS       ,IDFNT_LUCIDA_7_3, check, '$pref::showFPS', 						%prependFunc);
        NovaUI::addSetting(CHECK_ShowPing,               *IDSTR_NOVA_UI_SHOW_PING      ,IDFNT_LUCIDA_7_3, check, '$pref::showPing', 					%prependFunc);
        NovaUI::addSetting(CHECK_ShowMem,                *IDSTR_NOVA_UI_SHOW_MEM       ,IDFNT_LUCIDA_7_3, check, '$pref::showMem', 						%prependFunc);
        NovaUI::addSetting(CHECK_CanvasCursorTrapped,    *IDSTR_NOVA_UI_LOCK_CURSOR    ,IDFNT_LUCIDA_7_3, check, '$pref::canvasCursorTrapped', 			%prependFunc);
        NovaUI::addSetting(CHECK_EchoMatchedTextures,    *IDSTR_NOVA_UI_VERBOSE_HASHED ,IDFNT_LUCIDA_7_3, check, '$pref::echoMatchedTextures', 			%prependFunc, IDSTR_NOVA_HELP_VERBOSE_HASHED);
        NovaUI::addSetting(CHECK_ShowMatchedTextures,    *IDSTR_NOVA_UI_RENDER_HASHED  ,IDFNT_LUCIDA_7_3, check, '$pref::showMatchedTextures', 			%prependFunc @ 'schedule("flushtexturecache(simcanvas);",0.1);', IDSTR_NOVA_HELP_RENDER_HASHED);
		//$_zzNovaUIsettingVertOffset-=15;
		NovaUI::addSetting(BUTTON_BayesEdit,  			 *IDSTR_NOVA_UI_BAYES_EDITOR   ,IDFNT_FONTIS_8_4, button, '', 									%prependFunc @ "Nova::initBayesEditor();");
		$_zzNovaUIsettingVertOffset+=5;
		
		if(!isObject(playGUI))
		{
			NovaUI::addSetting(LABEL_TEXTURE_HASHER, *IDSTR_NOVA_LABEL_TEXTURE_HASHER, IDFNT_EDITOR_ALT, label);
			$_zzNovaUIsettingVertOffset-=8;
			
			NovaUI::addSetting(TEXTENTRY_TextureName,    	 *IDSTR_NOVA_UI_TEXTURE_NAME   ,IDFNT_LUCIDA_7_3, textEntry, '$Nova::textureName', 			%prependFunc);
			control::setText(TEXTENTRY_TextureName_textEntry,$Nova::textureName);
			$_zzNovaUIsettingVertOffset+=15;
			
			NovaUI::addSetting(CHECK_AllocateHashDirectory,  *IDSTR_NOVA_UI_ALLOCATE_HASH  ,IDFNT_LUCIDA_7_3, check, '$pref::allocateTextureHasherDirectory',%prependFunc @ 'schedule("Nova::allocateHashDirectory();",0.1);', IDSTR_NOVA_HELP_ALLOCATE_HASH);
			NovaUI::addSetting(BUTTON_GenerateTextureHash,   *IDSTR_NOVA_UI_HASH_SCRIPT	   ,IDFNT_FONTIS_10_3, button, '', 								%prependFunc @ "Nova::generateTextureHash($Nova::textureName);");
			$_zzNovaUIsettingVertOffset+=2;
			NovaUI::addSetting(LABEL_HashOutput, 			 '', IDFNT_CONSOLE, label);
			$_zzNovaUIsettingVertOffset+=10;
			NovaUI::addSetting(BUTTON_TextureHashDirectory,  *IDSTR_NOVA_UI_TEXTURE_DIRECTORY,IDFNT_FONTIS_8_4, button, '', 							%prependFunc @ "Nova::openHashDirectory();");
			$_zzNovaUIsettingVertOffset+=10;
			NovaUI::addSetting(CHECK_IncludeVehicleSkins,  	 *IDSTR_NOVA_UI_INCLUDE_SKINS  ,IDFNT_LUCIDA_7_3, check, '$pref::hasherIncludeSkins',		%prependFunc, IDSTR_NOVA_HELP_HASHER_INCLUDE_SKINS);
			NovaUI::addSetting(BUTTON_BatchGenerateHashes,   *IDSTR_NOVA_UI_BATCH_GENERATE ,IDFNT_FONTIS_8_4, button, '', "Nova::initBatchTextureHashing();");
		}
        //NovaUI::addSetting(testCheck1, "Test check 1",IDFNT_LUCIDA_7_3, check);
        //NovaUI::addSetting(testCheck2, "Test check 2",IDFNT_LUCIDA_7_3, check);
        //NovaUI::addSetting(LABEL_rendering, "TESTSTRING (tag string me!)",IDFNT_FONTIS_8_3, label);
    }
}

function FileList::onAction()
{
    if(!isObject(37))
    {
		%file = Control::getValue("FileList");
		modloader::appendMod(%file);modloader::Filelist();
	}
   //control::setVisible(FileList_scroller,0);
   //control::setVisible(modlist_scroller,1);
}

// ███████ ████████ ██████  ██ ███    ██  ██████      ███████ ██    ██ ███    ██  ██████ ████████ ██  ██████  ███    ██ ███████ 
// ██         ██    ██   ██ ██ ████   ██ ██           ██      ██    ██ ████   ██ ██         ██    ██ ██    ██ ████   ██ ██      
// ███████    ██    ██████  ██ ██ ██  ██ ██   ███     █████   ██    ██ ██ ██  ██ ██         ██    ██ ██    ██ ██ ██  ██ ███████ 
//      ██    ██    ██   ██ ██ ██  ██ ██ ██    ██     ██      ██    ██ ██  ██ ██ ██         ██    ██ ██    ██ ██  ██ ██      ██ 
// ███████    ██    ██   ██ ██ ██   ████  ██████      ██       ██████  ██   ████  ██████    ██    ██  ██████  ██   ████ ███████                                        

function String::Char(%string, %index)
{
    if(%index < 1)
    {
        %index++;
    }
    %char = strAlign(1,l,strAlignR(strlen(%string)-%index+1, %string));
    if(strlen(%char) != 0)
    {
        return %char;
    }
    return "";
}

function String::Len(%string)
{
    return strlen(%string);
}

function String::getSubStr(%string, %start, %length)
{
    %str = strAlign(%length,l,strAlignR(strlen(%string)-%start, %string));
    return %str;
}

function _(%v,%n) {
	if(strlen(%v)==0)
	{
		return %n;
	}
	return %v;
}

function resetFile(%file)
{
    dynDataWriteClassType(droppoint, %file);
    fileWrite(%file, overwrite, "           ");
}

function modLoader::Logger::Reset()
{
    dynDataWriteClassType(Droppoint, "modLoaderLog.html");
    filewrite("modLoaderLog.html", overwrite, "<!DOCTYPE html>",  "<html>", "<body style='background-color:#000000;'>","<h1 style=\"color:white;\">Modloader Log</h1>","<h2 style=\"color:white;\">Version: <br>" @ $Nova::Version @ "<br>" @ $Nova::memCommit @ "<br> <a href=\"https://github.com/PlagueDog/memstar\">https://github.com/PlagueDog/memstar</a></h2>");
}
modLoader::Logger::Reset();

function modLoader::Logger::newEntry(%type, %string)
{
    if(strlen(%string) == 0)   
    {
        return;
    }
    if(%type == "dir")
    {
        %prepend = "<b style=\"background-color:rgba(126, 126, 126, 0.5);\">" @ gettime() @ "</b>  <b> [ <b style=\"background-color:rgba(0, 0, 255, 50);\">REPATH</b> ]</b> &#8594";
    }
    if(%type == "info")
    {
        %prepend = "<br><b style=\"background-color:rgba(126, 126, 126, 0.5);\">" @ gettime() @ "</b>  <b> [ <b style=\"background-color:rgba(126, 126, 126, 0.5);\">INFO</b> ]</b> &#8594";
    }
    if(%type == "normal")
    {
        %prepend = "<b style=\"background-color:rgba(126, 126, 126, 0.5);\">" @ gettime() @ "</b>  <b> [ <b style=\"background-color:rgba(43, 150, 0, 0.7);\">OK</b> ]</b> &#8594";
    }
    if(%type == "warn")
    {
        %prepend = "<b style=\"background-color:rgba(126, 126, 126, 0.5);\">" @ gettime() @ "</b>  <b> [ <b style=\"background-color:rgba(204, 112, 0, 0.8);\">WARNING</b> ]</b> &#8594";
    }
    if(%type == "error")
    {
        %prepend = "<b style=\"background-color:rgba(126, 126, 126, 0.5);\">" @ gettime() @ "</b>  <b>[ <b style=\"background-color:rgba(255, 0, 0, 0.5);\">ERROR</b> ]</b> &#8594";
        $modloaderIDerr++;
    }
    filewrite("modLoaderLog.html", append, "<b style=\"color:darkgray;\">" @ %prepend @ "   " @ %string @ "</b><br>");
}

// ██████  ███████ ██████   █████  ████████ ██   ██ ██ ███    ██  ██████      ██      ██ ██████  
// ██   ██ ██      ██   ██ ██   ██    ██    ██   ██ ██ ████   ██ ██           ██      ██ ██   ██ 
// ██████  █████   ██████  ███████    ██    ███████ ██ ██ ██  ██ ██   ███     ██      ██ ██████  
// ██   ██ ██      ██      ██   ██    ██    ██   ██ ██ ██  ██ ██ ██    ██     ██      ██ ██   ██ 
// ██   ██ ███████ ██      ██   ██    ██    ██   ██ ██ ██   ████  ██████      ███████ ██ ██████  

function repath::append(%dir, %cd)
{
    if(strlen(%dir)==0)
    {
        echo("repath::append( directoryName );");
        return false;
    }
	
	StringM::explode($consoleworld::defaultsearchpath, ";", "repath::dir");
	%i=0;
	while(strlen($repath::dir[%i++])!=0)
	{
		if($repath::dir[%i]==%dir)
		{
            echo("REPATH: [" @ %dir @ "] is already assigned");
            return false;
		}
	}
    
    $basepath = $basepath @ %dir @ ";";
	$basepath = String::Replace($basepath, ";;", ";");
    $consoleWorld::defaultSearchPath = $basePath;
    //appendSearchPath();
	if(%dir=="."){%root=" (ROOT)";}
	if(%cd!=true){echo("REPATH: Added [" @ %dir @ "]"@%root);
    modLoader::Logger::newEntry(dir,"ADDED [<b style=\"color:cyan;\"> " @ %dir @ "</b><b> ] " @ %root @ "</b>" );
    }
	else{echo("REPATH: Added [" @ %dir @ "]"@" - CDROM");}
	deleteVariables("repath::*");
}

function repath::remove(%dir)
{
    if(strlen(%dir)==0)
    {
        echo("repath::remove( directoryName );");
        return false;
    }
	StringM::explode($consoleworld::defaultsearchpath, ";", "repath::dir");
    %i=0;
	while(strlen($repath::dir[%i++])!=0)
	{
		if($repath::dir[%i]==%dir)
		{
            $basepath = String::Replace($basepath, %dir @ ";", "");
            $consoleWorld::defaultSearchPath = $basePath;
            //appendSearchPath();
			echo("REPATH: Removed  [" @ %dir @ "]" );
            modLoader::Logger::newEntry(dir,"REMOVED [<b style=\"color:cyan;\"> " @ %dir @ " </b>]" );
            %foundDirectoryName=true;
		}
	}
	deleteVariables("repath::*");
}
$consoleworld::defaultsearchpath = $Basepath;
function Nova::setupDirectories()
{
    $basePath = "";
    $consoleWorld::defaultSearchPath = "";
    repath::append(".");
    repath::append("Scripts");
    repath::append("mods"); 
    repath::append("keymaps");
    repath::append("faces");
    repath::append("logos");
    repath::append("movies");
    repath::append("sounds");
    repath::append("multiplayer");
    // repath::append("training"); - Do not load this directory. The shell gui will do it automatically when the player goes into training.
    //repath::append("campaign\\Human");
    //repath::append("campaign\\Cybrid (Advanced)");
    repath::append("data\\movies");
    repath::append("data\\sounds");
    repath::append("terrain");
    repath::append("skins");
    
    if ($CDROM_drive != "")
    {
        repath::append($CDROM_drive @ "data\\movies",true);
        repath::append($CDROM_drive @ "data\\faces",true);
        repath::append($CDROM_drive @ "data\\logos",true);
        repath::append($CDROM_drive @ "data\\sounds",true);
    }
	
	schedule("Nova::allocateHashDirectory();",0.1);
}

Nova::setupDirectories();
$consoleWorld::defaultSearchPath = $basePath;

//exec("scriptgl_1003compat.cs");

function C::SV(%control,%bool)//Control::SetVisible
{
    control::setVisible(%control,%bool);
}

IDSTR_STRING_DEFAULT            = 00139998, "";
   
if ($cargv1 != "-s")
{
	console::mute(true);
    IDFNT_HUD_HIGH_RES_ALT         	= 00150500, "hud_high.pft";
    IDFNT_HUD_HIGH_RES_DIM_ALT     	= 00150501, "hud_high_dim.pft";
    IDFNT_HUD_HIGH_RES_GRAY_ALT    	= 00150502, "hud_high_gray.pft";
    IDFNT_HUD_HIGH_RES_YELLOW_ALT   = 00150503, "hud_high_yellow.pft";
    IDFNT_HUD_HIGH_RES_BLUE_ALT    	= 00150504, "hud_high_blue.pft";
    IDFNT_HUD_HIGH_RES_PURPLE_ALT   = 00150505, "hud_high_purple.pft";
    IDFNT_HUD_HIGH_RES_RED_ALT      = 00150506, "hud_high_red.pft";
    
    Memstar::addReplacement("c16ada49", "CS_POPUP.TGA");
    Memstar::addReplacement("42df2146", "CS_POPUPWIDE.TGA");
    Memstar::addReplacement("4651bab3", "HS_POPUP.TGA");
    Memstar::addReplacement("c233587a", "HS_POPUPWIDE.TGA");
    Memstar::addReplacement("ecd49394", "SP_BAR.TGA");
    Memstar::addReplacement("073984d7", "SP_POPUP.TGA");
    Memstar::addReplacement("8075d31e", "SP_POPUPWIDE1.TGA");
    Memstar::addReplacement("103b02e3", "SP_POPUPWIDE2.TGA");
    Memstar::addReplacement("761d8405", "COMPAT_ADV_TRAIN.TGA");
    Memstar::addReplacement("81305203", "COMPAT_HERC_TRAIN.TGA");
    Memstar::addReplacement("adf71ff8", "COMPAT_SQUAD_TRAIN.TGA");
    Memstar::addReplacement("18816dde", "COMPAT_TARGET_TRAIN.TGA");
    Memstar::addReplacement("778877ef", "COMPAT_WEAPONS_TRAIN.TGA");
    
    //Icons font
    Memstar::addReplacement("c6c05638", "IF_MR_36B_FRAME003.TGA");
    
    //Nova Logo
    IDBMP_ICON_NOVA = 00160132, "Nova_logo.bmp";
    Memstar::addReplacement("7a086101", "NOVA_LOGO.TGA");
	console::mute(false);
}

function Nova::determineGameLaunchSmackers()
{
	if(!$pref::playGameLaunchSmackers)
	{
		IDSMK_INTRO		= 00372003, "";
		IDSMK_CREDITS   = 00372020, "";
	}
	else
	{
		IDSMK_INTRO		= 00372003, "intro.smk";
		IDSMK_CREDITS   = 00372020, "credits.smk";
	}
}

function splashGUI::onOpen::NovaFunction()
{
	if(!$pref::playGameLaunchSmackers && $previousGUI != "mainmenuGUI")
	{
		IDSMK_INTRO		= 00372003, "";
		IDSMK_CREDITS   = 00372020, "";
	}
	else
	{
		IDSMK_INTRO		= 00372003, "intro.smk";
		IDSMK_CREDITS   = 00372020, "credits.smk";
	}
}




//████████ ███████ ██   ██ ████████ ██    ██ ██████  ███████     ██   ██  █████  ███████ ██   ██ ███████ ██████  
//   ██    ██       ██ ██     ██    ██    ██ ██   ██ ██          ██   ██ ██   ██ ██      ██   ██ ██      ██   ██ 
//   ██    █████     ███      ██    ██    ██ ██████  █████       ███████ ███████ ███████ ███████ █████   ██████  
//   ██    ██       ██ ██     ██    ██    ██ ██   ██ ██          ██   ██ ██   ██      ██ ██   ██ ██      ██   ██ 
//   ██    ███████ ██   ██    ██     ██████  ██   ██ ███████     ██   ██ ██   ██ ███████ ██   ██ ███████ ██   ██ 

function Nova::allocateHashDirectory()
{
	if($pref::allocateTextureHasherDirectory)
	{
		if(String::findSubStr($consoleWorld::defaultSearchPath, "mods\\textureHasher") >= 0)
		{
			$basepath = String::Replace($basepath, "mods\\textureHasher;", "");
			$consoleWorld::defaultSearchPath = $basePath;
			appendSearchPath();
		}
		//Push the hash directory to the front of the basepath to make it the highest priority
		    $basepath = "mods\\textureHasher;" @ $basepath;
			$basepath = String::Replace($basepath, ";;", ";");
			$consoleWorld::defaultSearchPath = $basePath;
			appendSearchPath();
	}
	else
	{
		if(String::findSubStr($consoleWorld::defaultSearchPath, "mods\\textureHasher") >= 0)
		{
			$basepath = String::Replace($basepath, "mods\\textureHasher;", "");
			$consoleWorld::defaultSearchPath = $basePath;
			appendSearchPath();
		}
	}
}

function Nova::generateTextureHash(%texture)
{
	if(String::Right(%texture,4) == ".bmp")
	{
		$Nova::textureName = String::Replace(stripFilePath(%texture),".bmp",".tga");
		$Nova::capturingTextureCRC = "true";
		IDBMP_BITMAP_DEFAULT = 00169998, %texture;
		newGuiObject(textureHasher, Simgui::guiBitmapCtrl, 0,0, 50,50);
		schedule("deleteObject(\"NamedGuiSet\\\\textureHasher\");", 0.05);
		//setCursor(simcanvas, %texture);
		//schedule("setCursor(simcanvas, 'cursor.bmp');",0.05);
		//schedule("control::setText(TEXTENTRY_GetTextureHashOutput_textEntry, $Nova::textureHashFunction);flushTextureCache();",0.05);
		
		Schedule("Nova::copyToClipboard($Nova::textureHashFunction);",0.1);
		
		control::setVisible(LABEL_HashOutput_label,1);
		control::setText(LABEL_HashOutput_label, *IDSTR_NOVA_UI_COPIED_CLIPBOARD);
		schedule("control::setVisible(LABEL_HashOutput_label,0);",2);
	}
	deleteVariables("Nova::textureCR*");
}


function Nova::initBatchTextureHashing()
{
	if(!$pref::shellScriptGL)
	{
		localMessageBox(*IDSTR_NOVA_UI_HASHER_PREF_ERROR);
		return;
	}
	
	Memstar::disableCursorAutoOn();
	$_zzTextureHasherdate = String::Replace(getDate(), "/", "");
	$_zzTextureHashertime = String::Replace(getTime(), ":", "");
	resetFile("mods/replacements/textureHashes_" @ $_zzTextureHasherdate @ $_zzTextureHashertime @ ".cs");
	flushTextureCache();
	//windowsKeyboardEnable(simcanvas);
	//winMouse();
	Nova::getTextureDirectories();
	Schedule("clientCursorOff();Nova::batchGenerateTextureHasher();",0.25);
	
	newObject(scriptGL_textureHasher, Simgui::TScontrol, 0, 0, 640, 480);
	addToSet(getGroup("NamedGuiSet\\NovaUImodular"), scriptGL_textureHasher);
	postAction("NamedGuiSet\\scriptGL_textureHasher", "Attach", 655);
	$Nova::textureHashing = true;
	//Control::setVisible(disableOverlay,true);
	//Control::setVisible(calculatingHashesText,true);
	//Control::setVisible(textureHashText,true);
	//Control::setVisible(textureHasherAbort,true);
	//Control::setVisible(textureHasherFileCount,true);
}

function Nova::getTextureDirectories()
{
	deleteVariables("_zzNovaTextureHasher*");
	deleteVariables("directoryFile*");
	
	//Get the skins directory
	if($pref::hasherIncludeSkins)
	{
		getDirectory("./logos");
		while(strlen($directoryFile[%l++]))
		{
			if(String::Findsubstr($directoryFile[%l], ".bmp") > 0 && $directoryFile[%l] != "." && $directoryFile[2+%l] != "..")
			{
				%dd++;
				$_zzNovaTextureHasher[texture,%dd] = "logos\\" @ $directoryFile[%l];
			}
		}
		deleteVariables("directoryFile*");
		
		getDirectory("./faces");
		while(strlen($directoryFile[%f++]))
		{
			if(String::Findsubstr($directoryFile[%f], ".bmp") > 0 && $directoryFile[%f] != "." && $directoryFile[2+%f] != "..")
			{
				%dd++;
				$_zzNovaTextureHasher[texture,%dd] = "faces\\" @ $directoryFile[%f];
			}
		}
		deleteVariables("directoryFile*");
		
		getDirectory("./skins");
		while(strlen($directoryFile[%s++]))
		{
			if(String::Findsubstr($directoryFile[%s], ".bmp") > 0 && $directoryFile[%s] != "." && $directoryFile[2+%s] != "..")
			{
				%dd++;
				$_zzNovaTextureHasher[texture,%dd] = "skins\\" @ $directoryFile[%s];
			}
		}
		deleteVariables("directoryFile*");
	}
	
	else
	{
		//Get the textureHasher directory
		getDirectory("./mods/textureHasher");
		while(strlen($directoryFile[%d++]))
		{
			if(String::Findsubstr($directoryFile[%d], ".bmp") > 0 && $directoryFile[%d] != "." && $directoryFile[2+%d] != "..")
			{
				%dd++;
				$_zzNovaTextureHasher[texture,%dd] = $directoryFile[%d];
			}
		}
	}
	
	//Get the file count
	%fc=1;
	while(strlen($_zzNovaTextureHasher[texture,%fc]))
	{
		%fc++;
		$_zzNovaTextureHasherFileCount++;
	}
}

$hashSpeed = 0.025;
function Nova::batchGenerateTextureHasher()
{
	$d++;
	deleteVariables("Nova::textureHashFunc*");
	$Nova::textureName = String::Replace(stripFilePath($_zzNovaTextureHasher[texture,$d]),".bmp",".tga");
	$Nova::capturingTextureCRC = "true";
	
	if($pref::hasherIncludeSkins)
	{
		IDBMP_BITMAP_DEFAULT = 00169998, $_zzNovaTextureHasher[texture,$d];
	}
	else
	{
		IDBMP_BITMAP_DEFAULT = 00169998, "mods\\textureHasher\\" @ $_zzNovaTextureHasher[texture,$d];
	}
	newGuiObject(textureHasher, Simgui::guiBitmapCtrl, 0,0, 4,4);
	schedule("deleteObject(\"NamedGuiSet\\\\textureHasher\");", 0.015);
	//setCursor(simcanvas, %texture);
	//schedule("setCursor(simcanvas, 'cursor.bmp');",0.05);
	//%fileWrite = fileWrite("mods/textureHasher/textureHasher.cs", overwrite, $Nova::textureHashFunction);
	
	//Control::setText(textureHasherFileCount, "File " @ $d @ " of " @ $_zzNovaTextureHasherFileCount);
	//schedule("Control::setText(textureHashText,$Nova::textureCRC);", 0.015);
	if(($Nova::textureCRC != $Nova::tempTextureCRC) && strlen($Nova::textureCRC))
	{
		$Nova::tempTextureCRC = $Nova::textureCRC;
		//Push the existing CRCs down
		%n=1;
		while(strlen($textureHasher[%n]))
		{
			$textureHasher[%n, offset] += 12;
			%n++;
		}
		$ddd++;
		$textureHasher[$ddd] = $Nova::textureCRC;
		$textureHasher[$ddd, fileName] = $_zzNovaTextureHasher[texture,$d];
		$textureHasher[$ddd, offset] = 95;
		//echo($textureHasher[$ddd], ", ", $textureHasher[$ddd, fileName], ", ", $textureHasher[$ddd, offset]);
	}
	
	if(strlen($_zzNovaTextureHasher[texture,$d]) && isObject("NamedGuiSet\\modloaderUI"))
	{
			schedule("Nova::batchGeneratetextureHasher();",0.02);
			schedule('if(strlen($Nova::textureHashFunction)){fileWrite("mods/replacements/textureHashes_" @ $_zzTextureHasherdate @ $_zzTextureHashertime @ ".cs", append, $Nova::textureHashFunction);}',0.015);
	}
	else
	{
		$d='';
		$Nova::capturingTextureCRC = "false";
		Nova::loadTextureHashes();
		deleteObject("NamedGuiSet\\scriptGL_textureHasher");
		//windowsKeyboardDisable(simcanvas);
		//guiMouse();
		$Nova::textureHashing = false;
		$Nova::textureCRC = "";
		flushTextureCache();
		clientCursorOn();
		deleteVariables("textureHashe*");
		deleteVariables("Nova::tempTextureCR*");
		deleteVariables("ddd");
		deleteVariables("_zzNovaTextureHasher*");
	}	
}

function Nova::loadTextureHashes(%directory)
{
	//If we dont have a simcanvas then we are a headless server
	if(!isObject(simcanvas))
	{
		return;
	}
	
	deleteVariables("directoryFile*");
	if(!strlen(%directory))
	{
		getDirectory("./mods/replacements");
		echo("Nova: LOADING TEXTURE HASHES...");
	}
	else
	{
		getDirectory("./mods/" @ %directory);
		echo("Nova: LOADING TEXTURE HASHES FROM [.\\mods\\" @ String::toLower(%directory) @ "]");
	}
	%console::printLevel = $console::printLevel;
	
	while(strlen($directoryFile[%d++]))
	{
		if(String::Findsubstr($directoryFile[%d], "textureHashes_") >= 0 && String::Findsubstr($directoryFile[%d], ".cs") > 0 )
		{
			$console::printLevel = 0;
			if(!strlen(%directory))
			{
				exec("mods\\replacements\\" @ $directoryFile[%d]);
			}
			else
			{
				exec("mods\\" @ %directory @ "\\" @ $directoryFile[%d]);
			}
			$console::printLevel = %console::printLevel;
			echo("  - Loading hashes [" @ $directoryFile[%d] @ "]");
		}
	}
	$console::printLevel = %console::printLevel;
}

function Nova::initBayesEditor()
{
	if($Nova::bayesLoadedAlready)
	{
		localMessageBox(*IDSTR_NOVA_UI_BAYES_ERROR);
		return;
	}
	Nova::loadBayesEditor();
}

function Nova::toggleFarLook()
{
	if($pref::farLook)
	{
		Nova::enableFarLook();
	}
	else
	{
		Nova::disableFarLook();
	}
}
Nova::toggleFarLook();