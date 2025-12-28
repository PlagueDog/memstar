//$GUI::AllStaticsGhosted - Internal boolean that changes states when a map has been completely ghosted

$modloader::pref::handshakeTimeout = 1;

function modloader::enforceModloader(%playerID)
{
    if(($pref::enforceModloader || $server::enforceNova) && getConnection(%playerID) == LOOPBACK)
    {        
		if ($cargv1 != "-s")
		{
			Nova::startFileServer($server::UDPPortNumber);
		}
	}
	
    focusserver();
    if(($pref::enforceModloader || $server::enforceNova) && getConnection(%playerID) != LOOPBACK)
    {       
		renameObject(%playerID, %playerID @ "_Net::PacketStream");
		
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
		$clientToken[%playerID] = Nova::randomStr(16);
		$modloaderHash[%playerID] = $clientToken[%playerID];
        echo("NOVA: " @ %playerName @ " [" @ %playerID @ "] connected. Performing Nova Authentication. [" @ $modloaderHash[%playerID] @ "]");
		
		%numMods=0;
		while(strlen($modloader::mod[%numMods++,fileName])){}
		
        remoteEval(%playerID, modloader::clientHandshake, $modloaderHash[%playerID], %numMods);
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
            echo("Nova: " @ %playerName @ " [" @ %playerID @ "] FAILED Nova Authentication. [" @ $modloaderHash[%playerID] @ "]");
            messageBox(%playerID, "--- [Nova] ---\n Authentication Failed\n\nNova is required in order to play on this server.");
			deleteObject(%playerID @ "_Net::PacketStream/GhostManager"); //Remove the players ghostmanager to prevent them from crashing
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


//Send our projectile data to the player
function Nova::sendProjectileData(%playerID)
{
	//Projectile ID cap is 127
	for(%i=0;%i<=127;%i++)
	{
		if(strlen($Net::bulletProjectileData[%i]))	{remoteEval(%playerID, modloader::newBullet, $Net::bulletProjectileData[%i],	$Net::bulletProjectileData[%i, impactTags]);}
		if(strlen($Net::missileProjectileData[%i]))	{remoteEval(%playerID, modloader::newMissile,$Net::missileProjectileData[%i],	$Net::missileProjectileData[%i, impactTags]);}
		if(strlen($Net::energyProjectileData[%i]))	{remoteEval(%playerID, modloader::newEnergy, $Net::energyProjectileData[%i],	$Net::energyProjectileData[%i, impactTags]);}
		if(strlen($Net::beamProjectileData[%i]))	{remoteEval(%playerID, modloader::newBeam, 	 $Net::beamProjectileData[%i],		$Net::beamProjectileData[%i, impactTags]);}
		if(strlen($Net::mineProjectileData[%i]))	{remoteEval(%playerID, modloader::newMine, 	 $Net::mineProjectileData[%i],		$Net::mineProjectileData[%i, impactTags]);}
		if(strlen($Net::bombProjectileData[%i]))	{remoteEval(%playerID, modloader::newBomb, 	 $Net::bombProjectileData[%i],		$Net::bombProjectileData[%i, impactTags]);}
	}
}

function Nova::sendWeaponData(%playerID)
{
	for(%i=0;%i<=255;%i++)
	{
		if(strlen($Net::weapon[%i]))
		{
			Net::newWeapon(%playerID, $Net::weapon[%i], $Net::weapon[%i, soundTagString], $Net::weapon[%i, descriptionTagString]);
			Net::weaponInfo1(%playerID, $Net::weapon[%i, weaponInfo1], $Net::weapon[%i, shortNameTagString], $Net::weapon[%i, longNameTagString]); //Include additional tag strings
			Net::weaponInfo2(%playerID, $Net::weapon[%i, weaponInfo2]);
			Net::weaponMuzzle(%playerID, $Net::weapon[%i, weaponMuzzle]);
			Net::weaponGeneral(%playerID, $Net::weapon[%i, weaponGeneral]);
			for(%ii=0;%ii>=32;%ii++)
			{
				if(strlen($Net::weapon[%i, weaponShot, %ii]))
				{
					Net::weaponShot(%playerID, $Net::weapon[%i, weaponShot, %ii]);
				}
			}
			if(strlen($Net::weapon[%i, weaponEnergy]))
			{
				Net::weaponEnergy(%playerID, $Net::weapon[%i, weaponEnergy]);
			}
			else if(strlen($Net::weapon[%i, weaponAmmo]))
			{
				Net::weaponAmmo(%playerID, $Net::weapon[%i, weaponAmmo]);
			}
		}
	}
}

function Nova::sendMountData(%playerId)
{
	for(%i=0;%i<=10000;%i++)
	{
		//TURRETS (structures)
		if(strlen($Net::turret[%i]))
		{
			Net::newTurret(%playerID, $Net::turret[%i]);
			Net::turretBase(%playerID, $Net::turret[%i, turretBase]);
			Net::turretAI(%playerID, $Net::turret[%i, turretAI]);
			for(%ii=0;%ii>=15;%ii++)
			{
				if(strlen($Net::turret[%i, newHardPoint, %ii]))
				{
					Net::newHardPoint(%playerID, $Net::turret[%i, newHardPoint, %ii]);
				}
			}
			continue;
		}
		
		//SENSORS
		else if(strlen($Net::sensor[%i, newSensor]))
		{
			Net::newSensor(%playerID, $Net::sensor[%i, newSensor]);
			Net::sensorMode(%playerID, $Net::sensor[%i, sensorModeActive]);
			Net::sensorMode(%playerID, $Net::sensor[%i, sensorModePassive]);
			Net::sensorInfo1(%playerID, $Net::sensor[%i, sensorInfo1], $Net::sensor[%i, shortNameTagString], $Net::sensor[%i, longNameTagString], $Net::sensor[%i, descriptionTagString]);
			Net::sensorInfo2(%playerID, $Net::sensor[%i, sensorInfo2]);
			continue;
		}
		
		//REACTORS
		else if(strlen($Net::reactor[%i, newReactor]))
		{
			Net::newReactor(%playerID, $Net::reactor[%i, newReactor]);
			Net::reactorInfo1(%playerID, $Net::reactor[%i, reactorInfo1], $Net::reactor[%i, shortNameTagString], $Net::reactor[%i, longNameTagString], $Net::reactor[%i, descriptionTagString]);
			Net::reactorInfo2(%playerID, $Net::reactor[%i, reactorInfo2]);
			continue;
		}
		
		//SHIELDS
		else if(strlen($Net::shield[%i, newshield]))
		{
			Net::newshield(%playerID, $Net::shield[%i, newshield]);
			Net::shieldInfo1(%playerID, $Net::shield[%i, shieldInfo1], $Net::shield[%i, shortNameTagString], $Net::shield[%i, longNameTagString], $Net::shield[%i, descriptionTagString]);
			Net::shieldInfo2(%playerID, $Net::shield[%i, shieldInfo2]);
			continue;
		}
		
		//ENGINES
		else if(strlen($Net::engine[%i, newEngine]))
		{
			Net::newEngine(%playerID, $Net::engine[%i, newEngine]);
			Net::engineInfo1(%playerID, $Net::engine[%i, engineInfo1], $Net::engine[%i, shortNameTagString], $Net::engine[%i, longNameTagString], $Net::engine[%i, descriptionTagString]);
			Net::engineInfo2(%playerID, $Net::engine[%i, engineInfo2]);
			continue;
		}
		
		//ARMORS
		else if(strlen($Net::armor[%i, newArmor]))
		{
			Net::newArmor(%playerID, $Net::armor[%i, newArmor]);
			Net::armorInfo1(%playerID, $Net::armor[%i, armorInfo1], $Net::armor[%i, shortNameTagString], $Net::armor[%i, longNameTagString], $Net::armor[%i, descriptionTagString]);
			Net::armorInfo2(%playerID, $Net::armor[%i, armorInfo2]);
			for(%ii=0;%ii>=127;%ii++)
			{
				if(strlen($Net::armor[%i, armorInfoSpecial, %ii]))
				{
					Net::armorInfoSpecial(%playerID, $Net::armor[%i, armorInfoSpecial, %ii]);
				}
			}
			continue;
		}
		
		//COMPUTERS
		else if(strlen($Net::computer[%i]))
		{
			Net::newComputer(%playerID, $Net::computer[%i]);
			Net::mountInfo1(%playerID, $Net::mountInfo1[%i, mountInfo1], $Net::mountInfo1[%i, shortNameTagString], $Net::mountInfo1[%i, longNameTagString], $Net::mountInfo1[%i, descriptionTagString]);
			Net::mountInfo2(%playerID, $Net::mountInfo2[%i, mountInfo2]);
			continue;
		}
		
		//ECMS
		else if(strlen($Net::ECM[%i]))
		{
			Net::newECM(%playerID, $Net::ECM[%i]);
			Net::mountInfo1(%playerID, $Net::mountInfo1[%i, mountInfo1], $Net::mountInfo1[%i, shortNameTagString], $Net::mountInfo1[%i, longNameTagString], $Net::mountInfo1[%i, descriptionTagString]);
			Net::mountInfo2(%playerID, $Net::mountInfo2[%i, mountInfo2]);
			continue;
		}
		
		//THERMALS
		else if(strlen($Net::thermal[%i]))
		{
			Net::newThermal(%playerID, $Net::thermal[%i]);
			Net::mountInfo1(%playerID, $Net::mountInfo1[%i, mountInfo1], $Net::mountInfo1[%i, shortNameTagString], $Net::mountInfo1[%i, longNameTagString], $Net::mountInfo1[%i, descriptionTagString]);
			Net::mountInfo2(%playerID, $Net::mountInfo2[%i, mountInfo2]);
			continue;
		}
		
		//CLOAKS
		else if(strlen($Net::cloak[%i]))
		{
			Net::newCloak(%playerID, $Net::cloak[%i]);
			Net::mountInfo1(%playerID, $Net::mountInfo1[%i, mountInfo1], $Net::mountInfo1[%i, shortNameTagString], $Net::mountInfo1[%i, longNameTagString], $Net::mountInfo1[%i, descriptionTagString]);
			Net::mountInfo2(%playerID, $Net::mountInfo2[%i, mountInfo2]);
			continue;
		}
		
		//MODULATORS
		else if(strlen($Net::modulator[%i]))
		{
			Net::newModulator(%playerID, $Net::modulator[%i]);
			Net::mountInfo1(%playerID, $Net::mountInfo1[%i, mountInfo1], $Net::mountInfo1[%i, shortNameTagString], $Net::mountInfo1[%i, longNameTagString], $Net::mountInfo1[%i, descriptionTagString]);
			Net::mountInfo2(%playerID, $Net::mountInfo2[%i, mountInfo2]);
			continue;
		}
		
		//CAPACITORS
		else if(strlen($Net::capacitor[%i]))
		{
			Net::newCapacitor(%playerID, $Net::capacitor[%i]);
			Net::mountInfo1(%playerID, $Net::mountInfo1[%i, mountInfo1], $Net::mountInfo1[%i, shortNameTagString], $Net::mountInfo1[%i, longNameTagString], $Net::mountInfo1[%i, descriptionTagString]);
			Net::mountInfo2(%playerID, $Net::mountInfo2[%i, mountInfo2]);
			continue;
		}
		
		//AMPLIFIERS
		else if(strlen($Net::amplifier[%i]))
		{
			Net::newAmplifier(%playerID, $Net::amplifier[%i]);
			Net::mountInfo1(%playerID, $Net::mountInfo1[%i, mountInfo1], $Net::mountInfo1[%i, shortNameTagString], $Net::mountInfo1[%i, longNameTagString], $Net::mountInfo1[%i, descriptionTagString]);
			Net::mountInfo2(%playerID, $Net::mountInfo2[%i, mountInfo2]);
			continue;
		}
		
		//LASER MODULES || FIELD STABILIZERS || LIFE SUPPORTS || AGRAV GENERATORS || ELECTROHULL || AMMO PACKS
		else if(strlen($Net::mountable[%i]))
		{
			Net::newMountable(%playerID, $Net::mountable[%i]);
			Net::mountInfo1(%playerID, $Net::mountInfo1[%i, mountInfo1], $Net::mountInfo1[%i, shortNameTagString], $Net::mountInfo1[%i, longNameTagString], $Net::mountInfo1[%i, descriptionTagString]);
			Net::mountInfo2(%playerID, $Net::mountInfo2[%i, mountInfo2]);
			continue;
		}

		// BOOSTERS
		else if(strlen($Net::booster[%i]))
		{
			Net::newBooster(%playerID, $Net::booster[%i]);
			Net::mountInfo1(%playerID, $Net::mountInfo1[%i, mountInfo1], $Net::mountInfo1[%i, shortNameTagString], $Net::mountInfo1[%i, longNameTagString], $Net::mountInfo1[%i, descriptionTagString]);
			Net::mountInfo2(%playerID, $Net::mountInfo2[%i, mountInfo2]);
			continue;
		}

		// REPAIRS
		else if(strlen($Net::repair[%i]))
		{
			Net::newRepair(%playerID, $Net::repair[%i]);
			Net::mountInfo1(%playerID, $Net::mountInfo1[%i, mountInfo1], $Net::mountInfo1[%i, shortNameTagString], $Net::mountInfo1[%i, longNameTagString], $Net::mountInfo1[%i, descriptionTagString]);
			Net::mountInfo2(%playerID, $Net::mountInfo2[%i, mountInfo2]);
			continue;
		}

		// BATTERIES
		else if(strlen($Net::battery[%i]))
		{
			Net::newBattery(%playerID, $Net::battery[%i]);
			Net::mountInfo1(%playerID, $Net::mountInfo1[%i, mountInfo1], $Net::mountInfo1[%i, shortNameTagString], $Net::mountInfo1[%i, longNameTagString], $Net::mountInfo1[%i, descriptionTagString]);
			Net::mountInfo2(%playerID, $Net::mountInfo2[%i, mountInfo2]);
			continue;
		}
	}
}

function Nova::sendVehicleData(%playerId)
{
	for(%i=0;%i<=255;%i++)
	{
		/////////////////////////////////////////////////////////////////////////////////////////
		//HERCS//////////////////////////////////////////////////////////////////////////////////
		/////////////////////////////////////////////////////////////////////////////////////////
		if(strlen($Net::herc[%i, HercBase]))
		{
			Net::newHerc(%playerID, %i);
			Net::hercBase(%playerID, $Net::herc[%i, hercBase], $Net::herc[%i, identityTagString]);
			Net::hercPos(%playerID, $Net::herc[%i, hercPos]);
			Net::hercRot(%playerID, $Net::herc[%i, hercRot]);
			Net::hercAnim(%playerID, $Net::herc[%i, hercAnim]);
			Net::hercCpit(%playerID, $Net::herc[%i, hercCpit]);
			Net::hercColl(%playerID, $Net::herc[%i, hercColl]);
			Net::hercAI(%playerID, $Net::herc[%i, hercAI]);
			if(strlen($Net::herc[%i, hercFootprint]))
			{
				Net::hercFootprint(%playerID, $Net::herc[%i, hercFootprint]);
			}
			//MAX WEAPONS IS 12!
			//Any higher and the vehicle hits a buffer overflow on its object data
			//HARDPOINTS
			for(%ii=0;%ii<=12;%ii++)
			{
				if(strlen($Net::herc[%i, newHardPoint, %ii]))
				{
					Net::newHardPoint(%playerID, $Net::herc[%i, newHardPoint, %ii]);
					if(strlen($Net::herc[%i, hardPointDamage, %ii]))
					{
						Net::hardPointDamage(%playerID, $Net::herc[%i, hardPointDamage, %ii]);
					}
					if(strlen($Net::herc[%i, hardPointSpecial, %ii]))
					{
						Net::hardPointSpecial(%playerID, $Net::herc[%i, hardPointSpecial, %ii]);
					}
				}
			}
			//MOUNT POINTS
			for(%ii=0;%ii<=12;%ii++)
			{
				if(strlen($Net::herc[%i, newMountPoint, %ii]))
				{
					Net::newMountPoint(%playerID, $Net::herc[%i, newMountPoint, %ii]);
				}
			}
			//COMPONENTS
			for(%ii=0;%ii<=20;%ii++)
			{
				if(strlen($Net::herc[%i, newComponent, %ii]))
				{
					Net::newComponent(%playerID, $Net::herc[%i, newComponent, %ii]);
				}
			}
			//CONFIGURATIONS
			for(%ii=0;%ii<=20;%ii++)
			{
				if(strlen($Net::herc[%i, newConfiguration, %arrayIndex]))
				{
					Net::newConfiguration(%playerID, $Net::herc[%i, newConfiguration, %ii]);
				}
			}
			//DEFAULT WEAPONS
			if(strlen($Net::herc[%i, defaultWeapons]))
			{
				Net::defaultWeapons(%playerID, $Net::herc[%i, defaultWeapons]);
			}
			//DEFAULT MOUNTS
			if(strlen($Net::herc[%i, defaultMounts]))
			{
				Net::defaultMountables(%playerID, $Net::herc[%i, defaultMounts]);
			}
			Net::clientVehicleStore(%playerID, %i, herc, $Net::herc[%i, identityTagString]);
		}
		
		/////////////////////////////////////////////////////////////////////////////////////////
		//TANKS//////////////////////////////////////////////////////////////////////////////////
		/////////////////////////////////////////////////////////////////////////////////////////
		if(strlen($Net::tank[%i, tankBase]))
		{
			Net::newtank(%playerID, %i);
			if(strlen($Net::tank[%i, vehiclePilotable]))
			{
				Net::vehiclePilotable(%playerID, $Net::tank[%i, vehiclePilotable]);
			}
			if(strlen($Net::tank[%i, vehicleArtillery]))
			{
				Net::vehicleArtillery(%playerID, $Net::tank[%i, vehicleArtillery]);
			}
			Net::tankBase(%playerID, $Net::tank[%i, tankBase], $Net::tank[%i, identityTagString]);
			Net::tankPos(%playerID, $Net::tank[%i, tankPos]);
			Net::tankRot(%playerID, $Net::tank[%i, tankRot]);
			Net::tankColl(%playerID, $Net::tank[%i, tankColl]);
			if(strlen($Net::tank[%i, tankAnim]))
			{
				Net::tankAnim(%playerID, $Net::tank[%i, tankAnim]);
			}
			if(strlen($Net::tank[%i, tankSound]))
			{
				Net::tankSound(%playerID, $Net::tank[%i, tankSound]);
			}
			if(strlen($Net::tank[%i, tankSlide]))
			{
				Net::tankSlide(%playerID, $Net::tank[%i, tankSlide]);
			}
			
			//MAX WEAPONS IS 12!
			//Any higher and the vehicle hits a buffer overflow on its object data
			//HARDPOINTS
			for(%ii=0;%ii<=12;%ii++)
			{
				if(strlen($Net::tank[%i, newHardPoint, %ii]))
				{
					Net::newHardPoint(%playerID, $Net::tank[%i, newHardPoint, %ii]);
					if(strlen($Net::tank[%i, hardPointDamage, %ii]))
					{
						Net::hardPointDamage(%playerID, $Net::tank[%i, hardPointDamage, %ii]);
					}
					if(strlen($Net::tank[%i, hardPointSpecial, %ii]))
					{
						Net::hardPointSpecial(%playerID, $Net::tank[%i, hardPointSpecial, %ii]);
					}
				}
			}
			//MOUNT POINTS
			for(%ii=0;%ii<=12;%ii++)
			{
				if(strlen($Net::tank[%i, newMountPoint, %ii]))
				{
					Net::newMountPoint(%playerID, $Net::tank[%i, newMountPoint, %ii]);
				}
			}
			//COMPONENTS
			for(%ii=0;%ii<=20;%ii++)
			{
				if(strlen($Net::tank[%i, newComponent, %ii]))
				{
					Net::newComponent(%playerID, $Net::tank[%i, newComponent, %ii]);
				}
			}
			//CONFIGURATIONS
			for(%ii=0;%ii<=20;%ii++)
			{
				if(strlen($Net::tank[%i, newConfiguration, %arrayIndex]))
				{
					Net::newConfiguration(%playerID, $Net::tank[%i, newConfiguration, %ii]);
				}
			}
			//DEFAULT WEAPONS
			if(strlen($Net::tank[%i, defaultWeapons]))
			{
				Net::defaultWeapons(%playerID, $Net::tank[%i, defaultWeapons]);
			}
			//DEFAULT MOUNTS
			if(strlen($Net::tank[%i, defaultMounts]))
			{
				Net::defaultMountables(%playerID, $Net::tank[%i, defaultMounts]);
			}

			if(strlen($Net::tank[%i, tankExplosion]))
			{
				Net::tankExplosion(%playerID, $Net::tank[%i, tankExplosion]);
			}
			Net::clientVehicleStore(%playerID, %i, tank, $Net::tank[%i, identityTagString]);
		}
		
		//////////////////////////////////////////////////////////////////////////////////////////
		//FLYERS//////////////////////////////////////////////////////////////////////////////////
		//////////////////////////////////////////////////////////////////////////////////////////
		if(strlen($Net::flyer[%i, flyerBase]))
		{
			Net::newflyer(%playerID, %i);
			Net::flyerBase(%playerID, $Net::flyer[%i, flyerBase], $Net::flyer[%i, identityTagString]);
			Net::flyerPos(%playerID, $Net::flyer[%i, flyerPos]);
			Net::flyerRot(%playerID, $Net::flyer[%i, flyerRot]);
			Net::flyerCpit(%playerID, $Net::flyer[%i, flyerCpit]);
			Net::flyerColl(%playerID, $Net::flyer[%i, flyerColl]);
			if(strlen($Net::flyer[%i, flyerExhaust]))
			{
				Net::flyerExhaust(%playerID, $Net::flyer[%i, flyerExhaust]);
				for(%ii=0;%ii<=12;%ii++)
				{
					if(strlen($Net::flyer[%i, flyerExhaustOffset, %ii]))
					{
						Net::flyerExhaustOffset(%playerID, $Net::flyer[%i, flyerExhaustOffset]);
					}
				}
			}
			Net::flyerAI(%playerID, $Net::flyer[%i, flyerAI]);
			Net::flyerNav(%playerID, $Net::flyer[%i, flyerNav]);
			Net::flyerSound(%playerID, $Net::flyer[%i, flyerSound]);
			//MAX WEAPONS IS 12!
			//Any higher and the vehicle hits a buffer overflow on its object data
			//HARDPOINTS
			for(%ii=0;%ii<=12;%ii++)
			{
				if(strlen($Net::flyer[%i, newHardPoint, %ii]))
				{
					Net::newHardPoint(%playerID, $Net::flyer[%i, newHardPoint, %ii]);
					if(strlen($Net::flyer[%i, hardPointDamage, %ii]))
					{
						Net::hardPointDamage(%playerID, $Net::flyer[%i, hardPointDamage, %ii]);
					}
					if(strlen($Net::flyer[%i, hardPointSpecial, %ii]))
					{
						Net::hardPointSpecial(%playerID, $Net::flyer[%i, hardPointSpecial, %ii]);
					}
				}
			}
			//MOUNT POINTS
			for(%ii=0;%ii<=12;%ii++)
			{
				if(strlen($Net::flyer[%i, newMountPoint, %ii]))
				{
					Net::newMountPoint(%playerID, $Net::flyer[%i, newMountPoint, %ii]);
				}
			}
			//COMPONENTS
			for(%ii=0;%ii<=20;%ii++)
			{
				if(strlen($Net::flyer[%i, newComponent, %ii]))
				{
					Net::newComponent(%playerID, $Net::flyer[%i, newComponent, %ii]);
				}
			}
			//CONFIGURATIONS
			for(%ii=0;%ii<=20;%ii++)
			{
				if(strlen($Net::flyer[%i, newConfiguration, %arrayIndex]))
				{
					Net::newConfiguration(%playerID, $Net::flyer[%i, newConfiguration, %ii]);
				}
			}
			//DEFAULT WEAPONS
			if(strlen($Net::flyer[%i, defaultWeapons]))
			{
				Net::defaultWeapons(%playerID, $Net::flyer[%i, defaultWeapons]);
			}
			//DEFAULT MOUNTS
			if(strlen($Net::flyer[%i, defaultMounts]))
			{
				Net::defaultMountables(%playerID, $Net::flyer[%i, defaultMounts]);
			}
			Net::clientVehicleStore(%playerID, %i, flyer, $Net::flyer[%i, identityTagString], true);
		}
		
		//////////////////////////////////////////////////////////////////////////////////////////
		//DRONES//////////////////////////////////////////////////////////////////////////////////
		//////////////////////////////////////////////////////////////////////////////////////////
		if(strlen($Net::drone[%i, droneBase]))
		{
			Net::newDrone(%playerID, %i);
			if(strlen($Net::drone[%i, vehiclePilotable]))
			{
				Net::vehiclePilotable(%playerID, $Net::drone[%i, vehiclePilotable]);
			}
			if(strlen($Net::drone[%i, vehicleArtillery]))
			{
				Net::vehicleArtillery(%playerID, $Net::drone[%i, vehicleArtillery]);
			}
			Net::droneBase(%playerID, $Net::drone[%i, droneBase], $Net::drone[%i, identityTagString]);
			Net::dronePos(%playerID, $Net::drone[%i, dronePos]);
			if(strlen($Net::drone[%i, droneRot]))
			{
				Net::droneRot(%playerID, $Net::drone[%i, droneRot]);
			}
			if(strlen($Net::drone[%i, droneColl]))
			{
				Net::droneColl(%playerID, $Net::drone[%i, droneColl]);
			}
			if(strlen($Net::drone[%i, droneAnim]))
			{
				Net::droneAnim(%playerID, $Net::drone[%i, droneAnim]);
			}
			if(strlen($Net::drone[%i, droneSound]))
			{
				Net::droneSound(%playerID, $Net::drone[%i, droneSound]);
			}
			if(strlen($Net::drone[%i, droneSlide]))
			{
				Net::droneSlide(%playerID, $Net::drone[%i, droneSlide]);
			}
			
			//MAX WEAPONS IS 12!
			//Any higher and the vehicle hits a buffer overflow on its object data
			//HARDPOINTS
			for(%ii=0;%ii<=12;%ii++)
			{
				if(strlen($Net::drone[%i, newHardPoint, %ii]))
				{
					Net::newHardPoint(%playerID, $Net::drone[%i, newHardPoint, %ii]);
					if(strlen($Net::drone[%i, hardPointDamage, %ii]))
					{
						Net::hardPointDamage(%playerID, $Net::drone[%i, hardPointDamage, %ii]);
					}
					if(strlen($Net::drone[%i, hardPointSpecial, %ii]))
					{
						Net::hardPointSpecial(%playerID, $Net::drone[%i, hardPointSpecial, %ii]);
					}
				}
			}
			//MOUNT POINTS
			for(%ii=0;%ii<=12;%ii++)
			{
				if(strlen($Net::drone[%i, newMountPoint, %ii]))
				{
					Net::newMountPoint(%playerID, $Net::drone[%i, newMountPoint, %ii]);
				}
			}
			//COMPONENTS
			for(%ii=0;%ii<=20;%ii++)
			{
				if(strlen($Net::drone[%i, newComponent, %ii]))
				{
					Net::newComponent(%playerID, $Net::drone[%i, newComponent, %ii]);
				}
			}
			//CONFIGURATIONS
			for(%ii=0;%ii<=20;%ii++)
			{
				if(strlen($Net::drone[%i, newConfiguration, %arrayIndex]))
				{
					Net::newConfiguration(%playerID, $Net::drone[%i, newConfiguration, %ii]);
				}
			}
			//DEFAULT WEAPONS
			if(strlen($Net::drone[%i, defaultWeapons]))
			{
				Net::defaultWeapons(%playerID, $Net::drone[%i, defaultWeapons]);
			}
			//DEFAULT MOUNTS
			if(strlen($Net::drone[%i, defaultMounts]))
			{
				Net::defaultMountables(%playerID, $Net::drone[%i, defaultMounts]);
			}

			if(strlen($Net::drone[%i, droneExplosion]))
			{
				Net::droneExplosion(%playerID, $Net::drone[%i, droneExplosion]);
			}
			Net::clientVehicleStore(%playerID, %i, tank, $Net::drone[%i, identityTagString], true);
		}
		if(%i == 255)
		{
			Net::enableVehicleTab(%playerID, true);
			schedule("Net::enableVehicleTab(" @ %playerID @ ", true);", 1);
		}
	}
}

function remoteNet::requestServerMod(%cli, %modIndex)
{
    if(%cli != 2048)
    {
		Nova::sendModData(%cli, %modIndex);
	}
}

function Nova::sendModData(%cli, %modIndex)
{
	if(focusserver())
	{
		if(strlen($modloader::mod[%modIndex,fileName]))
		{
			%fileName = "mods/" @ $modloader::mod[%modIndex,fileName];
			if((getFileSize(%fileName)/1024) <= $server::maxFileSize)
			{
				remoteEval(%cli, modloader::validateFile, %fileName, getSHA1(%fileName));
			}
		}
		focusclient();
	}
}

function remotemodloader::clientHandshake(%cli, %hash, %numMods)
{
    if(%cli == 2048)
    {
		$zzClientToken = %hash;
		%str = String::Replace($client::connectTo,"IP:","");
        %str = String::Replace(%str,":","_");
        %str = String::Replace(%str,".","_");
		createCacheDir(%str);
		createCacheDir(%str @ "\\vehicles");
		repath::append("mods\\cache\\" @ %str);
		
		//Nova::getServerTerrain();
		//Nova::getServerMods();
        eval("\x72\x65\x6d\x6f\x74\x65\x45\x76\x61\x6c\x28\x32\x30\x34\x38\x2c\x20\x6d\x6f\x64\x6c\x6f\x61"
        @ "\x64\x65\x72\x3a\x3a\x63\x6c\x69\x65\x6e\x74\x48\x61\x6e\x64\x73\x68\x61\x6b\x65\x5f\x43\x41\x4c\x4c"
        @ "\x42\x41\x43\x4b\x2c\x20\x25\x68\x61\x73\x68\x29\x3b");
        control::setText(IDSTR_LOADING, "[Nova] Authenticating...");
		$serverMods = %numMods;
		
		//playsound(0,"Dlink_engaged.wav", IDPRF_2D);
		control::setActive(IDSTR_TAB_VEHICLE_LAB, 0);
		$_zzRequiresReloads=true;
        deleteFunctions("HtmlOpe*");
        deleteFunctions("createSSMutex");

		$Net::serverCacheDirectory = "mods\\cache\\" @ %str @ "\\vehicles";
		setVehicleDir($Net::serverCacheDirectory);
		Nova::enableResetCacheButton();
		
		//$pref::packetRate = 4096;
		//$pref::packetSize = 1000;
    }
}

function Net::purgeVehicleFiles(%cli)
{
    if(%cli == 2048)                                                                   
    {        
		if(!$Gui::AllStaticsGhosted)
		{
			Nova::purgeVehicleFiles();
		}
	}
}

function Nova::enableResetCacheButton()
{
	if($_zzRequiresReloads)
	{
		if(isObject(waitroomGUI))
		{
			if(!isObject(reset_server_cache))
			{
				%playerInfoController = Nova::findGuiTagControl(waitroomGUI, IDPIC_CONTROLLER);
				loadObject(reset_server_cache, "reset_server_cache.object");
				control::setVisible(reset_server_cache,true);
				//control::setActive(reset_server_cache,true);
				addToSet(%playerInfoController,reset_server_cache);
			}
		}
	}
}

function Nova::resetServerCache()
{
	disconnect();
	deleteServerCache();
	%str = String::Replace($client::connectTo,"IP:","");
    %str = String::Replace(%str,":","_");
    %str = String::Replace(%str,".","_");
	createCacheDir(%str);
	createCacheDir(%str @ "\\vehicles");
	appendSearchPath();
	schedule("connect($client::connectTo);",0);
}

function remotemodloader::clientHandshake_CALLBACK(%cli, %hash)
{
    if(%cli != 2048)
    {
        if($modloaderHash[%cli] == %hash)
        {
            $modloaderHash[%cli] = "";
			
			//Send terrain
			if(isObject(8))
			{
				%terrainGridName = String::Explode(getTerrainGridFile(), "#", gridStringTrim);
				%fileName = $gridStringTrim[0] @ ".vol";
				%terrainFilePath = "multiplayer/" @ %fileName;
				deleteVariables("gridStringTrim*");
				remoteEval(%cli, Net::getServerFile, %terrainFilePath);
			}	
			
			//Send server mods
			while(strlen($modloader::mod[%i++,fileName]))
			{
				remoteEval(%cli, Net::getServerFile, ".\\mods\\" @ $modloader::mod[%i,fileName]);
			}
			
			if($server::sendWeaponData)
			{
				//Send projectile first
				//schedule("Nova::sendProjectileData(" @%cli @ ");",1);
				//schedule("Nova::sendWeaponData(" @ %cli @ ");",1);
				Nova::sendProjectileData(%cli);
				Nova::sendWeaponData(%cli);
			}
			
			if($server::sendMountData)
			{
				// schedule("Nova::sendMountData(" @ %cli @ ");", 1);
				Nova::sendMountData(%cli);
			}
			
			if($server::sendVehicleData)
			{
				Net::purgeVehicleFiles(%cli);
				// schedule("Nova::sendVehicleData(" @ %cli @ ");",1);
				Nova::sendVehicleData(%cli);
			}
        }
    }
}                                                                                                                               

function waitroomGUI::onOpen::modloaderFunc()
{	
    nameVehicleControllerObjects();
    ffevent(0,0,0,0);
	//schedule("if($pref::packetSize > 1000){$pref::packetSize = 1000;}",1);
	$Gui::OnGameExit = "disconnect();exitFromServerResets();";
	Nova::enableResetCacheButton();
	
	function ghostTickerTracker()
	{
		control::setActive(reset_server_cache,$Gui::AllStaticsGhosted);
		if(!$Gui::AllStaticsGhosted && isObject(waitroomGUI))
		{
			schedule("ghostTickerTracker();",0.1);
		}
	}
	
	ghostTickerTracker();
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
                                       
function modloader::broadcastProjectile(%id)                                          
{        
    if(!strlen(%id))                                                      
    {        
        echo("modloader::broadcastProjectile( projectileID );");
        return;
    }
    focusserver();
	%i=0;
    while(%i <= playerManager::getPlayerCount())                                                   
    {
        if($Net::projectile[%id, type] == "bullet")
        {
            remoteEval(playerManager::getPlayerNum(%i), modloader::newBullet, $Net::bulletProjectileData[%i],	$Net::bulletProjectileData[%i, impactTags]);
        }
        else if($Net::projectile[%id, type] == "missile")
        {
            remoteEval(playerManager::getPlayerNum(%i), modloader::newMissile, $Net::missileProjectileData[%i],	$Net::missileProjectileData[%i, impactTags]);
        }
        else if($Net::projectile[%id, type] == "energy")
        {
            remoteEval(playerManager::getPlayerNum(%i), modloader::newEnergy, $Net::energyProjectileData[%i],	$Net::energyProjectileData[%i, impactTags]);
        }
        else if($Net::projectile[%id, type] == "beam")
        {
            remoteEval(playerManager::getPlayerNum(%i), modloader::newBeam, $Net::beamProjectileData[%i],	$Net::beamProjectileData[%i, impactTags]);
        }
        else if($Net::projectile[%id, type] == "mine")
        {
            remoteEval(playerManager::getPlayerNum(%i), modloader::newMine, $Net::mineProjectileData[%i],	$Net::mineProjectileData[%i, impactTags]);
        }
        else if($Net::projectile[%id, type] == "bomb")
        {
            remoteEval(playerManager::getPlayerNum(%i), modloader::newBomb, $Net::bombProjectileData[%i],	$Net::bombProjectileData[%i, impactTags]);
        }               
		echo("Broadcasting projectile to " @ playerManager::getPlayerNum(%i));		
        %i++;            
    } 
    if (!$CmdLineServer)
    {
        focusclient();
    }
}     

function remotemodloader::newBullet(%cli,%projectileArgs, %impactTags)
{
    if(%cli == 2048)
    {
		%p=-1;
		%projectileArgs = String::replace(%projectileArgs, "'", "");
		String::Explode(%projectileArgs, ",", pa);
		%impactTags = String::replace(%impactTags, "'", "");
		String::Explode(%impactTags, ",", impactTag);
		Nova::handleProjectileTagStrings($pa[22], $impactTag[0], $pa[23], $impactTag[1], $pa[24], $impactTag[2]); 
		Nova::newBullet($pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++]);
		if($echoDownloads)
		{
			%p=-1;
			echo("Recieved projectile data [BULLET] - ", $pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]);
		}
		deleteVariables("pa*");
		deleteVariables("impactTag*");
    }
}

function remotemodloader::newMissile(%cli,%projectileArgs, %impactTags)
{
    if(%cli == 2048)
    {
		%i=-1;
		%projectileArgs = String::replace(%projectileArgs, "'", "");
		String::Explode(%projectileArgs, ",", pa);
		%impactTags = String::replace(%impactTags, "'", "");
		String::Explode(%impactTags, ",", impactTag);
		Nova::handleProjectileTagStrings($pa[34], $impactTag[0], $pa[35], $impactTag[1]); 
        Nova::newMissile($pa[%i++],$pa[%i++],$pa[%i++],$pa[%i++],$pa[%i++],$pa[%i++],$pa[%i++],$pa[%i++],$pa[%i++],$pa[%i++],$pa[%i++],$pa[%i++],$pa[%i++],$pa[%i++],$pa[%i++],$pa[%i++],$pa[%i++],$pa[%i++],$pa[%i++],$pa[%i++],$pa[%i++],$pa[%i++],$pa[%i++],$pa[%i++],$pa[%i++],$pa[%i++],$pa[%i++],$pa[%i++],$pa[%i++],$pa[%i++],$pa[%i++],$pa[%i++],$pa[%i++],$pa[%i++],$pa[%i++]);
		if($echoDownloads)
		{
			%i=-1;
			echo("Recieved projectile data [MISSILE] - ", $pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]);
		}
		deleteVariables("pa*");
		deleteVariables("impactTag*");
    }
}

