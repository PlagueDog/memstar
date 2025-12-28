//------------------------------------------------------------------------------
// This script defines a couple of tables.
//
// 1) A table of defaults for each action
// 2) A table of action map types and a description of what actions are allowed for each type
//
// All console commands are prefixed with 'ic' for inputConfig
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// Button and Axis defaults and constraints for each ACTION.
//
// defaultButtonAction(<action tag>, <make-break paired>, <make value>, <break value>, <optional moreInfoDesc>);
//  defaultAxialAction(<action tag>, <axisFlipped>, <deadzone>, <scale>, <centered>, <rangeType;linear;squared;cubed>, <optional moreInfoDesc>); 
//------------------------------------------------------------------------------                  
icDefaultButtonAction(IDACTION_FIRE,                     true,   1.0,  0.0);               		 
icDefaultButtonAction(IDACTION_ZOOM_MODE,                true,   1.0,  0.0);               		 

icDefaultButtonAction(IDACTION_WEAPON_SELECT,            false,  0.0,  0.0, *IDSUB_ACTION_SELECT_WEAPON_1);               		 
icDefaultButtonAction(IDACTION_WEAPON_SELECT,            false,  1.0,  0.0, *IDSUB_ACTION_SELECT_WEAPON_2);               		 
icDefaultButtonAction(IDACTION_WEAPON_SELECT,            false,  2.0,  0.0, *IDSUB_ACTION_SELECT_WEAPON_3);               		 
icDefaultButtonAction(IDACTION_WEAPON_SELECT,            false,  3.0,  0.0, *IDSUB_ACTION_SELECT_WEAPON_4);               		 
icDefaultButtonAction(IDACTION_WEAPON_SELECT,            false,  4.0,  0.0, *IDSUB_ACTION_SELECT_WEAPON_5);               		 
icDefaultButtonAction(IDACTION_WEAPON_SELECT,            false,  5.0,  0.0, *IDSUB_ACTION_SELECT_WEAPON_6);               		 

icDefaultButtonAction(IDACTION_WEAPON_GROUP_SELECT,      false,  0.0,  0.0, *IDSUB_ACTION_SELECT_WEAP_GROUP_1);               		 
icDefaultButtonAction(IDACTION_WEAPON_GROUP_SELECT,      false,  1.0,  0.0, *IDSUB_ACTION_SELECT_WEAP_GROUP_2);               		 
icDefaultButtonAction(IDACTION_WEAPON_GROUP_SELECT,      false,  2.0,  0.0, *IDSUB_ACTION_SELECT_WEAP_GROUP_3);               		 

icDefaultButtonAction(IDACTION_WEAPON_ADJ,               false,  1.0,  0.0, *IDSUB_ACTION_SELECT_NEXT_WEAPON);               		 
icDefaultButtonAction(IDACTION_WEAPON_ADJ,               false, -1.0,  0.0, *IDSUB_ACTION_SELECT_PREV_WEAPON);               		 
 icDefaultAxialAction(IDACTION_WEAPON_ADJ,               false,  0.1,  1.0, true,  linear);

icDefaultButtonAction(IDACTION_WEAPON_GROUP_ADJ,         false,  1.0,  0.0, *IDSUB_ACTION_SELECT_NEXT_WEAP_GROPUP);               		 
icDefaultButtonAction(IDACTION_WEAPON_GROUP_ADJ,         false, -1.0,  0.0, *IDSUB_ACTION_SELECT_PREV_WEAP_GROPUP);               		 
 icDefaultAxialAction(IDACTION_WEAPON_GROUP_ADJ,         false,  0.1,  1.0, true,  linear);

icDefaultButtonAction(IDACTION_WEAPON_MODE_SELECT,       false,  1.0,  0.0);               		 

icDefaultButtonAction(IDACTION_WEAPON_GROUP_TOGGLE,      false,  0.0,  0.0, *IDSUB_ACTION_WEAP_GROUP_1_TOGGLE);               		 
icDefaultButtonAction(IDACTION_WEAPON_GROUP_TOGGLE,      false,  1.0,  0.0, *IDSUB_ACTION_WEAP_GROUP_2_TOGGLE);               		 
icDefaultButtonAction(IDACTION_WEAPON_GROUP_TOGGLE,      false,  2.0,  0.0, *IDSUB_ACTION_WEAP_GROUP_3_TOGGLE);               		 
icDefaultButtonAction(IDACTION_WEAPON_GROUP_TOGGLE,      false,  3.0,  0.0, *IDSUB_ACTION_WEAP_GROUP_4_TOGGLE);               		 
icDefaultButtonAction(IDACTION_WEAPON_GROUP_TOGGLE,      false,  4.0,  0.0, *IDSUB_ACTION_WEAP_GROUP_5_TOGGLE);               		 
icDefaultButtonAction(IDACTION_WEAPON_GROUP_TOGGLE,      false,  5.0,  0.0, *IDSUB_ACTION_WEAP_GROUP_6_TOGGLE);               		 

