$mod::version = "1.00";
$mod::modName = "Turret Weapons";
$mod::description = "Turret weapons modified to be mountable on vehicles";
$mod::author = "PlagueDog";
//-------------------------------------------------------------------------								
// Cybrid Artillery Gun								
//	Id	Shape	Size	Sound_Tag	Damage	Tech Base	Description Tag	
newWeapon(	151 ,	"wc_artl.dts",	X,	IDSFX_NIKE,	1050 ,	C,	IDSPEC_WEA_BC	);

//	ShortName_tag	Longname_tag	SmallBmp	SmDisabledBmp	LargeBmp	LgDisabledBmp		
weaponInfo1(	IDWEA_CYART,	IDWDESC_CYART,	"HBC_S.bmp",	"HBC_SD.bmp",	"HBC_L.bmp",	"HBC_LD.bmp" 	);	
								
//	Techlevel	CombatValue	Mass					
weaponInfo2(	5 ,	1000 ,	10.00	);				
								
//	MuzzleShape	Trans MuzzleShape	FaceCamera	FlashColor (R)	(G)	(B)	FlashRange	
weaponMuzzle(	"mz_rail.dts",	"mz_railT.dts",	T,	255 ,	208 ,	62 ,	2.5	);
								
//	Reload/anim_time	Lock_time	Converge					
weaponGeneral(	4.0 ,	0.0 ,	T	);				
								
//	FireOffset (X)	(Y)	(Z)	FireTime				
weaponShot(	0.00 ,	15.65 ,	0.00		);			
								
//  	projectileId	maxAmmo	startAmmo	roundsPerVolley				
weaponAmmo(	60 ,	30 ,	30 ,	1	);			
								
//-------------------------------------------------------------------------								
// Rebel Artillery Gun								
//	Id	Shape	Size	Sound_Tag	Damage	Tech Base	Description Tag	
newWeapon(	152 ,	"wr_artl.dts",	X,	IDSFX_NIKE,	1050 ,	H,	IDSPEC_WEA_BC	);

//	ShortName_tag	Longname_tag	SmallBmp	SmDisabledBmp	LargeBmp	LgDisabledBmp		
weaponInfo1(	IDWEA_RBART,	IDWDESC_RBART,	"HBC_S.bmp",	"HBC_SD.bmp",	"HBC_L.bmp",	"HBC_LD.bmp" 	);	
								
//	Techlevel	CombatValue	Mass					
weaponInfo2(	5 ,	1000 ,	10.00	);				
								
//	MuzzleShape	Trans MuzzleShape	FaceCamera	FlashColor (R)	(G)	(B)	FlashRange	
weaponMuzzle(	"mz_rail.dts",	"mz_railT.dts",	T,	255 ,	208 ,	62 ,	2.5	);
								
//	Reload/anim_time	Lock_time	Converge					
weaponGeneral(	4.0 ,	0.0 ,	T	);				
								
//	FireOffset (X)	(Y)	(Z)	FireTime				
weaponShot(	0.00 ,	12.14 ,	0.00		);			
								
//  	projectileId	maxAmmo	startAmmo	roundsPerVolley				
weaponAmmo(	60 ,	30 ,	30 ,	1	);			
				
// Turret weapons								

//-------------------------------------------------------------------------								
// Turret Heavy Laser { common }								
//	Id	Shape	Size	Sound_Tag	Damage	Tech Base		
newWeapon(	153 ,	"wx_gun.dts",	X,	IDSFX_HLAS,	225,	A	);	

//	ShortName_tag	Longname_tag	SmallBmp	SmDisabledBmp	LargeBmp	LgDisabledBmp		
weaponInfo1(	IDWEA_TUR_HLAS,	IDWDESC_TUR_HLAS,	"HLAS_S.bmp",	"HLAS_SD.bmp",	"HLAS_L.bmp",	"HLAS_LD.bmp" 	);	

//	Techlevel	CombatValue	Mass					
weaponInfo2(	2 ,	150 ,	5.00	);				

//	MuzzleShape	Trans MuzzleShape	FaceCamera	FlashColor (R)	(G)	(B)	FlashRange	
weaponMuzzle(	"mz_las.dts",	"mz_lasT.dts",	T,	239 ,	90 ,	90 ,	2.5	);

//	Reload/anim_time	Lock_time	Converge					
weaponGeneral(	1.0 ,	0.0 ,	T	);				

