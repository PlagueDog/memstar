
#------------- Sound/Music
#set $pref::useHardware3Dsound true

newSfx();
sfxSetFormat( 22050, 32, 1 );
newObject( soundVol, SimVolume, "sound.vol" );
newObject( genericSoundVol, SimVolume, "generic.vol" );
newObject( genericCybridSoundVol, SimVolume, "cybrid.vol" );
newObject( cinematicVol, SimVolume, "cinematic.vol" );


# optional modifiers
#     SFX_IS_STREAMING
#     SFX_IS_LOOPING

#add profiles
sfxAdd2Dprofile( IDPRF_2D, 0.0 );
sfxAdd2Dprofile( IDPRF_2D_LOOPING, 0.0, SFX_IS_LOOPING );
sfxAdd2Dprofile( IDPRF_COMP, 0.0 );
sfxAdd3Dprofile( IDPRF_WEAPON, 0.0, 20.0, 800.0, SFX_IS_PITCHING );
sfxAdd3Dprofile( IDPRF_NEAR, 0.0, 10.0, 100.0, SFX_IS_PITCHING);
sfxAdd3Dprofile( IDPRF_MEDIUM, 0.0, 10.0, 500.0, SFX_IS_PITCHING);
sfxAdd3Dprofile( IDPRF_FAR, 0.0, 10.0, 1000.0, SFX_IS_PITCHING);
sfxAdd3Dprofile( IDPRF_FIRE, 0.0, 20.0, 150.0, SFX_IS_LOOPING );
sfxAdd3Dprofile( IDPRF_EXP_SM, 0.0, 10.0, 200.0, SFX_IS_PITCHING);
sfxAdd3Dprofile( IDPRF_EXP_MED, 0.0, 20.0, 600.0, SFX_IS_PITCHING);
sfxAdd3Dprofile( IDPRF_EXP_LRG, 0.0, 30.0, 1000.0, SFX_IS_PITCHING);
sfxAdd3Dprofile( IDPRF_TANK, 0.0, 5.0, 200.0, SFX_IS_LOOPING);
sfxAdd3Dprofile( IDPRF_FLYER_LOOPING, 0.0, 5.0, 1000.0, SFX_IS_LOOPING );
sfxAdd3Dprofile( IDPRF_FLYER, 0.0, 5.0, 1000.0 );
sfxAdd3Dprofile( IDPRF_ALARM, 0.0, 5.0, 3500.0, SFX_IS_LOOPING );
sfxAdd2Dprofile( IDPRF_STREAMING, 0.0, SFX_IS_LOOPING, SFX_IS_STREAMING );

