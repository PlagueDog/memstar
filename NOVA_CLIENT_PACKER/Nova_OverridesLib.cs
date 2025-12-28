function Nova::executeEvent(%object_str, %class_str, %event_str, %handle1, %handle2)
{
    //if(!$zz_MLoverride)
    //{
    //    exec("modLoaderOverridesLib.cs");
    //}
    //$zz_MLoverride=true;
    //schedule("$zz_MLoverride=false;",1);
    if(!strlen(%class_str) || !strlen(%event_str))
    {
        return;
    }
    %i=0;
    if(!strlen(%handle1)){%handle1=_;}
    if(!strlen(%handle2)){%handle2=_;}
    %console::printLevel=$console::printLevel;
    $console::printlevel=0;
    while(strlen($modLoader::mod[%i++,filename]))
    {
        if($modLoader::mod[%i,enabled])
        {
            //Trim the file extensions
            if(String::getSubStr($modLoader::mod[%i,filename], strlen($modLoader::mod[%i,filename])-3, strlen($modLoader::mod[%i,filename])) == ".cs")
            {%offset=3;}
        
            else{%offset=4;}
            
            %str = String::getSubStr($modLoader::mod[%i,filename], 0, strlen($modLoader::mod[%i,filename]) - %offset);
            if(strlen(%object_str) != 0)
            {
                eval(%object_str @ "::" @ %class_str @ "::" @ %event_str @ "::" @ %str @ "(" @ %handle1 @ "," @ %handle2 @ ");");
            }
            else if(strlen(%class_str) != 0)
            {
                eval(%event_str @ "::" @ %str @ "();");
            }
            else
            {
                eval(%class_str @ "::" @ %event_str @ "::" @ %str @ "(" @ %handle1 @ "," @ %handle2 @ ");");
            }
        }
    }
    eval(%class_str @ "::" @ %event_str @ "::" @ "modLoaderFunc(" @ %handle1 @ "," @ %handle2 @ ");");
    $console::printLevel=%console::printLevel;
}
    
function MLplyr::onAdd(%cli)
{
    player::onAdd(%cli); //Run existing scripts
    Nova::executeEvent("", player, onAdd, %cli); //Append mod/modloader functions to the event
    modloader::enforceModloader(%cli);
}

function MLveh::onAdd(%id)
{
    vehicle::onAdd(%id);
    Nova::executeEvent("", vehicle, onAdd, %id);
}

function onMissionEnd(){}
function MLMissionEnd()
{
    onMissionEnd();
	
    Nova::executeEvent("", "", onMissionEnd);
	
	if(isCampaign())
	{
		return;
	}
	
    if(($pref::enforceModloader || $server::enforceNova) && getConnection(%playerID) != LOOPBACK)
    {
        while(%i <= playerManager::getPlayerCount())                                                   
        {           
			if(focusserver() && isObject(8))
			{
				focusclient();
				%terrainGridName = String::Explode(getTerrainGridFile(), "#", gridStringTrim);
				%fileName = $gridStringTrim[0] @ ".vol";
				%terrainFilePath = "multiplayer/" @ %fileName;
				deleteVariables("gridStringTrim*");
				focusserver();
				remoteEval(playerManager::getPlayerNum(%i), modloader::validateFile, %terrainFilePath, getSHA1(%terrainFilePath));
				if (!$CmdLineServer)
				{
					focusclient();
				}
			}
            %i++;            
        }
	}
}

function vehicle::onAdd::modLoaderFunc(%id)
{
    %client = playerManager::vehicleIDtoPlayerNum(%id);
    if(%client != 0)
    {
        remoteEval(%client, modloader::clientBroadcastVehicleSkin);
    }
}

function MLplyr::onRemove(%cli)
{
    player::onRemove(%cli);
    Nova::executeEvent("", player, onRemove, %cli);
} 

function player::onRemove::modLoaderFunc(%player)
{
    $clientToken[%player] = 0;
}
function player::onRemove(){}
function player::onAdd(){}
function player::onAdd::modLoaderFunc(%cli)
{
    if($pref::enforceModloader)
    {
        remoteEval(%cli, modloader::setGameSpeed, $server::timescale);
    }
}