icDefaultButtonAction(IDACTION_USE_SPECIAL,              false,  0.0,  0.0, *IDSUB_ACTION_SPECIAL_1);               		 
icDefaultButtonAction(IDACTION_USE_SPECIAL,              false,  1.0,  0.0, *IDSUB_ACTION_SPECIAL_2);               		 
icDefaultButtonAction(IDACTION_USE_SPECIAL,              false,  2.0,  0.0, *IDSUB_ACTION_SPECIAL_3);               		 
icDefaultButtonAction(IDACTION_USE_SPECIAL,              false,  3.0,  0.0, *IDSUB_ACTION_SPECIAL_4);               		 

icDefaultButtonAction(IDACTION_REACTOR,                  false,  1.0,  0.0);               		 

icDefaultButtonAction(IDACTION_USE_REACTOR_CAPACITOR,    false,  1.0,  0.0);               		  

icDefaultButtonAction(IDACTION_LOOK_X,                   true,   1.0,  0.0, *IDSUB_ACTION_LOOK_LEFT);               		 
icDefaultButtonAction(IDACTION_LOOK_X,                   true,  -1.0,  0.0, *IDSUB_ACTION_LOOK_RIGHT);               		 
 icDefaultAxialAction(IDACTION_LOOK_X,                   true,   0.0,  0.5, true,  linear);

icDefaultButtonAction(IDACTION_LOOK_Y,                   true,   1.0,  0.0, *IDSUB_ACTION_LOOK_UP);               		 
icDefaultButtonAction(IDACTION_LOOK_Y,                   true,  -1.0,  0.0, *IDSUB_ACTION_LOOK_DOWN);               		 
 icDefaultAxialAction(IDACTION_LOOK_Y,                   true,   0.0,  0.5, true,  linear);

icDefaultButtonAction(IDACTION_LOOK_CENTER,              false,  1.0,  0.0);               		 

icDefaultButtonAction(IDACTION_MOVE_X,                   true,  -1.0,  0.0, *IDSUB_ACTION_STRAFE_LEFT);               		 
icDefaultButtonAction(IDACTION_MOVE_X,                   true,   1.0,  0.0, *IDSUB_ACTION_STRAFE_RIGHT);               		 
 icDefaultAxialAction(IDACTION_MOVE_X,                   false,  0.1,  1.0, false, linear);

icDefaultButtonAction(IDACTION_MOVE_Y,                   true,   1.0,  0.0, *IDSUB_ACTION_MOVE_FORWARD);               		 
icDefaultButtonAction(IDACTION_MOVE_Y,                   true,  -1.0,  0.0, *IDSUB_ACTION_MOVE_BACKWARD);               		 
 icDefaultAxialAction(IDACTION_MOVE_Y,                   false,  0.1,  1.0, false, linear);

icDefaultButtonAction(IDACTION_MOVE_Z,                   true,   1.0,  0.0, *IDSUB_ACTION_MOVE_UP);               		 
icDefaultButtonAction(IDACTION_MOVE_Z,                   true,  -1.0,  0.0, *IDSUB_ACTION_MOVE_DOWN);               		 
 icDefaultAxialAction(IDACTION_MOVE_Z,                   false,  0.1,  1.0, false, linear);

icDefaultButtonAction(IDACTION_PITCH,                    true,   1.0,  0.0, *IDSUB_ACTION_PITCH_DOWN);               		 
icDefaultButtonAction(IDACTION_PITCH,                    true,  -1.0,  0.0, *IDSUB_ACTION_PITCH_UP);               		 
 icDefaultAxialAction(IDACTION_PITCH,                    false,  0.1,  1.0, false, linear);

icDefaultButtonAction(IDACTION_YAW,                      true,   1.0,  0.0, *IDSUB_ACTION_TURN_LEFT);               		 
icDefaultButtonAction(IDACTION_YAW,                      true,  -1.0,  0.0, *IDSUB_ACTION_TURN_RIGHT);               		 
 icDefaultAxialAction(IDACTION_YAW,                      false,  0.1,  1.0, true,  squared);

