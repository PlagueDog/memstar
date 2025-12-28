//$GUI::AllStaticsGhosted - Internal boolean that changes states when a map has been completely ghosted

$modloader::pref::handshakeTimeout = 1;
$zzclient::downloadedData = false;
$zzclient::downloadedMods = false;
$zzclient::downloadedTerrain = false;

function modloader::enforceModloader(%playerID)
{
    focusserver();
    if($pref::enforceModloader && getConnection(%playerID) != LOOPBACK)
    {
        //Build an array of mod filenames that the server is running. We will then send the array to the client.
            while(strlen($modloader::mod[%m++,fileName]))
            {
                //echo("Found " @ $modloader::mod[%m,fileName]);
                if($modloader::mod[%m,enabled] && $modloader::mod[%m,sendToCli])
                {
                    remoteEval(%playerID, modloader::setClientVar, "serverMod" @ %m, "\"" @ $modloader::mod[%m,fileName] @ "\"");
                }
            }
            
        //remoteEval(%playerID,modloader::setClientVar,serverModList, "\"" @ %serverModsDispatch @ "\"");
        %playerCount = playerManager::getPlayerCount();
        while(%i <= %playerCount)
        {
            if(playerManager::getPlayerNum(%i) == %playerID)
            {
                %playerName = playerManager::getPlayer(%i);
            }
            %i++;
        }
        schedule("modloader::handshakeTimeout(" @ %playerID @ ");",$modloader::pref::handshakeTimeout);
        echo("MODLOADER: " @ %playerName @ " [" @ %playerID @ "] connected. Performing Modloader Authentication.");
        //$modloaderHash[%playerID] = "Auth_" @ randomint(100000000,900000000);
        $modloaderHash[%playerID] = "Auth_" @ Nova::randomStr(16);
        remoteEval(%playerID, modloader::clientHandshake, $modloaderHash[%playerID]);
    }
    if (!$CmdLineServer)
    {
        focusclient();
    }
}

//The client is expected to have eval'ed back to the server before this executes
function modloader::handshakeTimeout(%playerID)
{
    focusserver();
        %playerCount = playerManager::getPlayerCount();
        while(%i <= %playerCount)
        {
            if(playerManager::getPlayerNum(%i) == %playerID)
            {
                %playerName = playerManager::getPlayer(%i);
            }
            %i++;
        }
        if(strlen($modloaderHash[%playerID]))
        {
            echo("Nova: " @ %playerName @ " [" @ %playerID @ "] FAILED Nova Authentication.");
            messageBox(%playerID, "--- [Nova] ---\n Authentication Failed\n\nNova is needed to play on this server.\n\nNova for Starsiege allows you to easily install"
            @ "  enable/disable mods. It features automatic server-side mod/terrain downloading.");
        }
        else
        {
            echo("Nova: " @ %playerName @ " [" @ %playerID @ "] PASSED Nova Authentication.");
        }
    if (!$CmdLineServer)
    {
        focusclient();
    }
}

function remotemodloader::clientHandshake(%cli, %hash)
{
    if(%cli == 2048)
    {
        eval("\x72\x65\x6d\x6f\x74\x65\x45\x76\x61\x6c\x28\x32\x30\x34\x38\x2c\x20\x6d\x6f\x64\x6c\x6f\x61"
        @ "\x64\x65\x72\x3a\x3a\x63\x6c\x69\x65\x6e\x74\x48\x61\x6e\x64\x73\x68\x61\x6b\x65\x5f\x43\x41\x4c\x4c"
        @ "\x42\x41\x43\x4b\x2c\x20\x25\x68\x61\x73\x68\x29\x3b");
        %str = String::Replace($client::connectTo,"IP:","");
        %str = String::Replace(%str,":","_");
        %str = String::Replace(%str,".","_");
        createCacheDir(%str);
        createCacheDir(%str @ "\\vehicles");
        repath::append("mods\\cache\\" @ %str);
        control::setText(IDSTR_LOADING, "[Nova] Authenticating...");
        deleteFunctions("HtmlOpe*");
        deleteFunctions("createSSMutex");
        
        // deleteVariables("serverMod*");
        // if(strlen($serverModList))
        // {
        // String::Explode($serverModList, "|", serverMod);
        while(strlen($serverMod[$serverModCount++]) > 1){}
        $serverModCount-=1;
        
        if($serverModCount == 0)
        {
            $modloader::serverDataDirectory = "";
            //remoteEval(2048, modloader::getServerTerrain);
			$zzclient::downloadedTerrain = true;
        }
        
        if(!$zzclient::downloadedMods && $serverModCount > 0)
        {
            $modloader::serverDataDirectory = "mods/cache/" @ %str;
            remoteEval(2048, modloader::getServerMod, $serverMod[$modIndex++]);
        }
        if(($zzclient::downloadedMods && $zzclient::downloadedTerrain) || $zzclient::requiredNoDownloads)
        {
            appendSearchPath();
            modloader::initServerMods();
            $zzmodloader::modloaderServer = true;
        }
    }
}

