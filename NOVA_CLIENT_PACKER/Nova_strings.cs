IDSTR_NOVA_MOD_DIRECTORY         = 135050, "MOD DIRECTORY";
IDSTR_NOVA_REMOVE_MOD            = 135051, "Remove Mod";
IDSTR_NOVA_REQUIRE_MODLOADER     = 135052, "Require Nova Client";
IDSTR_NOVA_UI_DOWNLOAD_SKINS     = 135053, "Download Server Skins";
IDSTR_NOVA_UI_DISABLE_MINIMIZE   = 135054, "Disable Fullscreen Minimize"; //Obsolete
IDSTR_NOVA_UI_LOCK_CURSOR        = 135055, "Lock Game Cursor To Window";
IDSTR_NOVA_UI_VERBOSE_HASHED     = 135056, "Verbose Hashed Bitmaps";
IDSTR_NOVA_UI_RENDER_HASHED      = 135057, "Render Hashed Bitmaps";
IDSTR_NOVA_FOV                   = 135058, "Field of View:\x20";
IDSTR_NOVA_UI_SHOW_FACTORY_VEH   = 135059, "Show Special Factory Vehicles";
IDSTR_NOVA_UI_SPAWN_FADE_IN 	 = 135060, "Spawn Fade-In";
IDSTR_NOVA_UI_NO_MIPMAPPING      = 135061, "Disable Texture Mipmapping";
IDSTR_NOVA_UI_GL_NEAREST         = 135062, "Use GL_NEAREST Filtering";
IDSTR_NOVA_UI_HIRES_SHADOWS      = 135063, "High-Res. Dynamic Shadows";
IDSTR_NOVA_UI_FAR_WEATHER        = 135064, "Far Weather";
IDSTR_NOVA_UI_ENFORCE            = 135065, "Enforce Server Modloader";
IDSTR_NOVA_DEFAULT               = 135066, "Default";
IDSTR_NOVA_UPSCALED              = 135067, "Upscaled";
IDSTR_NOVA_WINDOWED              = 135068, "Windowed";
IDSTR_NOVA_LOWEST_PRIORITY       = 135069, "LOWEST PRIORITY";
IDSTR_NOVA_HIGHEST_PRIORITY      = 135070, "HIGHEST PRIORITY";
IDSTR_NOVA_KEYBINDS              = 135071, "Keybinds";
IDSTR_NOVA_KEYBINDS_DESC         = 135072, "KEYBINDS\n"
    @ "\xBA Open/Close Nova UI Overlay: [Alt N]\n"
    @ "\xBA Toggle Window Frame: [Ctrl Shift W]\n"
    @ "\xBA Edit defaultPrefs.cs: [Ctrl Shift D]\n"
    @ "\xBA Temporarily Reset Upscale Profile: [Ctrl Shift R]";
IDSTR_NOVA_UPSCALE_CONFIG        = 135073, "OpenGL Upscale Profile Config";
IDSTR_NOVA_CLEAR_CACHE           = 135074, "CLEAR SERVER CACHE";
IDSTR_NOVA_MOD_SERVER_ERR        = 135175, "------ [MODLOADER ERROR] ------\n\nCould not bypass ID totals mismatch. Client ID number totals are greater than the servers. This occurs when the client has pre-existing mods loaded that are adding additional vehicle/weapon/component IDs.";
    IDSTR_NOVA_IN_SIM1           = 135176, "Cannot manage mods while";
    IDSTR_NOVA_IN_SIM2           = 135177, "connected to a server.";
IDSTR_NOVA_UI_NO_SHADOWS         = 135178, "Disable Shadows";


///////////////////////////
//Hud scaler text strings//
///////////////////////////
IDSTR_NOVA_HUD_ELEMENT        = 135075, "HUD SCALER:";
IDSTR_NOVA_HUD_MULTIPLIER     = 135076, "Scale Multiplier:";
IDSTR_NOVA_HUD_ELEMENT_SEL    = 135077, "<<Select Hud Element>>";
IDSTR_NOVA_HUD_CHATBOX        = 135078, "Chatbox";
IDSTR_NOVA_HUD_CHATBOX_TEXT   = 135079, "Chatbox Text";
IDSTR_NOVA_HUD_INTERNALS      = 135080, "Internals";
IDSTR_NOVA_HUD_SQUAD_ORDERS   = 135081, "Squad Orders";
IDSTR_NOVA_HUD_DAMAGE_STATUS  = 135082, "Damage Statuses";
IDSTR_NOVA_HUD_WEAPON_DISPLAY = 135083, "Weapon Display";
IDSTR_NOVA_HUD_TIMERS         = 135084, "Sim Timers";
IDSTR_NOVA_HUD_PREF_CONFIG    = 135085, "Hud Pref. Config";
IDSTR_NOVA_HUD_ALL            = 135086, "All Hud Elements";
IDSTR_NOVA_HUD_RADAR          = 135099, "Radar";
IDSTR_NOVA_HUD_TARGET_SYS     = 135100, "Target System";
IDSTR_NOVA_HUD_SHIELDS        = 135101, "Shields";