//	FireOffset (X)	(Y)	(Z)	FireTime				
weaponShot(	0.00 ,	5.00 ,	0.52		);			
								
//	projectileId 	Charge_limit	Charge_rate					
weaponEnergy(	2 ,	30 ,	15.00	);				


//-------------------------------------------------------------------------								
// Turret Internal Pit Viper Single-Shot { common }								
//	Id	Shape	Size	Sound_Tag	Damage	Tech Base		
newWeapon(	154 ,	internal,	S,	IDSFX_VIP8,	20,	A	);	

//	ShortName_tag	Longname_tag	SmallBmp	SmDisabledBmp	LargeBmp	LgDisabledBmp		
weaponInfo1(	IDWEA_TUR_IVIP1,	IDWDESC_TUR_IVIP1,	"VIP_S.bmp",	"VIP_SD.bmp",	"VIP_L.bmp",	"VIP_LD.bmp" 	);	

//	Techlevel	CombatValue	Mass					
weaponInfo2(	0 ,	300 ,	3.00	);				

//	Reload/anim_time	Lock_time	Converge					
weaponGeneral(	1.5 ,	3.0 ,	F	);				

//	FireOffset (X)	(Y)	(Z)	FireTime				
weaponShot(	0.00 ,	0.00 ,	0.00		);			

//  	projectileId	maxAmmo	startAmmo	roundsPerVolley				
weaponAmmo(	24 ,	1 ,	1 ,	1	);			
								

//-------------------------------------------------------------------------								
// Turret Internal Heavy Laser { common }								
//	Id	Shape	Size	Sound_Tag	Damage	Tech Base		
newWeapon(	155,	"internal",	X,	IDSFX_HLAS,	225,	A	);	

//	ShortName_tag	Longname_tag	SmallBmp	SmDisabledBmp	LargeBmp	LgDisabledBmp		
weaponInfo1(	IDWEA_TUR_IHLAS,	IDWDESC_TUR_IHLAS,	"HLAS_S.bmp",	"HLAS_SD.bmp",	"HLAS_L.bmp",	"HLAS_LD.bmp" 	);	

//	Techlevel	CombatValue	Mass					
weaponInfo2(	2 ,	150 ,	5.00	);				

//	MuzzleShape	Trans MuzzleShape	FaceCamera	FlashColor (R)	(G)	(B)	FlashRange	
weaponMuzzle(	"mz_las.dts",	"mz_lasT.dts",	T,	239 ,	90 ,	90 ,	2.5	);

//	Reload/anim_time	Lock_time	Converge					
weaponGeneral(	1.0 ,	0.0 ,	T	);				

//	FireOffset (X)	(Y)	(Z)	FireTime				
weaponShot(	0.00 ,	5.00 ,	0.52		);			

//	projectileId 	Charge_limit	Charge_rate					
weaponEnergy(	2 ,	30 ,	15.00	);				
								

//-------------------------------------------------------------------------								
// Turret Cannon { common }								
//	Id	Shape	Size	Sound_Tag	Damage	Tech Base		
newWeapon(	156 ,	"wx_can.dts",	X,	IDSFX_BC,	1200,	A	);	

//	ShortName_tag	Longname_tag	SmallBmp	SmDisabledBmp	LargeBmp	LgDisabledBmp		
weaponInfo1(	IDWEA_TUR_CAN,	IDWDESC_TUR_CAN,	"BC_S.bmp",	"BC_SD.bmp",	"BC_L.bmp",	"BC_LD.bmp" 	);	

//	Techlevel	CombatValue	Mass					
weaponInfo2(	4 ,	920 ,	8.00	);				

//	MuzzleShape	Trans MuzzleShape	FaceCamera	FlashColor (R)	(G)	(B)	FlashRange	
weaponMuzzle(	"mz_rail.dts",	"mz_railT.dts",	T,	255 ,	208 ,	62 ,	2.5	);

//	Reload/anim_time	Lock_time	Converge					
weaponGeneral(	1.7 ,	0.0 ,	T	);				

//	FireOffset (X)	(Y)	(Z)	FireTime				
weaponShot(	0.00 ,	18.35 ,	0.57		);			

//  	projectileId	maxAmmo	startAmmo	roundsPerVolley				
weaponAmmo(	19 ,	25 ,	25 ,	1	);					