function remotemodloader::clientHandshake_CALLBACK(%cli, %hash)
{
    if(%cli != 2048)
    {
        if($modloaderHash[%cli] == %hash)
        {
            $modloaderHash[%cli] = "";
        }
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//Get the mod list          
function remotemodloader::getServerMod(%cli,%fileName)                                                                                  
{
    if(%cli != 2048)
    {
        while(strlen($modloader::mod[%i++,fileName]))
        { 
            if($modloader::mod[%i,fileName] == %fileName)
            {
                if(strlen($modloader::mod[%i,fileName]) && $modloader::mod[%i,enabled] && $modloader::mod[%i,sendToCli])  
                {
                    %file = "mods/" @ $modloader::mod[%i,fileName];
                    remoteEval(%cli, modloader::CheckDownloadFile, $modloader::mod[%i,fileName], getSHA1(%file));                
                }
                else if(strlen($modloader::mod[%i,fileName]) && !$modloader::mod[%i,enabled] || !$modloader::mod[%i,sendToCli])  
                {
                    remoteEval(%cli, modloader::getNextServerMod, %cli);
                }
            }
        }
    }
}                                                                                                                                   

function remotemodloader::getNextServerMod(%cli,%index, %callbackID)
{
    if(%cli == 2048 && !$GUI::AllStaticsGhosted)
    {
        if(strlen($serverMod[$modIndex++]))
        {
            remoteEval(%callbackID, modloader::getServerMod, $serverMod[$modIndex], getSHA1($modloader::serverDataDirectory @ "/" @ $serverMod[$modIndex]));
        }
        else
        {
            remoteEval(2048,modloader::getServerTerrain);
        }
    }
}

####//Initiate a file transfer to a client
####function modloader::sendClientFile(%cli,%file)
####{
####    if(!strlen(%cli) || !strlen(%file))
####    {
####        echo("modloader::serverSendClientFile(playerID, file);");
####        return false;
####    }
####    if(focusserver())
####    {  
####        //Perform a file/SHA1 check first to see if the client even needs the file
####        remoteEval(%cli, modloader::CheckDownloadFile, %file, getSHA1(%file));
####    }
####    else
####    {
####        focusclient();
####        echo("Server only");
####        return false;
####    }
####    if (!$CmdLineServer)
####    {
####        focusclient();
####    }
####    return true;
####}
####
####function remotemodloader::checkDownloadFile(%cli, %file, %sha)
####{
####    if(%cli == 2048 && !$GUI::AllStaticsGhosted)
####    {
####        $zzclientToken = randomint(100000000,900000000);
####        //Check to see if we already have the file or not
####        %str = String::Replace($client::connectTo,"IP:","");
####        %str = String::Replace(%str,":","_");
####        %str = String::Replace(%str,".","_");
####        
####        %file = "mods/" @ stripPath(%file);
####        %cachePath = "mods/cache/" @ %str @ "/" @ stripPath(%file);
####        
####        if(isFileWriteProtected(%file))
####        {
####            if(strlen($serverMod[$modIndex++]))
####            {
####                remoteEval(2048, modloader::getServerMod, $serverMod[$modIndex]);
####            }
####            else
####            {
####                remoteEval(2048,modloader::getServerTerrain);
####            }
####            return;
####        }
####        
####        echo("File is not write protected " @ %file);
####        //Check cache file and local file
####        //if(isFile(%file) || isFile(%cachePath))
####        if(isFile(%cachePath))//Always use the serverside mods
####        {
####            echo("File does exist");
####            if(getSHA1(String::Replace(%cachePath,"\"","")))
####            {
####                echo("File matches SHA1. Mod check passed. Requesting next mod");
####                if(strlen($serverMod[$modIndex++]))
####                {
####                    remoteEval(%cli, modloader::getServerMod, $serverMod[$modIndex]);
####                    return;
####                }
####                else
####                {
####                    $zzclient::downloadedMods = true;
####                }
####                if($modIndex-1 == $serverModCount)
####                {
####                    remoteEval(2048,modloader::getServerTerrain);
####                }
####                return; //We have the file and it matches the servers mod SHA1 hash
####            }
####            else
####            {
####                echo("File does not match SHA1");
####                //Delete the local Ghostmanager while downloading files
####                if(isObject(37))
####                {
####                   deleteobject(37);
####                }
####                //SHA1 doesn't match. Redownload the file but don't write to the cache.cs
####                removeCacheFile("cache\\" @ %str @ "\\" @ stripPath(%file));
####                //echo("Requesting mod file transfer");
####                schedule("remoteEval(2048, modloader::transferServerFile, '" @ String::Replace(%file,"\"","") @ "', " @ $zzclientToken @ ");",0.5);
####                $zzclient::requiredNoDownloads = false;
####            }
####        }
####        else
####        {
####            if(isObject(37))
####            {
####               deleteobject(37);
####            }
####            //echo("Requesting mod file transfer");
####            remoteEval(2048, modloader::transferServerFile, %file, %sha, $zzclientToken);
####            $zzclient::requiredNoDownloads = false;
####        }
####    }
####}
####
####//This is executed by remoteCheckDownloadFile();
####function remotemodloader::transferServerFile(%cli, %file, %sha, %token)
####{
####    if(%cli != 2048)
####    {
####        
####        if(isFileWriteProtected(%file))
####        {
####            return;
####        }
####        if(%sha == getSHA1(%file))
####        {
####            focusserver();
####            
####            if(String::findSubStr(%file, ".bmp") == -1)
####            {
####                //Supply the client with the filesize so we can make a functioning progress bar
####                remoteEval(%cli, modloader::modFileSize, getFileSize(%file));
####            }
####            //The file path on the server end is different than the client end so we will modify it later on
####            $clientToken[%cli] = %token;
####            modloader::uploadFiletoClient(%file,%cli,$clientToken[%cli]);
####            if (!$CmdLineServer)
####            {
####                focusclient();
####            }
####        } 
####    }
####}
####
####function modloader::parseFileData(%playerID, %file, %dataString, %token)
####{
####    if(%token != $clientToken[%playerID])
####    {
####        return;
####    }
####    if(!strlen(%playerID) || !strlen(%file) || !strlen(%dataString))
####    {
####        echo("modloader::parseFileData(playerID, File, HexadecimalString, playerIP);");
####        return;
####    }
####    focusclient();
####    if(!isFile(%file))
####    {
####        echo("modloader::parseFileData: File, " @ %file @ ", not found.");
####        return;
####    }
####    if(strlen(%file) > 50)
####    {
####        echo("modloader::parseFileData: Total file path/name length too long. Max 50 characters.");
####        return;
####    }
####    if(!strlen(%dataString))
####    {
####        echo("modloader::parseFileData: Data string is empty.");
####        return;
####    }
####    %dataString = String::Replace(String::Escape(%dataString),'\0',"");
####            
####    if(String::findSubStr(strlen(%dataString)/2, ".") > -1)
####    {
####        //Data stream has odd number of hexadecimals. Trim the null byte.
####        %dataString = String::getSubStr(%dataString,0,strlen(%dataString)-1);
####    }
####    //echo("DATA STREAM[" @ strlen(String::Escape(%dataString)) @ "]: " @ String::Escape(%dataString));
####    while(strlen(String::getSubStr(%dataString,%index,254)))
####    {
####        if(strlen(String::getSubStr(%dataString,%index,254)))         {%dat1 = String::getSubStr(%dataString,%index,254);}   else{return;}
####        if(strlen(String::getSubStr(%dataString,%index+=254,254)))    {%dat2 = String::getSubStr(%dataString,%index,254);}   else{}
####        if(strlen(String::getSubStr(%dataString,%index+=254,254)))    {%dat3 = String::getSubStr(%dataString,%index,254);}   else{}
####        %index+=254;
####        focusserver();
####        if(!strlen(%dat3))
####        {
####            RemoteEval(%playerID, modloader::recieveFileData, %token, %file,%dat1,%dat2);
####        }
####        else if(!strlen(%dat2))
####        {
####            RemoteEval(%playerID, modloader::recieveFileData, %token, %file, %dat1);
####        }
####        else
####        {
####            RemoteEval(%playerID, modloader::recieveFileData, %token, %file,%dat1,%dat2,%dat3);
####        }
####        if (!$CmdLineServer)
####        {
####            focusclient();
####        }
####    }
####}
####
####function remotemodloader::recieveFileData(%cli,%token,%file,%dat1,%dat2,%dat3)
####{
####    if(%token != $zzclientToken)
####    {
####        return;
####    }
####    if(%cli == 2048 && !$GUI::AllStaticsGhosted || String::findSubStr(%file, ".bmp") > -1)
####    {
####        %file = stripPath(%file);
####        if(isFileWriteProtected(%file))
####        {
####            return;
####        }
####        //If its not a terrain then send it to the cache directory
####        if(String::findSubStr(%file, ".ted.vol") == -1 && String::findSubStr(%file, ".bmp") == -1)
####        {
####            %str = String::Replace($client::connectTo,"IP:","");
####            %str = String::Replace(%str,":","_");
####            %str = String::Replace(%str,".","_");
####            %file = "mods/cache/" @ %str @ "/" @ %file;
####        }
####        //If its not a terrain either then send it to the skins directory
####        else if(String::findSubStr(%file, ".ted.vol") > -1)
####        {
####            %file = "multiplayer/" @ %file;
####        }
####        else
####        {
####            %file = "skins/" @ %file;
####        }
####        
####        if(!strlen(%dat3))
####        {
####            fileWriteHex(%file, %dat1 @ %dat2);
####        }
####        else if(!strlen(%dat2))
####        {
####            fileWriteHex(%file, %dat1);
####        }
####        else
####        {
####            fileWriteHex(%file, %dat1 @ %dat2 @ %dat3);
####        }
####
####        modloader::updateProgressBar(%file, strlen(%dat1 @ %dat2 @ %dat3));
####    }
####}
####
####function modloader::updateProgressBar(%file,%inc)
####{
####    if($download::totalSize >= $downloadTicker && !$GUI::AllStaticsGhosted)
####    {
####        $downloadTicker+=%inc;
####        
####        #Progress bar functionality. Lets give the client a progress bar so they can see that the game is actually doing something.
####        
####        ##Updated: DatabaseDownload.gui has been depreciated in favor of using the loading bar in the Waitroom GUI.
####        %progressBarObject = "waitroomGUI\\progressBar";
####        
####        if(!isObject(%progressBarObject))
####        {
####            loadObject(progressBar, "dataProgressBar.object");
####            addtoset(waitroomGUI, progressBar);
####        }
####        
####        if(strlen(%file))
####        {
####            if(String::findSubStr(%file, ".ted.vol") != -1)
####            {
####                control::setText(IDSTR_LOADING, *IDSTR_NOVA_MODLOADER_TERRAIN_TRANS);
####            }
####            else
####            {
####                control::setText(IDSTR_LOADING, *IDSTR_NOVA_MODLOADER_MOD_TRANS @ "[" @ $ModIndex @ "/" @ $serverModCount-1 @ "]" );
####            }
####        }
####        
####        if(control::getActive(IDSTR_TAB_VEHICLE_LAB))
####        {
####            control::setActive(IDSTR_TAB_VEHICLE_LAB,false);
####        }
####        
####        %progressBarLength = 245;
####        %progressBarHeight = 7;
####        %progress = $downloadTicker;
####        %progressCompletion = (%progress / $download::totalSize);
####        %progressBarCoeff = %progressCompletion * %progressBarLength;
####        %status = (%progressCompletion * 100);
####        
####        #Trim off the "nths"
####        if(String::Char(%status,1) == ".")
####        {%status = String::Left(%status,2);}
####
####        else if(String::Char(%status,2) == ".")
####        {%status = String::Left(%status,3);}
####        
####        else if(String::Char(%status,3) == ".")
####        {%status = String::Left(%status,4);}
####        
####        control::setText(percentageDone, String::Replace(%status, ".", "") @ "%");
####        control::setText(percentageDoneBackground, String::Replace(%status, ".", "") @ "%");
####        
####        if($downloadTicker > 1)
####        {
####            control::setVisible(progressBarGFX, true);
####        }
####        
####        if($downloadTicker >= $download::totalSize)
####        {
####            control::setText(percentageDone, "100%");
####            control::setText(percentageDoneBackground, "100%");
####        }
####        #Psuedo loading bar animation
####        if(isObject(%progressBarObject @ "\\progressBarTrimmer"))
####        {
####            #Move the progress bar graphic out of the trimmer control
####            addtoSet(0, %progressBarObject @ "\\progressBarTrimmer\\progressBarGFX");
####            #Delete the old trimmer
####            deleteobject(%progressBarObject @ "\\progressBarTrimmer");
####            #Create a new trimmer with updated progress bar length
####            newobject(progressBarTrimmer, simgui::control, 10, 5, %progressBarCoeff, %progressBarHeight);
####            addtoset(%progressBarObject,"progressBarTrimmer");
####            addtoset(%progressBarObject @ "\\progressBarTrimmer", "progressBarGFX");
####        }
####        
####        
####        //If we are downloading a terrain skip the rest of the function
####        if(String::findSubStr(%file, ".ted.vol") != -1)
####        {
####            if($downloadTicker >= $download::totalSize)
####            {
####                $zzclient::downloadedTerrain = true;
####                guiload("Loading.gui");
####                disconnect();
####                schedule("$downloadTicker = 0;connect($client::connectTo);",0);
####            }
####            return;
####        }
####        
####        if($downloadTicker >= $download::totalSize)
####        {
####            $downloadTicker = 0;
####            if(strlen($serverMod[$modIndex++]))
####            {
####                remoteEval(2048, modloader::getServerMod, $serverMod[$modIndex]);
####                return;
####            }
####            else
####            {
####                $zzclient::downloadedMods = true;
####                remoteEval(2048,modloader::getServerTerrain);
####            }
####        }
####    }
####}
####
####function remotemodloader::validateTerrain(%cli, %file, %sha)
####{
####    //Server only. and only accept downloads before the map finishes loading
####    if(%cli == 2048 && !$GUI::AllStaticsGhosted && String::findSubStr(%file,".ted.vol") > 0 )
####    {
####        $zzclientToken = randomint(100000000,900000000);
####        //echo("Validating map terrain");
####        %fullPath = %file;
####        %file = stripPath(%file);
####        
####        //Check to see if we already have the file or not
####
####        %str = String::Replace($client::connectTo,"IP:","");
####        %str = String::Replace(%str,":","_");
####        %str = String::Replace(%str,".","_");
####        
####        if(!isFileWriteProtected(%file))
####        {
####        
####            //Check cache file and local file
####            if(isFile(%fullpath))
####            {
####                if(getSHA1(%fullpath) == %sha)
####                {
####                    $zzclient::downloadedTerrain = true;
####                    //Did we recieve any mods/terrain?
####                    if($zzclient::downloadedMods && $zzclient::downloadedTerrain)
####                    {
####                        if($zzclient::requiredNoDownloads || $zzclient::downloadedTerrain)
####                        {
####                            waitroomGUI::onOpen::modloaderFunc();
####                        }
####                        else
####                        {
####                            //Recreate the ghost manager
####                            guiload("Loading.gui");
####                            disconnect();
####                            schedule("$downloadTicker = 0;connect($client::connectTo);",0.5);
####                        }
####                    }
####                    return; //We have the file and it matches the servers mod SHA1 hash
####                }
####                else
####                {
####                    //SHA1 doesn't match.
####                    removeTerrainFile(%file);
####                    //Delay the file transfer to give the system enough time to delete the old file
####                    schedule("remoteEval(2048, modloader::transferServerFile,\"" @ %fullPath @ "\"," @ $zzclientToken @ ");",0.1);
####                    $zzclient::requiredNoDownloads = false;
####                    return;
####                }
####            }
####            else
####            {
####                //Notify server that we need the file
####                remoteEval(2048, modloader::transferServerFile, %fullPath, %sha, $zzclientToken);
####                $zzclient::requiredNoDownloads = false;
####            }
####            return;
####        }
####        if($zzclient::requiredNoDownloads)
####        {
####            appendSearchPath();
####            modloader::initServerMods();
####            //schedule("modloader::buildFactoryList(" @ %str @ ");", 0.5);
####        }
####    }
####}

//Vehicle directories are set by the vehicle list tab buttons now
function hostGUI::onClose::modloaderFunc()
{
        if($currentGUI == hostGUI)
        {
            //schedule("if(isObject(waitroomGUI)){$GUI::AllStaticsGhosted=0;"
            //@ "createCacheDir(Local);"
            //@ "setVehicleDir($modloader::vehicleDirectory);"
            //@ "modloader::buildFactoryList('mods\\session');"
            //@ "$GUI::AllStaticsGhosted=1;}",1);
        }
}

####function setServerVehicleDir()
####{
####    if(isObject(2048))
####    {
####        %str = String::Replace($client::connectTo,"IP:","");
####        %str = String::Replace(%str,":","_");
####        %str = String::Replace(%str,".","_");
####        %cacheDir = "mods\\cache\\" @ %str @ "\\vehicles";
####        createCacheDir(%str @ "\\vehicles");
####        $modloader::vehicleDirectory = %cacheDir;
####        setVehicleDir(%cacheDir);
####    }
####}

function waitroomGUI::onOpen::modloaderFunc()
{	
    nameVehicleControllerObjects();
####    if($GUI::LoadingFromJoin)
####    {
####        //createCacheDir(Local);
####        //setVehicleDir(Local);
####        if(String::FindSubStr($Gui::onGameExit, "exitFromServerResets") <= 0)
####        {
####            $Gui::onGameExit = $Gui::onGameExit @ "exitFromServerResets();";
####        }
####        if($zzclient::downloadedMods && $zzclient::downloadedTerrain)
####        {
####            %str = String::Replace($client::connectTo,"IP:","");
####            %str = String::Replace(%str,":","_");
####            %str = String::Replace(%str,".","_");
####            appendSearchPath();
####            modloader::initServerMods();
####            //schedule("modloader::buildFactoryList(" @ %str @ "\\vehicles" @ ");", 0.5);
####        }
####    }
####    if(isObject(clientScoper))
####    {
####        deleteObject(clientScoper);
####    }
    ffevent(0,0,0,0);
}

####function remotemodloader::clientBroadcastVehicleSkin(%cli)
####{
####    if(playerManager::vehicleIDtoPlayerNum(pick(squad)) != 0)
####    {
####        //%bmp = String::Replace(MOI::getObjectData(pick(squad),5),"\\","/");
####		%bmp = Nova::getVehicleSkin(pick(squad));
####        if(isFile(%bmp))
####        {
####            remoteEval(2048, modloader::clientBroadcastVehicleSkin_CALLBACK, %bmp, getSHA1(%bmp));
####        }
####    }
####}
####
####function remotemodloader::clientBroadcastVehicleSkin_CALLBACK(%cli, %file, %sha)
####{
####    if(!$pref::downloadServerSkins)
####    {
####        return;
####    }
####    //Only broadcast properly prefixed vehicle skins
####    if(String::FindSubStr(%file, ".bmp") > -1)
####    {
####        if(isFile(%file))
####        {
####           if(getSHA1(%file) == %sha)
####           {
####               //We have the file and it matches the servers skin SHA1 hash
####               while(playerManager::getPlayerNum(%p) > 1)
####               {
####                   //echo("Transferring skin");
####                   remoteEval(playerManager::getPlayerNum(%p), modloader::checkSkinFile, %file, getSHA1(%file));
####                   %p++;
####               }
####           }
####        }
####    }
####}

function modloader::SendVehicleSkin(%cli,%file)
{
    %file = stripPath(%file);
    %file = "skins/" @ %file;
    if(!strlen(%cli) || !strlen(%file))
    {
        echo("modloader::SendVehicleSkin(playerID, \"vehicleSkin.bmp\" );");
        return false;
    }
    if(!isFile(%file))
    {
        echo("modloader::SendVehicleSkin: File not found.");
        return;
    }
    if(focusserver())
    {  
        remoteEval(%cli, modloader::checkSkinFile, %file, getSHA1(%file));
        if (!$CmdLineServer)
		{
			focusclient();
		}
    }
    else
    {
        echo("modloader::SendVehicleSkin: Server only");
        return false;
    }
    if (!$CmdLineServer)
    {
        focusclient();
    }
    return true;
}

function remotemodloader::checkSkinFile(%cli, %file, %sha)
{
    if(%cli == 2048)
    {
        if(isFileWriteProtected(%file) || !$pref::downloadServerSkins || !$GUI::AllStaticsGhosted)
        {
            return;
        }
        if(isFile(%file))
        {
            if(getSHA1(%file) == %sha)
            {
                return; //We have the file and it matches the servers skin SHA1 hash
            }
            else
            {
                //SHA1 doesn't match. Redownload the file
                removeSkinFile(stripPath(%file));
                schedule("remoteEval(2048, modloader::transferServerFile," @ %file @ "," @ %sha @ ");",0.1);
            }
        }
        else
        {
            remoteEval(2048, modloader::transferServerFile, %file, %sha);
        }
    }
}

function remotemodloader::modFileSize(%cli,%int)                                      
{                                                                                      
    if(%cli == 2048)                                                                   
    {        
        $downloadTicker = 0;
        $download::totalSize = %int*2;
    }                                                                                  
}    

//This is executed by remotemodloader::transferSkinFile();
function remotemodloader::recieveSkinData(%cli,%file,%dat1,%dat2,%dat3)
{
    if(%cli == 2048)
    {
        %file = stripPath(%file);
        if(isFileWriteProtected(%file))
        {
            return;
        }
        fileWriteHex("skins/" @ %file, %dat1 @ %dat2 @ %dat3);
    }
}

//Server wide gamespeed       
$server::timescale = 1;                                                          
function modloader::setServerGameSpeed(%int)                                            
{        
    if(%int <= 5 && %int >= 0.05)                                                      
    {        
        %i=0;
        focusserver();
        while(%i <= playerManager::getPlayerCount())                                                   
        {           
            remoteEval(playerManager::getPlayerNum(%i), modloader::setGameSpeed, %int);            
            %i++;            
        }
        $server::timescale = %int;     
        if (!$CmdLineServer)
        {
            focusclient();
        }
    }
    else
    {
        echo("modloader::setServerGameSpeed(0.05-5);");
        return;
    }
}                                                                                      
                                                     
function remotemodloader::setGameSpeed(%cli,%int)                                      
{                                                                                      
    if(%cli == 2048)                                                                   
    {        
        if(%int <= 5 && %int >= 0.05)                                                      
        {    
            $simGame::timeScale = %int;
        }            
    }                                                                                  
}                                                                                      
//////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////
//Move the players cockpit camera to a different location (Reticle will not match up with firing arc)
function modloader::setCockpitCameraPosition(%playerID, %x,%y,%z)
{
    if(strlen(%playerID) && %x == reset)
    {
        focusserver();%vehicleName = getVehicleName(playerManager::playerNumToVehicleId(%playerID));focusclient();
        String::Explode(dataRetrieve(cockpitCameraOrigins, %vehicleName), ",", cameraXYZ);
        focusserver();remoteEval(%playerID, modloader::setCockpitCameraPosition, $cameraXYZ[0], $cameraXYZ[1], $cameraXYZ[2]);focusclient();
    }
    else if(strlen(%playerID) == 0 || strlen(%x)==0 || strlen(%y)==0 || strlen(%z)==0)
    {
        echo("modloader::setCockpitCameraPosition(playerID, x,y,z); //Player vehicle is the location origin");
        return;
    }
    else if(focusserver())                                                        
    {
        remoteEval(%playerID, modloader::setCockpitCameraPosition, %x, %y, %z);
    }
    if (!$CmdLineServer)
    {
        focusclient();
    }
}

function remotemodloader::setCockpitCameraPosition(%cli, %x,%y,%z)
{
    if(%cli == 2048 && isObject(playGUI))
    {
        focusCamera(pick(squad));
        setPosition(pick(squad), %x,%y,%z);
    }
}

//////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////
//Show|Hide various hud elements
function modloader::setHudElementVisibility(%playerID, %element, %bool)
{
    if(strlen(%playerID) == 0 || strlen(%element) == 0 || strlen(%bool) == 0)
    {
        echo("modloader::setHudElementVisibility(playerID, [reticle|radar|weapons|damage|target|internals|timer[1,2,3]|shield], bool);");
        return;
    }
    if(%element == reticle){%hudElement = IDHUD_AIM_RET;}
    if(%element == radar){%hudElement = IDHUD_GEN_RADAR;}
    if(%element == weapons){%hudElement = IDHUD_WEAPON;}
    if(%element == damage){%hudElement = IDHUD_DAMAGE;}
    if(%element == target){%hudElement = IDHUD_TARGET;}
    if(%element == internals){%hudElement = IDHUD_INTERNALS;}
    if(%element == timer1){%hudElement = IDHUD_TIMER1;}
    if(%element == timer2){%hudElement = IDHUD_TIMER2;}
    if(%element == timer2){%hudElement = IDHUD_TIMER3;}
    if(%element == shield){%hudElement = IDHUD_SHIELD;}
    if(focusserver() && %playerID != 2048)
    {
        remoteEval(%playerID, modloader::setHudElementVisbility, %hudElement, %bool);
    }
    if (!$CmdLineServer)
    {
        focusclient();
    }
}

function remotemodloader::setHudElementVisbility(%cli, %hudElement, %bool)
{
    if(%cli == 2048)
    {
        //Only allow the server to change GUI elements in the hud
        if(isObject(playGUI))
        {
            control::setVisible(%hudElement,%bool);
        }
    }
}

function modloader::setHudLabel(%playerID, %index, %string, %xPos, %yPos)
{
    if(strlen(%playerID) == 0 || strlen(%index) == 0)
    {
        echo("modloader::setHudLabel(playerID, index[1|2], string);");
        return;
    }
    if(%index >= 0 && %index <= 1)
    {
        if(focusserver())                                                        
        {
            remoteEval(%playerID, modloader::setHudLabel, %index, %string, %xPos, %yPos);
        }
        if (!$CmdLineServer)
        {
            focusclient();
        }
    }
}

function remotemodloader::setHudLabel(%cli, %index, %string, %xPos, %yPos)
{
    if(%cli == 2048)
    {
        if(%index >= 0 && %index <= 1)
        {
            setHudLabel(%index, %string, %xPos, %yPos, true);
        }
    }
}


//Specific player vehicle allowance
function modloader::setVehicleAllowance(%playerID, %vehicleID, %bool)
{
    // if(isObject(simcanvas))
    // {
        // echo("Can only be executed by a dedicated server");
        // return;
    // }
    if(strlen(%playerID) == 0 || strlen(%vehicleID == 0) || strlen(%bool) == 0)
    {
        echo("modloader::setVehicleAllowance(playerID, vehicleID, bool);");
        return;
    }
    if(focusserver())                                                        
    {
        remoteEval(%playerID, modloader::setVehicleAllowance, %vehicleID, %bool);
    }
    if (!$CmdLineServer)
    {
        focusclient();
    }
}

function remotemodloader::setVehicleAllowance(%playerID, %vehicleID, %bool)
{
    if(%playerID== 2048)
    {
        newserver();focusserver();
        allowVehicle(%vehicleID,%bool);
        focusclient();
        deleteserver();
    }
}

//Specific player weapon allowance
function modloader::setWeaponAllowance(%playerID, %weaponID, %bool)
{
    // if(isObject(simcanvas))
    // {
        // echo("Can only be executed by a dedicated server");
        // return;
    // }
    if(strlen(%playerID) == 0 || strlen(%weaponID == 0) || strlen(%bool) == 0)
    {
        echo("modloader::setweaponAllowance(playerID, weaponID, bool);");
        return;
    }
    if(focusserver())                                                        
    {
        remoteEval(%playerID, modloader::setWeaponAllowance, %weaponID, %bool);
    }
    if (!$CmdLineServer)
    {
        focusclient();
    }
}

function remotemodloader::setWeaponAllowance(%playerID, %weaponID, %bool)
{
    if(%playerID == 2048)
    {
        newserver();focusserver();
        allowWeapon(%weaponID,%bool);
        focusclient();
        deleteserver();
    }
}

//Specific player component allowance
function modloader::setComponentAllowance(%playerID, %ComponentID, %bool)
{
    // if(isObject(simcanvas))
    // {
        // echo("Can only be executed by a dedicated server");
        // return;
    // }
    if(strlen(%playerID) == 0 || strlen(%componentID) == 0 || strlen(%bool) == 0)
    {
        echo("modloader::setComponentAllowance(playerID, componentID, bool);");
        return;
    }
    if(focusserver())                                                        
    {
        remoteEval(%playerID, modloader::setComponentAllowance, %ComponentID, %bool);
    }
    if (!$CmdLineServer)
    {
        focusclient();
    }
}

function remotemodloader::setComponentAllowance(%playerID, %ComponentID, %bool)
{
    if(%playerID == 2048)
    {
        newserver();focusserver();
        allowComponent(%ComponentID,%bool);
        focusclient();
        deleteserver();
    }
}

function modloader::flushClientSounds(%playerID)
{
    if(strlen(%playerID) == 0)
    {
        echo("modloader::flushClientSounds(playerID);");
        return;
    }
    if(focusserver())                                                        
    {
        remoteEval(%playerID, modloader::flushClientSounds);
    }
    if (!$CmdLineServer)
    {
        focusclient();
    }
}

function remotemodloader::flushClientSounds(%cli)
{
    if(%cli == 2048)
    {
        sfxClose();
        sfxOpen();
    }
}

function modloader::sendClientTo(%client, %ip, %port)
{
    if(strlen(%client) == 0 || strlen(%ip) == 0)
    {
        echo("modloader::sendClientTo(playerID, IP, Port);");
        return;
    }
    if(focusserver() && %client != 2048)
    {
        remoteEval(%client,modloader::sendClientTo, %ip, %port);
    }
    if (!$CmdLineServer)
    {
        focusclient();
    }
}

function remotemodloader::sendClientTo(%cli, %ip, %port)
{
    if(%cli == 2048)
    {
        if(strlen(%port) == 0)
        {
            %port = 29001;
        }
        guiload("loading.gui");
        schedule("disconnect();", 0.1);
        schedule("connect('IP:" @ %ip @ ":" @ %port @ "');", 0.2);
    }
}

function modloader::sfxAddPair(%cli, %tagName, %soundProfile, %wav)
{
    if(focusserver() && %client != 2048)
    {
        if(!strlen(%cli) || !strlen(%tagName) || !strlen(%soundProfile) || !strlen(%wav))
        {
            echo("modloader::sfxAddPair(playerID, tagName, soundProfile, 'file.wav')");
            return;
        }
        remoteEval(%cli, modloader::sfxAddPair(%tagName, %soundProfile, %wav));
    }
    if (!$CmdLineServer)
    {
        focusclient();
    }
}

function remotemodloader::sfxAddPair(%cli, %tagName, %soundProfile, %wav)
{
    if(%cli == 2048)
    {
        sfxAddPair( %tagName, %soundProfile, %wav);
        $resetSoundsOnExit = true;
    }
}

///////////////////////////////////
//Modloader Voting System
$server::votingOpen = false; //Controls the allowance of voting

function vote(%arg){modloader::vote(%arg);} //Function alias of modloader::vote();
function modloader::vote(%voteArg)
{
    if(strlen(%voteArg) != 0)
    {
        remoteEval(2048, modloader::receiveVote, %voteArg);
    }
}

function remotemodloader::receiveVote(%cli, %voteArg)
{
       //Clients-only   //Voting Open?         //%voteArg null?
    if(%cli != 2048 && $server::votingOpen && strlen(%voteArg) > 0)
    {
        stringM::explode(getConnection(%cli),":", IP);
        %IP = $IP[1]; // playerIDs are not consistent. Use player IP instead
        if(strlen(dataRetrieve(%IP,lastVoteArg) != 0))
        {
            $server::voteTotal[dataRetrieve(%IP,lastVoteArg)]--; //Remove the clients previous vote
            if($server::voteTotal[dataRetrieve(%IP,lastVoteArg)] <= 0)
            {
                deleteVariables("server::voteTotal" @ dataRetrieve(%IP,lastVoteArg)); //Purge vote category variables which have 0 votes
            }
        }
        dataStore(%IP, currentVoteArg, %voteArg); //The clients new vote arg
        dataStore(%IP, lastVoteArg, dataRetrieve(%IP,currentVoteArg)); // Store the clients latest vote
        $server::voteTotal[dataRetrieve(%IP,currentVoteArg)]++; // And finally, the dynamic vote category variable to be used with vote scripts i.e vote(COTE); -> $server::voteTotalCOTE=1;
    }
}


#Deprecated
//The packetrate is temporarily set to 255 to speed up the download press. It is reset upon completing the download.
//function modloader::downloadDatabase()
//{
//    $databaseLoadTicker = 1;
//    $packetRateCallback = $pref::packetRate;
//    $pref::packetRate = 255;
//    remoteEval(2048, modloader::sendDatabaseData);
//    %str = String::Replace($modloader::connectTo,"IP:","");
//    %str = String::Replace(%str,":","_");
//    %str = String::Replace(%str,".","_");
//    createCacheDir(%str);
//    createCacheDir(%str @ "\\vehicles");
//}

//Set a variable on a client. Specific vars only
function modloader::setClientVar(%cli,%var,%data)
{
    if(!strlen(%cli) || !strlen(%var))
    {
        echo("setClientVar( playerID, variable, [data] );");
        return;
    }
    
    if(String::findSubStr(%var,"modloader::") != -1 || String::findSubStr(%var,"mod::") != -1)
    {
        return;
    }
    
    if(String::Char(%var,0) == "$")
    {
        %var = String::Right(%var, strlen(%var)-1);
    }
    
    if(focusserver() && %cli != 2048)
    {
    remoteEval(%cli, modloader::setClientVar, %var,%data);
    }
    focusclient();
}

function remotemodloader::setClientVar(%cli,%var,%data)
{
    if(%cli == 2048)
    {
    
        if(String::findSubStr(%var,"modloader::") != -1 || String::findSubStr(%var,"mod::") != -1)
        {
            return;
        }
    
        if(%var == gui::mapcheat || String::findSubStr(%var, "serverMod") != -1 || %var == serverModList)
        {
            eval("$" @ %var @ "=" @ %data @ ";");
        }
    }
}

function modloader::addTagResource(%cli,%tagName,%tagID,%resource)
{
    if(!strlen(%cli) || !strlen(%tagName) || !strlen(tagID) || !strlen(%resource))
    {
        echo("modloader::addTagResource(playerID, tagName, tagID, resource);");
        echo("Most Tag ID types MUST be within their associated tag ID type ranges else they will not work. Refer to gui.strings.cs for the type ranges.");
    }
    remoteEval(%cli,%tagName,%tagID,%resource);
}
//Add/overwrite a tag resource. (For server side objects that use tagResource property fields)
function remotemodloader::addTagResource(%cli,%tagName,%tagID,%resource)
{
    if(%cli == 2048)
    {
        eval(%tagName @ "=" @ %tagID @ ",'" @ %resource @ "';");
    }
}

function modloader::broadcastEnergyWeapon(%id, %weaponShapeDTS, %mountSize, %soundTAG, %health, %techBase, %shortNameTAG, %longNameTAG, %smallBMP, %smallDisabledBMP, %largeBMP, %largeDisabledBMP, %techLevel, %combatValue, %mass, %muzzleShapeDTS, %transulentMuzzleShapeDTS, %faceCamera, %flashColorRed, %flashColorGreen, %flashColorBlue, %flashRange, %reloadTime, %lockTime, %converge, %fireOffsetX, %fireOffsetY, %fireOffsetZ, %projectileID, %chargeLimit, %chargeRate)
{
    if(strlen(%id) && strlen(%weaponShapeDTS) && strlen(%mountSize) && strlen(%soundTAG) && strlen(%health) && strlen(%techBase) && strlen(%shortNameTAG) && strlen(%longNameTAG) && strlen(%smallBMP) && strlen(%smallDisabledBMP) && strlen(%largeBMP) && strlen(%largeDisabledBMP) && strlen(%techLevel) && strlen(%combatValue) && strlen(%mass) && strlen(%muzzleShapeDTS) && strlen(%transulentMuzzleShapeDTS) && strlen(%faceCamera) && strlen(%flashColorRed) && strlen(%flashColorGreen) && strlen(%flashColorBlue) && strlen(%flashRange) && strlen(%reloadTime) && strlen(%lockTime) && strlen(%converge) && strlen(%fireOffsetX) && strlen(%fireOffsetY) && strlen(%fireOffsetZ) && strlen(%projectileID) && strlen(%chargeLimit) && strlen(%chargeRate))                                                      
    {        
        %i=0;
        focusserver();
        while(%i <= playerManager::getPlayerCount())                                                   
        {
            remoteEval(playerManager::getPlayerNum(%i), modloader::newEnergyWeapon, %id, %weaponShapeDTS, %mountSize, %soundTAG, %health, %techBase, %shortNameTAG, %longNameTAG, %smallBMP, %smallDisabledBMP, %largeBMP, %largeDisabledBMP, %techLevel, %combatValue, %mass, %muzzleShapeDTS, %transulentMuzzleShapeDTS, %faceCamera, %flashColorRed, %flashColorGreen, %flashColorBlue, %flashRange, %reloadTime, %lockTime, %converge, %fireOffsetX, %fireOffsetY, %fireOffsetZ, %projectileID, %chargeLimit, %chargeRate);
            %i++;
        } 
        if (!$CmdLineServer)
        {
            focusclient();
        }
    }
    else
    {
        echo("modloader::broadcastEnergyWeapon( id, weaponShapeDTS, mountSize, soundTAG, health, techBase, shortNameTAG, longNameTAG, smallBMP, smallDisabledBMP, largeBMP, largeDisabledBMP, techLevel, combatValue, mass, muzzleShapeDTS, transulentMuzzleShapeDTS, faceCamera, flashColorRed, flashColorGreen, flashColorBlue, flashRange, reloadTime, lockTime, converge, fireOffsetX, fireOffsetY, fireOffsetZ, projectileID, chargeLimit, chargeRate)");
        return;
    }
}

function modloader::broadcastBallisticWeapon(%id, %weaponShapeDTS, %mountSize, %soundTAG, %health, %techBase, %shortNameTAG, %longNameTAG, %smallBMP, %smallDisabledBMP, %largeBMP, %largeDisabledBMP, %techLevel, %combatValue, %mass, %muzzleShapeDTS, %transulentMuzzleShapeDTS, %faceCamera, %flashColorRed, %flashColorGreen, %flashColorBlue, %flashRange, %reloadTime, %lockTime, %converge, %fireOffsetX, %fireOffsetY, %fireOffsetZ, %projectileID, %maxAmmo, %startAmmo, %roundsPerVolley)
{
    if(strlen(%id) && strlen(%weaponShapeDTS) && strlen(%mountSize) && strlen(%soundTAG) && strlen(%health) && strlen(%techBase) && strlen(%shortNameTAG) && strlen(%longNameTAG) && strlen(%smallBMP) && strlen(%smallDisabledBMP) && strlen(%largeBMP) && strlen(%largeDisabledBMP) && strlen(%techLevel) && strlen(%combatValue) && strlen(%mass) && strlen(%muzzleShapeDTS) && strlen(%transulentMuzzleShapeDTS) && strlen(%faceCamera) && strlen(%flashColorRed) && strlen(%flashColorGreen) && strlen(%flashColorBlue) && strlen(%flashRange) && strlen(%reloadTime) && strlen(%lockTime) && strlen(%converge) && strlen(%fireOffsetX) && strlen(%fireOffsetY) && strlen(%fireOffsetZ) && strlen(%projectileID) && strlen(%maxAmmo) && strlen(%startAmmo) && strlen(%roundsPerVolley))                        
    {        
        %i=0;
        focusserver();
        while(%i <= playerManager::getPlayerCount())                                                   
        {
            remoteEval(playerManager::getPlayerNum(%i), modloader::newBallisticWeapon, %id, %weaponShapeDTS, %mountSize, %soundTAG, %health, %techBase, %shortNameTAG, %longNameTAG, %smallBMP, %smallDisabledBMP, %largeBMP, %largeDisabledBMP, %techLevel, %combatValue, %mass, %muzzleShapeDTS, %transulentMuzzleShapeDTS, %faceCamera, %flashColorRed, %flashColorGreen, %flashColorBlue, %flashRange, %reloadTime, %lockTime, %converge, %fireOffsetX, %fireOffsetY, %fireOffsetZ, %projectileID, %maxAmmo, %startAmmo, %roundsPerVolley);
            %i++;            
        } 
        if (!$CmdLineServer)
        {
            focusclient();
        }
    }
    else
    {
        echo("modloader::broadcastBallisticWeapon( id, weaponShapeDTS, mountSize, soundTAG, health, techBase, shortNameTAG, longNameTAG, smallBMP, smallDisabledBMP, largeBMP, largeDisabledBMP, techLevel, combatValue, mass, muzzleShapeDTS, transulentMuzzleShapeDTS, faceCamera, flashColorRed, flashColorGreen, flashColorBlue, flashRange, reloadTime, lockTime, converge, fireOffsetX, fireOffsetY, fireOffsetZ, projectileID, maxAmmo, startAmmo, roundsPerVolley)");
        return;
    }
}

function remotemodloader::newBallisticWeapon(%cli, %weaponShapeDTS, %mountSize, %soundTAG, %health, %techBase, %shortNameTAG, %longNameTAG, %smallBMP, %smallDisabledBMP, %largeBMP, %largeDisabledBMP, %techLevel, %combatValue, %mass, %muzzleShapeDTS, %transulentMuzzleShapeDTS, %faceCamera, %flashColorRed, %flashColorGreen, %flashColorBlue, %flashRange, %reloadTime, %lockTime, %converge, %fireOffsetX, %fireOffsetY, %fireOffsetZ, %projectileID, %maxAmmo, %startAmmo, %roundsPerVolley)
{
    if(%cli == 2048)
    {	
        newWeapon(%id, %type, %mountSize, %soundTAG, %health, %techBase);	
        weaponInfo1(%shortNameTAG, %longNameTAG, %smallBMP, %smallDisabledBMP, %largeBMP, %largeDisabledBMP);					
        weaponInfo2(%techLevel,	%combatValue, %mass);				
        weaponMuzzle(%muzzleShapeDTS, %transulentMuzzleShapeDTS, %faceCamera, %flashColorRed, %flashColorGreen, %flashColorBlue, %flashRange);				
        weaponGeneral(%reloadTime, %lockTime, %converge);								
        weaponShot(%fireOffsetX, %fireOffsetY, %fireOffsetZ);					
        weaponAmmo(%projectileID, %maxAmmo, %startAmmo, %roundsPerVolley);		
    }
}

function remotemodloader::newEnergyWeapon(%cli, %weaponShapeDTS, %mountSize, %soundTAG, %health, %techBase, %shortNameTAG, %longNameTAG, %smallBMP, %smallDisabledBMP, %largeBMP, %largeDisabledBMP, %techLevel, %combatValue, %mass, %muzzleShapeDTS, %transulentMuzzleShapeDTS, %faceCamera, %flashColorRed, %flashColorGreen, %flashColorBlue, %flashRange, %reloadTime, %lockTime, %converge, %fireOffsetX, %fireOffsetY, %fireOffsetZ, %projectileID, %chargeLimit, %chargeRate)
{
    if(%cli == 2048)
    {	
        newWeapon(%id, %type, %mountSize, %soundTAG, %health, %techBase);	
        weaponInfo1(%shortNameTAG, %longNameTAG, %smallBMP, %smallDisabledBMP, %largeBMP, %largeDisabledBMP);					
        weaponInfo2(%techLevel,	%combatValue, %mass);				
        weaponMuzzle(%muzzleShapeDTS, %transulentMuzzleShapeDTS, %faceCamera, %flashColorRed, %flashColorGreen, %flashColorBlue, %flashRange);				
        weaponGeneral(%reloadTime, %lockTime, %converge);								
        weaponShot(%fireOffsetX, %fireOffsetY, %fireOffsetZ);					
        weaponEnergy(%projectileID, %chargeLimit, %chargeRate);		
    }
}
                                       
function modloader::broadcastProjectile(%type,%projectileArgs)                                          
{        
    if(%type != bullet && %type != missile && %type != energy && %type != beam && %type != mine && %type != bomb)                                                      
    {        
        echo("modloader::broadcastProjectile([bullet|missile|energy|beam|mine|bomb], ['projectile args as one string']); //Modifies an existing projectile by using its ID or creates a new projectile by using an unused ID.");
        return;
    }
    %i=0;
    focusserver();
    while(%i < playerManager::getPlayerCount())                                                   
    {
		%p=-1;
        if(String::toLower(%type) == "bullet")
        {
            remoteEval(playerManager::getPlayerNum(%i), modloader::newBullet, %projectileArgs);
			
			String::Explode(%projectileArgs, ",", pa);
			newBullet(%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++]);
        }
        else if(String::toLower(%type) == "missile")
        {
            remoteEval(playerManager::getPlayerNum(%i), modloader::newMissile, %projectileArgs);
			
			String::Explode(%projectileArgs, ",", pa);
			newMissile(%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++]);
        }
        else if(String::toLower(%type) == "energy")
        {
            remoteEval(playerManager::getPlayerNum(%i), modloader::newEnergy, %projectileArgs);
			
			String::Explode(%projectileArgs, ",", pa);
			newEnergy(%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++]);
        }
        else if(String::toLower(%type) == "beam")
        {
            remoteEval(playerManager::getPlayerNum(%i), modloader::newBeam, %projectileArgs);
			
			String::Explode(%projectileArgs, ",", pa);
			newBeam(%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++]);
        }
        else if(String::toLower(%type) == "mine")
        {
			remoteEval(playerManager::getPlayerNum(%i), modloader::newMine, %projectileArgs);

			String::Explode(%projectileArgs, ",", pa);
			newMine(%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++]);
        }
        else if(String::toLower(%type) == "bomb")
        {
            remoteEval(playerManager::getPlayerNum(%i), modloader::newBomb, %projectileArgs);
			
			String::Explode(%projectileArgs, ",", pa);
			newBomb(%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++]);
        }               
		echo("Broadcasting projectile to " @ playerManager::getPlayerNum(%i));		
        %i++;            
    } 
    if (!$CmdLineServer)
    {
        focusclient();
    }
}     