////////////////////////////////////////
//Upscaler Profile Config text strings//
////////////////////////////////////////
IDSTR_NOVA_UPSCALER_ERR              = 135087, "The upscale configurator must be opened within the shell GUI.";
IDSTR_NOVA_UPSCALER_CONFIG           = 135088, "Upscale Profile Config";
IDSTR_NOVA_UPSCALER_RESOLUTION       = 135089, "Active Resolution";
//IDSTR_NOVA_UPSCALER_CONT_HORIZ_OFFS  = 135090, "GUI Container Horizontal Offset";
IDSTR_NOVA_UPSCALER_RENDER_SCALE     = 135091, "Render Scale";
IDSTR_NOVA_UPSCALER_RENDER_VERT_OFFS = 135092, "Render Horizontal Offset";
IDSTR_NOVA_UPSCALER_RESET_CONFIG     = 135093, "[Control + R] to Reset Config";
IDSTR_NOVA_UPSCALER_SAVE             = 135094, "SAVE PROFILE";
IDSTR_NOVA_MODE_INGAME_ERR           = 135104, "Cannot switch OpenGL modes while in-game";
IDSTR_NOVA_GUI_CONTAINER             = 135102, "GUI Container";
    //Upscale profile error message string (2 parts)
    IDSTR_NOVA_UPSCALER_NO_CONFIG_1  = 135095, "There is no upscale profile for\x20";
    IDSTR_NOVA_UPSCALER_NO_CONFIG_2  = 135096, "To stop recieving this message turn off Opengl upscaling or create an upscale profile for this resolution. The upscale profile creator can be accessed from the Nova UI overlay. (Alt + N)";
    
////////
//MISC//
////////
schedule('IDSTR_PACKET_RATE = 00130205, "PACKET RATE: (4-2000)";',1);

//function hostGUI::onOpen::modLoaderFunc()
//{
//    if(strlen($modloader::mod[1,fileName]))
//    {
//            IDSTR_CN_ERR_BAD_NUM_WEAPONS    = 00130850, "Client and server disagree on the number of IDs.\n" @ $zzmodloader::weaponIDlist @ "\n" @ $zzmodloader::vehicleIDlist @ "\n" @ $zzmodloader::componentIDlist;
//            IDSTR_CN_ERR_BAD_NUM_VEHICLES   = 00130409, "Client and server disagree on the number of IDs.\n" @ $zzmodloader::weaponIDlist @ "\n" @ $zzmodloader::vehicleIDlist @ "\n" @ $zzmodloader::componentIDlist;
//            IDSTR_CN_ERR_BAD_NUM_COMPONENTS = 00130851, "Client and server disagree on the number of IDs.\n" @ $zzmodloader::weaponIDlist @ "\n" @ $zzmodloader::vehicleIDlist @ "\n" @ $zzmodloader::componentIDlist;
//    }
//    else
//    {
//        IDSTR_CN_ERR_BAD_NUM_WEAPONS    = 00130850, "Client and server disagree on the number of weapons available.  Please get the latest version of Starsiege from www.starsiegeplayers.com";
//        IDSTR_CN_ERR_BAD_NUM_COMPONENTS = 00130851, "Client and server disagree on the number of components available.  Please get the latest version of Starsiege from www.starsiegeplayers.com";
//        IDSTR_CN_ERR_BAD_NUM_VEHICLES   = 00130409, "Client and server disagree on the number of vehicle chasses available.  Please get the latest version of Starsiege from www.starsiegeplayers.com";
//    }
//
//}

IDSTR_NOVA_MODLOADER_TERRAIN_TRANS = 135097, "DOWNLOADING TERRAIN...";
IDSTR_NOVA_MODLOADER_MOD_TRANS     = 135098, "DOWNLOADING SERVER MOD";

////////////////
//Recovery GUI//
////////////////
IDSTR_NOVA_RECOVERY_ERR_TEXT = 135103, "THE LAST SESSION OF STARSIEGE TERMINATED UNEXPECTEDLY.\nCONTINUE WITH MODS LOADED?";
IDSTR_NOVA_RECOVERY_RESET    = 135105, "RESET PREFS";