//-------------------------------------------------------------------------								
// Turret Internal Sentry {common}								
//	Id	Shape	Size	Sound_Tag	Damage	Tech Base		
newWeapon(	157 ,	"wc_pbw.dts",	I,	IDSFX_PBW,	1200,	A	);	

//	ShortName_tag	Longname_tag	SmallBmp	SmDisabledBmp	LargeBmp	LgDisabledBmp		
weaponInfo1(	IDWEA_TUR_ISENT,	IDWDESC_TUR_ISENT,	"PBW_S.bmp",	"PBW_SD.bmp",	"PBW_L.bmp",	"PBW_LD.bmp" 	);	

//	Techlevel	CombatValue	Mass					
weaponInfo2(	5 ,	900 ,	7.50	);				

//	MuzzleShape	Trans MuzzleShape	FaceCamera	FlashColor (R)	(G)	(B)	FlashRange	
weaponMuzzle(	"mz_pbw.dts",	"mz_pbwT.dts",	T,	72 ,	150 ,	255 ,	2.5	);

//	Reload/anim_time	Lock_time	Converge					
weaponGeneral(	1.8 ,	0.0 ,	T	);				

//	FireOffset (X)	(Y)	(Z)	FireTime				
weaponShot(	0.00 ,	0.00 ,	0.00		);			

//	projectileId 	Charge_limit	Charge_rate				
weaponEnergy(	9 ,	100 ,	50.00	);			


//-------------------------------------------------------------------------							
// Turret Heavy Autocannon { common }							
//	Id	Shape	Size	Sound_Tag	Damage	Tech Base	
newWeapon(	158 ,	"wx_gat.dts",	X,	IDSFX_HATC,	400,	A	);

//	ShortName_tag	Longname_tag	SmallBmp	SmDisabledBmp	LargeBmp	LgDisabledBmp	
weaponInfo1(	IDWEA_TUR_HATC,	IDWDESC_TUR_HATC,	"HATC_S.bmp",	"HATC_SD.bmp",	"HATC_L.bmp",	"HATC_LD.bmp" 	);

//	Techlevel	CombatValue	Mass				
weaponInfo2(	2 ,	150 ,	5.00	);			

//	MuzzleShape	Trans MuzzleShape	FaceCamera	FlashColor (R)	(G)	(B)	FlashRange
weaponMuzzle(	"mz_hatc.dts",	"mz_hatcT.dts",	F,	255 ,	208 ,	62 ,	2.5	);

//	Reload/anim_time	Lock_time	Converge					
weaponGeneral(	1.0 ,	0.0 ,	T	);				

//	FireOffset (X)	(Y)	(Z)	FireTime				
weaponShot(	0.00 ,	6.00 ,	1.50		);			

//  	projectileId	maxAmmo	startAmmo	roundsPerVolley				
weaponAmmo(	17 ,	4000 ,	4000 ,	21	);			


//-------------------------------------------------------------------------								
// MFAC Turret (Heavy Laser) { common }								
//	Id	Shape	Size	Sound_Tag	Damage	Tech Base		
newWeapon(	159 ,	"wx_TURM.dts",	X,	IDSFX_MFAC,	225,	H	);	

//	ShortName_tag	Longname_tag	SmallBmp	SmDisabledBmp	LargeBmp	LgDisabledBmp		
weaponInfo1(	IDWEA_TUR_MFAC,	IDWDESC_TUR_MFAC,	"HLAS_S.bmp",	"HLAS_SD.bmp",	"HLAS_L.bmp",	"HLAS_LD.bmp" 	);	

//	Techlevel	CombatValue	Mass					
weaponInfo2(	2 ,	150 ,	5.00	);				

//	MuzzleShape	Trans MuzzleShape	FaceCamera	FlashColor (R)	(G)	(B)	FlashRange	
weaponMuzzle(	"mz_mfac.dts",	"mz_mfacT.dts",	F,	72 ,	150 ,	255 ,	2.5	);

//	Reload/anim_time	Lock_time	Converge					
weaponGeneral(	2.8 ,	0.0 ,	T	);				

//	FireOffset (X)	(Y)	(Z)	FireTime				
weaponShot(	0.00 ,	5.00 ,	0.52		);			

//	projectileId 	Charge_limit	Charge_rate				
weaponEnergy(	13 ,	155 ,	50.00	);		