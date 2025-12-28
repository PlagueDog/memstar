function Nova::flushSounds()
{
    sfxclose();
    sfxopen();
}

// $offset = 0.5;
// function offsetUP()
// {
    // opengl::offsetgui($off+=0.001);
    // echo($off);
// }

// function offsetDOWN()
// {
    // opengl::offsetgui($off-=0.001);
    // echo($off);
// }

// $scale=2;
// function scaleUP()
// {
    // opengl::scalegui($scale+=0.01);
    // echo($scale);
// }

// function scaleDOWN()
// {
    // opengl::scalegui($scale-=0.01);
    // echo($scale);
// }

// bind(keyboard0, make, up, TO, "offsetUP();");
// bind(keyboard0, make, down, TO, "offsetDOWN();");
// bind(keyboard0, make, left, TO, "scaleDOWN();");
// bind(keyboard0, make, right, TO, "scaleUP();");
// $shift=-1;
// function shiftRIGHT()
// {
    // opengl::shiftGUI($shift+=0.01);
    // echo($shift);
// }
// function shiftLEFT()
// {
    // opengl::shiftGUI($shift-=0.01);
    // echo($shift);
// }
// bind(keyboard0, make, shift, left, TO, "shiftLEFT();");
// bind(keyboard0, make, shift, right, TO, "shiftRIGHT();");
function Nova::endframe(){}

function UPCTicker()
{
    //flushtexturecache(simcanvas);
    Control::setText(TextActiveRes, *IDSTR_NOVA_UPSCALER_RESOLUTION @ ": " @ _($pref::GWC::SIM_FS_WIDTH,'???') @ "x" @ _($pref::GWC::SIM_FS_HEIGHT,'???'));
    //Opengl::ScaleGUI(_($Opengl::scaler::RenderScale, 2));
    //Opengl::shiftGUI(_($OpenGL::scaler::RenderShift, -1));
	
    //control::setText(RenderScaleText, _($Opengl::scaler::RenderScale+0.00001,2));
    control::setText(RenderScaleText, _($Opengl::scaler::RenderScale,2));
    control::setText(RenderVertOffsetText, _($Opengl::scaler::RenderShift,-1));
    
    if($zzInUpscaleConfig)
    {
        schedule("UPCTicker();", 0.025);
    }
}

//function GUIoffsetSlider::onAction(){control::setText(GUIoffsetInput, $Opengl::scaler::GUIoffset);}
function RenderScaleSlider::onAction()
{
    control::setText(RenderScaleInput, flt($Opengl::scaler::RenderScale));
    Opengl::ScaleGUI(_(flt($Opengl::scaler::RenderScale), 2));
    Nova::FlushCanvas();
}

function RenderScaleInput::onAction()
{
    Opengl::ScaleGUI(_(flt($Opengl::scaler::RenderScale), 2));
    Nova::FlushCanvas();
}

function RenderVertOffsetInput::onAction()
{
    Opengl::shiftGUI(_(flt($OpenGL::scaler::RenderShift), -1));
    Nova::FlushCanvas();
}

function RenderVertOffsetSlider::onAction()
{
    control::setText(RenderVertOffsetInput, flt($Opengl::scaler::Rendershift));
    Opengl::shiftGUI(_(flt($OpenGL::scaler::RenderShift), -1));
    Nova::FlushCanvas();
}

function UpscalerProfileConfigButton::onAction()
{
    if(isObject(playGUI) || isObject(mapviewGUI) || isObject(editorGUI))
    {
        schedule("guipopdialog(simcanvas,0);guipopdialog(simcanvas,0);OpenGL::scaleGUI(2);OpenGL::shiftGUI(-1);",0.05);
        localmessagebox(*IDSTR_NOVA_UPSCALER_ERR);
    }
}

function UpscaleProfileConfigSave()
{
    //if(!isObject("NamedGuiSet\\OffsetPlaceholder"))
    //{
    //    break;
    //}
    ////Determine the aspect ratio and adjust the SPmain render offset accordingly
    //$Nova::aspectRatio = getwindowsize(width)/getwindowsize(height);
    //if(%AR == 1.77777 || %AR == 1.77864)
    //{
    //   $Nova::aspectRatio = "-0.75";
    //}
    //else if(%AR == 1.79999)
    //{
    //   $Nova::aspectRatio = "-0.8";
    //}
    //else if(%AR == 1.33333 || %AR == 1.3211)
    //{
    //   $Nova::aspectRatio = "-0.89";
    //}
    //else if(%AR == 1.59999)
    //{
    //   $Nova::aspectRatio = "-0.85";
    //}
    //else if(%AR == 2.38888)
    //{
    //   $Nova::aspectRatio = "-0.55";
    //}
    //else
    //{
    //   $Nova::aspectRatio = "-1"; 
    //}
    $Opengl::UpscaleProfile[getWindowSize(width),getWindowSize(height)] = "$Opengl::scaler::RenderScale=" @ _(flt($Opengl::scaler::RenderScale),2) @ ";" @ "OpenGL::ScaleGUI('" @ _(flt($Opengl::scaler::RenderScale),2) @ "');" @ "$Opengl::scaler::Rendershift='" @ flt($Opengl::scaler::RenderShift) @ "';Opengl::shiftGUI('" @ _(flt($Opengl::scaler::RenderShift),-1) @ "');";
    export("Opengl::UpscaleProfile*", "OpenglUpscalerProfiles.cs");    
}

$ScriptGL::latency = 12;
$zscale=1;