//Additional strings
IDSTR_NOVA_UPSCALED_OPENGL_UPSCALED       = 135106, "Upscaled";
IDSTR_NOVA_UPSCALED_OPENGL_NO_MINIMIZE    = 135107, "";
IDSTR_NOVA_UPSCALED_OPENGL_NO_MINIMIZE_   = 135108, "";
IDSTR_NOVA_UPSCALED_OPENGL_DEFAULT        = 135109, "Default";
IDSTR_NOVA_UPSCALED_OPENGL_RELOAD_NOTIFY  = 135110, "Switching OpenGL modes requires a reload";
IDSTR_NOVA_UI_NOTIFY                      = 135111, "Nova UI: Alt + N";
IDSTR_NOVA_UI_TERRAIN_DETAIL              = 135112, "Terrain Detail";
IDSTR_NOVA_UI_COCKPIT_SHAKE               = 135113, "Disable Cockpit Camera Shake";
IDSTR_NOVA_UI_COLLISION_MESH              = 135114, "Vehicle Collision Mesh";
IDSTR_NOVA_UI_LOW                         = 135115, "Low";
IDSTR_NOVA_UI_HIGH                        = 135116, "High";
IDSTR_NOVA_UI_MOD_DESCRIPTION             = 135117, "Description";
IDSTR_NOVA_UI_SEND_TO_CLIENTS             = 135118, "Send to Clients:";
IDSTR_NOVA_UI_TANK_ALIGNS                 = 135119, "Smooth Tank Alignment:";
IDSTR_NOVA_UI_LABEL_GAMEPLAY              = 135120, "GAMEPLAY";
IDSTR_NOVA_UI_LABEL_VISUALS               = 135121, "VISUALS";
IDSTR_NOVA_UI_LABEL_NETWORKING            = 135122, "NETWORKING";
IDSTR_NOVA_UI_LABEL_MISC                  = 135123, "MISCELLANEOUS";

IDSTR_NOVA_UI_MEDIUM                      = 135124, "Medium";
IDSTR_NOVA_UI_VERY_HIGH                   = 135125, "Very High";
IDSTR_NOVA_UI_VERY_LOW                    = 135126, "Very Low";

IDSTR_NOVA_UI_CURSOR_SENSITIVITY          = 135127, "Cursor Sensitivity";
IDSTR_NOVA_UI_DISABLE_SERVER_MOTD         = 135128, "Disable Master Server MOTD";
IDSTR_NOVA_UI_AUTO_CENTER_MAPVIEW         = 135129, "Auto Offset Hud Map View";
IDSTR_NOVA_UI_SHOW_FPS                    = 135130, "Show Framerate";
IDSTR_NOVA_UI_GRID_LINES                  = 135131, "Disable Mapview Grid Lines";
IDSTR_NOVA_UI_SMOOTH_TERRAIN              = 135132, "Hud Mapview: Smooth Heightmap";
IDSTR_NOVA_UI_SHOW_PING                   = 135133, "Show Server Latency";

IDSTR_NOVA_UI_NOTIFY_TEXT                 = 135134, "Show Nova UI Toggle Toast";

IDSTR_NOVA_UI_PACKET_FRAME                = 135135, "Packet Frame";
IDSTR_NOVA_UI_PACKET_RATE                 = 135136, "Packet Rate";
IDSTR_NOVA_UI_PACKET_SIZE                 = 135137, "Packet Size";