icDefaultButtonAction(IDACTION_ROLL,                     true,   1.0,  0.0, *IDSUB_ACTION_ROLL_LEFT);               	
icDefaultButtonAction(IDACTION_ROLL,                     true,  -1.0,  0.0, *IDSUB_ACTION_ROLL_RIGHT);               	
 icDefaultAxialAction(IDACTION_ROLL,                     false,  0.1,  1.0, false, linear);

icDefaultButtonAction(IDACTION_STOP,                     false,  1.0,  0.0);               		 

icDefaultButtonAction(IDACTION_SPEED,                    true,   1.0,  0.0, *IDSUB_ACTION_INCREASE_SPEED);        		 
icDefaultButtonAction(IDACTION_SPEED,                    true,  -1.0,  0.0, *IDSUB_ACTION_DECREASE_SPPED);        		 
 icDefaultAxialAction(IDACTION_SPEED,                    false,  0.1,  1.0, true,  squared);

icDefaultButtonAction(IDACTION_TURBO,                    false,  1.0,  0.0);        		 

icDefaultButtonAction(IDACTION_CROUCH,                   false,  1.0,  0.0);        		 
                                          
icDefaultButtonAction(IDACTION_CAMOUFLAGE,               false,  1.0,  0.0);        		 
                                           
icDefaultButtonAction(IDACTION_TARGET_ADJ_ENEMY,         false,  1.0,  0.0, *IDSUB_ACTION_TARGET_NEXT_ENEMY);        		 
icDefaultButtonAction(IDACTION_TARGET_ADJ_ENEMY,         false, -1.0,  0.0, *IDSUB_ACTION_TARGET_PREV_ENEMY);        		 
 icDefaultAxialAction(IDACTION_TARGET_ADJ_ENEMY,         false,  0.1,  1.0, true, linear);
                                           
icDefaultButtonAction(IDACTION_TARGET_ADJ_FRIENDLY,      false,  1.0,  0.0, *IDSUB_ACTION_TARGET_NEXT_FRIENDLY);        		 
icDefaultButtonAction(IDACTION_TARGET_ADJ_FRIENDLY,      false, -1.0,  0.0, *IDSUB_ACTION_TARGET_PREV_FRIENDLY);        		 
 icDefaultAxialAction(IDACTION_TARGET_ADJ_FRIENDLY,      false,  0.1,  1.0, true, linear);
                                           
icDefaultButtonAction(IDACTION_TARGET_CLOSEST_ENEMY,     false,  1.0,  0.0);        		  
                                           
icDefaultButtonAction(IDACTION_TARGET_CLOSEST_FRIENDLY,  false,  1.0,  0.0);        		  
                                           
icDefaultButtonAction(IDACTION_TARGET_SELECTED,          false,  1.0,  0.0);        		 

icDefaultButtonAction(IDACTION_USE_PROJECTILE_CAM,       false,  1.0,  0.0);        		 
                                           
icDefaultButtonAction(IDACTION_SPOT,                     true,   1.0, -1.0);        		  
                                           
icDefaultButtonAction(IDACTION_DROP_MARKER,              false,  1.0,  0.0);        		  
                                           
icDefaultButtonAction(IDACTION_SCAN_TARGET,              false,  1.0,  0.0);        		  
                                           
icDefaultButtonAction(IDACTION_SENSOR_MODE_TOGGLE,       false,  1.0,  0.0);        		 
                                           
icDefaultButtonAction(IDACTION_SENSOR_RANGE_TOGGLE,      false,  1.0,  0.0);        		 

icDefaultButtonAction(IDACTION_SENSOR_RANGE_SET,         false,  0.1,  0.0);        		 
                                           
icDefaultButtonAction(IDACTION_ECM_TOGGLE,	             false,  1.0,  0.0);        		  
                                           
icDefaultButtonAction(IDACTION_THERMAL_DIFFUSER_TOGGLE,  false,  1.0,  0.0);        		  
                                           
icDefaultButtonAction(IDACTION_SHIELD,                   false,  1.0,  0.0);        		 
                                           
icDefaultButtonAction(IDACTION_SHIELD_TRACK,             false,  1.0,  0.0);        		 
                                           
icDefaultButtonAction(IDACTION_SHIELD_FOCUS_ADJ,         false,  0.1,  0.0, *IDSUB_ACTION_NARROW_SHIELD_FOCUS);        		 
icDefaultButtonAction(IDACTION_SHIELD_FOCUS_ADJ,         false, -0.1,  0.0, *IDSUB_ACTION_WIDEN_SHIELD_FOCUS);        		 
 icDefaultAxialAction(IDACTION_SHIELD_FOCUS_ADJ,         false,  0.1,  0.5, true, linear);

