//////////////////////////////////////////////////////////////////////////////////
//PROJECTILE CREATION/////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
$MLproj_arr=0;
function newBullet(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12,%a13,%a14,%a15,%a16,%a17,%a18,%a19,%a20,%a21,%a22,%a23,%a24)
{
    if(modLoader::Database::CheckID(%a2,projectile,%a1))
    {
        if(%a2 > 127)
        {
            modLoader::Logger::newEntry(warn, "[<b style=\"color:orange;\"> " @ %a1 @ " </b>] ID: <b style=\"color:orange;\">" @ %a2 @ "</b>. Build SUCCESS. <b style=\"color:orange;\">ID OUT OF RANGE</b>. (Exceeds 127)" );
            return;
        }
        modLoader::Logger::newEntry(normal, "[<b style=\"color:lime;\"> " @ %a1 @ " </b>] ID: <b style=\"color:lime;\">" @ %a2 @ "</b>. Build SUCCESS" );
        Nova::newBullet(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12,%a13,%a14,%a15,%a16,%a17,%a18,%a19,%a20,%a21,%a22,%a23,%a24);
        $Net::bulletProjectileData[%a2] = "'" @ flt(%a1) @ "','" @ flt(%a2) @ "','" @ flt(%a3) @ "','" @ flt(%a4) @ "','" @ flt(%a5) @ "','" @ flt(%a6) @ "','" @ flt(%a7) @ "','" @ flt(%a8) @ "','" @ flt(%a9) @ "','" @ flt(%a10) @ "','" @ flt(%a11) @ "','" @ flt(%a12) @ "','" @ flt(%a13) @ "','" @ flt(%a14) @ "','" @ flt(%a15) @ "','" @ flt(%a16) @ "','" @ flt(%a17) @ "','" @ flt(%a18) @ "','" @ flt(%a19) @ "','" @ flt(%a20) @ "','" @ flt(%a21) @ "','" @ flt(%a22) @ "','" @ flt(%a23) @ "','" @ flt(%a24) @ "'";
		$Net::bulletProjectileData[%a2, impactTags] = "'" @ %a22 @ "','" @ %a23 @ "','" @ %a24 @ "'";
		$Net::projectile[%a2, type] = bullet;
    }
}

function newMissile(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12,%a13,%a14,%a15,%a16,%a17,%a18,%a19,%a20,%a21,%a22,%a23,%a24,%a25,%a26,%a27,%a28,%a29,%a30,%a31,%a32,%a33,%a34,%a35)
{
    if(modLoader::Database::CheckID(%a2,projectile,%a1))
    {
        if(%a2 > 127)
        {
            modLoader::Logger::newEntry(warn, "[<b style=\"color:orange;\"> " @ %a1 @ " </b>] ID: <b style=\"color:orange;\">" @ %a2 @ "</b>. Build SUCCESS. <b style=\"color:orange;\">ID OUT OF RANGE</b>. (Exceeds 127)" );
            return;
        }
        modLoader::Logger::newEntry(normal, "[<b style=\"color:lime;\"> " @ %a1 @ " </b>] ID: <b style=\"color:lime;\">" @ %a2 @ "</b>. Build SUCCESS" );
        datastore(projectileDatabase, MLproj_arr_abbr @ $MLproj_arr , %a1);
        Nova::newMissile(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12,%a13,%a14,%a15,%a16,%a17,%a18,%a19,%a20,%a21,%a22,%a23,%a24,%a25,%a26,%a27,%a28,%a29,%a30,%a31,%a32,%a33,%a34,%a35);
        $Net::missileProjectileData[%a2] = "'" @ flt(%a1) @ "','" @ flt(%a2) @ "','" @ flt(%a3) @ "','" @ flt(%a4) @ "','" @ flt(%a5) @ "','" @ flt(%a6) @ "','" @ flt(%a7) @ "','" @ flt(%a8) @ "','" @ flt(%a9) @ "','" @ flt(%a10) @ "','" @ flt(%a11) @ "','" @ flt(%a12) @ "','" @ flt(%a13) @ "','" @ flt(%a14) @ "','" @ flt(%a15) @ "','" @ flt(%a16) @ "','" @ flt(%a17) @ "','" @ flt(%a18) @ "','" @ flt(%a19) @ "','" @ flt(%a20) @ "','" @ flt(%a21) @ "','" @ flt(%a22) @ "','" @ flt(%a23) @ "','" @ flt(%a24) @ "','" @ flt(%a25) @ "','" @ flt(%a26) @ "','" @ flt(%a27) @ "','" @ flt(%a28) @ "','" @ flt(%a29) @ "','" @ flt(%a30) @ "','" @ flt(%a31) @ "','" @ flt(%a32) @ "','" @ flt(%a33) @ "','" @ flt(%a34) @ "','" @ flt(%a35) @ "'";
		$Net::missileProjectileData[%a2, impactTags] = "'" @ %a34 @ "','" @ %a35 @ "'";
		$Net::projectile[%a2, type] = missile;
    }
}

function newEnergy(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12,%a13,%a14,%a15,%a16,%a17,%a18,%a19,%a20,%a21,%a22,%a23,%a24,%a25)
{
    if(modLoader::Database::CheckID(%a2,projectile,%a1))
    {
        if(%a2 > 127)
        {
            modLoader::Logger::newEntry(warn, "[<b style=\"color:orange;\"> " @ %a1 @ " </b>] ID: <b style=\"color:orange;\">" @ %a2 @ "</b>. Build SUCCESS. <b style=\"color:orange;\">ID OUT OF RANGE</b>. (Exceeds 127)" );
            return;
        }
        modLoader::Logger::newEntry(normal, "[<b style=\"color:lime;\"> " @ %a1 @ " </b>] ID: <b style=\"color:lime;\">" @ %a2 @ "</b>. Build SUCCESS" );
        datastore(projectileDatabase, MLproj_arr_abbr @ $MLproj_arr , %a1);
        Nova::newEnergy(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12,%a13,%a14,%a15,%a16,%a17,%a18,%a19,%a20,%a21,%a22,%a23,%a24,%a25);
        $Net::energyProjectileData[%a2] = "'" @ flt(%a1) @ "','" @ flt(%a2) @ "','" @ flt(%a3) @ "','" @ flt(%a4) @ "','" @ flt(%a5) @ "','" @ flt(%a6) @ "','" @ flt(%a7) @ "','" @ flt(%a8) @ "','" @ flt(%a9) @ "','" @ flt(%a10) @ "','" @ flt(%a11) @ "','" @ flt(%a12) @ "','" @ flt(%a13) @ "','" @ flt(%a14) @ "','" @ flt(%a15) @ "','" @ flt(%a16) @ "','" @ flt(%a17) @ "','" @ flt(%a18) @ "','" @ flt(%a19) @ "','" @ flt(%a20) @ "','" @ flt(%a21) @ "','" @ flt(%a22) @ "','" @ flt(%a23) @ "','" @ flt(%a24) @ "','" @ flt(%a25) @ "'";
		$Net::energyProjectileData[%a2, impactTags] = "'" @ %a22 @ "','" @ %a23 @ "','" @ %a24 @ "'";
		$Net::projectile[%a2, type] = energy;
    }
}

function newBeam(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12,%a13,%a14,%a15,%a16,%a17,%a18,%a19,%a20,%a21,%a22,%a23,%a24,%a25,%a26,%a27,%a28,%a29,%a30,%a31,%a32,%a33)
{
    if(modLoader::Database::CheckID(%a2,projectile,%a1))
    {
        if(%a2 > 127)
        {
            modLoader::Logger::newEntry(warn, "[<b style=\"color:orange;\"> " @ %a1 @ " </b>] ID: <b style=\"color:orange;\">" @ %a2 @ "</b>. Build SUCCESS. <b style=\"color:orange;\">ID OUT OF RANGE</b>. (Exceeds 127)" );
            return;
        }
        modLoader::Logger::newEntry(normal, "[<b style=\"color:lime;\"> " @ %a1 @ " </b>] ID: <b style=\"color:lime;\">" @ %a2 @ "</b>. Build SUCCESS" );
        datastore(projectileDatabase, MLproj_arr_abbr @ $MLproj_arr , %a1);
        Nova::newBeam(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12,%a13,%a14,%a15,%a16,%a17,%a18,%a19,%a20,%a21,%a22,%a23,%a24,%a25,%a26,%a27,%a28,%a29,%a30,%a31,%a32,%a33);
        $Net::beamProjectileData[%a2] = "'" @ flt(%a1) @ "','" @ flt(%a2) @ "','" @ flt(%a3) @ "','" @ flt(%a4) @ "','" @ flt(%a5) @ "','" @ flt(%a6) @ "','" @ flt(%a7) @ "','" @ flt(%a8) @ "','" @ flt(%a9) @ "','" @ flt(%a10) @ "','" @ flt(%a11) @ "','" @ flt(%a12) @ "','" @ flt(%a13) @ "','" @ flt(%a14) @ "','" @ flt(%a15) @ "','" @ flt(%a16) @ "','" @ flt(%a17) @ "','" @ flt(%a18) @ "','" @ flt(%a19) @ "','" @ flt(%a20) @ "','" @ flt(%a21) @ "','" @ flt(%a22) @ "','" @ flt(%a23) @ "','" @ flt(%a24) @ "','" @ flt(%a25) @ "','" @ flt(%a26) @ "','" @ flt(%a27) @ "','" @ flt(%a28) @ "','" @ flt(%a29) @ "','" @ flt(%a30) @ "','" @ flt(%a31) @ "','" @ flt(%a32) @ "','" @ flt(%a33) @ "'";
		$Net::beamProjectileData[%a2, impactTags] = "'" @ %a31 @ "','" @ %a32 @ "','" @ %a33 @ "'";
		$Net::projectile[%a2, type] = beam;
    }
}

function newMine(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12,%a13,%a14,%a15,%a16,%a17,%a18,%a19,%a20,%a21,%a22,%a23,%a24,%a25)
{
    if(modLoader::Database::CheckID(%a2,projectile,%a1))
    {
        if(%a2 > 127)
        {
            modLoader::Logger::newEntry(warn, "[<b style=\"color:orange;\"> " @ %a1 @ " </b>] ID: <b style=\"color:orange;\">" @ %a2 @ "</b>. Build SUCCESS. <b style=\"color:orange;\">ID OUT OF RANGE</b>. (Exceeds 127)" );
            return;
        }
        modLoader::Logger::newEntry(normal, "[<b style=\"color:lime;\"> " @ %a1 @ " </b>] ID: <b style=\"color:lime;\">" @ %a2 @ "</b>. Build SUCCESS" );
        datastore(projectileDatabase, MLproj_arr_abbr @ $MLproj_arr , %a1);
        Nova::newMine(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12,%a13,%a14,%a15,%a16,%a17,%a18,%a19,%a20,%a21,%a22,%a23,%a24,%a25);
        $Net::mineProjectileData[%a2] = "'" @ flt(%a1) @ "','" @ flt(%a2) @ "','" @ flt(%a3) @ "','" @ flt(%a4) @ "','" @ flt(%a5) @ "','" @ flt(%a6) @ "','" @ flt(%a7) @ "','" @ flt(%a8) @ "','" @ flt(%a9) @ "','" @ flt(%a10) @ "','" @ flt(%a11) @ "','" @ flt(%a12) @ "','" @ flt(%a13) @ "','" @ flt(%a14) @ "','" @ flt(%a15) @ "','" @ flt(%a16) @ "','" @ flt(%a17) @ "','" @ flt(%a18) @ "','" @ flt(%a19) @ "','" @ flt(%a20) @ "','" @ flt(%a21) @ "','" @ flt(%a22) @ "','" @ flt(%a23) @ "','" @ flt(%a24) @ "','" @ flt(%a25) @ "'";
		$Net::mineProjectileData[%a2, impactTags] = "'" @ %a24 @ "','" @ %a25 @ "'";
		$Net::projectile[%a2, type] = mine;
    }
}