function Nova::executeShellScriptGL()
{	
	//Get the dimensions of the left banner so we can offset it correctly
	String::Explode(glGetTextureDimensions("left_banner.tga"), " ", "left_banner");
	
	function scriptGL::shellgui::onPostDraw()
	{
		if(isObject(editorGUI) || isObject(mapviewGUI) || isObject(playGUI))
		{
			return;
		}
		glColor4ub( 255, 255, 255,100);
		glDrawTexture( "left_banner.tga", $GLEX_SCALED, -($left_banner[0]),0, 1,1);
		glDrawTexture( "right_banner.tga", $GLEX_SCALED, 639,0, 1,1);
	}
	
	function scriptGL::shellgui::onPreDraw()
	{
		if(isObject(editorGUI) || isObject(mapviewGUI) || isObject(playGUI))
		{
			return;
		}
		glColor4ub(0,0,0,255);
		glRectangle(-640,0,16000,16000);
		%displayGamma = $pref::Display::gammaValue;
		
		//glEnable( $GL_TEXTURE_2D );
		//glBlendFunc( $GL_DST_COLOR, $GL_DST_ALPHA );
		//glTexEnvi( $GL_MODULATE );
		glColor4ub( 255*%displayGamma, 255*%displayGamma, 255*%displayGamma,100);
		
		glDrawTexture( "left_banner.tga", $GLEX_SCALED, -($left_banner[0]),0, 1,1);
		glDrawTexture( "right_banner.tga", $GLEX_SCALED, 640,0, 1,1);
		
		if($currentGUI == mainmenuGUI || $currentGUI == recPlayerGUI || $currentGUI == trainingGUI)
		{
			glDrawTexture( "mainmenu.tga", $GLEX_SCALED, 0, 0, 1,1);
		}
		
		else if($currentGUI == hostGUI || $currentGUI == waitroomGUI || $currentGUI == optionsGUI || $currentGUI == loadingGUI  || $currentGUI == vehLabGUI || $currentGUI == vehDepotGUI || $currentGUI == addrBookGUI || $currentGUI == debriefGUI || $currentGUI == inputConfigGUI)
		{
			glDrawTexture( "mfd.tga", $GLEX_SCALED, 0, 0, 1,1);
		}
		
		else if($currentGUI == theTeamGUI || $currentGUI == ircGUI)
		{
			glDrawTexture( "ircPanel.tga", $GLEX_SCALED, 0, 0, 1,1);
		}
		
		else if($currentGUI == SPmainGUI)
		{
			glDrawTexture( "sp_main.tga", $GLEX_SCALED, 0, 0, 1,1);
		}
		
		else if($currentGUI == omniWebGUI || $currentGUI == profileGUI || $currentGUI == saveLoadGUI || $currentGUI == recruitGUI)
		{
			glDrawTexture( "omniweb.tga", $GLEX_SCALED, 0, 0, 1,1);
		}
		
		else if($currentGUI == tutorialGUI)
		{
			glDrawTexture( "tutorial.tga", $GLEX_SCALED, 0, 0, 1,1);
		}
		
		else if($currentGUI == splashGUI || $currentGUI == introGUI || $currentGUI == theTeamGUI)
		{
			glColor4ub(0,0,0,255);
			glRectangle(-1,0,641,480);
		}
		
		else if($currentGUI == scrambleGUI && strlen($zzScriptGLScrambleTGA))
		{
			glDrawTexture( $zzScriptGLScrambleTGA, $GLEX_SCALED, 0, 0, 1,1);
		}
		
		//If it is a custom GUI then draw the mfd.tga background
		else
		{
			glDrawTexture( "mfd.tga", $GLEX_SCALED, 0, 0, 1,1);
		}
		
		//Texture Hasher UI
		if($Nova::textureHashing && isObject("NamedGuiSet\\scriptGL_textureHasher"))
		{
			glColor4ub( 255*%displayGamma, 255*%displayGamma, 255*%displayGamma, 255);
			glDrawTexture( "mfd.tga", $GLEX_SCALED, 0, 0, 1,1);
			
			//Texture Hasher Label
			glSetFont( default, 30, $GLEX_PIXEL, 0 );
			glColor4ub( 0, 200, 0, 255);
			String::Explode(glGetStringDimensions(*IDSTR_NOVA_HASHING_TEXTURES), "\x20", stringDimensions);
			glDrawString( (640/2)-($stringDimensions[0]/2), 20, *IDSTR_NOVA_HASHING_TEXTURES);	
			
			//File position
			glSetFont( default, 14, $GLEX_PIXEL, 0 );
			glColor4ub( 200*%displayGamma, 200*%displayGamma, 200*%displayGamma, 255);
			String::Explode(glGetStringDimensions("File " @ $d @ " of " @ $_zzNovaTextureHasherFileCount), "\x20", stringDimensions);
			glDrawString( (640/2)-($stringDimensions[0]/2), 58, "File " @ $d @ " of " @ $_zzNovaTextureHasherFileCount);
			
			//Texture CRC
			glSetFont( default, 20, $GLEX_PIXEL, 0 );
			glColor4ub( 200*%displayGamma, 200*%displayGamma, 0, 255);
			String::Explode(glGetStringDimensions($Nova::textureCRC), "\x20", stringDimensions);
			glDrawString( (640/2)-($stringDimensions[0]/2), 75, $Nova::textureCRC);	
			
			//Abort label
			glSetFont( default, 20, $GLEX_PIXEL, 0 );
			glColor4ub( 255*%displayGamma, 0, 0, 255);
			String::Explode(glGetStringDimensions(*IDSTR_NOVA_HASHING_TEXTURES_ABORT), "\x20", stringDimensions);
			glDrawString( (640/2)-($stringDimensions[0]/2), 440, *IDSTR_NOVA_HASHING_TEXTURES_ABORT);
			if($Nova::textureCRC != %Nova::textureCRC)
			
			glSetFont( default, 10, $GLEX_PIXEL, 0 );
			glColor4ub( 0, 200*%displayGamma, 0, 255);
			%n=1;
			while(strlen($textureHasher[%n]))
			{
				%offset = $textureHasher[%n, offset];
				if(%offset <= 435 && %offset >= 95)
				{
					String::Explode(glGetStringDimensions($textureHasher[%n, fileName] @ " --> " @ $textureHasher[%n]), "\x20", stringDimensions);
					glDrawString( (640/2)-($stringDimensions[0]/2), $textureHasher[%n, offset], $textureHasher[%n, fileName] @ " --> " @ $textureHasher[%n]);
					//glDrawString( 30, $textureHasher[%n, offset], $textureHasher[%n] @ " --> " @ $textureHasher[%n, fileName]);
				}
				%n++;
			}
		}
		
		if($currentGUI != introGUI)
		{
			glSetFont( default, 11, $GLEX_PIXEL, 0 );
			//glColor4ub( 255, 225, 0, 255);
			glColor4ub( 32*%displayGamma, 32*%displayGamma, 32*%displayGamma, 255);
			glDrawString( 40, 467, $Nova::Version);	
			glDrawString( 185, 467, $Nova::memCommit);
		}
	}
}