#--- Weapons
sfxAddPair( IDSFX_LAS,     IDPRF_WEAPON,    "sfx_las.wav", 1 );
sfxAddPair( IDSFX_HLAS,    IDPRF_WEAPON,    "sfx_hlas.wav", 1 );
sfxAddPair( IDSFX_CLAS,    IDPRF_WEAPON,    "sfx_clas.wav", 1 );
sfxAddPair( IDSFX_TLAS,    IDPRF_WEAPON,    "sfx_tlas.wav", 1 );
sfxAddPair( IDSFX_EMP,     IDPRF_WEAPON,    "sfx_emp.wav", 1 );
sfxAddPair( IDSFX_ELF,     IDPRF_WEAPON,    "sfx_elf.wav", 1 );
sfxAddPair( IDSFX_BLAS,    IDPRF_WEAPON,    "sfx_blas.wav", 1 );
sfxAddPair( IDSFX_HBLA,    IDPRF_WEAPON,    "sfx_hblas.wav", 1 );
sfxAddPair( IDSFX_PBW,     IDPRF_WEAPON,    "sfx_pbw.wav", 1 );
sfxAddPair( IDSFX_PLAS,    IDPRF_WEAPON,    "sfx_plas.wav", 1 );
sfxAddPair( IDSFX_BLNK,    IDPRF_WEAPON,    "sfx_blnk.wav", 1 );
sfxAddPair( IDSFX_QGUN,    IDPRF_WEAPON,    "sfx_qgun.wav", 1 );
sfxAddPair( IDSFX_MFAC,    IDPRF_WEAPON,    "sfx_mfac.wav", 1 );
sfxAddPair( IDSFX_NANO,    IDPRF_WEAPON,    "sfx_nano.wav", 1 );
sfxAddPair( IDSFX_NCAN,    IDPRF_WEAPON,    "sfx_ncan.wav", 1 );
sfxAddPair( IDSFX_ATC,     IDPRF_WEAPON,    "sfx_atc.wav", 1 );
sfxAddPair( IDSFX_HATC,    IDPRF_WEAPON,    "sfx_hatc.wav", 1 );
sfxAddPair( IDSFX_EMC,     IDPRF_WEAPON,    "sfx_emc.wav", 1 );
sfxAddPair( IDSFX_BC,      IDPRF_WEAPON,    "sfx_bc.wav", 1 );
sfxAddPair( IDSFX_HBC,     IDPRF_WEAPON,    "sfx_hbc.wav", 1 );
sfxAddPair( IDSFX_RAIL,    IDPRF_WEAPON,    "sfx_rail.wav", 1 );
sfxAddPair( IDSFX_LGB,     IDPRF_WEAPON,    "sfx_lgb.wav", 1 );
sfxAddPair( IDSFX_HAD,     IDPRF_WEAPON,    "sfx_had.wav", 1 );
sfxAddPair( IDSFX_VIP8,    IDPRF_WEAPON,    "sfx_vip.wav", 1 );
sfxAddPair( IDSFX_VIP12,   IDPRF_WEAPON,    "sfx_vip.wav", 1 );
sfxAddPair( IDSFX_SPR6,    IDPRF_WEAPON,    "sfx_spr.wav", 1 );
sfxAddPair( IDSFX_SPR10,   IDPRF_WEAPON,    "sfx_spr.wav", 1 );
sfxAddPair( IDSFX_SWRM,    IDPRF_WEAPON,    "sfx_swrm.wav", 1 );
sfxAddPair( IDSFX_MIN,     IDPRF_WEAPON,    "sfx_min.wav", 1 );
sfxAddPair( IDSFX_APH,     IDPRF_WEAPON,    "sfx_aph.wav", 1 );
sfxAddPair( IDSFX_SHRK,    IDPRF_WEAPON,    "sfx_shrk.wav", 1 );
sfxAddPair( IDSFX_ARA4,    IDPRF_WEAPON,    "sfx_ara.wav", 1 );
sfxAddPair( IDSFX_ARA8,    IDPRF_WEAPON,    "sfx_ara.wav", 1 );
sfxAddPair( IDSFX_ARA12,   IDPRF_WEAPON,    "sfx_ara.wav", 1 );
sfxAddPair( IDSFX_PRX6,    IDPRF_WEAPON,    "sfx_prx.wav", 1 );
sfxAddPair( IDSFX_PRX10,   IDPRF_WEAPON,    "sfx_prx.wav", 1 );
sfxAddPair( IDSFX_PRX15,   IDPRF_WEAPON,    "sfx_prx.wav", 1 );
sfxAddPair( IDSFX_CMD10,   IDPRF_WEAPON,    "sfx_cmd.wav", 1 );
sfxAddPair( IDSFX_CMD15,   IDPRF_WEAPON,    "sfx_cmd.wav", 1 );
sfxAddPair( IDSFX_CMD20,   IDPRF_WEAPON,    "sfx_cmd.wav", 1 );
sfxAddPair( IDSFX_NIKE,    IDPRF_WEAPON,    "sfx_nike.wav", 1 );
sfxAddPair( IDSFX_AML,     IDPRF_WEAPON,    "sfx_aml.wav", 1 );
sfxAddPair( IDSFX_RAD,     IDPRF_WEAPON,    "sfx_rad.wav", 1 );
sfxAddPair( IDSFX_DIS,     IDPRF_WEAPON,    "sfx_dis.wav", 1 );
sfxAddPair( IDSFX_EHUL,    IDPRF_WEAPON,    "sfx_ehul.wav", 1 );
sfxAddPair( IDSFX_PROB,    IDPRF_WEAPON,    "sfx_prob.wav", 1 );
sfxAddPair( IDSFX_FOG,     IDPRF_WEAPON,    "sfx_fog.wav", 1 );
sfxAddPair( IDSFX_SGUN,    IDPRF_WEAPON,    "sfx_sgun.wav", 1 );
                              


