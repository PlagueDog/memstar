// $WinConsoleEnabled=true;function Engine::endFrame(){}
Console::enable(true);
//LOGGING
// $console::logmode=2;
// $console::printlevel=2;
function Nova::isCargv(%cargv)
{
	while(32>=%i++)
	{
		if(%cargv == $cargv[%i])
		{
			return true;
		}
	}
	return false;
}

function Nova::getCargv()
{
	while(32>=%i++)
	{
		if(strlen($cargv[%i]))
		{
			if(!strlen(%result))
			{
				%result = $cargv[%i];
			}
			else
			{
				%result = %result @ "," @$cargv[%i];
			}			
		}
	}
	return %result;
}

function Nova::isFunction(%fName)
{
	if(!strlen(%fName))
	{
		echo("Nova::isfunction( \"functionName\" );");
		return false;
	}
	%fName = String::Replace(%fName, "(", "");
	%fName = String::Replace(%fName, ")", "");
	%fName = String::Replace(%fName, ";", "");
	return isFunction(%fName);
}

function Nova::randomStr(%length)
{
	while(%i++<=%length)
	{
		%r = randomInt(1,3);
		if(%r==1) //numbers
		{
			%s = chr(randomInt(48,56));
		}
		else if(%r==2) //uppercase
		{
			%s = chr(randomInt(65,90));
		}
		else //lowercase
		{
			%s = chr(randomInt(97,122));
		}
		%str = %str @ %s;
	}
	return %str;
}

//Retained to keep legacy functions from breaking
function StringM::explode(%arg1,%arg2,%arg3)
{
    String::Explode(%arg1,%arg2,%arg3);
}
//Retained to keep legacy functions from breaking
function strAlignR(%arg1, %arg2)
{
    String::Right(%arg2, %arg1);
}

function console::mute(%bool)
{
	if(%bool == 1 || String::toLower(%bool) == "t" || String::toLower(%bool) == "true")
	{
		$console::printLevel=0;
	}
	else
	{
		$console::printLevel = 1;
	}
}

//Correct floating point errors
function flt(%flt)
{ 
	deleteVariables("_flt*");
	String::Explode(%flt,".",_flt);
	//Assign the exploded variable to a local variable to avoid updating the parse throughout the function
	%len = $_flt[1];
	
	if(String::char(%flt,1) == "@")
	{
		return %flt;
	}
	
	%decimal = String::findSubStr(%flt, ".");
	%char = String::char(%flt, %decimal+2);
	
	if(%decimal == -1)
	{
		return %flt;
	}
	
	// echo('Debug0');
	for(%i=65;%i<=122;%i++)
	{
		if((%char == chr(%i)) && %char != '0')
		{
			return %flt;
		}
	}
	
	if(!strlen(%len))
	{
		return %flt;
	}
	
	// echo('Debug1');
	//Check for *.00|25|75
	if(strlen(%len) == 2)
	{
		if($_flt[1] == '00' || $_flt[1] == 25 || $_flt[1] == 50 || $_flt[1] == 75)
		{
			return %flt;
		}
	}
	
	if(strlen(%len) == 3)
	{
		if($_flt[1] == '000' || $_flt[1] == 250 || $_flt[1] == 500 || $_flt[1] == 750)
		{
			return %flt;
		}
	}
	
	if(strlen(%len) == 4)
	{
		if($_flt[1] == '0000' || $_flt[1] == 2500 || $_flt[1] == 5000 || $_flt[1] == 7500)
		{
			return %flt;
		}
	}
	
	if(strlen(%len) == 5)
	{
		if($_flt[1] == '00000' || $_flt[1] == 25000 || $_flt[1] == 50000 || $_flt[1] == 75000)
		{
			return %flt;
		}
	}

	// echo('Debug2');
	if(strlen(%len) == 9)
	{
		if(%flt >= 0)
		{
			return (%flt + '0.000000002');
		}
		else
		{
			return (%flt - '0.000000002');
		}
	}
	else if(strlen(%len) == 8)
	{
		if(%flt >= 0)
		{
			return (%flt + '0.00000002');
		}
		else
		{
			return (%flt - '0.00000002');
		}
	}
	else if(strlen(%len) == 7)
	{
		if(%flt >= 0)
		{
			return (%flt + '0.0000002');
		}
		else
		{
			return (%flt - '0.0000002');
		}
	}
	else if(strlen(%len) == 6)
	{
		if(%flt >= 0)
		{
			return (%flt + '0.000002');
		}
		else
		{
			return (%flt - '0.000002');
		}
	}
	else if(strlen(%len) == 5)
	{
		if(%flt >= 0)
		{
			return (%flt + '0.00002');
		}
		else
		{
			return (%flt - '0.00002');
		}
	}
	else if(strlen(%len) == 4)
	{
		if(%flt >= 0)
		{
			return (%flt + '0.0002');
		}
		else
		{
			return (%flt - '0.0002');
		}
	}
	else if(strlen(%len) == 3)
	{
		if(%flt >= 0)
		{
			return (%flt + '0.002');
		}
		else
		{
			return (%flt - '0.002');
		}
	}
	else if(strlen(%len) == 2)
	{
		if(%flt >= 0)
		{
			return (%flt + '0.02');
		}
		else
		{
			return (%flt - '0.02');
		}
	}
	
	//Check for *.0|5
	if(strlen(%len) == 1)
	{
		if($_flt[1] == 0 || $_flt[1] == 5)
		{
			return %flt;
		}
	}
	
	return %flt;
}

//function Nova::migrateGUI(%currentGUI)
//{
//	while(getNextObject(%currentGUI, %iter) != 0)
//	{
//		%iter = getNextObject(%currentGUI, %iter);
//		%objectIDstring = %objectIDstring @ %iter @ ",";
//	}
//	%objectIDstring = String::Left(%objectIDstring, strLen(%objectIDstring)-1);
//	newobject(scriptGL, Simgui::TSControl, 0,0,640,480);
//   addToSet($currentGUI, scriptGL);
//	%evalString = "addToSet('" @ %currentGUI @ "\\ScriptGL'," @ %objectIDstring @ ");";
//	eval(%evalString);
//	Nova::copyToClipboard(%evalString);
//}