icDefaultButtonAction(IDACTION_SHIELD_FOCUS_SET,         false,  1.0,  0.0);        		 
                                           
icDefaultButtonAction(IDACTION_SHIELD_ROTATION_ADJ,      false,  0.1,  0.0, *IDSUB_ACTION_ROTATE_SHIELD_LEFT);        		 
icDefaultButtonAction(IDACTION_SHIELD_ROTATION_ADJ,      false, -0.1,  0.0, *IDSUB_ACTION_ROTATE_SHIELD_RIGHT);        		 
 icDefaultAxialAction(IDACTION_SHIELD_ROTATION_ADJ,      false,  0.1,  0.5, true, linear);
                                      
icDefaultButtonAction(IDACTION_SHIELD_ROTATION_SET,      false,  1.0,  0.0);        		 
                                           
icDefaultButtonAction(IDACTION_USE_SHIELD_CAPACITOR,     false,  1.0,  0.0);        		  
                                           
icDefaultButtonAction(IDACTION_NAVPOINT_NEXT,            false,  1.0,  0.0);        		 
                                           
icDefaultButtonAction(IDACTION_NAVPOINT_PREV,            false,  1.0,  0.0);        		 

icDefaultButtonAction(IDACTION_NEXT,                     false,  1.0,  0.0);        		 
                       
icDefaultButtonAction(IDACTION_PREV,                     false,  1.0,  0.0);        		 
                                           
icDefaultButtonAction(IDACTION_TELL_TARGET,              false,  1.0,  0.0);        		 
                                           
icDefaultButtonAction(IDACTION_TELL_TEAM,                false,  1.0,  0.0);        		 
                                           
icDefaultButtonAction(IDACTION_TELL_ALL,                 false,  1.0,  0.0);        		 
                                           
icDefaultButtonAction(IDACTION_TOGGLE_CHAT_DISPLAY,      false,  1.0,  0.0);        		 
                                           
icDefaultButtonAction(IDACTION_QUICKCHAT,                false,  1.0,  0.0, *IDSUB_ACTION_QUICK_CHAT_1);        		 
icDefaultButtonAction(IDACTION_QUICKCHAT,                false,  2.0,  0.0, *IDSUB_ACTION_QUICK_CHAT_2);        		 
icDefaultButtonAction(IDACTION_QUICKCHAT,                false,  3.0,  0.0, *IDSUB_ACTION_QUICK_CHAT_3);        		 
icDefaultButtonAction(IDACTION_QUICKCHAT,                false,  4.0,  0.0, *IDSUB_ACTION_QUICK_CHAT_4);        		 
icDefaultButtonAction(IDACTION_QUICKCHAT,                false,  5.0,  0.0, *IDSUB_ACTION_QUICK_CHAT_5);        		 
icDefaultButtonAction(IDACTION_QUICKCHAT,                false,  6.0,  0.0, *IDSUB_ACTION_QUICK_CHAT_6);        		 
icDefaultButtonAction(IDACTION_QUICKCHAT,                false,  7.0,  0.0, *IDSUB_ACTION_QUICK_CHAT_7);        		 
icDefaultButtonAction(IDACTION_QUICKCHAT,                false,  8.0,  0.0, *IDSUB_ACTION_QUICK_CHAT_8);        		 

icDefaultButtonAction(IDACTION_TOGGLE_SCOREBOARD,        false,  1.0,  0.0);        		 
                                           
icDefaultButtonAction(IDACTION_TOGGLE_HUD_CONFIG,        false,  1.0,  0.0);        		 
                                           
icDefaultButtonAction(IDACTION_TOGGLE_PREF_CONFIG,       false,  1.0,  0.0);        		 
                                           
icDefaultButtonAction(IDACTION_REVERSE_THROTTLE,         false,  1.0,  0.0);        		 
                                           
icDefaultButtonAction(IDACTION_ZOOM_SET,                 false,  1.0,  0.0);        		 
                           
icDefaultButtonAction(IDACTION_ZOOM_ADJ,                 true,   1.0,  0.0, *IDSUB_ACTION_ZOOM_IN);        		 
icDefaultButtonAction(IDACTION_ZOOM_ADJ,                 true,  -1.0,  0.0, *IDSUB_ACTION_ZOOM_OUT);        		 
icDefaultButtonAction(IDACTION_ZOOM_ADJ,                 true,   1.0, -1.0, *IDSUB_ACTION_ZOOM_TEMP);        		 
 icDefaultAxialAction(IDACTION_ZOOM_ADJ,                 false,  0.1,  1.0, true,  linear);

