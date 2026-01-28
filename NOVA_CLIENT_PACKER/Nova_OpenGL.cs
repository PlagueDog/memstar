//No need to load the rest of this file if the client is a headless server
if ($cargv1 != "-s")
{
$GuiEdit::GridSnapX = 1;
$GuiEdit::GridSnapY = 1;
// $winconsoleenabled=1;
$SimGui::firstPreRender = true;
$pref::preLoad = true;
$pref::OpenGL::NoAddFade = true; //Fix z-buffering artifacts on overlapped projectiles
forceframerender(true);
$GUI::noPalTrans = true; //Disable GUI transition flicker
$canvas::Xaxis = 5;
$canvas::Yaxis = 30;

bind( keyboard0, make, control, shift, "r", TO, "if($pref::GWC::SIM_FS_MODE == 'Upscaled' && !$zzPendingReload && $Opengl::Active){Nova::ResetOpenGLCanvas();guipushdialog(simcanvas,'UpscalerProfileConfig.dlg');$zzInUpscaleConfig=true;schedule(\"UPCTicker();\", 0.025);}");

String::Explode(Nova::getDesktopResolution(),x,Nova::SystemResolution);
$Nova::SystemResolution[width] = $Nova::SystemResolution[0];
$Nova::SystemResolution[height] = $Nova::SystemResolution[1];
$Nova::SystemResolution = Nova::getDesktopResolution();

if(!isFile("mods/ScriptGL/RenderContext"))
{
	schedule("$pref::shellScriptGL = false;",1);
}
if(strlen($pref::Gpu::Name))
{
	$Nova::OpenGLDevice = $pref::Gpu::Name;
}

function Nova::scaleGui()
{
	OpenGL::scaleGUI(Nova::calcScale()+0.001);
	OpenGL::shiftGUI(Nova::calcOffset());
}

// function Nova::calcOffset()
// {
	// %aspectRatio = getWindowSize(width)/getWindowSize(height);
	// if(%aspectRatio == 1.33333)
	// {
		// return -1;
	// }
	// %offset = -(1.333334) * %aspectRatio;
	// return %offset;
// }

function Nova::calcScale()
{
	// %scale = getWindowSize(height)/240;
	
	%scale = $pref::GWC::SIM_FS_HEIGHT/240;
	%aspectRatio = $pref::GWC::SIM_FS_WIDTH / $pref::GWC::SIM_FS_HEIGHT;
	
	if(%aspectRatio == 1.25)
	{
		%scale/=1.066;
	}
	return %scale;
}

function Nova::calcOffset()
{
	if(strlen($Opengl::UpscaleProfile[$pref::GWC::SIM_FS_WIDTH, $pref::GWC::SIM_FS_HEIGHT]) && !$zzPendingReload && $Opengl::Active)
	{
		schedule("eval($Opengl::UpscaleProfile[$pref::GWC::SIM_FS_WIDTH, $pref::GWC::SIM_FS_HEIGHT]);",0);
		return -1;
	}
	%aspectRatio = $pref::GWC::SIM_FS_WIDTH / $pref::GWC::SIM_FS_HEIGHT;
	%guiAspectRatio = 640/480;
	%scaledWidth = (640/$pref::GWC::SIM_FS_WIDTH)*2;
	%centeredOffsetX = -(%aspectRatio-%guiAspectRatio)/2;
	if(%aspectRatio == 2.12121)
	{
		%offset = 0.23107;
	}
	else if(%aspectRatio == 2.22222)
	{
		%offset = 0.1306;
	}
	else if(%aspectRatio == 2.33333)
	{
		%offset = 0.072499;
	}
	else if(%aspectRatio == 2.34146)
	{
		// %offset = 0.246;
		%offset = 0.064692;
	}
	else if(%aspectRatio == 2.37037)
	{
		%offset = 0.044;
	}
	else if(%aspectRatio == 2.38888)
	{
		%offset = 0.0323;
	}
	else if(%aspectRatio == 1.25)
	{
		return -1;
	}
	else if(%aspectRatio <= 1.77777 && %aspectRatio >= 1.77)
	{
		%offset = 0.5278;
	}
	else if(%aspectRatio == 1.77864)
	{
		%offset = 0.5278;
	}
	else if(%aspectRatio <= 1.33333 && %aspectRatio >= 1.3)
	{
		return -1;
	}
	else if(%aspectRatio == 1.59999)
	{
		//%offset = 0.61029;
		//if($pref::GWC::SIM_FS_WIDTH == 1280)
		//{
			%offset = 0.69918;
		//}
	}
	else if(%aspectRatio == 1.5625)
	{
		%offset = 0.73602;
	}
	else if(%aspectRatio == 1.5)
	{
		%offset = 0.81;
	}
	else
	{
		eval($Opengl::UpscaleProfile[$pref::GWC::SIM_FS_WIDTH, $pref::GWC::SIM_FS_HEIGHT]);
		return -1;
	}
	return %centeredOffsetX-%offset;
}

if(!strlen($Opengl::UpscaleProfile[640,480]))   {$Opengl::UpscaleProfile[640,480]   = "$Opengl::scaler::RenderScale=2;       OpenGL::ScaleGUI('2');       $Opengl::scaler::Rendershift=-1;      Opengl::shiftGUI('-1');";}
if(!strlen($Opengl::UpscaleProfile[720,400]))   {$Opengl::UpscaleProfile[720,400]   = "$Opengl::scaler::RenderScale=1.81;    OpenGL::ScaleGUI('1.81');    $Opengl::scaler::Rendershift=-0.8;    Opengl::shiftGUI('-0.8');";}
if(!strlen($Opengl::UpscaleProfile[720,480]))   {$Opengl::UpscaleProfile[720,480]   = "$Opengl::scaler::RenderScale=2;       OpenGL::ScaleGUI('2');       $Opengl::scaler::Rendershift=-0.89;   Opengl::shiftGUI('-0.89');";}
if(!strlen($Opengl::UpscaleProfile[720,576]))   {$Opengl::UpscaleProfile[720,576]   = "$Opengl::scaler::RenderScale=2.252;   OpenGL::ScaleGUI('2.252');   $Opengl::scaler::Rendershift=-1;      Opengl::shiftGUI('-1');";}
if(!strlen($Opengl::UpscaleProfile[800,600]))   {$Opengl::UpscaleProfile[800,600]   = "$Opengl::scaler::RenderScale=2.498;   OpenGL::ScaleGUI('2.498');   $Opengl::scaler::Rendershift=-1;      Opengl::shiftGUI('-1');";}
if(!strlen($Opengl::UpscaleProfile[832,624]))   {$Opengl::UpscaleProfile[832,624]   = "$Opengl::scaler::RenderScale=2.61046; OpenGL::ScaleGUI('2.61046'); $Opengl::scaler::Rendershift=-1;    	Opengl::shiftGUI('-1');";}
if(!strlen($Opengl::UpscaleProfile[1024,768]))  {$Opengl::UpscaleProfile[1024,768]  = "$Opengl::scaler::RenderScale=3.2;     OpenGL::ScaleGUI('3.2');     $Opengl::scaler::Rendershift=-1;    	Opengl::shiftGUI('-1');";}
if(!strlen($Opengl::UpscaleProfile[1152,864]))  {$Opengl::UpscaleProfile[1152,864]  = "$Opengl::scaler::RenderScale=3.6;     OpenGL::ScaleGUI('3.6');     $Opengl::scaler::Rendershift=-1;    	Opengl::shiftGUI('-1');";}
if(!strlen($Opengl::UpscaleProfile[1152,870]))  {$Opengl::UpscaleProfile[1152,870]  = "$Opengl::scaler::RenderScale=3.625;   OpenGL::ScaleGUI('3.625');   $Opengl::scaler::Rendershift=-1;      Opengl::shiftGUI('-1');";}
if(!strlen($Opengl::UpscaleProfile[1152,872]))  {$Opengl::UpscaleProfile[1152,872]  = "$Opengl::scaler::RenderScale=3.6;     OpenGL::ScaleGUI('3.6');     $Opengl::scaler::Rendershift=-1;    	Opengl::shiftGUI('-1');";}
if(!strlen($Opengl::UpscaleProfile[1280,720]))  {$Opengl::UpscaleProfile[1280,720]  = "$Opengl::scaler::RenderScale=3.019;   OpenGL::ScaleGUI('3.019');   $Opengl::scaler::Rendershift=-0.755;	Opengl::shiftGUI('-0.755');";}
if(!strlen($Opengl::UpscaleProfile[1280,800]))  {$Opengl::UpscaleProfile[1280,800]  = "$Opengl::scaler::RenderScale=3.32792; OpenGL::ScaleGUI('3.32792'); $Opengl::scaler::Rendershift=-0.8325; Opengl::shiftGUI('-0.8325');";}
if(!strlen($Opengl::UpscaleProfile[1280,960]))  {$Opengl::UpscaleProfile[1280,960]  = "$Opengl::scaler::RenderScale=4;       OpenGL::ScaleGUI('4');       $Opengl::scaler::Rendershift=-1;    	Opengl::shiftGUI('-1');";}
if(!strlen($Opengl::UpscaleProfile[1280,1024])) {$Opengl::UpscaleProfile[1280,1024] = "$Opengl::scaler::RenderScale=4;       OpenGL::ScaleGUI('4');       $Opengl::scaler::Rendershift=-1;    	Opengl::shiftGUI('-1');";}
if(!strlen($Opengl::UpscaleProfile[1366,768]))  {$Opengl::UpscaleProfile[1366,768]  = "$Opengl::scaler::RenderScale=3.2;     OpenGL::ScaleGUI('3.2');     $Opengl::scaler::Rendershift=-0.75; 	Opengl::shiftGUI('-0.75');";}
if(!strlen($Opengl::UpscaleProfile[1440,900]))  {$Opengl::UpscaleProfile[1440,900]  = "$Opengl::scaler::RenderScale=3.749;   OpenGL::ScaleGUI('3.749');   $Opengl::scaler::Rendershift=-0.8325; Opengl::shiftGUI('-0.8325');";}
if(!strlen($Opengl::UpscaleProfile[1400,1050])) {$Opengl::UpscaleProfile[1400,1050] = "$Opengl::scaler::RenderScale=4.3749;  OpenGL::ScaleGUI('4.3749');  $Opengl::scaler::Rendershift=-1; 	    Opengl::shiftGUI('-1');";}
if(!strlen($Opengl::UpscaleProfile[1440,1080])) {$Opengl::UpscaleProfile[1440,1080] = "$Opengl::scaler::RenderScale=4.4949;  OpenGL::ScaleGUI('4.4949');  $Opengl::scaler::Rendershift=-1; 	    Opengl::shiftGUI('-1');";}
if(!strlen($Opengl::UpscaleProfile[1600,900]))  {$Opengl::UpscaleProfile[1600,900]  = "$Opengl::scaler::RenderScale=3.73713; OpenGL::ScaleGUI('3.73713'); $Opengl::scaler::Rendershift=-0.7475; Opengl::shiftGUI('-0.7475');";}
if(!strlen($Opengl::UpscaleProfile[1600,1024])) {$Opengl::UpscaleProfile[1600,1024] = "$Opengl::scaler::RenderScale=4.259;   OpenGL::ScaleGUI('4.259');   $Opengl::scaler::Rendershift=-0.85; 	Opengl::shiftGUI('-0.85');";}
if(!strlen($Opengl::UpscaleProfile[1600,1200])) {$Opengl::UpscaleProfile[1600,1200] = "$Opengl::scaler::RenderScale=5.019;   OpenGL::ScaleGUI('5.019');   $Opengl::scaler::Rendershift=-1;    	Opengl::shiftGUI('-1');";}
if(!strlen($Opengl::UpscaleProfile[1680,720]))  {$Opengl::UpscaleProfile[1680,720]  = "$Opengl::scaler::RenderScale=3.019;   OpenGL::ScaleGUI('3.019');   $Opengl::scaler::Rendershift=-0.575;	Opengl::shiftGUI('-0.575');";}
if(!strlen($Opengl::UpscaleProfile[1680,1050])) {$Opengl::UpscaleProfile[1680,1050] = "$Opengl::scaler::RenderScale=4.3759;  OpenGL::ScaleGUI('4.3759');  $Opengl::scaler::Rendershift=-0.83; 	Opengl::shiftGUI('-0.83');";}
if(!strlen($Opengl::UpscaleProfile[1920,820]))  {$Opengl::UpscaleProfile[1920,820]  = "$Opengl::scaler::RenderScale=3.415;   OpenGL::ScaleGUI('3.415');   $Opengl::scaler::Rendershift=-0.56875;Opengl::shiftGUI('-0.56875');";}
if(!strlen($Opengl::UpscaleProfile[1920,1080])) {$Opengl::UpscaleProfile[1920,1080] = "$Opengl::scaler::RenderScale=4.519;   OpenGL::ScaleGUI('4.519');   $Opengl::scaler::Rendershift=-0.75; 	Opengl::shiftGUI('-0.75');";}
if(!strlen($Opengl::UpscaleProfile[1920,1440])) {$Opengl::UpscaleProfile[1920,1440] = "$Opengl::scaler::RenderScale=6;       OpenGL::ScaleGUI('6');       $Opengl::scaler::Rendershift=-1; 	    Opengl::shiftGUI('-1');";}
if(!strlen($Opengl::UpscaleProfile[1920,1200])) {$Opengl::UpscaleProfile[1920,1200] = "$Opengl::scaler::RenderScale=5.0009;  OpenGL::ScaleGUI('5.0009');  $Opengl::scaler::Rendershift=-0.8325; Opengl::shiftGUI('-0.8325');";}
if(!strlen($Opengl::UpscaleProfile[2100,990]))  {$Opengl::UpscaleProfile[2100,990]  = "$Opengl::scaler::RenderScale=4.119;   OpenGL::ScaleGUI('4.119');   $Opengl::scaler::Rendershift=-0.6275; Opengl::shiftGUI('-0.6275');";}
if(!strlen($Opengl::UpscaleProfile[2310,990]))  {$Opengl::UpscaleProfile[2310,990]  = "$Opengl::scaler::RenderScale=4.119;   OpenGL::ScaleGUI('4.119');   $Opengl::scaler::Rendershift=-0.57;   Opengl::shiftGUI('-0.57');";}
if(!strlen($Opengl::UpscaleProfile[2560,1080])) {$Opengl::UpscaleProfile[2560,1080] = "$Opengl::scaler::RenderScale=4.519;   OpenGL::ScaleGUI('4.519');   $Opengl::scaler::Rendershift=-0.565;  Opengl::shiftGUI('-0.565');";}
if(!strlen($Opengl::UpscaleProfile[2560,1440])) {$Opengl::UpscaleProfile[2560,1440] = "$Opengl::scaler::RenderScale=6;       OpenGL::ScaleGUI('6');       $Opengl::scaler::Rendershift=-0.75;   Opengl::shiftGUI('-0.75');";}
if(!strlen($Opengl::UpscaleProfile[3024,1296])) {$Opengl::UpscaleProfile[3024,1296] = "$Opengl::scaler::RenderScale=5.4148;  OpenGL::ScaleGUI('5.4148');  $Opengl::scaler::Rendershift=-0.5725; Opengl::shiftGUI('-0.5725');";}
if(!strlen($Opengl::UpscaleProfile[3200,1800])) {$Opengl::UpscaleProfile[3200,1800] = "$Opengl::scaler::RenderScale=7.49;    OpenGL::ScaleGUI('7.49');    $Opengl::scaler::Rendershift=-0.75;   Opengl::shiftGUI('-0.75');";}
if(!strlen($Opengl::UpscaleProfile[3440,1440])) {$Opengl::UpscaleProfile[3440,1440] = "$Opengl::scaler::RenderScale=6;       OpenGL::ScaleGUI('6');       $Opengl::scaler::Rendershift=-0.56;   Opengl::shiftGUI('-0.56');";}
if(!strlen($Opengl::UpscaleProfile[3840,1620])) {$Opengl::UpscaleProfile[3840,1620] = "$Opengl::scaler::RenderScale=6.75;    OpenGL::ScaleGUI('6.75');    $Opengl::scaler::Rendershift=-0.5625; Opengl::shiftGUI('-0.5625');";}
if(!strlen($Opengl::UpscaleProfile[3840,2160])) {$Opengl::UpscaleProfile[3840,2160] = "$Opengl::scaler::RenderScale=9.027;   OpenGL::ScaleGUI('9.027');   $Opengl::scaler::Rendershift=-0.75;   Opengl::shiftGUI('-0.75');";}
if(!strlen($Opengl::UpscaleProfile[5160,2160])) {$Opengl::UpscaleProfile[5160,2160] = "$Opengl::scaler::RenderScale=8.9999;  OpenGL::ScaleGUI('8.9999');  $Opengl::scaler::Rendershift=-0.5575; Opengl::shiftGUI('-0.5575');";}
if(!strlen($Opengl::UpscaleProfile[6880,2880])) {$Opengl::UpscaleProfile[6880,2880] = "$Opengl::scaler::RenderScale=12;      OpenGL::ScaleGUI('12');      $Opengl::scaler::Rendershift=-0.5575; Opengl::shiftGUI('-0.5575');";}

if(isFile("OpenglUpscalerProfiles.cs"))
{
    exec("OpenglUpscalerProfiles.cs");
}

//Keep track of the game cursor
function Nova::containCursor()
{
    if($pref::GWC::SIM_FS_MODE == "Upscaled" && !isObject(playgui) && !isObject(mapviewgui) && !isObject(editorgui) && !isObject(splashgui) && !isObject(modloaderInitERRgui))
    {
        %X = $Nova::cursorLocX;
        %Y = $Nova::cursorLocY;
        %max_X = 640;
        if(%X > %max_X)
        {
			Nova::setCursorLoc(%max_X-1, %Y);
        }
        else if(%Y > 480)
        {
            Nova::setCursorLoc(%X, 479);
        }
    }
	
    //if($pref::GWC::SIM_FS_MODE == "Upscaled")
    //{
    //    if($pref::GWC::SIM_FS_HEIGHT < $Nova::SystemResolution[height])
    //    {
    //        if(getWindowPosition(x) > 0 && getWindowPosition(y) > 0)
    //        {
    //            $canvas::Xaxis = getWindowPosition(x);
    //            $canvas::Yaxis = getWindowPosition(y);
    //        }
    //    }
    //}
}
    
function Nova::isAMD()
{
    if(String::findSubStr($Nova::OpenGLDevice, "AMD ")!=-1){return true;}
    if(String::findSubStr($Nova::OpenGLDevice, " AMD ")!=-1){return true;}
    if(String::findSubStr($Nova::OpenGLDevice, " AMD")!=-1){return true;}
    if(String::findSubStr($Nova::OpenGLDevice, "Radeon")!=-1){return true;}
    if(String::findSubStr($Nova::OpenGLDevice, "AMD Radeon")!=-1){return true;}
    return false;
}

function toggleBorder()
{
	if($pref::GWC::SIM_FS_MODE != "Upscaled")
	{
		return;
	}
    if(Nova::isAMD())
    {
        return;
    }
    $pref::windowBorder^=1;
    if(!$pref::windowBorder)
    {
        disableWindowBorder(); 
    }
    else
    {
        enableWindowBorder();
    }
}

function Nova::desktopResolution(%plane)
{
    if(%plane == width)
    {
        String::Explode(Nova::getDesktopResolution(),x,StringExplode);
        return $String::Explode[0];
    }
    if(%plane == height)
    {
        String::Explode(Nova::getDesktopResolution(),x,StringExplode);
        return $String::Explode[1];
    }
}

//Alias of Nova::OpenGLMode(); for use in consoleCMD fields of Simgui Objects
function mlogl(%mode)
{
    if(isObject(playgui) || isObject(editorgui) || isObject(mapviewgui))
    {
        localMessageBox(*IDSTR_NOVA_MODE_INGAME_ERR);
        return;
    }
        //Nova::OpenGLMode(%mode);
}

function Nova::shellMode(%mode)
{
    if(%mode == $pref::GWC::SIM_FS_MODE)
    {
        return;
    }
    if(isObject(playgui) || isObject(editorgui) || isObject(mapviewgui))
    {
        localMessageBox(*IDSTR_NOVA_MODE_INGAME_ERR);
        return;
    }
    if(%mode == "Default")
    {
        $pref::GWC::SIM_FS_MODE="Default";
        $zzPendingReload=1;
        localMessageBox(*IDSTR_NOVA_UPSCALED_OPENGL_RELOAD_NOTIFY);
    }
    else if(%mode == "Upscaled")
    {
        $pref::GWC::SIM_FS_MODE="Upscaled";
        $zzPendingReload=1;
        localMessageBox(*IDSTR_NOVA_UPSCALED_OPENGL_RELOAD_NOTIFY);
    }
}

function Nova::handleCanvasAdjustments()
{
    if($pref::GWC::SIM_FS_MODE == "Upscaled" && !$zzPendingReload && $Opengl::Active)
    {
        if($pref::GWC::SIM_FS_HEIGHT < $Nova::SystemResolution[height])
        {
			enableWindowBorder();
            if($canvas::Yaxis < 0)
            {
                schedule("setWindowPos($canvas::Xaxis-0,0, $pref::GWC::SIM_FS_WIDTH+6, $pref::GWC::SIM_FS_HEIGHT+29);",0.1);
            }
            else
            {
                schedule("setWindowPos($canvas::Xaxis-0,$canvas::Yaxis-0, $pref::GWC::SIM_FS_WIDTH+6, $pref::GWC::SIM_FS_HEIGHT+29);",0.1);
            }
        }
        else
        {
            //schedule("setWindowPos(-5,-27, $pref::GWC::SIM_FS_WIDTH+6, $pref::GWC::SIM_FS_HEIGHT+32);",0.1);
            schedule("setWindowPos(0,0, $pref::GWC::SIM_FS_WIDTH, $pref::GWC::SIM_FS_HEIGHT);",0);
        }
    }
}

function Nova::updateWindowScale()
{
	if(getWindowSize(width) < $Nova::SystemResolution[width] && getWindowSize(height) < $Nova::SystemResolution[height])
	{
		setWindowPos($canvas::Xaxis-0,$canvas::Yaxis-0, $pref::GWC::SIM_FS_WIDTH+6, $pref::GWC::SIM_FS_HEIGHT+29);
	}
}

function splashGUI::onOpen::handleOpenGL()
{
	//if(!$pref::shellScriptGL)
	//{
	//	schedule("Nova::FlushCanvas();",0.1);
	//}	
    if(getWindowSize(height) >= $Nova::SystemResolution[height])
    {
        if($pref::GWC::SIM_FS_MODE == "Upscaled" && !$zzPendingReload && $Opengl::Active)
        {
            schedule("Nova::handleCanvasAdjustments();",0.1);
            schedule("Nova::UpscaledGUI();", 0.1);
            schedule("Nova::FlushCanvas();", 0.2);
        }
    }
}

function introGUI::onOpen::handleOpenGL()
{
	if($hashSmacker)
	{
		Nova::batchGenerateTextureHasher();
	}
    if($pref::GWC::SIM_FS_MODE == "Upscaled" && !$zzPendingReload && $Opengl::Active)
    {
		splashGUI::onOpen::handleOpenGL();
		if($pref::shellScriptGL && isFile("mods/ScriptGL/RenderContext") && !$_ScriptGLInitialized && $pref::shellScriptGL)
		{
			function introGUI::onOpen::handleOpenGL(){}
			Nova::initScriptGLContext();
		}
	}
}

function omniWebGUI::onOpen::NovaFunction()
{
    if($pref::GWC::SIM_FS_MODE == "Upscaled" && !$zzPendingReload && $Opengl::Active)
    {
		Nova::purgeControlAndAttach();
	}
}

function inputConfigGUI::onOpen::NovaFunction()
{
	//$Gui::InputConfigFromSim = true;
	if(isCampaign())
	{
		if($pref::GWC::SIM_FS_MODE == "Upscaled" && !$zzPendingReload && $Opengl::Active)
		{
			Nova::scaleGui();
			newobject(CANVAS_CULL_RIGHT, Simgui::TScontrol,640,0,8000,480);addtoset($CurrentGUI, CANVAS_CULL_RIGHT);
			newobject(CANVAS_CULL_BOTTOM, Simgui::TScontrol,0,480,8000,8000);addtoset($CurrentGUI, CANVAS_CULL_BOTTOM);
		}
	}
}

function mainmenuGUI::onOpen::handleOpenGL()
{
	$zzIsCampaign = false;
	if(!$pref::shellScriptGL)
	{
		//if($previousGUI == playGUI || $previousGUI == introGUI)
		//{
			schedule("Nova::FlushCanvas();",0.15);
		//}
	}	
}

function mainmenuGUI::onClose::handleOpenGL()
{
	if(!$pref::shellScriptGL)
	{
		//if($previousGUI == playGUI || $previousGUI == introGUI)
		//{
			schedule("Nova::FlushCanvas();",0.5);
		//}
	}	
}

function recPlayerGUI::onOpen::handleOpenGL()
{
	if(!$pref::shellScriptGL && $previousGUI == playGUI)
	{
		schedule("Nova::FlushCanvas();",0.15);
	}	
}

//This is executed when the game canvas loses focus
function Nova::handleLoseFocus()
{
    Nova::handleCanvasAdjustments();
}

function optionsGUI::onOpen::handleOpenGL()
{
    if($pref::GWC::SIM_FS_MODE == "Upscaled" && !$zzPendingReload && $Opengl::Active)
    {
		//control::setVisible(IDSTR_WINDOWED_MODE,0);
		control::setActive(IDSTR_WINDOWED_MODE,0);
		renameObject(Nova::findGuiTagControl(OptionsGUI, IDSTR_FULLSCREEN_MODE), OptionsGUI_fullscreenModeBox);
		schedule('Control::setText(OptionsGUI_fullscreenModeBox, "OpenGL: " @ $pref::Gpu::Name);',0);
	}
}

function optionsGUI::onClose::handleOpenGL()
{
    if($pref::GWC::SIM_FS_MODE == "Upscaled" && !$zzPendingReload && $Opengl::Active)
    {
       schedule("setWindowSize(simcanvas, _($pref::GWC::SIM_FS_WIDTH,640), _($pref::GWC::SIM_FS_HEIGHT,480));",0); 
       schedule("OpenGL::flipScaler();",1.25);
       // schedule("eval($Opengl::UpscaleProfile[$pref::GWC::SIM_FS_WIDTH, $pref::GWC::SIM_FS_HEIGHT]);",1.1);
       schedule("Nova::scaleGUI();",1.1);
       Nova::createShellCullers();
       Nova::handleCanvasAdjustments();
    }
}


function Nova::parseOpenGLModes()
{
    %trimPosition = String::findSubStr($Nova::opengl::info, "(FS)");
    %list = String::Right($Nova::OpenGL::Info, strlen($Nova::OpenGL::Info)-(%trimPosition+5));
    $Nova::Opengl::ResolutionList = String::Replace(%list, "\x20", "");
    String::Explode($Nova::OpenGL::ResolutionList, ",", "Nova::OpenGL::Resolution");
}
Nova::parseOpenGLModes();

function forceToShellRes()
{
    goToShellRes();
}

function gotoshellres()
{
    if($pref::GWC::SIM_FS_DEVICE == OpenGL)
    {
        if($pref::GWC::SIM_FS_MODE != "Upscaled")
        {
            setWindowSize(simcanvas, 640, 480);
        }
        else
        {
            if(strlen($Opengl::UpscaleProfile[$pref::GWC::SIM_FS_WIDTH, $pref::GWC::SIM_FS_HEIGHT]))
            {
                OpenGL::flipScaler(); //Make the scaling expand from top left to bottom right
                // eval($Opengl::UpscaleProfile[$pref::GWC::SIM_FS_WIDTH, $pref::GWC::SIM_FS_HEIGHT]);
                Nova::scaleGUI();
				if($currentGUI == waitroomGUI)
				{
					Nova::createShellCullers();
				}
                Nova::handleCanvasAdjustments();
            }
        }
    }
    else
    {
		OpenGL::unflipScaler();
		OpenGL::restoreBitmapCtrl();
        setWindowSize(simcanvas, 640, 480);
    }
}

function Nova::createShellCullers()
{
    while(isObject("NamedGuiSet/CANVAS_CULL_RIGHT")){deleteObject("NamedGuiSet/CANVAS_CULL_RIGHT");}
    while(isObject("NamedGuiSet/CANVAS_CULL_BOTTOM")){deleteObject("NamedGuiSet/CANVAS_CULL_BOTTOM");}
    
	
    if($currentGUI == NovaUIgui && $currentGUI != waitroomGUI)
    {
        return;
    }
    
	if($currentGUI == playGUI || $currentGUI == editorGUI || $currentGUI == mapviewGUI)
	{
		return;
	}
	
	if(!$pref::shellScriptGL)
	{
		schedule("newobject(CANVAS_CULL_RIGHT, Simgui::TScontrol,640,0,8000,480);addtoset($CurrentGUI, CANVAS_CULL_RIGHT);",0);
	}
    schedule("newobject(CANVAS_CULL_BOTTOM, Simgui::TScontrol,0,480,8000,8000);addtoset($CurrentGUI, CANVAS_CULL_BOTTOM);",0);
}

// function gotosimres()
function Nova::gotosimres()
{
	if(isFunction("client::onGoToSimRes"))
	{
		client::onGoToSimRes();
	}
    // unlockWindowSize(simcanvas);
    if($Opengl::Active)
    {
        OpenGL::terrainFix(true);
        if($pref::GWC::SIM_FS_MODE == "Default")
        {
            // setWindowSize(simcanvas, _($pref::GWC::SIM_FS_WIDTH,640), _($pref::GWC::SIM_FS_HEIGHT,480));
            Nova::sendWindowToFront();
        }
        if(isFullscreenMode(simcanvas) && $pref::GWC::SIM_FS_MODE != "Upscaled" && !$zzPendingReload && $Opengl::Active)
        {
            Nova::handleCanvasAdjustments();
            lockWindowSize();
        }
    }
    else
    {
        OpenGL::terrainFix(false);
        // setWindowSize(simcanvas, _($pref::GWC::SIM_FS_WIDTH,640), _($pref::GWC::SIM_FS_HEIGHT,480));
    }
}
    
function Nova::FlushCanvas()
{
	if($pref::GWC::SIM_FS_MODE != "Upscaled")
	{
		return;
	}
	
    if($pref::ShellScriptGL)
    {
        return;
    }
    //Is this a 4:3 resolution? If so, no need to flush the canvas
    if(($pref::GWC::SIM_FS_WIDTH/$pref::GWC::SIM_FS_HEIGHT) >= 1.3 && ($pref::GWC::SIM_FS_WIDTH/$pref::GWC::SIM_FS_HEIGHT) <= 1.34)
    {
        return;
    }
    
    if(!Nova::isAMD())
    {
        disableWindowBorder();
        schedule("enableWindowBorder();",0.025);
		if(getWindowSize(width) >= $Nova::SystemResolution[width] && getWindowSize(height) >= $Nova::SystemResolution[height])
		{
			schedule("disableWindowBorder();",0.045);
		}
    }
    else
    {
		clientCursorOff();
        newObject(CANVAS_CULL, Simgui::TScontrol, 0, 0, 8000, 8000);
        addToSet($CurrentGUI, CANVAS_CULL);
        opengl::ShiftGUI(-1);
        if(!$zzInUpscaleConfig)
        {
            // schedule("eval($Opengl::UpscaleProfile[$pref::GWC::SIM_FS_WIDTH, $pref::GWC::SIM_FS_HEIGHT]);",0.075);
            schedule("Nova::scaleGUI();",0.075);
        }
        else
        {
            opengl::ShiftGUI($OpenGL::scaler::RenderShift);
        }
        schedule("deleteObject($CurrentGUI @ '\\CANVAS_CULL');flushTextureCache(simcanvas);",0.15);
		schedule("clientCursorOn();",0.16);
        
    }
}


function Nova::UpscaledGUI()
{
    if(($CurrentGUI == playgui || $CurrentGUI == editorgui || $CurrentGUI == mapviewgui) || $pref::GWC::SIM_FS_MODE != "Upscaled")
    {
        OpenGL::unflipScaler();
        OpenGL::scaleGUI(2);
        OpenGL::shiftGUI(-1);
    }
    
    //If we have an upscale profile...
    else if(strlen($Opengl::UpscaleProfile[$pref::GWC::SIM_FS_WIDTH, $pref::GWC::SIM_FS_HEIGHT]) && !$zzPendingReload && $Opengl::Active)
    {
        OpenGL::flipScaler();
        // eval($Opengl::UpscaleProfile[$pref::GWC::SIM_FS_WIDTH, $pref::GWC::SIM_FS_HEIGHT]);
        Nova::scaleGUI();
		if($currentGUI == waitroomGUI)
		{
			Nova::createShellCullers();
		}
    }
	
	else if(!$zzPendingReload && $Opengl::Active)
	{
        OpenGL::flipScaler();
        Nova::scaleGUI();
		if($currentGUI == waitroomGUI)
		{
			Nova::createShellCullers();
		}
	}
    
    //If we do not have an upscale profile use the default Opengl setup
    else if(!strlen($Opengl::UpscaleProfile[$pref::GWC::SIM_FS_WIDTH, $pref::GWC::SIM_FS_HEIGHT]) && !$zzPendingReload && $Opengl::Active)
    {
        $Opengl::scaler::RenderScale=2;
        $Opengl::scaler::Rendershift=-1;
        OpenGL::flipScaler();
        OpenGL::scaleGUI(2);//Default internal constant is 2.0
        OpenGL::shiftGUI(-1);//Default internal constant is -1.0
        Nova::createShellCullers();
        if(!$zzUpscaleProfileErr)
        {
            $zzUpscaleProfileErr=1;
            schedule("guipopdialog(simcanvas,0);localMessageBox(*IDSTR_NOVA_UPSCALER_NO_CONFIG_1 @ $pref::GWC::SIM_FS_WIDTH @ \"x\" @ $pref::GWC::SIM_FS_HEIGHT @ \".\\n\\n\" @ *IDSTR_NOVA_UPSCALER_NO_CONFIG_2);",0.1);
        }
    }
    Nova::handleCanvasAdjustments();
}

function loadingGUI::onOpen::handleOpenGL()
{
	HudManager::Multiplier("chat",      _($pref::hudScale::chat,1));
    HudManager::Multiplier("text",      _($pref::hudScale::text,1));
    HudManager::Multiplier("internals", _($pref::hudScale::internals,1));
    HudManager::Multiplier("orders",    _($pref::hudScale::orders,1));
    HudManager::Multiplier("radar",     _($pref::hudScale::radar,1));
    HudManager::Multiplier("retical",   _($pref::hudScale::reticle,1));
    HudManager::Multiplier("shields",   _($pref::hudScale::shields,1));
    HudManager::Multiplier("status",    _($pref::hudScale::status,1));
    HudManager::Multiplier("weapons",   _($pref::hudScale::weapons,1));
    HudManager::Multiplier("timers",    _($pref::hudScale::timers,1));
    HudManager::Multiplier("config",    _($pref::hudScale::config,1));
	Nova::flyerCampaignStateCheck();
	if(isCampaign())
	{
		if($previousGUI == SPmainGUI)
		{
			if($pref::shellScriptGL)
			{
				Nova::enableFadeEvents();
			}
		}
	}
	fov(_($client::fov,68));
}

function editorGUI::onOpen::handleOpenGL()
{	
}

function Nova::initScriptGLContext()
{
	if($pref::GWC::SIM_FS_MODE != "Upscaled" || $zzPendingReload)
	{
		return;
	}
	//Do not create a render context if we have a packet stream active. It will break recording.
	if(isObject(636))
	{
		return;
	}
	//We need temporarily disable cockpit fade-in when switching to Shell ScriptGL to prevent the fade-in from occurring in the GUI
	//Toggle it off real quick then toggle it back on once we have finished transitioning to Shell ScriptGL
	if($pref::spawnFadein)
	{
		$_pref::spawnFadein=1;
		$pref::spawnFadein=0;
	}

	$_ScriptGLInitialized = true;
	//Reconfigure the loading screen to look like the splash screen.
	//IDPAL_LOADING = 00180010, "logo.ppl";
	//IDBMP_LOADING = 00160044, "logo.bmp";
	IDPBA_CONNECTING_LEFT  = 00170029, "loadLeft.pba";
	IDPBA_CONNECTING_RIGHT = 00170030, "loadRight.pba";
	//IDPBA_LOADING_LEFT     = 00170053, "";
	//IDPBA_LOADING_RIGHT    = 00170054, "";
	$_initRenderContext=1;
	$_preInitRenderContextGUI = $currentGUI;
	schedule("playDemo(\"mods/ScriptGL/RenderContext\");",0.1);
}

function playGUI::onOpen::handleOpenGL()
{	
	$Gui::InputConfigFromSim = true;
	schedule("Nova::getHudObjects();",0.1);
	//bindCommand(keyboard, make, f11, to, "Nova::openSimPrefs();");
	if(!$pref::spawnFadeIn)
	{
		ffevent(0,0,0,0);
	}
	
	if($pref::GWC::SIM_FS_DEVICE == Glide && String::findSubStr($Nova::GlideDevice, "openglide") != -1 && $pref::terrainDetail < 20)
	{
		$pref::terrainDetail = 20; //Prevent OpenGlide from running out of emulated vmem
	}
	
	if($_initRenderContext)
	{
		$_initRenderContext='';
		client::sendKeyInput('escape');
		schedule("client::sendKeyInput('escape');",0); //Just incase the first escape doesn't fire correctly
		disconnect();
		//schedule("guiload(" @ String::Replace($_preInitRenderContextGUI, "GUI", ".gui") @ ");clientCursorOn();",0.05);
		schedule("guiload(String::Replace($_preInitRenderContextGUI, \"GUI\", \".gui\"));clientCursorOn();",0.05);
		schedule("if(!$pref::shellScriptGL){Nova::flushCanvas();}",0.06);
		schedule("if($_pref::spawnFadein){$_pref::spawnFadein=0;$pref::spawnFadein=1;}if($currentGUI != introGUI && !$_NotInNovaUI){$_NotInNovaUI=false;modloadergui::onOpen();}",0.065);
		
		//Reconfigure the loading screen back to its original assets
		//IDPAL_LOADING = 00180010, "loadConnect.ppl";
		//IDBMP_LOADING = 00160044, "loadConnect.bmp";
		IDPBA_CONNECTING_LEFT  = 00170029, "conLeft.pba";
		IDPBA_CONNECTING_RIGHT = 00170030, "conRight.pba";
		//IDPBA_LOADING_LEFT     = 00170053, "loadLeft.pba";
		//IDPBA_LOADING_RIGHT    = 00170054, "loadRight.pba";
	}
	
    Nova::RefreshRadarVectorObjects();
	if($pref::customTerrainVisbility)
	{
		Nova::disableTerrainUpdates();
		Nova::setTerrainVisibilities();
	}
	else
	{
		//Don't worry about updating the visibility. The server will do it for us momentarily.
		Nova::enableTerrainUpdates();
	}		
	
    if($OpenGL::Active)
    {
        OpenGL::unflipScaler();
        OpenGL::scaleGUI(2);
        OpenGL::shiftGUI(-1);
        schedule("Console::RenderOffset(getWindowSize(height)-6);",0.1);
        if(!isObject("NamedGuiSet\\ModloaderUI"))
        {
            schedule("setCursorPos(getWindowSize(width)/2,getWindowSize(height)/2);",0.1);
        }
        Nova::AdjustLensflareBitmaps();
        Nova::handleCanvasAdjustments();
        Nova::toggleCockpitShake(1);
        Nova::toggleCollisionMesh(1);
        Nova::toggleSmoothTankAligns();
        Nova::toggleCockpitShake();
		Nova::determineShadows();
        
        //Refresh rain/snow objects to fix their color palette when using ShellScriptGL
        if($pref::GWC::SIM_FS_MODE == "Upscaled")
        {
            if(isObject(628)){storeObject(628,'temp\\\\clip.tmp');loadObject(initSnowfall, 'temp\\\\clip.tmp');deleteObject(initSnowfall);}
        }
		//Nova::setCursorLoc(getWindowSize(width)/2,getWindowSize(height)/2);
		if($pref::shellScriptGL)
		{
			Nova::enableFadeEvents();
		}
    }
	
	//Make sure we don't soft lock into a black screen if we switch between the playgui and the mapviewgui too fast
	if($previousGUI == mapviewGUI)
	{
		focusCamera(player);
		schedule("focusCamera(player);",0);
		schedule("focusCamera(player);",0.025);
		schedule("focusCamera(player);",0.05);
		schedule("focusCamera(player);",0.075);
		schedule("focusCamera(player);",0.08);
		schedule("focusCamera(player);",0.09);
	}
    schedule('while(isObject("NamedGuiSet\\CANVAS_CULL_RIGHT")){deleteObject("NamedGuiSet\\CANVAS_CULL_RIGHT");}',0.1);
    schedule('while(isObject("NamedGuiSet\\CANVAS_CULL_BOTTOM")){deleteObject("NamedGuiSet\\CANVAS_CULL_BOTTOM");}',0.1);
}

function playGUI::onClose::handleOpenGL()
{
	$Gui::InputConfigFromSim = true;
	Nova::enableTerrainUpdates();
	$_inFreeCam=0;
	
    if($OpenGL::Active)
    {
		glRescanTextureDirectory();
        if($pref::GWC::SIM_FS_MODE == "Upscaled" && !isObject(mapviewGUI) && !isObject(editorGUI))
        {
            Schedule("Nova::FlushCanvas();",0.5);
            ffEvent(0,0,0,0);
        }
    }
}

function scrambleGUI::onOpen::handleOpenGL()
{
        if(String::findSubStr($tempVar, "Human") != -1)
        {
            IDBMP_IRC_AWAY = 00160200, "humanScramble.bmp";
        }
        else if(String::findSubStr($tempVar, "Cybrid") != -1)
        {
            IDBMP_IRC_AWAY = 00160200, "cybridScramble.bmp";
        }
}

function scrambleGUI::onClose::handleOpenGL()
{
	if(IDBMP_IRC_AWAY != "irc_icon_away.bmp")
	{
		IDBMP_IRC_AWAY = 00160200, "irc_icon_away.bmp";
	}
}

function SPmainGUI::onOpen::handleOpenGL()
{
	$zzIsCampaign = true;
	if($pref::shellScriptGL)
	{
		Nova::disableFadeEvents();
	}
    if($pref::GWC::SIM_FS_MODE == "Upscaled" && !isObject(mapviewGUI) && !isObject(editorGUI))
    {
        //loadobject(SP_BAR, "SP_BAR.object");
        //%iter1 = getNextObject(SPMAINGUI, 0);
        //%iter2 = getNextObject(SPMAINGUI, %iter1);
        //%iter3 = getNextObject(SPMAINGUI, %iter2);
        //%iter4 = getNextObject(SPMAINGUI, %iter3);
        //renameObject(%iter4, SP_BAR_POS);
        //addToSet(%iter4, SP_BAR);
        //guiSetSelection(simcanvas, "SPMAINGUI\\SP_BAR_POS\\SP_BAR");
        //guiSendtoBack(simcanvas);
		//Nova::purgeEditControl();
		ffEvent(0,0,0,0);
    }
}

function Nova::purgeEditControl()
{
    if(isObject(EditControl))
    {
        deleteObject(EditControl);
        schedule("if(isObject(EditControl)){deleteObject(EditControl);}",0.000);
        schedule("if(isObject(EditControl)){deleteObject(EditControl);}",0.015);
        schedule("if(isObject(EditControl)){deleteObject(EditControl);}",0.025);
        schedule("if(isObject(EditControl)){deleteObject(EditControl);}",0.035);
        schedule("if(isObject(EditControl)){deleteObject(EditControl);}",0.045);
        schedule("if(isObject(EditControl)){deleteObject(EditControl);}",0.055);
        schedule("if(isObject(EditControl)){deleteObject(EditControl);}",0.065);
    }
}

function trainingGUI::onOpen::handleOpenGL()
{
	$zzIsCampaign = true;
    if($pref::GWC::SIM_FS_MODE == "Upscaled")
    {
		//Key up to refresh the Simgui::guiBitmapCtrl
		client::sendKeyInput(up);
		client::sendKeyInput(up);
		client::sendKeyInput(up);
    }
}

function mapviewGUI::onOpen::NovaFunction()
{
   if($pref::autoCenterMapview)
   {
       setHudMapViewOffset(GetPosition(getNextObject(squad,0),X),GetPosition(getNextObject(squad,0),Y));
   }
}

function Nova::getInputChat()
{
    renameObject(getNextObject("NamedGuiSet\\HudDLGChatWindow",0), "HudDLGChatInput");
    control::setText("HudDLGChatInput", $pref::chatPrependText);
}

function waitroomGUI::onOpen::handleOpenGL()
{
	if($pref::shellScriptGL)
	{
		Nova::disableFadeEvents();
	}
   // if($OpenGL::Active)
   // {
       // schedule("if(isObject('NamedGuiSet\\ScriptGL')){deleteObject('NamedGuiSet\\ScriptGL');}",0.5);
       // schedule("if(isObject('NamedGuiSet\\ScriptGL')){deleteObject('NamedGuiSet\\ScriptGL');}",0.5);
   // }
}

function scriptGL::hud::onPreDraw()
{
	scriptGL::hud::drawLatencyFramerate();
	scriptgl::hud::drawReticle();
}

if(client::is3GB())
{
	$zzClientIs3GB=true;
}

function scriptGL::hud::drawMemUsage()
{
    %w=getWindowSize(width);
    %originX=%w-120;
    %originY=2;
    glTexEnvi( $GL_BLEND );	
    glColor4ub( 24,48,63,100);
    glRectangle( %originX+16, %originY+4, 84, 13 );
    glRectangle( %originX+13, %originY+1, 84, 15 );
	
	%mem = client::getMemUsed();
	
	if($zzClientIs3GB)
	{
		%warning = %mem/3000;
	}
	else
	{
		%warning = %mem/1500;
	}
	
	if(%warning < 0.5){%r=0;%g=255;}
	if(%warning > 0.65){%r=255;%g=255;}
	if(%warning > 0.9){%r=235;%g=25;}
	glColor4ub( %r, %g, 0, 255 );
	
	glSetFont( Default, 12, $GLEX_PIXEL, 0 );
	glDrawString( %originX+18, %originY+2, "RAM: " @ %mem @ "MB");	
}

function scriptGL::hud::drawLatencyFramerate()
{
	if($pref::showMem)
	{
		scriptGL::hud::drawMemUsage();
	}
	
    if(!$pref::showFPS && !$pref::showPing)
    {
        return;
    }
    %w=getWindowSize(width);
    %originX=%w-120;
    %originY=2;
    glTexEnvi( $GL_BLEND );	
    glColor4ub( 24,48,63,100);
    if($pref::showFPS && $pref::showPing)
    {
        glRectangle( %originX+16, %originY+18, 84, 47 );
        glRectangle( %originX+13, %originY+15, 84, 47 );
        %fpsOffset = 18;
        %pingOffset = 38;
    }
    else if((!$pref::showFPS && $pref::showPing) || ($pref::showFPS && !$pref::showPing))
    {
        glRectangle( %originX+16, %originY+18, 84, 26 );
        glRectangle( %originX+13, %originY+15, 84, 26 );
        if($pref::showFPS)
        {
            %fpsOffset = 18;
        }
        else
        {
            %pingOffset = 18;
        }
    }
    
    if($pref::showFPS)
    {   
        String::Explode($consoleWorld::frameRate, ".", framerate);
        
        if($framerate[0] <= 24)
        {
            glColor4ub( 255, 0, 0, 255 );
        }
        if($framerate[0] > 24 && $framerate[0] <=35)
        {
            glColor4ub( 255, 255, 0, 255 );
        }
        if($framerate[0] > 34)
        {
            glColor4ub( 0, 255, 0, 255 );
        }
        
        glSetFont( Default, 20, $GLEX_PIXEL, 0 );
		glDrawString( %originX+18, %originY+%fpsOffset, $framerate[0] @ "FPS");	
        
    }
    
    if($pref::showFPS && $pref::showPing)
    {
        %padding = 4;
    }
    else
    {
        %padding = 0;
    }
    
    if($pref::showPing)
    {
        Nova::getPing();
        if($client::ping <= 99)
        {
            glColor4ub( 0, 255, 0, 255 );
        }
        if($client::ping > 99 && $client::ping < 300)
        {
            glColor4ub( 255, 255, 0, 255 );
        }
        if($client::ping >= 300)
        {
            glColor4ub( 255, 0, 0, 255 );
        }
        
        glSetFont( Default, 20, $GLEX_PIXEL, 0 );
        glDrawString( %originX+18, %originY+%pingOffset, $client::ping @ "ms");	
    }
}

console::mute(true);
Memstar::addReplacement("c8252b2f", "CONSOLE.TGA");
Memstar::addReplacement("1ad13704", "DARKFONT.TGA");
//Memstar::addReplacement("71fe5df5", "FONTIS10_1.TGA");
//Memstar::addReplacement("29276470", "FONTIS10_2.TGA");
//Memstar::addReplacement("836b1be7", "FONTIS10_3.TGA");
//Memstar::addReplacement("34e69413", "FONTIS10_4.TGA");
//Memstar::addReplacement("2f5f68e4", "FONTIS10_5.TGA");
//Memstar::addReplacement("5ac09b23", "FONTIS10_6.TGA");
//Memstar::addReplacement("01a9e5e5", "FONTIS10_7.TGA");
//Memstar::addReplacement("36d57963", "FONTIS11_2.TGA");
//Memstar::addReplacement("fcdb1d12", "FONTIS11_3.TGA");
//Memstar::addReplacement("9747e4bc", "FONTIS11_4.TGA");
//Memstar::addReplacement("872b0dcb", "FONTIS11_5.TGA");
//Memstar::addReplacement("83fa3ccc", "FONTIS11_6.TGA");
//Memstar::addReplacement("531c5911", "FONTIS11_7.TGA");
//Memstar::addReplacement("85d3248a", "FONTIS12_1.TGA");
//Memstar::addReplacement("6e43f4ea", "FONTIS12_2.TGA");
//Memstar::addReplacement("09fa0743", "FONTIS12_3.TGA");
//Memstar::addReplacement("048ba68a", "FONTIS12_4.TGA");
//Memstar::addReplacement("fde9386c", "FONTIS12_5.TGA");
//Memstar::addReplacement("dd6b5167", "FONTIS12_6.TGA");
//Memstar::addReplacement("c9c1639e", "FONTIS12_7.TGA");
//Memstar::addReplacement("f8f887b6", "FONTIS13_1.TGA");
//Memstar::addReplacement("c4265597", "FONTIS13_2.TGA");
//Memstar::addReplacement("942ad178", "FONTIS13_3.TGA");
//Memstar::addReplacement("110cb0db", "FONTIS13_4.TGA");
//Memstar::addReplacement("9c9c7ad5", "FONTIS13_5.TGA");
//Memstar::addReplacement("af302f8f", "FONTIS14_1.TGA");
//Memstar::addReplacement("28cb6426", "FONTIS14_2.TGA");
//Memstar::addReplacement("660ec831", "FONTIS14_3.TGA");
//Memstar::addReplacement("34aa1c48", "FONTIS14_4.TGA");
//Memstar::addReplacement("1c077647", "FONTIS14_5.TGA");
//Memstar::addReplacement("07c28052", "FONTIS15_1.TGA");
//Memstar::addReplacement("c3d508a8", "FONTIS15_2.TGA");
//Memstar::addReplacement("8df1e7c1", "FONTIS15_3.TGA");
//Memstar::addReplacement("d1be89a2", "FONTIS15_4.TGA");
//Memstar::addReplacement("8cab61a2", "FONTIS15_5.TGA");
//Memstar::addReplacement("7785d996", "FONTIS16_1.TGA");
//Memstar::addReplacement("e65f8cf0", "FONTIS16_2.TGA");
//Memstar::addReplacement("eb1427a9", "FONTIS16_3.TGA");
//Memstar::addReplacement("27c2b4aa", "FONTIS16_4.TGA");
//Memstar::addReplacement("14e023da", "FONTIS16_5.TGA");
//Memstar::addReplacement("3bf9a37d", "FONTIS17_1.TGA");
//Memstar::addReplacement("fdcae017", "FONTIS17_2.TGA");
//Memstar::addReplacement("6201a5ea", "FONTIS17_3.TGA");
//Memstar::addReplacement("26efd29b", "FONTIS17_4.TGA");
//Memstar::addReplacement("0d5a370d", "FONTIS17_5.TGA");
//Memstar::addReplacement("8c94aee9", "FONTIS18_1.TGA");
//Memstar::addReplacement("42386e63", "FONTIS18_2.TGA");
//Memstar::addReplacement("9c2fe7f6", "FONTIS18_3.TGA");
//Memstar::addReplacement("a04005af", "FONTIS18_4.TGA");
//Memstar::addReplacement("931c5784", "FONTIS18_5.TGA");
//Memstar::addReplacement("b6f7566f", "FONTIS19_1.TGA");
//Memstar::addReplacement("9eecd199", "FONTIS19_2.TGA");
//Memstar::addReplacement("f2f41b2b", "FONTIS19_3.TGA");
//Memstar::addReplacement("92a39793", "FONTIS19_4.TGA");
//Memstar::addReplacement("02179be8", "FONTIS19_5.TGA");
//Memstar::addReplacement("ee1c91c7", "FONTIS20_1.TGA");
//Memstar::addReplacement("cf4d9412", "FONTIS20_2.TGA");
//Memstar::addReplacement("60559336", "FONTIS20_3.TGA");
//Memstar::addReplacement("07ed33ae", "FONTIS20_4.TGA");
//Memstar::addReplacement("c0bde2a5", "FONTIS20_5.TGA");
//Memstar::addReplacement("17911926", "FONTIS21_1.TGA");
//Memstar::addReplacement("bad7f001", "FONTIS21_2.TGA");
//Memstar::addReplacement("b12e2c77", "FONTIS21_3.TGA");
//Memstar::addReplacement("7ca2c583", "FONTIS21_4.TGA");
//Memstar::addReplacement("f3cbeabf", "FONTIS21_5.TGA");
//Memstar::addReplacement("1fbb9a7c", "FONTIS22_1.TGA");
//Memstar::addReplacement("15635dc7", "FONTIS22_2.TGA");
//Memstar::addReplacement("d0dd8d12", "FONTIS22_3.TGA");
//Memstar::addReplacement("b1265b38", "FONTIS22_4.TGA");
//Memstar::addReplacement("5009113d", "FONTIS22_5.TGA");
//Memstar::addReplacement("b55f6857", "FONTIS26_1_0.TGA");
//Memstar::addReplacement("d33cfaf8", "FONTIS26_1_1.TGA");
//Memstar::addReplacement("fa26e085", "FONTIS26_2_0.TGA");
//Memstar::addReplacement("66aa025a", "FONTIS26_2_1.TGA");
//Memstar::addReplacement("1bc6faaa", "FONTIS26_3_0.TGA");
//Memstar::addReplacement("b5a97b49", "FONTIS26_3_1.TGA");
//Memstar::addReplacement("f8177d73", "FONTIS26_4_0.TGA");
//Memstar::addReplacement("9e233404", "FONTIS26_4_1.TGA");
//Memstar::addReplacement("798beb36", "FONTIS26_5_0.TGA");
//Memstar::addReplacement("9aba459d", "FONTIS26_5_1.TGA");
//Memstar::addReplacement("660ce01b", "FONTIS30_1_0.TGA");
//Memstar::addReplacement("32aefc32", "FONTIS30_1_1.TGA");
//Memstar::addReplacement("926486f2", "FONTIS30_2_0.TGA");
//Memstar::addReplacement("a50d70a9", "FONTIS30_2_1.TGA");
//Memstar::addReplacement("d36e8cd6", "FONTIS30_3_0.TGA");
//Memstar::addReplacement("970b752a", "FONTIS30_3_1.TGA");
//Memstar::addReplacement("dfbca682", "FONTIS30_4_0.TGA");
//Memstar::addReplacement("a455c1bd", "FONTIS30_4_1.TGA");
//Memstar::addReplacement("082ea149", "FONTIS30_5_0.TGA");
//Memstar::addReplacement("fabec543", "FONTIS30_5_1.TGA");
//Memstar::addReplacement("d42a456f", "FONTIS7_1.TGA");
//Memstar::addReplacement("83137ee4", "FONTIS7_2.TGA");
//Memstar::addReplacement("124fd42b", "FONTIS7_3.TGA");
//Memstar::addReplacement("184f457e", "FONTIS7_4.TGA");
//Memstar::addReplacement("d4678bc2", "FONTIS7_5.TGA");
//Memstar::addReplacement("c47962bf", "FONTIS8_1.TGA");
//Memstar::addReplacement("6a896ebc", "FONTIS8_2.TGA");
//Memstar::addReplacement("64a9f9aa", "FONTIS8_3.TGA");
//Memstar::addReplacement("647c1ada", "FONTIS8_4.TGA");
//Memstar::addReplacement("2dc10e32", "FONTIS8_5.TGA");
//Memstar::addReplacement("f0c7e28e", "FONTIS8_7.TGA");
//Memstar::addReplacement("6c581cd7", "FONTIS9_1.TGA");
//Memstar::addReplacement("b97a2e96", "FONTIS9_2.TGA");
//Memstar::addReplacement("c6ae0150", "FONTIS9_3.TGA");
//Memstar::addReplacement("5c4dce49", "FONTIS9_4.TGA");
//Memstar::addReplacement("4c930b78", "FONTIS9_5.TGA");
//Memstar::addReplacement("28059666", "FONTIS9_7.TGA");
//Memstar::addReplacement("1cc43d1d", "HUD_HIGH.TGA");
//Memstar::addReplacement("686af986", "HUD_HIGH_BLUE.TGA");
//Memstar::addReplacement("8c6a51aa", "HUD_HIGH_DIM.TGA");
//Memstar::addReplacement("916ea02c", "HUD_HIGH_GRAY.TGA");
//Memstar::addReplacement("a16d2034", "HUD_HIGH_I.TGA");
//Memstar::addReplacement("805f1dd2", "HUD_HIGH_PURPLE.TGA");
//Memstar::addReplacement("444dd27c", "HUD_HIGH_RED.TGA");
//Memstar::addReplacement("cafc296c", "HUD_HIGH_YELLOW.TGA");
//Memstar::addReplacement("43120d86", "HUD_LOW.TGA");
//Memstar::addReplacement("a66be26f", "HUD_LOW_BLUE.TGA");
//Memstar::addReplacement("008ae88e", "HUD_LOW_DIM.TGA");
//Memstar::addReplacement("2d4a1e15", "HUD_LOW_GRAY.TGA");
//Memstar::addReplacement("6a73750f", "HUD_LOW_I.TGA");
//Memstar::addReplacement("4b9cee7e", "HUD_LOW_PURPLE.TGA");
//Memstar::addReplacement("1a8a3cb5", "HUD_LOW_RED.TGA");
//Memstar::addReplacement("8128b435", "HUD_LOW_YELLOW.TGA");
//Memstar::addReplacement("3743b8c2", "LUCIDA10_1.TGA");
//Memstar::addReplacement("1196c1f8", "LUCIDA10_2.TGA");
//Memstar::addReplacement("c1e9e5fb", "LUCIDA10_3.TGA");
//Memstar::addReplacement("9b5c297a", "LUCIDA10_4.TGA");
Memstar::addReplacement("41a1f01f", "LUCIDA10_5.TGA");
//Memstar::addReplacement("e65cc552", "LUCIDA10_6.TGA");
//Memstar::addReplacement("dffecc18", "LUCIDA11_1.TGA");
//Memstar::addReplacement("073880bc", "LUCIDA11_2.TGA");
//Memstar::addReplacement("035498a5", "LUCIDA11_3.TGA");
//Memstar::addReplacement("3c0b0408", "LUCIDA11_4.TGA");
//Memstar::addReplacement("24ed4577", "LUCIDA11_5.TGA");
//Memstar::addReplacement("d8564efe", "LUCIDA11_6.TGA");
//Memstar::addReplacement("c890a55d", "LUCIDA12_1.TGA");
Memstar::addReplacement("12b23918", "LUCIDA12_2.TGA");
//Memstar::addReplacement("a5c1d7b3", "LUCIDA12_3.TGA");
//Memstar::addReplacement("70403abe", "LUCIDA12_4.TGA");
//Memstar::addReplacement("e46339a9", "LUCIDA12_5.TGA");
//Memstar::addReplacement("771721e1", "LUCIDA12_6.TGA");
//Memstar::addReplacement("d2203544", "LUCIDA7_1.TGA");
//Memstar::addReplacement("f354bc55", "LUCIDA7_2.TGA");
//Memstar::addReplacement("d47aa87a", "LUCIDA7_3.TGA");
//Memstar::addReplacement("bc5746d2", "LUCIDA7_3I.TGA");
//Memstar::addReplacement("bb02ff66", "LUCIDA7_4.TGA");
//Memstar::addReplacement("6724d6fc", "LUCIDA7_5.TGA");
//Memstar::addReplacement("8e3b3b16", "LUCIDA7_6.TGA");
//Memstar::addReplacement("41bb4e35", "LUCIDA7_7.TGA");
//Memstar::addReplacement("c8252b2f", "LUCIDA8_1.TGA");
//Memstar::addReplacement("f1168cde", "LUCIDA8_2.TGA");
//Memstar::addReplacement("5e1da3f9", "LUCIDA8_3.TGA");
//Memstar::addReplacement("fbce8a38", "LUCIDA8_4.TGA");
//Memstar::addReplacement("75a8067d", "LUCIDA8_5.TGA");
//Memstar::addReplacement("5a477746", "LUCIDA8_6.TGA");
//Memstar::addReplacement("ec031b8b", "LUCIDA8_7.TGA");
//Memstar::addReplacement("b48913b4", "LUCIDA9_1.TGA");
//Memstar::addReplacement("387fa886", "LUCIDA9_2.TGA");
//Memstar::addReplacement("a8605faa", "LUCIDA9_3.TGA");
//Memstar::addReplacement("4ba2d76d", "LUCIDA9_3I.TGA");
//Memstar::addReplacement("e31f8608", "LUCIDA9_4.TGA");
//Memstar::addReplacement("2b3c00d9", "LUCIDA9_5.TGA");
//Memstar::addReplacement("5220a0a7", "LUCIDA9_6.TGA");
//Memstar::addReplacement("bcf00b7a", "LUCIDA9_7.TGA");
//Memstar::addReplacement("8756b37d", "MEFONT.TGA");
//Memstar::addReplacement("2897c6a2", "MEFONTHL.TGA");

//Hashes that keep popping up during the batch process. Filter them out
Memstar::addReplacement("6ea100f7","");
Memstar::addReplacement("3edc277b","");
Memstar::addReplacement("28bedc6d","");
Memstar::addReplacement("f9a5cd14","");
Memstar::addReplacement("2d0c2b72","");
Memstar::addReplacement("33377c6b","");
Memstar::addReplacement("b41b1f1a","");
Memstar::addReplacement("cce2e804","");
Memstar::addReplacement("ab427ca0","");
Memstar::addReplacement("cc6bddd5","");
Memstar::addReplacement("5663120b","");
Memstar::addReplacement("ecd39446","");
console::mute(false);
}

