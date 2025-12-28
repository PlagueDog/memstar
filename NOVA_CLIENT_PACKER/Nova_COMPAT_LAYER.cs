$__COMPAT_LAYER = Nova::getCompatLayer();
$__COMPAT_ENV[1] = "Win2000";
$__COMPAT_ENV[3] = "NT4SP5";
$__COMPAT_ENV[4] = "VISTARTM";
$__COMPAT_ENV[5] = "VISTASP1";
$__COMPAT_ENV[6] = "VISTASP2";
$__COMPAT_ENV[7] = "WIN7RTM";
$__COMPAT_ENV[8] = "WIN8RTM";

function __COMPAT_LAYER_CHECK()
{
    for(%i=0;%i<9;%i++)
    {
        if(String::findSubStr($__COMPAT_LAYER,$__COMPAT_ENV[%i]) != -1)
        {
            $__COMPAT_LAYER_FAIL = false;
            return;
        }
        $__COMPAT_LAYER_FAIL = true;
    }
}

if($__COMPAT_LAYER_FAIL)
{
	IDSTR_MISSING_FILE_TITLE = 00131400,"Compatibility Mode Required";
	IDSTR_MISSING_FILE_ERROR = 00131401,"Starsiege must be ran in compatibility mode when using Nova.\n\nSupported Compatibility Modes:\nWindows XP through Windows 8";
	checkForFile(randomInt(1,9999999));
}