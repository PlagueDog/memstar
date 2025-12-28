//ScriptGL for 1.003 causes fonts inside Simgui::HudWindow which use the same color index as the retical to render as white blocks. Hiding the retical control fixes this.
//For now we codepatch all the internal GuiPushDialogs related to the hud to execute the functions in here.

if(String::Right($version, 6) == "1.003r")
{
    function ScriptGL::GuiPushDialog(%file)
    {
        if(isObject(playgui))
        {
            if(control::getVisible(IDHUD_AIM_RET))
            {
                control::setVisible(IDHUD_AIM_RET, 0); //Hide the retical
                guiPushDialog(simcanvas, %file); // Load the dialog
                clientCursorOn();
                bind(keyboard0, make, Escape, TO, "control::setVisible(IDHUD_AIM_RET, 1);GuiPopDialog(simcanvas, 0);unbind(keyboard0, make, Escape);");
            }
        }
    }
}