function modLoader::Logger::Reset()
{
    dyndatawriteclasstype(Droppoint, "modLoaderLog.html");
    filewrite("modLoaderLog.html", overwrite, "<!DOCTYPE html>",  "<html>", "<body style='background-color:#000000;'>","<h1 style=\"color:white;\">Modloader Log</h1>","<h2 style=\"color:white;\">Version: <br>" @ $Nova::Version @ "</h2>");
}
modLoader::Logger::Reset();

function modLoader::Logger::newEntry(%type, %string)
{
    if(strlen(%string) == 0)   
    {
        return;
    }
    if(%type == "dir")
    {
        %prepend = "<b style=\"background-color:rgba(126, 126, 126, 0.5);\">" @ gettime() @ "</b>  <b> [ <b style=\"background-color:rgba(0, 0, 255, 50);\">REPATH</b> ]</b> &#8594";
    }
    if(%type == "info")
    {
        %prepend = "<br><b style=\"background-color:rgba(126, 126, 126, 0.5);\">" @ gettime() @ "</b>  <b> [ <b style=\"background-color:rgba(126, 126, 126, 0.5);\">INFO</b> ]</b> &#8594";
    }
    if(%type == "normal")
    {
        %prepend = "<b style=\"background-color:rgba(126, 126, 126, 0.5);\">" @ gettime() @ "</b>  <b> [ <b style=\"background-color:rgba(43, 150, 0, 0.7);\">OK</b> ]</b> &#8594";
    }
    if(%type == "warn")
    {
        %prepend = "<b style=\"background-color:rgba(126, 126, 126, 0.5);\">" @ gettime() @ "</b>  <b> [ <b style=\"background-color:rgba(204, 112, 0, 0.8);\">WARNING</b> ]</b> &#8594";
    }
    if(%type == "error")
    {
        %prepend = "<b style=\"background-color:rgba(126, 126, 126, 0.5);\">" @ gettime() @ "</b>  <b>[ <b style=\"background-color:rgba(255, 0, 0, 0.5);\">ERROR</b> ]</b> &#8594";
        $modloaderIDerr++;
    }
    filewrite("modLoaderLog.html", append, "<b style=\"color:darkgray;\">" @ %prepend @ "   " @ %string @ "</b><br>");
}