#--- Effects
sfxAddPair( IDSFX_EXP_SMALL, IDPRF_EXP_SM, "exploNear2.wav" );
sfxAddPair( IDSFX_EXP_MED, IDPRF_EXP_MED, "explo3.wav" );
sfxAddPair( IDSFX_EXP_LARGE, IDPRF_EXP_LRG, "explosionLarge.wav" );
sfxAddPair( IDSFX_FIRE, IDPRF_FIRE, "sfx_fire.wav" );
sfxAddPair( IDSFX_HITWALL, IDPRF_NEAR, "sfx_herc_bld.wav" );
sfxAddPair( IDSFX_HITVEHICLE, IDPRF_NEAR, "sfx_herchit1.wav" );
sfxAddPair( IDSFX_CAMOUFLAGE, IDPRF_2D, "bc_mfd.wav" );
sfxAddPair( IDSFX_BULLETARMOR, IDPRF_NEAR, "bullet_armor.wav" );
sfxAddPair( IDSFX_HERC_DEBRIS_BOUNCE, IDPRF_MEDIUM, "sfx_hitdebris.wav" );
sfxAddPair( IDSFX_TELEPORTER, IDPRF_NEAR, "teleporter.wav" );	
sfxAddPair( IDSFX_ELECTRICAL_BZZT, IDPRF_FIRE, "sfx_electrical_bzzt.wav" );	

#--- Shell
sfxAddPair( IDSFX_ROLLOVER, IDPRF_2D, "mous_ovr.wav" );
sfxAddPair( IDSFX_BUTTON_DOWN, IDPRF_2D, "button_down.wav" );
sfxAddPair( IDSFX_BUTTON_UP, IDPRF_2D, "button_up.wav" );
sfxAddPair( IDSFX_SHELL_MUSIC, IDPRF_STREAMING, "SHELL1.wav" );