IDSTR_NOVA_HELP_FACTORY_VEHICLES          = 135138, "Determines whether or not Nova will build 'special' factory vehicles so that they show up in the factory vehicle list in the vehicle depots. (I.E drones, flyers, artilleries, cinematic vehicles)";
IDSTR_NOVA_HELP_TERRAIN_DETAIL            = 135139, "Terrain detail determines the overall level of detail of the terrain. Values lower than 25 will greatly impact performance.";
IDSTR_NOVA_HELP_COLLISION_MESH            = 135140, "Toggles display of the collision mesh for vehicles and weapons. The mesh is what projectiles must hit in order to register damage.";
IDSTR_NOVA_HELP_SMOOTH_TANK            	  = 135141, "Slows down the surface alignment of tanks.";
IDSTR_NOVA_HELP_MAP_CENTER            	  = 135142, "Repositions the overhead satellite view to the players vehicle position when the map is opened.";
IDSTR_NOVA_HELP_FAR_WEATHER            	  = 135143, "Increases the render distance and particle density of precipitation.";
IDSTR_NOVA_HELP_HIRES_SHADOWS          	  = 135144, "Increases resolution of dynamic shadows to 128x128 up from 64x64.";
IDSTR_NOVA_HELP_GLNEAREST          	      = 135145, "GL_NEAREST texture filtering does not blur a textures pixels, thus maintaining its sharpness when stretched.";
IDSTR_NOVA_HELP_GRID_LINES         	      = 135146, "Toggles the display of the line grid in map view.";
IDSTR_NOVA_HELP_SMOOTH_TERRAIN         	  = 135147, "Smoothens the elevation rings of the terrain in the map view.";
IDSTR_NOVA_HELP_ENFORCE_NOVA         	  = 135148, "Enforces connecting clients to be Nova clients in order to play on the server. If a connected client is not a Nova client it will be kicked from the server.";
IDSTR_NOVA_HELP_VERBOSE_HASHED		      = 135149, "Echos hashed phoenix bitmaps detected by the TGA image replacer.";
IDSTR_NOVA_HELP_RENDER_HASHED		      = 135150, "Renders hashed phoenix bitmap surfaces which are detected by the TGA image replacer.";
IDSTR_NOVA_HELP_PACKETRATE			      = 135151, "A higher packet rate will reduce latency and provide faster updates in regards to objects in a map. Packet rate is capped to the servers set packet rate.";
IDSTR_NOVA_UI_INTRO_SMACKERS		      = 135152, "Play Intro & Credits Videos";
IDSTR_NOVA_UI_COPIED_CLIPBOARD		      = 135153, "Hash Script Copied to Clipboard";
IDSTR_NOVA_UI_HASH_SCRIPT			      = 135154, "Create Hash Script";
IDSTR_NOVA_UI_TEXTURE_NAME			      = 135156, "Texture Name";
IDSTR_NOVA_UI_TEXTURE_DIRECTORY			  = 135157, "Texture Directory";
IDSTR_NOVA_UI_BATCH_GENERATE			  = 135158, "Batch Generate Hashes";
IDSTR_NOVA_UI_ALLOCATE_HASH			  	  = 135159, "Allocate Texture Hasher Dir.";
IDSTR_NOVA_HELP_ALLOCATE_HASH			  = 135160, "Load textures from the texture hasher directory when loading textures in the game.";
IDSTR_NOVA_LABEL_TEXTURE_HASHER			  = 135161, "TEXTURE HASHER";
IDSTR_NOVA_HASHING_TEXTURES			      = 135162, "CALCULATING TEXTURE HASHES";
IDSTR_NOVA_HASHING_TEXTURES_ABORT	      = 135163, "Press 'Escape' to abort the process";
IDSTR_NOVA_UI_WINDOW_SIZE	      		  = 135164, "Update Window Size";
IDSTR_NOVA_HELP_WINDOW_SIZE			      = 135165, "Updates the size of the window if the game window has been moved from one display to another which has differing window scaling set within the operating system.";
IDSTR_NOVA_UI_BAYES_ERROR	      		  = 135166, "The Bayesian Network Editor can only be loaded once per session.";
IDSTR_NOVA_UI_BAYES_EDITOR	      		  = 135167, "Bayesian Network Editor";
IDSTR_NOVA_HELP_HASHER_INCLUDE_SKINS	  = 135168, "When this is enabled the texture hasher will scan the vehicle skins directory instead of the texture hasher directory.";
IDSTR_NOVA_UI_INCLUDE_SKINS	  			  = 135169, "Scan Skins|Faces|Logos Directories";
IDSTR_NOVA_UI_ECHO_CHAT		  			  = 135170, "Echo Chat Messages to Console";
IDSTR_NOVA_UI_FAR_LOOK		  			  = 135171, "Cockpit Far Look";
IDSTR_NOVA_HELP_FAR_LOOK		  		  = 135172, "Lowers the yaw and pitch constraints of the cockpit camera allowing a player to look further to the sides and up and down.";

IDSTR_POLLING_MASTER                      = 130842, "Polling master server %d [%s]";

if($pref::GWC::SIM_FS_MODE == "Upscaled")
{
	//If upscaled, blank out the mode switch text and windowed mode combo box label as switching to actual windowed mode is disabled
    //IDSTR_MODE_SWITCH_INST = 00130175, "WINDOWED MODES ARE DISABLED WHEN USING UPSCALED OPENGL";
    IDSTR_MODE_SWITCH_INST = 00130175, "";
	//IDSTR_WINDOWED_MODE = 00130173, "";
}

