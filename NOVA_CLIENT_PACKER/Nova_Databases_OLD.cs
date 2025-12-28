//////////////////////////////////////////////////////////////////////////////////
//PROJECTILE CREATION/////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
$MLproj_arr=0;
function newBullet(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12,%a13,%a14,%a15,%a16,%a17,%a18,%a19,%a20,%a21,%a22,%a23,%a24)
{
    //datastore(projectileDatabase, MLproj_arr_id @ $MLproj_arr++ , %a2);
    if(modLoader::Database::CheckID(%a2,projectile,%a1))
    {
        //datastore(projectileDatabase, MLproj_arr_abbr @ $MLproj_arr , %a1);
        if(%a2 > 127)
        {
            modLoader::Logger::newEntry(warn, "[<b style=\"color:orange;\"> " @ %a1 @ " </b>] ID: <b style=\"color:orange;\">" @ %a2 @ "</b>. Build SUCCESS. <b style=\"color:orange;\">ID OUT OF RANGE</b>. (Exceeds 127)" );
            return;
        }
        modLoader::Logger::newEntry(normal, "[<b style=\"color:lime;\"> " @ %a1 @ " </b>] ID: <b style=\"color:lime;\">" @ %a2 @ "</b>. Build SUCCESS" );
        // MLNBullet(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12,%a13,%a14,%a15,%a16,%a17,%a18,%a19,%a20,%a21,%a22,%a23,%a24);
        Nova::newBullet(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12,%a13,%a14,%a15,%a16,%a17,%a18,%a19,%a20,%a21,%a22,%a23,%a24);
        $Net::bulletProjectileData[%a2] = "'" @ %a1 @ "','" @ %a2 @ "','" @ %a3 @ "','" @ %a4 @ "','" @ %a5 @ "','" @ %a6 @ "','" @ %a7 @ "','" @ %a8 @ "','" @ %a9 @ "','" @ %a10 @ "','" @ %a11 @ "','" @ %a12 @ "','" @ %a13 @ "','" @ %a14 @ "','" @ %a15 @ "','" @ %a16 @ "','" @ %a17 @ "','" @ %a18 @ "','" @ %a19 @ "','" @ %a20 @ "','" @ %a21 @ "','" @ %a22 @ "','" @ %a23 @ "','" @ %a24 @ "'";
    }
}

function newMissile(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12,%a13,%a14,%a15,%a16,%a17,%a18,%a19,%a20,%a21,%a22,%a23,%a24,%a25,%a26,%a27,%a28,%a29,%a30,%a31,%a32,%a33,%a34,%a35)
{
    //datastore(projectileDatabase, MLproj_arr_id @ $MLproj_arr++, %a2);
    if(modLoader::Database::CheckID(%a2,projectile,%a1))
    {
        if(%a2 > 127)
        {
            modLoader::Logger::newEntry(warn, "[<b style=\"color:orange;\"> " @ %a1 @ " </b>] ID: <b style=\"color:orange;\">" @ %a2 @ "</b>. Build SUCCESS. <b style=\"color:orange;\">ID OUT OF RANGE</b>. (Exceeds 127)" );
            return;
        }
        modLoader::Logger::newEntry(normal, "[<b style=\"color:lime;\"> " @ %a1 @ " </b>] ID: <b style=\"color:lime;\">" @ %a2 @ "</b>. Build SUCCESS" );
        datastore(projectileDatabase, MLproj_arr_abbr @ $MLproj_arr , %a1);
        // MLNMissile(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12,%a13,%a14,%a15,%a16,%a17,%a18,%a19,%a20,%a21,%a22,%a23,%a24,%a25,%a26,%a27,%a28,%a29,%a30,%a31,%a32,%a33,%a34,%a35);
        Nova::newMissile(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12,%a13,%a14,%a15,%a16,%a17,%a18,%a19,%a20,%a21,%a22,%a23,%a24,%a25,%a26,%a27,%a28,%a29,%a30,%a31,%a32,%a33,%a34,%a35);
        $Net::missileProjectileData[%a2] = "'" @ %a1 @ "','" @ %a2 @ "','" @ %a3 @ "','" @ %a4 @ "','" @ %a5 @ "','" @ %a6 @ "','" @ %a7 @ "','" @ %a8 @ "','" @ %a9 @ "','" @ %a10 @ "','" @ %a11 @ "','" @ %a12 @ "','" @ %a13 @ "','" @ %a14 @ "','" @ %a15 @ "','" @ %a16 @ "','" @ %a17 @ "','" @ %a18 @ "','" @ %a19 @ "','" @ %a20 @ "','" @ %a21 @ "','" @ %a22 @ "','" @ %a23 @ "','" @ %a24 @ "','" @ %a25 @ "','" @ %a26 @ "','" @ %a27 @ "','" @ %a28 @ "','" @ %a29 @ "','" @ %a30 @ "','" @ %a31 @ "','" @ %a32 @ "','" @ %a33 @ "','" @ %a34 @ "','" @ %a35 @ "'";
    }
}