function remotemodloader::newEnergy(%cli,%projectileArgs, %impactTags)
{
    if(%cli == 2048)
    {
		%p=-1;
		%projectileArgs = String::replace(%projectileArgs, "'", "");
		String::Explode(%projectileArgs, ",", pa);
		%impactTags = String::replace(%impactTags, "'", "");
		String::Explode(%impactTags, ",", impactTag);
		Nova::handleProjectileTagStrings($pa[22], $impactTag[0], $pa[23], $impactTag[1], $pa[24], $impactTag[2]); 
		Nova::newEnergy($pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++]);
		if($echoDownloads)
		{
			%i=-1;
			echo("Recieved projectile data [ENERGY] - ", $pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]);
		}
		deleteVariables("pa*");
		deleteVariables("impactTag*");
    }
}

function remotemodloader::newBeam(%cli,%projectileArgs, %impactTags)
{
    if(%cli == 2048)
    {
		%p=-1;
		%projectileArgs = String::replace(%projectileArgs, "'", "");
		String::Explode(%projectileArgs, ",", pa);
		%impactTags = String::replace(%impactTags, "'", "");
		String::Explode(%impactTags, ",", impactTag);
		Nova::handleProjectileTagStrings($pa[31], $impactTag[0], $pa[32], $impactTag[1], $pa[33], $impactTag[2]); 
		Nova::newBeam($pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++]);
		if($echoDownloads)
		{
			%i=-1;
			echo("Recieved projectile data [BEAM] - ", $pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]);
		}
		deleteVariables("pa*");
		deleteVariables("impactTag*");
    }
}

