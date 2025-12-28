//Avoid multiple datLoad.cs executions. Due to the nature of Modloader, executing datLoad.cs multiple times stacks IDs.
if(dataRetrieve(datLoaded,0) != true)
{
    //RENDERERS
    modLoader::Logger::newEntry(info, "<b style=\"color:Orange;\">&#8595;&#8595;&#8595; Detected Graphic Renderers &#8595;&#8595;&#8595;</b>" );
    modLoader::Logger::newEntry(none, "<b style=\"color:Yellow;\"> " @ $Nova::SoftwareDevice @ "</b>");
    if(isGfxDriver(simcanvas, Glide))
    {
    modLoader::Logger::newEntry(none, "<b style=\"color:Yellow;\">&#9;&#9;&#9;" @ $Nova::GlideDevice @ "</b>");
    }
    modLoader::Logger::newEntry(none, "<b style=\"color:Yellow;\">&#9;&#9;&#9;" @ $Nova::OpenGLDevice @ "</b>");
	if(strlen($pref::Gpu::vendor))
	{
    modLoader::Logger::newEntry(none, "<b style=\"color:DarkSlateGrey;\">&#9;&#9;&#9;[GPU Vendor]: " @ $pref::Gpu::vendor @ "</b>");
	}
    if(strlen($pref::Gpu::driver))
	{
    modLoader::Logger::newEntry(none, "<b style=\"color:DarkSlateGrey;\">&#9;&#9;&#9;[GPU Driver]: " @ $pref::Gpu::driver @ "</b>");
	}
    //PROJECTILES
    modLoader::Logger::newEntry(info, "<b style=\"color:Orange;\">&#8595;&#8595;&#8595; Building Projectiles &#8595;&#8595;&#8595;</b>" );
    exec( "datProjectile.cs" );
    
    //WEAPONS
    modLoader::Logger::newEntry(info, "<b style=\"color:Orange;\">&#8595;&#8595;&#8595; Building Weapons &#8595;&#8595;&#8595;</b>" );
    exec( "datWeapon.cs" );
    
    //TURRETS
    modLoader::Logger::newEntry(info, "<b style=\"color:Orange;\">&#8595;&#8595;&#8595; Building Turrets &#8595;&#8595;&#8595;</b>" );
    exec( "datTurret.cs" );
    
    //SENSORS
    modLoader::Logger::newEntry(info, "<b style=\"color:Orange;\">&#8595;&#8595;&#8595; Building Sensors &#8595;&#8595;&#8595;</b>" );
    exec( "datSensor.cs" );
    
    //REACTORS
    modLoader::Logger::newEntry(info, "<b style=\"color:Orange;\">&#8595;&#8595;&#8595; Building Reactors &#8595;&#8595;&#8595;</b>" );
    exec( "datReactor.cs" );
    
    //SHIELDS
    modLoader::Logger::newEntry(info, "<b style=\"color:Orange;\">&#8595;&#8595;&#8595; Building Shields &#8595;&#8595;&#8595;</b>" );
    exec( "datShield.cs" );
    
    //ENGINES
    modLoader::Logger::newEntry(info, "<b style=\"color:Orange;\">&#8595;&#8595;&#8595; Building Engines &#8595;&#8595;&#8595;</b>" );
    exec( "datEngine.cs" );
    
    //INTERNAL MOUNTS
    modLoader::Logger::newEntry(info, "<b style=\"color:Orange;\">&#8595;&#8595;&#8595; Building Internal Mounts &#8595;&#8595;&#8595;</b>" );
    exec( "datIntMounts.cs" );
    
    //ARMORS
    modLoader::Logger::newEntry(info, "<b style=\"color:Orange;\">&#8595;&#8595;&#8595; Building Armors &#8595;&#8595;&#8595;</b>" );
    exec( "datArmor.cs");
    
    //VEHICLES
    modLoader::Logger::newEntry(info, "<b style=\"color:Orange;\">&#8595;&#8595;&#8595; Building Vehicles &#8595;&#8595;&#8595;</b>" );
    exec( "Nova_datVehicle.cs" );

    exec( "autoexec.cs" );
    exec( "datFormation.cs" );
    exec( "datIRC.cs" );
    dataStore(datLoaded,0,true);
}