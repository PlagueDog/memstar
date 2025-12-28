if(isObject(MissionEditor){return;}
function MissionCreateSetup()
{
    if(!isObject(MissionEditor){return;}
#--------------------------------------------------------------------------------------------------------------------------------
# Register objects to be used with the mission editor in this file
#--------------------------------------------------------------------------------------------------------------------------------
#
#  Default bitmap indecies: 
#     0  -  Folder bitmap  (default for SimSets)
#     2  -  Node bitmap    (default for unregistered classes)
#
#  To add your own bitmaps, starting at index 4 use:
#  simTreeRegBitmaps treeName bitmap1.bmp bitmap2.bmp bitmap3.bmp bitmapN.bmp...
#
#--------------------------------------------------------------------------------------------------------------------------------
# Bitmaps for ES3 (indecies:  6, 8, 10, 12...)
#--------------------------------------------------------------------------------------------------------------------------------

simTreeRegBitmaps( MissionEditor, "Herc.bmp", "HercOpen.bmp" );
simTreeRegBitmaps( MissionEditor, "static.bmp", "staticOpen.bmp" );
simTreeRegBitmaps( MissionEditor, "interior.bmp", "interiorOpen.bmp" );
simTreeRegBitmaps( MissionEditor, "dropPt.bmp", "dropPtOpen.bmp" );
simTreeRegBitmaps( MissionEditor, "GUI.bmp", "GUIOpen.bmp" );

#--------------------------------------------------------------------------------------------------------------------------------
#  To Associate class types with bitmaps use:
#  simTreeRegClass treeName className NormalBmpIndex SelectedBmpIndex
#--------------------------------------------------------------------------------------------------------------------------------

simTreeRegClass( MissionEditor, Herc, 8, 9 );
simTreeRegClass( MissionEditor, StaticShape, 10, 11 );
simTreeRegClass( MissionEditor, StaticInterior, 12, 13 );
simTreeRegClass( MissionEditor, SimDropPoint, 14, 15 );
simTreeRegClass( MissionEditor, SimGui::Responder, 16, 17 );

#--------------------------------------------------------------------------------------------------------------------------------
#  To add custom object type masks to the MissionEditor
#  MissionRegType objectClassName scaleFactor
#    Where objectClassName is a description of the object ("Herc")
#          scaleFactor is the mask scale factor ( 1<<scaleFactor )
#--------------------------------------------------------------------------------------------------------------------------------

MissionRegType( SimMissionObjectType, 4 );
MissionRegType( SimMarkerObjectType, 10 );
MissionRegType( SimTriggerObjectType, 11 );
MissionRegType( BuildingObjectType, 10 );
MissionRegType( SimVehicleObjectType, 12 );

#--------------------------------------------------------------------------------------------------------------------------------
#  To add commands to the Mission Editor's "Scripts" menu
#
#  simTreeRegScript treeName menuText consoleCommand
#  simTreeRegScript treeName $separator
#--------------------------------------------------------------------------------------------------------------------------------

simTreeRegScript( MissionEditor, "showGfx", "showGfx();" );
simTreeRegScript( MissionEditor, "showGfx Software", "showGfxSW();" );
simTreeRegScript( MissionEditor, "showGfx Hardware", "showGfxHW();" );
simTreeRegScript( MissionEditor, "Show Server Tree", "serverTree();" );
simTreeRegScript( MissionEditor, "Show Client Tree", "clientTree();" );
simTreeRegScript( MissionEditor, "Run LSEditor", "LS::Create( LSEditor, simCanvas);" );
simTreeRegScript( MissionEditor, "Run TED", "Ted(simCanvas);" );
simTreeRegScript( MissionEditor, "Outline Mode", "messageCanvasDevice( simCanvas, outline);" );
simTreeRegScript( $separator );
simTreeRegScript( MissionEditor, "Debug", "debug();" );
simTreeRegScript( $separator );
simTreeRegScript( MissionEditor, "Camera Speed Slow",  "$easyCameraSpeed = 500;" );
simTreeRegScript( MissionEditor, "Camera Speed Fast",  "$easyCameraSpeed = 1000;" );
simTreeRegScript( MissionEditor, "Camera Speed Turbo", "$easyCameraSpeed = 2000;" );
simTreeRegScript( MissionEditor, "Focus TedObject", 	"focus(TedObject);" 	);
simTreeRegScript( MissionEditor, "UnFocus TedObject", 	"unfocus(TedObject);" 	);

#--------------------------------------------------------------------------------------------------------------------------------
#  Add commands to the Mission Editor's "Terrain" menu
#--------------------------------------------------------------------------------------------------------------------------------

MissionRegTerrain( Desert, Day,       newMission, Desert, "desert.d.ppl" );
MissionRegTerrain( Desert, Dusk,      newMission, Desert, "desert.t.ppl" );
MissionRegTerrain( Desert, Night,     newMission, Desert, "desert.n.ppl" );
MissionRegTerrain( Europa, Day,       newMission, Europa, "Europa.d.ppl" );
MissionRegTerrain( Europa, Dusk,      newMission, Europa, "Europa.t.ppl" );
MissionRegTerrain( Europa, Night,     newMission, Europa, "Europa.n.ppl" );
MissionRegTerrain( Ice, Day,          newMission, Ice, "Ice.d.ppl" );
MissionRegTerrain( Ice, Dusk,         newMission, Ice, "Ice.t.ppl" );
MissionRegTerrain( Ice, Night,        newMission, Ice, "Ice.n.ppl" );
MissionRegTerrain( Mars, Day,         newMission, Mars, "Mars.d.ppl" );
MissionRegTerrain( Mars, Dusk,        newMission, Mars, "Mars.t.ppl" );
MissionRegTerrain( Mars, Night,       newMission, Mars, "Mars.n.ppl" );
MissionRegTerrain( Mercury, Day,      newMission, Mercury, "Mercury.d.ppl" );
MissionRegTerrain( Mercury, Dusk,     newMission, Mercury, "Mercury.t.ppl" );
MissionRegTerrain( Mercury, Night,    newMission, Mercury, "Mercury.n.ppl" );
MissionRegTerrain( Moon, Day,         newMission, Moon, "Moon.d.ppl" );
MissionRegTerrain( Moon, Dusk,        newMission, Moon, "Moon.t.ppl" );
MissionRegTerrain( Moon, Night,       newMission, Moon, "Moon.n.ppl" );
MissionRegTerrain( Pluto, Day,        newMission, Pluto, "Pluto.d.ppl" );
MissionRegTerrain( Pluto, Dusk,       newMission, Pluto, "Pluto.t.ppl" );
MissionRegTerrain( Pluto, Night,      newMission, Pluto, "Pluto.n.ppl" );
MissionRegTerrain( Temperate, Day,    newMission, Temperate, "Temperate.d.ppl" );
MissionRegTerrain( Temperate, Dusk,   newMission, Temperate, "Temperate.t.ppl" );
MissionRegTerrain( Temperate, Night,  newMission, Temperate, "Temperate.n.ppl" );
MissionRegTerrain( Titan, Day,        newMission, Titan, "Titan.d.ppl" );
MissionRegTerrain( Titan, Dusk,       newMission, Titan, "Titan.t.ppl" );
MissionRegTerrain( Titan, Night,      newMission, Titan, "Titan.n.ppl" );
MissionRegTerrain( Venus, Day,        newMission, Venus, "Venus.d.ppl" );
MissionRegTerrain( Venus, Dusk,       newMission, Venus, "Venus.t.ppl" );
MissionRegTerrain( Venus, Night,      newMission, Venus, "Venus.n.ppl" );

#--------------------------------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------------------------------

$separator = "-----------------------";

#--------------------------------------------------------------------------------------------------------------------------------
#  To add commands to the Mission Editor's "Objects" menu
#
#  MissionRegObject menuCategory menuText consoleCommand
#  MissionRegObject menuCategory $separator
#  MissionRegObject $separator
#
#--------------------------------------------------------------------------------------------------------------------------------

MissionRegObject( Environment, Sky,                            MissionCreateObject, sky, SimSky );
MissionRegObject( Environment, Planet,                         MissionCreateObject, planet, SimPlanet, _B_O_G_U_S_, 0, 30, f, f, 1, 1, 1, 0, 0, 0  );
MissionRegObject( Environment, Stars,                          MissionCreateObject, starField, SimStarField );
MissionRegObject( Environment, Snowfall,                       MissionCreateObject, snowfall, Snowfall, 1, 0, 0, 0 );
MissionRegObject( Environment, Rain,                           MissionCreateObject, snowfall, Snowfall, 1, 0, 0, 1 );
MissionRegObject( Environment, $separator );
MissionRegObject( Environment, "ESPalette",                    MissionCreateObject, Palette, ESPalette );

#--------------------------------------------------------------------------------------------------------------------------------
MissionRegObject( $separator );

MissionRegObject( "Mission Logic", "Sim Group",                 MissionCreateObject, Group, SimGroup );
MissionRegObject( "Mission Logic", "Sim Set",                   MissionCreateObject, Set, SimSet );
MissionRegObject( "Mission Logic", $separator );                 
MissionRegObject( "Mission Logic", "Shape Group",               MissionCreateObject, shapeGroup, SimShapeGroup );
MissionRegObject( "Mission Logic", "Shape Group Rep",           MissionCreateObject, shapeGroupRep_, ShapeGroupRep );
MissionRegObject( "Mission Logic", $separator );
MissionRegObject( "Mission Logic", "Camera Path Group",         MissionCreateObject, cameraPathGroup, SimCameraPathGroup );
MissionRegObject( "Mission Logic", "Camera Path Rep",           MissionCreateObject, cameraPathRep, SimCameraPathRep );
MissionRegObject( "Mission Logic", "Camera Waypoint",           MissionCreateObject, cameraWaypoint, SimCameraWaypoint );
MissionRegObject( "Mission Logic", $separator );
MissionRegObject( "Mission Logic", "Drop Point Group",          MissionCreateObject, DropPointGroup, ESDropPointGroup );
MissionRegObject( "Mission Logic", "Drop Point",                MissionCreateObject, dropPoint, SimDropPoint );
MissionRegObject( "Mission Logic", "Path Marker",               MissionCreateObject, marker, SimMarker );
MissionRegObject( "Mission Logic", "Navigation Point",          MissionCreateObject, navigationMarker, ESNavMarker );
MissionRegObject( "Mission Logic", $separator );
MissionRegObject( "Mission Logic", "Trigger",                   MissionCreateObject, trigger, SimTrigger );
MissionRegObject( "Mission Logic", "Magnet",                    MissionCreateObject, simMagnet, SimMagnet );
MissionRegObject( "Mission Logic", $separator );
MissionRegObject( "Mission Logic", "Sound Source",              MissionCreateObject, simSoundSource, SimSoundSource );
MissionRegObject( "Mission Logic", "Sound Cockpit",             MissionCreateObject, simSoundCockpit, SimSoundCockpit );

#--------------------------------------------------------------------------------------------------------------------------------


MissionRegObject( "Terran Vehicles", "Terran Apocalypse",                MissionCreateObject, Herc, Herc, 1 );
MissionRegObject( "Terran Vehicles", "Terran Minotaur",                  MissionCreateObject, Herc, Herc, 2 );
MissionRegObject( "Terran Vehicles", "Terran Gorgon",                    MissionCreateObject, Herc, Herc, 3 );
MissionRegObject( "Terran Vehicles", "Terran Talon",                     MissionCreateObject, Herc, Herc, 4 );
MissionRegObject( "Terran Vehicles", "Terran Basilisk",                  MissionCreateObject, Herc, Herc, 5 );
MissionRegObject( "Terran Vehicles", "Paladin Tank",                     MissionCreateObject, Tank, Tank, 6 );
MissionRegObject( "Terran Vehicles", "Myrmidon Tank",                    MissionCreateObject, Tank, Tank, 7 );
MissionRegObject( "Terran Vehicles", "Disruptor Tank",                   MissionCreateObject, Tank, Tank, 8 );
MissionRegObject( "Terran Vehicles", "Nike Artillery",                   MissionCreateObject, Tank, Tank, 133 );
MissionRegObject( "Terran Vehicles", "Supressor Tank",                   MissionCreateObject, Tank, Tank, 134 );

MissionRegObject( "Terran Vehicles", "Banshee Flyer",                    MissionCreateObject, Flyer, Flyer, 9 );
MissionRegObject( "Terran Vehicles", "Cargo ship",                       MissionCreateObject, Flyer, Flyer, 18 );
MissionRegObject( "Terran Vehicles", "Escape ship",                      MissionCreateObject, Flyer, Flyer, 19 );
MissionRegObject( "Terran Vehicles", "Drop ship",                        MissionCreateObject, Flyer, Flyer, 130 );
MissionRegObject( "Terran Vehicles", "Draco Bomber",                     MissionCreateObject, Flyer, Flyer, 131 );
MissionRegObject( "Terran Vehicles", "Conveyor",                         MissionCreateObject, Flyer, Flyer, 132 );
 

MissionRegObject( "Knight Vehicles", "Knight's Apocalypse",             MissionCreateObject, Herc, Herc, 10 );
MissionRegObject( "Knight Vehicles", "Knight's Minotaur",               MissionCreateObject, Herc, Herc, 11 );
MissionRegObject( "Knight Vehicles", "Knight's Gorgon",                 MissionCreateObject, Herc, Herc, 12 );
MissionRegObject( "Knight Vehicles", "Knight's Talon",                  MissionCreateObject, Herc, Herc, 13 );
MissionRegObject( "Knight Vehicles", "Knight's Basilisk",               MissionCreateObject, Herc, Herc, 14 );
MissionRegObject( "Knight Vehicles", "Knight's Paladin",                MissionCreateObject, Tank, Tank, 15 );
MissionRegObject( "Knight Vehicles", "Knight's Myrmidon",               MissionCreateObject, Tank, Tank, 16 );
MissionRegObject( "Knight Vehicles", "Knight's Disruptor",              MissionCreateObject, Tank, Tank, 17 );
MissionRegObject( "Knight Vehicles", "Knight's Banshee",                MissionCreateObject, Flyer, Flyer, 110 );
MissionRegObject( "Knight Vehicles", "Knight's Drop Ship",              MissionCreateObject, Flyer, Flyer, 111 );


MissionRegObject( "Cybrid Vehicles", "Cybrid Seeker",                   MissionCreateObject, Herc, Herc, 20 );
MissionRegObject( "Cybrid Vehicles", "Cybrid Goad",                     MissionCreateObject, Herc, Herc, 21 );
MissionRegObject( "Cybrid Vehicles", "Cybrid Shepherd",                 MissionCreateObject, Herc, Herc, 22 );
MissionRegObject( "Cybrid Vehicles", "Cybrid Adjudicator",              MissionCreateObject, Herc, Herc, 23 );
MissionRegObject( "Cybrid Vehicles", "Cybrid Executioner",              MissionCreateObject, Herc, Herc, 24 );
MissionRegObject( "Cybrid Vehicles", "Bolo Tank",                       MissionCreateObject, Tank, Tank, 25 );
MissionRegObject( "Cybrid Vehicles", "Recluse Tank",                    MissionCreateObject, Tank, Tank, 26 );
MissionRegObject( "Cybrid Vehicles", "Platinum Adjudicator",            MissionCreateObject, Herc, Herc, 27 );
MissionRegObject( "Cybrid Vehicles", "Platinum Executioner",            MissionCreateObject, Herc, Herc, 28 );
MissionRegObject( "Cybrid Vehicles", "Cybrid Artillery",                MissionCreateObject, Tank, Tank, 90 );
MissionRegObject( "Cybrid Vehicles", "Cybrid Advocate",                 MissionCreateObject, Flyer, Flyer, 91 );
MissionRegObject( "Cybrid Vehicles", "Cybrid Drop Ship",                MissionCreateObject, Flyer, Flyer, 92 );
MissionRegObject( "Cybrid Vehicles", "Cybrid Consul Bomber",            MissionCreateObject, Flyer, Flyer, 93 );


MissionRegObject( "Metagen Vehicles", "Metagen Seeker",                 MissionCreateObject, Herc, Herc, 35 );
MissionRegObject( "Metagen Vehicles", "Metagen Goad",                   MissionCreateObject, Herc, Herc, 36 );
MissionRegObject( "Metagen Vehicles", "Metagen Shepherd",               MissionCreateObject, Herc, Herc, 37 );
MissionRegObject( "Metagen Vehicles", "Metagen Adjudicator",            MissionCreateObject, Herc, Herc, 38 );
MissionRegObject( "Metagen Vehicles", "Metagen Executioner",            MissionCreateObject, Herc, Herc, 39 );


MissionRegObject( "Rebel Vehicles", "Rebel Emancipator",               MissionCreateObject, Herc, Herc, 30 );
MissionRegObject( "Rebel Vehicles", "Avenger Tank",                    MissionCreateObject, Tank, Tank, 31 );
MissionRegObject( "Rebel Vehicles", "Dreadnought Tank",                MissionCreateObject, Tank, Tank, 32 );
MissionRegObject( "Rebel Vehicles", "Rebel Olympian",                  MissionCreateObject, Herc, Herc, 33 );
MissionRegObject( "Rebel Vehicles", "Rebel Thumper",                   MissionCreateObject, Tank, Tank, 72 );
MissionRegObject( "Rebel Vehicles", "Rebel Artillery",                 MissionCreateObject, Tank, Tank, 137 );


MissionRegObject( "Special Vehicles", "Harabec's Apocalypse",            MissionCreateObject, Herc, Herc, 40 );
MissionRegObject( "Special Vehicles", "Harabec's Apocalypse, cin",      MissionCreateObject, Herc, Herc, 43 );
MissionRegObject( "Special Vehicles", "Harabec's Predator",              MissionCreateObject, Tank, Tank, 41 );
MissionRegObject( "Special Vehicles", "Harabec's Super Predator",        MissionCreateObject, Tank, Tank, 45 );
MissionRegObject( "Special Vehicles", "Caanan's Basilisk",               MissionCreateObject, Herc, Herc, 42 );
MissionRegObject( "Special Vehicles", "Caanan's Basilisk, cin",         MissionCreateObject, Herc, Herc, 44 );
MissionRegObject( "Special Vehicles", "Prometheus",                    MissionCreateObject, Herc, Herc, 29 );


MissionRegObject( "Pirate Vehicles", "Pirate's Apocalypse",            MissionCreateObject, Herc, Herc, 50 );
MissionRegObject( "Pirate Vehicles", "Pirate's Dreadlock",             MissionCreateObject, Tank, Tank, 51 );
MissionRegObject( "Pirate Vehicles", "Pirate's Emancipator",           MissionCreateObject, Herc, Herc, 52 );

MissionRegObject( "Drone Vehicles", "Terran Empty Cargo",               MissionCreateObject, Drone, Tank, 60 );
MissionRegObject( "Drone Vehicles", "Terran Ammo Cargo",                MissionCreateObject, Drone, Tank, 61 );
MissionRegObject( "Drone Vehicles", "Terran Big Ammo Cargo",            MissionCreateObject, Drone, Tank, 62 );
MissionRegObject( "Drone Vehicles", "Terran Big Personnel Cargo",       MissionCreateObject, Drone, Tank, 63 );
MissionRegObject( "Drone Vehicles", "Terran Fuel Cargo",                MissionCreateObject, Drone, Tank, 64 );
MissionRegObject( "Drone Vehicles", "Terran Minotaur Cargo",            MissionCreateObject, Drone, Tank, 65 );
MissionRegObject( "Drone Vehicles", "Terran Utility Truck",             MissionCreateObject, Drone, Tank, 71 );
MissionRegObject( "Drone Vehicles", "Terran Sovereign",                 MissionCreateObject, Tank, Tank, 135 );
MissionRegObject( "Drone Vehicles", "Terran Surveyor",                  MissionCreateObject, Tank, Tank, 136 );
MissionRegObject( "Drone Vehicles", "Terran Starefield",                MissionCreateObject, Tank, Tank,  73 );
MissionRegObject( "Drone Vehicles", "Rebel Empty Cargo",                MissionCreateObject, Drone, Tank, 66 );
MissionRegObject( "Drone Vehicles", "Rebel Ammo Cargo",                 MissionCreateObject, Drone, Tank, 67 );
MissionRegObject( "Drone Vehicles", "Rebel Big Cargo Transport",        MissionCreateObject, Drone, Tank, 68 );
MissionRegObject( "Drone Vehicles", "Rebel Bix Box Cargo Transport",    MissionCreateObject, Drone, Tank, 69 );
MissionRegObject( "Drone Vehicles", "Rebel Box Cargo Transport",        MissionCreateObject, Drone, Tank, 70 );
MissionRegObject( "Drone Vehicles", "Cybrid Omnicrawler",               MissionCreateObject, Drone, Tank, 94 );
MissionRegObject( "Drone Vehicles", "Cybrid Protector",                 MissionCreateObject, Drone, Tank, 95 );
MissionRegObject( "Drone Vehicles", "Cybrid Jamma",                     MissionCreateObject, Drone, Tank, 96 );

#--------------------------------------------------------------------------------------------------------------------------------
MissionRegObject( $separator );

#---Turrets-------------------
MissionRegObject( Turrets, "Cybrid Laser Turret",              MissionCreateObject, Turret_CY_LS, Turret, 1 );
MissionRegObject( Turrets, "Cybrid Missle Turret",             MissionCreateObject, Turret_CY_MS, Turret, 2 );
MissionRegObject( Turrets, $separator );
MissionRegObject( Turrets, "Mars Big Cannon",                  MissionCreateObject, Turret_H_CN,   Turret, 3 );
MissionRegObject( Turrets, "Moon Cannon",                      MissionCreateObject, Turret_M_CN,   Turret, 25 );
MissionRegObject( Turrets, "Trooper Cannon",                   MissionCreateObject, Turret_T_CN,   Turret, 26 );
MissionRegObject( Turrets, "Desert Cannon",                    MissionCreateObject, Turret_D_CN,   Turret, 31 );
MissionRegObject( Turrets, $separator );
MissionRegObject( Turrets, "Human Desert Laser Short",         MissionCreateObject, Turret_H_D_LS, Turret, 27 );
MissionRegObject( Turrets, "Human Desert Laser Tall",          MissionCreateObject, Turret_H_D_LS, Turret, 28 );
MissionRegObject( Turrets, "Human Earth Laser Short",          MissionCreateObject, Turret_H_E_LS, Turret, 4 );
MissionRegObject( Turrets, "Human Earth Laser Tall",           MissionCreateObject, Turret_H_E_LS, Turret, 5 );
MissionRegObject( Turrets, "Human Mars Laser Short",           MissionCreateObject, Turret_H_M_LS, Turret, 6 );
MissionRegObject( Turrets, "Human Mars Laser Tall",            MissionCreateObject, Turret_H_M_LS, Turret, 7 );
MissionRegObject( Turrets, "Human Moon Laser Short",           MissionCreateObject, Turret_H_M_LS, Turret, 8 );
MissionRegObject( Turrets, "Human Moon Laser Tall",            MissionCreateObject, Turret_H_M_LS, Turret, 9 );
MissionRegObject( Turrets, "Human Titan Laser Short",          MissionCreateObject, Turret_H_T_LS, Turret, 10 );
MissionRegObject( Turrets, "Human Titan Laser Tall",           MissionCreateObject, Turret_H_T_LS, Turret, 11 );
MissionRegObject( Turrets, "Human Venus Laser Short",          MissionCreateObject, Turret_H_V_LS, Turret, 12 );
MissionRegObject( Turrets, "Human Venus Laser Tall",           MissionCreateObject, Turret_H_V_LS, Turret, 13 );
MissionRegObject( Turrets, $separator );
MissionRegObject( Turrets, "Human Desert Missle Short",        MissionCreateObject, Turret_H_D_MS, Turret, 29 );
MissionRegObject( Turrets, "Human Desert Missle Tall",         MissionCreateObject, Turret_H_D_MS, Turret, 30 );
MissionRegObject( Turrets, "Human Earth Missle Short",         MissionCreateObject, Turret_H_E_MS, Turret, 14 );
MissionRegObject( Turrets, "Human Earth Missle Tall",          MissionCreateObject, Turret_H_E_MS, Turret, 15 );
MissionRegObject( Turrets, "Human Mars Missle Short",          MissionCreateObject, Turret_H_M_MS, Turret, 16 );
MissionRegObject( Turrets, "Human Mars Missle Tall",           MissionCreateObject, Turret_H_M_MS, Turret, 17 );
MissionRegObject( Turrets, "Human Moon Missle Short",          MissionCreateObject, Turret_H_M_MS, Turret, 18 );
MissionRegObject( Turrets, "Human Moon Missle Tall",           MissionCreateObject, Turret_H_M_MS, Turret, 19 );
MissionRegObject( Turrets, "Human Titan Missle Short",         MissionCreateObject, Turret_H_T_MS, Turret, 20 );
MissionRegObject( Turrets, "Human Titan Missle Tall",          MissionCreateObject, Turret_H_T_MS, Turret, 21 );
MissionRegObject( Turrets, "Human Venus Missle Short",         MissionCreateObject, Turret_H_V_MS, Turret, 22 );
MissionRegObject( Turrets, "Human Venus Missle Tall",          MissionCreateObject, Turret_H_V_MS, Turret, 23 );

MissionRegObject( Turrets, $separator );
MissionRegObject( Turrets, "Human Desert Laser Short Rearm",   MissionCreateObject, Turret_H_D_LS, Turret, 32 );
MissionRegObject( Turrets, "Human Titan Laser Short Rearm",    MissionCreateObject, Turret_H_D_LS, Turret, 34 );
MissionRegObject( Turrets, "Trooper Cannon Rearm",             MissionCreateObject, Turret_T_CN,   Turret, 35 );
MissionRegObject( Turrets, "Cybrid Laser Turret Rearm",        MissionCreateObject, Turret_CY_LS,  Turret, 33 );

#---CYBRID SHAPES----------------
MissionRegObject( $separator );
MissionRegObject( "Cybrid Shapes", "Cybrid Bomberbay",         MissionCreateObject, cBomberBay,         StaticShape, true, "cBomberbay.dts" );
MissionRegObject( "Cybrid Shapes", "Cybrid Comm",              MissionCreateObject, cComm,              StaticShape, true, "cComm.dts" );
MissionRegObject( "Cybrid Shapes", "Cybrid Drop Pod",          MissionCreateObject, cDropPod,           StaticShape, true, "cDropPod.dts" );
MissionRegObject( "Cybrid Shapes", "Cybrid Entrance",          MissionCreateObject, cEntrance,          StaticShape, true, "cEntrance.dts" );
MissionRegObject( "Cybrid Shapes", "Cybrid Generator",         MissionCreateObject, cGenerator,         StaticShape, true, "cGenerator.dts" );
MissionRegObject( "Cybrid Shapes", "Cybrid Motion Detector",   MissionCreateObject, cMotiondetector,    StaticShape, true, "cMotionDetector.dts" );
MissionRegObject( "Cybrid Shapes", "Cybrid Nexus",             MissionCreateObject, cNexus,             StaticShape, true, "cNexus.dts" );
MissionRegObject( "Cybrid Shapes", "Cybrid Perimeter",         MissionCreateObject, cPerimeter,         StaticShape, false, "cPerimeter.dts" );
MissionRegObject( "Cybrid Shapes", "Cybrid Power",             MissionCreateObject, cPower,             StaticShape, true,  "cPower.dts" );
MissionRegObject( "Cybrid Shapes", "Cybrid Relay Station",     MissionCreateObject, cRelay,             StaticShape, true,  "cRelay.dts" );
MissionRegObject( "Cybrid Shapes", "Cybrid Search Array",      MissionCreateObject, cSearcharray,       StaticShape, true, "cSearchArray.dts" );
MissionRegObject( "Cybrid Shapes", "Cybrid Storage",           MissionCreateObject, cStorage,           StaticShape, true, "cStorage.dts" );
MissionRegObject( "Cybrid Shapes", "Cybrid Tracking",          MissionCreateObject, cTracking,          StaticShape, true, "cTracking.dts" );
MissionRegObject( "Cybrid Shapes", "Cybrid Uplink",            MissionCreateObject, cUplink,            StaticShape, true, "cUplink.dts" );
MissionRegObject( "Cybrid Shapes", "Cybrid Wall",              MissionCreateObject, cWall,              StaticShape, true, "cWall.dts" );
MissionRegObject( "Cybrid Shapes", "Cybrid Temple Wall",       MissionCreateObject, cTempleWall,        StaticShape, true, "cTempleWall.dts" );
MissionRegObject( "Cybrid Shapes", "Prometheus' Hut(ZED)",     MissionCreateObject, cTemple,            StaticShape, true, "cTemple.dts" );
MissionRegObject( "Cybrid Shapes", $separator );
MissionRegObject( "Cybrid Shapes", "Beacon         (ZED)",     MissionCreateObject, cbeacon,            StaticInterior, "cBeacon.dis" );
MissionRegObject( "Cybrid Shapes", "Landing Pad    (ZED)",     MissionCreateObject, clandpad,           StaticInterior, "clandpad.dis" );
MissionRegObject( "Cybrid Shapes", "Large Platform (ZED)",     MissionCreateObject, cbrid_p1,           StaticInterior, "cbrid_p1.dis" );
MissionRegObject( "Cybrid Shapes", "Prometheus Base(ZED)",     MissionCreateObject, cprombase,          StaticInterior, "cPromBase.dis" );
MissionRegObject( "Cybrid Shapes", "Small Platform (ZED)",     MissionCreateObject, cbrid_p2,           StaticInterior, "cbrid_p2.dis" );
MissionRegObject( "Cybrid Shapes", "Silo           (ZED)",     MissionCreateObject, csilo,              StaticInterior, "csilo.dis" );
MissionRegObject( "Cybrid Shapes", "Silo Platform  (ZED)",     MissionCreateObject, csilo_p1,           StaticInterior, "csilo_p1.dis" );
MissionRegObject( "Cybrid Shapes", "Tunnels        (ZED)",     MissionCreateObject, cbrid,              StaticInterior, "cbrid.dis" );
MissionRegObject( "Cybrid Shapes", $separator );
MissionRegObject( "Cybrid Shapes", "Cybrid Tower 1",           MissionCreateObject, cbig1,              StaticInterior, "cbig1.dis");



#---DESERT EARTH SHAPES---------
MissionRegObject( "Desert Shapes", "Desert Big Dish",          MissionCreateObject, hDesertBigDish,     StaticShape, true, "hDesertBigDish.dts" );
MissionRegObject( "Desert Shapes", "Desert Church",            MissionCreateObject, hDesertchurch,      StaticShape, true, "hDesertchurch.dts" );
MissionRegObject( "Desert Shapes", "Desert Defense",           MissionCreateObject, hDesertDefense,     StaticShape, true, "hDesertDefense.dts" );
MissionRegObject( "Desert Shapes", "Desert Dwelling",          MissionCreateObject, hDesertdwelling,    StaticShape, true, "hDesertdwelling.dts" );
MissionRegObject( "Desert Shapes", "Desert Garage",            MissionCreateObject, hDesertgarage,      StaticShape, true, "hDesertgarage.dts" );
MissionRegObject( "Desert Shapes", "Desert Gathering",         MissionCreateObject, hDesertgathering,   StaticShape, true, "hDesertgathering.dts" );
MissionRegObject( "Desert Shapes", "Desert LandMine",          MissionCreateObject, hDesertMine,        StaticShape, true, "hDesertMine.dts" );
MissionRegObject( "Desert Shapes", "Desert Power",             MissionCreateObject, hDesertpower,       StaticShape, true, "hDesertpower.dts" );
MissionRegObject( "Desert Shapes", "Desert Silo",              MissionCreateObject, hDesertsilo,        StaticShape, true, "hDesertsilo.dts" );
MissionRegObject( "Desert Shapes", "Desert Sludge",            MissionCreateObject, hDesertsludge,      StaticShape, true, "hDesertsludge.dts" );
MissionRegObject( "Desert Shapes", "Desert Uplink",            MissionCreateObject, hDesertuplink,      StaticShape, true, "hDesertuplink.dts" );
MissionRegObject( "Desert Shapes", "Desert Vapor",             MissionCreateObject, hDesertvapor,       StaticShape, true, "hDesertvapor.dts" );
MissionRegObject( "Desert Shapes", "Desert Water Tower",       MissionCreateObject, hDesertWaterTower,  StaticShape, true, "hDesertWaterTower.dts" );
MissionRegObject( "Desert Shapes", $separator);
MissionRegObject( "Desert Shapes", "Desert Troop Barracks",    MissionCreateObject, hDesertTroopBarracks,       StaticShape, true, "hDesertTroopBarracks.dts" );
MissionRegObject( "Desert Shapes", "Desert Troop Barracks, sm", MissionCreateObject, hDesertSmallTroopBarracks,  StaticShape, true, "hDesertSmallTroopBarracks.dts" );
MissionRegObject( "Desert Shapes", "Desert Troop Com, sm",      MissionCreateObject, hDesertSmallTroopCom,   StaticShape, true, "hDesertSmallTroopCom.dts" );
MissionRegObject( "Desert Shapes", "Desert Troop Garage",      MissionCreateObject, hDesertTroopGarage,     StaticShape, true, "hDesertTroopGarage.dts" );
MissionRegObject( "Desert Shapes", "Desert Troop Hall",        MissionCreateObject, hDesertTroopHall,       StaticShape, true, "hDesertTroopHall.dts" );
MissionRegObject( "Desert Shapes", "Desert Troop HQ, sm",      MissionCreateObject, hDesertSmallTroopHQ,    StaticShape, true, "hDesertSmallTroopHQ.dts" );
MissionRegObject( "Desert Shapes", "Desert Pillar1",           MissionCreateObject, hdesertpillar1,         StaticShape, true, "hdesertpillar_1.dts" );
MissionRegObject( "Desert Shapes", "Desert Pillar2",           MissionCreateObject, hdesertpillar2,         StaticShape, true, "hdesertpillar_2.dts" );
MissionRegObject( "Desert Shapes", "Desert Pillar3",           MissionCreateObject, hdesertpillar3,         StaticShape, true, "hdesertpillar_3.dts" );
MissionRegObject( "Desert Shapes", "Desert Pillar4",           MissionCreateObject, hdesertpillar4,         StaticShape, true, "hdesertpillar_4.dts" );
MissionRegObject( "Desert Shapes", "Desert Pillar5",           MissionCreateObject, hdesertpillar5,         StaticShape, true, "hdesertpillar_5_tall.dts" );
MissionRegObject( "Desert Shapes", "Desert Pillar6",           MissionCreateObject, hdesertpillar6,         StaticShape, true, "hdesertpillar_6_tall.dts" );
MissionRegObject( "Desert Shapes", "Desert Altar",             MissionCreateObject, hdesertaltar,           StaticShape, true, "hdesertpillar_altar.dts" );
MissionRegObject( "Desert Shapes", "Desert Hall",              MissionCreateObject, hdeserthall,            StaticShape, true, "hdesertpillar_hall.dts" );

MissionRegObject( "Desert Shapes", $separator);
MissionRegObject( "Desert Shapes", "Pyramid 1(ZED)",           MissionCreateObject, hpyramid,           StaticInterior, "hpyramid.dis" );
MissionRegObject( "Desert Shapes", "Pyramid 2(ZED)",           MissionCreateObject, hpyramid2,          StaticInterior, "hpyramid2.dis" );
MissionRegObject( "Desert Shapes", "Pyramid 3(ZED)",           MissionCreateObject, hpyramid3,          StaticInterior, "hpyramid3.dis" );
MissionRegObject( "Desert Shapes", $separator);
MissionRegObject( "Desert Shapes", "Desert Tower 1",    MissionCreateObject, hbigsand1m,	       StaticInterior, "hbigsand1d.dis");
MissionRegObject( "Desert Shapes", "Desert Tower 2",    MissionCreateObject, hbigsand2m,	       StaticInterior, "hbigsand2d.dis");
MissionRegObject( "Desert Shapes", "Desert Tower 3",    MissionCreateObject, hbigsand3m,	       StaticInterior, "hbigsand3d.dis");
MissionRegObject( "Desert Shapes", "Desert Tower 4",    MissionCreateObject, hbigsand4m,	       StaticInterior, "hbigsand4d.dis");
MissionRegObject( "Desert Shapes", "Desert Tower 5",    MissionCreateObject, hbigsand5m,	       StaticInterior, "hbigsand5d.dis");
MissionRegObject( "Desert Shapes", "Desert Tower 6",    MissionCreateObject, hbigsand6m,	       StaticInterior, "hbigsand6d.dis");
MissionRegObject( "Desert Shapes", "Desert Ruins 1",    MissionCreateObject, hruinsd1,	           StaticInterior, "hruinsd1.dis");
MissionRegObject( "Desert Shapes", "Desert Ruins 2",    MissionCreateObject, hruinsd2,	           StaticInterior, "hruinsd2.dis");
MissionRegObject( "Desert Shapes", "Desert Ruins 3",    MissionCreateObject, hruinsd3,	           StaticInterior, "hruinsd3.dis");
MissionRegObject( "Desert Shapes", "Desert Ruins 4",    MissionCreateObject, hruinsd4,	           StaticInterior, "hruinsd4.dis");



#---GENERIC EARTH SHAPES----
MissionRegObject( "Earth Shapes", "Troop Barracks",            MissionCreateObject, hTroopbarracks,     StaticShape, true, "hTroopbarracks.dts" );
MissionRegObject( "Earth Shapes", "Troop Barracks, Small",     MissionCreateObject, hTroopbarrackssm,   StaticShape, true, "hTroopbarrackssm.dts" );
MissionRegObject( "Earth Shapes", "Troop Bigdish",             MissionCreateObject, hTroopbigdish,      StaticShape, true, "hTroopbigdish.dts" );
MissionRegObject( "Earth Shapes", "Troop Comm",                MissionCreateObject, hTroopcomm,         StaticShape, true, "hTroopcomm.dts" );
MissionRegObject( "Earth Shapes", "Troop Energy Projector",    MissionCreateObject, htroop_orbital_power,   StaticShape, true, "htroop_orbital_power.dts" );
MissionRegObject( "Earth Shapes", "Troop Garage",              MissionCreateObject, hTroopgarage,       StaticShape, true, "hTroopgarage.dts" );
MissionRegObject( "Earth Shapes", "Troop Garage, Small",       MissionCreateObject, hTroopgaragesm,     StaticShape, true, "hTroopgaragesm.dts" );
MissionRegObject( "Earth Shapes", "Troop Generator",           MissionCreateObject, hTroopgenerator,    StaticShape, true, "hTroopgenerator.dts" );
MissionRegObject( "Earth Shapes", "Troop Hall",                MissionCreateObject, hTroophall,         StaticShape, true, "hTroophall.dts" );
MissionRegObject( "Earth Shapes", "Troop HQ",                  MissionCreateObject, hTroophq,           StaticShape, true, "hTroophq.dts" );
MissionRegObject( "Earth Shapes", "Troop Listen",              MissionCreateObject, hTrooplisten,       StaticShape, true, "hTrooplisten.dts" );
MissionRegObject( "Earth Shapes", "Troop Power",               MissionCreateObject, hTrooppower,        StaticShape, true, "hTrooppower.dts" );
MissionRegObject( "Earth Shapes", "Troop Supply",              MissionCreateObject, hTroopsupply,       StaticShape, true, "hTroopsupply.dts" );
MissionRegObject( "Earth Shapes", "Troop Supply Dump",         MissionCreateObject, hTroop_supply_dump, StaticShape, true, "hTroop_supply_dump.dts" );
MissionRegObject( "Earth Shapes", "Troop Trackbase",           MissionCreateObject, hTrooptrackbase,    StaticShape, true, "hTrooptrackbase.dts" );
MissionRegObject( "Earth Shapes", "Troop Trackdish",           MissionCreateObject, hTrooptrackdish,    StaticShape, true, "hTrooptrackdish.dts" );
MissionRegObject( "Earth Shapes", "Troop Uplink",              MissionCreateObject, hTroopuplink,       StaticShape, true, "hTroopuplink.dts" );
MissionRegObject( "Earth Shapes", "Troop Planet Defense",      MissionCreateObject, hTroopPlanetDefense,    StaticShape, true, "hTroopPlanetDefense.dts" );
MissionRegObject( "Earth Shapes", $separator);
MissionRegObject( "Earth Shapes", "Bridge           (ZED)",     MissionCreateObject, hbridgee,           StaticInterior, "hbridgee.dis" );
MissionRegObject( "Earth Shapes", "Garage           (ZED)",     MissionCreateObject, hgaragee,           StaticInterior, "hgaragee.dis" );
MissionRegObject( "Earth Shapes", "Landing Pad      (ZED)",     MissionCreateObject, hlandpade,          StaticInterior, "hlandpade.dis" );
MissionRegObject( "Earth Shapes", "Landpad Platform (ZED)",     MissionCreateObject, hlandpade_p1,       StaticInterior, "hlandpade_p1.dis" );
MissionRegObject( "Earth Shapes", "Hangar           (ZED)",     MissionCreateObject, hhangare,           StaticInterior, "hhangare.dis" );
MissionRegObject( "Earth Shapes", "Hangar Platform  (ZED)",     MissionCreateObject, hhangare_p1,        StaticInterior, "hhangare_p1.dis" );
MissionRegObject( "Earth Shapes", "Airport          (ZED)",     MissionCreateObject, hairporte,          StaticInterior, "hairporte.dis" );
MissionRegObject( "Earth Shapes", "Airport Platform (ZED)",     MissionCreateObject, hairporte_p1,       StaticInterior, "hairporte_p1.dis" );
MissionRegObject( "Earth Shapes", "Huge Wall        (ZED)",     MissionCreateObject, hwallhuge,          StaticInterior, "hwallhuge.dis" );

#---EUROPA SHAPES------------
MissionRegObject( "Europa Shapes", "Europa Rock2",          MissionCreateObject, xeuroparock2,       StaticShape, "xeuroparock02.dts" );
MissionRegObject( "Europa Shapes", "Europa Rock3",          MissionCreateObject, xeuroparock3,       StaticShape, "xeuroparock03.dts" );
MissionRegObject( "Europa Shapes", $separator);
MissionRegObject( "Europa Shapes", "Heal (ZED)", 			MissionCreateObject, hheale,       	StaticInterior, "hheale.dis" );
MissionRegObject( "Europa Shapes", "Ammo (ZED)", 			MissionCreateObject, hammoe,       	StaticInterior, "hammoe.dis" );
MissionRegObject( "Europa Shapes", "Trooper Hanger (ZED)", 	MissionCreateObject, hTroophouseE,	StaticInterior, "hTroophouseE.dis" );
MissionRegObject( "Europa Shapes", "Ruins1 (ZED)", 	        MissionCreateObject, hRuinsT,	    StaticInterior, "hRuinsT.dis" );
MissionRegObject( "Europa Shapes", "Ruins2 (ZED)", 	        MissionCreateObject, hRuinsT2,	    StaticInterior, "hRuinsT2.dis" );
MissionRegObject( "Europa Shapes", "Ruins3 (ZED)", 	        MissionCreateObject, hRuinsT3,	    StaticInterior, "hRuinsT3.dis" );


#---SNOW SHAPES--------------
MissionRegObject( "Ice Shapes", "Office",                      MissionCreateObject, hOffice,            StaticShape, true, "hOffice.dts" );
MissionRegObject( "Ice Shapes", "Solar Station",               MissionCreateObject, hSnowSolar,         StaticShape, true, "hSolarStation.dts" );
MissionRegObject( "Ice Shapes", "Gathering Hall",              MissionCreateObject, hSnowGath,          StaticShape, true, "hSnowGathering.dts" );
MissionRegObject( "Ice Shapes", "Snowlland",                   MissionCreateObject, hSnowLand,          StaticShape, true, "hSnowlland.dts" );
MissionRegObject( "Ice Shapes", "Snow Church",                 MissionCreateObject, hSnowChurch,        StaticShape, true, "hSnowChurch.dts" );
MissionRegObject( "Ice Shapes", "Snow Com",                    MissionCreateObject, hSnowCom,           StaticShape, true, "hSnowCom.dts" );
MissionRegObject( "Ice Shapes", "Snow Residence",              MissionCreateObject, hSnowRes,           StaticShape, true, "hSnowResidence.dts" );
MissionRegObject( "Ice Shapes", "Snow Storage",                MissionCreateObject, hSnowStor,          StaticShape, true, "hSnowStorage.dts" );
MissionRegObject( "Ice Shapes", "Snow Uplink",                 MissionCreateObject, hSnowUplink,        StaticShape, true, "hSnowUplink.dts" );
MissionRegObject( "Ice Shapes", "Snow Rock1",                  MissionCreateObject, hSnowRock1,         StaticShape, false,"xSnowRock1.dts" );
MissionRegObject( "Ice Shapes", "Snow Rock2",                  MissionCreateObject, hSnowRock2,         StaticShape, false,"xSnowRock2.dts" );
MissionRegObject( "Ice Shapes", "Snow Rock3",                  MissionCreateObject, hSnowRock3,         StaticShape, false,"xSnowRock3.dts" );
MissionRegObject( "Ice Shapes", "Snow Tree",                   MissionCreateObject, hTree,              StaticShape, false,"xTree.dts" );
MissionRegObject( "Ice Shapes", "SnowMan",                     MissionCreateObject, hSnowMan,           StaticShape, false,"xSnowMan.dts" );
MissionRegObject( "Ice Shapes", $separator );
MissionRegObject( "Ice Shapes", "Airport     (ZED)",           MissionCreateObject, hairporta,          StaticInterior, "hairporta.dis" );
MissionRegObject( "Ice Shapes", "Gate        (ZED)",           MissionCreateObject, hgatea,             StaticInterior, "hgatea.dis" );
MissionRegObject( "Ice Shapes", "Landing Pad (ZED)",           MissionCreateObject, hlandpada,          StaticInterior, "hlandpada.dis" );
MissionRegObject( "Ice Shapes", "Pad Office  (ZED)",           MissionCreateObject, hpadofficea,        StaticInterior, "hpadofficea.dis" );
MissionRegObject( "Ice Shapes", "Heal Pad    (ZED)",           MissionCreateObject, hheal2a,            StaticInterior, "hheal2a.dis" );
MissionRegObject( "Ice Shapes", "Reload Pad  (ZED)",           MissionCreateObject, hammo2a,            StaticInterior, "hammo2a.dis" );
MissionRegObject( "Ice Shapes", "Heal        (ZED)",           MissionCreateObject, hheal2a,       StaticInterior, "hheal2a.dis" );
MissionRegObject( "Ice Shapes", "Ammo        (ZED)",           MissionCreateObject, hammo2a,       StaticInterior, "hammo2a.dis" );

#---MARS SHAPES (A-N) ---------------
MissionRegObject( "Mars Shapes A-N", "Atmosphere",             MissionCreateObject, hAtmos,             StaticShape, true, "hAtmos.dts" );
MissionRegObject( "Mars Shapes A-N", "Barracks",               MissionCreateObject, hMarsBarracks,      StaticShape, true, "hMarsBarracks.dts" );
MissionRegObject( "Mars Shapes A-N", "Crane",                  MissionCreateObject, hCrane,             StaticShape, true, "hCrane.dts" );
MissionRegObject( "Mars Shapes A-N", "Civilian Garage",        MissionCreateObject, hMarsCivGarage,     StaticShape, true, "hMarsCivGarage.dts" );
MissionRegObject( "Mars Shapes A-N", "Comm Tower",             MissionCreateObject, hMarsCommTower,     StaticShape, true, "hMarsCommTower.dts" );
MissionRegObject( "Mars Shapes A-N", "Farm House",             MissionCreateObject, hMarsFarmHouse,     StaticShape, true, "hMarsFarmHouse.dts" );
MissionRegObject( "Mars Shapes A-N", "Fertillizer",            MissionCreateObject, hFertillizer,       StaticShape, true, "hFertillizer.dts" );
MissionRegObject( "Mars Shapes A-N", "Fusion Reactor",         MissionCreateObject, hMarsFusionReactor, StaticShape, true, "hMarsFusionReactor.dts" );
MissionRegObject( "Mars Shapes A-N", "Garage",                 MissionCreateObject, hGarage,            StaticShape, true, "hGarage.dts" );
MissionRegObject( "Mars Shapes A-N", "Hanger",                 MissionCreateObject, hMarsHanger,        StaticShape, true, "hMarsHanger.dts" );
MissionRegObject( "Mars Shapes A-N", "LandMine",               MissionCreateObject, hMarsMine,          StaticShape, true, "hMarsMine.dts" );
MissionRegObject( "Mars Shapes A-N", "Mass Driver",            MissionCreateObject, hMarsMassDriver,    StaticShape, true, "hMarsMassDriver.dts" );
MissionRegObject( "Mars Shapes A-N", "Meeting Hall",           MissionCreateObject, hMeethall,          StaticShape, true, "hMeethall.dts" );
MissionRegObject( "Mars Shapes A-N", "Mine Train",             MissionCreateObject, hMarsMineTrain,     StaticShape, true, "hMarsMineTrain.dts" );
MissionRegObject( "Mars Shapes A-N", "Mine Train Cargo",       MissionCreateObject, hMarsMineTrain_cargo,     StaticShape, true, "hMarsMineTrain_cargo.dts" );
MissionRegObject( "Mars Shapes A-N", "Monument",               MissionCreateObject, XMonument,          StaticShape, true,"xMonument.dts" );

#---MARS SHAPES (P-Z) ---------------
MissionRegObject( "Mars Shapes P-Z", "Perimeter Fence",            MissionCreateObject, hPerimeter,         StaticShape, true, "hPerimeter.dts" );
MissionRegObject( "Mars Shapes P-Z", "PileDriver",                 MissionCreateObject, hMarsPileDriver,    StaticShape, true, "hMarsPiledriver.dts" );
MissionRegObject( "Mars Shapes P-Z", "Police Flag",            	   MissionCreateObject, xMarsFlag_Police,   StaticShape, true, "xMarsFlag_Police.dts" );
MissionRegObject( "Mars Shapes P-Z", "Police (FIN)",               MissionCreateObject, hPolicefin,         StaticShape, true, "hPolicefin.dts" );
MissionRegObject( "Mars Shapes P-Z", "Police (HQ)",                MissionCreateObject, hPolicehq,          StaticShape, true, "hPolicehq.dts" );
MissionRegObject( "Mars Shapes P-Z", "Power (FIN)",                MissionCreateObject, hPowerfin,          StaticShape, true, "hPowerfin.dts" );
MissionRegObject( "Mars Shapes P-Z", "Rebel Flag",            	   MissionCreateObject, xMarsFlag_Rebel,    StaticShape, true, "xMarsFlag_Rebel.dts" );
MissionRegObject( "Mars Shapes P-Z", "Refinery",                   MissionCreateObject, hRefinery,          StaticShape, true, "hRefinery.dts" );
MissionRegObject( "Mars Shapes P-Z", "Residential Big",            MissionCreateObject, hMarsResidentialBig,    StaticShape, true, "hMarsResidentialBig.dts" );
MissionRegObject( "Mars Shapes P-Z", "Residential Small",          MissionCreateObject, hMarsResidentialSmall,  StaticShape, true, "hMarsResidentialSmall.dts" );
MissionRegObject( "Mars Shapes P-Z", "Residential Small NODOOR",   MissionCreateObject, hMarsResidentialSmall_NODOOR,  StaticShape, true, "hMarsResidentialSmall_NODOOR.dts" );
MissionRegObject( "Mars Shapes P-Z", "StreetLight",            	   MissionCreateObject, hMarsStreetlight,   StaticShape, true, "hMarsStreetlight.dts" );
MissionRegObject( "Mars Shapes P-Z", "Supply Dump",                MissionCreateObject, hMarsSupplyDump ,   StaticShape, true, "hMarsSupplyDump.dts" );
MissionRegObject( "Mars Shapes P-Z", "Tracking Station",           MissionCreateObject, hTracking,          StaticShape, true, "hTracking.dts" );
MissionRegObject( "Mars Shapes P-Z", "Water Tower",                MissionCreateObject, hMarsWaterTower,        StaticShape, true, "hMarsWaterTower.dts" );
MissionRegObject( "Mars Shapes P-Z", "Water Converter",            MissionCreateObject, hMarsWaterConverter,    StaticShape, true, "hMarsWaterConverter.dts" );
MissionRegObject( "Mars Shapes P-Z", "Uplink Station",             MissionCreateObject, hUplink,            StaticShape, true, "hUplink.dts" );
MissionRegObject( "Mars Shapes P-Z", "Wall (NORMAL)",              MissionCreateObject, hWall,              StaticShape, true,"hWall.dts" );
MissionRegObject( "Mars Shapes P-Z", "Big Wall 1",            	   MissionCreateObject, hMarsBigWall_1,     StaticShape, true, "hMarsBigWall_1.dts" );
MissionRegObject( "Mars Shapes P-Z", "Big Wall 2",            	   MissionCreateObject, hMarsBigWall_2,     StaticShape, true, "hMarsBigWall_2.dts" );
MissionRegObject( "Mars Shapes P-Z", "Big Wall 3",            	   MissionCreateObject, hMarsBigWall_3,     StaticShape, true, "hMarsBigWall_3.dts" );
MissionRegObject( "Mars Shapes P-Z", $separator);
MissionRegObject( "Mars Shapes P-Z", "Mars Rock, 1",               MissionCreateObject, xMarsrock1,         StaticShape, false,"xMarsrock1.dts" );
MissionRegObject( "Mars Shapes P-Z", "Mars Rock, 2",               MissionCreateObject, xMarsrock2,         StaticShape, false,"xMarsrock2.dts" );
MissionRegObject( "Mars Shapes P-Z", "Mars Rock, 3",               MissionCreateObject, xMarsrock3,         StaticShape, false,"xMarsrock3.dts" );

#--Mars Interior Shapes
MissionRegObject( "Mars Interior Shapes", "Bridge, sm",         MissionCreateObject, hbridge2m,          StaticInterior, "hbridge2m.dis" );
MissionRegObject( "Mars Interior Shapes", "Bridge, lg",         MissionCreateObject, hbridgem,           StaticInterior, "hbridgem.dis" );
MissionRegObject( "Mars Interior Shapes", "Bridge, post",          MissionCreateObject, hbridgepostm,       StaticInterior, "hbridgepostm.dis" );
MissionRegObject( "Mars Interior Shapes", "Garage",               MissionCreateObject, hgaragem,           StaticInterior, "hgaragem.dis" );
MissionRegObject( "Mars Interior Shapes", "Hangar ",              MissionCreateObject, hhangarm,           StaticInterior, "hhangarm.dis" );
MissionRegObject( "Mars Interior Shapes", "Landing Pad       ",   MissionCreateObject, hLandPadM,	       StaticInterior, "hLandPadM.dis");
MissionRegObject( "Mars Interior Shapes", "Landing Pad, lg ",   MissionCreateObject, hLandPad2M,	       StaticInterior, "hLandPad2M.dis");
MissionRegObject( "Mars Interior Shapes", "Mass Platform     ",   MissionCreateObject, hmassplatm,	       StaticInterior, "hmassplatm.dis");
MissionRegObject( "Mars Interior Shapes", "Platform" ,            MissionCreateObject, hplatformm,         StaticInterior, "hplatformm.dis" );
MissionRegObject( "Mars Interior Shapes", "Quarry ",              MissionCreateObject, hquarrym,           StaticInterior, "hquarrym.dis" );
MissionRegObject( "Mars Interior Shapes", "Heal Pad ",            MissionCreateObject, hheal2m,            StaticInterior, "hheal2m.dis" );
MissionRegObject( "Mars Interior Shapes", "Ammo Pad ",            MissionCreateObject, hammo2m,            StaticInterior, "hammo2m.dis" );
MissionRegObject( "Mars Interior Shapes", $separator );
MissionRegObject( "Mars Interior Shapes", "Fertillizer Ruin1 ",   MissionCreateObject, hfertruinm,	       StaticInterior, "hfertruinm.dis");
MissionRegObject( "Mars Interior Shapes", "Fertillizer Ruin2 ",   MissionCreateObject, hfertruin2m,        StaticInterior, "hfertruin2m.dis");
MissionRegObject( "Mars Interior Shapes", "Fertillizer Ruin3 ",   MissionCreateObject, hfertruin3m,	       StaticInterior, "hfertruin3m.dis");
MissionRegObject( "Mars Interior Shapes", "Building Ruin1 ",   MissionCreateObject, hruinsm,	       	   StaticInterior, "hRuinsM.dis");
MissionRegObject( "Mars Interior Shapes", "Building Ruin2 ",   MissionCreateObject, hruinsm2,	       StaticInterior, "hRuinsM2.dis");
MissionRegObject( "Mars Interior Shapes", "Building Ruin3 ",   MissionCreateObject, hruinsm3,	       StaticInterior, "hRuinsM3.dis");
MissionRegObject( "Mars Interior Shapes", "Building Ruin4 ",   MissionCreateObject, hruinsm4,	       StaticInterior, "hRuinsM4.dis");
MissionRegObject( "Mars Interior Shapes", "Building Ruin6 ",   MissionCreateObject, hruinsm6,	       StaticInterior, "hRuinsM6.dis");
MissionRegObject( "Mars Interior Shapes", $separator );
MissionRegObject( "Mars Interior Shapes", "Big House  ",   MissionCreateObject, hbighousem,	       StaticInterior, "hbighousem.dis");
MissionRegObject( "Mars Interior Shapes", "Big Hump   ",   MissionCreateObject, hBigHumpm,	       StaticInterior, "hbighumpm.dis");
MissionRegObject( "Mars Interior Shapes", "Big House2 ",   MissionCreateObject, hbighouse2m,	       StaticInterior, "hbighouse2m.dis");
MissionRegObject( "Mars Interior Shapes", "Big Gully  ",   MissionCreateObject, hBigGullym,	       StaticInterior, "hBigGullym.dis");
MissionRegObject( "Mars Interior Shapes", "Big Cargo  ",   MissionCreateObject, hBigCargom,	       StaticInterior, "hBigCargom.dis");
MissionRegObject( "Mars Interior Shapes", "Big Wall   ",   MissionCreateObject, hbigwall1m,	       StaticInterior, "hbigwall1m.dis");
MissionRegObject( "Mars Interior Shapes", "Big Ammo   ",   MissionCreateObject, hBigAmmo,	       StaticInterior, "hBigAmmo.dis");
MissionRegObject( "Mars Interior Shapes", "Big Healing",   MissionCreateObject, hBigHealing,	       StaticInterior, "hBigHealing.dis");
MissionRegObject( "Mars Interior Shapes", "Big Mole   ",         MissionCreateObject, hBigMolem,	       StaticInterior, "hbigmolem.dis");
MissionRegObject( "Mars Interior Shapes", $separator );
MissionRegObject( "Mars Interior Shapes", "Mars Tower 1",        MissionCreateObject, hbigmars1m,	       StaticInterior, "hbigmars1m.dis");
MissionRegObject( "Mars Interior Shapes", "Mars Tower 2",        MissionCreateObject, hbigmars2m,	       StaticInterior, "hbigmars2m.dis");
MissionRegObject( "Mars Interior Shapes", "Mars Tower 3",        MissionCreateObject, hbigmars3m,	       StaticInterior, "hbigmars3m.dis");
MissionRegObject( "Mars Interior Shapes", "Mars Tower 4",        MissionCreateObject, hbigmars4m,	       StaticInterior, "hbigmars4m.dis");
MissionRegObject( "Mars Interior Shapes", "Mars Tower 5",        MissionCreateObject, hbigmars5m,	       StaticInterior, "hbigmars5m.dis");
MissionRegObject( "Mars Interior Shapes", "Mars Tower 6",        MissionCreateObject, hbigmars6m,	       StaticInterior, "hbigmars6m.dis");
MissionRegObject( "Mars Interior Shapes", "Mars Tower Ruin 2",        MissionCreateObject, hbigmars2mruin,	       StaticInterior, "hbigmars2mruin.dis");
MissionRegObject( "Mars Interior Shapes", "Mars Tower Ruin 3",        MissionCreateObject, hbigmars3mruin,	       StaticInterior, "hbigmars3mruin.dis");



#---MERCURY SHAPES------------
MissionRegObject( "Mercury Shapes", "Mercury LandMine",        MissionCreateObject, hMercuryMine,       StaticShape, true, "hMercuryMine.dts" );
MissionRegObject( "Mercury Shapes", "Mercury Rock1",           MissionCreateObject, xmercuryrock1,      StaticShape, false,"xmercuryrock01.dts" );
MissionRegObject( "Mercury Shapes", "Mercury Rock2",           MissionCreateObject, xmercuryrock2,      StaticShape, false,"xmercuryrock02.dts" );
MissionRegObject( "Mercury Shapes", "Mercury Rock3",           MissionCreateObject, xmercuryrock3,      StaticShape, false,"xmercuryrock03.dts" );

#---MOON SHAPES--------------
MissionRegObject( "Moon Shapes", "Moon Barracks",              MissionCreateObject, hMoonbarracks,      StaticShape, true, "hMoonbarracks.dts" );
MissionRegObject( "Moon Shapes", "Moon Bigdish",               MissionCreateObject, hMoonbigdish,       StaticShape, true, "hMoonbigdish.dts" );
MissionRegObject( "Moon Shapes", "Moon Gather",                MissionCreateObject, hMoongather,        StaticShape, true, "hMoongather.dts" );
MissionRegObject( "Moon Shapes", "Moon Generator",             MissionCreateObject, hMoongenerator,     StaticShape, true, "hMoongenerator.dts" );
MissionRegObject( "Moon Shapes", "Moon Listen",                MissionCreateObject, hMoonlisten,        StaticShape, true, "hMoonlisten.dts" );
MissionRegObject( "Moon Shapes", "Moon Power",                 MissionCreateObject, hMoonpower,         StaticShape, true, "hMoonpower.dts" );
MissionRegObject( "Moon Shapes", "Moon Refinery",              MissionCreateObject, hMoonrefinery,      StaticShape, true, "hMoonrefinery.dts" );
MissionRegObject( "Moon Shapes", "Moon Residence",             MissionCreateObject, hMoonresidence,     StaticShape, true, "hMoonresidence.dts" );
MissionRegObject( "Moon Shapes", "Moon Silos",                 MissionCreateObject, hMoonsilos,         StaticShape, true, "hMoonsilos.dts" );
MissionRegObject( "Moon Shapes", "Moon Uplink",                MissionCreateObject, hMoonuplink,        StaticShape, true, "hMoonuplink.dts" );
MissionRegObject( "Moon Shapes", "Moon Wall",                  MissionCreateObject, hMoonwall,          StaticShape, false,"hMoonwall.dts" );
MissionRegObject( "Moon Shapes", "Moon Wallcap",               MissionCreateObject, hMoonwallcap,       StaticShape, false,"hMoonwallcap.dts" );
MissionRegObject( "Moon Shapes", "Moon Walldoor",              MissionCreateObject, hMoonwalldoor,      StaticShape, false,"hMoonwalldoor.dts" );
MissionRegObject( "Moon Shapes", "Moon Planet Defense",        MissionCreateObject, hMoonPlanetDefense, StaticShape, true, "hMoonPlanetDefense.dts" );
MissionRegObject( "Moon Shapes", $separator );
MissionRegObject( "Moon Shapes", "Moon Trooper Com",           MissionCreateObject, hMoonCom,           StaticShape, true, "hMoontroopercom.dts" );
MissionRegObject( "Moon Shapes", "Moon Trooper HQ",            MissionCreateObject, hMoonHQ,            StaticShape, true, "hMoontrooperhq.dts" );
MissionRegObject( "Moon Shapes", "Moon Trooper Silos",         MissionCreateObject, hMoonSilos,         StaticShape, true, "hMoontroopersilos.dts" );
MissionRegObject( "Moon Shapes", "Moon Trooper Storage",       MissionCreateObject, hMoonStorage,       StaticShape, true, "hMoontrooperstorage.dts" );
MissionRegObject( "Moon Shapes", "Moon Trooper Tracking",      MissionCreateObject, hMoontracking,      StaticShape, true, "hMoontrooptracking.dts" );
MissionRegObject( "Moon Shapes", $separator );
MissionRegObject( "Moon Shapes", "Landing Pad  (ZED)",         MissionCreateObject, hlandpadl,          StaticInterior, "hlandpadl.dis" );
MissionRegObject( "Moon Shapes", "Mine Hallway (ZED)",         MissionCreateObject, hminel,             StaticInterior, "hminel.dis" );
MissionRegObject( "Moon Shapes", "Mine Lift    (ZED)",         MissionCreateObject, hmineliftl,         StaticInterior, "hmineliftl.dis" );
MissionRegObject( "Moon Shapes", "Silo         (ZED)",         MissionCreateObject, hsilol,             StaticInterior, "hsilol.dis" );
MissionRegObject( "Moon Shapes", "Spaceport    (ZED)",         MissionCreateObject, hlunaport,          StaticInterior, "hlunaport.dis" );
missionRegObject( "Moon Shapes", $separator );
MissionRegObject( "Moon Shapes", "Moon Tower 1 (ZED)",         MissionCreateObject, hbigluna1,          StaticInterior, "hbigluna1.dis" );
MissionRegObject( "Moon Shapes", "Moon Tower 2 (ZED)",         MissionCreateObject, hbigluna2,          StaticInterior, "hbigluna2.dis" );
MissionRegObject( "Moon Shapes", "Moon Tower 3 (ZED)",         MissionCreateObject, hbigluna3,          StaticInterior, "hbigluna3.dis" );
MissionRegObject( "Moon Shapes", "Moon Tower 4 (ZED)",         MissionCreateObject, hbigluna4,          StaticInterior, "hbigluna4.dis" );
MissionRegObject( "Moon Shapes", "Moon Tower 5 (ZED)",         MissionCreateObject, hbigluna5,          StaticInterior, "hbigluna5.dis" );
MissionRegObject( "Moon Shapes", "Moon Tower 6 (ZED)",         MissionCreateObject, hbigluna6,          StaticInterior, "hbigluna6.dis" );
missionRegObject( "Moon Shapes", $separator );
MissionRegObject( "Moon Shapes", "Moon Rock1",                 MissionCreateObject, xmoonrock1,         StaticShape, false,"xmoonrock01.dts" );
MissionRegObject( "Moon Shapes", "Moon Rock2",                 MissionCreateObject, xmoonrock2,         StaticShape, false,"xmoonrock02.dts" );
MissionRegObject( "Moon Shapes", "Moon Rock3",                 MissionCreateObject, xmoonrock3,         StaticShape, false,"xmoonrock03.dts" );
MissionRegObject( "Moon Shapes", $separator );
MissionRegObject( "Moon Shapes", "Apollo",                       MissionCreateObject, xApollo,          StaticShape, true, "xApollo.dts" );
MissionRegObject( "Moon Shapes", "Moon Flag",                  MissionCreateObject, xMoonflag,          StaticShape, false,"xMoonflag.dts" );

#---PLUTO SHAPES------------
MissionRegObject( "Pluto Shapes", "Pluto Rock1",               MissionCreateObject, xplutorock1,        StaticShape, false,"xplutorock01.dts" );
MissionRegObject( "Pluto Shapes", "Pluto Rock2",               MissionCreateObject, xplutorock2,        StaticShape, false,"xplutorock02.dts" );
MissionRegObject( "Pluto Shapes", "Pluto Rock3",               MissionCreateObject, xplutorock3,        StaticShape, false,"xplutorock03.dts" );

#---TEMPERATE SHAPES---------
MissionRegObject( "Temperate Shapes", "Kong Civil",            MissionCreateObject, hKongcivil,         StaticShape, true, "hKongcivil.dts" );
MissionRegObject( "Temperate Shapes", "Kong Gate",             MissionCreateObject, hKonggate,          StaticShape, true, "hKonggate.dts" );
MissionRegObject( "Temperate Shapes", "Kong Gathering",        MissionCreateObject, hKonggathering,     StaticShape, true, "hKonggathering.dts" );
MissionRegObject( "Temperate Shapes", "Kong House",            MissionCreateObject, hKonghouse,         StaticShape, true, "hKonghouse.dts" );
MissionRegObject( "Temperate Shapes", "Kong Deck House",       MissionCreateObject, hKongdeckhouse,     StaticShape, true, "hKongdeckhouse.dts" );
MissionRegObject( "Temperate Shapes", "Kong Office",           MissionCreateObject, hKongoffice,        StaticShape, true, "hKongoffice.dts" );
MissionRegObject( "Temperate Shapes", "Kong Pagoda",           MissionCreateObject, hKongpagoda,        StaticShape, true, "hKongpagoda.dts" );
MissionRegObject( "Temperate Shapes", "Kong Sludge",           MissionCreateObject, hKongsludge,        StaticShape, true, "hKongsludge.dts" );
MissionRegObject( "Temperate Shapes", "Kong Temple",           MissionCreateObject, hKongtemple,        StaticShape, true, "hKongtemple.dts" );
MissionRegObject( "Temperate Shapes", "Kong High Temple",      MissionCreateObject, hKonglgtemple,      StaticShape, true, "hKonglgtemple.dts" );
MissionRegObject( "Temperate Shapes", $separator );
MissionRegObject( "Temperate Shapes", "Hong Temple (ZED)",	   MissionCreateObject, hHongTemple,		StaticInterior, "hHongTemple.dis" );
MissionRegObject( "Temperate Shapes", "Kong Heal   (ZED)",	   MissionCreateObject, heal,				StaticInterior, "hkongheal.dis" );
MissionRegObject( "Temperate Shapes", "Kong Reload (ZED)",	   MissionCreateObject, reload,				StaticInterior, "hkongammo.dis" );
MissionRegObject( "Temperate Shapes", $separator );
MissionRegObject( "Temperate Shapes", "Kong Tower 1",          MissionCreateObject, hbigkong1t,	       StaticInterior,  "hbigkong1t.dis");
MissionRegObject( "Temperate Shapes", "Kong Tower 2",          MissionCreateObject, hbigkong2t,	       StaticInterior,  "hbigkong2t.dis");
MissionRegObject( "Temperate Shapes", "Kong Tower 3",          MissionCreateObject, hbigkong3t,	       StaticInterior,  "hbigkong3t.dis");
MissionRegObject( "Temperate Shapes", "Kong Tower 4",          MissionCreateObject, hbigkong4t,	       StaticInterior,  "hbigkong4t.dis");
MissionRegObject( "Temperate Shapes", "Kong Tower 5",          MissionCreateObject, hbigkong5t,	       StaticInterior,  "hbigkong5t.dis");
MissionRegObject( "Temperate Shapes", "Kong Tower 6",          MissionCreateObject, hbigkong6t,	       StaticInterior,  "hbigkong6t.dis");
MissionRegObject( "Temperate Shapes", "Kong Tower 7",          MissionCreateObject, hbigkong7t,	       StaticInterior,  "hbigkong7t.dis");
MissionRegObject( "Temperate Shapes", "Kong Hide",             MissionCreateObject, hkonghidet,	       StaticInterior,  "hkonghide.dis");
MissionRegObject( "Temperate Shapes", "Kong Hide2",            MissionCreateObject, hkonghide2t,	   StaticInterior,  "hkonghide2.dis");


#---TITAN SHAPES-------------
MissionRegObject ( "Titan Shapes", "Titan Rock1",               MissionCreateObject, xtitanrock1,       StaticShape, false,"xtitanrock01.dts" );
MissionRegObject ( "Titan Shapes", "Titan Rock2",               MissionCreateObject, xtitanrock2,       StaticShape, false,"xtitanrock02.dts" );
MissionRegObject ( "Titan Shapes", "Titan Rock3",               MissionCreateObject, xtitanrock3,       StaticShape, false,"xtitanrock03.dts" );
MissionRegObject ("Titan Shapes",  "Titan Bigdish",             MissionCreateObject, hTitanbigdish,     StaticShape, true, "hTitanbigdish.DTS");
MissionRegObject ("Titan Shapes",  "Titan Gather",              MissionCreateObject, hTitangather,      StaticShape, true, "hTitangather.DTS");
MissionRegObject ("Titan Shapes",  "Titan Listen",     	        MissionCreateObject, hTitanlisten,         StaticShape, true, "hTitanlisten.DTS");
MissionRegObject ("Titan Shapes",  "Titan Power",               MissionCreateObject, hTitanpower,       StaticShape, true, "hTitanpower.DTS");
MissionRegObject( "Titan Shapes",  "Titan Microwave",           MissionCreateObject, hTitanMicrowave,    StaticShape, true,  "hTitanMicrowave.dts" );
MissionRegObject ("Titan Shapes",  "Titan Refinery",            MissionCreateObject, hTitanrefinery,    StaticShape, true, "hTitanrefinery.DTS");
MissionRegObject ("Titan Shapes",  "Titan Residence",           MissionCreateObject, hTitanresidence,   StaticShape, true, "hTitanresidence.DTS");
MissionRegObject ("Titan Shapes",  "Titan Silos",     	        MissionCreateObject, hTitansilos,          StaticShape, true, "hTitansilos.DTS");
MissionRegObject ("Titan Shapes",  "Titan Uplink",              MissionCreateObject, hTitanuplink,      StaticShape, true, "hTitanuplink.DTS");
MissionRegObject ("Titan Shapes",  "Titan Wall",     	        MissionCreateObject, hTitanwall,        StaticShape, false,"hTitanwall.DTS");
MissionRegObject ("Titan Shapes",  "Titan Wallcap",             MissionCreateObject, hTitanwallcap,     StaticShape, false,"hTitanwallcap.DTS");
MissionRegObject ("Titan Shapes",  "Titan Walldoor",            MissionCreateObject, hTitanwalldoor,    StaticShape, false,"hTitanwalldoor.DTS");
MissionRegObject ("Titan Shapes", $separator);
MissionRegObject ("Titan Shapes", "Titan Trooper Com",          MissionCreateObject, hTitantroopercom,  StaticShape, true, "hTitantroopercom.DTS");
MissionRegObject ("Titan Shapes", "Titan Trooper Command",      MissionCreateObject, hTitancommand,  	StaticShape, true, "hTitancommand.DTS");
MissionRegObject ("Titan Shapes", "Titan Trooper HQ",    	    MissionCreateObject, hTitantrooperhq,   StaticShape, true, "hTitantrooperhq.DTS");
MissionRegObject ("Titan Shapes", "Titan Trooper Residence",    MissionCreateObject, hTitantrooperresidenc,     StaticShape, true, "hTitantrooperresidence.DTS");
MissionRegObject ("Titan Shapes", "Titan Trooper Silos",        MissionCreateObject, hTitantroopsilos,  StaticShape, true, "hTitantroopsilos.DTS");
MissionRegObject ("Titan Shapes", "Titan Beam",                 MissionCreateObject, hTitanBeam,        StaticShape, true, "hTitanBEAM.DTS");
MissionRegObject ("Titan Shapes", "Titan Dias Irae",            MissionCreateObject, hTitanDiasIrae,    StaticShape, true, "hTitanDiasIrae.DTS");
MissionRegObject ("Titan Shapes", $separator);
MissionRegObject ("Titan Shapes", "Health Pad",    	          MissionCreateObject, hhealt,    	        StaticInterior,  "hhealt.dis");
MissionRegObject ("Titan Shapes", "Ammo Pad",    	          MissionCreateObject, hammot,    	        StaticInterior,  "hammot.dis");
MissionRegObject( "Titan Shapes", "Trooper Hanger (ZED)",	  MissionCreateObject, hTroophouseE,	    StaticInterior,  "hTroophouseE.dis" );

#---VENUS SHAPES-----------
MissionRegObject( "Venus Shapes", "Venus Array",               MissionCreateObject, hVenusarray,        StaticShape, true, "hVenusarray.dts" );
MissionRegObject( "Venus Shapes", "Venus Arbor",               MissionCreateObject, hVenusarbor,        StaticShape, true,  "hVenusArbor.dts" );
MissionRegObject( "Venus Shapes", "Venus Bigdish",             MissionCreateObject, hVenusbigdish,      StaticShape, true, "hVenusbigdish.dts" );
MissionRegObject( "Venus Shapes", "Venus Com",                 MissionCreateObject, hVenuscom,          StaticShape, true, "hVenuscom.dts" );
MissionRegObject( "Venus Shapes", "Venus Converter1",          MissionCreateObject, hVenusconverter1,   StaticShape, true, "hVenusconverter1.dts" );
MissionRegObject( "Venus Shapes", "Venus Converter2",          MissionCreateObject, hVenusconverter2,   StaticShape, true, "hVenusconverter2.dts" );
MissionRegObject( "Venus Shapes", "Venus Converter3",          MissionCreateObject, hVenusconverter3,   StaticShape, true, "hVenusconverter3.dts" );
MissionRegObject( "Venus Shapes", "Venus Dispertion",          MissionCreateObject, hVenusdispertion,   StaticShape, true, "hVenusdispertion.dts" );
MissionRegObject( "Venus Shapes", "Venus Drill",               MissionCreateObject, hVenusdrill,        StaticShape, true, "hVenusdrill.dts" );
MissionRegObject( "Venus Shapes", "Venus Eternal Flame",       MissionCreateObject, hVenusEternalFlame, StaticShape, true, "hVenusEternalFlame.dts" );
MissionRegObject( "Venus Shapes", "Venus Garage",              MissionCreateObject, hVenusgarage,       StaticShape, true, "hVenusgarage.dts" );
MissionRegObject( "Venus Shapes", "Venus Gathering",           MissionCreateObject, hVenusgathering,    StaticShape, true, "hVenusgathering.dts" );
MissionRegObject( "Venus Shapes", "Venus Geotherm",            MissionCreateObject, hVenusgeotherm,     StaticShape, true, "hVenusgeotherm.dts" );
MissionRegObject( "Venus Shapes", "Venus Lightrod",            MissionCreateObject, hVenuslightrod,     StaticShape, true, "hVenuslightrod.dts" );
MissionRegObject( "Venus Shapes", "Venus listening",           MissionCreateObject, hVenuslistening,    StaticShape, true, "hVenuslistening.dts" );
MissionRegObject( "Venus Shapes", "Venus Microwave",           MissionCreateObject, hVenusMicrowave,    StaticShape, true,  "hVenusMicrowave.dts" );
MissionRegObject( "Venus Shapes", "Venus Perimwall",           MissionCreateObject, hVenusperimwall,    StaticShape, false,"hVenusperimwall.dts" );
MissionRegObject( "Venus Shapes", "Venus Refinery",            MissionCreateObject, hVenusrefinery,     StaticShape, true, "hVenusrefinery.dts" );
MissionRegObject( "Venus Shapes", "Venus Small Garage",        MissionCreateObject, hVenussmgarage,     StaticShape, true, "hVenussmgarage.dts" );
MissionRegObject( "Venus Shapes", "Venus Small Wall",          MissionCreateObject, hVenussmwall,       StaticShape, false,"hVenussmwall.dts" );
MissionRegObject( "Venus Shapes", "Venus Storage",             MissionCreateObject, hVenusstorage,      StaticShape, true, "hVenusstorage.dts" );
MissionRegObject( "Venus Shapes", "Venus TR HQ",               MissionCreateObject, hVenustrhq,         StaticShape, true, "hVenustrhq.dts" );
MissionRegObject( "Venus Shapes", "Venus TR Link",             MissionCreateObject, hVenustrlink,       StaticShape, true, "hVenustrlink.dts" );
MissionRegObject( "Venus Shapes", $separator );
MissionRegObject( "Venus Shapes", "Venus Rock1",               MissionCreateObject, xvenusrock1,        StaticShape, false,"xvenusrock01.dts" );
MissionRegObject( "Venus Shapes", "Venus Rock2",               MissionCreateObject, xvenusrock2,        StaticShape, false,"xvenusrock02.dts" );
MissionRegObject( "Venus Shapes", "Venus Rock3",               MissionCreateObject, xvenusrock3,        StaticShape, false,"xvenusrock03.dts" );
#---Venus Interior Shapes------------
MissionRegObject( "Venus Interior Shapes", "Bridge        (ZED)",   MissionCreateObject, hbridgev,           StaticInterior, "hbridgev.dis" );
MissionRegObject( "Venus Interior Shapes", "Convertor     (ZED)",   MissionCreateObject, hconv,              StaticInterior, "hconv.dis" );
MissionRegObject( "Venus Interior Shapes", "Small Bridge  (ZED)",   MissionCreateObject, hsmlbridgev,        StaticInterior, "hsmlbridgev.dis" );
MissionRegObject( "Venus Interior Shapes", "Small Lift    (ZED)",   MissionCreateObject, hsmliftv,           StaticInterior, "hsmliftv.dis" );
MissionRegObject( "Venus Interior Shapes", "Small Oil Rig (ZED)",   MissionCreateObject, hsmloilv,           StaticInterior, "hsmloilv.dis" );
MissionRegObject( "Venus Interior Shapes", "Landing Pad   (ZED)",   MissionCreateObject, hlandpadv,          StaticInterior, "hlandpadv.dis" );
MissionRegObject( "Venus Interior Shapes", "Large Lift    (ZED)",   MissionCreateObject, hlgliftv,           StaticInterior, "hlgliftv.dis" );
MissionRegObject( "Venus Interior Shapes", "Large Oil Rig (ZED)",   MissionCreateObject, hlgoilv,            StaticInterior, "hlgoilv.dis" );
MissionRegObject( "Venus Interior Shapes", "Med Oil Rig   (ZED)",   MissionCreateObject, hmidoilv,           StaticInterior, "hmidoilv.dis" );
MissionRegObject( "Venus Interior Shapes", "Hub           (ZED)",   MissionCreateObject, hhubv,              StaticInterior, "hhubv.dis" );
MissionRegObject( "Venus Interior Shapes", "Heal          (ZED)",   MissionCreateObject, hhealv,             StaticInterior, "hhealv.dis" );
MissionRegObject( "Venus Interior Shapes", "Ammo          (ZED)",   MissionCreateObject, hammov,             StaticInterior, "hammov.dis" );
MissionRegObject( "Venus Interior Shapes", "Monument      (ZED)",   MissionCreateObject, hmonumentv,         StaticInterior, "hmonumentv.dis" );

#--------------------------------------------------------------------------------------------------------------------------------
MissionRegObject( $separator );


#----GENERIC SHAPES
MissionRegObject( "Generic Shapes", "Boxes",                 MissionCreateObject, xBoxes,            StaticShape, true, "xBoxes.dts");
MissionRegObject( "Generic Shapes", "Container",             MissionCreateObject, xContainer,        StaticShape, true, "xContainer.dts");
MissionRegObject( "Generic Shapes", "Fuel Tank",             MissionCreateObject, xfueltank,         StaticShape, true, "xFuelTank.dts");
MissionRegObject( "Generic Shapes", "Human Drop Pod",        MissionCreateObject, hDropPod,          StaticShape, true, "hDropPod.dts");
MissionRegObject( "Generic Shapes", "Monument",              MissionCreateObject, xMonument,         StaticShape, true, "xMonument.dts");
MissionRegObject( "Generic Shapes", "Target Drone",              MissionCreateObject, xObstacle,         StaticShape, true, "xobstacle.dts");
MissionRegObject( "Generic Shapes", "Trooper Container",     MissionCreateObject, xtroopercontainer, StaticShape, true, "xtroopercontainer.dts");
MissionRegObject( "Generic Shapes", $separator );
MissionRegObject( "Generic Shapes", "BridgeHub1 (ZED)",          MissionCreateObject, xbridgehub,         StaticInterior, "xbridgehub.dis");
MissionRegObject( "Generic Shapes", "BridgeHub2 (ZED)",          MissionCreateObject, xbridgehub2,        StaticInterior, "xbridgehub2.dis");
MissionRegObject( "Generic Shapes", "BridgeHub3 (ZED)",          MissionCreateObject, xbridgehub3,        StaticInterior, "xbridgehub3.dis");
MissionRegObject( "Generic Shapes", "BridgeHub4 (ZED)",          MissionCreateObject, xbridgehub4,        StaticInterior, "xbridgehub4.dis");
MissionRegObject( "Generic Shapes", $separator);
MissionRegObject( "Generic Shapes", "Yellow Flag",          MissionCreateObject, yellowFlag,       StaticShape, "team1.dts" );
MissionRegObject( "Generic Shapes", "Blue Flag",            MissionCreateObject, blueFlag,         StaticShape, "team2.dts" );
MissionRegObject( "Generic Shapes", "Red Flag",             MissionCreateObject, readFlag,         StaticShape, "team3.dts" );
MissionRegObject( "Generic Shapes", "Purple Flag",          MissionCreateObject, purpleFlag,       StaticShape, "team4.dts" );
MissionRegObject( "Generic Shapes", "Cryo Door",            MissionCreateObject, xCryodDoor,       StaticShape, "xCryodoor.dts" );
MissionRegObject( "Generic Shapes", $separator );
MissionRegObject( "Generic Shapes", "Movie House (ZED)",   MissionCreateObject, hmovie,           StaticInterior, "hmovie.dis" );
MissionRegObject( "Generic Shapes", $separator );
MissionRegObject( "Generic Shapes", "Tower 1",          MissionCreateObject, hhugescraper1,	       StaticInterior,  "hhugescraper1.dis");
MissionRegObject( "Generic Shapes", "Tower 2",          MissionCreateObject, hhugescraper2,	       StaticInterior,  "hhugescraper2.dis");
MissionRegObject( "Generic Shapes", "Tower 3",          MissionCreateObject, hhugescraper3,	       StaticInterior,  "hhugescraper3.dis");
MissionRegObject( "Generic Shapes", "Tower 4",          MissionCreateObject, hhugescraper4,	       StaticInterior,  "hhugescraper4.dis");
MissionRegObject( "Generic Shapes", "Tower 5",          MissionCreateObject, hhugescraper5,	       StaticInterior,  "hhugescraper5.dis");
MissionRegObject( "Generic Shapes", "Tower 6",          MissionCreateObject, hhugescraper6,	       StaticInterior,  "hhugescraper6.dis");
MissionRegObject( "Generic Shapes", "Tower 7",          MissionCreateObject, hhugescraper7,	       StaticInterior,  "hhugescraper7.dis");
MissionRegObject( "Generic Shapes", "Tower 8",          MissionCreateObject, hhugescraper8,	       StaticInterior,  "hhugescraper8.dis");

#----Heal/Ammo SHAPES
MissionRegObject( "Heal/Ammo Shapes", "Heal Lite",       MissionCreateObject, xheal_lite,          StaticShape, true, "xheal_lite.dts");
MissionRegObject( "Heal/Ammo Shapes", "Heal Dark",       MissionCreateObject, xheal_dark,          StaticShape, true, "xheal_dark.dts");
MissionRegObject( "Heal/Ammo Shapes", "Heal Venus",      MissionCreateObject, xheal_venus,         StaticShape, true, "xheal_venus.dts");
MissionRegObject( "Heal/Ammo Shapes", "Ammo Lite",       MissionCreateObject, xammo_lite,          StaticShape, true, "xammo_lite.dts");
MissionRegObject( "Heal/Ammo Shapes", "Ammo Dark",       MissionCreateObject, xammo_dark,          StaticShape, true, "xammo_dark.dts");
MissionRegObject( "Heal/Ammo Shapes", "Ammo Venus",      MissionCreateObject, xammo_venus,         StaticShape, true, "xammo_venus.dts");
MissionRegObject( "Heal/Ammo Shapes", "ZenAll Lite",     MissionCreateObject, xzen_lite,           StaticShape, true, "xzen_lite.dts");
MissionRegObject( "Heal/Ammo Shapes", "ZenAll Dark",     MissionCreateObject, xzen_dark,           StaticShape, true, "xzen_dark.dts");
MissionRegObject( "Heal/Ammo Shapes", $separator );
MissionRegObject( "Heal/Ammo Shapes", "Heal Lite (ZED)", MissionCreateObject, hHeallite,           StaticInterior, "hHeallite.dis");
MissionRegObject( "Heal/Ammo Shapes", "Heal Dark (ZED)", MissionCreateObject, hHealdark,           StaticInterior, "hHealdark.dis");
MissionRegObject( "Heal/Ammo Shapes", "Heal Venus(ZED)", MissionCreateObject, hHealvenus,          StaticInterior, "hHealvenus.dis");
MissionRegObject( "Heal/Ammo Shapes", "Ammo Lite (ZED)", MissionCreateObject, hAmmolite,           StaticInterior, "hAmmolite.dis");
MissionRegObject( "Heal/Ammo Shapes", "Ammo Dark (ZED)", MissionCreateObject, hAmmodark,           StaticInterior, "hAmmodark.dis");
MissionRegObject( "Heal/Ammo Shapes", "Ammo Venus(ZED)", MissionCreateObject, hAmmovenus,          StaticInterior, "hAmmovenus.dis");
MissionRegObject( "Heal/Ammo Shapes", "ZenAll Lite (ZED)", MissionCreateObject, hZenlite,           StaticInterior, "hZenlite.dis");
MissionRegObject( "Heal/Ammo Shapes", "ZenAll Dark (ZED)", MissionCreateObject, hZendark,           StaticInterior, "hZendark.dis");


#----Cinematic SHAPES
MissionRegObject( "Cinematic Shapes", "Big Tunnel (ZED)",       MissionCreateObject, hBigtunnel,        StaticInterior, "hBigtunnel.dis");
MissionRegObject( "Cinematic Shapes", "Asteroid",               MissionCreateObject, cin_astro1,        StaticInterior, "cin_astro1.DTS");
MissionRegObject( "Cinematic Shapes", "Banshee",                MissionCreateObject, cin_bans,          StaticInterior, "cin_bans.DTS");
MissionRegObject( "Cinematic Shapes", "Basilisk, empty",        MissionCreateObject, cin_baslempty,     StaticInterior, "cin_baslempty.DTS");
MissionRegObject( "Cinematic Shapes", "Basilisk, pilot",        MissionCreateObject, cin_baslhb,        StaticInterior, "cin_baslhb.dts");
MissionRegObject( "Cinematic Shapes", "Caanon",                 MissionCreateObject, cin_caanon,        StaticInterior, "cin_caanon.DTS");
MissionRegObject( "Cinematic Shapes", "Cyro Tubes",             MissionCreateObject, cin_cryo,          StaticInterior, "cin_cryo.DTS");
MissionRegObject( "Cinematic Shapes", "cydrop",                 MissionCreateObject, cin_cydrop,        StaticInterior, "cin_cydrop.DTS");
MissionRegObject( "Cinematic Shapes", "cydrop2",                MissionCreateObject, cin_cydrop2,       StaticInterior, "cin_cydrop2.DTS");
MissionRegObject( "Cinematic Shapes", "Cybrid Spacecraft",      MissionCreateObject, cin_cyspac,        StaticInterior, "cin_cyspac.DTS");
MissionRegObject( "Cinematic Shapes", "Cybrid Spacecraft, sm",  MissionCreateObject, cin_cyspacsm,      StaticInterior, "cin_cyspacsm.DTS");
MissionRegObject( "Cinematic Shapes", "DiesIrae Beam",          MissionCreateObject, cin_beam,          StaticInterior, "cin_beam.DTS");
MissionRegObject( "Cinematic Shapes", "drop",                   MissionCreateObject, cin_drop,          StaticInterior, "cin_drop.dts");
MissionRegObject( "Cinematic Shapes", "drop1",                  MissionCreateObject, cin_drop1,         StaticInterior, "cin_drop1.dts");
MissionRegObject( "Cinematic Shapes", "drop2",                  MissionCreateObject, cin_drop2,         StaticInterior, "cin_drop2.dts");
MissionRegObject( "Cinematic Shapes", "drop3",                  MissionCreateObject, cin_drop3,         StaticInterior, "cin_drop3.dts");
MissionRegObject( "Cinematic Shapes", "Escape Craft",           MissionCreateObject, cin_escape,        StaticInterior, "cin_escape.DTS");
MissionRegObject( "Cinematic Shapes", "Fire",                   MissionCreateObject, cin_fire,          StaticInterior, "cin_fire.DTS");
MissionRegObject( "Cinematic Shapes", "Mine Entrance",          MissionCreateObject, cin_mine,          StaticInterior, "cin_mine.DTS");
MissionRegObject( "Cinematic Shapes", "spac",                   MissionCreateObject, cin_spac,          StaticInterior, "cin_spac.DTS");
MissionRegObject( "Cinematic Shapes", $separator );
MissionRegObject( "Cinematic Shapes", "Earth",                  MissionCreateObject, cin_earth,         StaticInterior, "cin_earth.DTS");
MissionRegObject( "Cinematic Shapes", "Mars",                   MissionCreateObject, cin_mars,          StaticInterior, "cin_mars.DTS");
MissionRegObject( "Cinematic Shapes", "Mercury",                MissionCreateObject, cin_merc,          StaticInterior, "cin_merc.DTS");
MissionRegObject( "Cinematic Shapes", "Moon",                   MissionCreateObject, cin_moon,          StaticInterior, "cin_moon.DTS");
MissionRegObject( "Cinematic Shapes", "Pluto",                  MissionCreateObject, cin_pluto,         StaticInterior, "cin_pluto.dts");
MissionRegObject( "Cinematic Shapes", "Saturn",                 MissionCreateObject, cin_saturn,        StaticInterior, "cin_saturn.DTS");
MissionRegObject( "Cinematic Shapes", "Titan",                  MissionCreateObject, cin_titan,         StaticInterior, "cin_titan.DTS");
MissionRegObject( "Cinematic Shapes", "Venus",                  MissionCreateObject, cin_venus,         StaticInterior, "cin_venus.DTS");

#----Multiplayer SHAPES
MissionRegObject( "Multiplayer Shapes", "Low-Poly Monument",    MissionCreateObject, hLPMonument,        StaticShape,    "xMonument_LOW.dts");
MissionRegObject( "Multiplayer Shapes", $separator );
MissionRegObject( "Multiplayer Shapes", "Football Uprights (ZED)",   MissionCreateObject, football,   StaticInterior,   "football.dis");
MissionRegObject( "Multiplayer Shapes", "Teleporter (ZED)",    MissionCreateObject, teleport,         StaticInterior,   "teleport.dis");
MissionRegObject( "Multiplayer Shapes", "Teleporter Effect",   MissionCreateObject, fx_tele_t,        StaticShape, true, "fx_tele_t.dts");
}
schedule("exec(winMissionEditor);",0);