function remotemodloader::newMine(%cli, %projectileArgs, %impactTags)
{
    if(%cli == 2048)
    {
		%p=-1;
		%projectileArgs = String::replace(%projectileArgs, "'", "");
		String::Explode(%projectileArgs, ",", pa);
		%impactTags = String::replace(%impactTags, "'", "");
		String::Explode(%impactTags, ",", impactTag);
		Nova::handleProjectileTagStrings($pa[24], $impactTag[0], $pa[25], $impactTag[1]); 
		Nova::newMine($pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++]);
		if($echoDownloads)
		{
			%i=-1;
			echo("Recieved projectile data [MINE] - ", $pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]);
		}
		deleteVariables("pa*");
		deleteVariables("impactTag*");
    }
}

function remotemodloader::newBomb(%cli,%projectileArgs, %impactTags)
{
    if(%cli == 2048)
    {
		%p=-1;
		%projectileArgs = String::replace(%projectileArgs, "'", "");
		String::Explode(%projectileArgs, ",", pa);
		%impactTags = String::replace(%impactTags, "'", "");
		String::Explode(%impactTags, ",", impactTag);
		Nova::handleProjectileTagStrings($pa[18], $impactTag[0], $pa[19], $impactTag[1]); 
		Nova::newBomb($pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++],$pa[%p++]);
		if($echoDownloads)
		{
			%i=-1;
			echo("Recieved projectile data [BOMB] - ", $pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]," ",$pa[%i++]);
		}
		deleteVariables("pa*");
		deleteVariables("impactTag*");
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

