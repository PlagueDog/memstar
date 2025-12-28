function Nova::initIRC()
{
    if(!$client::Name)
    {
        $client::Name = "StarsiegePlayer";
    }
    $IRC::channelKey = SSP;
    $IRC::AutoReconnect = "True";
    $IRC::MsgHistory = "50";
    $IRC::NickName = $client::Name;
    $IRC::OnInSim = "True";
    $IRC::Port1 = "6667";
    $IRC::RealName = $client::Name;
    $IRC::Server = "irc.freenode.net";
    $IRC::Server1 = "irc.freenode.net";
    $IRC::ServerPort = "6667";
    ircConnect("irc.freenode.net", 6667);
    schedule('ircSend("/join #Nova-Starsiege");',8);
    schedule("ircSend(\"/MODE #Nova-Starsiege +s\");", 9);
}