function newEnergy(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12,%a13,%a14,%a15,%a16,%a17,%a18,%a19,%a20,%a21,%a22,%a23,%a24,%a25)
{
    //datastore(projectileDatabase, MLproj_arr_id @ $MLproj_arr++ , %a2);
    if(modLoader::Database::CheckID(%a2,projectile,%a1))
    {
        if(%a2 > 127)
        {
            modLoader::Logger::newEntry(warn, "[<b style=\"color:orange;\"> " @ %a1 @ " </b>] ID: <b style=\"color:orange;\">" @ %a2 @ "</b>. Build SUCCESS. <b style=\"color:orange;\">ID OUT OF RANGE</b>. (Exceeds 127)" );
            return;
        }
        modLoader::Logger::newEntry(normal, "[<b style=\"color:lime;\"> " @ %a1 @ " </b>] ID: <b style=\"color:lime;\">" @ %a2 @ "</b>. Build SUCCESS" );
        datastore(projectileDatabase, MLproj_arr_abbr @ $MLproj_arr , %a1);
        // MLNEnergy(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12,%a13,%a14,%a15,%a16,%a17,%a18,%a19,%a20,%a21,%a22,%a23,%a24,%a25);
        Nova::newEnergy(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12,%a13,%a14,%a15,%a16,%a17,%a18,%a19,%a20,%a21,%a22,%a23,%a24,%a25);
        $Net::energyProjectileData[%a2] = "'" @ %a1 @ "','" @ %a2 @ "','" @ %a3 @ "','" @ %a4 @ "','" @ %a5 @ "','" @ %a6 @ "','" @ %a7 @ "','" @ %a8 @ "','" @ %a9 @ "','" @ %a10 @ "','" @ %a11 @ "','" @ %a12 @ "','" @ %a13 @ "','" @ %a14 @ "','" @ %a15 @ "','" @ %a16 @ "','" @ %a17 @ "','" @ %a18 @ "','" @ %a19 @ "','" @ %a20 @ "','" @ %a21 @ "','" @ %a22 @ "','" @ %a23 @ "','" @ %a24 @ "','" @ %a25 @ "'";
    }
}

function newBeam(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12,%a13,%a14,%a15,%a16,%a17,%a18,%a19,%a20,%a21,%a22,%a23,%a24,%a25,%a26,%a27,%a28,%a29,%a30,%a31,%a32)
{
    //datastore(projectileDatabase, MLproj_arr_id @ $MLproj_arr++ , %a2);
    if(modLoader::Database::CheckID(%a2,projectile,%a1))
    {
        if(%a2 > 127)
        {
            modLoader::Logger::newEntry(warn, "[<b style=\"color:orange;\"> " @ %a1 @ " </b>] ID: <b style=\"color:orange;\">" @ %a2 @ "</b>. Build SUCCESS. <b style=\"color:orange;\">ID OUT OF RANGE</b>. (Exceeds 127)" );
            return;
        }
        modLoader::Logger::newEntry(normal, "[<b style=\"color:lime;\"> " @ %a1 @ " </b>] ID: <b style=\"color:lime;\">" @ %a2 @ "</b>. Build SUCCESS" );
        datastore(projectileDatabase, MLproj_arr_abbr @ $MLproj_arr , %a1);
        // MLNBeam(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12,%a13,%a14,%a15,%a16,%a17,%a18,%a19,%a20,%a21,%a22,%a23,%a24,%a25,%a26,%a27,%a28,%a29,%a30,%a31,%a32,%a33);
        Nova::newBeam(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12,%a13,%a14,%a15,%a16,%a17,%a18,%a19,%a20,%a21,%a22,%a23,%a24,%a25,%a26,%a27,%a28,%a29,%a30,%a31,%a32,%a33);
        $Net::beamProjectileData[%a2] = "'" @ %a1 @ "','" @ %a2 @ "','" @ %a3 @ "','" @ %a4 @ "','" @ %a5 @ "','" @ %a6 @ "','" @ %a7 @ "','" @ %a8 @ "','" @ %a9 @ "','" @ %a10 @ "','" @ %a11 @ "','" @ %a12 @ "','" @ %a13 @ "','" @ %a14 @ "','" @ %a15 @ "','" @ %a16 @ "','" @ %a17 @ "','" @ %a18 @ "','" @ %a19 @ "','" @ %a20 @ "','" @ %a21 @ "','" @ %a22 @ "','" @ %a23 @ "','" @ %a24 @ "','" @ %a25 @ "','" @ %a26 @ "','" @ %a27 @ "','" @ %a28 @ "','" @ %a29 @ "','" @ %a30 @ "','" @ %a31 @ "','" @ %a32 @ "','" @ %a33 @ "'";
    }
}

function newMine(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12,%a13,%a14,%a15,%a16,%a17,%a18,%a19,%a20,%a21,%a22,%a23,%a24,%a25)
{
    //datastore(projectileDatabase, MLproj_arr_id @ $MLproj_arr++ , %a2);
    if(modLoader::Database::CheckID(%a2,projectile,%a1))
    {
        if(%a2 > 127)
        {
            modLoader::Logger::newEntry(warn, "[<b style=\"color:orange;\"> " @ %a1 @ " </b>] ID: <b style=\"color:orange;\">" @ %a2 @ "</b>. Build SUCCESS. <b style=\"color:orange;\">ID OUT OF RANGE</b>. (Exceeds 127)" );
            return;
        }
        modLoader::Logger::newEntry(normal, "[<b style=\"color:lime;\"> " @ %a1 @ " </b>] ID: <b style=\"color:lime;\">" @ %a2 @ "</b>. Build SUCCESS" );
        datastore(projectileDatabase, MLproj_arr_abbr @ $MLproj_arr , %a1);
        // MLNMine(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12,%a13,%a14,%a15,%a16,%a17,%a18,%a19,%a20,%a21,%a22,%a23,%a24,%a25);
        Nova::newMine(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12,%a13,%a14,%a15,%a16,%a17,%a18,%a19,%a20,%a21,%a22,%a23,%a24,%a25);
        $Net::mineProjectileData[%a2] = "'" @ %a1 @ "','" @ %a2 @ "','" @ %a3 @ "','" @ %a4 @ "','" @ %a5 @ "','" @ %a6 @ "','" @ %a7 @ "','" @ %a8 @ "','" @ %a9 @ "','" @ %a10 @ "','" @ %a11 @ "','" @ %a12 @ "','" @ %a13 @ "','" @ %a14 @ "','" @ %a15 @ "','" @ %a16 @ "','" @ %a17 @ "','" @ %a18 @ "','" @ %a19 @ "','" @ %a20 @ "','" @ %a21 @ "','" @ %a22 @ "','" @ %a23 @ "','" @ %a24 @ "','" @ %a25 @ "'";
    }
}