//function vehicle::OnAttacked(%ed,%er){}
//function vehicle::OnEnabled(%a){}
//function vehicle::OnDisabled(%a){}
//function vehicle::OnDestroyed(%ed,%er){}
//function vehicle::OnArrived(%a){}   
//function vehicle::OnScan(%er,%ed){}    
//function vehicle::OnSpot(%er,%ed){}    
//function vehicle::OnNewLeader(%a){}   
//function vehicle::OnNewTarget(%er,%ed){}
//function vehicle::OnTargeted(%er,%ed){}
//function vehicle::OnMessage(%a){}     
//function vehicle::OnAction(%a){}       
//
//function MLveh::OnAttacked(%ed,%er) {Nova::executeEvent("", vehicle, onAttacked, %ed, %er); vehicle::OnAttacked(%ed,%er); }
//function MLVeh::OnEnabled(%a)       {Nova::executeEvent("", vehicle, onEnabled, %a);        vehicle::OnEnabled(%a);       }
//function MLVeh::OnDisabled(%a)      {Nova::executeEvent("", vehicle, onDisabled, %a);       vehicle::OnDisabled(%a);      }
//function MLVeh::OnDestroyed(%ed,%er){Nova::executeEvent("", vehicle, onDestroyed, %ed, %er);vehicle::OnDestroyed(%ed,%er);}
//function MLVeh::OnArrived(%a)       {Nova::executeEvent("", vehicle, onArrived, %a);        vehicle::OnArrived(%a);       }
//function MLVeh::OnScan(%er,%ed)     {Nova::executeEvent("", vehicle, onScan, %er, %ed);     vehicle::OnScan(%er,%ed);     }
//function MLVeh::OnSpot(%er,%ed)     {Nova::executeEvent("", vehicle, onSpot, %er, %ed);     vehicle::OnSpot(%er,%ed);     }
//function MLVeh::OnNewLeader(%a)     {Nova::executeEvent("", vehicle, onNewLeader, %a);      vehicle::OnNewLeader(%a);     }
//function MLVeh::OnNewTarget(%er,%ed){Nova::executeEvent("", vehicle, onNewTarget, %er, %ed);vehicle::OnNewTarget(%er,%ed);}
//function MLVeh::OnTargeted(%er,%ed) {Nova::executeEvent("", vehicle, onTargeted, %er, %ed); vehicle::OnTargeted(%er,%ed); }
//function MLVeh::OnMessage(%a)       {Nova::executeEvent("", vehicle, onMessage, %a);        vehicle::OnMessage(%a);       }
//function MLVeh::OnAction(%a)        {Nova::executeEvent("", vehicle, onAction, %a);         vehicle::OnAction(%a);        }

$Hardware::3D::Type1 = "3Dfx Chipset";
$Hardware::3D::Callback1 = "OptionsVideo::setUpGlide();";
$Hardware::3D::Type2 = "nVidia Riva TNT Chipset";
$Hardware::3D::Callback2 = "OptionsVideo::setUpTNT();";
$Hardware::3D::Type3 = "nVidia Riva TNT2 Chipset";
$Hardware::3D::Callback3 = "OptionsVideo::setUpTNT2();";
$Hardware::3D::Type4 = "i740 Chipset";
$Hardware::3D::Callback4 = "OptionsVideo::setUpI740();";
$Hardware::3D::Type5 = "S3 Savage 3D";
$Hardware::3D::Callback5 = "OptionsVideo::setUpSavage3D();";
$Hardware::3D::Type6 = "ATI Rage 128 Chipset";
$Hardware::3D::Callback6 = "OptionsVideo::setUpRage128();";
$Hardware::3D::Type7 = "Matrox G200 Chipset";
$Hardware::3D::Callback7 = "OptionsVideo::setUpG200();";
$Hardware::3D::Type8 = "Matrox G400 Chipset";
$Hardware::3D::Callback8 = "OptionsVideo::setUpG400();";

function OptionsVideo::setUpGenericOpenGL()
{
   echo("setting up generic OpenGL");

   $pref::OpenGL::NoPackedTextures     = false;
   $pref::OpenGL::NoPalettedTextures   = false;
   $pref::OpenGL::VisDistCap           = 4000;

   flushTextureCache();
}

function OptionsVideo::setUpGlide()
{
   echo("setting up Glide");
   
   flushTextureCache();
}

function OptionsVideo::setUpTNT()
{
   echo("setting up Riva TNT");

   $pref::OpenGL::NoPackedTextures     = false;
   $pref::OpenGL::NoPalettedTex        = true;
   $pref::OpenGL::VisDistCap           = 1600;

   flushTextureCache();
}

function OptionsVideo::setUpTNT2()
{
   echo("setting up Riva TNT2");

   $pref::OpenGL::NoPackedTextures     = false;
   $pref::OpenGL::NoPalettedTex        = true;
   $pref::OpenGL::VisDistCap           = 1800;

   flushTextureCache();
}

function OptionsVideo::setUpI740()
{
   echo("setting up i740");

   $pref::OpenGL::NoPackedTextures     = false;
   $pref::OpenGL::NoPalettedTextures   = false;
   $pref::OpenGL::VisDistCap           = 750;

   flushTextureCache();
}

function OptionsVideo::setUpSavage3D()
{
   echo("setting up S3 Savage 3D");

   $pref::OpenGL::NoPackedTextures     = false;
   $pref::OpenGL::NoPalettedTextures   = false;
   $pref::OpenGL::VisDistCap           = 750;

   flushTextureCache();
}

function OptionsVideo::setUpRage128()
{
   echo("setting up ATI Rage 128");

   $pref::OpenGL::NoPackedTextures     = false;
   $pref::OpenGL::NoPalettedTextures   = false;
   $pref::OpenGL::VisDistCap           = 1500;

   flushTextureCache();
}

function OptionsVideo::setUpG200()
{
   echo("setting up Matrox G200");

   $pref::OpenGL::NoPackedTextures     = false;
   $pref::OpenGL::NoPalettedTextures   = false;
   $pref::OpenGL::VisDistCap           = 1000;

   flushTextureCache();
}

function OptionsVideo::setUpG400()
{
   echo("setting up Matrox G400");

   $pref::OpenGL::NoPackedTextures     = false;
   $pref::OpenGL::NoPalettedTextures   = false;
   $pref::OpenGL::VisDistCap           = 1200;

   flushTextureCache();
}