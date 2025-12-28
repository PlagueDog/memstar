//#####################################################################
//
// Squadmate Action Defaults
//
//#####################################################################

function vehicle::onAction( %action, %param1, %param2 )
{
	if(isCampaign())
	{
		echo( "vehicle::onAction" );
		echo( "Responding to player action" @ %action @ %param1 @ %param2 );
		order( PlayerSquad, %action, %param1, %param2 );
	}
}   