function Nova::guiOpen(%gui)
{
    if($pref::PromptToRecord == true)
    {
        $pref::fixRecording = 1;
    }
    else
    {
        $pref::fixRecording = 0;
    }
    Nova::toggleRecordingFix();
    
    $CurrentGUI = %gui;
    
    //Mute unknown function calls
    %console::printLevel=$console::printLevel;
    $console::printLevel=0;
    guiTransition(%gui, onOpen);
    %i = 0;
    while(%i <= 50)
    {
        eval( %gui @ "::onOpen::" @ %i @ "();");
        %i++;
    }
	
	//Execute our Nova scripts when the gui opens
    eval(%gui @ "::onOpen::modLoaderFunc();");
    eval(%gui @ "::onOpen::handleOpenGL();");
    eval(%gui @ "::onOpen::NovaFunction();");
    eval(%gui @ "::onOpen::CampaignFunction();");
    Nova::executeEvent('', %gui , onOpen, '');
    
    //Unmute function calls
    $console::printLevel=%console::printLevel;
    
    if($CurrentGUI == splashGUI || $CurrentGUI == modloaderInitERRGUI || $currentGUI == novaUIGUI)
    {
        return;
    }
    
	if($pref::novaNotifyText)
	{
		//if(!$Opengl::Active)
		//{
			loadObject(NovaUI_Notify_Text, "NovaUI_Notify_Text.object");
			addToSet($CurrentGUI, NovaUI_Notify_Text);
			schedule("control::setVisible(NovaUI_Notify_Text, false);",2);
		//}
		//else
		//{
		//	$_novaNotifyTextTrigger = 1;
		//	$_novaNotifyTextTriggerFadein = 1;
		//	schedule("$_novaNotifyTextTriggerFadein = 0;", 2);
		//}
	}
        
    //if($pref::GWC::SIM_FS_MODE == "Upscaled" && !$zzPendingReload && $Opengl::Active)
		
	//Handle the training shell bitmap resizing using our compatibility images that work with ScriptGL
    if($Opengl::Active)
    {
        schedule("Nova::UpscaledGUI();", 0);
        if($CurrentGUi == TrainingGUI)
        {
            TrainingMission trainingMission1 {
            desc           = *IDSTR_TRAINING_1;
            name           = "training1";
            bmp            = "COMPAT_herc_train.BMP";
            playerVeh      = "TR_Emancipator.fvh";
            squadMate1     = "";squadMateVeh1  = "";squadMate2     = "";squadMateVeh2  = "";squadMate3     = "";squadMateVeh3  = "";
            };
            
            TrainingMission trainingMission2 {
            desc           = *IDSTR_TRAINING_2;
            name           = "training2";
            bmp            = "COMPAT_target_train.BMP";
            playerVeh      = "TR_Emancipator.fvh";
            squadMate1     = "";squadMateVeh1  = "";squadMate2     = "";squadMateVeh2  = "";squadMate3     = "";squadMateVeh3  = "";
            };
            
            TrainingMission trainingMission3 {
            desc           = *IDSTR_TRAINING_3;
            name           = "training3";
            bmp            = "COMPAT_weapons_train.BMP";
            playerVeh      = "TR_Emancipator.fvh";
            squadMate1     = "";squadMateVeh1  = "";squadMate2     = "";squadMateVeh2  = "";squadMate3     = "";squadMateVeh3  = "";
            };
            
            TrainingMission trainingMission4 {
            desc           = *IDSTR_TRAINING_4;
            name           = "training4";
            bmp            = "COMPAT_squad_train.BMP";
            playerVeh      = "TR_Basilisk.fvh";
            squadMate1     = *IDPLT_CALL_BIO_DERM;squadMateVeh1  = "TR_SquadmateEmancipator.fvh";squadMate2     = *IDPLT_CALL_VERITY;squadMateVeh2  = "TR_SquadmateEmancipator.fvh";squadMate3     = "";squadMateVeh3  = "";
            };
        }
        if($CurrentGUi == theTeamGUI)
        {
			//Unpatch the gui bitmap ctrl texture chunk scaling
            schedule("OpenGL::restoreBitmapCtrl();",0);
            Nova::purgeControlAndAttach();
            return;
        }
        if($previousGUI == theTeamGUI)
        {
            schedule("OpenGL::patchBitmapCtrl();",0);
        }
        if($previousGUI == playgui)
        {
        }
        
        //Determine the correct TGA image to use with ScriptGL on the scramble GUI
        if($currentGUI == scrambleGUI)
        {
            if(String::findSubStr($tempVar, "Human") != -1)
            {
                $zzScriptGLScrambleTGA =  "humanScramble.tga";
            }
            else if(String::findSubStr($tempVar, "Cybrid") != -1)
            {
                $zzScriptGLScrambleTGA =  "cybridScramble.tga";
            }
        }
        if($pref::ShellScriptGL && $currentGUI != playgui && $currentGUI != editorgui)
        {
			if(!isObject($currentGUI @ "\\scriptGL"))
			{
				if(!isObject(655))
				{
					newObject(editCamera, simCameraEdit);
				}
				
				// if(!isObject(loadingGUI) && !isObject(mapviewGUI))
				//if($currentGUI != loadingGUI && $currentGUI != mapviewGUI)
				if($currentGUI != mapviewGUI)
				{
					if($previousGUI != hostGUI && $previousGUI != SPmainGUI)
					{
						Nova::purgeControlAndAttach();
					}
				}
				else
				{
					//If we are exiting a loadingGUI we need to schedule the scriptGL canvas setup
					schedule("ScriptGL::loadingGUIoffload();",0.1);
					function ScriptGL::loadingGUIoffload()
					{
						if(!isObject(loadingGUI) && !isObject(mapviewGUI))
						{
							Nova::purgeControlAndAttach();
						}
					}
				}
			
				setposition(655,0,2500,1);
				Nova::executeShellScriptGL();
			}
        }
        else
        {
            deleteFunctions("scriptGL::shellgui::*");
        }
		
		if(!$pref::ShellScriptGL)
		{
			Nova::createShellCullers();
		}
		if($currentGUI != playGUI && $currentGUI != mapviewGUI && $currentGUI != editorGUI)
		{
			$GUI::noPalTrans = true;
			//Resize the internal gui bitmapCtrl texture chuncks to 640x480
			OpenGL::patchBitmapCtrl();
			
			//Flip the OpenGL render to scale from the top left corner
			OpenGL::flipScaler();
			if(!$_ScriptGLInitialized && $pref::shellScriptGL)
			{
				$_NotInNovaUI = true;
				Nova::initScriptGLContext();
			}
		}
    }
	else
	{
		if($currentGUI != playGUI && $currentGUI != mapviewGUI && $currentGUI != editorGUI)
		{
			if($pref::GWC::SIM_FS_DEVICE == Glide && $pref::GWC::SIM_FS_MODE == Upscaled && !$zzPendingReload)
			{
				enableWindowBorder();
			}
			
			$GUI::noPalTrans = false;
			gotoshellres();
		}
	}
}

function Nova::purgeControlAndAttach()
{
	if(!strLen($currentGUI))
	{
		return;
	}
	
    // guiSetSelection(simcanvas, $currentGUI @ "\\scriptGL");
    // guiSendToBack(simcanvas);
    // if(isObject(EditControl))
	// {
		// deleteObject(EditControl);
	// }
    // schedule("if(isObject(EditControl)){deleteObject(EditControl);}",0.000);
    // schedule("if(isObject(EditControl)){deleteObject(EditControl);}",0.015);
    // schedule("if(isObject(EditControl)){deleteObject(EditControl);}",0.025);
    // schedule("if(isObject(EditControl)){deleteObject(EditControl);}",0.035);
    // schedule("if(isObject(EditControl)){deleteObject(EditControl);}",0.045);
    // schedule("if(isObject(EditControl)){deleteObject(EditControl);}",0.055);
    // schedule("if(isObject(EditControl)){deleteObject(EditControl);}",0.065);
	
	while(getNextObject($currentGUI, %iter) != 0)
	{
		%iter = getNextObject($currentGUI, %iter);
		%objectIDstring = %objectIDstring @ %iter @ ",";
	}
	%objectIDstring = String::Left(%objectIDstring, strLen(%objectIDstring)-1);
	%evalString = "addToSet('" @ $currentGUI @ "\\ScriptGL'," @ %objectIDstring @ ");";
	
	if((!$Gui::InputConfigFromSim && !isCampaign()) || isMultiplayer() || $currentGUI == debriefGUI)
	{
		newobject(scriptGL, Simgui::TSControl, 0,0,640,480);
		addToSet($currentGUI, scriptGL);
		eval(%evalString);
	}
	
    postAction($currentGUI @ "\\scriptGL", "Attach", 655);
    //schedule("postAction(\"NamedGuiSet\\\\scriptGL\", \"Attach\", 655);",0.000);	
}

function Nova::guiClose(%gui)
{
    $_zzNovaUIsettingVertOffset=0;
    $PreviousGui = Nova::getLastGui();
    %console::printLevel=$console::printLevel;
    $console::printLevel=0;
	
	//Execute our Nova functions when a gui closes
    eval(%gui @ "::onClose::NovaFunction();");
    eval(%gui @ "::onClose::modLoaderFunc();");
    eval(%gui @ "::onClose::handleOpenGL();");
    eval(%gui @ "::onClose::CampaignFunction();");
    guiTransition(%gui, onClose);
    %i = 0;
    while(%i <= 50)
    {
        eval(%gui @ "::onClose::" @ %i @ "();");
        %i++;
    }
    Nova::executeEvent('', %gui, onClose, '');
        $console::printLevel=%console::printLevel;
		Nova::determineVehicleDirectory();
}

function editorGUI::onOpen::modLoaderFunc()
{
    //Set the correct interior mask for terrain relighting to StaticInteriors instead of SimInteriors
    $SimTerrain::InteriorMask = ( 1 << 6 );
	if(!$__editorOpened)
	{
		schedule("MissionObjectList::Inspect(1,8);",0);
		schedule("MissionObjectList::Inspect(1,-1);",0.1);
		$__editorOpened=1;
	}
	
	if($pref::customTerrainVisbility)
	{
		//Block terrain update packets from the server
		Nova::disableTerrainUpdates();
		
		//Set our own terrain render distance
		Nova::setTerrainVisibilities();
	}
	else
	{
		//Enable terrain update packets from the server
		Nova::enableTerrainUpdates();
	}	
}                                                