#--- Human Computer Voice
sfxAddPair( IDCV_MISSION_COMPLETE,  IDPRF_COMP, "mission_comp.wav" );
sfxAddPair( IDCV_MISSION_FAILED,    IDPRF_COMP, "mission_fail.wav" );
sfxAddPair( IDCV_MISSION_ABORTED,   IDPRF_COMP, "mission_abort.wav" );
sfxAddPair( IDCV_NEW_OBJECTIVE,     IDPRF_COMP, "mission_obj_new.wav" );
sfxAddPair( IDCV_RULES_VIOLATED,    IDPRF_COMP, "rules_violated.wav" );
sfxAddPair( IDCV_BOUNDARY,          IDPRF_COMP, "boundary.wav" );
sfxAddPair( IDCV_60SEC_WARN,        IDPRF_COMP, "1min_2mis_end.wav" );
sfxAddPair( IDCV_30SEC_WARN,        IDPRF_COMP, "30sec_2mis_end.wav" );
sfxAddPair( IDCV_WEAPON_DESTROYED,  IDPRF_COMP, "weap_dest.wav" );     
sfxAddPair( IDCV_LOW_ENERGY,        IDPRF_COMP, "low_energy.wav" );     
sfxAddPair( IDCV_RELOAD_ENTER,      IDPRF_COMP, "reload_ent.wav" );     
sfxAddPair( IDCV_RELOAD_COMPLETE,   IDPRF_COMP, "reload_complete.wav" );     
sfxAddPair( IDCV_REPAIR_ENTER,      IDPRF_COMP, "repair_ent.wav" );     
sfxAddPair( IDCV_REPAIR_COMPLETE,   IDPRF_COMP, "repair_complete.wav" );     
sfxAddPair( IDCV_BLUE_CAPTURED,     IDPRF_COMP, "blue_flag_cap.wav" );     
sfxAddPair( IDCV_BLUE_STOLEN,       IDPRF_COMP, "blue_flag_stol.wav" );       
sfxAddPair( IDCV_BLUE_SURRENDERED,  IDPRF_COMP, "blue_flag_sur.wav" );  
sfxAddPair( IDCV_RED_CAPTURED,      IDPRF_COMP, "red_flag_cap.wav" );      
sfxAddPair( IDCV_RED_STOLEN,        IDPRF_COMP, "red_flag_stol.wav" );        
sfxAddPair( IDCV_RED_SURRENDERED,   IDPRF_COMP, "red_flag_sur.wav" );   
sfxAddPair( IDCV_PURPLE_CAPTURED,   IDPRF_COMP, "purp_flag_cap.wav" );   
sfxAddPair( IDCV_PURPLE_STOLEN,     IDPRF_COMP, "purp_flag_stol.wav" );     
sfxAddPair( IDCV_PURPLE_SURRENDERED,IDPRF_COMP, "purp_flag_sur.wav" );   
sfxAddPair( IDCV_YELLOW_CAPTURED,   IDPRF_COMP, "yel_flag_cap.wav" );   
sfxAddPair( IDCV_YELLOW_STOLEN,     IDPRF_COMP, "yel_flag_stol.wav" );     
sfxAddPair( IDCV_YELLOW_SURRENDERED,IDPRF_COMP, "yel_flag_sur.wav" );
sfxAddPair( IDCV_DLINK_ENGAGED,     IDPRF_COMP, "dlink_engaged.wav" );
sfxAddPair( IDCV_DLINK_ABORTED,     IDPRF_COMP, "dlink_abort.wav" );
sfxAddPair( IDCV_DLINK_COMPLETE,    IDPRF_COMP, "dlink_complete.wav" );
sfxAddPair( IDCV_NAVPOINT,          IDPRF_COMP, "navpoint.wav" );
sfxAddPair( IDCV_ACTIVE_SENSORS,    IDPRF_COMP, "active_sensors.wav" );
sfxAddPair( IDCV_PASSIVE_SENSORS,   IDPRF_COMP, "pass_sensors.wav" );
sfxAddPair( IDCV_SCANNING,          IDPRF_COMP, "trans_data.wav" );
sfxAddPair( IDCV_ECM_ON,            IDPRF_COMP, "jam_engaged.wav"  );
sfxAddPair( IDCV_ECM_OFF,           IDPRF_COMP, "jam_disabled.wav" );
sfxAddPair( IDCV_TDIFF_ON,          IDPRF_COMP, "therm_dif_on.wav" );
sfxAddPair( IDCV_TDIFF_OFF,         IDPRF_COMP, "therm_dif_off.wav" );
sfxAddPair( IDCV_ARTILLERY,         IDPRF_COMP, "Artillery_warn.wav" );
sfxAddPair( IDCV_DAM_INTERNAL,      IDPRF_COMP, "Internal_dam.wav" );
sfxAddPair( IDCV_DES_SHIELD,        IDPRF_COMP, "Dest_shieldgen.wav" );
sfxAddPair( IDCV_WRN_MISSILE,       IDPRF_COMP, "launch_warn2.wav" );
sfxAddPair( IDCV_MISSILE_LOCK,      IDPRF_COMP, "lock_warn.wav" );
sfxAddPair( IDCV_POWER_ON,          IDPRF_COMP, "Powerup.wav" );
sfxAddPair( IDCV_POWER_OFF,         IDPRF_COMP, "Shut_down.wav" );
sfxAddPair( IDCV_TARGETED,          IDPRF_COMP, "targeted.wav" );
sfxAddPair( IDCV_SHIELD_CRITICAL,   IDPRF_COMP, "shields_crit.wav" );
sfxAddpair( IDCV_REACTOR_DESTROYED, IDPRF_COMP, "dest_reactor.wav" );
sfxAddpair( IDCV_COMPUTER_DAMAGED,  IDPRF_COMP, "dam_computer.wav" );