icDefaultButtonAction(IDACTION_CENTER_TURRET,            false,  1.0,  0.0);        		 
                                           
icDefaultButtonAction(IDACTION_CENTER_BODY,              false,  1.0,  0.0);        		 
                                           
icDefaultButtonAction(IDACTION_AUTOTARGET,               false,  1.0,  0.0);        		 
                                           
icDefaultButtonAction(IDACTION_GENERAL_COMMAND,          false,  1.0,  0.0);        		  
                                           
icDefaultButtonAction(IDACTION_ORDER_SQUADMATE_1,        false,  1.0,  0.0);        		  
                                    
icDefaultButtonAction(IDACTION_ORDER_SQUADMATE_2,        false,  1.0,  0.0);        		  

icDefaultButtonAction(IDACTION_ORDER_SQUADMATE_3,        false,  1.0,  0.0);        		  
                                           
icDefaultButtonAction(IDACTION_ORDER_ALL_SQUADMATES,     false,  1.0,  0.0);        		  
                                           
icDefaultButtonAction(IDACTION_TOGGLE_NAME_TAG,          false,  1.0,  0.0);        		  
                                           
icDefaultButtonAction(IDACTION_SET_NAME_TAG,             true,   1.0,  0.0);        		  

icDefaultButtonAction(IDACTION_RESPAWN,                  false,  1.0,  0.0);        		  
                              
icDefaultButtonAction(IDACTION_EASY_CAMERA,              false,  1.0,  0.0);        		  

icDefaultButtonAction(IDACTION_FLY_CAMERA,               false,  1.0,  0.0);        		  

icDefaultButtonAction(IDACTION_ORBIT_CAMERA,             false,  1.0,  0.0);        		  

icDefaultButtonAction(IDACTION_PILOT_CAMERA,             false,  1.0,  0.0);        		  

icDefaultButtonAction(IDACTION_OUTLINE,                  false,  1.0,  0.0);        		  

icDefaultButtonAction(IDACTION_COLLISION_DETAIL,         false,  1.0,  0.0);        		  

icDefaultButtonAction(IDACTION_DROP_IN_GAME,             false,  1.0,  0.0);        		  

icDefaultButtonAction(IDACTION_EXIT_SIM,                 false,  1.0,  0.0);        		  

icDefaultButtonAction(IDACTION_MAP_VIEW,                 false,  1.0,  0.0);        		  

icDefaultButtonAction(IDACTION_PAUSE,                    false,  1.0,  0.0);        		  

icDefaultButtonAction(IDACTION_IGNORE_TOGGLE,            false,  1.0,  0.0);        		  

icDefaultButtonAction(IDACTION_FLUSH_CHAT,               false,  1.0,  0.0);        		     		  


//------------------------------------------------------------------------------
// Defines various action map types and describes which actions are available for
// each type
//------------------------------------------------------------------------------                                           