function guiTransition(%gui, %action)
{
    // schedule("flushtexturecache();Console::RenderOffset(-getWindowSize(height)+474);",0.1);
    schedule("Console::RenderOffset(-getWindowSize(height)+474);",0.1);
    while($modLoader::mod[%i++,filename])
    {
        if($modLoader::mod[%i,enabled])
        {
            %console::printLevel=$console::printLevel;
            $console::printlevel=0;
            //Trim the file extensions
            if(String::Right($modLoader::mod[%i,filename], 3) == ".cs")
            {%offset=3;}
        
            else{%offset=4;}
            
            %strlen = strlen($modLoader::mod[%i,filename]);
            %str = stralign(%offset,l,$modLoader::mod[%i,filename]);
            //if(%action == onOpen)
            //{
                eval(%gui @ "::" @ %action @ "::" @ %str @ "();");
                //eval(%gui @ "::" @ %action @ "::modLoaderFunc();");
                //eval(%gui @ "::" @ %action @ "::NovaFunction();");
            //}
            //else
            //{
            //    eval(%gui @ "::onClose::" @ %str @ "();"); 
            //    eval(%gui @ "::onClose::modLoaderFunc();");
            //    eval(%gui @ "::onClose::NovaFunction();");
            //}
            //eval(%gui @ "::onClose::handleOpenGL();");
            $console::printLevel=%console::printLevel;
        }
    }
}

function waitroomGUI::onClose::modLoaderFunc()
{
    unlockWindowSize(simcanvas);
    $Gui::LoadingFromJoin = false;
    $zzmodloader::builtFactoryList = false;
}

//These are functions/vars that need to be reset when the client leaves the server
function exitFromServerResets()
{
	deleteObject("serverModDatabase");
    $zzClientToken="";
    $simgame::timescale=1;
    $zzmodloader::builtFactoryList=0;
	$modIndex=0;
	$serverMods=0;
	$downloadTicker=0;
	$download::totalSize=0;
    if($resetSoundsOnExit)
    {
        exec("sound.cs");
        $resetSoundsOnExit = false;
    }
	
	%str = String::Replace($client::connectTo,"IP:","");
    %str = String::Replace(%str,":","_");
    %str = String::Replace(%str,".","_");
    repath::remove("mods\\cache\\" @ %str);
	appendSearchPath();
	
	if($_zzRequiresReloads)
	{
		Nova::reloadBaseClient();
		Nova::reloadScriptData();
		$_zzRequiresReloads = false;
	}
	$Net::serverCacheDirectory = "";
	
	schedule("purgeResources();",2);
	schedule("purgeResources();",2);
	schedule("purgeResources();",2);
}

function Nova::reloadBaseClient()
{
	while(strlen($modloader::mod[%m++,fileName]) != 0)
    {
		console::mute(true);
		repath::remove("mods\\" @ String::Replace($modloader::mod[%mod,fileName], ".mlv", ""));
		repath::remove("mods\\" @ String::Replace($modloader::mod[%mod,fileName], ".vol", ""));
		console::mute(false);
	}
	
	if(isObject(ModloaderDatabase))
    {
		deleteObject(ModloaderDatabase);
    }
	
	appendSearchPath();
	
	purgeResources();
	purgeResources();
	purgeResources();
	
	if(isObject(scriptsVol))
	{
		deleteObject(scriptsVol);
	}
    newObject( scriptsVol, SimVolume, "scripts.vol" );
	
	deleteObject(novaVol, shellVol, darkstarVol, editorVol, gameObjectsVol, soundVol, scriptsVol);
	
	purgeResources();
	purgeResources();
	purgeResources();
	
	newObject(shellVol, simVolume, "shell.vol");
	newObject(darkstarVol, simVolume, "darkstar.vol");
	newObject(editorVol, simVolume, "editor.vol");
	newObject(gameObjectsVol, simVolume, "gameobjects.vol");
	newObject(soundVol, simVolume, "sound.vol");
	newObject(NovaVol, simVolume, "Nova.vol");
	
    if (isFile("missions.vol"))
    {
		if(isObject(missionsVol))
		{
			deleteObject(missionsVol);
		}
        newObject( missionsVol, SimVolume, "missions.vol" );
    }
	
    if (isFile("patch.vol"))
    {
		if(isObject(patchVol))
		{
			deleteObject(patchVol);
		}
        newObject( patchVol, SimVolume, "patch.vol" );
    }
}

function joinGUI::onOpen::NovaFunction()
{
    joinGUI::MOTDcatcher();
	
	//Remove ted generated files if we have ran the mission editor
	Nova::purgeTedFiles();
	Nova::setTotalWeapons(53);
	Nova::setTotalComponents(111);
	Nova::setTotalVehicles(82);
}

function hostGUI::onOpen::NovaFunction()
{
	Nova::purgeTedFiles();
}

function loadingGUI::onOpen::NovaFunction()
{
	if($previousGUI != joinGUI)
	{
		Nova::setTotalWeapons($Nova::totalWeapons);
		Nova::setTotalComponents($Nova::totalComponents);
		Nova::setTotalVehicles($Nova::totalVehicles);
	}
	else
	{
		Nova::setTotalWeapons(53);
		Nova::setTotalComponents(111);
		Nova::setTotalVehicles(82);
	}
}

function joinGUI::MOTDcatcher()
{
	function joinGUI::MOTDlistener()
	{
		if(!$pref::disableMasterServerMOTD)
		{
			return;
		}
		
		//Send enter key input to the MOTD window
		//Clear the MOTD message variable
		if(strlen($Dlg::PriorityMesg))
		{
			//Send some key inputs to handle closing the MOTD box
			client::sendKeyInput("enter");
			client::sendKeyInput("escape");
			client::sendKeyInput("escape");
			$Dlg::PriorityMesg = '';
		}
		
		//Stop the looping function if we leave the joinGUI
		if(isObject(joinGUI))
		{
			schedule("joinGUI::MOTDlistener();",0.05);
		}
	}
	
	//Kick off the MOTD listening function
	joinGUI::MOTDlistener();
}

//This cannot be used on a headless server as it requires a GUI to function
function MOI::getObjectData(%id,%index,%server)
{
    if(!strlen(%id)||!%index)
    {
        echo("MOI::getObjectData( ObjectID, IndexNumber, [server] );");
        echo("MOI::getObjectData: Returns the object property data. Checkboxes return booleans. ID-TAG lists return a tag ID number.");
        return;
    }
        
    if(strlen(%server)){%env=1;}
    else{%env=0;}
    if(isObject(%id))
    {
        %MOI = "M::OI_" @ randomInt(1,9999999);
        //The object inspector MUST be created on the client end OF the server
        if ($CmdLineServer)
        {
            focusClient();
        }
        loadobject(%MOI,"modloader_OI.object");
        %InspectTagListObject = getnextobject(%MOI,getnextobject(%MOI,0));
        MissionObjectList::Inspect(%env,%id);
        while(getNextObject(%MOI, %InspectTagListObject) != 0 && %i <= %index)
        {
            %InspectTagListObject = getNextObject(%MOI, %InspectTagListObject);
            renameobject(%InspectTagListObject, "InspectTagListObject" @ %i++);
		}
        %value = control::getValue(InspectTagListObject @ %index);
        deleteObject(%MOI);
        if ($CmdLineServer)
        {
            focusServer();
        }
        //echo(%value);
        return %value;
    }
    return false;
}

//This cannot be used on a headless server as it requires a GUI to function
function MOI::setObjectData(%id,%index,%data,%server)
{
    if(!strlen(%id)||!%index||!strlen(%data))
    {
        echo("MOI::setObjectData( ObjectID, IndexNumber, Input, [server] );");
        return;
    }
    if(strlen(%server)){%env=1;}
    else{%env=0;}
    if(isObject(%id))
    {
        %MOI = "M::OI_" @ randomInt(1,9999999);
        loadobject(%MOI,"modloader_OI.object");
        %InspectTagListObject = getnextobject(%MOI,getnextobject(%MOI,0));
        MissionObjectList::Inspect(%env,%id);
        while(getNextObject(%MOI, %InspectTagListObject) != 0 && %i <= %index)
        {
            %InspectTagListObject = getNextObject(%MOI, %InspectTagListObject);
            renameobject(%InspectTagListObject, "InspectTagListObject" @ %i++);
		}
        control::setValue(InspectTagListObject @ %index, %data);
        MissionObjectList::Apply();
        deleteObject(%MOI);
    }
    return false;
}