function newBomb(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12,%a13,%a14,%a15,%a16,%a17,%a18,%a19)
{
    if(modLoader::Database::CheckID(%a2,projectile,%a1))
    {
        if(%a2 > 127)
        {
            modLoader::Logger::newEntry(warn, "[<b style=\"color:orange;\"> " @ %a1 @ " </b>] ID: <b style=\"color:orange;\">" @ %a2 @ "</b>. Build SUCCESS. <b style=\"color:orange;\">ID OUT OF RANGE</b>. (Exceeds 127)" );
            return;
        }
        modLoader::Logger::newEntry(normal, "[<b style=\"color:lime;\"> " @ %a1 @ " </b>] ID: <b style=\"color:lime;\">" @ %a2 @ "</b>. Build SUCCESS" );
        Nova::newBomb(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12,%a13,%a14,%a15,%a16,%a17,%a18,%a19);
        $Net::bombProjectileData[%a2] = "'" @ flt(%a1) @ "','" @ flt(%a2) @ "','" @ flt(%a3) @ "','" @ flt(%a4) @ "','" @ flt(%a5) @ "','" @ flt(%a6) @ "','" @ flt(%a7) @ "','" @ flt(%a8) @ "','" @ flt(%a9) @ "','" @ flt(%a10) @ "','" @ flt(%a11) @ "','" @ flt(%a12) @ "','" @ flt(%a13) @ "','" @ flt(%a14) @ "','" @ flt(%a15) @ "','" @ flt(%a16) @ "','" @ flt(%a17) @ "','" @ flt(%a18) @ "','" @ flt(%a19) @ "'";
		$Net::bombProjectileData[%a2, impactTags] = "'" @ %a18 @ "','" @ %a19 @ "'";
    }
}
//////////////////////////////////////////////////////////////////////////////////
//WEAPON CREATION/////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
function newWeapon(%id,%shape,%mountSize,%soundTag,%Damage,%techBase,%descriptionTag)
{
    datastore(weaponDatabase, MLweap_arr_id @ $MLweap_arr++ , %id);
    if(modLoader::Database::CheckID(%id,weapon))
    {
        Nova::newWeapon(%id,%shape,%mountSize,%soundTag,%damage,%techBase,%descriptionTag);
		$Nova::totalWeapons = Nova::getTotalWeapons();
		$Net::currentWeaponIndex = %id;
		if(!$__weaponPurge)
		{
			$Net::weapon[$Net::currentWeaponIndex] = "'" @ %id @ "','" @ %shape @ "','" @ %mountSize @ "','" @ %soundTag @ "','" @ %damage @ "','" @ %techBase @ "','" @ %descriptionTag @ "'";
			$Net::weapon[$Net::currentWeaponIndex, descriptionTagString] = *%descriptionTag;
			$Net::weapon[$Net::currentWeaponIndex, soundTagString] = $Net::soundTag[%soundTag, fileName];
		}
        $zzclient::weapons++;
        if(!$zzmodloader::haltIDVars){$zzmodloader::weaponID[$WIDC++] = %id;}
    }
}

function weaponInfo1(%shortNameTag,%longNameTag,%smallBMP,%smallDisabledBMP,%largeBMP,%largeDisabledBMP)
{
    datastore(weaponDatabase, MLweap_arr_longname @ $MLweap_arr, %longNameTag);
    
    //Don't log a success if there is an actual ID conflict also don't add the weaponInfo to the database
    if(!$mlWeapConflict){
    modLoader::Logger::newEntry(normal, "[<b style=\"color:lime;\"> " @ *dataRetrieve(weaponDatabase, MLweap_arr_longname @ $MLweap_arr)  @ " </b>] ID: <b style=\"color:lime;\">" @ dataRetrieve(weaponDatabase, MLweap_arr_id @ $MLweap_arr) @ "</b>. Build SUCCESS" );}
    else{
    modLoader::Logger::newEntry("error", "[<b style=\"color:red;\"> " @ *dataRetrieve(weaponDatabase, MLweap_arr_longname @ $MLweap_arr) @ " </b>] ID: <b style=\"color:red;\">" @ dataRetrieve(weaponDatabase, MLweap_arr_id @ $mlWeapConflictID) @ "</b>. WARNING. Overwriting ID already assigned to [<b style=\"color:yellow;\"> " @ *dataRetrieve(weaponDatabase, MLweap_arr_longname @ $mlWeapConflictID-1) @ " </b>]" );
    $mlWeapConflict="";}
	
    Nova::weaponInfo1(%shortNameTag,%longNameTag,%smallBMP,%smallDisabledBMP,%largeBMP,%largeDisabledBMP);
	$Net::weapon[$Net::currentWeaponIndex, weaponInfo1] = "'" @ %shortNameTag @ "','" @ %longNameTag @ "','" @ %smallBMP @ "','" @ %smallDisabledBMP @ "','" @ %largeBMP @ "','" @ %largeDisabledBMP @ "'";
	
	//Need those name tags as well!
	$Net::weapon[$Net::currentWeaponIndex, shortNameTagString] = *%shortNameTag;
	$Net::weapon[$Net::currentWeaponIndex, longNameTagString] = *%longNameTag;
}

function weaponInfo2(%techLevel, %combatValue, %mass)
{
	Nova::weaponInfo2(%techLevel, %combatValue, %mass);
	$Net::weapon[$Net::currentWeaponIndex, weaponInfo2] = "'" @ %techLevel @ "','" @ %combatValue @ "','" @ %mass @ "'";
}

function weaponMuzzle(%muzzleShape, %transparentMuzzleShape, %faceCamera, %flashColorRed, %flashColorGreen, %flashColorBlue, %flashRange)
{
	Nova::weaponMuzzle(%muzzleShape, %transparentMuzzleShape, %faceCamera, %flashColorRed, %flashColorGreen, %flashColorBlue, %flashRange);
	$Net::weapon[$Net::currentWeaponIndex, weaponMuzzle] = "'" @ %muzzleShape @ "','" @ %transparentMuzzleShape @ "','" @ %faceCamera @ "','" @ %flashColorRed @ "','" @ %flashColorGreen @ "','" @ %flashColorBlue @ "','" @ %flashRange @ "'";
}

function weaponGeneral(%reloadTime, %lockTime, %converge)
{
	Nova::weaponGeneral(%reloadTime, %lockTime, %converge);
	$Net::weapon[$Net::currentWeaponIndex, weaponGeneral] = "'" @ flt(%reloadTime) @ "','" @ flt(%lockTime) @ "','" @ %converge @ "'";
}

function weaponShot(%fireOffsetX, %fireOffsetY, %fireOffsetZ, %fireTime)
{
	%arrayIndex=1;
	if(!strlen(%fireTime))
	{
		%fireTime = 0;
	}
	Nova::weaponShot(%fireOffsetX, %fireOffsetY, %fireOffsetZ, %fireTime);
	%arrayIndex = 1;
	//Some weapons have more than one shot such as the `Twin Laser`
	//Increment our arrayIndex and input it into the $Net::weapon[$Net::currentWeaponIndex, weaponShot, %arrayIndex]
	while(strlen($Net::weapon[$Net::currentWeaponIndex, weaponShot, %arrayIndex]))
	{
		%arrayIndex++;
	}
	$Net::weapon[$Net::currentWeaponIndex, weaponShot, %arrayIndex] = "'" @ flt(%fireOffsetX) @ "','" @ flt(%fireOffsetY) @ "','" @ flt(%fireOffsetZ) @ "','" @ flt(%fireTime) @ "'";
}

function weaponAmmo(%projectileID, %maxAmmo, %startAmmo, %roundsPerVolley)
{
	Nova::weaponAmmo(%projectileID, %maxAmmo, %startAmmo, %roundsPerVolley);
	$Net::weapon[$Net::currentWeaponIndex, weaponAmmo] = "'" @ %projectileID @ "','" @ %maxAmmo @ "','" @ %startAmmo @ "','" @ %roundsPerVolley @ "'";
}

function weaponEnergy(%projectileID, %chargeLimit, %chargeRate)
{
	Nova::weaponEnergy(%projectileID, %chargeLimit, %chargeRate);
	$Net::weapon[$Net::currentWeaponIndex, weaponEnergy] = "'" @ %projectileID @ "','" @ %chargeLimit @ "','" @ %chargeRate @ "'";
}

//////////////////////////////////////////////////////////////////////////////////
//TURRET CREATION/////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
function newturret(%id)
{
    Nova::newTurret(%id);
    if(!$zzmodloader::haltIDVars){$zzmodloader::turretID[$TIDC++] = %id;}
    datastore(turretDatabase, newturret_ @ $MLturr_arr++ , %id);
	
	$Net::currentTurretIndex = %id;
	$Net::turret[$Net::currentTurretIndex] = true;
	
	$Nova::buildingTurret = true;
	$Nova::buildingHerc = false;
	$Nova::buildingTank = false;
	$Nova::buildingFlyer = false;
	$Nova::buildingDrone = false;
}

function turretBase(%name,%shape,%hulk,%radarCrossSection,%activationDistance,%rotationVelocity,%checkFreq,%teamID,%numHardPoints,%unknown)
{
    Nova::turretBase(%name,%shape,%hulk,%radarCrossSection,%activationDistance,%rotationVelocity,%checkFreq,%teamID,%numHardPoints,%unknown);
	$Net::turret[$Net::currentTurretIndex, turretbase] = "'" @ %name @ "','" @ %shape @ "','" @ %hulk @ "','" @ %radarCrossSection @ "','" @ flt(%activationDistance) @ "','" @ flt(%rotationVelocity) @ "','" @ flt(%checkFreq) @ "','" @ %teamID @ "','" @ %numHardPoints @ "','" @ %unknown @ "'";
    while(%i<=$MLturr_arr)
    {
        if(dataRetrieve(turretDatabase, newturret_ @ $MLturr_arr) != dataRetrieve(turretDatabase, MLturr_arr_id @ %i-1))
        {
            %i++;
        }
        else
        {
            modLoader::Logger::newEntry(normal, "[<b style=\"color:lime;\"> " @ *%name @ " </b>] ID: <b style=\"color:lime;\">" @ dataRetrieve(turretDatabase, newturret_ @ $MLturr_arr) @ "</b>. <b style=\"color:yellow;\">Overwrite</b> SUCCESS" );
            return;
        }
    }
    modLoader::Logger::newEntry(normal, "[<b style=\"color:lime;\"> " @ *%name @ " </b>] ID: <b style=\"color:lime;\">" @ dataRetrieve(turretDatabase, newturret_ @ $MLturr_arr)  @ "</b>. Build SUCCESS" );
}

//This function appears to function identically to newHardPoint although it has far less assembly execution which is strange because this function is used in turret scripts as well as vehicle scripts.
//Some turret scripts use newHardPointExt
//(For now just reroute this function to execute 'newHardPoint' instead)
function newHardPointExt(%id, %size, %side, %parentPart, %offsetX, %offsetY, %offsetZ, %minRotateX, %maxRotateX, %minRotateZ, %maxRotateZ)
{
	newHardPoint(%id, %size, %side, %parentPart, %offsetX, %offsetY, %offsetZ, %minRotateX, %maxRotateX, %minRotateZ, %maxRotateZ);
}

function newHardPoint(%id, %size, %side, %parentPart, %offsetX, %offsetY, %offsetZ, %minRotateX, %maxRotateX, %minRotateZ, %maxRotateZ)
{
	Nova::newHardPoint(%id, %size, %side, %parentPart, %offsetX, %offsetY, %offsetZ, %minRotateX, %maxRotateX, %minRotateZ, %maxRotateZ);

	//This function is used in all vehicle and turret scripts so we must check our $Nova::building* variable
	//to determine what exactly is being built with this function at the time it is being executed
	%arrayIndex=1;
	if($Nova::buildingTurret)
	{
		while(strlen($Net::turret[$Net::currentVehicleID, newHardPoint, %arrayIndex]))
		{
			%arrayIndex++;
		}
		$Net::turret[$Net::currentVehicleID, newHardPoint, %arrayIndex] = "'" @ %id @ "','" @ %size @ "','" @ %side @ "','" @ %parentPart @ "','" @ flt(%offsetX) @ "','" @ flt(%offsetY) @ "','" @ flt(%offsetZ) @ "','" @ flt(%minRotateX) @ "','" @ flt(%maxRotateX) @ "','" @ flt(%minRotateZ) @ "','" @ flt(%maxRotateZ) @ "'";
		$_zzConstructor_hardpointIndex = %arrayIndex;
	}
	
	//HERCS
	else if($Nova::buildingHerc)
	{
		while(strlen($Net::herc[$Net::currentVehicleID, newHardPoint, %arrayIndex]))
		{
			%arrayIndex++;
		}
		$Net::herc[$Net::currentVehicleID, newHardPoint, %arrayIndex] = "'" @ %id @ "','" @ %size @ "','" @ %side @ "','" @ %parentPart @ "','" @ flt(%offsetX) @ "','" @ flt(%offsetY) @ "','" @ flt(%offsetZ) @ "','" @ flt(%minRotateX) @ "','" @ flt(%maxRotateX) @ "','" @ flt(%minRotateZ) @ "','" @ flt(%maxRotateZ) @ "'";
		$_zzConstructor_hardpointIndex = %arrayIndex;
	}
		
	//TANKS
	else if($Nova::buildingTank)
	{
		while(strlen($Net::tank[$Net::currentVehicleID, newHardPoint, %arrayIndex]))
		{
			%arrayIndex++;
		}
		$Net::tank[$Net::currentVehicleID, newHardPoint, %arrayIndex] = "'" @ %id @ "','" @ %size @ "','" @ %side @ "','" @ %parentPart @ "','" @ flt(%offsetX) @ "','" @ flt(%offsetY) @ "','" @ flt(%offsetZ) @ "','" @ flt(%minRotateX) @ "','" @ flt(%maxRotateX) @ "','" @ flt(%minRotateZ) @ "','" @ flt(%maxRotateZ) @ "'";
		$_zzConstructor_hardpointIndex = %arrayIndex;
	}
	//FLYERS
	else if($Nova::buildingFlyer)
	{
		while(strlen($Net::flyer[$Net::currentVehicleID, newHardPoint, %arrayIndex]))
		{
			%arrayIndex++;
		}
 		$Net::flyer[$Net::currentVehicleID, newHardPoint, %arrayIndex] = "'" @ %id @ "','" @ %size @ "','" @ %side @ "','" @ %parentPart @ "','" @ flt(%offsetX) @ "','" @ flt(%offsetY) @ "','" @ flt(%offsetZ) @ "','" @ flt(%minRotateX) @ "','" @ flt(%maxRotateX) @ "','" @ flt(%minRotateZ) @ "','" @ flt(%maxRotateZ) @ "'";
		$_zzConstructor_hardpointIndex = %arrayIndex;
	}
	
	//DRONES
	else if($Nova::buildingDrone)
	{
		while(strlen($Net::drone[$Net::currentVehicleID, newHardPoint, %arrayIndex]))
		{
			%arrayIndex++;
		}
		$Net::drone[$Net::currentVehicleID, newHardPoint, %arrayIndex] = "'" @ %id @ "','" @ %size @ "','" @ %side @ "','" @ %parentPart @ "','" @ flt(%offsetX) @ "','" @ flt(%offsetY) @ "','" @ flt(%offsetZ) @ "','" @ flt(%minRotateX) @ "','" @ flt(%maxRotateX) @ "','" @ flt(%minRotateZ) @ "','" @ flt(%maxRotateZ) @ "'";
		$_zzConstructor_hardpointIndex = %arrayIndex;
	}
}