//--------------------------------------
// Herc.sae
//--------------------------------------
icNewActionMapType(*IDMAP_ACTION_VEHICLE, "herc.sae");
icActionAllowed(IDACTION_YAW);                    
icActionAllowed(IDACTION_SPEED);                  
icActionAllowed(IDACTION_LOOK_X);                 
icActionAllowed(IDACTION_LOOK_Y);
icActionAllowed(IDACTION_LOOK_CENTER);            
icActionAllowed(IDACTION_FIRE);                   
icActionAllowed(IDACTION_REVERSE_THROTTLE);       
icActionAllowed(IDACTION_CENTER_TURRET);          
icActionAllowed(IDACTION_CENTER_BODY);                  
icActionAllowed(IDACTION_STOP);                   
icActionAllowed(IDACTION_CROUCH);                 
icActionAllowed(IDACTION_MAP_VIEW);               
icActionAllowed(IDACTION_TOGGLE_NAME_TAG);        
icActionAllowed(IDACTION_SET_NAME_TAG);      
icActionAllowed(IDACTION_DROP_MARKER);            
icActionAllowed(IDACTION_WEAPON_SELECT);          
icActionAllowed(IDACTION_WEAPON_GROUP_SELECT);    
icActionAllowed(IDACTION_WEAPON_ADJ);             
icActionAllowed(IDACTION_WEAPON_GROUP_ADJ);       
icActionAllowed(IDACTION_WEAPON_MODE_SELECT);     
icActionAllowed(IDACTION_WEAPON_GROUP_TOGGLE); 
icActionAllowed(IDACTION_GENERAL_COMMAND);        
icActionAllowed(IDACTION_ORDER_SQUADMATE_1);      
icActionAllowed(IDACTION_ORDER_SQUADMATE_2);      
icActionAllowed(IDACTION_ORDER_SQUADMATE_3);      
icActionAllowed(IDACTION_ORDER_ALL_SQUADMATES); 
icActionAllowed(IDACTION_CAMOUFLAGE);             
icActionAllowed(IDACTION_USE_SHIELD_CAPACITOR);   
icActionAllowed(IDACTION_USE_REACTOR_CAPACITOR);  
icActionAllowed(IDACTION_TURBO);                  
icActionAllowed(IDACTION_TARGET_SELECTED);        
icActionAllowed(IDACTION_SPOT);             
icActionAllowed(IDACTION_SCAN_TARGET);            
icActionAllowed(IDACTION_TARGET_CLOSEST_ENEMY);   
icActionAllowed(IDACTION_TARGET_CLOSEST_FRIENDLY);
icActionAllowed(IDACTION_SENSOR_MODE_TOGGLE);     
icActionAllowed(IDACTION_SENSOR_RANGE_TOGGLE);    
icActionAllowed(IDACTION_SENSOR_RANGE_SET);   
icActionAllowed(IDACTION_TARGET_ADJ_ENEMY);       
icActionAllowed(IDACTION_TARGET_ADJ_FRIENDLY);    
icActionAllowed(IDACTION_REACTOR);                
icActionAllowed(IDACTION_NAVPOINT_NEXT);          
icActionAllowed(IDACTION_NAVPOINT_PREV);   
icActionAllowed(IDACTION_ZOOM_SET);               
icActionAllowed(IDACTION_ZOOM_ADJ);
icActionAllowed(IDACTION_SHIELD);                 
icActionAllowed(IDACTION_SHIELD_TRACK);           
icActionAllowed(IDACTION_SHIELD_FOCUS_ADJ);       
icActionAllowed(IDACTION_SHIELD_FOCUS_SET);       
icActionAllowed(IDACTION_SHIELD_ROTATION_ADJ);    
icActionAllowed(IDACTION_SHIELD_ROTATION_SET);  
icActionAllowed(IDACTION_ECM_TOGGLE);	          
icActionAllowed(IDACTION_THERMAL_DIFFUSER_TOGGLE);
icActionAllowed(IDACTION_USE_SPECIAL);
icActionAllowed(IDACTION_IGNORE_TOGGLE);


//--------------------------------------
// CameraOrbit.sae
//--------------------------------------
icNewActionMapType(*IDMAP_ACTION_ORBIT_CAMERA, "cameraOrbit.sae");
icActionAllowed(IDACTION_SPEED);                  
icActionAllowed(IDACTION_MOVE_Y);                 
icActionAllowed(IDACTION_YAW);                    
icActionAllowed(IDACTION_PITCH);                  
icActionAllowed(IDACTION_ZOOM_MODE);                  
icActionAllowed(IDACTION_PREV);                   
icActionAllowed(IDACTION_NEXT);                   

//--------------------------------------
// GameActions.sae
//--------------------------------------
icNewActionMapType(*IDMAP_ACTION_GENERAL, "gameActions.sae");
icActionAllowed(IDACTION_RESPAWN);      
icActionAllowed(IDACTION_PILOT_CAMERA);           
icActionAllowed(IDACTION_ORBIT_CAMERA);           
icActionAllowed(IDACTION_DROP_IN_GAME);           
icActionAllowed(IDACTION_TOGGLE_HUD_CONFIG);      
icActionAllowed(IDACTION_TOGGLE_PREF_CONFIG);     
icActionAllowed(IDACTION_TELL_TARGET);           
icActionAllowed(IDACTION_TELL_TEAM);              
icActionAllowed(IDACTION_TELL_ALL);               
icActionAllowed(IDACTION_TOGGLE_SCOREBOARD);      
icActionAllowed(IDACTION_TOGGLE_CHAT_DISPLAY);    
icActionAllowed(IDACTION_QUICKCHAT);              
icActionAllowed(IDACTION_PAUSE);              
icActionAllowed(IDACTION_FLUSH_CHAT);
icActionAllowed(IDACTION_COLLISION_DETAIL);