function newBomb(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12,%a13,%a14,%a15,%a16,%a17,%a18,%a19)
{
    //datastore(projectileDatabase, MLproj_arr_id @ $MLproj_arr++ , %a2);
    if(modLoader::Database::CheckID(%a2,projectile,%a1))
    {
        if(%a2 > 127)
        {
            modLoader::Logger::newEntry(warn, "[<b style=\"color:orange;\"> " @ %a1 @ " </b>] ID: <b style=\"color:orange;\">" @ %a2 @ "</b>. Build SUCCESS. <b style=\"color:orange;\">ID OUT OF RANGE</b>. (Exceeds 127)" );
            return;
        }
        modLoader::Logger::newEntry(normal, "[<b style=\"color:lime;\"> " @ %a1 @ " </b>] ID: <b style=\"color:lime;\">" @ %a2 @ "</b>. Build SUCCESS" );
        //datastore(projectileDatabase, MLproj_arr_abbr @ $MLproj_arr , %a1);
        // MLNBomb(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12,%a13,%a14,%a15,%a16,%a17,%a18,%a19);
        Nova::newBomb(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12,%a13,%a14,%a15,%a16,%a17,%a18,%a19);
        $Net::bombProjectileData[%a2] = "'" @ %a1 @ "','" @ %a2 @ "','" @ %a3 @ "','" @ %a4 @ "','" @ %a5 @ "','" @ %a6 @ "','" @ %a7 @ "','" @ %a8 @ "','" @ %a9 @ "','" @ %a10 @ "','" @ %a11 @ "','" @ %a12 @ "','" @ %a13 @ "','" @ %a14 @ "','" @ %a15 @ "','" @ %a16 @ "','" @ %a17 @ "','" @ %a18 @ "','" @ %a19 @ "'";
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
        // MLNweapon(%id,%shape,%mountSize,%soundTag,%Damage,%techBase,%descriptionTag);
        Nova::newWeapon(%id,%shape,%mountSize,%soundTag,%Damage,%techBase,%descriptionTag);
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
    modLoader::Logger::newEntry("error", "[<b style=\"color:red;\"> " @ *dataRetrieve(weaponDatabase, MLweap_arr_longname @ $MLweap_arr) @ " </b>] ID: <b style=\"color:red;\">" @ dataRetrieve(weaponDatabase, MLweap_arr_id @ $mlWeapConflictID) @ "</b>. FAILED. ID already assigned to [<b style=\"color:yellow;\"> " @ *dataRetrieve(weaponDatabase, MLweap_arr_longname @ $mlWeapConflictID-1) @ " </b>]" );
    $mlWeapConflict="";
    return;}
	
    // MLweapInfo1(%shortNameTag,%longNameTag,%smallBMP,%smallDisabledBMP,%largeBMP,%largeDisabledBMP);
    Nova::weaponInfo1(%shortNameTag,%longNameTag,%smallBMP,%smallDisabledBMP,%largeBMP,%largeDisabledBMP);
}

//////////////////////////////////////////////////////////////////////////////////
//TURRET CREATION/////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
function newturret(%id)
{
    // MLNTurret(%id);
    Nova::newTurret(%id);
    if(!$zzmodloader::haltIDVars){$zzmodloader::turretID[$TIDC++] = %id;}
    datastore(turretDatabase, newturret_ @ $MLturr_arr++ , %id);
}

function turretBase(%name,%shape,%hulk,%radarCrossSection,%activationDistance,%rotationVelocity,%checkFreq,%teamID,%numHardPoints,%unknown)
{
    // MLturrBase(%name,%shape,%hulk,%radarCrossSection,%activationDistance,%rotationVelocity,%checkFreq,%teamID,%numHardPoints,%unknown);
    Nova::turretBase(%name,%shape,%hulk,%radarCrossSection,%activationDistance,%rotationVelocity,%checkFreq,%teamID,%numHardPoints,%unknown);
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
//////////////////////////////////////////////////////////////////////////////////
//SENSOR CREATION/////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
function newSensor(%id,%sweepTime)
{
    datastore(sensorDatabase, MLsens_arr_id @ $MLsens_arr++ , %id);
    if(modLoader::Database::CheckID(%id,sensor))
    {
        // MLNSensor(%id,%sweepTime);
        Nova::newSensor(%id,%sweepTime);
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
    modLoader::Logger::newEntry("error", "[<b style=\"color:red;\"> " @ *dataRetrieve(weaponDatabase, MLsens_arr_longname @ $MLsens_arr) @ " </b>] ID: <b style=\"color:red;\">" @ dataRetrieve(sensorDatabase, MLsens_arr_id @ $mlsensConflictID) @ "</b>. FAILED. ID already assigned to [<b style=\"color:yellow;\"> " @ *dataRetrieve(sensorDatabase, MLsens_arr_longname @ $mlsensConflictID-1) @ " </b>]" );
    $mlsensConflict="";
    return;}
	
    // MLsensInfo1(%shortNameTag,%longNameTag,%smallBMP,%smallDisabledBMP,%largeBMP,%largeDisabledBMP,%descriptionTag);
    Nova::sensorInfo1(%shortNameTag,%longNameTag,%smallBMP,%smallDisabledBMP,%largeBMP,%largeDisabledBMP,%descriptionTag);
}
//////////////////////////////////////////////////////////////////////////////////
//REACTOR CREATION////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
function newReactor(%id,%output,%battery,%meltdown)
{
    datastore(reactorDatabase, MLreac_arr_id @ $MLreac_arr++ , %id);
    if(modLoader::Database::CheckID(%id,reactor))
    {
        // MLNReactor(%id,%output,%battery,%meltdown);
        Nova::newReactor(%id,%output,%battery,%meltdown);
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
    modLoader::Logger::newEntry("error", "[<b style=\"color:red;\"> " @ *dataRetrieve(reactorDatabase, MLreac_arr_longname @ $MLreac_arr) @ " </b>] ID: <b style=\"color:red;\">" @ dataRetrieve(reactorDatabase, MLreac_arr_id @ $mlreacConflictID) @ "</b>. FAILED. ID already assigned to [<b style=\"color:yellow;\"> " @ *dataRetrieve(reactorDatabase, MLreac_arr_longname @ $mlreacConflictID-1) @ " </b>]" );
    $mlreacConflict="";
    return;}
    // MLreactInfo1(%shortNameTag,%longNameTag,%smallBMP,%smallDisabledBMP,%largeBMP,%largeDisabledBMP,%descriptionTag);
    Nova::reactorInfo1(%shortNameTag,%longNameTag,%smallBMP,%smallDisabledBMP,%largeBMP,%largeDisabledBMP,%descriptionTag);
}
//////////////////////////////////////////////////////////////////////////////////
//SHIELD CREATION/////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
function newShield(%id,%chargeLimit,%chargeRate,%decayTime,%constant)
{
    datastore(shieldDatabase, MLshld_arr_id @ $MLshld_arr++ , %id);
    if(modLoader::Database::CheckID(%id,shield))
    {
    // MLNShield(%id,%chargeLimit,%chargeRate,%decayTime,%constant);
    Nova::newShield(%id,%chargeLimit,%chargeRate,%decayTime,%constant);
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
    modLoader::Logger::newEntry("error", "[<b style=\"color:red;\"> " @ *dataRetrieve(shieldDatabase, MLshld_arr_longname @ $MLshld_arr) @ " </b>] ID: <b style=\"color:red;\">" @ dataRetrieve(shieldDatabase, MLshld_arr_id @ $mlshldConflictID) @ "</b>. FAILED. ID already assigned to [<b style=\"color:yellow;\"> " @ *dataRetrieve(shieldDatabase, MLshld_arr_longname @ $mlshldConflictID-1) @ " </b>]" );
    $mlshldConflict="";
    return;}
	
    // MLshldInfo1(%shortNameTag,%longNameTag,%smallBMP,%smallDisabledBMP,%largeBMP,%largeDisabledBMP,%descriptionTag);
    Nova::shieldInfo1(%shortNameTag,%longNameTag,%smallBMP,%smallDisabledBMP,%largeBMP,%largeDisabledBMP,%descriptionTag);
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
    // MLNEngine(%id,%velocityRating,%accelerationRating,%accelerationMaxVelocity);
    Nova::newEngine(%id,%velocityRating,%accelerationRating,%accelerationMaxVelocity);
    if(!$zzmodloader::haltIDVars){$zzmodloader::componentID[$CIDC++] = %id;}
    }
}