function hardPointDamage(%sustainableDamage)
{
	Nova::hardPointDamage(%sustainableDamage);
	if(strlen($_zzConstructor_hardpointIndex))
	{
		//HERCS
		if($Nova::buildingHerc)
		{
			$Net::herc[$Net::currentVehicleID, hardPointDamage, $_zzConstructor_hardpointIndex] = %sustainableDamage;
		}
		
		//TANKS
		if($Nova::buildingTank)
		{
			$Net::tank[$Net::currentVehicleID, hardPointDamage, $_zzConstructor_hardpointIndex] = %sustainableDamage;
		}
		
		//FLYERS
		if($Nova::buildingFlyer)
		{
			$Net::flyer[$Net::currentVehicleID, hardPointDamage, $_zzConstructor_hardpointIndex] = %sustainableDamage;
		}
		
		//DRONES
		if($Nova::buildingDrone)
		{
			$Net::drone[$Net::currentVehicleID, hardPointDamage, $_zzConstructor_hardpointIndex] = %sustainableDamage;
		}
	}
}

function hardPointSpecial(%weaponID)
{
	Nova::hardPointSpecial(%weaponID);
	if(strlen($_zzConstructor_hardpointIndex))
	{
		//HERCS
		if($Nova::buildingHerc)
		{
			$Net::herc[$Net::currentVehicleID, hardPointSpecial, $_zzConstructor_hardpointIndex] = %weaponID;
		}
		
		//TANK
		if($Nova::buildingTank)
		{
			$Net::tank[$Net::currentVehicleID, hardPointSpecial, $_zzConstructor_hardpointIndex] = %weaponID;
		}
				
		//FLYER
		if($Nova::buildingFlyer)
		{
			$Net::flyer[$Net::currentVehicleID, hardPointSpecial, $_zzConstructor_hardpointIndex] = %weaponID;
		}
		
		//DRONES
		if($Nova::buildingDrone)
		{
			$Net::drone[$Net::currentVehicleID, hardPointSpecial, $_zzConstructor_hardpointIndex] = %weaponID;
		}
	}
}

function turretAI(%fireAI)
{
	Nova::turretAI(%fireAI);
	$Net::turret[$Net::currentTurretIndex, turretAI] = %fireAI;
}

//////////////////////////////////////////////////////////////////////////////////
//SENSOR CREATION/////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
function newSensor(%id,%sweepTime)
{
    datastore(sensorDatabase, MLsens_arr_id @ $MLsens_arr++ , %id);
    if(modLoader::Database::CheckID(%id,sensor))
    {
        Nova::newSensor(%id,%sweepTime);
		$Nova::totalComponents = Nova::getTotalComponents();
		$Net::currentSensorIndex = %id;
		$Net::sensor[$Net::currentSensorIndex, newSensor] = "'" @ %id @ "','" @ flt(%sweepTime) @ "'";
        $zzclient::components++;
        if(!$zzmodloader::haltIDVars){$zzmodloader::componentID[$CIDC++] = %id;}
    }
}

function sensorInfo1(%shortNameTag,%longNameTag,%smallBMP,%smallDisabledBMP,%largeBMP,%largeDisabledBMP,%descriptionTag)
{
    datastore(sensorDatabase, MLsens_arr_longname @ $MLsens_arr, %longNameTag);
    
    if(!$mlsensConflict){
    modLoader::Logger::newEntry(normal, "[<b style=\"color:lime;\"> " @ *dataRetrieve(sensorDatabase, MLsens_arr_longname @ $MLsens_arr)  @ " </b>] ID: <b style=\"color:lime;\">" @ dataRetrieve(sensorDatabase, MLsens_arr_id @ $MLsens_arr) @ "</b>. Build SUCCESS" );}
    else{
    modLoader::Logger::newEntry("error", "[<b style=\"color:red;\"> " @ *dataRetrieve(weaponDatabase, MLsens_arr_longname @ $MLsens_arr) @ " </b>] ID: <b style=\"color:red;\">" @ dataRetrieve(sensorDatabase, MLsens_arr_id @ $mlsensConflictID) @ "</b>. WARNING. Overwriting ID already assigned to [<b style=\"color:yellow;\"> " @ *dataRetrieve(sensorDatabase, MLsens_arr_longname @ $mlsensConflictID-1) @ " </b>]" );
    $mlsensConflict="";}

    Nova::sensorInfo1(%shortNameTag,%longNameTag,%smallBMP,%smallDisabledBMP,%largeBMP,%largeDisabledBMP,%descriptionTag);
	
	$Net::sensor[$Net::currentSensorIndex, sensorInfo1] = "'" @ %shortNameTag @ "','" @ %longNameTag @ "','" @ %smallBMP @ "','" @ %smallDisabledBMP @ "','" @ %largeBMP @ "','" @ %largeDisabledBMP @ "','" @ %descriptionTag @ "'";
	
	//Need those tag strings as well!
	$Net::sensor[$Net::currentSensorIndex, shortNameTagString] = *%shortNameTag;
	$Net::sensor[$Net::currentSensorIndex, longNameTagString] = *%longNameTag;
	$Net::sensor[$Net::currentSensorIndex, descriptionTagString] = *%descriptionTag;
}

function sensorInfo2(%techLevel, %techBase, %combatValue, %mass, %mountSize, %damage)
{
	Nova::sensorInfo2(%techLevel, %techBase, %combatValue, %mass, %mountSize, %damage);
	$Net::sensor[$Net::currentSensorIndex, sensorInfo2] = "'" @ %techLevel @ "','" @ %techBase @ "','" @ %combatValue @ "','" @ %mass @ "','" @ %mountSize @ "','" @ %damage @ "'";
}

function sensorMode(%active_passive, %base, %range, %shutdown, %squat, %stop, %slow, %fast, %active, %camo, %jamShield, %thermalJam)
{
	Nova::sensorMode(%active_passive, %base, %range, %shutdown, %squat, %stop, %slow, %fast, %active, %camo, %jamShield, %thermalJam);
	if(%active_passive == a)
	{
		$Net::sensor[$Net::currentSensorIndex, sensorModeActive] = "'" @ %active_passive @ "','" @ flt(%base) @ "','" @ flt(%range) @ "','" @ flt(%shutDown) @ "','" @ flt(%squat) @ "','" @ flt(%stop) @ "','" @ flt(%slow) @ "','" @ flt(%fast) @ "','" @ flt(%active) @ "','" @ flt(%camo) @ "','" @ flt(%jamShield) @ "','" @ flt(%thermalJam) @ "'";
	}
	else
	{
		$Net::sensor[$Net::currentSensorIndex, sensorModePassive] = "'" @ %active_passive @ "','" @ flt(%base) @ "','" @ flt(%range) @ "','" @ flt(%shutDown) @ "','" @ flt(%squat) @ "','" @ flt(%stop) @ "','" @ flt(%slow) @ "','" @ flt(%fast) @ "','" @ flt(%active) @ "','" @ flt(%camo) @ "','" @ flt(%jamShield) @ "','" @ flt(%thermalJam) @ "'";
	}
}

//////////////////////////////////////////////////////////////////////////////////
//REACTOR CREATION////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
function newReactor(%id,%output,%battery,%meltdown)
{
    datastore(reactorDatabase, MLreac_arr_id @ $MLreac_arr++ , %id);
    if(modLoader::Database::CheckID(%id,reactor))
    {
        Nova::newReactor(%id,%output,%battery,%meltdown);
		$Nova::totalComponents = Nova::getTotalComponents();
		$Net::currentReactorIndex = %id;
		$Net::reactor[$Net::currentReactorIndex, newReactor] = "'" @ %id @ "','" @ %output @ "','" @ %battery @ "','" @ flt(%meltdown) @ "'";
        $zzclient::components++;
        if(!$zzmodloader::haltIDVars){$zzmodloader::componentID[$CIDC++] = %id;}
    }
}

function reactorInfo1(%shortNameTag,%longNameTag,%smallBMP,%smallDisabledBMP,%largeBMP,%largeDisabledBMP,%descriptionTag)
{
    datastore(reactorDatabase, MLreac_arr_longname @ $MLreac_arr, %longNameTag);
    
    if(!$mlreacConflict){
    modLoader::Logger::newEntry(normal, "[<b style=\"color:lime;\"> " @ *dataRetrieve(reactorDatabase, MLreac_arr_longname @ $MLreac_arr)  @ " </b>] ID: <b style=\"color:lime;\">" @ dataRetrieve(reactorDatabase, MLreac_arr_id @ $MLreac_arr) @ "</b>. Build SUCCESS" );}
    else{
    modLoader::Logger::newEntry("error", "[<b style=\"color:red;\"> " @ *dataRetrieve(reactorDatabase, MLreac_arr_longname @ $MLreac_arr) @ " </b>] ID: <b style=\"color:red;\">" @ dataRetrieve(reactorDatabase, MLreac_arr_id @ $mlreacConflictID) @ "</b>. WARNING. Overwriting ID already assigned to [<b style=\"color:yellow;\"> " @ *dataRetrieve(reactorDatabase, MLreac_arr_longname @ $mlreacConflictID-1) @ " </b>]" );
    $mlreacConflict="";}

    Nova::reactorInfo1(%shortNameTag,%longNameTag,%smallBMP,%smallDisabledBMP,%largeBMP,%largeDisabledBMP,%descriptionTag);
	$Net::reactor[$Net::currentReactorIndex, reactorInfo1] = "'" @ %shortNameTag @ "','" @ %longNameTag @ "','" @ %smallBMP @ "','" @ %smallDisabledBMP @ "','" @ %largeBMP @ "','" @ %largeDisabledBMP @ "','" @ %descriptionTag @ "'";
	$Net::reactor[$Net::currentReactorIndex, shortNameTagString] = *%shortNameTag;
	$Net::reactor[$Net::currentReactorIndex, longNameTagString] = *%longNameTag;
	$Net::reactor[$Net::currentReactorIndex, descriptionTagString] = *%descriptionTag;
}

function reactorInfo2(%techLevel, %techBase, %combatValue, %mass, %mountSize, %damage)
{
	Nova::reactorInfo2(%techLevel, %techBase, %combatValue, %mass, %mountSize, %damage);
	$Net::reactor[$Net::currentReactorIndex, reactorInfo2] = "'" @ %techLevel @ "','" @ %techBase @ "','" @ %combatValue @ "','" @ %mass @ "','" @ %mountSize @ "','" @ %damage @ "'";
}

//////////////////////////////////////////////////////////////////////////////////
//SHIELD CREATION/////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
function newShield(%id,%chargeLimit,%chargeRate,%decayTime,%constant)
{
    datastore(shieldDatabase, MLshld_arr_id @ $MLshld_arr++ , %id);
    if(modLoader::Database::CheckID(%id,shield))
    {

    Nova::newShield(%id,%chargeLimit,%chargeRate,%decayTime,%constant);
	$Nova::totalComponents = Nova::getTotalComponents();
	$Net::currentShieldIndex = %id;
	$Net::shield[$Net::currentShieldIndex, newShield] = "'" @ %id @ "','" @ %chargeLimit @ "','" @ flt(%chargeRate) @ "','" @ flt(%decayTime) @ "','" @ flt(%constant) @ "'";
    $zzclient::components++;
    if(!$zzmodloader::haltIDVars){$zzmodloader::componentID[$CIDC++] = %id;}
    }
}