$zzmodloader::builtFactoryList = false;
function modloader::buildFactoryList()
{
    //if($GUI::AllStaticsGhosted)
    //{
    //    return;
    //}
    
    //No net manager?
    %console::printlevel = $console::printlevel;
    if(!isObject(2048))
    {
            newServer();
            focusServer();
            initializeServer();
            newObject( serverDelegate, ESCSDelegate, true, "LOOPBACK", 0);
            loadMission("");
            sfxAddPair( IDSFX_REACTOR_ON,       IDPRF_2D, "" );
            sfxAddPair( IDSFX_REACTOR_OFF,      IDPRF_2D, "" );
			//sfxClose();
            Nova::purgeVehicleFiles();
            $console::printlevel=0;
            while(strlen($zzmodloader::vehicle[%i++,id]))
            {
                %file = $zzmodloader::vehicle[$zzmodloader::vehicle[%i,id],script];
                //Drones and Flyers
                if((String::findSubStr(%file,"drone") != -1 || String::findSubStr(%file,"flyer") != -1 ) && !$pref::showDroneVehicles)
                {
                    //echo("Skipping " @ %file);
                    continue;
                }
                
                //Special Platinum Exec and Adju
                if((String::findSubStr(%file,"pl_exec.") != -1 || String::findSubStr(%file,"pl_adju.") != -1 ) && !$pref::showDroneVehicles)
                {
                    //echo("Skipping " @ %file);
                    continue;
                }
                
                //Superpred and Starsiege Bus
                if((String::findSubStr(%file,"superpred") != -1 || String::findSubStr(%file,"ss_bus") != -1 ) && !$pref::showDroneVehicles)
                {
                    //echo("Skipping " @ %file);
                    continue;
                }
                
                //Cinematic Vehicles and Cannon/Harabec vehicles
                if((String::findSubStr(%file,"_cin_") != -1 || String::findSubStr(%file,"_ha_apoc") != -1 || String::findSubStr(%file,"_ca_") != -1 ) && !$pref::showDroneVehicles)
                {
                    //echo("Skipping " @ %file);
                    continue;
                }
                
                //Artillery Vehicles and Supressor
                if((String::findSubStr(%file,"_nike") != -1 || String::findSubStr(%file,"_artl") != -1 || String::findSubStr(%file,"_supr") != -1 ) && !$pref::showDroneVehicles)
                {
                    //echo("Skipping " @ %file);
                    continue;
                }
                
                //Prom and Pouncer
                if((String::findSubStr(%file,"_prom") != -1 || String::findSubStr(%file,"_bike") != -1 ) && !$pref::showDroneVehicles)
                {
                    //echo("Skipping " @ %file);
                    continue;
                }
                if($zzmodloader::vehicle[%i,type] == "Drone")
                {
                    newobject(_dummyVehicle,Tank, $zzmodloader::vehicle[%i,id]);
                }
                else
                {
                    newobject(_dummyVehicle,$zzmodloader::vehicle[%i,type], $zzmodloader::vehicle[%i,id]);
                }
                if(*$zzmodloader::vehicle[%i,name] == "<INVALID TAG>")
                {
                    storeObject(_dummyVehicle, "mods\\session\\[" @ $zzmodloader::vehicle[%i,id] @ "].fvh");
                }
                else
                {
                    storeObject(_dummyVehicle, "mods\\session\\" @ *$zzmodloader::vehicle[%i,name] @ " [ID-" @ $zzmodloader::vehicle[%i,id] @ "].fvh");
                }
                deleteObject(_dummyVehicle);
            }
            focusclient();
            deleteServer();
            deleteVariables("directoryFile*");
            focusclient();$console::printlevel=1;
    }
    else
    {
            %str = String::Replace($client::connectTo,"IP:","");
            %str = String::Replace(%str,":","_");
            %str = String::Replace(%str,".","_");
            getDirectory("./mods/cache/" @ %str @ "/vehicles");
            sfxAddPair( IDSFX_REACTOR_ON,       IDPRF_2D, "" );
            sfxAddPair( IDSFX_REACTOR_OFF,      IDPRF_2D, "" );
			// sfxClose();
            while(strlen($zzmodloader::vehicle[%i++,id]))
            {
                %file = $zzmodloader::vehicle[$zzmodloader::vehicle[%i,id],script];
                //Drones and Flyers
                if((String::findSubStr(%file,"drone") != -1 || String::findSubStr(%file,"flyer") != -1 ) && !$pref::showDroneVehicles)
                {
                    //echo("Skipping " @ %file);
                    continue;
                }
                
                //Special Platinum Exec and Adju
                if((String::findSubStr(%file,"pl_exec.") != -1 || String::findSubStr(%file,"pl_adju.") != -1 ) && !$pref::showDroneVehicles)
                {
                    //echo("Skipping " @ %file);
                    continue;
                }
                
                //Superpred and Starsiege Bus
                if((String::findSubStr(%file,"superpred") != -1 || String::findSubStr(%file,"ss_bus") != -1 ) && !$pref::showDroneVehicles)
                {
                    //echo("Skipping " @ %file);
                    continue;
                }
                
                //Cinematic Vehicles and Cannon/Harabec vehicles
                if((String::findSubStr(%file,"_cin_") != -1 || String::findSubStr(%file,"_ha_apoc") != -1 || String::findSubStr(%file,"_ca_") != -1 ) && !$pref::showDroneVehicles)
                {
                    //echo("Skipping " @ %file);
                    continue;
                }
                
                //Artillery Vehicles and Supressor
                if((String::findSubStr(%file,"_nike") != -1 || String::findSubStr(%file,"_artl") != -1 || String::findSubStr(%file,"_supr") != -1 ) && !$pref::showDroneVehicles)
                {
                    //echo("Skipping " @ %file);
                    continue;
                }
                
                //Prom and Pouncer
                if((String::findSubStr(%file,"_prom") != -1 || String::findSubStr(%file,"_bike") != -1 ) && !$pref::showDroneVehicles)
                {
                    //echo("Skipping " @ %file);
                    continue;
                }
                if($zzmodloader::vehicle[%i,type] == "Drone")
                {
                    newobject(_dummyVehicle,Tank, $zzmodloader::vehicle[%i,id]);
                }
                else
                {
                    newobject(_dummyVehicle,$zzmodloader::vehicle[%i,type], $zzmodloader::vehicle[%i,id]);
                }
                if(*$zzmodloader::vehicle[%i,name] == "<INVALID TAG>")
                {
                    storeObject(_dummyVehicle, "mods\\session\\[" @ $zzmodloader::vehicle[%i,id] @ "].fvh");
                }
                else
                {
                    storeObject(_dummyVehicle, "mods\\session\\" @ *$zzmodloader::vehicle[%i,name] @ " [ID-" @ $zzmodloader::vehicle[%i,id] @ "].fvh");
                }
                deleteObject(_dummyVehicle);
            }
    }
    sfxAddPair( IDSFX_REACTOR_ON,       IDPRF_2D, "powerOn.wav" );
    sfxAddPair( IDSFX_REACTOR_OFF,      IDPRF_2D, "powerOff.wav" );
	// sfxOpen();
    $console::printlevel = %console::printlevel;
    focusclient();
    deleteVariables("directoryFile*");
    appendSearchPath();
}

function nameGUIObject(%id,%index,%name)
{
        %i=0;
        while(isMember(%id,(getNextObject(%id, %object))) != 0 && %i++ <= %index)
        {
            %object=getNextObject(%id, %object);
        }
        renameObject(%object,%name);            
}