#--- Cybrid Computer Voice
sfxAddPair( IDCV_CY_MISSION_COMPLETE,  IDPRF_COMP, "cy_mission_comp.wav" );
sfxAddPair( IDCV_CY_MISSION_FAILED,    IDPRF_COMP, "cy_mission_fail.wav" );
sfxAddPair( IDCV_CY_MISSION_ABORTED,   IDPRF_COMP, "cy_mission_abort.wav" );
sfxAddPair( IDCV_CY_NEW_OBJECTIVE,     IDPRF_COMP, "cy_mission_obj_new.wav" );
sfxAddPair( IDCV_CY_RULES_VIOLATED,    IDPRF_COMP, "cy_rules_violated.wav" );
sfxAddPair( IDCV_CY_BOUNDARY,          IDPRF_COMP, "cy_boundary.wav" );
sfxAddPair( IDCV_CY_60SEC_WARN,        IDPRF_COMP, "cy_1min_2mis_end.wav" );
sfxAddPair( IDCV_CY_30SEC_WARN,        IDPRF_COMP, "cy_30sec_2mis_end.wav" );
sfxAddPair( IDCV_CY_WEAPON_DESTROYED,  IDPRF_COMP, "cy_weap_dest.wav" );     
sfxAddPair( IDCV_CY_LOW_ENERGY,        IDPRF_COMP, "cy_low_energy.wav" );     
sfxAddPair( IDCV_CY_RELOAD_ENTER,      IDPRF_COMP, "cy_reload_ent.wav" );     
sfxAddPair( IDCV_CY_RELOAD_COMPLETE,   IDPRF_COMP, "cy_reload_complete.wav" );     
sfxAddPair( IDCV_CY_REPAIR_ENTER,      IDPRF_COMP, "cy_repair_ent.wav" );     
sfxAddPair( IDCV_CY_REPAIR_COMPLETE,   IDPRF_COMP, "cy_repair_complete.wav" );     
sfxAddPair( IDCV_CY_BLUE_CAPTURED,     IDPRF_COMP, "cy_blue_flag_cap.wav" );     
sfxAddPair( IDCV_CY_BLUE_STOLEN,       IDPRF_COMP, "cy_blue_flag_stol.wav" );       
sfxAddPair( IDCV_CY_BLUE_SURRENDERED,  IDPRF_COMP, "cy_blue_flag_sur.wav" );  
sfxAddPair( IDCV_CY_RED_CAPTURED,      IDPRF_COMP, "cy_red_flag_cap.wav" );      
sfxAddPair( IDCV_CY_RED_STOLEN,        IDPRF_COMP, "cy_red_flag_stol.wav" );        
sfxAddPair( IDCV_CY_RED_SURRENDERED,   IDPRF_COMP, "cy_red_flag_sur.wav" );   
sfxAddPair( IDCV_CY_PURPLE_CAPTURED,   IDPRF_COMP, "cy_purp_flag_cap.wav" );   
sfxAddPair( IDCV_CY_PURPLE_STOLEN,     IDPRF_COMP, "cy_purp_flag_stol.wav" );     
sfxAddPair( IDCV_CY_PURPLE_SURRENDERED,IDPRF_COMP, "cy_purp_flag_sur.wav" );   
sfxAddPair( IDCV_CY_YELLOW_CAPTURED,   IDPRF_COMP, "cy_yel_flag_cap.wav" );   
sfxAddPair( IDCV_CY_YELLOW_STOLEN,     IDPRF_COMP, "cy_yel_flag_stol.wav" );     
sfxAddPair( IDCV_CY_YELLOW_SURRENDERED,IDPRF_COMP, "cy_yel_flag_sur.wav" );
sfxAddPair( IDCV_CY_DLINK_ENGAGED,     IDPRF_COMP, "cy_dlink_engaged.wav" );
sfxAddPair( IDCV_CY_DLINK_ABORTED,     IDPRF_COMP, "cy_dlink_abort.wav" );
sfxAddPair( IDCV_CY_DLINK_COMPLETE,    IDPRF_COMP, "cy_dlink_complete.wav" );
sfxAddPair( IDCV_CY_NAVPOINT,          IDPRF_COMP, "cy_navpoint.wav" );
sfxAddPair( IDCV_CY_ACTIVE_SENSORS,    IDPRF_COMP, "cy_active_sensors.wav" );
sfxAddPair( IDCV_CY_PASSIVE_SENSORS,   IDPRF_COMP, "cy_pass_sensors.wav" );
sfxAddPair( IDCV_CY_SCANNING,          IDPRF_COMP, "cy_trans_data.wav" );
sfxAddPair( IDCV_CY_ECM_ON,            IDPRF_COMP, "cy_jam_engaged.wav"  );
sfxAddPair( IDCV_CY_ECM_OFF,           IDPRF_COMP, "cy_jam_disabled.wav" );
sfxAddPair( IDCV_CY_TDIFF_ON,          IDPRF_COMP, "cy_therm_dif_on.wav" );
sfxAddPair( IDCV_CY_TDIFF_OFF,         IDPRF_COMP, "cy_therm_dif_off.wav" );
sfxAddPair( IDCV_CY_ARTILLERY,         IDPRF_COMP, "cy_Artillery_warn.wav" );
sfxAddPair( IDCV_CY_DAM_INTERNAL,      IDPRF_COMP, "cy_Internal_dam.wav" );
sfxAddPair( IDCV_CY_DES_SHIELD,        IDPRF_COMP, "cy_Dest_shieldgen.wav" );
sfxAddPair( IDCV_CY_SHIELD_CRITICAL,   IDPRF_COMP, "cy_shields_crit.wav" );
sfxAddPair( IDCV_CY_POWER_ON,          IDPRF_COMP, "cy_Powerup.wav" );
sfxAddPair( IDCV_CY_POWER_OFF,         IDPRF_COMP, "cy_Shut_down.wav" );
sfxAddPair( IDCV_CY_TARGETED,          IDPRF_COMP, "cy_targeted.wav" );
sfxAddPair( IDCV_CY_MISSILE_LOCK,      IDPRF_COMP, "cy_lock_warn.wav" );
sfxAddPair( IDCV_CY_WRN_MISSILE,       IDPRF_COMP, "cy_launch_warn2.wav" );
sfxAddpair( IDCV_CY_REACTOR_DESTROYED, IDPRF_COMP, "cy_dest_reactor.wav" );
sfxAddpair( IDCV_CY_COMPUTER_DAMAGED,  IDPRF_COMP, "cy_dam_computer.wav" );