function shieldInfo1(%shortNameTag,%longNameTag,%smallBMP,%smallDisabledBMP,%largeBMP,%largeDisabledBMP,%descriptionTag)
{
    datastore(shieldDatabase, MLshld_arr_longname @ $MLshld_arr, %longNameTag);
    
    if(!$mlshldConflict){
    modLoader::Logger::newEntry(normal, "[<b style=\"color:lime;\"> " @ *dataRetrieve(shieldDatabase, MLshld_arr_longname @ $MLshld_arr)  @ " </b>] ID: <b style=\"color:lime;\">" @ dataRetrieve(shieldDatabase, MLshld_arr_id @ $MLshld_arr) @ "</b>. Build SUCCESS" );}
    else{
    modLoader::Logger::newEntry("error", "[<b style=\"color:red;\"> " @ *dataRetrieve(shieldDatabase, MLshld_arr_longname @ $MLshld_arr) @ " </b>] ID: <b style=\"color:red;\">" @ dataRetrieve(shieldDatabase, MLshld_arr_id @ $mlshldConflictID) @ "</b>. WARNING. Overwriting ID already assigned to [<b style=\"color:yellow;\"> " @ *dataRetrieve(shieldDatabase, MLshld_arr_longname @ $mlshldConflictID-1) @ " </b>]" );
    $mlshldConflict="";}
	
    Nova::shieldInfo1(%shortNameTag,%longNameTag,%smallBMP,%smallDisabledBMP,%largeBMP,%largeDisabledBMP,%descriptionTag);
	$Net::shield[$Net::currentShieldIndex, shieldInfo1] = "'" @ %shortNameTag @ "','" @ %longNameTag @ "','" @ %smallBMP @ "','" @ %smallDisabledBMP @ "','" @ %largeBMP @ "','" @ %largeDisabledBMP @ "','" @ %descriptionTag @ "'";
	$Net::shield[$Net::currentShieldIndex, shortNameTagString] = *%shortNameTag;
	$Net::shield[$Net::currentShieldIndex, longNameTagString] = *%longNameTag;
	$Net::shield[$Net::currentShieldIndex, descriptionTagString] = *%descriptionTag;
}

function shieldInfo2(%techLevel, %techBase, %combatValue, %mass, %mountSize, %damage)
{
	Nova::shieldInfo2(%techLevel, %techBase, %combatValue, %mass, %mountSize, %damage);
	$Net::shield[$Net::currentShieldIndex, shieldInfo2] = "'" @ %techLevel @ "','" @ %techBase @ "','" @ %combatValue @ "','" @ %mass @ "','" @ %mountSize @ "','" @ %damage @ "'";
}

//////////////////////////////////////////////////////////////////////////////////
//ENGINE CREATION/////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
function newEngine(%id,%velocityRating,%accelerationRating,%accelerationMaxVelocity)
{
    datastore(engineDatabase, MLengi_arr_id @ $MLengi_arr++ , %id);
    if(modLoader::Database::CheckID(%id,engine))
    {
    $zzclient::components++;
	
    Nova::newEngine(%id,%velocityRating,%accelerationRating,%accelerationMaxVelocity);
	$Nova::totalComponents = Nova::getTotalComponents();
	$Net::currentEngineIndex = %id;
	$Net::engine[$Net::currentEngineIndex, newEngine] = "'" @ %id @ "','" @ %velocityRating @ "','" @ %accelerationRating @ "','" @ %accelerationMaxVelocity @ "'";
    if(!$zzmodloader::haltIDVars){$zzmodloader::componentID[$CIDC++] = %id;}
    }
}

function engineInfo1(%shortNameTag,%longNameTag,%smallBMP,%smallDisabledBMP,%largeBMP,%largeDisabledBMP,%descriptionTag)
{
    datastore(engineDatabase, MLengi_arr_longname @ $MLengi_arr, %longNameTag);
    
    if(!$mlengiConflict){
    modLoader::Logger::newEntry(normal, "[<b style=\"color:lime;\"> " @ *dataRetrieve(engineDatabase, MLengi_arr_longname @ $MLengi_arr)  @ " </b>] ID: <b style=\"color:lime;\">" @ dataRetrieve(engineDatabase, MLengi_arr_id @ $MLengi_arr) @ "</b>. Build SUCCESS" );}
    else{
    modLoader::Logger::newEntry("error", "[<b style=\"color:red;\"> " @ *dataRetrieve(engineDatabase, MLengi_arr_longname @ $MLengi_arr) @ " </b>] ID: <b style=\"color:red;\">" @ dataRetrieve(engineDatabase, MLengi_arr_id @ $mlengiConflictID) @ "</b>. WARNING. Overwriting ID already assigned to [<b style=\"color:yellow;\"> " @ *dataRetrieve(engineDatabase, MLengi_arr_longname @ $mlengiConflictID-1) @ " </b>]" );
    $mlengiConflict="";}
	
    Nova::engineInfo1(%shortNameTag,%longNameTag,%smallBMP,%smallDisabledBMP,%largeBMP,%largeDisabledBMP,%descriptionTag);
	$Net::engine[$Net::currentEngineIndex, engineInfo1] = "'" @ %shortNameTag @ "','" @ %longNameTag @ "','" @ %smallBMP @ "','" @ %smallDisabledBMP @ "','" @ %largeBMP @ "','" @ %largeDisabledBMP @ "','" @ %descriptionTag @ "'";
	$Net::engine[$Net::currentEngineIndex, shortNameTagString] = *%shortNameTag;
	$Net::engine[$Net::currentEngineIndex, longNameTagString] = *%longNameTag;
	$Net::engine[$Net::currentEngineIndex, descriptionTagString] = *%descriptionTag;
}

function engineInfo2(%techLevel, %techBase, %combatValue, %mass, %mountSize, %damage)
{
	Nova::engineInfo2(%techLevel, %techBase, %combatValue, %mass, %mountSize, %damage);
	$Net::engine[$Net::currentEngineIndex, engineInfo2] = "'" @ %techLevel @ "','" @ %techBase @ "','" @ %combatValue @ "','" @ %mass @ "','" @ %mountSize @ "','" @ %damage @ "'";
}

//////////////////////////////////////////////////////////////////////////////////
//INT. MOUNT CREATION/////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
function newComputer(%id,%type,%zoom,%scanRange,%leadIndicator,%targetLabels,%targetClosest,%autoTarget)
{
    datastore(mountDatabase, MLmnt_arr_id @ $MLmnt_arr++ , %id);
    if(modLoader::Database::CheckID(%id,mount))
    {
		
    Nova::newComputer(%id,%type,%zoom,%scanRange,%leadIndicator,%targetLabels,%targetClosest,%autoTarget);
	$Nova::totalComponents = Nova::getTotalComponents();
	$Net::currentComputerIndex = %id;
	$Net::currentComponentID = %id;
	$Net::computer[$Net::currentComponentID] = "'" @ %id @ "','" @ %type @ "','" @ flt(%zoom) @ "','" @ %scanRange @ "','" @ %leadIndicator @ "','" @ %targetLabels @ "','" @ %targetClosest @ "','" @ %autoTarget @ "'";
    $zzclient::components++;
    if(!$zzmodloader::haltIDVars){$zzmodloader::componentID[$CIDC++] = %id;}
    }
}

function mountInfo1(%shortNameTag,%longNameTag,%smallBMP,%smallDisabledBMP,%largeBMP,%largeDisabledBMP,%descriptionTag)
{
    datastore(mountDatabase, MLmnt_arr_longname @ $MLmnt_arr, %longNameTag);
    modLoader::Logger::newEntry(normal, "[<b style=\"color:lime;\"> " @ *%longNameTag  @ " </b>] ID: <b style=\"color:lime;\">" @ dataRetrieve(mountDatabase, MLmnt_arr_id @ $MLmnt_arr)  @ "</b>. Build SUCCESS" );

	//$Net::currentComponentID is determined by the current component being built
    Nova::mountInfo1(%shortNameTag,%longNameTag,%smallBMP,%smallDisabledBMP,%largeBMP,%largeDisabledBMP,%descriptionTag);
	
	//TO DO: Remove this check once all function intercepts are implemented
	if($Net::currentComponentID)
	{
		$Net::mountInfo1[$Net::currentComponentID, mountInfo1] = "'" @ %shortNameTag @ "','" @ %longNameTag @ "','" @ %smallBMP @ "','" @ %smallDisabledBMP @ "','" @ %largeBMP @ "','" @ %largeDisabledBMP @ "','" @ %descriptionTag @ "'";
		$Net::mountInfo1[$Net::currentComponentID, shortNameTagString] = *%shortNameTag;
		$Net::mountInfo1[$Net::currentComponentID, longNameTagString] = *%longNameTag;
		$Net::mountInfo1[$Net::currentComponentID, descriptionTagString] = *%descriptionTag;
	}
}

function mountInfo2(%techLevel, %techBase, %combatValue, %mass, %mountSize, %damage)
{
	Nova::mountInfo2(%techLevel, %techBase, %combatValue, %mass, %mountSize, %damage);
	
	//TO DO: Remove this check once all function intercepts are implemented
	if($Net::currentComponentID)
	{
		$Net::mountInfo2[$Net::currentComponentID, mountInfo2] = "'" @ %techLevel @ "','" @ %techBase @ "','" @ %combatValue @ "','" @ %mass @ "','" @ %mountSize @ "','" @ %damage @ "'";
	}
	$Net::currentComponentID = "";
}

function newECM(%id,%type,%rating,%chargeRate,%jammingDistance,%jammingChance)
{
    datastore(mountDatabase, MLmnt_arr_id @ $MLmnt_arr++ , %id);
    if(modLoader::Database::CheckID(%id,mount))
    {
    $zzclient::components++;

    Nova::newECM(%id,%type,%rating,%chargeRate,%jammingDistance,%jammingChance);
	$Nova::totalComponents = Nova::getTotalComponents();
	$Net::currentComponentID = %id;
	$Net::ecm[$Net::currentComponentID] = "'" @ %id @ "','" @ %type @ "','" @ %rating @ "','" @ flt(%chargeRate) @ "','" @ flt(%jammingDistance) @ "','" @ flt(%jammingChance) @ "'";
    if(!$zzmodloader::haltIDVars){$zzmodloader::componentID[$CIDC++] = %id;}
    }
}

function newThermal(%id,%type,%rating,%chargeRate,%jammingDistance,%jammingChance)
{
    datastore(mountDatabase, MLmnt_arr_id @ $MLmnt_arr++ , %id);
    if(modLoader::Database::CheckID(%id,mount))
    {
    $zzclient::components++;

    Nova::newThermal(%id,%type,%rating,%chargeRate,%jammingDistance,%jammingChance);
	$Nova::totalComponents = Nova::getTotalComponents();
	$Net::currentComponentID = %id;
	$Net::thermal[$Net::currentComponentID] = "'" @ %id @ "','" @ %type @ "','" @ %rating @ "','" @ flt(%chargeRate) @ "','" @ flt(%jammingDistance) @ "','" @ flt(%jammingChance) @ "'";
    if(!$zzmodloader::haltIDVars){$zzmodloader::componentID[$CIDC++] = %id;}
    }
}

function newCloak(%id,%rating,%Dmg_Amt_Glitch,%glitchCoef,%Dmg_Amt_Fail,%failCoef)
{
    datastore(mountDatabase, MLmnt_arr_id @ $MLmnt_arr++ , %id);
    if(modLoader::Database::CheckID(%id,mount))
    {
    $zzclient::components++;

    Nova::newCloak(%id,%rating,%Dmg_Amt_Glitch,%glitchCoef,%Dmg_Amt_Fail,%failCoef);
	$Nova::totalComponents = Nova::getTotalComponents();
	$Net::currentComponentID = %id;
	$Net::cloak[$Net::currentComponentID] = "'" @ %id @ "','" @ flt(%rating) @ "','" @ flt(%Dmg_Amt_Glitch) @ "','" @ flt(%glitchCoef) @ "','" @ flt(%Dmg_Amt_Fail) @ "','" @ flt(%failCoef) @ "'";
    if(!$zzmodloader::haltIDVars){$zzmodloader::componentID[$CIDC++] = %id;}
    }
}

function newModulator(%id,%type,%focusBoost)
{
    datastore(mountDatabase, MLmnt_arr_id @ $MLmnt_arr++ , %id);
    if(modLoader::Database::CheckID(%id,mount))
    {
    $zzclient::components++;

    Nova::newModulator(%id,%type,%focsBoost);
	$Nova::totalComponents = Nova::getTotalComponents();
	$Net::currentComponentID = %id;
	$Net::modulator[$Net::currentComponentID] = "'" @ %id @ "','" @ %type @ "','" @ flt(%focusBoost) @ "'";
    if(!$zzmodloader::haltIDVars){$zzmodloader::componentID[$CIDC++] = %id;}
    }
}