function nameVehicleControllerObjects()
{
	renameObject(Nova::findGuiTagControl($currentGUI, IDVCZ_VEH_BTN_LOAD), 		VehiclePanelLoadButton);
	renameObject(Nova::findGuiTagControl($currentGUI, IDVCZ_VEH_BTN_SAVE), 		VehiclePanelSaveButton);
	renameObject(Nova::findGuiTagControl($currentGUI, IDVCZ_TD_VEH_NAME), 		VehicleViewFrameVehicleName);
	renameObject(Nova::findGuiTagControl($currentGUI, IDVCZ_SAVE_BTN_SAVE), 	VehicleSaveButton);
	renameObject(Nova::findGuiTagControl($currentGUI, IDVCZ_LOAD_BTN_LOAD), 	VehicleLoadPanelOKButton);
	renameObject(Nova::findGuiTagControl($currentGUI, IDVCZ_LOAD_BTN_CANCEL), 	VehicleLoadPanelCANCELButton);
	renameObject(Nova::findGuiTagControl($currentGUI, IDSTR_FACTORY_VEH), 		VehicleLoadPanelTab_Standard);
	renameObject(Nova::findGuiTagControl($currentGUI, IDSTR_CUST_VEH), 			VehicleLoadPanelTab_Custom);
	renameObject(Nova::findGuiTagControl($currentGUI, IDVCZ_LOAD_BTN_DELETE), 	VehicleLoadPanelDeleteButton);
}

function VehicleLoadPanelTab_Standard::onAction()
{
        $modloader::vehicleDirectory = "mods\\session";
        setVehicleDir('mods\\session');
}

function VehicleLoadPanelTab_Custom::onAction()
{
    focusClient();
    if(isObject(vehLabGUI) || isObject(vehDepotGUI))
    {
        $modloader::vehicleDirectory = "vehicles";
        setVehicleDir(vehicles);
    }
    else if(isObject(waitroomGUI))
    {
        if($modloader::vehicleDirectory != "vehicles")
        {
			if(strlen($Net::serverCacheDirectory))
			{
				$modloader::vehicleDirectory = $Net::serverCacheDirectory;
				setVehicleDir($Net::serverCacheDirectory);
				control::setText(VehicleLoadPanelTab_Custom, *IDSTR_TAB_SERVERINFO);
			}
			else
			{
				$modloader::vehicleDirectory = "vehicles";
				setVehicleDir(vehicles);
				control::setText(VehicleLoadPanelTab_Custom, *IDSTR_CUST_VEH);
			}
        }
        else if($modloader::vehicleDirectory == "vehicles")
        {
			//
        }
    }
}

//If we cancel the load vehicle menu we need to fetch the previous vehicle directory variable to prevent a empty vehicle list
function VehicleLoadPanelCANCELButton::onAction()
{
        setVehicleDir("'" @ $modloader::vehicleDirectory @ "'");
}

function VehiclePanelSaveButton::onAction()
{
	if(strlen($Net::serverCacheDirectory))
	{
		$modloader::vehicleDirectory = $Net::serverCacheDirectory;
		setVehicleDir($Net::serverCacheDirectory);
	}
	else
	{
		$modloader::vehicleDirectory = "vehicles";
		setVehicleDir(vehicles);
	}
}

function vehlabGUI::onOpen::modLoaderFunc()
{
   %ext = String::right($client::vehicle,4);
   if(%ext == ".fvh")
   {
       $modloader::vehicleDirectory = "mods\\session";
       setVehicleDir("mods\\session");
   }
   else
   {
       $modloader::vehicleDirectory = "vehicles";
       setVehicleDir(vehicles);
   }
   nameVehicleControllerObjects();
}

function Nova::onVehicleFileLookup()
{
    %fileName = Nova::getLoadedVehicleFileName();
    //Factory Vehicles
    if(isFile("mods\\session\\" @ %fileName) || String::Right(%fileName, 4) == ".fvh")
    {
       $modloader::vehicleDirectory = "mods\\session";
       setVehicleDir("mods\\session");
       //echo(DEBUG_FVH);
    }
    
    //Custom Vehicles
    if(isFile("vehicles\\" @ %fileName) || String::Right(%fileName, 4) == ".veh")
    {
		if(strlen($Net::serverCacheDirectory))
		{
			$modloader::vehicleDirectory = $Net::serverCacheDirectory;
			setVehicleDir($Net::serverCacheDirectory);
			control::setText(VehicleLoadPanelTab_Custom, *IDSTR_TAB_SERVERINFO);
		}
		else
		{
			$modloader::vehicleDirectory = "vehicles";
			setVehicleDir("vehicles");
			control::setText(VehicleLoadPanelTab_Custom, *IDSTR_CUST_VEH);
		}
        //echo(DEBUG_VEH);
    }
}
        
function vehdepotGUI::onOpen::modLoaderFunc()
{
   nameVehicleControllerObjects();
}

function vehlabGUI::onClose::modLoaderFunc()
{
    focusclient();
    $zzmodloader::builtFactoryList = false;
}

function vehdepotGUI::onClose::modLoaderFunc()
{
}

function SPmainGUI::onOpen::modloaderFunc()
{
    //Maintain compatibility with localization mods
    if(isObject(SPMAINGUI))
    {
		export("client::*", "playerPrefs.cs");
        //Fix for the erratic problem of having no player vehicle selected when the player returns to the campaign screen after the CinHA/CinCA cinematic
        if(isCampaign())
        {
            String::Explode($tempVar, "\\", zzMissionScript);
            
            if($pref::showDroneVehicles)
            {
                $pref::showDroneVehicles = false;
                modloader::buildFactoryList();
            }
            
            //Human
            if($zzMissionScript[2] == "cinHA.cs")
            {
                setVehicleDir("mods\\session");
                GameSetVehicle(0, "");
                GameSetVehicle(0, "Outrider Emancipator [ID-52].fvh");
                schedule("guiload(\"spmain.gui\");",0.1);
                if($pref::GWC::SIM_FS_MODE == "Upscaled" && !$zzPendingReload && $Opengl::Active)
                {
                schedule("Nova::FlushCanvas();",0.15);
                }
            }
            
            //Cybrid
            else if($zzMissionScript[2] == "cinCA.cs")
            {
                setVehicleDir("mods\\session");
                GameSetVehicle(0, "");
                GameSetVehicle(0, "Seeker [ID-20].fvh");
                schedule("guiload(\"spmain.gui\");",0.1);
                if($pref::GWC::SIM_FS_MODE == "Upscaled" && !$zzPendingReload && $Opengl::Active)
                {
                schedule("Nova::FlushCanvas();",0.15);
                }
            }
            deleteVariables("zzMissionScript*");
            deleteVariables("tempVar");
        }
    }
}