//Strips directories and leaves the file name
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
	deleteVariables("filePath*");
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

function Net::enableVehicleTab(%cli, %bool)
{
	remoteEval(%cli, Nova::enableVehicleTab, %bool);
}

function remoteNova::enableVehicleTab(%cli, %bool)
{
	if(%cli == 2048)
	{
		if(isObject(waitroomGUI))
		{
			control::setActive(IDSTR_TAB_VEHICLE_LAB, %bool);	
			control::setActive(reset_server_cache,%bool);
		}
	}
}

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



//WEAPON FUNCTIONS
function Net::newWeapon(%cli, %netArgs, %soundTagString, %descriptionTagString)
{
	remoteEval(%cli, newWeapon, %netArgs, %soundTagString, %descriptionTagString);
}
function remotenewWeapon(%cli, %netArgs, %soundTagString, %descriptionTagString)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::handleNetDescriptionTagString($arg[6], %descriptionTagString);
	Nova::handleNetSoundTagString($arg[3], %soundTagString);
	Nova::newWeapon($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved weapon data [newWeapon] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
	if(isObject(waitroomGUI))
	{
        control::setText(IDSTR_LOADING,  *IDSTR_NOVA_NET_WEAPON_DATA);
	}
}}

function Net::weaponInfo1(%cli, %netArgs, %shortTagString, %longTagString)
{
	remoteEval(%cli, weaponInfo1, %netArgs, %shortTagString, %longTagString);
}