function newCapacitor(%id,%type,%capacity,%chargeRate,%popDamage)
{
    datastore(mountDatabase, MLmnt_arr_id @ $MLmnt_arr++ , %id);
    if(modLoader::Database::CheckID(%id,mount))
    {
    $zzclient::components++;

    Nova::newCapacitor(%id,%type,%capacity,%chargeRate,%popDamage);
	$Nova::totalComponents = Nova::getTotalComponents();
	$Net::currentComponentID = %id;
	$Net::capacitor[$Net::currentComponentID] = "'" @ %id @ "','" @ %type @ "','" @ flt(%capacity) @ "','" @ flt(%chargeRate) @ "','" @ flt(%popDamage) @ "'";
    if(!$zzmodloader::haltIDVars){$zzmodloader::componentID[$CIDC++] = %id;}
    }
}

function newAmplifier(%id,%type,%multiplier)
{
    datastore(mountDatabase, MLmnt_arr_id @ $MLmnt_arr++ , %id);
    if(modLoader::Database::CheckID(%id,mount))
    {
    $zzclient::components++;

    Nova::newAmplifier(%id,%type,%multiplier);
	$Nova::totalComponents = Nova::getTotalComponents();
	$Net::currentComponentID = %id;
	$Net::amplifier[$Net::currentComponentID] = "'" @ %id @ "','" @ %type @ "','" @ %multiplier @ "'";
    if(!$zzmodloader::haltIDVars){$zzmodloader::componentID[$CIDC++] = %id;}
    }
}

function newMountable(%id,%type)
{
    datastore(mountDatabase, MLmnt_arr_id @ $MLmnt_arr++ , %id);
    if(modLoader::Database::CheckID(%id,mount))
    {

    Nova::newMountable(%id,%type);
	$Nova::totalComponents = Nova::getTotalComponents();
	$Net::currentComponentID = %id;
	$Net::mountable[$Net::currentComponentID] = "'" @ %id @ "','" @ %type @ "'";
    $zzclient::components++;
    if(!$zzmodloader::haltIDVars){$zzmodloader::componentID[$CIDC++] = %id;}
    }
}

function newBooster(%id,%type,%boostRatio,%capacity,%burnRate,%chargeRate)
{
    datastore(mountDatabase, MLmnt_arr_id @ $MLmnt_arr++ , %id);
    if(modLoader::Database::CheckID(%id,mount))
    {
		
    Nova::newBooster(%id,%type,%boostRatio,%capacity,%burnRate,%chargeRate);
	$Nova::totalComponents = Nova::getTotalComponents();
	$Net::currentComponentID = %id;
	$Net::booster[$Net::currentComponentID] = "'" @ %id @ "','" @ %type @ "','" @ flt(%boostRatio) @ "','" @ flt(%capacity) @ "','" @ flt(%burnRate) @ "','" @ flt(%chargeRate) @ "'";
    $zzclient::components++;
    if(!$zzmodloader::haltIDVars){$zzmodloader::componentID[$CIDC++] = %id;}
    }
}

function newRepair(%id,%type,%repairRate,%energyDrain)
{
    datastore(mountDatabase, MLmnt_arr_id @ $MLmnt_arr++ , %id);
    if(modLoader::Database::CheckID(%id,mount))
    {

    Nova::newRepair(%id,%type,%repairRate,%energyDrain);
	$Nova::totalComponents = Nova::getTotalComponents();
	$Net::currentComponentID = %id;
	$Net::repair[$Net::currentComponentID] = "'" @ %id @ "','" @ %type @ "','" @ %repairRate @ "','" @ %energyDrain @ "'";
    $zzclient::components++;
    if(!$zzmodloader::haltIDVars){$zzmodloader::componentID[$CIDC++] = %id;}
    }
}

function newBattery(%id,%type,%capacity)
{
    datastore(mountDatabase, MLmnt_arr_id @ $MLmnt_arr++ , %id);
    if(modLoader::Database::CheckID(%id,mount))
    {

    Nova::newBattery(%id,%type,%capacity);
	$Nova::totalComponents = Nova::getTotalComponents();
	$Net::currentComponentID = %id;
	$Net::battery[$Net::currentComponentID] = "'" @ %id @ "','" @ %type @ "','" @ %capacity @ "'";
    $zzclient::components++;
    if(!$zzmodloader::haltIDVars){$zzmodloader::componentID[$CIDC++] = %id;}
    }
}

//////////////////////////////////////////////////////////////////////////////////
//ARMOR CREATION//////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////

function newArmor(%id,%type,%conShrug,%elecShrug,%thermShrug,%conEff,%elecEff,%thermEff,%RCS_MOD,%reallocRate,%regenRate)
{
    datastore(armorDatabase, MLarmr_arr_id @ $MLarmr_arr++ , %id);
    if(modLoader::Database::CheckID(%id,armor))
    {

    Nova::newArmor(%id,%type,%conShrug,%elecShrug,%thermShrug,%conEff,%elecEff,%thermEff,%RCS_MOD,%reallocRate,%regenRate);
	$Nova::totalComponents = Nova::getTotalComponents();
	$Net::currentArmorIndex = %id;
	$Net::armor[$Net::currentArmorIndex, newArmor] = "'" @ %id @ "','" @ %type @ "','" @ flt(conShrug) @ "','" @ flt(%elecShrug) @ "','" @ flt(%thermShrug) @ "','" @ flt(%conEff) @ "','" @ flt(%elecEff) @ "','" @ flt(%thermEff) @ "','" @ flt(%RCS_MOD) @ "','" @ flt(%reallocRate) @ "','" @ flt(%regenRate) @ "'";
    $zzclient::components++;
    if(!$zzmodloader::haltIDVars){$zzmodloader::componentID[$CIDC++] = %id;}
    }
}

function armorInfo1(%shortNameTag,%longNameTag,%smallBMP,%smallDisabledBMP,%largeBMP,%largeDisabledBMP,%descriptionTag)
{
    datastore(armorDatabase, MLarmr_arr_longname @ $MLarmr_arr, %longNameTag);
    
    if(!$mlarmrConflict){
    modLoader::Logger::newEntry(normal, "[<b style=\"color:lime;\"> " @ *dataRetrieve(armorDatabase, MLarmr_arr_longname @ $MLarmr_arr)  @ " </b>] ID: <b style=\"color:lime;\">" @ dataRetrieve(armorDatabase, MLarmr_arr_id @ $MLarmr_arr) @ "</b>. Build SUCCESS" );}
    else{
    modLoader::Logger::newEntry("error", "[<b style=\"color:red;\"> " @ *dataRetrieve(armorDatabase, MLarmr_arr_longname @ $MLarmr_arr) @ " </b>] ID: <b style=\"color:red;\">" @ dataRetrieve(armorDatabase, MLarmr_arr_id @ $mlarmrConflictID) @ "</b>. WARNING. Overwriting ID already assigned to [<b style=\"color:yellow;\"> " @ *dataRetrieve(armorDatabase, MLarmr_arr_longname @ $mlarmrConflictID-1) @ " </b>]" );
    $mlarmrConflict="";}
	

    Nova::armorInfo1(%shortNameTag,%longNameTag,%smallBMP,%smallDisabledBMP,%largeBMP,%largeDisabledBMP,%descriptionTag);
	$Net::armor[$Net::currentArmorIndex, armorInfo1] = "'" @ %shortNameTag @ "','" @ %longNameTag @ "','" @ %smallBMP @ "','" @ %smallDisabledBMP @ "','" @ %largeBMP @ "','" @ %largeDisabledBMP @ "','" @ %descriptionTag @ "'";
	$Net::armor[$Net::currentArmorIndex, shortNameTagString] = *%shortNameTag;
	$Net::armor[$Net::currentArmorIndex, longNameTagString] = *%longNameTag;
	$Net::armor[$Net::currentArmorIndex, descriptionTagString] = *%descriptionTag;
}

function armorInfo2(%techLevel, %techBase, %combatValue, %mass, %mountSize, %damage)
{
	Nova::armorInfo2(%techLevel, %techBase, %combatValue, %mass, %mountSize, %damage);
	$Net::armor[$Net::currentArmorIndex, armorInfo2] = "'" @ %techLevel @ "','" @ %techBase @ "','" @ %combatValue @ "','" @ %mass @ "','" @ %mountSize @ "','" @ %damage @ "'";
}

function armorInfoSpecial(%projectileID, %shrug, %effective)
{
	Nova::armorInfoSpecial(%projectileID, %shrug, %effective);
	while(strlen($Net::armor[$Net::currentArmorIndex, armorInfoSpecial, %arrayIndex]))
	{
		%arrayIndex++;
	} 	
	$Net::armor[$Net::currentArmorIndex, armorInfoSpecial, %arrayIndex] = "'" @ %projectileID @ "','" @ %shrug @ "','" @ %effective @ "'";
}

//////////////////////////////////////////////////////////////////////////////////
//HERC CREATION///////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
function hercBase(%identityTag,%abbr,%shape,%mass,%maxMass,%RCS,%techLevel,%combatValue)
{
    $zzmodloader::vehicle[$ML_VEH_NAME++,name] = %identityTag;
    vehiclePilotable(true);

    Nova::hercBase(%identityTag,%abbr,%shape,%mass,%maxMass,%RCS,%techLevel,%combatValue);
	$Net::herc[$Net::currentVehicleID, hercBase] = "'" @ %identityTag @ "','" @ %abbr @ "','" @ %shape @ "','" @ flt(%mass) @ "','" @ flt(%maxMass) @ "','" @ flt(%RCS) @ "','" @ flt(%techLevel) @ "','" @ flt(%combatValue) @ "'";
	$Net::herc[$Net::currentVehicleID, identityTagString] = *%identityTag;
    $_cv = *%identityTag;
}

function hercPos(%maxPosAcc, %minPosVel, %maxForPosVel, %maxRevPosVel)
{
	Nova::hercPos(%maxPosAcc, %minPosVel, %maxForPosVel, %maxRevPosVel);
	$Net::herc[$Net::currentVehicleID, hercPos] = "'" @ flt(%maxPosAcc) @ "','" @ flt(%minPosVel) @ "','" @ flt(%maxForPosVel) @ "','" @ flt(%maxRevPosVel) @ "'";
}

function hercRot(%minRotVel, %maxRVSlow, %maxRVFast)
{
	Nova::hercRot(%minRotVel, %maxRVSlow, %maxRVFast);
	$Net::herc[$Net::currentVehicleID, hercRot] = "'" @ %minRotVel @ "','" @ %maxRVSlow @ "','" @ %maxRVFast @ "'";
}

function hercAnim(%toStandVel, %toRunVel, %toFastRunVel, %toFastTurnVel)
{
	Nova::hercAnim(%toStandVel, %toRunVel, %toFastRunVel, %toFastTurnVel);
	$Net::herc[$Net::currentVehicleID, hercAnim] = "'" @ %toStandVel @ "','" @ %toRunVel @ "','" @ %toFastRunVel @ "','" @ %toFastTurnVel @ "'";
}

function hercCpit(%Xoff,%Yoff,%Zoff)
{
    Nova::hercCpit(%Xoff,%Yoff,%Zoff);
	$Net::herc[$Net::currentVehicleID, hercCpit] = "'" @ flt(%Xoff) @ "','" @ flt(%Yoff) @ "','" @ flt(%Zoff) @ "'";
	
    //Save the cockpit postion to a database. We'll need it for the modloader::setCockpitCameraPosition command to reset the camera to its origin
    datastore(cockpitCameraOrigins, $_cv, %Xoff @ "," @ %Yoff @ "," @ %Zoff);
    translucentCockpit();
}

function hercColl(%sphereOffsetX, %sphereOffsetY, %sphereOffsetZ, %sphereRadius)
{
	Nova::hercColl(%sphereOffsetX, %sphereOffsetY, %sphereOffsetZ, %sphereRadius);
	$Net::herc[$Net::currentVehicleID, hercColl] = "'" @ flt(%sphereOffsetX) @ "','" @ flt(%sphereOffsetY) @ "','" @ flt(%sphereOffsetZ) @ "','" @ flt(%sphereRadius) @ "'";
}

function hercAI(%hercFireAI, %targetPartAI, %hercManeuverAI, %hercRetreatAI)
{
	Nova::hercAI(%hercFireAI, %targetPartAI, %hercManeuverAI, %hercRetreatAI);
	$Net::herc[$Net::currentVehicleID, hercAI] = "'" @ %hercFireAI @ "','" @ %targetPartAI @ "','" @ %hercManeuverAI @ "','" @ %hercRetreatAI @ "'";
}