IDSTR_NOVA_UI_LENSFLARE		  		      = 135173, "Render Lensflare";
IDSTR_NOVA_UI_CUSTOM_TERRAIN_VIS		  = 135174, "Use Custom Terrain Visbility";
IDSTR_NOVA_HELP_CUSTOM_TERRAIN_VIS		  = 135179, "Use a custom terrain visibility distance instead of the servers.";
IDSTR_NOVA_UI_CUSTOM_TERRAIN_VIS_DIST	  = 135180, "Terrain Visibility Distance";
IDSTR_NOVA_UI_FREE_CAM	  				  = 135181, "Free Cam Mode";
IDSTR_NOVA_HELP_FREE_CAM		  		  = 135182, "Switches to a free camera that can fly around the map.";
IDSTR_NOVA_UI_HASHER_PREF_ERROR 		  = 135183, "Shell ScriptGL must be enabled in order to batch process textures.";
IDSTR_NOVA_UI_RESERVED = 135184, "";
IDSTR_NOVA_UI_SHELL_SCRIPTGL 			  = 135185, "Draw Shell GUI Using ScriptGL";
IDSTR_NOVA_HELP_SHELL_SCRIPTGL 			  = 135186, "Drawing the shell GUI backgrounds and side bars using scriptGL.";

IDSTR_NOVA_NET_WEAPON_DATA		= 135187, "DOWNLOADING WEAPON DATA";
IDSTR_NOVA_NET_TURRET_DATA		= 135188, "DOWNLOADING TURRET DATA";
IDSTR_NOVA_NET_SENSOR_DATA		= 135189, "DOWNLOADING SENSOR DATA";
IDSTR_NOVA_NET_REACTOR_DATA		= 135190, "DOWNLOADING REACTOR DATA";
IDSTR_NOVA_NET_SHIELD_DATA		= 135191, "DOWNLOADING SHIELD DATA";
IDSTR_NOVA_NET_ENGINE_DATA		= 135192, "DOWNLOADING ENGINE DATA";
IDSTR_NOVA_NET_ARMOR_DATA		= 135193, "DOWNLOADING ARMOR DATA";
IDSTR_NOVA_NET_COMPUTER_DATA	= 135194, "DOWNLOADING COMPUTER DATA";
IDSTR_NOVA_NET_INT_MOUNT_DATA	= 135195, "DOWNLOADING INT.MOUNT DATA";
IDSTR_NOVA_NET_VEHICLE_DATA		= 135196, "DOWNLOADING VEHICLE DATA";

IDSTR_NOVA_RESET_SERVER_CACHE	= 135197, "RESET SERVER CACHE";
IDSTR_NOVA_UI_SHOW_MEM	= 135198, "Show Ram Usage";

IDSTR_NOVA_RECOVERY_RESET_PROFILES = 135199, "RESET UPSCALE PROFILES";

//IDSTR_NOVA_UI_COLL_MESH_UNDAMAGED_RED = 135200, "Undamaged (RED):";
//IDSTR_NOVA_UI_COLL_MESH_UNDAMAGED_GREEN = 135201, "Undamaged (GREEN):";
//IDSTR_NOVA_UI_COLL_MESH_UNDAMAGED_BLUE = 135202, "Undamaged (BLUE):";
//
//IDSTR_NOVA_UI_COLL_MESH_DAMAGED_RED = 135203, "Damaged (RED):";
//IDSTR_NOVA_UI_COLL_MESH_DAMAGED_GREEN = 135204, "Damaged (GREEN):";
//IDSTR_NOVA_UI_COLL_MESH_DAMAGED_BLUE = 135205, "Damaged (BLUE):";
//
//IDSTR_NOVA_UI_COLL_MESH_DAMAGED_RED = 135206, "Critical (RED):";
//IDSTR_NOVA_UI_COLL_MESH_DAMAGED_GREEN = 135207, "Critical (GREEN):";
//IDSTR_NOVA_UI_COLL_MESH_DAMAGED_BLUE = 135208, "Critical (BLUE):";
//
IDSTR_NOVA_UI_LABEL_COLLMESH = 135200, "Collision Mesh Colors";
IDSTR_NOVA_UI_LABEL_COLLMESH_UNDAMAGED = 135201, "UNDAMAGED";
IDSTR_NOVA_UI_LABEL_COLLMESH_DAMAGED = 135202, "DAMAGED";
IDSTR_NOVA_UI_LABEL_COLLMESH_CRITICAL = 135203, "CRITICAL";