function remoteweaponInfo1(%cli, %netArgs, %shortTagString, %longTagString)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::handleNetNameTagStrings($arg[0], %shortTagString, $arg[1], %longTagString);
	Nova::weaponInfo1($arg[0],$arg[1],$arg[2],$arg[3],$arg[4],$arg[5]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved weapon data [weaponInfo1] - ", *$arg[0]," ",*$arg[1]," ",$arg[2]," ",$arg[3]," ",$arg[4]," ",$arg[5]);
	}
	deleteVariables("arg*");
}}

//Take the tag strings we got from the server and assign them to a TAG with the same tag ID used in Net::newWeapon
function Nova::handleNetNameTagStrings(%shortTagID, %shortTagString, %longTagID, %longTagString)
{
	eval("IDNET_TAG_STRING_SHORT = %shortTagID, %shortTagString;");
	eval("IDNET_TAG_STRING_LONG = %longTagID, %longTagString;");
}

function Nova::handleNetDescriptionTagString(%descriptionTagID, %descriptionTagString)
{
	eval("IDNET_TAG_STRING_DESCRIPTION = %descriptionTagID, %descriptionTagString;");
}

function Nova::handleNetSoundTagString(%soundTagID, %soundTagString)
{
	%soundTagName = String::toUpper(String::Replace(%soundTagString,".wav",""));
	%soundTagName = String::Replace(%soundTagName,"IDSFX_","");
	%soundTagName = String::Replace(%soundTagName,"SFX_","");
	%soundTagName = "IDSFX_NET_" @ %soundTagName;

	//eval(%soundTagName @ "=" @ %soundTagID @ ", '';");
	//echo(%soundTagName @ "=" @ %soundTagID @ ", '';");
	Nova::sfxAddPair( %soundTagName, $Net::soundTag[%soundTagID, soundPrefTag], %soundTagString);
	//echo( "[sfxAddPair] " @%soundTagName, " ", $Net::soundTag[%soundTagID, soundPrefTag], " ", %soundTagString);
}

function Nova::handleProjectileTagStrings(%impactTagID, %impactTagString, %shieldImpactTagID, %shieldImpactTagString, %terrainImpactTagID, %terrainImpactTagString)
{
	eval("IDNET_TAG_STRING_IMPACT = %impactTagID, %impactTagString;");
	eval("IDNET_TAG_STRING_SHIELD_IMPACT = %shieldImpactTagID, %shieldImpactTagString;");
	eval("IDNET_TAG_STRING_TERRAIN_IMPACT = %terrainImpactTagID, %terrainImpactTagString;");
}

function Net::weaponInfo2(%cli, %netArgs)
{
	remoteEval(%cli, weaponInfo2, %netArgs);
}