#--- Vehicle
sfxAddPair( IDSFX_TANK_ENGINE_0,    IDPRF_TANK, "sfx_tank0.wav", 1 );
sfxAddPair( IDSFX_TANK_ENGINE_1,    IDPRF_TANK, "sfx_tank1.wav", 1 );
sfxAddPair( IDSFX_TANK_ENGINE_2,    IDPRF_TANK, "sfx_tank2.wav", 1 );
sfxAddPair( IDSFX_TANK_ENGINE_3,    IDPRF_TANK, "sfx_tank3.wav", 1 );
sfxAddPair( IDSFX_TANK_ENGINE_4,    IDPRF_TANK, "sfx_tank4.wav", 1 );
sfxAddPair( IDSFX_HERC_ENGINE_0,    IDPRF_TANK, "hercsmall.wav", 1 );
sfxAddPair( IDSFX_HERC_ENGINE_1,    IDPRF_TANK, "hercmed.wav", 1 );
sfxAddPair( IDSFX_HERC_ENGINE_2,    IDPRF_TANK, "hercbig.wav", 1 );

sfxAddPair( IDSFX_FLYER_ON,    IDPRF_FLYER, "sfx_flyer_on.wav", 1 );
sfxAddPair( IDSFX_FLYER_OFF,    IDPRF_FLYER, "sfx_flyer_off.wav", 1 );
sfxAddPair( IDSFX_FLYER_ENGINE,    IDPRF_FLYER_LOOPING, "sfx_flyer.wav", 1 );
sfxAddPair( IDSFX_FLYER_DAMAGED,    IDPRF_FLYER_LOOPING, "sfx_flyer_damaged.wav", 1 );