function hercFootprint(%bitmapTag)
{
	Nova::hercFootprint(%bitmapTag);
	$Net::herc[$Net::currentVehicleID, hercFootprint] = %bitmapTag;
}

//////////////////////////////////////////////////////////////////////////////////
//TANK CREATION///////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
function tankBase(%identityTag,%abbr,%shape,%mass,%maxMass,%RCS,%techLevel,%combatValue)
{
    $zzmodloader::vehicle[$ML_VEH_NAME++,name] = %identityTag;
    vehiclePilotable(true);

    Nova::tankBase(%identityTag,%abbr,%shape,%mass,%maxMass,%RCS,%techLevel,%combatValue);
	$Net::tank[$Net::currentVehicleID, tankBase] = "'" @ %identityTag @ "','" @ %abbr @ "','" @ %shape @ "','" @ flt(%mass) @ "','" @ flt(%maxMass) @ "','" @ flt(%RCS) @ "','" @ flt(%techLevel) @ "','" @ flt(%combatValue) @ "'";
	$Net::tank[$Net::currentVehicleID, identityTagString] = *%identityTag;
    $_cv = *%identityTag;
}

function tankPos(%maxPosAcc, %minPosVel, %maxForPosVel, %maxRevPosVel)
{
	Nova::tankPos(%maxPosAcc, %minPosVel, %maxForPosVel, %maxRevPosVel);
	$Net::tank[$Net::currentVehicleID, tankPos] = "'" @ flt(%maxPosAcc) @ "','" @ flt(%minPosVel) @ "','" @ flt(%maxForPosVel) @ "','" @ flt(%maxRevPosVel) @ "'";
}

function tankRot(%minRotVel, %maxRVSlow, %maxRVFast, %maxRVTurret)
{
	Nova::tankRot(%minRotVel, %maxRVSlow, %maxRVFast, %maxRVTurret);
	$Net::tank[$Net::currentVehicleID, tankRot] = "'" @ %minRotVel @ "','" @ %maxRVSlow @ "','" @ %maxRVFast @ "','" @ %maxRVTurret @ "'";
}

function tankCpit(%Xoff,%Yoff,%Zoff)
{
    datastore(cockpitCameraOrigins, $_cv, %Xoff @ "," @ %Yoff @ "," @ %Zoff);

    Nova::tankCpit(%Xoff,%Yoff,%Zoff);
	$Net::tank[$Net::currentVehicleID, tankCpit] = "'" @ flt(%Xoff) @ "','" @ flt(%Yoff) @ "','" @ flt(%Zoff) @ "'";
    translucentCockpit();
}

function tankColl(%sphereOffsetX, %sphereOffsetY, %sphereOffsetZ, %sphereRadius)
{
	Nova::tankColl(%sphereOffsetX, %sphereOffsetY, %sphereOffsetZ, %sphereRadius);
	$Net::tank[$Net::currentVehicleID, tankColl] = "'" @ flt(%sphereOffsetX) @ "','" @ flt(%sphereOffsetY) @ "','" @ flt(%sphereOffsetZ) @ "','" @ flt(%sphereRadius) @ "'";
}

function tankAnim(%treadAnimRotCoeff, %treadAnimPosCoeff)
{
	Nova::tankAnim(%treadAnimRotCoeff, %treadAnimPosCoeff);
	$Net::tank[$Net::currentVehicleID, tankAnim] = "'" @ %treadAnimRotCoeff @ "','" @ %treadAnimPosCoeff @ "'";
}

function tankSound(%soundTag, %hasThrusters)
{
	Nova::tankSound(%soundTag, %hasThrusters);
	$Net::tank[$Net::currentVehicleID, tankSound] = "'" @ %soundTag @ "','" @ %hasThrusters @ "'";
}

function tankAI(%tankFireAI, %targetPartAI, %tankManeuverAI, %tankRetreatAI)
{
	Nova::tankAI(%tankFireAI, %targetPartAI, %tankManeuverAI, %tankRetreatAI);
	$Net::tank[$Net::currentVehicleID, tankAI] = "'" @ %hercFireAI @ "','" @ %targetPartAI @ "','" @ %hercManeuverAI @ "','" @ %hercRetreatAI @ "'";
}

function tankSlide(%slideCoeff)
{
	Nova::tankSlide(%slideCoeff);
	$Net::tank[$Net::currentVehicleID, tankSlide] = "'" @ %slideCoeff @ "'";
}

//////////////////////////////////////////////////////////////////////////////////
//DRONE CREATION//////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
function droneBase(%identityTag,%abbr,%shape,%mass,%maxMass,%RCS,%techLevel,%combatValue)
{
    $zzmodloader::vehicle[$ML_VEH_NAME++,name] = %identityTag;
    vehiclePilotable(true);

    Nova::droneBase(%identityTag,%abbr,%shape,%mass,%maxMass,%RCS,%techLevel,%combatValue);
	$Net::drone[$Net::currentVehicleID, droneBase] = "'" @ %identityTag @ "','" @ %abbr @ "','" @ %shape @ "','" @ flt(%mass) @ "','" @ flt(%maxMass) @ "','" @ flt(%RCS) @ "','" @ flt(%techLevel) @ "','" @ flt(%combatValue) @ "'";
	$Net::drone[$Net::currentVehicleID, identityTagString] = *%identityTag;
    $_cv = *%identityTag;
}

function dronePos(%maxPosAcc, %minPosVel, %maxForPosVel, %maxRevPosVel)
{
	Nova::dronePos(%maxPosAcc, %minPosVel, %maxForPosVel, %maxRevPosVel);
	$Net::drone[$Net::currentVehicleID, dronePos] = "'" @ flt(%maxPosAcc) @ "','" @ flt(%minPosVel) @ "','" @ flt(%maxForPosVel) @ "','" @ flt(%maxRevPosVel) @ "'";
}

function droneRot(%minRotVel, %maxRVSlow, %maxRVFast, %maxRVTurret)
{
	Nova::droneRot(%minRotVel, %maxRVSlow, %maxRVFast, %maxRVTurret);
	$Net::drone[$Net::currentVehicleID, droneRot] = "'" @ %minRotVel @ "','" @ %maxRVSlow @ "','" @ %maxRVFast @ "','" @ %maxRVTurret @ "'";
}

function droneColl(%sphereOffsetX, %sphereOffsetY, %sphereOffsetZ, %sphereRadius)
{
	Nova::droneColl(%sphereOffsetX, %sphereOffsetY, %sphereOffsetZ, %sphereRadius);
	$Net::drone[$Net::currentVehicleID, droneColl] = "'" @ flt(%sphereOffsetX) @ "','" @ flt(%sphereOffsetY) @ "','" @ flt(%sphereOffsetZ) @ "','" @ flt(%sphereRadius) @ "'";
}

function droneAnim(%treadAnimRotCoeff, %treadAnimPosCoeff)
{
	Nova::droneAnim(%treadAnimRotCoeff, %treadAnimPosCoeff);
	$Net::drone[$Net::currentVehicleID, droneAnim] = "'" @ %treadAnimRotCoeff @ "','" @ %treadAnimPosCoeff @ "'";
}

function droneSound(%soundTag, %hasThrusters)
{
	Nova::droneSound(%soundTag, %hasThrusters);
	$Net::drone[$Net::currentVehicleID, droneSound] = "'" @ %soundTag @ "','" @ %hasThrusters @ "'";
}

function droneSlide(%slideCoeff)
{
	Nova::droneSlide(%slideCoeff);
	$Net::drone[$Net::currentVehicleID, droneSlide] = "'" @ %slideCoeff @ "'";
}

function droneExplosion(%bool, %impulseCoeff)
{
	Nova::droneExplosion(%bool, %impulseCoeff);
	$Net::drone[$Net::currentVehicleID, droneExplosion] = "'" @ %bool @ "','" @ %impulseCoeff @ "'";
}


//////////////////////////////////////////////////////////////////////////////////
//FLYER CREATION//////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
function flyerBase(%identityTag,%abbr,%shape,%mass,%maxMass,%RCS,%techLevel,%combatValue)
{
    $zzmodloader::vehicle[$ML_VEH_NAME++,name] = %identityTag;
    vehiclePilotable(true);

    Nova::flyerBase(%identityTag,%abbr,%shape,%mass,%maxMass,%RCS,%techLevel,%combatValue);
	$Net::flyer[$Net::currentVehicleID, flyerBase] = "'" @ %identityTag @ "','" @ %abbr @ "','" @ %shape @ "','" @ flt(%mass) @ "','" @ flt(%maxMass) @ "','" @ flt(%RCS) @ "','" @ flt(%techLevel) @ "','" @ flt(%combatValue) @ "'";
	$Net::flyer[$Net::currentVehicleID, identityTagString] = *%identityTag;
    $_cv = *%identityTag;
}

function flyerPos(%maxPosAcc, %thrustMultiplier, %maxLiftVel, %maxFallVel,	%maxFlyVel,	%fastLean)
{
	Nova::flyerPos(%maxPosAcc, %thrustMultiplier, %maxLiftVel, %maxFallVel,	%maxFlyVel,	%fastLean);
	$Net::flyer[$Net::currentVehicleID, flyerPos] = "'" @ flt(%maxPosAcc) @ "','" @ flt(%thrustMultiplier) @ "','" @ flt(%maxLiftVel) @ "','" @ flt(%maxFallVel) @ "','" @ flt(%maxFlyVel) @ "','" @ flt(%fastLean) @ "'";
}

function flyerRot(%maxRotXVel, %maxRotYVel, %maxRotZVel)
{
	Nova::flyerRot(%maxRotXVel, %maxRotYVel, %maxRotZVel);
	$Net::flyer[$Net::currentVehicleID, flyerRot] = "'" @ %maxRotXVel @ "','" @ %maxRotYVel @ "','" @ %maxRotZVel @ "'";
}

function flyerCpit(%Xoff,%Yoff,%Zoff)
{
    datastore(cockpitCameraOrigins, $_cv, %Xoff @ "," @ %Yoff @ "," @ %Zoff);

    Nova::flyerCpit(%Xoff,%Yoff,%Zoff);
	$Net::flyer[$Net::currentVehicleID, flyerCpit] = "'" @ flt(%Xoff) @ "','" @ flt(%Yoff) @ "','" @ flt(%Zoff) @ "'";
    translucentCockpit();
}

function flyerColl(%sphereOffsetX, %sphereOffsetY, %sphereOffsetZ, %sphereRadius)
{
	Nova::flyerColl(%sphereOffsetX, %sphereOffsetY, %sphereOffsetZ, %sphereRadius);
	$Net::flyer[$Net::currentVehicleID, flyerColl] = "'" @ flt(%sphereOffsetX) @ "','" @ flt(%sphereOffsetY) @ "','" @ flt(%sphereOffsetZ) @ "','" @ flt(%sphereRadius) @ "'";
}

function flyerAI(%flyerAI)
{
	Nova::flyerAI("", "", "flyerManeuver.ai", "");
}

function flyerNav(%maxLean, %maxBank, %taxiRange, %shortRange, %mediumRange)
{
	Nova::flyerNav(%maxLean, %maxBank, %taxiRange, %shortRange, %mediumRange);
	$Net::flyer[$Net::currentVehicleID, flyerNav] = "'" @ flt(%maxLean) @ "','" @ flt(%maxBank) @ "','" @ flt(%taxiRange) @ "','" @ flt(%shortRange) @ "','" @ flt(%mediumRange) @ "'";
}

function flyerSound(%startupSoundTAG, %shutdownSoundTAG, %flyingSoundTAG, %damagedFlyingSoundTAG)
{
	Nova::flyerSound(%startupSoundTAG, %shutdownSoundTAG, %flyingSoundTAG, %damagedFlyingSoundTAG);
	$Net::flyer[$Net::currentVehicleID, flyerSound] = "'" @ %startupSoundTAG @ "','" @ %shutdownSoundTAG @ "','" @ %flyingSoundTAG @ "','" @ %damagedFlyingSoundTAG @ "'";
}

function flyerExhaust(%transparentExhaustShape, %exhaustShape, %numSources)
{
	Nova::flyerExhaust(%transparentExhaustShape, %exhaustShape, %numSources);
	$Net::flyer[$Net::currentVehicleID, flyerExhaust] = "'" @ %transparentExhaustShape @ "','" @ %exhaustShape @ "','" @ %numSources @ "'";
}

function flyerExhaustOffset(%nodeIndex, %xOffset, %yOffset, %zOffset)
{
	Nova::flyerExhaustOffset(%nodeIndex, %xOffset, %yOffset, %zOffset);
	while(strlen($Net::flyer[$Net::currentVehicleID, flyerExhaustOffset, %arrayIndex])){%arrayIndex++;}
	$Net::flyer[$Net::currentVehicleID, flyerExhaustOffset] = "'" @ %nodeIndex @ "','" @ flt(%xOffset) @ "','" @ flt(%yOffset) @ "','" @ flt(%zOffset) @ "'";
}

