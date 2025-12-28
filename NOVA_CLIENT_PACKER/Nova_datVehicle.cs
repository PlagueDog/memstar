# Terrain plain
buildHerc( 1, "H", "datHerc_tr_apoc.cs" );
buildHerc( 2, "H", "datHerc_tr_mino.cs" );
buildHerc( 3, "H", "datHerc_tr_gorg.cs" );
buildHerc( 4, "H", "datHerc_tr_talo.cs" );
buildHerc( 5, "H", "datHerc_tr_basl.cs" );
buildTank( 6, "H", "datTank_tr_pala.cs" );
buildTank( 7, "H", "datTank_tr_myrm.cs" );
buildTank( 8, "H", "datTank_tr_disr.cs" );
buildFlyer(9, "datFlyer_tr_bans.cs" );

# Knights
buildHerc( 10, "H", "datHerc_kn_apoc.cs" );
buildHerc( 11, "H", "datHerc_kn_mino.cs" );
buildHerc( 12, "H", "datHerc_kn_gorg.cs" );
buildHerc( 13, "H", "datHerc_kn_talo.cs" );
buildHerc( 14, "H", "datHerc_kn_basl.cs" );
buildTank( 15, "H", "datTank_kn_pala.cs" );
buildTank( 16, "H", "datTank_kn_myrm.cs" );
buildTank( 17, "H", "datTank_kn_disr.cs" );

# Support vehicles
buildFlyer(18, "datFlyer_tr_carg.cs" );
buildFlyer(19, "datFlyer_tr_escape.cs" );

# Cybrids
buildHerc( 20, "C", "datHerc_cy_seek.cs" );
buildHerc( 21, "C", "datHerc_cy_goad.cs" );
buildHerc( 22, "C", "datHerc_cy_shep.cs" );
buildHerc( 23, "C", "datHerc_cy_adju.cs" );
buildHerc( 24, "C", "datHerc_cy_exec.cs" );
buildTank( 25, "C", "datTank_cy_bolo.cs" );
buildHerc( 27, "C", "datHerc_pl_adju.cs" );
buildHerc( 28, "C", "datHerc_pl_exec.cs" );
buildTank( 26, "C", "datTank_cy_recl.cs" );
buildHerc( 29, "C", "datHerc_cy_prom.cs" );

# Rebels
buildHerc( 30, "H", "datHerc_rb_eman.cs" );
buildTank( 31, "H", "datTank_rb_aven.cs" );
buildTank( 32, "H", "datTank_rb_drea.cs" );
buildHerc( 33, "H", "datHerc_rb_oly.cs" );


#Metagen
buildHerc( 35, "C", "datHerc_mg_seek.cs" );
buildHerc( 36, "C", "datHerc_mg_goad.cs" );
buildHerc( 37, "C", "datHerc_mg_shep.cs" );
buildHerc( 38, "C", "datHerc_mg_adju.cs" );
buildHerc( 39, "C", "datHerc_mg_exec.cs" );

# Misc
buildHerc( 40, "H", "datHerc_ha_apoc.cs" );
buildTank( 41, "H", "datTank_ha_pred.cs" );
buildHerc( 42, "H", "datHerc_ca_basl.cs" );
buildHerc( 43, "H", "datHerc_cin_apoc.cs" );
buildHerc( 44, "H", "datHerc_cin_basl.cs" );
buildTank( 45, "H", "datTank_SuperPred.cs" );

# Pirates
buildHerc( 50, "H", "datHerc_pi_apoc.cs" );
buildTank( 51, "H", "datTank_pi_drea.cs" );
buildHerc( 52, "H", "datHerc_pi_eman.cs" );

# More Cybrid (player pilotable platinum guard versions
buildHerc( 55, "C", "datHerc_pl_adju2.cs" );
buildHerc( 56, "C", "datHerc_pl_exec2.cs" );


# Drones
exec("datDroneGeneric.cs");
buildDrone( 60, "datDrone_tr_fltb.cs");
buildDrone( 61, "datDrone_tr_ammo.cs");
buildDrone( 62, "datDrone_tr_big_ammo.cs");
buildDrone( 63, "datDrone_tr_big_personnel.cs");     
buildDrone( 64, "datDrone_tr_fuel.cs");      
buildDrone( 65, "datDrone_tr_mino.cs");          
buildDrone( 66, "datDrone_rb_fltb.cs");               
buildDrone( 67, "datDrone_rb_ammo.cs");           
buildDrone( 68, "datDrone_rb_big.cs");
buildDrone( 69, "datDrone_rb_big_box.cs");
buildDrone( 70, "datDrone_rb_box.cs");
buildDrone( 71, "datDrone_tr_util.cs");
buildDrone( 72, "datDrone_rb_thum.cs");
buildDrone( 73, "datDrone_tr_star.cs");
buildDrone( 135, "datDrone_tr_sove.cs" );
buildDrone( 136, "datDrone_tr_surv.cs" );

# More cybrid (can't change id's anymore)
buildTank( 90, "C", "datTank_cy_artl.cs" );
buildFlyer( 91, "datFlyer_cy_advo.cs" );
buildFlyer( 92, "datFlyer_cy_drop.cs" );
buildFlyer( 93, "datFlyer_cy_cons.cs" );
buildDrone( 94, "datDrone_cy_omni.cs" );
buildDrone( 95, "datDrone_cy_prot.cs" );
buildDrone( 96, "datDrone_cy_jamm.cs" );

# More knights
buildFlyer( 110, "datFlyer_kn_bans.cs" );
buildFlyer( 111, "datFlyer_kn_drop.cs" );

# More Terrain vehicles
buildFlyer( 130, "datFlyer_tr_drop.cs" );
buildFlyer( 131, "datFlyer_tr_drac.cs" );
buildFlyer( 132, "datFlyer_tr_conv.cs" );
buildTank(  133, "H", "datTank_tr_nike.cs" );
buildTank(  134, "H", "datTank_tr_supr.cs" );

# More Rebels
buildTank(  137, "H", "datTank_rb_artl.cs" );
buildTank(  138, "H", "datTank_rb_bike.cs" );


#Starsiege Magic Bus
buildTank(  150, "H", "datTank_ss_bus.cs" );

//Clear the vehicle vars
deleteVariables("modloaderLogger::Build*");