if(Nova::isAMD())
{
    if(String::findSubStr($Nova::OpenGLDevice, "AMD ")!=-1){%amd=true;}
    if(String::findSubStr($Nova::OpenGLDevice, " AMD ")!=-1){%amd=true;}
    if(String::findSubStr($Nova::OpenGLDevice, " AMD")!=-1){%amd=true;}
    if(String::findSubStr($Nova::OpenGLDevice, "Radeon")!=-1){%amd=true;}
    if(String::findSubStr($Nova::OpenGLDevice, "AMD Radeon")!=-1){%amd=true;}
    if(%amd)
    {
        OpenGL::AMDAATweak(true);
    }
}

messageCanvasDevice(simcanvas, enableCacheNoise, 0);
flushTextureCache();

//Replace the standard reticle bitmaps with empty tga images
if(isFile(".\\mods\\scriptGL\\ScriptGL_reticle.zip"))
{
	console::mute(true);
	Memstar::addReplacement("25e16966", "reticle_override.TGA");
	Memstar::addReplacement("c7f8d3c6", "reticle_override.TGA");
	Memstar::addReplacement("59ccc303", "reticle_override.TGA");
	console::mute(false);
}

$_zzScriptGLHudFlatten = 0;

$scriptGL::hudColor[0,R] = 0;
$scriptGL::hudColor[0,G] = 255;
$scriptGL::hudColor[0,B] = 32;