function remotemodloader::newBullet(%cli,%projectileArgs)
{
    if(%cli == 2048)
    {
		%p=-1;
		String::Explode(%projectileArgs, ",", pa);
		newBullet(%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++]);
    }
}

function remotemodloader::newMissile(%cli,%projectileArgs)
{
    if(%cli == 2048)
    {
		%i=-1;
		String::Explode(%projectileArgs, ",", pa);
        newMissile(%pa[%i++],%pa[%i++],%pa[%i++],%pa[%i++],%pa[%i++],%pa[%i++],%pa[%i++],%pa[%i++],%pa[%i++],%pa[%i++],%pa[%i++],%pa[%i++],%pa[%i++],%pa[%i++],%pa[%i++],%pa[%i++],%pa[%i++],%pa[%i++],%pa[%i++],%pa[%i++],%pa[%i++],%pa[%i++],%pa[%i++],%pa[%i++],%pa[%i++],%pa[%i++],%pa[%i++],%pa[%i++],%pa[%i++],%pa[%i++],%pa[%i++],%pa[%i++],%pa[%i++]);
    }
}

function remotemodloader::newEnergy(%cli,%projectileArgs)
{
    if(%cli == 2048)
    {
		%p=-1;
		String::Explode(%projectileArgs, ",", pa);
		newEnergy(%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++]);
    }
}