function ProfileGUI::onSelectFace()
{
    if(!isObject("ProfileGUI\\PrimaryButtons"))
    {
		if($pref::shellScriptGL && $OpenGL::Active)
		{
            // %offset = getNextObject("ProfileGUI\\ScriptGL",0);
            renameObject(getNextObject("ProfileGUI\\ScriptGL",0),"PrimaryButtons");
		}
		else
		{
            renameObject(getNextObject(ProfileGUI,0),"PrimaryButtons");
		}
    }
    
    //Set the default Simgui::GuiBitmapCtrl bmp
    IDBMP_BITMAP_DEFAULT = 00169998, $engine::campaignFace;
    $engine::GuiBitmapCtrl::DefaultImage = 169998;
    
    //Hide the default face box
    if(control::getVisible(IDPPF_CTRL_BTN_FACE))
    {
        control::setVisible(IDPPF_CTRL_BTN_FACE,0);
    }
    
    //Does our face override box exist?
    if(isObject("NamedGuiSet\\CampaignFaceOverrideBox"))
    {
        //Refresh the Simgui::GuiBitmapCtrl
        if(isObject("NamedGuiSet\\CampaignFaceOverrideBox\\CampaignFaceOverride"))
        {
            deleteObject("NamedGuiSet\\CampaignFaceOverride");
            newObject(CampaignFaceOverride, Simgui::GuiBitmapCtrl, 2, 1, 100, 100);
            addToSet("NamedGuiSet\\CampaignFaceOverrideBox", CampaignFaceOverride);
        }
    }
    else
    {
        //Create our face override box and create the Simgui::GuiBitmapCtrl
        //newObject(CampaignFaceOverrideBox, Simgui::TestButton, 55, 85, 102, 102, "", "Control::setVisible(144111,1);Control::setVisible(144112,0);");
        newObject(CampaignFaceOverrideBox, Simgui::TestButton, 55, 85, 102, 102);
        //if($pref::GWC::SIM_FS_MODE == "Upscaled" && !$zzPendingReload)
        //{
		if($pref::shellScriptGL && $OpenGL::Active)
		{
            // %offset = getNextObject(ProfileGUI,0);
            addToSet(getNextObject("ProfileGUI\\ScriptGL",0),CampaignFaceOverrideBox);
		}
		else
		{
            addToSet(getNextObject(ProfileGUI,0),CampaignFaceOverrideBox);
		}
        //}
        //else
        //{
        //    addToSet(getNextObject(ProfileGUI,0),CampaignFaceOverrideBox);
        //}
        newObject(CampaignFaceOverride, Simgui::GuiBitmapCtrl, 2, 1, 100, 100);
        addToSet("NamedGuiSet\\CampaignFaceOverrideBox", CampaignFaceOverride);
        if(!isObject("NamedGuiSet\\PrimaryButtons\\FaceSelectFrame"))
        {
            loadObject("FaceSelectFrame", "campaignFaceSelectFrame.object");
            addToSet("NamedGuiSet\\PrimaryButtons", "FaceSelectFrame");
        }
    }
}

//theTeamGUI modloader compat
function theTeamGUI::randomPic()
{
    $teamPic = randomInt(0,7);
    if($teamPic >= 1 && $teamPic <= 7)
    {
        IDBMP_THE_TEAM = 00169997, "teampic" @ $teamPic @ ".bmp";
    }
    else
    {
        IDBMP_THE_TEAM = 00169997, "theteam.bmp";
    }
}
theTeamGUI::randomPic();

function theTeamGUIcyclePicture(%direction)
{
    if(%direction == "next")
    {
        $teamPic++;
        if($teamPic > 7)
        {
            $teamPic = 0;
        }
    }
    if(%direction == "prev")
    {
        $teamPic--;
        if($teamPic < 0)
        {
            $teamPic = 7;
        }
    }
    
    if($teamPic >= 1 && $teamPic <= 7)
    {
        IDBMP_THE_TEAM = 00169997, "teampic" @ $teamPic @ ".bmp";
    }
    else
    {
        IDBMP_THE_TEAM = 00169997, "theteam.bmp";
    }
    schedule("guiLoad('theTeam.gui');",0);
}

function newGUIObject(%name, %class, %x, %y, %width, %height, %conVar, %conCmd)
{
    if(!strlen(%name) && !strlen(%class))
    {
        echo("newGUIObject(ObjectName, ClassName, [x], [y], [width], [height], [conVar], [conCmd]);");
        break;
    }
    else
    {
        newobject(%name, %class, %x, %y, _(%width,50), _(%height,50), _(%conVar,%null), _(%conCmd,%null));
        addToSet($currentGUI, %name);
    }
}

function Nova::pushConsole()
{
	$windowHeight = getWindowSize(height);
	Console::RenderOffset(-$windowHeight);
	%i=0;
	if($mute::pushConsole::process)
	{
		return;
	}

	$mute::pushConsole::process = true;
	function Nova::pushConsole::process(%i)
	{
		//World
		%i+=4;
		if(isObject(playGUI) || isObject(mapviewGUI) || isObject(editorGUI))
		{
			if(%i <= ($windowHeight-10))
			{
				Console::RenderOffset(-$windowHeight+%i);
				schedule("Nova::pushConsole::process(" @ %i @ ");", 0.001);
			}
			else
			{
				$mute::pushConsole::process = false;
				return;
			}
		}
		//Shell
		else
		{
			if(%i <= (480-10))
			{
				Console::RenderOffset(-$windowHeight+%i);
				schedule("Nova::pushConsole::process(" @ %i @ ");", 0.001);
			}
			else
			{
				$mute::pushConsole::process = false;
				return;
			}
		}
	}
    Nova::pushConsole::process();
}

//Does not work. Components remain on a vehicle even when if it has been destroyed thus causing this function to always return false
//function vehicleIsDestroyed(%id)
//{
//	for(%i=0;%i<=16;%i++)
//	{
//		%compScore+=getComponentID(%id,%i);
//	}
//	if(%compScore>0)
//	{
//		return false;
//	}
//	return true;
//}

function isVehicle(%id)
{
	//Temporarily mute the console
	%varHold = $console::printLevel;
	$console::printLevel = 0;
	if(getVehicleAvailableMass(%id) != 0)
	{
		$console::printLevel = %varHold;
		return true;
	}
	$console::printLevel = %varHold;
	return false;
}

function isWorldObject(%id)
{
	if(!isVehicle(%id))
	{
		//Non world objects will always return exactly 0,0 on the world axis
		if(floor(getPosition(%id,X)) == 0 && floor(getPosition(%id,Y)) == 0 )
		{
			return false;
		}
	}
	return true;
}

//Override the games getDistance; with our own using math functions from mem.dll since they are more optimized.
function getDistance(%object1, %object2)
{
	if((!strlen(%object1) && !strlen(%object2)) || (strlen(%object1) && !strlen(%object2)) || (!strlen(%object1) && strlen(%object2)))
	{
		echo("getDistance(object1, object2);");
		return;
	}
	%x1 = getPosition(%object1,x); 
	%y1 = getPosition(%object1,y); 
	%x2 = getPosition(%object2,x); 
	%y2 = getPosition(%object2,y); 
	%dist = sqrt(pow((%x2-%x1),2) + pow((%y2-%y1),2));
	return %dist;
}

if(String::Right($client::vehicle,4) == ".veh")
{
    $modloader::vehicleDirectory = "vehicles";
    $Nova::CurrentVehicleDir = "vehicles";
}
else
{
    $modloader::vehicleDirectory = "mods\\session";
    $Nova::CurrentVehicleDir = "mods\\session";
}

setVehicleDir($modloader::vehicleDirectory);

function Mem::initShadows()
{
    if($pref::hiresShadows)
    {
        Nova::EnableHiresShadows();
    }
    else
    {
        Nova::DisableHiresShadows();
    }
}

function Mem::initFarWeather()
{
    if($pref::FarWeather)
    {
        Nova::EnableFarWeather();
    }
    else
    {
        Nova::DisableFarWeather();
    }
}

//Used to get a players vehicle skin
//Useful to detect players using shape-corrupting/crashing skins
function Nova::getVehicleSkin(%id)
{
	Nova::disableInspectWindow();//Disable the inspect dialog window cause it hitches the game
	inspectObject(%id);
	%temp = $inspector::vehicleSkin;
	deleteVariables("inspector::*");
	if(!$CmdLineServer)
	{
		Nova::enableInspectWindow();//Enable the inspect window after
	}
	return %temp;
}

//Returns a vehicles mass
//Useful to detect players using hacked vehicles
function Nova::getVehicleMass(%id, %type)
{
	if(!strlen(%id) && !strlen(%type))
	{
		echo("Nova::getVehicleMass(id, [current | max]");
		return;
	}
	Nova::disableInspectWindow();
	inspectObject(%id);
	%temp = $inspector::vehicleMass;
	deleteVariables("inspector::*");
	if(%type == current)
	{
		String::Explode(%temp, "/", __m);
		%return = String::Replace($__m[0], " ", "");
		%return = String::Replace(%return, "<--TOOMUCHMASS!", "");
		deleteVariables("__m*");
	}
	else if(%type == max)
	{
		String::Explode(%temp, "/", __m);
		%return = String::Replace($__m[1], " ", "");
		%return = String::Replace(%return, "<--TOOMUCHMASS!", "");
		deleteVariables("__m*");
	}
	else
	{
		%return = String::Replace(%temp, " ", "");
	}
	if(!$CmdLineServer)
	{
		Nova::enableInspectWindow();
	}
	return %return;
}