function engineInfo1(%shortNameTag,%longNameTag,%smallBMP,%smallDisabledBMP,%largeBMP,%largeDisabledBMP,%descriptionTag)
{
    datastore(engineDatabase, MLengi_arr_longname @ $MLengi_arr, %longNameTag);
    
    if(!$mlengiConflict){
    modLoader::Logger::newEntry(normal, "[<b style=\"color:lime;\"> " @ *dataRetrieve(engineDatabase, MLengi_arr_longname @ $MLengi_arr)  @ " </b>] ID: <b style=\"color:lime;\">" @ dataRetrieve(engineDatabase, MLengi_arr_id @ $MLengi_arr) @ "</b>. Build SUCCESS" );}
    else{
    modLoader::Logger::newEntry("error", "[<b style=\"color:red;\"> " @ *dataRetrieve(engineDatabase, MLengi_arr_longname @ $MLengi_arr) @ " </b>] ID: <b style=\"color:red;\">" @ dataRetrieve(engineDatabase, MLengi_arr_id @ $mlengiConflictID) @ "</b>. FAILED. ID already assigned to [<b style=\"color:yellow;\"> " @ *dataRetrieve(engineDatabase, MLengi_arr_longname @ $mlengiConflictID-1) @ " </b>]" );
    $mlengiConflict="";
    return;}
	
    // MLengiInfo1(%shortNameTag,%longNameTag,%smallBMP,%smallDisabledBMP,%largeBMP,%largeDisabledBMP,%descriptionTag);
    Nova::engineInfo1(%shortNameTag,%longNameTag,%smallBMP,%smallDisabledBMP,%largeBMP,%largeDisabledBMP,%descriptionTag);
}
//////////////////////////////////////////////////////////////////////////////////
//INT. MOUNT CREATION/////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
function newComputer(%id,%type,%zoom,%scanRange,%leadIndicator,%targetLabels,%targetClosest,%autoTarget)
{
    datastore(mountDatabase, MLmnt_arr_id @ $MLmnt_arr++ , %id);
    if(modLoader::Database::CheckID(%id,mount))
    {
    // MLNComputer(%id,%type,%zoom,%scanRange,%leadIndicator,%targetLabels,%targetClosest,%autoTarget);
    Nova::newComputer(%id,%type,%zoom,%scanRange,%leadIndicator,%targetLabels,%targetClosest,%autoTarget);
    $zzclient::components++;
    if(!$zzmodloader::haltIDVars){$zzmodloader::componentID[$CIDC++] = %id;}
    }
}

function mountInfo1(%shortNameTag,%longNameTag,%smallBMP,%smallDisabledBMP,%largeBMP,%largeDisabledBMP,%descriptionTag)
{
    datastore(mountDatabase, MLmnt_arr_longname @ $MLmnt_arr, %longNameTag);
    modLoader::Logger::newEntry(normal, "[<b style=\"color:lime;\"> " @ *%longNameTag  @ " </b>] ID: <b style=\"color:lime;\">" @ dataRetrieve(mountDatabase, MLmnt_arr_id @ $MLmnt_arr)  @ "</b>. Build SUCCESS" );
    // MLmntInfo1(%shortNameTag,%longNameTag,%smallBMP,%smallDisabledBMP,%largeBMP,%largeDisabledBMP,%descriptionTag);
    Nova::mountInfo1(%shortNameTag,%longNameTag,%smallBMP,%smallDisabledBMP,%largeBMP,%largeDisabledBMP,%descriptionTag);
}