function remotemodloader::newBeam(%cli,%projectileArgs)
{
    if(%cli == 2048)
    {
		%p=-1;
		String::Explode(%projectileArgs, ",", pa);
		newBeam(%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++]);
    }
}

function remotemodloader::newMine(%cli, %projectileArgs)
{
    if(%cli == 2048)
    {
		%p=-1;
		String::Explode(%projectileArgs, ",", pa);
		newMine(%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++]);
    }
}

function remotemodloader::newBomb(%cli,%projectileArgs)
{
    if(%cli == 2048)
    {
		%p=-1;
		String::Explode(%projectileArgs, ",", pa);
		newBomb(%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++],%pa[%p++]);
    }
}

##Deprecated
//Server send database data to client
// function remotemodloader::sendDatabaseData(%cli)
// {
    // if(%cli != 2048 && $pref::enforceModloader)
    // {
        // remoteEval(%cli, modloader::setClientVar, databaseSize, strlen(dataRetrieve(database,merged)));
        // modloader::sendDatabases(%cli);
    // }
// }

##Deprecated
//The primary uploader. Chunk sizes are 255 as anything slightly bigger won't get parsed by the remoteEval function on the receivers end
// function modloader::sendDatabases(%cli)
// {
    // if($pref::enforceModloader)
    // {
        // while(strlen(String::getSubStr(dataRetrieve(database,merged),%i,254)) != 0)
        // {
            // remoteEval(%cli,modloader::receieveDatabaseData, String::getSubStr(dataRetrieve(database,merged),%i,254));
            // %i+=254;
        // }
    // }