#--- Misc
sfxAddPair( IDSFX_FOOTFALL_TERRAIN, IDPRF_NEAR, "step.wav", 1 );
sfxAddPair( IDSFX_FOOTFALL_ZED,     IDPRF_NEAR, "bigfoot_l.wav", 1 );
sfxAddPair( IDSFX_DROP,             IDPRF_2D, "drop.wav", 1 );
sfxAddPair( IDSFX_BEEP,             IDPRF_2D, "bptslct.wav", 1 );

sfxAddPair( IDSFX_REACTOR_ON,       IDPRF_2D, "powerOn.wav", 1 );
sfxAddPair( IDSFX_REACTOR_OFF,      IDPRF_2D, "powerOff.wav", 1 );

sfxAddPair( IDSFX_ENEMY_SELECT,     IDPRF_2D, "trgSlct.wav", 1 );
sfxAddPair( IDSFX_FRIENDLY_SELECT,  IDPRF_2D, "friendSlct.wav", 1 );

sfxAddPair( IDSFX_COMPUTER_ON,      IDPRF_2D, "comp_on.wav", 1 );
sfxAddPair( IDSFX_COMPUTER_OFF,     IDPRF_2D, "comp_off.wav", 1 );

sfxAddPair( IDSFX_NO_TARGET,        IDPRF_2D, "button_down.wav", 1 );
sfxAddPair( IDSFX_NEW_MESSAGE,      IDPRF_2D, "chatDisp.wav", 1 );

sfxAddPair( IDSFX_PILOT_HIT,        IDPRF_2D, "sfx_pilot_hit.wav", 1 );

sfxAddPair( IDSFX_SHIELD_ON,        IDPRF_2D, "sfx_shield_on.wav", 1 );
sfxAddPair( IDSFX_SHIELD_OFF,       IDPRF_2D, "sfx_shield_off.wav", 1 );

sfxAddPair( IDSFX_SPOT_ON,          IDPRF_2D_LOOPING, "sfx_spot_locking.wav", 1 );
sfxAddPair( IDSFX_SPOT_LOCK,        IDPRF_2D, "sfx_spot_locked.wav", 1 );

sfxAddPair( IDSFX_ROCKET_BOOST_ON,  IDPRF_2D, "sfx_rocket_on.wav", 1 );
sfxAddPair( IDSFX_ROCKET_BOOST_OFF, IDPRF_2D, "sfx_rocket_off.wav", 1 );

sfxAddPair( IDSFX_TURBINE_BOOST_ON,  IDPRF_2D, "sfx_turbo_on.wav", 1 );
sfxAddPair( IDSFX_TURBINE_BOOST_OFF, IDPRF_2D, "sfx_turbo_off.wav", 1 );

sfxAddPair(IDSFX_SHIELD_CAP_USE,       IDPRF_2D, "sfx_shieldcap.wav", 1 );
sfxAddPair(IDSFX_SHIELD_CAP_BACKFIRE,  IDPRF_2D, "sfx_shieldcap_backfire.wav", 1 );
sfxAddPair(IDSFX_SHIELD_CAP_NOTREADY,  IDPRF_2D, "sfx_shieldcap_unready.wav", 1 );

sfxAddPair(IDSFX_REACTOR_CAP_USE,       IDPRF_2D, "sfx_reactorcap.wav", 1 );
sfxAddPair(IDSFX_REACTOR_CAP_BACKFIRE,  IDPRF_2D, "sfx_reactorcap_backfire.wav", 1 );
sfxAddPair(IDSFX_REACTOR_CAP_NOTREADY,  IDPRF_2D, "sfx_reactorcap_unready.wav", 1 );

sfxAddPair(IDSFX_INCOMING_ARTILLERY, IDPRF_FAR, "sfx_incoming.wav" );	
sfxAddPair(IDSFX_REACTOR_EXPLODE,  IDPRF_NEAR, "sfx_reactor_xplo.wav" );

