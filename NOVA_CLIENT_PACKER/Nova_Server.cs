if($CmdLineServer)
{
    function Nova::handleLoseFocus(){}
    function Nova::pushConsole(){}
    function Nova::Console::onOpen(){}
    
    focusClient();
    newObject(simCanvas,SimGui::Canvas,Starsiege,50,50,true,1);
    setCursor( simCanvas, "cursor.bmp" );
    inputActivate(all);
    GuiNewContentCtrl( simCanvas, simGui::TSControl );
    setFullscreenMode(simcanvas,false);
    focusServer();
}