// }


function remotemodloader::getServerTerrain(%cli)
{
	//DISABLED FOR NOW
	return;
    if(%cli == 2048)
    {
        return;
    }
    
    if(focusserver() && isObject(8))
    {
        focusclient();
        //%gridFile = "multiplayer/" @ String::Replace(MOI::getObjectData(8,10,1), ".dtf", ".vol"); //NOT SUPPORTED ON HEADLESS SERVERS
        %terrainGridName = String::Explode(getTerrainGridFile(), "#", gridStringTrim);
        %fileName = $gridStringTrim[0] @ ".vol";
        %terrainFilePath = "multiplayer/" @ %fileName;
		deleteVariables("gridStringTrim*");
        focusserver();
        remoteEval(%cli, modloader::validateTerrain, %terrainFilePath, getSHA1(%terrainFilePath));
        if (!$CmdLineServer)
        {
            focusclient();
        }
    }
}
    
function modloader::UpdateWorldObjects()
{
    if($GUI::AllStaticsGhosted)
    {
        while(getNextObject(Ghostgroup,%nextObject) != 0)
        {
            %nextObject = getNextObject(Ghostgroup,%nextObject);
            if(String::FindSubStr(MOI::getObjectData(%nextObject,27), ".dts") > 0 || String::FindSubStr(MOI::getObjectData(%nextObject,24), ".dis") > 0)
            {
                %MOI = "M::OI_" @ randomInt(1,9999999);
                loadobject(%MOI,"modloader_OI.object");
                MissionObjectList::Inspect(0,%nextObject);
                MissionObjectList::Apply();
                deleteObject(%MOI);
            }
        }
    }
}