##----Ice
sfxAddPair(IDSFX_AMBIENT_ICE1,  IDPRF_2D, "sfx_ice1.wav" );	        	
sfxAddPair(IDSFX_AMBIENT_ICE2,  IDPRF_2D, "sfx_ice2.wav" );		        	
sfxAddPair(IDSFX_AMBIENT_ICE3,  IDPRF_2D, "sfx_ice3.wav" );		        	
sfxAddPair(IDSFX_AMBIENT_AVALANCHE,  IDPRF_2D, "sfx_avalanche.wav" );				

##----Venus
sfxAddPair(IDSFX_AMBIENT_LAVA,  IDPRF_2D,  "sfx_lava.wav" );	        	

##----Titan
sfxAddPair(IDSFX_AMBIENT_METEOR,  IDPRF_2D,	"sfx_meteor.wav" );  		
sfxAddPair(IDSFX_AMBIENT_GLOOP,  IDPRF_2D,	"sfx_swamp.wav" );			

##----Mercury
sfxAddPair(IDSFX_AMBIENT_QUAKE,  IDPRF_2D, "sfx_quake.wav" );	  			

##----Temperate
sfxAddPair(IDSFX_AMBIENT_RAIN,  IDPRF_2D_LOOPING,	"sfx_rain.wav" );  			
sfxAddPair(IDSFX_AMBIENT_THUNDER1,  IDPRF_2D, "sfx_thunder1.wav" );	  		
sfxAddPair(IDSFX_AMBIENT_THUNDER2,  IDPRF_2D, "sfx_thunder2.wav" );	  		

##----Mars
//sfxAddPair(IDSFX_AMBIENT_WINDGUST,  IDPRF_2D, "sfx_windgust.wav" );	  		
sfxAddPair(IDSFX_AMBIENT_WIND1,  IDPRF_2D_LOOPING,	  "sfx_windloop.wav" );			
sfxAddPair(IDSFX_AMBIENT_WIND2,  IDPRF_2D_LOOPING,	  "sfx_windloop2.wav" );						

##----Human Ambient Sounds
sfxAddPair(IDSFX_AMBIENT_HUMAN_INDUSTRIAL1,  IDPRF_NEAR,	"sfx_machine1.wav" );
sfxAddPair(IDSFX_AMBIENT_HUMAN_INDUSTRIAL2,  IDPRF_NEAR,	"sfx_machine2.wav" );
sfxAddPair(IDSFX_AMBIENT_HUMAN_INDUSTRIAL3,  IDPRF_NEAR,	"sfx_machine3.wav" );
sfxAddPair(IDSFX_AMBIENT_HUMAN_RADAR,  IDPRF_NEAR,		"sfx_radar_station.wav" );

##----Cybrid Ambient Sounds
sfxAddPair(IDSFX_AMBIENT_CYBRID_COM,  IDPRF_NEAR,			"sfx_cybrid_com.wav" );
sfxAddPair(IDSFX_AMBIENT_CYBRID_INDUSTRY,  IDPRF_NEAR,	"sfx_cybrid_indust.wav" );

##----General Industrial Sounds
sfxAddPair(IDSFX_AMBIENT_INDUSTRIAL,  IDPRF_NEAR,			"sfx_industrial.wav" );
sfxAddPair(IDSFX_AMBIENT_STEAM,  IDPRF_NEAR,				"sfx_steam.wav" );

##----Alarm Sounds
sfxAddPair(IDSFX_ALARM,				IDPRF_ALARM, "alarm2.wav" );			
sfxAddPair(IDSFX_SIREN,				IDPRF_ALARM, "sfx_siren.wav" );			
sfxAddPair(IDSFX_CYBRID_ALARM,		IDPRF_ALARM, "sfx_cybrid_alarm.wav" );

##---Special Sounds for Dies Irae and orbital guns

sfxAddPair(IDSFX_BIG_BEAM,   	IDPRF_ALARM,	"sfx_bigbeam.wav" );
sfxAddPair(IDSFX_BIG_BEAM_FIRE,	IDPRF_FAR,	"sfx_bigbeam_fire.wav" );
sfxAddPair(IDSFX_ORBITAL_GUN,  	IDPRF_FAR,	"sfx_orbital_gun.wav" );
sfxOpen();