$scriptGL::hudColor[1,R] = 255;
$scriptGL::hudColor[1,G] = 255;
$scriptGL::hudColor[1,B] = 0;

$scriptGL::hudColor[2,R] = 80;
$scriptGL::hudColor[2,G] = 175;
$scriptGL::hudColor[2,B] = 230;

$scriptGL::hudColor[3,R] = 255;
$scriptGL::hudColor[3,G] = 195;
$scriptGL::hudColor[3,B] = 5;

$scriptGL::hudColor[4,R] = 255;
$scriptGL::hudColor[4,G] = 75;
$scriptGL::hudColor[4,B] = 75;

$scriptGL::hudColor[5,R] = 200;
$scriptGL::hudColor[5,G] = 200;
$scriptGL::hudColor[5,B] = 200;

$scriptGL::hudColor[6,R] = 255;
$scriptGL::hudColor[6,G] = 0;
$scriptGL::hudColor[6,B] = 255;

$scriptGL::hudColor[7,R] = 175;
$scriptGL::hudColor[7,G] = 250;
$scriptGL::hudColor[7,B] = 145;

$scriptGL::hudColor[8,R] = 0;
$scriptGL::hudColor[8,G] = 255;
$scriptGL::hudColor[8,B] = 32;

$_zzScriptGLHitFeedbackExpand = 0;
$_zzScriptGLHitFeedFade = 0;
$_zzScriptGLHasHitFeedback=0;