function Nova::playerNameToID(%playerName)
{
    %iter=-1;
    %playerListSize = playerManager::getPlayerCount();
    while(%iter++ <= %playerListSize)
    {
        if(%playerName == playerManager::getPlayer(%iter))
        {
            return playerManager::getPlayerNum(%iter);
        }
    }
    return 0;
}

function Nova::getPing()
{
    if(!$zzgetPingThrottle)
    {
        $client::pingStart = getSimTime();
        remoteeval(2048,"eval","ping");
        $zzgetPingThrottle = true;
        schedule("$zzgetPingThrottle='';",2);
    }
}

function remoteping()
{
    $client::ping = floor((getSimTime()-$client::pingStart)*1000);
}

// function genTerrain(%radius)
// {
    // if(isObject(terrain_grid))
    // {deleteObject(terrain_grid);}

    // if(%radius < 50)
    // {%radius = 50;}

    // %resolution = %radius * 0.0175;
    // %m=0;
    // %x_position=-(%radius/1);
    // %y_position=-(%radius/1);
    // newobject(terrain_grid, simgroup);
    // while(%y_position <= %radius)
    // {
        // #Place tiles along the X axis then increment to the next Y axis tile set
        // while(%x_position <= %radius)
        // {
            // #Get the Y axis rotation of the tile
            // %slope_x = (getTerrainHeight(%x_position,%y_position)-getTerrainHeight(%x_position-32,%y_position))*1.5;
            // #Get the X axis rotation of the tile
            // %slope_y = (getTerrainHeight(%x_position,%y_position)-getTerrainHeight(%x_position,%y_position-32))*1.5;
            
            // #Position the tile object in the center of the terrain tile
            // loadobject(gridTile @ %m++, terrain_marker);
            // setPosition(gridTile @ %m, %x_position-16, %y_position-16, getTerrainHeight(%x_position-16, %y_position-16), 0, %slope_y);
            // loadobject(gridTile @ %m++, terrain_marker);
            // setPosition(gridTile @ %m, %x_position-16, %y_position-16, getTerrainHeight(%x_position-16, %y_position-16), -90, %slope_x);
            // %x_position+=%resolution;
            // addtoSet(terrain_grid, gridTile @ %m,  gridTile @ %m-1);
        // }
        // #Move the y axis over to the next tile set
        // %y_position+=%resolution;
        // #Reset the x axis position to the first tile
        // %x_position=-(%radius/1);
    // }