function Nova::getGuiObjectPosition(%id)
{
	if(!isObject(%id))
	{
		echo("Nova::getGuiObjectPosition: Object ", %id, " not found.");
		return 0;
	}
	Nova::disableInspectWindow();
	inspectObject(%id);
	Nova::enableInspectWindow();
	return $inspector::gObjectX @ "," @ $inspector::gObjectY;
}

function Nova::getGuiObjectExtent(%id)
{
	if(!isObject(%id))
	{
		echo("Nova::getGuiObjectPosition: Object ", %id, " not found.");
		return 0;
	}
	Nova::disableInspectWindow();
	inspectObject(%id);
	Nova::enableInspectWindow();
	return $inspector::gObjectXext @ "," @ $inspector::gObjectYext;
}

function Nova::findGuiTagControl(%groupID, %tagID)
{
	if(!strlen(%groupID)){echo("Nova::findGuiTagControl(groupID, tagID);");return;}
	if(!strlen(%tagID)){echo("Nova::findGuiTagControl(groupID, tagID);");return;}
	if(!isObject(%groupID)){echo("Nova::findGuiTagControl: group not found.");return;}
	if(*%tagID == "<INVALID TAG>"){echo("Nova::findGuiTagControl: invalid tag ID.");return;}
	$Nova::foundTagControl = "";
	
	function Nova::findGuiTagControl_INTERNAL(%groupID, %tagID)
	{
	while(getNextObject(%groupID, %iter))
	{
		%iter = getNextObject(%groupID, %iter);
		if(getNextObject(%iter, 0))
		{
			Nova::findGuiTagControl_INTERNAL(%iter, %tagID);
		}
		if(Nova::getGuiObjectControlID(%iter) == %tagID)
		{
			$Nova::foundTagControl = %iter; //this variable will be 'returned' by Nova::findGuiTagControl
			return true;
		}
	}
	}
	
	Nova::findGuiTagControl_INTERNAL(%groupID, %tagID);
	
	//Purge the callback function
	deleteFunctions("Nova::findGuiTagControl_INTERNAL");
	return $Nova::foundTagControl;
}

function Nova::getGuiObjectControlID(%id)
{
	if(!isObject(%id))
	{
		echo("Nova::getGuiObjectPosition: Object ", %id, " not found.");
		return 0;
	}
	Nova::disableInspectWindow();
	inspectObject(%id);
	Nova::enableInspectWindow();
	return $inspector::gObjectControlID;
}

function Nova::getObjectRotation(%id, %axis)
{
	if(!isObject(%id))
	{
		echo("Nova::getObjectRotation: Object ", %id, " not found.");
		return 0;
	}
	else if(!strlen(%axis))
	{
		echo("Nova::getObjectRotation( id, axis);");
		return 0;
	}
	Nova::disableInspectWindow();
	inspectObject(%id);
	Nova::enableInspectWindow();
	if(%axis == x)
	{
		return $inspector::ObjectRotX;
	}
	else if(%axis == y)
	{
		return $inspector::ObjectRotY;
	}
	else if(%axis == z)
	{
		return $inspector::ObjectRotZ;
	}
	else
	{
		echo("Nova::getObjectRotation: Invalid axis. Valid axes are x, y, and z.");
		return;
	}
}

function Nova::determineVehicleDirectory()
{
	if(String::Right($client::vehicle,4) == ".veh")
	{
		if(strlen($Net::serverCacheDirectory))
		{
			$modloader::vehicleDirectory = $Net::serverCacheDirectory;
			$Nova::CurrentVehicleDir = $Net::serverCacheDirectory;
		}
		else
		{
			$modloader::vehicleDirectory = "vehicles";
			$Nova::CurrentVehicleDir = "vehicles";
		}
	}
	else
	{
		$modloader::vehicleDirectory = "mods\\session";
		$Nova::CurrentVehicleDir = "mods\\session";
	}
	setVehicleDir($modloader::vehicleDirectory);
}

//Override the games native getTeam; function with our own which actually works on the client side
function getTeam(%id)
{
	Nova::disableInspectWindow();
	inspectObject(%id);
	Nova::enableInspectWindow();
	return $inspector::objectTeam;
}

function Nova::focusVehicleCamera(%id)
{
	if(!isObject(%id))
	{
		echo("Nova::focusVehicleCamera: ", %id, " not found."); 
		return;
	}
	if(!isVehicle(%id))
	{
		echo("Nova::focusVehicleCamera: ", %id, " is not a vehicle."); 
		return;
	}
	if(%id != Nova::playerNameToID($client::name) && isVehicle(%id))
	{
		focusCamera(%id);
		schedule("Control::setVisible(IDHUD_AIM_RET,true);",0.1);
		schedule("Control::setVisible(IDHUD_DAMAGE,true);",0.1);
		schedule("Control::setVisible(IDHUD_TARGET,true);",0.1);
		return true;
	}
}

function Nova::setTerrainVisibilities()
{
	if($pref::customTerrainVisbility)
	{
		Nova::disableTerrainUpdates();
		if(isObject(8))
		{
			setTerrainVisibility(8, _($pref::terrainVisDist,2000), _($pref::terrainVisDist*0.4725,1750), 0, 0);
		}
	}
	else
	{
		Nova::enableTerrainUpdates();
	}
}

function Nova::freeCam()
{
	if(!$_inFreeCam)
	{
		setPosition(655, getPosition(pick(squad),x), getPosition(pick(squad),y), getPosition(pick(squad),z)+8, getPosition(pick(squad),d));
		exec(Nova_move);focusCamera(655);
	}
	else
	{
		focusCamera(player);
	}
	$_inFreeCam^=1;
}

//function Nova::openSimPrefs()
//{
//	if(isObject(playGui))
//	{
//		GuiPushDialog(simcanvas, "SimPrefs.dlg");
//		renameObject(Nova::findGuiTagControl("NamedGuiSet\\SimPrefsWindow", IDSTR_EDIT), KeyMapButton);
//		if($zzIsCampaign)
//		{
//			control::setVisible(KeyMapButton,0);
//			control::setActive(KeyMapButton,0);
//		}
//	}
//}

//Some inits that need to be called
Mem::initFarWeather();
Nova::setCursorSensitivity(_($pref::cursorSensitivity,1));
Nova::toggleMapviewSmoothTerrain($pref::mapviewSmoothTerrain);

function Nova::getLastClientObject()
{
	while(getNextObject(0,%iter) != 0)
	{
		%iter = getNextObject(0,%iter);
	}
	return %iter;
}

function Nova::externalSetCollMeshColors()
{
	%u_r = _($pref::collMeshColor::undamaged::red,135);
	%u_g = _($pref::collMeshColor::undamaged::green,135);
	%u_b = _($pref::collMeshColor::undamaged::blue,135);
	
	%d_r = _($pref::collMeshColor::damaged::red,255);
	%d_g = _($pref::collMeshColor::damaged::green,255);
	%d_b = _($pref::collMeshColor::damaged::blue,0);
	
	%c_r = _($pref::collMeshColor::critical::red,255);
	%c_g = _($pref::collMeshColor::critical::green,0);
	%c_b = _($pref::collMeshColor::critical::blue,0);
	
	Nova::setCollMeshColors("undamaged", %u_r, %u_g, %u_b);
	Nova::setCollMeshColors("damaged", %d_r, %d_g, %d_b);
	Nova::setCollMeshColors("critical", %c_r, %c_g, %c_b);
}