function remoteNova::hitFeedback(%cli)
{
    if(%cli == 2048)
    {
		$_zzScriptGLHitFeedbackExpand = 0;
		$_zzScriptGLHitFeedbackFade = 0;
        $_zzScriptGLHasHitFeedback = 1;
    }
}

function scriptgl::hud::drawReticle()
{
	if(!isFile(".\\mods\\scriptGL\\ScriptGL_reticle.zip"))
	{
		return;
	}
	if($pref::hudScale::retical == 1){%scale = 0.25;}
	if($pref::hudScale::retical == 2){%scale = 0.35;}
	if($pref::hudScale::retical == 3){%scale = 0.40;}
	if($pref::hudScale::retical == 4){%scale = 0.45;}

	glEnable( $GL_TEXTURE_2D );
	glTexEnvi( $GL_MODULATE );
	if(control::getVisible(IDHUD_AIM_RET))
	{
		if($_zzScriptGLHudFlatten > 0)
		{
			$_zzScriptGLHudFlatten -= 0.05;
		}
				
		//If this variable is set true it will trigger the 'pulse' effect on the crosshair
		if($_zzScriptGLHasHitFeedback)
		{
			//Expand
			if($_zzScriptGLHitFeedbackExpand < 0.5)
			{
				$_zzScriptGLHitFeedbackExpand += 0.025;
			}
			
			//Fade out
			if($_zzScriptGLHitFeedbackFade < 200)
			{
				$_zzScriptGLHitFeedbackFade += 8;
			}
			
			//Hud palette color index
			if($pref::hud::paletteIndex < 8)
			{
				%palOffset = 1;
			}
			else
			{
				%palOffset = -7;
			}
			glColor4ub( $scriptGL::hudColor[$pref::hud::paletteIndex+%palOffset,R], $scriptGL::hudColor[$pref::hud::paletteIndex+%palOffset,G], $scriptGL::hudColor[$pref::hud::paletteIndex+%palOffset,B], 200-$_zzScriptGLHitFeedbackFade);
			glDrawTexture( "reticle.tga", $GLEX_ROTATED, $vehicle::reticleXPosition, $vehicle::reticleYPosition, %scale+$_zzScriptGLHitFeedbackExpand,%scale+$_zzScriptGLHitFeedbackExpand);
			if($_zzScriptGLHitFeedbackFade >= 200)
			{
				$_zzScriptGLHitFeedbackExpand = 0;
				$_zzScriptGLHitFeedbackFade = 0;
				$_zzScriptGLHasHitFeedback = 0;
			}
		}
		
		glColor4ub( $scriptGL::hudColor[$pref::hud::paletteIndex,R], $scriptGL::hudColor[$pref::hud::paletteIndex,G], $scriptGL::hudColor[$pref::hud::paletteIndex,B], 255);
	
		//Default reticle
		if(!$vehicle::reticleOnTarget && !$vehicle::reticleMissilesLocked)
		{
			glDrawTexture( "reticle.tga", $GLEX_ROTATED, $vehicle::reticleXPosition, $vehicle::reticleYPosition, %scale,%scale-$_zzScriptGLHudFlatten);
		}
		
		//When missiles are locked
		else if($vehicle::reticleMissilesLocked)
		{
			glDrawTexture( "reticleMissileLock.tga", $GLEX_ROTATED, $vehicle::reticleXPosition, $vehicle::reticleYPosition, %scale,%scale-$_zzScriptGLHudFlatten);
		}
		
		//When the reticle is over a target vehicle
		else if($vehicle::reticleOnTarget)
		{
			glDrawTexture( "reticleOnTarget.tga", $GLEX_ROTATED, $vehicle::reticleXPosition, $vehicle::reticleYPosition, %scale,%scale-$_zzScriptGLHudFlatten);
		}
	}
	else if(!control::getVisible(IDHUD_AIM_RET))
	{
		
		//Flatten out the crosshair to hide it when we lose energy
		if($_zzScriptGLHudFlatten < %scale)
		{
			$_zzScriptGLHudFlatten += 0.05;
		}
		else
		{
			$vehicle::reticleOnTarget=0;
			$vehicle::reticleMissilesLocked=0;
			return;
		}
		if($pref::relativeDamageStatus)
		{
			setHudLabel(0,0,0,0,0);
			setHudLabel(1,0,0,0,0);
		}
	}
	
	if($pref::relativeDamageStatus)
	{
		//$console::printlevel=0;
		$hudObject[damage] = Nova::findGuiTagControl(651, IDHUD_DAMAGE);
		//$hudObject[shields] = Nova::findGuiTagControl(651, IDHUD_SHIELD);
		if(isObject($hudObject[damage]) && getGroup($hudObject[damage]) == 651)
		{
			//Damage Status
			String::Explode(getGuiObjectExtent($hudObject[damage]), ",", damageStatus);
			%selfWidth = $damageStatus[0];
			%selfHeight = $damageStatus[1];
			setGuiObjectPosition($hudObject[damage], $vehicle::reticleXPosition-(%selfWidth*2)-$pref::relativeDamageStatusOffset, $vehicle::reticleYPosition-(%selfHeight/2));
			//if(control::getVisible(IDHUD_AIM_RET))
			//{
			//	setHudLabel(0, *IDSTR_ENERGY @ " " @ round($vehicle::energyLevel*100), $vehicle::reticleXPosition/getWindowSize(width),$vehicle::reticleYPosition/getWindowSize(height)+0.05+($pref::relativeDamageStatusOffset2/getWindowSize(height)), true);
			//	setHudLabel(1, *IDSTR_SHIELDS @ " " @ round($vehicle::shieldStr), $vehicle::reticleXPosition/getWindowSize(width),$vehicle::reticleYPosition/getWindowSize(height)+0.03+($pref::relativeDamageStatusOffset2/getWindowSize(height)), true);
			//}
			//else
			//{
			//	setHudLabel(0,0,0,0,0);
			//	setHudLabel(1,0,0,0,0);
			//}
		}
		
		$hudObject[target] = Nova::findGuiTagControl(651, IDHUD_TARGET);
		if(isObject($hudObject[target]) && getGroup($hudObject[target]) == 651)
		{
			//Target Status
			String::Explode(getGuiObjectExtent($hudObject[target]), ",", targetStatus);
			%targetWidth = $targetStatus[0];
			%targetHeight = $targetStatus[1];
			setGuiObjectPosition($hudObject[target], $vehicle::reticleXPosition+(%targetWidth*1)+$pref::relativeDamageStatusOffset, $vehicle::reticleYPosition-(%targetHeight/2));
		}
		//$console::printlevel=1;
	}
	
	if($pref::relativeShieldRadar)
	{
		//$console::printlevel=0;
		$hudObject[shield] = Nova::findGuiTagControl(651, IDHUD_SHIELD);
		if(isObject($hudObject[shield]) && getGroup($hudObject[shield]) == 651)
		{
			String::Explode(getGuiObjectExtent($hudObject[shield]), ",", shieldExtent);
			setGuiObjectPosition($hudObject[shield], $vehicle::reticleXPosition-($shieldExtent[0]*2)-$pref::relativeDamageStatusOffset, $vehicle::reticleYPosition+(75*($pref::hudScale::status/2)));
		}
		$hudObject[radar] = Nova::findGuiTagControl(651, IDHUD_GEN_RADAR);
		if(isObject($hudObject[radar]) && getGroup($hudObject[radar]) == 651)
		{
			String::Explode(getGuiObjectExtent($hudObject[radar]), ",", radarExtent);
			setGuiObjectPosition($hudObject[radar], $vehicle::reticleXPosition+($radarExtent[0]/4)+$pref::relativeDamageStatusOffset, $vehicle::reticleYPosition+(75*($pref::hudScale::status/2)));
		}
		//$console::printlevel=1;
	}
}

function scriptGL::Nova::onPreDraw()
{
	//Don't function in the campaign. Some campaigns are not meant to have their over map moved around.
	if(isObject(mapviewgui) && $pref::autoCenterMapview)
	{
		setHudMapViewOffset(GetPosition(getNextObject(squad,0),X),GetPosition(getNextObject(squad,0),Y));
	}
}

IDSTR_LWR_CYBRID                 = 00130885, "Cybrid ";
IDSTR_LWR_HUMAN                  = 00130886, "Human ";
IDSTR_LWR_COMMON                 = 00130887, "Common ";
IDSTR_LWR_ALIEN                  = 00130888, "Alien ";
IDSTR_LWR_YES                    = 00130889, "Yes";
IDSTR_LWR_NO                     = 00130890, "No";