// }

// function modloader::getTerrainTiles(%radius)
// {
    // #Clear the database
    // %m=0;
    // while(strlen(dataRetrieve(terrainTile @ %m++, location)) != 0)
    // {
        // dataRelease(terrainTile, location @ %m);
    // }
    
    // if(%radius < 3500)
    // {%radius = 3500;}

    // %resolution = %radius * 0.015;
    // %m=0;
    // %x_position=-(%radius/1);
    // %y_position=-(%radius/1);
    // while(%y_position <= %radius)
    // {
        // while(%x_position <= %radius)
        // {
            // #Get the Y axis slope of the tile
            // %slope_x = (getTerrainHeight(%x_position,%y_position)-getTerrainHeight(%x_position-32,%y_position))*1.5;
            // #Get the X axis slope of the tile
            // %slope_y = (getTerrainHeight(%x_position,%y_position)-getTerrainHeight(%x_position,%y_position-32))*1.5;
            
            // #Create a database of the terrains tile locations and their slopes
            // dataStore(terrainTiles, location @ %m++, %x_position-16 @ "," @ %y_position-16 @ "," @ getTerrainHeight(%x_position-16, %y_position-16) @ "," @ 0 @ "," @ %slope_y );
            // dataStore(terrainTiles, location @ %m++, %x_position-16 @ "," @ %y_position-16 @ "," @ getTerrainHeight(%x_position-16, %y_position-16) @ "," @ -90 @ "," @ %slope_x );
            // %x_position+=%resolution;
        // }
        // #Move the y axis over to the next tile set
        // %y_position+=%resolution;
        // #Reset the x axis position to the first tile
        // %x_position=-(%radius/1);
    // }
// }

// function modloader::sendTerrainTiles(%cli)
// {
    // if(%cli != 2048)
    // {
        // %m=0;
        // while(strlen(dataRetrieve(terrainTile, location @ %m++)) != 0)
        // {
            // remoteEval(%cli, modloader::receieveTerrainData, %m, dataRetrieve(terrainTile, location @ %m));
        // }
    // }
// }

// function remotemodloader::receieveTerrainData(%cli, %index, %postion_AND_slope)
// {
    // if(%cli == 2048)
    // {
        // if(!isObject(terrain_grid))
        // {newobject(terrain_grid, simgroup);}
        // StringM::explode(%postion_AND_slope, ",", loc);
        // #If the tile marker already exists update its position
        // if(isObject("terrain_grid\\" @ gridTile @ %index))
        // {
            // setPosition("terrain_grid\\" @ gridTile @ %index, $loc[0],$loc[1],$loc[2],$loc[3],$loc[4]);
        // }
        
        // #Else create a new one
        // else
        // {
            // loadobject(gridTile @ %index, terrain_marker);
            // setPosition(gridTile @ %index, $loc[0],$loc[1],$loc[2],$loc[3],$loc[4]);
            // addtoSet(terrain_grid, gridTile @ %index);
        // }
    // }
// }

// function waitroomGUI::onOpen::modLoaderFunc()
// {
    // schedule("modloader::checkForTerrain();", 5);
// }

// function modloader::checkForTerrain()
// {
    // if(isObject(waitRoomGUI))
    // {
        // if(getTerrainHeight(5000,5000) == -999 && getTerrainHeight(5000,-5000) == -999 && getTerrainHeight(-5000,5000) == -999 && getTerrainHeight(-5000,-5000) == -999)
        // {
            // loadobject(missingTerrain_notify, "ml_text_missingTerrain.object");
            // addtoset(waitroomGUI, missingTerrain_notify);
            // localmessagebox("You don't have the custom terrain the server is currently running. Check the servers game info tab for a possible download site.");
            // control::setText(IDSTR_LOADING, "");
        // }
    // }
// }


//Function to merge each database type into 1 master database
// function modloader::mergeDatabases()
// {
    // if(!$mergedDatabases && focusserver())
    // {
        // $mergedDatabases = true;
        // while(strlen($projectileDatChunk[%p++]))
        // {
            // dataStore(database, merged, dataRetrieve(database, merged) @ String::Replace($projectileDatChunk[%p], ")\n", ");\n"));
        // }
        // while(strlen($weaponDatChunk[%w++]))
        // {
            // dataStore(database, merged, dataRetrieve(database, merged) @ String::Replace($weaponDatChunk[%w], ")\n", ");\n"));
        // }
        // while(strlen($sensorDatChunk[%s++]))
        // {
            // dataStore(database, merged, dataRetrieve(database, merged) @ String::Replace($sensorDatChunk[%s], ")\n", ");\n"));
        // }
        // while(strlen($reactorDatChunk[%r++]))
        // {
            // dataStore(database, merged, dataRetrieve(database, merged) @ String::Replace($reactorDatChunk[%r], ")\n", ");\n"));
        // }
        // while(strlen($shieldDatChunk[%sh++]))
        // {
            // dataStore(database, merged, dataRetrieve(database, merged) @ String::Replace($shieldDatChunk[%sh], ")\n", ");\n"));
        // }
        // while(strlen($engineDatChunk[%e++]))
        // {
            // dataStore(database, merged, dataRetrieve(database, merged) @ String::Replace($engineDatChunk[%e], ")\n", ");\n"));
        // }
        // while(strlen($internalDatChunk[%in++]))
        // {
            // dataStore(database, merged, dataRetrieve(database, merged) @ String::Replace($internalDatChunk[%in], ")\n", ");\n"));
        // }
        // while(strlen($armorDatChunk[%a++]))
        // {
            // dataStore(database, merged, dataRetrieve(database, merged) @ String::Replace($armorDatChunk[%a], ")\n", ");\n"));
        // }
        // while(strlen($simDatChunk[%si++]))
        // {
            // dataStore(database, merged, dataRetrieve(database, merged) @ String::Replace($simDatChunk[%si], ")\n", ");\n"));
        // }
        // focusclient();
    // }
// }
//Strips directories leaves the file name
function stripPath(%path)
{
    if(!strlen(%path))
    {
        echo("stripPath(filePath); //Strips directory names and returns the filename");
        return;
    }
    
    if(String::findSubStr(%path, "/") > 0)
    {
        deleteVariables("filepath_");
        String::Explode(%path, "/", filepath_);
        %i=0;
        while(strlen($filepath_[%i])){%i++;}
        %path = $filepath_[%i-1];
    }
    else if(String::findSubStr(%path, "\\") > 0)
    {
        deleteVariables("filepath_");
        String::Explode(%path, "\\", filepath_);
        %i=0;
        while(strlen($filepath_[%i])){%i++;}
        %path = $filepath_[%i-1];
    }
    return %path;
}

function isFileWriteProtected(%file)
{
    if($WriteProtect[%file] == true)
    {
        return true;
    }
    return false;
}

function WriteProtect(%file)
{
    if(!strlen(%file))
    {
        echo("WriteProtect( fileName ); //Prevents a file from being overwritten by server download data");
    }
    while(strlen($WriteProtect[%wp++]))
    {
        if($WriteProtect[%wp] == %file)
        {
            echo(%file @ " is already download write protected. Use WriteProtections(); to see a list of current write protected files.");
            return false;
        }
    }
    eval("$WriteProtect[" @ %file @ "] = \"true\";");
    return true;
}

function WriteProtections()
{
    echo("-----------------------------");
    echo("-----Default Protections-----");
    echo("-\/-\/-\/-\/-\/-\/-\/-\/-\/-");
    export("WriteProtect*");
    echo("-/\-/\-/\-/\-/\-/\-/\-/\-/\-");
    echo("-----Default Protections-----");
    echo("-----------------------------");
}
%i=0;