function remoteweaponInfo2(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::weaponInfo2($arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved weapon data [weaponInfo2] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::weaponMuzzle(%cli, %netArgs)
{
	
	remoteEval(%cli, weaponMuzzle, %netArgs);
}

function remoteweaponMuzzle(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::weaponMuzzle($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved weapon data [weaponMuzzle] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::weaponGeneral(%cli, %netArgs)
{
	remoteEval(%cli, weaponGeneral, %netArgs);
}

function remoteweaponGeneral(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::weaponGeneral($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved weapon data [weaponGeneral] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::weaponShot(%cli, %netArgs)
{
	remoteEval(%cli, weaponShot, %netArgs);
}

function remoteweaponShot(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::weaponShot($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved weapon data [weaponShot] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::weaponEnergy(%cli, %netArgs)
{
	remoteEval(%cli, weaponEnergy, %netArgs);
}

function remoteweaponEnergy(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::weaponEnergy($arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved weapon data [weaponEnergy] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::weaponAmmo(%cli, %netArgs)
{
	remoteEval(%cli, weaponAmmo, %netArgs);
}

function remoteweaponAmmo(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::weaponAmmo($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved weapon data [weaponAmmo] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::newTurret(%cli, %netArgs)
{
	remoteEval(%cli, newTurret, %netArgs);
}

function remoteNewTurret(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::newTurret($arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved turret data [newTurret] - ", $arg[%i++]);
	}
	deleteVariables("arg*");
	if(isObject(waitroomGUI))
	{
        control::setText(IDSTR_LOADING,  *IDSTR_NOVA_NET_TURRET_DATA);
	}
}}

function Net::turretBase(%cli, %netArgs)
{
	remoteEval(%cli, turretBase, %netArgs);
}

function remoteTurretBase(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::turretBase($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved turret data [turretBase] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::turretAI(%cli, %netArgs)
{
	remoteEval(%cli, turretAI, %netArgs);
}

function remoteTurretAI(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::turretAI($arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved turret data [turretAI] - ", $arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::newHardPoint(%cli, %netArgs)
{
	remoteEval(%cli, newHardPoint, %netArgs);
}

function remoteNewHardPoint(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::newHardPoint($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved hardPoint data [] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::newSensor(%cli, %netArgs)
{
	remoteEval(%cli, newSensor, %netArgs);
}

function remoteNewSensor(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::newSensor($arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved sensor data [newSensor] - ", $arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
	if(isObject(waitroomGUI))
	{
        control::setText(IDSTR_LOADING,  *IDSTR_NOVA_NET_SENSOR_DATA);
	}
}}

function Net::sensorInfo1(%cli, %netArgs, %shortTagString, %longTagString, %descriptionTagString)
{
	remoteEval(%cli, sensorInfo1, %netArgs, %shortTagString, %longTagString, %descriptionTagString);
}

function remoteSensorInfo1(%cli, %netArgs, %shortTagString, %longTagString, %descriptionTagString)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::handleNetNameTagStrings($arg[0], %shortTagString, $arg[1], %longTagString);
	Nova::handleNetDescriptionTagString($arg[6], %descriptionTagString);
	Nova::sensorInfo1($arg[0],$arg[1],$arg[2],$arg[3],$arg[4],$arg[5],$arg[6]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved sensor data [sensorInfo1] - ", *$arg[0]," ",*$arg[1]," ",$arg[2]," ",$arg[3]," ",$arg[4]," ",$arg[5], *$arg[6]);
	}
	deleteVariables("arg*");
}}

function Net::sensorInfo2(%cli, %netArgs)
{
	remoteEval(%cli, sensorInfo2, %netArgs);
}

function remoteSensorInfo2(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::sensorInfo2($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved sensor data [sensorInfo2] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::sensorMode(%cli, %netArgs)
{
	remoteEval(%cli, sensorMode, %netArgs);
}

function remoteSensorMode(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::sensorMode($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved sensor data [sensorMode] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::newReactor(%cli, %netArgs)
{
	remoteEval(%cli, newReactor, %netArgs);
}

function remoteNewReactor(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::newReactor($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved reactor data [newReactor] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
	if(isObject(waitroomGUI))
	{
        control::setText(IDSTR_LOADING,  *IDSTR_NOVA_NET_REACTOR_DATA);
	}
}}

function Net::reactorInfo1(%cli, %netArgs, %shortTagString, %longTagString, %descriptionTagString)
{
	remoteEval(%cli, reactorInfo1, %netArgs, %shortTagString, %longTagString, %descriptionTagString);
}

function remoteReactorInfo1(%cli, %netArgs, %shortTagString, %longTagString, %descriptionTagString)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::handleNetNameTagStrings($arg[0], %shortTagString, $arg[1], %longTagString);
	Nova::handleNetDescriptionTagString($arg[6], %descriptionTagString);
	Nova::reactorInfo1($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved reactor data [reactorInfo1] - ", *$arg[%i++]," ",*$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++], *$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::reactorInfo2(%cli, %netArgs)
{
	remoteEval(%cli, reactorInfo2, %netArgs);
}

function remoteReactorInfo2(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::reactorInfo2($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved reactor data [reactorInfo2] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::newShield(%cli, %netArgs)
{
	remoteEval(%cli, newShield, %netArgs);
}

function remoteNewShield(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::newShield($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved shield data [newShield] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
	if(isObject(waitroomGUI))
	{
        control::setText(IDSTR_LOADING,  *IDSTR_NOVA_NET_SHIELD_DATA);
	}
}}

function Net::shieldInfo1(%cli, %netArgs, %shortTagString, %longTagString, %descriptionTagString)
{
	remoteEval(%cli, shieldInfo1, %netArgs, %shortTagString, %longTagString, %descriptionTagString);
}

function remoteShieldInfo1(%cli, %netArgs, %shortTagString, %longTagString, %descriptionTagString)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::handleNetNameTagStrings($arg[0], %shortTagString, $arg[1], %longTagString);
	Nova::handleNetDescriptionTagString($arg[6], %descriptionTagString);
	Nova::shieldInfo1($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved shield data [shieldInfo1] - ", *$arg[%i++]," ",*$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++], *$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::shieldInfo2(%cli, %netArgs)
{
	remoteEval(%cli, shieldInfo2, %netArgs);
}

function remoteShieldInfo2(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::shieldInfo2($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved shield data [shieldInfo2] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::newEngine(%cli, %netArgs)
{
	remoteEval(%cli, newEngine, %netArgs);
}

function remoteNewEngine(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::newEngine($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved engine data [newEngine] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
	if(isObject(waitroomGUI))
	{
        control::setText(IDSTR_LOADING,  *IDSTR_NOVA_NET_ENGINE_DATA);
	}
}}

function Net::engineInfo1(%cli, %netArgs, %shortTagString, %longTagString, %descriptionTagString)
{
	remoteEval(%cli, engineInfo1, %netArgs, %shortTagString, %longTagString, %descriptionTagString);
}

function remoteEngineInfo1(%cli, %netArgs, %shortTagString, %longTagString, %descriptionTagString)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::handleNetNameTagStrings($arg[0], %shortTagString, $arg[1], %longTagString);
	Nova::handleNetDescriptionTagString($arg[6], %descriptionTagString);
	Nova::engineInfo1($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved engine data [engineInfo1] - ", *$arg[%i++]," ",*$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++], *$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::engineInfo2(%cli, %netArgs)
{
	remoteEval(%cli, engineInfo2, %netArgs);
}

function remoteEngineInfo2(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::engineInfo2($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved engine data [engineInfo2] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::newComputer(%cli, %netArgs)
{
	remoteEval(%cli, newComputer, %netArgs);
}

function remoteNewComputer(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::newComputer($arg[%i++],$arg[%i++],flt($arg[%i++]),$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved computer data [newComputer] - ", $arg[%i++]," ",$arg[%i++]," ",flt($arg[%i++])," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
	if(isObject(waitroomGUI))
	{
        control::setText(IDSTR_LOADING,  *IDSTR_NOVA_NET_COMPUTER_DATA);
	}
}}

function Net::mountInfo1(%cli, %netArgs, %shortTagString, %longTagString, %descriptionTagString)
{
	remoteEval(%cli, mountInfo1, %netArgs, %shortTagString, %longTagString, %descriptionTagString);
}

function remoteMountInfo1(%cli, %netArgs, %shortTagString, %longTagString, %descriptionTagString)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::handleNetNameTagStrings($arg[0], %shortTagString, $arg[1], %longTagString);
	Nova::handleNetDescriptionTagString($arg[6], %descriptionTagString);
	Nova::mountInfo1($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved mount data [mountInfo1] - ", *$arg[%i++]," ",*$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++], *$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::mountInfo2(%cli, %netArgs)
{
	remoteEval(%cli, mountInfo2, %netArgs);
}

function remoteMountInfo2(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::mountInfo2($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved mount data [mountInfo2] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::newECM(%cli, %netArgs)
{
	remoteEval(%cli, newECM, %netArgs);
}

function remoteNewECM(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::newECM($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved ECM data [newECM] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
	if(isObject(waitroomGUI))
	{
        control::setText(IDSTR_LOADING,  *IDSTR_NOVA_NET_INT_MOUNT_DATA);
	}
}}

function Net::newThermal(%cli, %netArgs)
{
	remoteEval(%cli, newThermal, %netArgs);
}

function remoteNewThermal(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::newThermal($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved thermal data [newThermal] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
	if(isObject(waitroomGUI))
	{
        control::setText(IDSTR_LOADING,  *IDSTR_NOVA_NET_INT_MOUNT_DATA);
	}
}}

function Net::newCloak(%cli, %netArgs)
{
	remoteEval(%cli, newCloak, %netArgs);
}

function remoteNewCloak(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::newCloak($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved cloak data [newCloak] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
	if(isObject(waitroomGUI))
	{
        control::setText(IDSTR_LOADING,  *IDSTR_NOVA_NET_INT_MOUNT_DATA);
	}
}}

function Net::newModulator(%cli, %netArgs)
{
	remoteEval(%cli, newModulator, %netArgs);
}

function remoteNewmodulator(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::newModulator($arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved modulator data [newModulator] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
	if(isObject(waitroomGUI))
	{
        control::setText(IDSTR_LOADING,  *IDSTR_NOVA_NET_INT_MOUNT_DATA);
	}
}}

function Net::newCapacitor(%cli, %netArgs)
{
	remoteEval(%cli, newCapacitor, %netArgs);
}

function remoteNewCapacitor(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::newCapacitor($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved capacitor data [newCapacitor] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
	if(isObject(waitroomGUI))
	{
        control::setText(IDSTR_LOADING,  *IDSTR_NOVA_NET_INT_MOUNT_DATA);
	}
}}

function Net::newamplifier(%cli, %netArgs)
{
	remoteEval(%cli, newamplifier, %netArgs);
}

function remoteNewAmplifier(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::newAmplifier($arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved amplifier data [newAmplifier] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
	if(isObject(waitroomGUI))
	{
        control::setText(IDSTR_LOADING,  *IDSTR_NOVA_NET_INT_MOUNT_DATA);
	}
}}

function Net::newMountable(%cli, %netArgs)
{
	remoteEval(%cli, newMountable, %netArgs);
}

function remoteNewMountable(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::newMountable($arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved mountable data [newMountable] - ", $arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
	if(isObject(waitroomGUI))
	{
        control::setText(IDSTR_LOADING,  *IDSTR_NOVA_NET_INT_MOUNT_DATA);
	}
}}

function Net::newBooster(%cli, %netArgs)
{
	remoteEval(%cli, newBooster, %netArgs);
}

function remoteNewBooster(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::newBooster($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved booster data [newBooster] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
	if(isObject(waitroomGUI))
	{
        control::setText(IDSTR_LOADING,  *IDSTR_NOVA_NET_INT_MOUNT_DATA);
	}
}}

function Net::newRepair(%cli, %netArgs)
{
	remoteEval(%cli, newRepair, %netArgs);
}

function remoteNewRepair(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::newRepair($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved repair data [newRepair] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
	if(isObject(waitroomGUI))
	{
        control::setText(IDSTR_LOADING,  *IDSTR_NOVA_NET_INT_MOUNT_DATA);
	}
}}

function Net::newBattery(%cli, %netArgs)
{
	remoteEval(%cli, newBattery, %netArgs);
}

function remoteNewBattery(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::newBattery($arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved battery data [newBattery] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
	if(isObject(waitroomGUI))
	{
        control::setText(IDSTR_LOADING,  *IDSTR_NOVA_NET_INT_MOUNT_DATA);
	}
}}

function Net::newArmor(%cli, %netArgs)
{
	remoteEval(%cli, newArmor, %netArgs);
}

function remoteNewArmor(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::newArmor($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved armor data [newArmor] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
	if(isObject(waitroomGUI))
	{
        control::setText(IDSTR_LOADING,  *IDSTR_NOVA_NET_ARMOR_DATA);
	}
}}

function Net::armorInfo1(%cli, %netArgs, %shortTagString, %longTagString, %descriptionTagString)
{
	remoteEval(%cli, armorInfo1, %netArgs, %shortTagString, %longTagString, %descriptionTagString);
}

function remoteArmorInfo1(%cli, %netArgs, %shortTagString, %longTagString, %descriptionTagString)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::handleNetNameTagStrings($arg[0], %shortTagString, $arg[1], %longTagString);
	Nova::handleNetDescriptionTagString($arg[6], %descriptionTagString);
	Nova::armorInfo1($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved armor data [armorInfo1] - ", *$arg[%i++]," ",*$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++], *$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::armorInfo2(%cli, %netArgs)
{
	remoteEval(%cli, armorInfo2, %netArgs);
}

function remoteArmorInfo2(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::armorInfo2($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved armor data [armorInfo2] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::armorInfoSpecial(%cli, %netArgs)
{
	remoteEval(%cli, armorInfoSpecial, %netArgs);
}

function remoteArmorSpecial(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::armorInfoSpecial($arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved armor data [armorInfoSpecial] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::sfxAddPair(%cli, %tagID, %soundPrefTagID, %fileName)
{
	remoteEval(%cli, Nova::sfxAddPair, %tagID, %soundPrefTagID, %fileName);
}

function remoteNova::sfxAddPair(%cli, %tagID, %soundPrefTagID, %fileName)
{if(%cli == 2048)
{
	Nova::sfxAddPair(%tagID, %soundPrefTagID, %fileName);
}}

function Net::newMountPoint(%cli, %netArgs)
{
	remoteEval(%cli, newMountPoint, %netArgs);
}

function remoteNewMountPoint(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	if(!strlen($arg[4]))
	{
		Nova::newMountPoint($arg[%i++], $arg[%i++], $arg[%i++], $arg[%i++]);
	}
	else
	{
		Nova::newMountPoint($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	}
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved mount point data [newMountPoint] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::newComponent(%cli, %netArgs)
{
	remoteEval(%cli, newComponent, %netArgs);
}

function remoteNewComponent(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::newComponent($arg[%i++], $arg[%i++], $arg[%i++], $arg[%i++], $arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved component data [newComponent] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::hardPointDamage(%cli, %sustainableDamage)
{
	remoteEval(%cli, hardPointDamage, %sustainableDamage);
}

function remoteHardPointDamage(%cli, %sustainableDamage)
{if(%cli == 2048)
{
	Nova::hardPointDamage(%sustainableDamage);
	if($echoDownloads)
	{
		echo("Recieved hardPoint damage - ", %sustainableDamage);
	}
	deleteVariables("arg*");
}}

function Net::hardPointSpecial(%cli, %weaponID)
{
	remoteEval(%cli, hardPointSpecial, %weaponID);
}

function remoteHardPointSpecial(%cli, %weaponID)
{if(%cli == 2048)
{
	Nova::hardPointSpecial(%weaponID);
	if($echoDownloads)
	{
		echo("Recieved hardPoint special - ", %weaponID);
	}
	deleteVariables("arg*");
}}

function Net::vehicleArtillery(%cli, %weaponID)
{
	remoteEval(%cli, vehicleArtillery, %weaponID);
}

function remoteVehicleArtillery(%cli, %bool)
{if(%cli == 2048)
{
	Nova::vehicleArtillery(%bool);
	if($echoDownloads)
	{
		echo("Recieved vehicle artillery - ", %bool);
	}
}}

function Net::vehiclePilotable(%cli, %weaponID)
{
	remoteEval(%cli, vehiclePilotable, %weaponID);
}

function remoteVehiclePilotable(%cli, %bool)
{if(%cli == 2048)
{
	Nova::vehiclePilotable(%bool);
	if($echoDownloads)
	{
		echo("Recieved vehicle Pilotable - ", %bool);
	}
}}

function Net::defaultWeapons(%cli, %netArgs)
{
	remoteEval(%cli, defaultWeapons, %netArgs);
}

function remoteDefaultWeapons(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::defaultWeapons($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved default weapon data - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::defaultMountables(%cli, %netArgs)
{
	remoteEval(%cli, defaultMountables, %netArgs);
}

function remoteDefaultMountables(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::defaultMountables($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved default mountable data - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::clientVehicleStore(%cli, %vehicleID, %vehicleType, %identityTagString, %drone)
{
	remoteEval(%cli, Nova::clientVehicleStore, %vehicleID, %vehicleType, %identityTagString, %drone);
}

function remoteNova::clientVehicleStore(%cli, %vehicleID, %vehicleType, %identityTagString, %drone)
{if(%cli == 2048)
{
	if(%drone && !$pref::showDroneVehicles)
	{
		return;
	}
	
	if(%vehicleID == 96)
	{
		return;
	}
	sfxClose();
	//echo("STORING SERVER VEHICLE DATA - ", %vehicleID, ", ", %vehicleType, ", ", %identityTagString);
	newObject(tempVehicleData, %vehicleType, %vehicleID);
	storeObject(tempVehicleData, "mods\\session\\" @ %identityTagString @ " [ID-" @ %vehicleID @ "].fvh");
	deleteObject(tempVehicleData);
    sfxOpen();
}}

////////
//HERC FUNCTIONS
////////
function Net::newHerc(%cli, %id)
{
	remoteEval(%cli, newHerc, %id);
}

function remoteNewHerc(%cli, %id)
{if(%cli == 2048)
{
	Nova::newHerc(%id);
	if($echoDownloads)
	{
		echo("Recieved new herc - ", %id);
	}
	if(isObject(waitroomGUI))
	{
        control::setText(IDSTR_LOADING,  *IDSTR_NOVA_NET_VEHICLE_DATA);
	}
	allowVehicle(%id,true);
}}

function Net::hercBase(%cli, %netArgs, %identityTagString)
{
	remoteEval(%cli, hercBase, %netArgs, %identityTagString);
}

function remoteHercBase(%cli, %netArgs, %identityTagString)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	eval("IDVEH_NET_IDENTITY = $arg[0], %identityTagString;");
	Nova::hercBase($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved herc data [hercBase, ", *$arg[0], "] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::hercPos(%cli, %netArgs)
{
	remoteEval(%cli, hercPos, %netArgs);
}

function remoteHercPos(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::hercPos($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved herc data [hercPos] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::hercRot(%cli, %netArgs)
{
	remoteEval(%cli, hercRot, %netArgs);
}

function remoteHercRot(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::hercRot($arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved herc data [hercRot] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::hercColl(%cli, %netArgs)
{
	remoteEval(%cli, hercColl, %netArgs);
}

function remoteHercColl(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::hercColl($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved herc data [hercColl] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::hercAnim(%cli, %netArgs)
{
	remoteEval(%cli, hercAnim, %netArgs);
}

function remoteHercAnim(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::hercAnim($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved herc data [hercAnim] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::hercCpit(%cli, %netArgs)
{
	remoteEval(%cli, hercCpit, %netArgs);
}

function remoteHercCpit(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::hercCpit($arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved herc data [hercCpit] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::hercAI(%cli, %netArgs)
{
	remoteEval(%cli, hercAI, %netArgs);
}

function remoteHercAI(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::hercAI($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved herc data [hercAI] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::hercFootprint(%cli, %netArgs)
{
	remoteEval(%cli, hercFootprint, %netArgs);
}

function remoteHercFootprint(%cli, %netArgs)
{if(%cli == 2048)
{
	Nova::hercFootprint(%netArgs);
	if($echoDownloads)
	{
		echo("Recieved herc data [hercFootprint] - ", %netArgs);
	}
}}

////////
//flyer FUNCTIONS
////////
function Net::newflyer(%cli, %id)
{
	remoteEval(%cli, newflyer, %id);
}

function remoteNewflyer(%cli, %id)
{if(%cli == 2048)
{
	Nova::newflyer(%id);
	if($echoDownloads)
	{
		echo("Recieved new flyer - ", %id);
	}
	if(isObject(waitroomGUI))
	{
        control::setText(IDSTR_LOADING,  *IDSTR_NOVA_NET_VEHICLE_DATA);
	}
	allowVehicle(%id,true);
}}

function Net::flyerBase(%cli, %netArgs, %identityTagString)
{
	remoteEval(%cli, flyerBase, %netArgs, %identityTagString);
}

function remoteflyerBase(%cli, %netArgs, %identityTagString)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	eval("IDVEH_NET_IDENTITY = $arg[0], %identityTagString;");
	Nova::flyerBase($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved flyer data [flyerBase, ", *$arg[0], "] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::flyerPos(%cli, %netArgs)
{
	remoteEval(%cli, flyerPos, %netArgs);
}

function remoteflyerPos(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::flyerPos($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved flyer data [flyerPos] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::flyerRot(%cli, %netArgs)
{
	remoteEval(%cli, flyerRot, %netArgs);
}

function remoteflyerRot(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::flyerRot($arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved flyer data [flyerRot] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::flyerColl(%cli, %netArgs)
{
	remoteEval(%cli, flyerColl, %netArgs);
}

function remoteflyerColl(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::flyerColl($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved flyer data [flyerColl] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::flyerExhaust(%cli, %netArgs)
{
	remoteEval(%cli, flyerExhaust, %netArgs);
}

function remoteflyerExhaust(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::flyerExhaust($arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved flyer data [flyerExhaust] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::flyerCpit(%cli, %netArgs)
{
	remoteEval(%cli, flyerCpit, %netArgs);
}

function remoteflyerCpit(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::flyerCpit($arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved flyer data [flyerCpit] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::flyerAI(%cli, %netArgs)
{
	remoteEval(%cli, flyerAI, %netArgs);
}

function remoteflyerAI(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::flyerAI($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved flyer data [flyerAI] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::flyerNav(%cli, %netArgs)
{
	remoteEval(%cli, flyerNav, %netArgs);
}

function remoteflyerNav(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::flyerNav($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved flyer data [flyerNav] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::flyerSound(%cli, %netArgs)
{
	remoteEval(%cli, flyerSound, %netArgs);
}

function remoteflyerSound(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::flyerSound($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved flyer data [flyerSound] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::flyerExhaustOffset(%cli, %netArgs)
{
	remoteEval(%cli, flyerExhaustOffset, %netArgs);
}

function remoteflyerExhaustOffset(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::flyerExhaustOffset($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved flyer data [flyerExhaustOffset] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

////////
//TANK FUNCTIONS
////////
function Net::newTank(%cli, %id)
{
	remoteEval(%cli, newtank, %id);
}

function remoteNewtank(%cli, %id)
{if(%cli == 2048)
{
	Nova::newtank(%id);
	if($echoDownloads)
	{
		echo("Recieved new tank - ", %id);
	}
	if(isObject(waitroomGUI))
	{
        control::setText(IDSTR_LOADING,  *IDSTR_NOVA_NET_VEHICLE_DATA);
	}
	allowVehicle(%id,true);
}}

function Net::tankBase(%cli, %netArgs, %identityTagString)
{
	remoteEval(%cli, tankBase, %netArgs, %identityTagString);
}

function remoteTankBase(%cli, %netArgs, %identityTagString)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	eval("IDVEH_NET_IDENTITY = $arg[0], %identityTagString;");
	Nova::tankBase($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved tank data [tankBase, ", *$arg[0], "] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::tankPos(%cli, %netArgs)
{
	remoteEval(%cli, tankPos, %netArgs);
}

function remoteTankPos(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::tankPos($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved tank data [tankPos] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::tankRot(%cli, %netArgs)
{
	remoteEval(%cli, tankRot, %netArgs);
}

function remoteTankRot(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::tankRot($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved tank data [tankRot] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::tankColl(%cli, %netArgs)
{
	remoteEval(%cli, tankColl, %netArgs);
}

function remoteTankColl(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::tankColl($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved tank data [tankColl] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::tankAnim(%cli, %netArgs)
{
	remoteEval(%cli, tankAnim, %netArgs);
}

function remoteTankAnim(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::tankAnim($arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved tank data [tankAnim] - ", $arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::tankSound(%cli, %netArgs)
{
	remoteEval(%cli, tankSound, %netArgs);
}

function remoteTankSound(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::tankSound($arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved tank data [tankSound] - ", $arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::tankSlide(%cli, %netArgs)
{
	remoteEval(%cli, tankSlide, %netArgs);
}

function remoteTankSlide(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::tankSlide($arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved tank data [tankSlide] - ", $arg[%i++]);
	}
	deleteVariables("arg*");
}}

////////
//DRONE FUNCTIONS
////////
function Net::newDrone(%cli, %id)
{
	remoteEval(%cli, newDrone, %id);
}

function remoteNewDrone(%cli, %id)
{if(%cli == 2048)
{
	Nova::newDrone(%id);
	if($echoDownloads)
	{
		echo("Recieved new drone - ", %id);
	}
	if(isObject(waitroomGUI))
	{
        control::setText(IDSTR_LOADING,  *IDSTR_NOVA_NET_VEHICLE_DATA);
	}
	allowVehicle(%id,true);
}}

function Net::droneBase(%cli, %netArgs, %identityTagString)
{
	remoteEval(%cli, droneBase, %netArgs, %identityTagString);
}

function remoteDroneBase(%cli, %netArgs, %identityTagString)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	eval("IDVEH_NET_IDENTITY = $arg[0], %identityTagString;");
	Nova::droneBase($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved drone data [droneBase, ", *$arg[0], "] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::dronePos(%cli, %netArgs)
{
	remoteEval(%cli, dronePos, %netArgs);
}

function remoteDronePos(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::dronePos($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved drone data [dronePos] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::droneRot(%cli, %netArgs)
{
	remoteEval(%cli, droneRot, %netArgs);
}

function remoteDroneRot(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::droneRot($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved drone data [droneRot] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::droneColl(%cli, %netArgs)
{
	remoteEval(%cli, droneColl, %netArgs);
}

function remoteDroneColl(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::droneColl($arg[%i++],$arg[%i++],$arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved drone data [droneColl] - ", $arg[%i++]," ",$arg[%i++]," ",$arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::droneAnim(%cli, %netArgs)
{
	remoteEval(%cli, droneAnim, %netArgs);
}

function remoteDroneAnim(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::droneAnim($arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved drone data [droneAnim] - ", $arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::droneSound(%cli, %netArgs)
{
	remoteEval(%cli, droneSound, %netArgs);
}

function remoteDroneSound(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::droneSound($arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved drone data [droneSound] - ", $arg[%i++]," ",$arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::droneSlide(%cli, %netArgs)
{
	remoteEval(%cli, droneSlide, %netArgs);
}

function remoteDroneSlide(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::droneSlide($arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved drone data [droneSlide] - ", $arg[%i++]);
	}
	deleteVariables("arg*");
}}

function Net::droneExplosion(%cli, %netArgs)
{
	remoteEval(%cli, droneExplosion, %netArgs);
}

function remoteDroneExplosion(%cli, %netArgs)
{if(%cli == 2048)
{
	%i=-1;
	%netArgs = String::replace(%netArgs, "'", "");
	String::Explode(%netArgs, ",", arg);
	Nova::droneExplosion($arg[%i++],$arg[%i++]);
	%i=-1;
	if($echoDownloads)
	{
		echo("Recieved drone data [droneExplosion] - ", $arg[%i++], " ", $arg[%i++]);
	}
	deleteVariables("arg*");
}}

//function modloader::parseFileData(%playerID, %file, %dataString, %token)
//{
//    if(%token != $clientToken[%playerID])
//    {
//      return;
//    }
//	focusserver();
//    %dataString = String::Replace(String::Escape(%dataString),'\0',"");
//            
//    if(String::findSubStr(strlen(%dataString)/2, ".") > -1)
//    {
//        //Data stream has odd number of hexadecimals. Trim the null byte.
//        %dataString = String::getSubStr(%dataString,0,strlen(%dataString)-1);
//    }
//        if(strlen(String::Replace(String::getSubStr(%dataString,%index,254), "\x20", "")))         {%dat1 = String::Replace(String::getSubStr(%dataString,%index,254),"\x20", "");}
//        if(strlen(String::Replace(String::getSubStr(%dataString,%index+=254,254), "\x20", "")))    {%dat2 = String::Replace(String::getSubStr(%dataString,%index,254),"\x20", "");}
//        if(strlen(String::Replace(String::getSubStr(%dataString,%index+=254,254), "\x20", "")))    {%dat3 = String::Replace(String::getSubStr(%dataString,%index,254),"\x20", "");}
//
//        //%index+=254;
//        remoteEval(%playerID, modloader::recieveFileData, %token, %file,%dat1,%dat2,%dat3);
//			
//        if (!$CmdLineServer)
//        {
//            focusclient();
//        }
//}

//function remotemodloader::recieveFileData(%cli,%token,%file,%dat1,%dat2,%dat3)
//{
//	if(%token != $zzClientToken)
//	{
//		return;
//	}
//    if(%cli == 2048)
//    {
//		control::setActive(IDSTR_TAB_VEHICLE_LAB, false);
//        %file = stripPath(%file);
//        //Send it to the cache directory
//        %str = String::Replace($client::connectTo,"IP:","");
//        %str = String::Replace(%str,":","_");
//        %str = String::Replace(%str,".","_");
//        %file = "mods/cache/" @ %str @ "/" @ %file;
//		
//		fileWriteHex(%file, %dat1 @ %dat2 @ %dat3);
//		
//		if(isObject(waitroomGUI))
//		{
//			modloader::updateProgressBar(%file, strlen(%dat1 @ %dat2 @ %dat3));
//		}
//    }
//}

function Nova::getServerTerrain()
{
	remoteEval(2048, modloader::getServerTerrain);
}

//SERVER-SIDE function
function remotemodloader::getServerTerrain(%cli)
{
    if(%cli == 2048)
    {
        return;
    }
    
    if(focusserver() && isObject(8))
    {
        //focusclient();
        %terrainGridName = String::Explode(getTerrainGridFile(), "#", gridStringTrim);
        %fileName = $gridStringTrim[0] @ ".vol";
        %terrainFilePath = "multiplayer/" @ %fileName;
		deleteVariables("gridStringTrim*");
        remoteEval(%cli, modloader::validateFile, %terrainFilePath, getSHA1(%terrainFilePath));
        if (!$CmdLineServer)
        {
            focusclient();
        }
    }
}

//CLIENT-SIDE function
//function remotemodloader::validateFile(%cli, %file, %sha)
//{
//    //Server only. and only accept downloads before the map finishes loading
//    if(%cli == 2048 && !$GUI::AllStaticsGhosted && (String::findSubStr(%file,".vol") > 0 || String::findSubStr(%file,".mlv") > 0) )
//    {
//        //echo("Validating map terrain");
//        %fullPath = %file; //Server-side file path
//        %file = stripPath(%file); //file name
//        
//        %str = String::Replace($client::connectTo,"IP:","");
//        %str = String::Replace(%str,":","_");
//        %str = String::Replace(%str,".","_");
//        %cacheFile = "mods/cache/" @ %str @ "/" @ %file;
//		
//		// echo("//----------------\\");
//		// echo("fullpath: ", _(%fullpath,NULL));
//		// echo("file: ",_(%file,NULL));
//		// echo("cache file: ",_(%cacheFile,NULL));
//		// echo("\\----------------//");
//		
//		// echo("debug_0");
//		//The server-cache file should have higher priority
//		//Do we have the file? (cache directory)
//		if(isFile(%cacheFile))
//		{
//			// echo("debug_1");
//			if(getSHA1(%cacheFile) == %sha) //Check for cache file
//			{
//				// echo("debug_2");
//				if(String::findSubStr(%cacheFile, ".ted.vol") != -1)
//				{
//					remoteEval(2048, Net::requestServerMod, $modIndex++);
//				}
//				Net::loadServerMod(%cacheFile);
//				control::setActive(IDSTR_TAB_VEHICLE_LAB, true);
//				return;
//			}
//		}
//		
//		//Do we have the file? (mods|multiplayer directory)
//		if(isFile(%fullPath))
//		{
//			// echo("debug_4");
//			if(getSHA1(%fullpath) == %sha) //Check for mod/multiplayer file
//			{
//				// echo("debug_5");
//				if(String::findSubStr(%fullPath, ".ted.vol") != -1)
//				{
//					remoteEval(2048, Net::requestServerMod, $modIndex++);
//				}
//				Net::loadServerMod(%fullpath);
//				control::setActive(IDSTR_TAB_VEHICLE_LAB, true);
//				return;
//			}
//		}
//		
//		//We dont have the file in either directory or our files don't match the servers
//		else
//		{
//			// echo("debug_7");
//			%cacheFile = String::Replace(%cacheFile,"mods/", "");
//			removeCacheFile(%cacheFile);
//			control::setActive(IDSTR_TAB_VEHICLE_LAB, true);
//			
//			%cacheFile = String::Replace(%cacheFile,"mods/", "");
//			schedule("remoteEval(2048, modloader::transferServerFile,\"" @ %fullPath @ "\",'" @ %sha @ "','" @ $zzClientToken @ "');",0.1);
//		}
//    }
//}

//function remoteNet::throttleGhostManager(%cli, %padding)
//{
//	if(%cli != 2048)
//	{
//		//Do nothing
//		return true;
//	}
//}

//function remotemodloader::transferServerFile(%cli, %file, %sha, %token)
//{
//    if(%cli != 2048)
//    {
//        if(%sha == getSHA1(%file))
//        {
//            focusserver();
//			
//            //The file path on the server end is different than the client end so we will modify it later on
//            $clientToken[%cli] = %token;
//			remoteEval(%cli, modloader::modFileSize, getFileSize(%file));
//            //modloader::uploadFiletoClient(%file,%cli,$clientToken[%cli]);
//
//			//Websocket function
//			remoteEval(%cli, Net::getServerFile, %file);
//            if (!$CmdLineServer)
//            {
//                focusclient();
//            }
//        } 
//    }
//}

function remoteNet::getServerFile(%cli,%file)                                      
{                                                                                      
    if(%cli == 2048)                                                                   
    {
		String::Explode($Client::ConnectTo, ":", serverIP);
		%ip = $serverIP[1];
		%port = $serverIP[2];
		%cacheDir = String::Replace($client::connectTo,"IP:","");
		%cacheDir = String::Replace(%cacheDir,":","_");
		%cacheDir = String::Replace(%cacheDir,".","_");
		
		%file = String::Replace(%file, "/", "\\");
		//%file = String::Replace(%file, "mods\\", "");
		//Websocket function
		//echo("Requesting Server File, ", %ip, " | ", %port, " | ", %file, " | ", ".\\mods\\cache\\" @ %cacheDir @ "\\");
		Nova::requestServerFile(%ip, %port, %file, ".\\mods\\cache\\" @ %cacheDir @ "\\");
	}
}
	
//function remotemodloader::modFileSize(%cli,%int)                                      
//{                                                                                      
//    if(%cli == 2048)                                                                   
//    {
//        $downloadTicker = 0;
//        //$download::totalSize = %int*2;
//        $download::totalSize = %int;
//    }                                                                                  
//}

//function modloader::updateProgressBar(%file,%inc)
//{
//    if($download::totalSize >= $downloadTicker && !$GUI::AllStaticsGhosted)
//    {
//        $downloadTicker+=%inc;
//        %fileName = stripPath(%file);
//        #Progress bar functionality. Lets give the client a progress bar so they can see that the game is actually doing something.
//        
//        ##Updated: DatabaseDownload.gui has been depreciated in favor of using the loading bar in the Waitroom GUI.
//        %progressBarObject = "waitroomGUI\\progressBar";
//        
//        if(!isObject(%progressBarObject))
//        {
//            loadObject(progressBar, "dataProgressBar.object");
//            addtoset(waitroomGUI, progressBar);
//        }
//        
//        if(strlen(%file))
//        {
//            if(String::findSubStr(%file, ".ted.vol") != -1)
//            {
//                control::setText(IDSTR_LOADING, *IDSTR_NOVA_MODLOADER_TERRAIN_TRANS);
//            }
//            else
//            {
//                control::setText(IDSTR_LOADING, *IDSTR_NOVA_MODLOADER_MOD_TRANS @ " [" @ $modIndex @ "/" @ $serverMods-1 @ "]" );
//            }
//        }
//        
//        %progressBarLength = 245;
//        %progressBarHeight = 7;
//        %progress = $downloadTicker;
//        %progressCompletion = (%progress / $download::totalSize);
//        %progressBarCoeff = %progressCompletion * %progressBarLength;
//        %status = (%progressCompletion * 100);
//        
//        #Trim off the "nths"
//        if(String::findSubStr(%status,".") != -1)
//		{
//			String::explode(%status, ".", status);
//			%status = $status[0];
//		}
//        
//        control::setText(percentageDone, %fileName @ " - " @ String::Replace(%status, ".", "") @ "%");
//        control::setText(percentageDoneBackground, %fileName @ " - " @ String::Replace(%status, ".", "") @ "%");
//        
//        if($downloadTicker > 1)
//        {
//            control::setVisible(progressBarGFX, true);
//            control::setVisible(progressBar, true);
//        }
//        
//        // if($downloadTicker >= $download::totalSize)
//        // {
//            // control::setText(percentageDone, %file @ " - " @ "100%");
//            // control::setText(percentageDoneBackground, %file @ " - " @ "100%");
//        // }
//		
//        #Psuedo loading bar animation
//        if(isObject(%progressBarObject @ "\\progressBarTrimmer") && $downloadTicker < $download::totalSize)
//        {
//            #Move the progress bar graphic out of the trimmer control
//            addtoSet(0, %progressBarObject @ "\\progressBarTrimmer\\progressBarGFX");
//            #Delete the old trimmer
//            deleteobject(%progressBarObject @ "\\progressBarTrimmer");
//            #Create a new trimmer with updated progress bar length
//            newobject(progressBarTrimmer, simgui::control, 10, 5, %progressBarCoeff, %progressBarHeight);
//            addtoset(%progressBarObject,"progressBarTrimmer");
//            addtoset(%progressBarObject @ "\\progressBarTrimmer", "progressBarGFX");
//        }
//		
//        if($downloadTicker >= $download::totalSize)
//        {
//            control::setText(percentageDone, %fileName @ " - " @ "100%");
//            control::setText(percentageDoneBackground, %fileName @ " - " @ "100%");
//            appendSearchPath();
//			$downloadTicker=0;
//			schedule("control::setVisible(progressBar, false);",0.5);
//			
//			//Once we have the terrain file, request the server mods
//			if(String::findSubStr(%file, ".ted.vol") != -1)
//            {
//				remoteEval(2048, Net::requestServerMod, $modIndex++);
//			}
//			Net::loadServerMod(%file);
//			control::setActive(IDSTR_TAB_VEHICLE_LAB, true);
//        }
//    }
//}

function Net::loadServerMod(%file)
{
	if(!strlen(%file))
	{
		return;
	}
	
	appendSearchPath(); //Update the file list
	
	//Dont manually load the volume if it is a terrain volume. The ghostManager will load it automatically
    if(String::findSubStr(%file, ".ted.vol") == -1)
    {
		// echo("LOADING: ", %file);
		if(!isObject(ServerModDatabase))
		{
			newObject(ServerModDatabase, simGroup);
		}
		
		String::Explode(stripFilePath(%file), ".", file);
		
		if(!isObject("ServerModDatabase\\" @ $file[0]))
		{
			echo("Nova Loading File: ", stripFilePath(%file), " (Server Mod)");
			newObject(stripFilePath(%file), simVolume, stripFilePath(%file));
			addToSet(ServerModDatabase, stripFilePath(%file));
		}
		deleteVariables("file*");
	}
}