function newECM(%id,%type,%chargeRate,%jammingDistance,%jammingChance)
{
    datastore(mountDatabase, MLmnt_arr_id @ $MLmnt_arr++ , %id);
    if(modLoader::Database::CheckID(%id,mount))
    {
    $zzclient::components++;
    // MLNECM(%id,%type,%chargeRate,%jammingDistance,%jammingChance);
    Nova::newECM(%id,%type,%chargeRate,%jammingDistance,%jammingChance);
    if(!$zzmodloader::haltIDVars){$zzmodloader::componentID[$CIDC++] = %id;}
    }
}

function newThermal(%id,%type,%chargeRate,%jammingDistance,%jammingChance)
{
    datastore(mountDatabase, MLmnt_arr_id @ $MLmnt_arr++ , %id);
    if(modLoader::Database::CheckID(%id,mount))
    {
    $zzclient::components++;
    // MLNThermal(%id,%type,%chargeRate,%jammingDistance,%jammingChance);
    Nova::newThermal(%id,%type,%chargeRate,%jammingDistance,%jammingChance);
    if(!$zzmodloader::haltIDVars){$zzmodloader::componentID[$CIDC++] = %id;}
    }
}

function newCloak(%id,%rating,%Dmg_Amt_Glitch,%glitchCeef,%Dmg_Amt_Fail,%failCoef)
{
    datastore(mountDatabase, MLmnt_arr_id @ $MLmnt_arr++ , %id);
    if(modLoader::Database::CheckID(%id,mount))
    {
    $zzclient::components++;
    // MLNCloak(%id,%rating,%Dmg_Amt_Glitch,%glitchCeef,%Dmg_Amt_Fail,%failCoef);
    Nova::newCloak(%id,%rating,%Dmg_Amt_Glitch,%glitchCeef,%Dmg_Amt_Fail,%failCoef);
    if(!$zzmodloader::haltIDVars){$zzmodloader::componentID[$CIDC++] = %id;}
    }
}

function newModulator(%id,%type,%focusBoost)
{
    datastore(mountDatabase, MLmnt_arr_id @ $MLmnt_arr++ , %id);
    if(modLoader::Database::CheckID(%id,mount))
    {
    $zzclient::components++;
    // MLNModulator(%id,%type,%focsBoost);
    Nova::newModulator(%id,%type,%focsBoost);
    if(!$zzmodloader::haltIDVars){$zzmodloader::componentID[$CIDC++] = %id;}
    }
}

function newCapacitor(%id,%type,%capacity,%chargeRate,%popDamage)
{
    datastore(mountDatabase, MLmnt_arr_id @ $MLmnt_arr++ , %id);
    if(modLoader::Database::CheckID(%id,mount))
    {
    $zzclient::components++;
    // MLNCapacitor(%id,%type,%capacity,%chargeRate,%popDamage);
    Nova::newCapacitor(%id,%type,%capacity,%chargeRate,%popDamage);
    if(!$zzmodloader::haltIDVars){$zzmodloader::componentID[$CIDC++] = %id;}
    }
}

function newAmplifier(%id,%type,%multiplier)
{
    datastore(mountDatabase, MLmnt_arr_id @ $MLmnt_arr++ , %id);
    if(modLoader::Database::CheckID(%id,mount))
    {
    $zzclient::components++;
    // MLNAmplifier(%id,%type,%multiplier);
    Nova::newAmplifier(%id,%type,%multiplier);
    if(!$zzmodloader::haltIDVars){$zzmodloader::componentID[$CIDC++] = %id;}
    }
}

function newMountable(%id,%type)
{
    datastore(mountDatabase, MLmnt_arr_id @ $MLmnt_arr++ , %id);
    if(modLoader::Database::CheckID(%id,mount))
    {
    // MLNMountable(%id,%type);
    Nova::newMountable(%id,%type);
    $zzclient::components++;
    if(!$zzmodloader::haltIDVars){$zzmodloader::componentID[$CIDC++] = %id;}
    }
}

function newBooster(%id,%type,%boostRatio,%capacity,%burnRate,%chargeRate)
{
    datastore(mountDatabase, MLmnt_arr_id @ $MLmnt_arr++ , %id);
    if(modLoader::Database::CheckID(%id,mount))
    {
    // MLNBooster(%id,%type,%boostRatio,%capacity,%burnRate,%chargeRate);
    Nova::newBooster(%id,%type,%boostRatio,%capacity,%burnRate,%chargeRate);
    $zzclient::components++;
    if(!$zzmodloader::haltIDVars){$zzmodloader::componentID[$CIDC++] = %id;}
    }
}

function newRepair(%id,%type,%repairRate,%energyDrain)
{
    datastore(mountDatabase, MLmnt_arr_id @ $MLmnt_arr++ , %id);
    if(modLoader::Database::CheckID(%id,mount))
    {
    // MLNRepair(%id,%type,%repairRate,%energyDrain);
    Nova::newRepair(%id,%type,%repairRate,%energyDrain);
    $zzclient::components++;
    if(!$zzmodloader::haltIDVars){$zzmodloader::componentID[$CIDC++] = %id;}
    }
}