//Write protect the default vehicle skins
$WriteProtect["apoc_cs01.bmp"] = true;
$WriteProtect["apoc_cs02.bmp"] = true;
$WriteProtect["apoc_cs04.bmp"] = true;
$WriteProtect["apoc_cs05.bmp"] = true;
$WriteProtect["apoc_desert.bmp"] = true;
$WriteProtect["apoc_dust.bmp"] = true;
$WriteProtect["apoc_ha.bmp"] = true;
$WriteProtect["apoc_ice.bmp"] = true;
$WriteProtect["APOC_kn.bmp"] = true;
$WriteProtect["apoc_lava.bmp"] = true;
$WriteProtect["Apoc_pirate.bmp"] = true;
$WriteProtect["apoc_snow.bmp"] = true;
$WriteProtect["apoc_team_gerald.bmp"] = true;
$WriteProtect["apoc_team_helen1.bmp"] = true;
$WriteProtect["apoc_team_helen2.bmp"] = true;
$WriteProtect["apoc_team_joe1.bmp"] = true;
$WriteProtect["apoc_team_joe2.bmp"] = true;
$WriteProtect["apoc_team_mike1.bmp"] = true;
$WriteProtect["apoc_team_mike2.bmp"] = true;
$WriteProtect["apoc_team_mike3.bmp"] = true;
$WriteProtect["apoc_team_robert.bmp"] = true;
$WriteProtect["apoc_titan.bmp"] = true;
$WriteProtect["APOC_tr.BMP"] = true;
$WriteProtect["aven_cs01.bmp"] = true;
$WriteProtect["aven_cs05.bmp"] = true;
$WriteProtect["aven_cs07.bmp"] = true;
$WriteProtect["aven_rb.bmp"] = true;
$WriteProtect["basl_ca.bmp"] = true;
$WriteProtect["basl_cs01.bmp"] = true;
$WriteProtect["basl_cs04.bmp"] = true;
$WriteProtect["basl_cs05..bmp"] = true;
$WriteProtect["basl_desert.bmp"] = true;
$WriteProtect["basl_dust.bmp"] = true;
$WriteProtect["basl_ice.bmp"] = true;
$WriteProtect["basl_kn.bmp"] = true;
$WriteProtect["basl_lava.bmp"] = true;
$WriteProtect["basl_snow.bmp"] = true;
$WriteProtect["basl_titan.bmp"] = true;
$WriteProtect["basl_tr.bmp"] = true;
$WriteProtect["bolo_cy.bmp"] = true;
$WriteProtect["bolo_grass.bmp"] = true;
$WriteProtect["bolo_rock.bmp"] = true;
$WriteProtect["bolo_sand.bmp"] = true;
$WriteProtect["bolo_snowrock.bmp"] = true;
$WriteProtect["disr_desert.bmp"] = true;
$WriteProtect["disr_dust.bmp"] = true;
$WriteProtect["disr_ice.bmp"] = true;
$WriteProtect["DISR_kn.BMP"] = true;
$WriteProtect["disr_lava.bmp"] = true;
$WriteProtect["disr_snow.bmp"] = true;
$WriteProtect["disr_titan.bmp"] = true;
$WriteProtect["DISR_tr.BMP"] = true;
$WriteProtect["drea_cs01.bmp"] = true;
$WriteProtect["drea_cs04.bmp"] = true;
$WriteProtect["drea_cs05.bmp"] = true;
$WriteProtect["drea_cs07.bmp"] = true;
$WriteProtect["DREA_pirate.bmp"] = true;
$WriteProtect["DREA_rb.BMP"] = true;
$WriteProtect["eman_cow.bmp"] = true;
$WriteProtect["eman_cs01.bmp"] = true;
$WriteProtect["eman_cs02.bmp"] = true;
$WriteProtect["eman_cs04.bmp"] = true;
$WriteProtect["eman_cs05.bmp"] = true;
$WriteProtect["eman_gmans.bmp"] = true;
$WriteProtect["EMAN_pirate.BMP"] = true;
$WriteProtect["EMAN_rb.BMP"] = true;
$WriteProtect["exec_a_sect.bmp"] = true;
$WriteProtect["EXEC_cy.BMP"] = true;
$WriteProtect["exec_mg.bmp"] = true;
$WriteProtect["EXEC_pl.BMP"] = true;
$WriteProtect["exec_turt.bmp"] = true;
$WriteProtect["goad_a_sect.bmp"] = true;
$WriteProtect["GOAD_cy.BMP"] = true;
$WriteProtect["goad_cy_ajh1.bmp"] = true;
$WriteProtect["goad_cy_moodring.bmp"] = true;
$WriteProtect["goad_flaming.bmp"] = true;
$WriteProtect["GOAD_MG.bmp"] = true;
$WriteProtect["goad_plague.bmp"] = true;
$WriteProtect["gorg_desert.bmp"] = true;
$WriteProtect["gorg_dust.bmp"] = true;
$WriteProtect["gorg_ice.bmp"] = true;
$WriteProtect["gorg_kn.bmp"] = true;
$WriteProtect["gorg_lava.bmp"] = true;
$WriteProtect["gorg_snow.bmp"] = true;
$WriteProtect["gorg_titan.bmp"] = true;
$WriteProtect["gorg_tr.bmp"] = true;
$WriteProtect["judg_a_sect.bmp"] = true;
$WriteProtect["JUDG_cy.BMP"] = true;
$WriteProtect["JUDG_MG.bmp"] = true;
$WriteProtect["JUDG_pl.BMP"] = true;
$WriteProtect["mino_desert.bmp"] = true;
$WriteProtect["mino_dust.bmp"] = true;
$WriteProtect["mino_ice.bmp"] = true;
$WriteProtect["mino_kn.bmp"] = true;
$WriteProtect["mino_lava.bmp"] = true;
$WriteProtect["mino_snow.bmp"] = true;
$WriteProtect["mino_titan.bmp"] = true;
$WriteProtect["mino_tr.bmp"] = true;
$WriteProtect["myrm_cs01.bmp"] = true;
$WriteProtect["myrm_cs05.bmp"] = true;
$WriteProtect["myrm_desert.bmp"] = true;
$WriteProtect["myrm_dust.bmp"] = true;
$WriteProtect["myrm_ice.bmp"] = true;
$WriteProtect["myrm_kn.bmp"] = true;
$WriteProtect["myrm_lava.bmp"] = true;
$WriteProtect["myrm_snow.bmp"] = true;
$WriteProtect["myrm_titan.bmp"] = true;
$WriteProtect["myrm_tr.bmp"] = true;
$WriteProtect["olyp_cow.bmp"] = true;
$WriteProtect["olyp_cs01.bmp"] = true;
$WriteProtect["olyp_cs04.bmp"] = true;
$WriteProtect["olyp_cs05.bmp"] = true;
$WriteProtect["olyp_cs07.bmp"] = true;
$WriteProtect["OLYP_rb.BMP"] = true;
$WriteProtect["olyp_rino.bmp"] = true;
$WriteProtect["pala_cs01.bmp"] = true;
$WriteProtect["pala_cs05.bmp"] = true;
$WriteProtect["pala_cs07.bmp"] = true;
$WriteProtect["pala_desert.bmp"] = true;
$WriteProtect["pala_dust.bmp"] = true;
$WriteProtect["pala_ice.bmp"] = true;
$WriteProtect["pala_kn.bmp"] = true;
$WriteProtect["pala_lava.bmp"] = true;
$WriteProtect["pala_snow.bmp"] = true;
$WriteProtect["pala_titan.bmp"] = true;
$WriteProtect["pala_tr.bmp"] = true;
$WriteProtect["pred_airforce.bmp"] = true;
$WriteProtect["PRED_ha.bmp"] = true;
$WriteProtect["recl_cy.bmp"] = true;
$WriteProtect["recl_grass.bmp"] = true;
$WriteProtect["recl_rock.bmp"] = true;
$WriteProtect["recl_sand.bmp"] = true;
$WriteProtect["recl_snow.bmp"] = true;
$WriteProtect["seek_a_sect.bmp"] = true;
$WriteProtect["seek_cy.bmp"] = true;
$WriteProtect["seek_mg.bmp"] = true;
$WriteProtect["shep_a_sect.bmp"] = true;
$WriteProtect["shep_cy.bmp"] = true;
$WriteProtect["shep_mg.bmp"] = true;
$WriteProtect["talo_desert.bmp"] = true;
$WriteProtect["talo_dust.bmp"] = true;
$WriteProtect["talo_ice.bmp"] = true;
$WriteProtect["talo_kn.bmp"] = true;
$WriteProtect["talo_lava.bmp"] = true;
$WriteProtect["talo_snow.bmp"] = true;
$WriteProtect["talo_titan.bmp"] = true;
$WriteProtect["talo_tr.bmp"] = true;
$WriteProtect["Nova_config.cs"] = true;

//Write protect the default mission terrains
$WriteProtect["ctf_balance_of_power.ted.vol"] = true;
$WriteProtect["ctf_circular_logic.ted.vol"] = true;
$WriteProtect["ctf_city_on_the_edge.ted.vol"] = true; 
$WriteProtect["ctf_stab_n_grab.ted.vol"] = true;     
$WriteProtect["ctf_titanic_assault.ted.vol"] = true;
$WriteProtect["ctf_winter_wasteland.ted.vol"] = true;  
$WriteProtect["dm_avalanche.ted.vol"] = true;         
$WriteProtect["dm_bloody_brunch.ted.vol"] = true;    
$WriteProtect["dm_city_on_the_edge.ted.vol"] = true; 
$WriteProtect["dm_cold_titan_night.ted.vol"] = true;
$WriteProtect["dm_fear_in_isolation.ted.vol"] = true; 
$WriteProtect["dm_heavens_peak.ted.vol"] = true;      
$WriteProtect["dm_impact.ted.vol"] = true;            
$WriteProtect["dm_lunacy.ted.vol"] = true;            
$WriteProtect["dm_mercury_rising.ted.vol"] = true;    
$WriteProtect["dm_moonstrike.ted.vol"] = true;         
$WriteProtect["dm_requiem_for_gen_lanz.ted.vol"] = true;
$WriteProtect["dm_sacrifice_to_bast.ted.vol"] = true; 
$WriteProtect["dm_state_of_confusion.ted.vol"] = true; 
$WriteProtect["dm_terran_conquest.ted.vol"] = true;   
$WriteProtect["dm_the_guardian.ted.vol"] = true;      
$WriteProtect["dm_twin_siege.ted.vol"] = true;         
$WriteProtect["starsiege_football.ted.vol"] = true;   
$WriteProtect["war_martian_standoff.ted.vol"] = true; 
schedule("modloader::patchEvents();",0);

//Server functions
function Nova::checkVehicleValid(%id)
{
	if(playerManager::vehicleIdToPlayerNum(%id) != 0) //Only check player vehicles
	{
		//Check for multiple anti-grav
		%cCount = getComponentCount(%id);
		while(%iter <= %cCount)
		{
			if(getComponentID(%id,%iter++) == 910)
			{
				%aGrav++;
			}
			if(%aGrav > 1)
			{
				schedule("deleteObject(" @ %id @ ");",0.05); //Don't delete it too fast else Starsiege explodes
				MessageBox(playerManager::vehicleIdToPlayerNum(%id), "Invalid Vehicle (Multiple anti-grav mounts)");
				return;
			}
		}
		
		String::Explode(Nova::getVehicleMass(%id), "/", m);
		if($m[0] <= 0)
		{
			schedule("deleteObject(" @ %id @ ");",0.05);
			MessageBox(playerManager::vehicleIdToPlayerNum(%id), "Invalid Vehicle (Negative mass)");
			return;
		}
		else if($m[0]-0.05 > $m[1]) //Account for floating point errors
		{
			schedule("deleteObject(" @ %id @ ");",0.05);
			MessageBox(playerManager::vehicleIdToPlayerNum(%id), "Invalid Vehicle (Total mass exceeds max mass)");
			return;
		}
	}
}