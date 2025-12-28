//Misc fixes for campaigns and Nova's interaction with them
//function playGUI::onOpen::CampaignFunction()
function Nova::campaignCompat()
{
    %currentMission = $temp;
	Nova::campaignCompatTicker(%currentMission);
	return;
}

function Nova::campaignCompatTicker(%currentMission)
{
	if(!isObject(playgui) && !isObject(waitroomgui))
	{
		schedule("Nova::campaignCompatTicker($temp);",0.1);
	}
	else
	{
		//Make sure the camera attaches to the spline camera in cinematic recordings
        if(String::findSubStr($temp, ".rec") > 1)
        {
			if(isObject("ghostgroup/path1"))
			{
				ffevent(0,0,0,5);
				schedule("focusCamera( splineCamera, path1 );",0);
				schedule("focusCamera( splineCamera, path1 );",0.1);
				schedule("focusCamera( splineCamera, path1 );",0.2);
				schedule("focusCamera( splineCamera, path1 );",0.3);
				schedule("focusCamera( splineCamera, path1 );",0.35);
				$temp=0;
			}
        }
	}
}