function newBattery(%id,%type,%capacity)
{
    datastore(mountDatabase, MLmnt_arr_id @ $MLmnt_arr++ , %id);
    if(modLoader::Database::CheckID(%id,mount))
    {
    // MLNBattery(%id,%type,%capacity);
    Nova::newBattery(%id,%type,%capacity);
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
    // MLNArmor(%id,%type,%conShrug,%elecShrug,%thermShrug,%conEff,%elecEff,%thermEff,%RCS_MOD,%reallocRate,%regenRate);
    Nova::newArmor(%id,%type,%conShrug,%elecShrug,%thermShrug,%conEff,%elecEff,%thermEff,%RCS_MOD,%reallocRate,%regenRate);
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
    modLoader::Logger::newEntry("error", "[<b style=\"color:red;\"> " @ *dataRetrieve(armorDatabase, MLarmr_arr_longname @ $MLarmr_arr) @ " </b>] ID: <b style=\"color:red;\">" @ dataRetrieve(armorDatabase, MLarmr_arr_id @ $mlarmrConflictID) @ "</b>. FAILED. ID already assigned to [<b style=\"color:yellow;\"> " @ *dataRetrieve(armorDatabase, MLarmr_arr_longname @ $mlarmrConflictID-1) @ " </b>]" );
    $mlarmrConflict="";
    return;}
	
    // MLarmInfo1(%shortNameTag,%longNameTag,%smallBMP,%smallDisabledBMP,%largeBMP,%largeDisabledBMP,%descriptionTag);
    Nova::armorInfo1(%shortNameTag,%longNameTag,%smallBMP,%smallDisabledBMP,%largeBMP,%largeDisabledBMP,%descriptionTag);
}
//////////////////////////////////////////////////////////////////////////////////
//HERC CREATION///////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
function hercBase(%identityTag,%abbr,%shape,%mass,%maxMass,%RCS,%techLevel,%combatValue)
{
    $zzmodloader::vehicle[$ML_VEH_NAME++,name] = %identityTag;
    vehiclePilotable(true);
    // MLhcBase(%identityTag,%abbr,%shape,%mass,%maxMass,%RCS,%techLevel,%combatValue);
    Nova::hercBase(%identityTag,%abbr,%shape,%mass,%maxMass,%RCS,%techLevel,%combatValue);
    $_cv = *%identityTag;
}

function hercCpit(%Xoff,%Yoff,%Zoff)
{
    // MLhcCpit(%Xoff,%Yoff,%Zoff);
    Nova::hercCpit(%Xoff,%Yoff,%Zoff);
    //Save the cockpit postion to a database. We'll need it for the modloader::setCockpitCameraPosition command to reset the camera to its origin
    datastore(cockpitCameraOrigins, $_cv, %Xoff @ "," @ %Yoff @ "," @ %Zoff);
    translucentCockpit();
}
//////////////////////////////////////////////////////////////////////////////////
//TANK CREATION///////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
function tankBase(%identityTag,%abbr,%shape,%mass,%maxMass,%RCS,%techLevel,%combatValue)
{
    $zzmodloader::vehicle[$ML_VEH_NAME++,name] = %identityTag;
    vehiclePilotable(true);
    // MLtkBase(%identityTag,%abbr,%shape,%mass,%maxMass,%RCS,%techLevel,%combatValue);
    Nova::tankBase(%identityTag,%abbr,%shape,%mass,%maxMass,%RCS,%techLevel,%combatValue);
    $_cv = *%identityTag;
}
function tankCpit(%Xoff,%Yoff,%Zoff)
{
    datastore(cockpitCameraOrigins, $_cv, %Xoff @ "," @ %Yoff @ "," @ %Zoff);
    // MLtkCpit(%Xoff,%Yoff,%Zoff);
    Nova::tankCpit(%Xoff,%Yoff,%Zoff);
    translucentCockpit();
}

//////////////////////////////////////////////////////////////////////////////////
//DRONE CREATION//////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
function DroneBase(%identityTag,%abbr,%shape,%mass,%maxMass,%RCS,%techLevel,%combatValue)
{
    $zzmodloader::vehicle[$ML_VEH_NAME++,name] = %identityTag;
    vehiclePilotable(true);
    // MLDrnBase(%identityTag,%abbr,%shape,%mass,%maxMass,%RCS,%techLevel,%combatValue);
    Nova::droneBase(%identityTag,%abbr,%shape,%mass,%maxMass,%RCS,%techLevel,%combatValue);
    $_cv = *%identityTag;
}

//////////////////////////////////////////////////////////////////////////////////
//FLYER CREATION//////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
function flyerBase(%identityTag,%abbr,%shape,%mass,%maxMass,%RCS,%techLevel,%combatValue)
{
    $zzmodloader::vehicle[$ML_VEH_NAME++,name] = %identityTag;
    vehiclePilotable(true);
    // MLflyBase(%identityTag,%abbr,%shape,%mass,%maxMass,%RCS,%techLevel,%combatValue);
    Nova::flyerBase(%identityTag,%abbr,%shape,%mass,%maxMass,%RCS,%techLevel,%combatValue);
    $_cv = *%identityTag;
}

function flyerCpit(%Xoff,%Yoff,%Zoff)
{
    datastore(cockpitCameraOrigins, $_cv, %Xoff @ "," @ %Yoff @ "," @ %Zoff);
    // MLflyCpit(%Xoff,%Yoff,%Zoff);
    Nova::flyerCpit(%Xoff,%Yoff,%Zoff);
    translucentCockpit();
}
//////////////////////////////////////////////////////////////////////////////////
//VEHICLE BUILDERS////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////

function newHerc(%id,%techBase)
{
    // MLNHerc(%id,%techBase);
    Nova::newHerc(%id,%techBase);
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
        vehiclePilotable(true);
        newHerc( %id, %techBase );
        exec( %script );
        %strlen = strlen(%script);
        %str = strAlign(%strlen-3,l,%script);
        modLoader::Logger::newEntry(normal, "[<b style=\"color:lime;\"> " @ %str @ " </b>] ID: <b style=\"color:lime;\">" @ %id @ "</b>. Build SUCCESS" );
    }
}

function newTank(%id,%techBase)
{
    // MLNTank(%id,%techBase);
    Nova::newTank(%id,%techBase);
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
        vehiclePilotable(true);
        newTank( %id, %techBase );
        exec( %script );
        %strlen = strlen(%script);
        %str = strAlign(%strlen-3,l,%script);
        modLoader::Logger::newEntry(normal, "[<b style=\"color:lime;\"> " @ %str @ " </b>] ID: <b style=\"color:lime;\">" @ %id @ "</b>. Build SUCCESS" );
    }
}

function newDrone(%id)
{
    // MLNDrone(%id);
    Nova::newDrone(%id);
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
        vehiclePilotable(true);
        newDrone( %id );
        exec( %script );
        %strlen = strlen(%script);
        %str = strAlign(%strlen-3,l,%script);
        modLoader::Logger::newEntry(normal, "[<b style=\"color:lime;\"> " @ %str @ " </b>] ID: <b style=\"color:lime;\">" @ %id @ "</b>. Build SUCCESS" );
    }
}

