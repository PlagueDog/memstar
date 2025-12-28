   function ME::Move( %speed, %rot, %turbo )
   {
      if( %speed == "" ) 
         %speed = 100.0;
      if( %rot == "" )
         %rot = 1.5;
      if( %turbo == "" )
         %turbo = 5.0;

     $ME::CameraMoveSpeed   = %speed;
     $ME::CameraRotateSpeed = %rot;
	 $ME::CameraTurboSpeed  = %turbo;
   }

   newActionMap( "editCamera.sae" );

	#------- Keyboard Controls
   bindAction( keyboard, make,  a, TO, IDACTION_MOVE_X, -1.0 );
   bindAction( keyboard, break, a, TO, IDACTION_MOVE_X,  0.0 );
   bindAction( keyboard, make,  d, TO, IDACTION_MOVE_X,  1.0 );
   bindAction( keyboard, break, d, TO, IDACTION_MOVE_X,  0.0 );
   bindAction( keyboard, make,  s, TO, IDACTION_MOVE_Y, -1.0 );
   bindAction( keyboard, break, s, TO, IDACTION_MOVE_Y,  0.0 );
   bindAction( keyboard, make,  w, TO, IDACTION_MOVE_Y,  1.0 );
   bindAction( keyboard, break, w, TO, IDACTION_MOVE_Y,  0.0 );
   bindAction( keyboard, make,  e, TO, IDACTION_MOVE_Z,  1.0 );
   bindAction( keyboard, break, e, TO, IDACTION_MOVE_Z,  0.0 );
   bindAction( keyboard, make,  c, TO, IDACTION_MOVE_Z, -1.0 );
   bindAction( keyboard, break, c, TO, IDACTION_MOVE_Z,  0.0 );
   
   //
   bindAction(mouse, xaxis, TO, IDACTION_YAW, scale, 0.002, flip);
   bindAction(mouse, yaxis, TO, IDACTION_PITCH, scale, 0.002, flip);

   // movement binds
   bindCommand( keyboard, make, 0, TO, "ME::Move(1024);");
   for( %i = 1; %i < 10; %i++ )
   {
		bindCommand( keyboard, make, %i, TO, "ME::Move(" @ ( 1 << %i ) @ ");" );
   }
  $alt=1;