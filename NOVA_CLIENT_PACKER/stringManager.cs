// FILENAME: stringManager.cs
//
// AUTHOR: Drake
//
// AUTHOR NOTE: Special characters will NOT be added into the list.
// I just can't be bothered. Special characters will show up as a blank
// in a string should you have any in said string.
//
//---------------------------------------------------------------------

function stringM::explode(%string, %delim, %arr) {
    if(strAlign(1, left, %arr) != "$")
        %arr = strCat("$", %arr);
    %i = 0;
    %j = 0;
    while(%j < strLen(%string)) {
		%del = String::getSubstr(%string, %j, strlen(%delim));
        if(%del == %delim) {
			%entry = String::getSubstr(%string, 0, %j);
			%string = String::getSubstr(%string, (%j + strlen(%delim)), strlen(%string));
            eval(%arr @ "[" @ %i @ "] = \"" @ %entry @ "\";");
            %i++;
            %j = 0;
        }
        %j++;
    }
    eval(%arr @ "[" @ %i @ "] = \"" @ %string @ "\";");
}

//---------------------------------------------------------------------
// strAlignR();
// INFO: Obtain string information from the right side of a string
// instead of the left.
//---------------------------------------------------------------------

function strAlignR(%count, %string)
{
	%length = strLen(%string);
	//%string = convertSpaces(%string);
	%last = "";
	for(%i = 1; %i <= %length; %i++)
	{
		%strShort = strAlign(%i, left, %string);
		//%strShort = String::Left(%string, %i);
		%letter[%i] = checkString(%strShort, %last);
		//if(%letter[%i] == "_") %letter[%i] = " ";
		%last = %strShort;
	}
	%final = %length - %count + 1;
	%result = "";
	for(%j = %length; %j >= %final; %j--)
		%result = %letter[%j] @ %result;

	return %result;
}

//---------------------------------------------------------------------
// pickChar();
// INFO: Obtain a specific character in a string. Mainly a data
// handling function.
//---------------------------------------------------------------------

function pickChar(%string, %char)
{
	%length = strLen(%string);
	%last = "";
	for(%i = 1; %i <= %length; %i++)
	{
		%strShort = strAlign(%i, left, %string);
		%letter[%i] = checkString(%strShort, %last);
		%last = %strShort;
	}
	return %letter[%char];
}

//---------------------------------------------------------------------
// checkString();
// INFO: Checks a string for a valid character based on the character
// array at the bottom.
//---------------------------------------------------------------------

function checkString(%strShort, %last)
{
	for(%i = 0; %i <= $SM::charCount; %i++)
		if(%strShort == %last @ $SM::char_array[%i])
			return $SM::char_array[%i];
	return " ";
}

//---------------------------------------------------------------------
// getChar();
// INFO: Obtain a character from the array at the bottom through a
// function.
//---------------------------------------------------------------------

function getChar(%char)
{
	if(!(%char < 0) && !(%char > $SM::charCount))
		return $SM::char_array[%char];
	else
	{
		echo("Character does not exist.");
		return " ";
	}
}

//---------------------------------------------------------------------
// Alphabet Array List
// Note: Need to have the letters individually in an array to check for
// in the provided functions.
//---------------------------------------------------------------------

$SM::char_array[0] = "\x20";
$SM::char_array[1] = "a";
$SM::char_array[2] = "b";
$SM::char_array[3] = "c";
$SM::char_array[4] = "d";
$SM::char_array[5] = "e";
$SM::char_array[6] = "f";
$SM::char_array[7] = "g";
$SM::char_array[8] = "h";
$SM::char_array[9] = "i";
$SM::char_array[10] = "j";
$SM::char_array[11] = "k";
$SM::char_array[12] = "l";
$SM::char_array[13] = "m";
$SM::char_array[14] = "n";
$SM::char_array[15] = "o";
$SM::char_array[16] = "p";
$SM::char_array[17] = "q";
$SM::char_array[18] = "r";
$SM::char_array[19] = "s";
$SM::char_array[20] = "t";
$SM::char_array[21] = "u";
$SM::char_array[22] = "v";
$SM::char_array[23] = "w";
$SM::char_array[24] = "x";
$SM::char_array[25] = "y";
$SM::char_array[26] = "z";
$SM::char_array[27] = "A";
$SM::char_array[28] = "B";
$SM::char_array[29] = "C";
$SM::char_array[30] = "D";
$SM::char_array[31] = "E";
$SM::char_array[32] = "F";
$SM::char_array[33] = "G";
$SM::char_array[34] = "H";
$SM::char_array[35] = "I";
$SM::char_array[36] = "J";
$SM::char_array[37] = "K";
$SM::char_array[38] = "L";
$SM::char_array[39] = "M";
$SM::char_array[40] = "N";
$SM::char_array[41] = "O";
$SM::char_array[42] = "P";
$SM::char_array[43] = "Q";
$SM::char_array[44] = "R";
$SM::char_array[45] = "S";
$SM::char_array[46] = "T";
$SM::char_array[47] = "U";
$SM::char_array[48] = "V";
$SM::char_array[49] = "W";
$SM::char_array[50] = "X";
$SM::char_array[51] = "Y";
$SM::char_array[52] = "Z";
$SM::char_array[53] = "0";
$SM::char_array[54] = "1";
$SM::char_array[55] = "2";
$SM::char_array[56] = "3";
$SM::char_array[57] = "4";
$SM::char_array[58] = "5";
$SM::char_array[59] = "6";
$SM::char_array[60] = "7";
$SM::char_array[61] = "8";
$SM::char_array[62] = "9";
$SM::char_array[63] = "!";
$SM::char_array[64] = "@";
$SM::char_array[65] = "#";
$SM::char_array[66] = "$";
$SM::char_array[67] = "%";
$SM::char_array[68] = "^";
$SM::char_array[69] = "&";
$SM::char_array[70] = "*";
$SM::char_array[71] = "(";
$SM::char_array[72] = ")";
$SM::char_array[73] = "_";
$SM::char_array[74] = "-";
$SM::char_array[75] = "+";
$SM::char_array[76] = "=";
$SM::char_array[77] = "[";
$SM::char_array[78] = "]";
$SM::char_array[79] = "{";
$SM::char_array[80] = "}";
$SM::char_array[81] = "\\";
$SM::char_array[82] = "|";
$SM::char_array[83] = "?";
$SM::char_array[84] = "/";
$SM::char_array[85] = ".";
$SM::char_array[86] = ",";
$SM::char_array[87] = ">";
$SM::char_array[88] = "<";
$SM::char_array[89] = " ";
$SM::char_array[90] = ":";
$SM::char_array[91] = ";";
$SM::char_array[92] = "\"";
$SM::char_array[93] = "'";
$SM::char_array[94] = "`";
$SM::char_array[95] = "~";
$SM::char_array[96] = "¦";

//---------------------------------------------------------------------
// WARNING: If adding a character: make sure this line is LAST.
// This line obtains character count and does NOT need to be modified.
//---------------------------------------------------------------------

$SM::charCount = eval("%count=1;while($SM::char_array[%count] != \"\")%count++;return %count;");

// EoF
// - Drake 2/22/2011
//---------------------------------------------------------------------