function newFlyer(%id,%techBase)
{
    // MLNFlyer(%id,%techBase);
    Nova::newFlyer(%id,%techBase);
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
        vehiclePilotable(true);
        newFlyer( %id );
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

function defaultWeapons(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12)
{
	Nova::defaultWeapons(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12);
}

function defaultMountables(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12)
{
	Nova::defaultMountables(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12);
}

function vehiclePilotable(%bool)
{
	Nova::vehiclePilotable(%bool);
}

function vehicleArtillery(%bool)
{
	Nova::vehicleArtillery(%bool);
}

function translucentCockpit(%bool)
{
	Nova::translucentCockpit(%bool);
}

function newHardPoint(%id, %size, %side, %parentPart, %offsetX, %offsetY, %offsetZ, %minRotateX, %maxRotateX, %minRotateZ, %maxRotateZ)
{
	Nova::newHardPoint(%id, %size, %side, %parentPart, %offsetX, %offsetY, %offsetZ, %minRotateX, %maxRotateX, %minRotateZ, %maxRotateZ);
}

function newHardPointExt(%id, %size, %side, %parentPart, %offsetX, %offsetY, %offsetZ, %minRotateX, %maxRotateX, %minRotateZ, %maxRotateZ)
{
	Nova::newHardPointExt(%id, %size, %side, %parentPart, %offsetX, %offsetY, %offsetZ, %minRotateX, %maxRotateX, %minRotateZ, %maxRotateZ);
}

function hardPointSpecial(%weaponID)
{
	Nova::hardPointSpecial(%weaponID);
}

function hardPointDamage(%sustainableDamage)
{
	Nova::hardPointDamage(%sustainableDamage);
}

function newMountPoint(%id, %size, %damageParent, %m1,%m2,%m3,%m4,%m5,%m6,%m7,%m8,%m9,%m10,%m11,%m12,%m13,%m14,%m15,%m16,%m17,%m18,%m19,%m20,%m21,%m22,%m23,%m24,%m25,%m26,%m27,%m28,%m29,%m30)
{
	Nova::newMountPoint(%id, %size, %damageParent, %m1,%m2,%m3,%m4,%m5,%m6,%m7,%m8,%m9,%m10,%m11,%m12,%m13,%m14,%m15,%m16,%m17,%m18,%m19,%m20,%m21,%m22,%m23,%m24,%m25,%m26,%m27,%m28,%m29,%m30);
}

function hercPos(%maxPosAcc, %minPosVel, %maxForPosVel, %maxRevPosVel)
{
	Nova::hercPos(%maxPosAcc, %minPosVel, %maxForPosVel, %maxRevPosVel);
}

function hercRot(%minRotVel, %maxRVSlow, %maxRVFast)
{
	Nova::hercRot(%minRotVel, %maxRVSlow, %maxRVFast);
}

function hercAnim(%toStandVel, %toRunVel, %toFastRunVel, %toFastTurnVel)
{
	Nova::hercAnim(%toStandVel, %toRunVel, %toFastRunVel, %toFastTurnVel);
}

function hercCpit(%X, %Y, %Z)
{
	Nova::hercCpit(%X, %Y, %Z);
}

function hercColl(%sphereOffsetX, %sphereOffsetY, %sphereOffsetZ, %sphereRadius)
{
	Nova::hercColl(%sphereOffsetX, %sphereOffsetY, %sphereOffsetZ, %sphereRadius);
}

function hercAI(%hercFireAI, %targetPartAI, %hercManeuverAI, %hercRetreatAI)
{
	Nova::hercAI(%hercFireAI, %targetPartAI, %hercManeuverAI, %hercRetreatAI);
}

function hercFootprint(%bitmapTAG)
{
	Nova::hercFootprint(%bitmapTAG);
}

function tankPos(%maxPosAcc, %minPosVel, %maxForPosVel, %maxRevPosVel)
{
	Nova::tankPos(%maxPosAcc, %minPosVel, %maxForPosVel, %maxRevPosVel);
}

function tankRot(%minRotVel, %maxRVSlow, %maxRVFast, %maxRVTurret)
{
	Nova::tankRot(%minRotVel, %maxRVSlow, %maxRVFast, %maxRVTurret);
}

function tankCpit(%X, %Y, %Z)
{
	Nova::tankCpit(%X, %Y, %Z);
}

function tankColl(%sphereOffsetX, %sphereOffsetY, %sphereOffsetZ, %sphereRadius)
{
	Nova::tankColl(%sphereOffsetX, %sphereOffsetY, %sphereOffsetZ, %sphereRadius);
}

function tankAnim(%treadAnimRotCoeff, %treadAnimPosCoeff)
{
	Nova::tankAnim(%treadAnimRotCoeff, %treadAnimPosCoeff);
}

function tankSound(%soundTAG, %hasThrusters)
{
	Nova::tankSound(%soundTAG, %hasThrusters);
}

function tankSlide(%slideCoeff)
{
	Nova::tankSlide(%slideCoeff);
}

function tankAI(%tankFireAI, %targetPartAI, %tankManeuverAI, %tankRetreatAI)
{
	Nova::tankAI(%tankFireAI, %targetPartAI, %tankManeuverAI, %tankRetreatAI);
}

function dronePos(%maxPosAcc, %minPosVel, %maxForPosVel, %maxRevPosVel)
{
	Nova::dronePos(%maxPosAcc, %minPosVel, %maxForPosVel, %maxRevPosVel);
}

function droneRot(%minRotVel, %maxRVSlow, %maxRVFast, %maxRVTurret)
{
	Nova::droneRot(%minRotVel, %maxRVSlow, %maxRVFast, %maxRVTurret);
}

function droneColl(%sphereOffsetX, %sphereOffsetY, %sphereOffsetZ, %sphereRadius)
{
	Nova::droneColl(%sphereOffsetX, %sphereOffsetY, %sphereOffsetZ, %sphereRadius);
}

function droneAnim(%treadAnimRotCoeff, %treadAnimPosCoeff)
{
	Nova::droneAnim(%treadAnimRotCoeff, %treadAnimPosCoeff);
}

function droneSound(%soundTAG, %hasThrusters)
{
	Nova::droneSound(%soundTAG, %hasThrusters);
}

function droneSlide(%slideCoeff)
{
	Nova::droneSlide(%slideCoeff);
}

function droneExplosion(%bool, %impulseCoeff)
{
	Nova::droneExplosion(%bool, %impulseCoeff);
}

function flyerPos(%maxPosAcc, %thrustMultiplier, %maxLiftVel, %maxFallVel,	%maxFlyVel,	%fastLean)
{
	Nova::flyerPos(%maxPosAcc, %thrustMultiplier, %maxLiftVel, %maxFallVel,	%maxFlyVel,	%fastLean);
}

function flyerRot(%maxRotXVel, %maxRotYVel, %maxRotZVel)
{
	Nova::flyerRot(%maxRotXVel, %maxRotYVel, %maxRotZVel);
}

function flyerCpit(%X, %Y, %Z)
{
	Nova::flyerCpit(%X, %Y, %Z);
}

function flyerColl(%sphereOffsetX, %sphereOffsetY, %sphereOffsetZ, %sphereRadius)
{
	Nova::flyerColl(%sphereOffsetX, %sphereOffsetY, %sphereOffsetZ, %sphereRadius);
}

function flyerAI(%flyerAI)
{
	Nova::flyerAI("", "", "flyerManeuver.ai", "");
}

function flyerNav(%maxLean, %maxBank, %taxiRange, %shortRange, %mediumRange)
{
	Nova::flyerNav(%maxLean, %maxBank, %taxiRange, %shortRange, %mediumRange);
}

function flyerSound(%startupSoundTAG, %shutdownSoundTAG, %flyingSoundTAG, %damagedFlyingSoundTAG)
{
	Nova::flyerSound(%startupSoundTAG, %shutdownSoundTAG, %flyingSoundTAG, %damagedFlyingSoundTAG);
}

function flyerExhaust(%transparentExhaustShape, %exhaustShape, %numSources)
{
	Nova::flyerExhaust(%transparentExhaustShape, %exhaustShape, %numSources);
}

function turretAI(%fireAI)
{
	Nova::turretAI(%fireAI);
}

function newComponent(%id, %type, %parent, %maxDamage, %identityTAG)
{
	Nova::newComponent(%id, %type, %parent, %maxDamage, %identityTAG);
}

function newConfiguration(%containee, %container, %percent)
{
	Nova::newConfiguration(%containee, %container, %percent);
}

function weaponInfo2(%techLevel, %combatValue, %mass)
{
	Nova::weaponInfo2(%techLevel, %combatValue, %mass);
}

function weaponMuzzle(%muzzleShape, %transparentMuzzleShape, %faceCamera, %flashColorRed, %flashColorGreen, %flashColorBlue, %flashRange)
{
	Nova::weaponMuzzle(%muzzleShape, %transparentMuzzleShape, %faceCamera, %flashColorRed, %flashColorGreen, %flashColorBlue, %flashRange);
}

function weaponGeneral(%reloadTime, %lockTime, %converge)
{
	Nova::weaponGeneral(%reloadTime, %lockTime, %converge);
}

function weaponShot(%fireOffsetX, %fireOffsetY, %fireOffsetZ, %fireTime)
{
	Nova::weaponShot(%fireOffsetX, %fireOffsetY, %fireOffsetZ, %fireTime);
}

function weaponAmmo(%projectileIC, %maxAmmo, %startAmmo, %roundsPerVolley)
{
	Nova::weaponAmmo(%projectileIC, %maxAmmo, %startAmmo, %roundsPerVolley);
}

function weaponEnergy(%projectileId, %chargeLimit, %chargeRate)
{
	Nova::weaponEnergy(%projectileId, %chargeLimit, %chargeRate);
}

function newMountable(%id, %type)
{
	Nova::newMountable(%id, %type);
}

function mountInfo2(%techBase, %techBase, %combatValue, %mass, %mountSize, %damage)
{
	Nova::mountInfo2(%techBase, %techBase, %combatValue, %mass, %mountSize, %damage);
}

function engineInfo2(%techLevel, %techBase, %combatValue, %mass, %mountSize, %damage)
{
	Nova::engineInfo2(%techLevel, %techBase, %combatValue, %mass, %mountSize, %damage);
}

function sensorInfo2(%techLevel, %techBase, %combatValue, %mass, %mountSize, %damage)
{
	Nova::sensorInfo2(%techLevel, %techBase, %combatValue, %mass, %mountSize, %damage);
}

function sensorMode(%active_passive, %base, %range, %shutdown, %squat, %stop, %slow, %fast, %active, %camo, %jamShield, %thermalJam)
{
	Nova::sensorMode(%active_passive, %base, %range, %shutdown, %squat, %stop, %slow, %fast, %active, %camo, %jamShield, %thermalJam);
}

function reactorInfo2(%techLevel, %techBase, %combatValue, %mass, %mountSize, %damage)
{
	Nova::reactorInfo2(%techLevel, %techBase, %combatValue, %mass, %mountSize, %damage);
}

function shieldInfo2(%techLevel, %techBase, %combatValue, %mass, %mountSize, %damage)
{
	Nova::shieldInfo2(%techLevel, %techBase, %combatValue, %mass, %mountSize, %damage);
}

function armorInfo2(%techLevel, %techBase, %combatValue, %mass, %mountSize, %damage)
{
	Nova::armorInfo2(%techLevel, %techBase, %combatValue, %mass, %mountSize, %damage);
}

function armorInfoSpecial(%projectileID, %shrug, %effective)
{
	Nova::armorInfoSpecial(%projectileID, %shrug, %effective);
}

// function cloakInfo1()
// {
	// Nova::cloakInfo1();
// }

// function cloakInfo2()
// {
	// Nova::cloakInfo2();
// }

