$basepath = $basepath @ ";multiplayer;";
appendSearchPath();
//SSP Client
function noCD4()
    {control::setVisible(IDMMT_MAIN,0);
    control::setVisible(IDMMT_SP,0);
    control::setVisible(IDMMT_MP,1);}

function noCD3()
    {control::setVisible(IDMMT_MAIN,0);
    control::setVisible(IDMMT_MT,0);
    control::setVisible(IDMMT_SP,1);}

function noCD2()
{guiload("Training.gui");}

function noCD1()
{guiload("Tutorial.gui");}

//StarsiegeComplete Client
function MPB::onAction()
{control::SetVisible(MM, 0);control::SetVisible(MP, 1);}

function SPB::onAction()
{control::SetVisible(MM, 0);control::SetVisible(SP, 1);}

function JGB::onAction(){}

function HGB::onAction(){}

function TRAINB::onAction()
{guiload("Training.gui");}

function TUTB::onAction()
{guiload("Tutorial.gui");}