//////////////////////////////////////////////////////////////////////////////////
//VEHICLE BUILDERS////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////

function newHerc(%id,%techBase)
{
	$Nova::buildingTurret = false;
	$Nova::buildingHerc = true;
	$Nova::buildingTank = false;
	$Nova::buildingFlyer = false;
	$Nova::buildingDrone = false;
	
    Nova::newHerc(%id,%techBase);
	$Nova::totalVehicles = Nova::getTotalVehicles();
	$Net::vehicleID = %id;
	$Net::currentVehicleID = %id;
    $zzclient::vehicles++;
    $zzmodloader::vehicle[$ML_VEH_ID++,id] = %id;
    $zzmodloader::vehicle[$ML_VEH_TYPE++,type] = Herc;
    if(!$zzmodloader::haltIDVars){$zzmodloader::vehicleID[$VIDC++] = %id;}
}

function buildHerc(%id, %techBase, %script)
{
    if(modLoader::Logger::LogVehicle(%id, %script))
    {
        $modloaderLogger::BuildVehicleID[$ml::BuildVehicle, file] = %script;
        $zzmodloader::vehicle[%id,script] = %script;
        newHerc( %id, %techBase );
        vehiclePilotable(true);
        exec( %script );
        %strlen = strlen(%script);
        %str = strAlign(%strlen-3,l,%script);
        modLoader::Logger::newEntry(normal, "[<b style=\"color:lime;\"> " @ %str @ " </b>] ID: <b style=\"color:lime;\">" @ %id @ "</b>. Build SUCCESS" );
    }
}

function newTank(%id,%techBase)
{
	$Nova::buildingTurret = false;
	$Nova::buildingHerc = false;
	$Nova::buildingTank = true;
	$Nova::buildingFlyer = false;
	$Nova::buildingDrone = false;
	
    Nova::newTank(%id,%techBase);
	$Nova::totalVehicles = Nova::getTotalVehicles();
	$Net::vehicleID = %id;
	$Net::currentVehicleID = %id;
    $zzclient::vehicles++;
    $zzmodloader::vehicle[$ML_VEH_ID++,id] = %id;
    $zzmodloader::vehicle[$ML_VEH_TYPE++,type] = Tank;
    if(!$zzmodloader::haltIDVars){$zzmodloader::vehicleID[$VIDC++] = %id;}
}

function buildTank(%id, %techBase, %script)
{
    if(modLoader::Logger::LogVehicle(%id, %script))
    {
        $modloaderLogger::BuildVehicleID[$ml::BuildVehicle, file] = %script;
        $zzmodloader::vehicle[%id,script] = %script;
        newTank( %id, %techBase );
        vehiclePilotable(true);
        exec( %script );
        %strlen = strlen(%script);
        %str = strAlign(%strlen-3,l,%script);
        modLoader::Logger::newEntry(normal, "[<b style=\"color:lime;\"> " @ %str @ " </b>] ID: <b style=\"color:lime;\">" @ %id @ "</b>. Build SUCCESS" );
    }
}

function newDrone(%id)
{
	$Nova::buildingTurret = false;
	$Nova::buildingHerc = false;
	$Nova::buildingTank = false;
	$Nova::buildingFlyer = false;
	$Nova::buildingDrone = true;
	
    Nova::newDrone(%id);
	$Nova::totalVehicles = Nova::getTotalVehicles();
	$Net::vehicleID = %id;
	$Net::currentVehicleID = %id;
    $zzclient::vehicles++;
    $zzmodloader::vehicle[$ML_VEH_ID++,id] = %id;
    $zzmodloader::vehicle[$ML_VEH_TYPE++,type] = Drone;
    if(!$zzmodloader::haltIDVars){$zzmodloader::vehicleID[$VIDC++] = %id;}
}

function buildDrone(%id, %script)
{
    if(modLoader::Logger::LogVehicle(%id, %script))
    {
        $modloaderLogger::BuildVehicleID[$ml::BuildVehicle, file] = %script;
        $zzmodloader::vehicle[%id,script] = %script;
        newDrone( %id );
        vehiclePilotable(true);
        exec( %script );
        %strlen = strlen(%script);
        %str = strAlign(%strlen-3,l,%script);
        modLoader::Logger::newEntry(normal, "[<b style=\"color:lime;\"> " @ %str @ " </b>] ID: <b style=\"color:lime;\">" @ %id @ "</b>. Build SUCCESS" );
    }
}

function newFlyer(%id,%techBase)
{
	$Nova::buildingTurret = false;
	$Nova::buildingHerc = false;
	$Nova::buildingTank = false;
	$Nova::buildingFlyer = true;
	$Nova::buildingDrone = false;
	
    Nova::newFlyer(%id,%techBase);
	$Nova::totalVehicles = Nova::getTotalVehicles();
	$Net::vehicleID = %id;
	$Net::currentVehicleID = %id;
    $zzclient::vehicles++;
    $zzmodloader::vehicle[$ML_VEH_ID++,id] = %id;
    $zzmodloader::vehicle[$ML_VEH_TYPE++,type] = Flyer;
    if(!$zzmodloader::haltIDVars){$zzmodloader::vehicleID[$VIDC++] = %id;}
}

function buildFlyer(%id, %script)
{
    if(modLoader::Logger::LogVehicle(%id, %script))
    {
        $modloaderLogger::BuildVehicleID[$ml::BuildVehicle, file] = %script;
        $zzmodloader::vehicle[%id,script] = %script;
        newFlyer( %id );
        vehiclePilotable(true);
        exec( %script );
        %strlen = strlen(%script);
        %str = strAlign(%strlen-3,l,%script);
        modLoader::Logger::newEntry("normal", "[<b style=\"color:lime;\"> " @ %str @ " </b>] ID: <b style=\"color:lime;\">" @ %id @ "</b>. Build SUCCESS" );
    }
}

function modLoader::Logger::LogVehicle(%id, %script)
{
   $modloaderLogger::BuildVehicleID[$ml::BuildVehicle++] = %id;
   %i=0;
   while(%i<=$ml::BuildVehicle)
   {
       if(%id != $modloaderLogger::BuildVehicleID[%i-1])
       {
           %i++;
       }
       else 
       {
           %strlen = strlen(%script);
           %str = strAlign(%strlen-3,l,%script);
           modLoader::Logger::newEntry("error", "[<b style=\"color:red;\"> " @ %str @ " </b>] ID: <b style=\"color:red;\">" @ %id @ "</b>. Build FAILURE. ID already assigned to [<b style=\"color:yellow;\"> " @ $modloaderLogger::BuildVehicleID[%i-1, file] @ " </b>]" );
           $ml::BuildVehicle--;
           return false;
       }
   }
   return true;
}

function modLoader::Database::CheckID(%id, %type, %a1, %a2, %a3)
{
    return true;
    if(%type == "weapon")
    {
        $modloaderLogger::weaponID[$MLweap_arr] = %id;
        %i=0;
        while(%i<=$MLweap_arr)
        {
            if(%id != dataRetrieve(weaponDatabase, MLweap_arr_id @ %i-1))
            {
                %i++;
            }
            else 
            {
                $mlWeapConflict=true;
                $mlWeapConflictID=%i;
                $MLweap_arr--;
                return false;
            }
        }
    }
    if(%type == "projectile")
    {
        $modloaderLogger::projectileID[$MLproj_arr] = %id;
        %i=0;
        while(%i<=$MLproj_arr)
        {
            if(%id != dataRetrieve(projectileDatabase, MLproj_arr_id @ %i-1))
            {
                %i++;
            }
            else 
            {
                modLoader::Logger::newEntry("error", "[<b style=\"color:red;\"> " @ %a1 @ " </b>] ID: <b style=\"color:red;\">" @ %id @ "</b>. FAILED. ID already assigned to [<b style=\"color:yellow;\"> " @ dataRetrieve(projectileDatabase, MLproj_arr_abbr @ %i-1) @ " </b>]" );
                $MLproj_arr--;
                return false;
            }
        }
    }
    if(%type == "sensor")
    {
        $modloaderLogger::sensorID[$MLsens_arr] = %id;
        %i=0;
        while(%i<=$MLsens_arr)
        {
            if(%id != dataRetrieve(sensorDatabase, MLsens_arr_id @ %i-1))
            {
                %i++;
            }
            else 
            {
                $mlsensConflict=true;
                $mlsensConflictID=%i;
                $MLsens_arr--;
                return false;
            }
        }
    }
    if(%type == "reactor")
    {
        $modloaderLogger::reactorID[$MLreac_arr] = %id;
        %i=0;
        while(%i<=$MLreac_arr)
        {
            if(%id != dataRetrieve(reactorDatabase, MLreac_arr_id @ %i-1))
            {
                %i++;
            }
            else 
            {
                $mlreacConflict=true;
                $mlreacConflictID=%i;
                $MLreac_arr--;
                return false;
            }
        }
    }
    if(%type == "shield")
    {
        $modloaderLogger::shieldID[$MLshld_arr] = %id;
        %i=0;
        while(%i<=$MLshld_arr)
        {
            if(%id != dataRetrieve(shieldDatabase, MLshld_arr_id @ %i-1))
            {
                %i++;
            }
            else 
            {
                $mlshldConflict=true;
                $mlshldConflictID=%i;
                $MLshld_arr--;
                return false;
            }
        }
    }
    if(%type == "engine")
    {
        $modloaderLogger::engineID[$MLengi_arr] = %id;
        %i=0;
        while(%i<=$MLengi_arr)
        {
            if(%id != dataRetrieve(engineDatabase, MLengi_arr_id @ %i-1))
            {
                %i++;
            }
            else 
            {
                $mlengiConflict=true;
                $mlengiConflictID=%i;
                $MLengi_arr--;
                return false;
            }
        }
    }
    if(%type == "mount")
    {
        $modloaderLogger::mountID[$MLmnt_arr] = %id;
        %i=0;
        while(%i<=$MLmnt_arr)
        {
            if(%id != dataRetrieve(mountDatabase, MLmnt_arr_id @ %i-1))
            {
                %i++;
            }
            else 
            {
                $mlmntConflict=true;
                $mlmntConflictID=%i;
                $MLmnt_arr--;
                return false;
            }
        }
    }
    if(%type == "armor")
    {
        $modloaderLogger::mountID[$MLarmr_arr] = %id;
        %i=0;
        while(%i<=$MLarmr_arr)
        {
            if(%id != dataRetrieve(armorDatabase, MLarmr_arr_id @ %i-1))
            {
                %i++;
            }
            else 
            {
                $mlarmrConflict=true;
                $mlarmrConflictID=%i;
                $MLarmr_arr--;
                return false;
            }
        }
    }
    return true;
}

function vehiclePilotable(%bool)
{
	Nova::vehiclePilotable(%bool);
	if($Nova::buildingHerc){$Net::tank[$Net::currentVehicleID, vehiclePilotable] = %bool;}
	if($Nova::buildingTank){$Net::tank[$Net::currentVehicleID, vehiclePilotable] = %bool;}
	if($Nova::buildingFlyer){$Net::flyer[$Net::currentVehicleID, vehiclePilotable] = %bool;}
	if($Nova::buildingDrone){$Net::drone[$Net::currentVehicleID, vehiclePilotable] = %bool;}
}

function vehicleArtillery(%bool)
{
	Nova::vehicleArtillery(%bool);
	if($Nova::buildingTank){$Net::tank[$Net::currentVehicleID, vehicleArtillery] = %bool;}
	if($Nova::buildingDrone){$Net::drone[$Net::currentVehicleID, vehicleArtillery] = %bool;}
}

function translucentCockpit(%bool)
{
	Nova::translucentCockpit(%bool);
}

function newMountPoint(%id, %size, %damageParent, %m1,%m2,%m3,%m4,%m5,%m6,%m7,%m8,%m9,%m10,%m11,%m12)
{
	%arrayIndex=1;
	//Check for existing mount points
	
	if($Nova::buildingHerc){while(strlen($Net::herc[$Net::currentVehicleID, newMountPoint, %arrayIndex])){%arrayIndex++;}}
	if($Nova::buildingTank){while(strlen($Net::tank[$Net::currentVehicleID, newMountPoint, %arrayIndex])){%arrayIndex++;}}
	if($Nova::buildingFlyer){while(strlen($Net::flyer[$Net::currentVehicleID, newMountPoint, %arrayIndex])){%arrayIndex++;}}
	if($Nova::buildingDrone){while(strlen($Net::drone[$Net::currentVehicleID, newMountPoint, %arrayIndex])){%arrayIndex++;}}
	
	if(!strlen(%m2))
	{
		Nova::newMountPoint(%id, %size, %damageParent, %m1);
		if($Nova::buildingHerc){$Net::herc[$Net::currentVehicleID, newMountPoint, %arrayIndex] = "'" @ %id @ "','" @ %size @ "','" @ %damageParent @ "','" @ %m1 @ "'";}
		if($Nova::buildingTank){$Net::tank[$Net::currentVehicleID, newMountPoint, %arrayIndex] = "'" @ %id @ "','" @ %size @ "','" @ %damageParent @ "','" @ %m1 @ "'";}
		if($Nova::buildingFlyer){$Net::flyer[$Net::currentVehicleID, newMountPoint, %arrayIndex] = "'" @ %id @ "','" @ %size @ "','" @ %damageParent @ "','" @ %m1 @ "'";}
		if($Nova::buildingDrone){$Net::drone[$Net::currentVehicleID, newMountPoint, %arrayIndex] = "'" @ %id @ "','" @ %size @ "','" @ %damageParent @ "','" @ %m1 @ "'";}
	}
	else
	{
		Nova::newMountPoint(%id, %size, %damageParent, %m1,%m2,%m3,%m4,%m5,%m6,%m7,%m8,%m9,%m10,%m11,%m12);
		if($Nova::buildingHerc){$Net::herc[$Net::currentVehicleID, newMountPoint, %arrayIndex] = "'" @ %id @ "','" @ %size @ "','" @ %damageParent @ "','" @ %m1 @ "','" @ %m2 @ "','" @ %m3 @ "','" @ %m4 @ "','" @ %m5 @ "','" @ %m6 @ "','" @ %m7 @ "','" @ %m8 @ "','" @ %m9 @ "','" @ %m10 @ "','" @ %m11 @ "','" @ %m12 @ "','" @ %m13 @ "','" @ %m14 @ "','" @ %m15 @ "','" @ %m16 @ "','" @ %m17 @ "','" @ %m18 @ "','" @ %m19 @ "','" @ %m20 @ "','" @ %m21 @ "','" @ %m22 @ "','" @ %m23 @ "','" @ %m24 @ "','" @ %m25 @ "'";}
		if($Nova::buildingTank){$Net::tank[$Net::currentVehicleID, newMountPoint, %arrayIndex] = "'" @ %id @ "','" @ %size @ "','" @ %damageParent @ "','" @ %m1 @ "','" @ %m2 @ "','" @ %m3 @ "','" @ %m4 @ "','" @ %m5 @ "','" @ %m6 @ "','" @ %m7 @ "','" @ %m8 @ "','" @ %m9 @ "','" @ %m10 @ "','" @ %m11 @ "','" @ %m12 @ "','" @ %m13 @ "','" @ %m14 @ "','" @ %m15 @ "','" @ %m16 @ "','" @ %m17 @ "','" @ %m18 @ "','" @ %m19 @ "','" @ %m20 @ "','" @ %m21 @ "','" @ %m22 @ "','" @ %m23 @ "','" @ %m24 @ "','" @ %m25 @ "'";}
		if($Nova::buildingFlyer){$Net::flyer[$Net::currentVehicleID, newMountPoint, %arrayIndex] = "'" @ %id @ "','" @ %size @ "','" @ %damageParent @ "','" @ %m1 @ "','" @ %m2 @ "','" @ %m3 @ "','" @ %m4 @ "','" @ %m5 @ "','" @ %m6 @ "','" @ %m7 @ "','" @ %m8 @ "','" @ %m9 @ "','" @ %m10 @ "','" @ %m11 @ "','" @ %m12 @ "','" @ %m13 @ "','" @ %m14 @ "','" @ %m15 @ "','" @ %m16 @ "','" @ %m17 @ "','" @ %m18 @ "','" @ %m19 @ "','" @ %m20 @ "','" @ %m21 @ "','" @ %m22 @ "','" @ %m23 @ "','" @ %m24 @ "','" @ %m25 @ "'";}
		if($Nova::buildingDrone){$Net::drone[$Net::currentVehicleID, newMountPoint, %arrayIndex] = "'" @ %id @ "','" @ %size @ "','" @ %damageParent @ "','" @ %m1 @ "','" @ %m2 @ "','" @ %m3 @ "','" @ %m4 @ "','" @ %m5 @ "','" @ %m6 @ "','" @ %m7 @ "','" @ %m8 @ "','" @ %m9 @ "','" @ %m10 @ "','" @ %m11 @ "','" @ %m12 @ "','" @ %m13 @ "','" @ %m14 @ "','" @ %m15 @ "','" @ %m16 @ "','" @ %m17 @ "','" @ %m18 @ "','" @ %m19 @ "','" @ %m20 @ "','" @ %m21 @ "','" @ %m22 @ "','" @ %m23 @ "','" @ %m24 @ "','" @ %m25 @ "'";}
	}
}

function newComponent(%id, %type, %parent, %maxDamage, %identityTag)
{
	%arrayIndex=1;
	//Check for existing component entries
	while(strlen($Net::herc[$Net::currentVehicleID, newComponent, %arrayIndex])){%arrayIndex++;} 
	while(strlen($Net::tank[$Net::currentVehicleID, newComponent, %arrayIndex])){%arrayIndex++;} 
	while(strlen($Net::flyer[$Net::currentVehicleID, newComponent, %arrayIndex])){%arrayIndex++;} 
	while(strlen($Net::drone[$Net::currentVehicleID, newComponent, %arrayIndex])){%arrayIndex++;} 
	
	Nova::newComponent(%id, %type, %parent, %maxDamage, %identityTag);
	if($Nova::buildingHerc){$Net::herc[$Net::currentVehicleID, newComponent, %arrayIndex] = "'" @ %id @ "','" @ %type @ "','" @ %parent @ "','" @ %maxDamage @ "','" @ %identityTag @ "'";}
	if($Nova::buildingTank){$Net::tank[$Net::currentVehicleID, newComponent, %arrayIndex] = "'" @ %id @ "','" @ %type @ "','" @ %parent @ "','" @ %maxDamage @ "','" @ %identityTag @ "'";}
	if($Nova::buildingFlyer){$Net::tank[$Net::currentVehicleID, newComponent, %arrayIndex] = "'" @ %id @ "','" @ %type @ "','" @ %parent @ "','" @ %maxDamage @ "','" @ %identityTag @ "'";}
	if($Nova::buildingDrone){$Net::drone[$Net::currentVehicleID, newComponent, %arrayIndex] = "'" @ %id @ "','" @ %type @ "','" @ %parent @ "','" @ %maxDamage @ "','" @ %identityTag @ "'";}
}

function newConfiguration(%containee, %container, %percent)
{
	%arrayIndex=1;
	//Check for existing component entries
	while(strlen($Net::herc[$Net::currentVehicleID, newConfiguration, %arrayIndex])){%arrayIndex++;} 
	while(strlen($Net::tank[$Net::currentVehicleID, newConfiguration, %arrayIndex])){%arrayIndex++;} 
	while(strlen($Net::flyer[$Net::currentVehicleID, newConfiguration, %arrayIndex])){%arrayIndex++;} 
	while(strlen($Net::drone[$Net::currentVehicleID, newConfiguration, %arrayIndex])){%arrayIndex++;} 
	
	Nova::newConfiguration(%containee, %container, %percent);
	if($Nova::buildingHerc){$Net::herc[$Net::currentVehicleID, newConfiguration, %arrayIndex] = "'" @ %containee @ "','" @ %container @ "','" @ flt(%percent) @ "'";}
	if($Nova::buildingTank){$Net::tank[$Net::currentVehicleID, newConfiguration, %arrayIndex] = "'" @ %containee @ "','" @ %container @ "','" @ flt(%percent) @ "'";}
	if($Nova::buildingTank){$Net::flyer[$Net::currentVehicleID, newConfiguration, %arrayIndex] = "'" @ %containee @ "','" @ %container @ "','" @ flt(%percent) @ "'";}
	if($Nova::buildingDrone){$Net::drone[$Net::currentVehicleID, newConfiguration, %arrayIndex] = "'" @ %containee @ "','" @ %container @ "','" @ flt(%percent) @ "'";}
}

function defaultWeapons(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12)
{
	Nova::defaultWeapons(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12);
	if($Nova::buildingHerc){$Net::herc[$Net::currentVehicleID, defaultWeapons] = "'" @ %a1 @ "','" @ %a2 @ "','" @ %a3 @ "','" @ %a4 @ "','" @ %a5 @ "','" @ %a6 @ "','" @ %a7 @ "','" @ %a8 @ "','" @ %a9 @ "','" @ %a10 @ "','" @ %a11 @ "','" @ %a12 @ "'";}
	if($Nova::buildingTank){$Net::tank[$Net::currentVehicleID, defaultWeapons] = "'" @ %a1 @ "','" @ %a2 @ "','" @ %a3 @ "','" @ %a4 @ "','" @ %a5 @ "','" @ %a6 @ "','" @ %a7 @ "','" @ %a8 @ "','" @ %a9 @ "','" @ %a10 @ "','" @ %a11 @ "','" @ %a12 @ "'";}
	if($Nova::buildingFlyer){$Net::flyer[$Net::currentVehicleID, defaultWeapons] = "'" @ %a1 @ "','" @ %a2 @ "','" @ %a3 @ "','" @ %a4 @ "','" @ %a5 @ "','" @ %a6 @ "','" @ %a7 @ "','" @ %a8 @ "','" @ %a9 @ "','" @ %a10 @ "','" @ %a11 @ "','" @ %a12 @ "'";}
	if($Nova::buildingDrone){$Net::drone[$Net::currentVehicleID, defaultWeapons] = "'" @ %a1 @ "','" @ %a2 @ "','" @ %a3 @ "','" @ %a4 @ "','" @ %a5 @ "','" @ %a6 @ "','" @ %a7 @ "','" @ %a8 @ "','" @ %a9 @ "','" @ %a10 @ "','" @ %a11 @ "','" @ %a12 @ "'";}
}

function defaultMountables(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12)
{
	Nova::defaultMountables(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12);
	if($Nova::buildingHerc){$Net::herc[$Net::currentVehicleID, defaultMounts] = "'" @ %a1 @ "','" @ %a2 @ "','" @ %a3 @ "','" @ %a4 @ "','" @ %a5 @ "','" @ %a6 @ "','" @ %a7 @ "','" @ %a8 @ "','" @ %a9 @ "','" @ %a10 @ "','" @ %a11 @ "','" @ %a12 @ "'";}
	if($Nova::buildingTank){$Net::tank[$Net::currentVehicleID, defaultMounts] = "'" @ %a1 @ "','" @ %a2 @ "','" @ %a3 @ "','" @ %a4 @ "','" @ %a5 @ "','" @ %a6 @ "','" @ %a7 @ "','" @ %a8 @ "','" @ %a9 @ "','" @ %a10 @ "','" @ %a11 @ "','" @ %a12 @ "'";}
	if($Nova::buildingTank){$Net::flyer[$Net::currentVehicleID, defaultMounts] = "'" @ %a1 @ "','" @ %a2 @ "','" @ %a3 @ "','" @ %a4 @ "','" @ %a5 @ "','" @ %a6 @ "','" @ %a7 @ "','" @ %a8 @ "','" @ %a9 @ "','" @ %a10 @ "','" @ %a11 @ "','" @ %a12 @ "'";}
	if($Nova::buildingDrone){$Net::drone[$Net::currentVehicleID, defaultMounts] = "'" @ %a1 @ "','" @ %a2 @ "','" @ %a3 @ "','" @ %a4 @ "','" @ %a5 @ "','" @ %a6 @ "','" @ %a7 @ "','" @ %a8 @ "','" @ %a9 @ "','" @ %a10 @ "','" @ %a11 @ "','" @ %a12 @ "'";}
}

//Intercept sfxAddPair as well. We need it in order to properly assign custom sound files to weapon/vehicle tags
function sfxAddPair(%tagID, %soundPrefTag, %fileName)
{
	$Net::soundTag[%tagID] = %tagID;
	$Net::soundTag[%tagID, fileName] = %fileName;
	$Net::soundTag[%tagID, soundPrefTag] = %soundPrefTag;
	Nova::sfxAddPair(%tagID, %soundPrefTag, %fileName);
}
// function cloakInfo1()
// {
	// Nova::cloakInfo1();
// }

// function cloakInfo2()
// {
	// Nova::cloakInfo2();
// }

function Nova::purgeWeaponData()
{
	for(%i=0;%i<=512;%i++)
	{
		$__weaponPurge = true;
		newWeapon(%i);
	}
	$__weaponPurge = false;
}

function Nova::reloadScriptData()
{
	Nova::purgeWeaponData();
	deleteVariables("Net::*");
	exec( "datProjectile.cs" );
	exec( "datWeapon.cs" );
	exec( "datTurret.cs" );
	exec( "datSensor.cs" );
	exec( "datReactor.cs" );
	exec( "datShield.cs" );
	exec( "datEngine.cs" );
	exec( "datIntMounts.cs" );
	exec( "datArmor.cs");
	exec( "Nova_datVehicle.cs" );
	Nova::initConstructors();
}