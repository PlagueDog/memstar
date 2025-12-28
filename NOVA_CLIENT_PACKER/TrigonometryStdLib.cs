// Trigonometry Standard Library (version 2.1)
// by Com. Sentinal [M.I.B.]
//--------------------------------------------------------------------------
// This file contains the following new commands:
//
//  sin(%angle);
//  cos(%angle);
//  tan(%angle);
//  sec(%angle);
//  csc(%angle);
//  cot(%angle);
//  round(%number);
//  getAngle(%x1, %y1, %x2, %y2);
//  getVector(%deltaX, %deltaY);
//  getHypotenuse(%deltaX, %deltaY, [%deltaZ]);
//  getOtherSide(%hypotenuse, %side);
//  square(%number);
//  cube(%number);
//  raiseToPower(%number, %exponent);
//  isInteger(%number);
//  getAverage(%num1, %num2, [%num3, %num4, %num5, %num6, %num7, %num8, %num9, %num10]);
//  abs(%number);
//  degToRad(%degrees);
//  radToDeg(%radians);
//  simplifyAngle(%angle, [%axis]);
//  adjustAngle(%angle);
//  pi();
//
//--------------------------------------------------------------------------


function square(%number)
{
   %answer = (%number * %number);
   return %answer;
}

function cube(%number)
{
   %answer = (%number * %number * %number);
   return %answer;
}

function isInteger(%number)
{
   %excess = (%number - floor(%number));
   if(%excess == 0)
   {
      return true;
   }
   else
   {
      return false;
   }
}

function raiseToPower(%number, %exponent)
{
   if(isInteger(%exponent) == false)
   {
      echo("raiseToPower() - Exponents must be integers!");
      %exponent = round(%exponent);
      echo("Exponent rounded to " @ %exponent);
   }
   %answer = %number;
   if(%exponent == 0)
   {
      %answer = 1;
   }
   else if(%exponent == 1)
   {
      %answer = %number;
   }
   else if(%exponent < 0)
   {
      if(%exponent == -1)
      {
         %answer = (1 / %number);
      }
      else if(%exponent <= -2)
      {
         %count = -2;
         while(%count >= %exponent)
         {
            %count--;
            %answer = (%answer * %number); 
         }
         %answer = (1 / %answer);
      }
   }
   else if(%exponent >= 2)
   {
      %count = 2;
      while(%count <= %exponent)
      {
         %count++;
         %answer = (%answer * %number); 
      }
   }
   return %answer;
}

function getHypotenuse(%deltaX, %deltaY, %deltaZ)
{
   if(%deltaZ == "")
   {
      %answer = sqrt((%deltaX * %deltaX) + (%deltaY * %deltaY));
   }
   else if(%deltaZ != "")
   {
      %hyp = sqrt((%deltaX * %deltaX) + (%deltaY * %deltaY));
      %answer = sqrt((%hyp * %hyp) + (%deltaZ * %deltaZ));
   }
   return %answer;
}

function getOtherSide(%hypotenuse, %side)
{
   %answer = sqrt((%hypotenuse * %hypotenuse) - (%side * %side));
   return %answer; 
}

function getAverage(%num1, %num2, %num3, %num4, %num5, %num6, %num7, %num8, %num9, %num10)
{
   if(%num1 == "")
   {
      echo("getAverage() - You must input at least 2 numbers.");
      %answer = "";
   }
   else if((%num1 != "")&&(%num2 == ""))
   {
      echo("getAverage() - You must input at least 2 numbers.");
      %answer = %num1;
   }   
   else if((%num1 != "")&&(%num2 != "")&&(%num3 == ""))
   {
      %answer = (%num1 + %num2) / 2;
   }   
   else if((%num1 != "")&&(%num2 != "")&&(%num3 != "")&&(%num4 == ""))
   {
      %answer = (%num1 + %num2 + %num3) / 3;
   }   
   else if((%num1 != "")&&(%num2 != "")&&(%num3 != "")&&(%num4 != "")&&(%num5 == ""))
   {
      %answer = (%num1 + %num2 + %num3 + %num4) / 4;
   }   
   else if((%num1 != "")&&(%num2 != "")&&(%num3 != "")&&(%num4 != "")&&(%num5 != "")&&(%num6 == ""))
   {
      %answer = (%num1 + %num2 + %num3 + %num4 + %num5) / 5;
   }   
   else if((%num1 != "")&&(%num2 != "")&&(%num3 != "")&&(%num4 != "")&&(%num5 != "")&&(%num6 != "")&&(%num7 == ""))
   {
      %answer = (%num1 + %num2 + %num3 + %num4 + %num5 + %num6) / 6;
   }   
   else if((%num1 != "")&&(%num2 != "")&&(%num3 != "")&&(%num4 != "")&&(%num5 != "")&&(%num6 != "")&&(%num7 != "")&&(%num8 == ""))
   {
      %answer = (%num1 + %num2 + %num3 + %num4 + %num5 + %num6 + %num7) / 7;
   }   
   else if((%num1 != "")&&(%num2 != "")&&(%num3 != "")&&(%num4 != "")&&(%num5 != "")&&(%num6 != "")&&(%num7 != "")&&(%num8 != "")&&(%num9 == ""))
   {
      %answer = (%num1 + %num2 + %num3 + %num4 + %num5 + %num6 + %num7 + %num8) / 8;
   }   
   else if((%num1 != "")&&(%num2 != "")&&(%num3 != "")&&(%num4 != "")&&(%num5 != "")&&(%num6 != "")&&(%num7 != "")&&(%num8 != "")&&(%num9 != "")&&(%num10 == ""))
   {
      %answer = (%num1 + %num2 + %num3 + %num4 + %num5 + %num6 + %num7 + %num8 + %num9) / 9;
   }      
   else if((%num1 != "")&&(%num2 != "")&&(%num3 != "")&&(%num4 != "")&&(%num5 != "")&&(%num6 != "")&&(%num7 != "")&&(%num8 != "")&&(%num9 != "")&&(%num10 != ""))
   {
      %answer = (%num1 + %num2 + %num3 + %num4 + %num5 + %num6 + %num7 + %num8 + %num9 + %num10) / 10;
   }
   return %answer;
}

function abs(%number)
{
   if(%number >= 0)
   {
      %answer = %number;
   }
   else if(%number < 0)
   {
      %answer = -%number;
   }
}

function simplifyAngle(%angle, %axis)
{
   if(%axis == "")
   {
      %axis = x;
   }
   if(%axis == x)
   {
      %angle = adjustAngle(%angle);
      if((%angle > 0)&&(%angle < 90))
      {
         %answer = %angle;
      }
      else if((%angle > 90)&&(%angle < 180))
      {
         %answer = (180 - %angle);
      }
      else if((%angle > 180)&&(%angle < 270))
      {
         %answer = (%angle - 180);
      }
      else if((%angle > 270)&&(%angle < 360))
      {
         %answer = (360 - %angle);
      }
      else if((%angle == 90) || (%angle == 270))
      {
         %answer = 90;
      }   
      else if((%angle == 0) || (%angle == 180) || (%angle == 360))
      {
         %answer = 0;
      }
   }
   else if(%axis == y)
   {
      %angle = adjustAngle(%angle);
      if((%angle > 0)&&(%angle < 90))
      {
         %answer = (90 - %angle);
      }
      else if((%angle > 90)&&(%angle < 180))
      {
         %answer = (%angle - 90);
      }
      else if((%angle > 180)&&(%angle < 270))
      {
         %answer = (270 - %angle);
      }
      else if((%angle > 270)&&(%angle < 360))
      {
         %answer = (%angle - 270);
      }
      else if((%angle == 90) || (%angle == 270))
      {
         %answer = 0;
      }   
      else if((%angle == 0) || (%angle == 180) || (%angle == 360))
      {
         %answer = 90;
      }
   }
   else
   {
      echo("simplifyAngle() - axis must be either x or y");
      echo("If axis is given no value, it defaults to x.");
      %answer = "";
   }
   return %answer;
}

function DegToRad(%degrees)
{
   %radians = (%degrees * 3.14159) / 180;
   return %radians;
}

function RadToDeg(%radians)
{
   %degrees = (%radians * 180) / 3.14159;
   return %degrees;
}

function pi()
{
   %pi = 3.14159;
   return %pi;
}

function round(%number)
{
   %roundNum = %number;
   if(%number > 0)
   {
      %excess = (%number - floor(%number));
      if(%excess >= 0.5)
      {
         %roundNum = (floor(%number) + 1);
      }
      else
      {
         %roundNum = floor(%number);
      }
   }
   if(%number < 0)
   {
      %excess = (floor(%number) - %number);
      if(%excess <= 0.5)
      {
         %roundNum = floor(%number);
      }
      else
      {
         %roundNum = (floor(%number) - 1);
      }
   }
   return %roundNum;
}

function adjustAngle(%angle)
{
   if(%angle >= 360)
   {
      while(%angle >= 360)
      {
         %angle = (%angle - 360);
      }
   }
   else if(%angle < 0)
   {
      while(%angle < 0)
      {
         %angle = (%angle + 360);
      }
   }
   return %angle;
}

function getAngle(%x1, %y1, %x2, %y2)
{
   //Angle is defined from point A (x1, y1) to point B (x2, y2)
   //Point A (x1, y1) is the base point (center of the protractor)
   //Be sure to enter the coordinates for the base point first (x1, y1)

   %deltaX = %x2 - %x1;
   %deltaY = %y2 - %y1;
   %angle = getVector(%deltaX, %deltaY);
   return %angle;
}

function csc(%angle)
{
   %answer = (1 / sin(%angle));
   return %answer;
}

function sec(%angle)
{
   %answer = (1 / cos(%angle));
   return %answer;
}

function cot(%angle)
{
   %answer = (cos(%angle) / sin(%angle));
   return %answer;
}

function tan(%angle)
{
   %answer = (sin(%angle) / cos(%angle));
   return %answer;
}

function sin(%angle)
{
   %angle = round(adjustAngle(%angle));
   return $sin[%angle];
}

function cos(%angle)
{
   %angle = round(adjustAngle(%angle));
   return $cos[%angle];
} 

$sin0 = 0;
$sin1 = 0.017452;
$sin2 = 0.034899;
$sin3 = 0.052335;
$sin4 = 0.069756;
$sin5 = 0.087156;
$sin6 = 0.104528;
$sin7 = 0.121869;
$sin8 = 0.139173;
$sin9 = 0.156434;
$sin10 = 0.173648;
$sin11 = 0.190809;
$sin12 = 0.207912;
$sin13 = 0.224951;
$sin14 = 0.241922;
$sin15 = 0.258819;
$sin16 = 0.275637;
$sin17 = 0.292372;
$sin18 = 0.309017;
$sin19 = 0.325568;
$sin20 = 0.342020;
$sin21 = 0.358368;
$sin22 = 0.374607;
$sin23 = 0.390731;
$sin24 = 0.406737;
$sin25 = 0.422618;
$sin26 = 0.438371;
$sin27 = 0.453990;
$sin28 = 0.469472;
$sin29 = 0.484810;
$sin30 = 0.5;
$sin31 = 0.515038;
$sin32 = 0.529919;
$sin33 = 0.544639;
$sin34 = 0.559193;
$sin35 = 0.573576;
$sin36 = 0.587785;
$sin37 = 0.601815;
$sin38 = 0.615661;
$sin39 = 0.629320;
$sin40 = 0.642788;
$sin41 = 0.656059;
$sin42 = 0.669131;
$sin43 = 0.681998;
$sin44 = 0.694658;
$sin45 = 0.707107;
$sin46 = 0.719340;
$sin47 = 0.731354;
$sin48 = 0.743145;
$sin49 = 0.754710;
$sin50 = 0.766044;
$sin51 = 0.777146;
$sin52 = 0.788011;
$sin53 = 0.798636;
$sin54 = 0.809017;
$sin55 = 0.819152;
$sin56 = 0.829038;
$sin57 = 0.838671;
$sin58 = 0.848048;
$sin59 = 0.857167;
$sin60 = 0.866025;
$sin61 = 0.874620;
$sin62 = 0.882948;
$sin63 = 0.891007;
$sin64 = 0.898794;
$sin65 = 0.906308;
$sin66 = 0.913545;
$sin67 = 0.920505;
$sin68 = 0.927184;
$sin69 = 0.933580;
$sin70 = 0.939693;
$sin71 = 0.945519;
$sin72 = 0.951057;
$sin73 = 0.956305;
$sin74 = 0.961262;
$sin75 = 0.965926;
$sin76 = 0.970296;
$sin77 = 0.974370;
$sin78 = 0.978148;
$sin79 = 0.981627;
$sin80 = 0.984808;
$sin81 = 0.987688;
$sin82 = 0.990268;
$sin83 = 0.992546;
$sin84 = 0.994522;
$sin85 = 0.996195;
$sin86 = 0.997564;
$sin87 = 0.998630;
$sin88 = 0.999391;
$sin89 = 0.999848;
$sin90 = 1;
$sin91 = 0.999848;
$sin92 = 0.999391;
$sin93 = 0.998630;
$sin94 = 0.997564;
$sin95 = 0.996195;
$sin96 = 0.994522;
$sin97 = 0.992546;
$sin98 = 0.990268;
$sin99 = 0.987688;
$sin100 = 0.984808;
$sin101 = 0.981627;
$sin102 = 0.978148;
$sin103 = 0.974370;
$sin104 = 0.970296;
$sin105 = 0.965926;
$sin106 = 0.961262;
$sin107 = 0.956305;
$sin108 = 0.951057;
$sin109 = 0.945519;
$sin110 = 0.939693;
$sin111 = 0.933580;
$sin112 = 0.927184;
$sin113 = 0.920505;
$sin114 = 0.913545;
$sin115 = 0.906308;
$sin116 = 0.898794;
$sin117 = 0.891007;
$sin118 = 0.882948;
$sin119 = 0.874620;
$sin120 = 0.866025;
$sin121 = 0.857167;
$sin122 = 0.848048;
$sin123 = 0.838671;
$sin124 = 0.829038;
$sin125 = 0.819152;
$sin126 = 0.809017;
$sin127 = 0.798636;
$sin128 = 0.788011;
$sin129 = 0.777146;
$sin130 = 0.766044;
$sin131 = 0.754710;
$sin132 = 0.743145;
$sin133 = 0.731354;
$sin134 = 0.719340;
$sin135 = 0.707107;
$sin136 = 0.694658;
$sin137 = 0.681998;
$sin138 = 0.669131;
$sin139 = 0.656059;
$sin140 = 0.642788;
$sin141 = 0.629320;
$sin142 = 0.615661;
$sin143 = 0.601815;
$sin144 = 0.587785;
$sin145 = 0.573576;
$sin146 = 0.559193;
$sin147 = 0.544639;
$sin148 = 0.529919;
$sin149 = 0.515038;
$sin150 = 0.5;
$sin151 = 0.484810;
$sin152 = 0.469472;
$sin153 = 0.453990;
$sin154 = 0.438371;
$sin155 = 0.422618;
$sin156 = 0.406737;
$sin157 = 0.390731;
$sin158 = 0.374607;
$sin159 = 0.358368;
$sin160 = 0.342020;
$sin161 = 0.325568;
$sin162 = 0.309017;
$sin163 = 0.292372;
$sin164 = 0.275637;
$sin165 = 0.258819;
$sin166 = 0.241922;
$sin167 = 0.224951;
$sin168 = 0.207912;
$sin169 = 0.190809;
$sin170 = 0.173648;
$sin171 = 0.156434;
$sin172 = 0.139173;
$sin173 = 0.121869;
$sin174 = 0.104528;
$sin175 = 0.087156;
$sin176 = 0.069756;
$sin177 = 0.052335;
$sin178 = 0.034899;
$sin179 = 0.017452;
$sin180 = 0;
$sin181 = -0.017452;
$sin182 = -0.034899;
$sin183 = -0.052335;
$sin184 = -0.069756;
$sin185 = -0.087156;
$sin186 = -0.104528;
$sin187 = -0.121869;
$sin188 = -0.139173;
$sin189 = -0.156434;
$sin190 = -0.173648;
$sin191 = -0.190809;
$sin192 = -0.207912;
$sin193 = -0.224951;
$sin194 = -0.241922;
$sin195 = -0.258819;
$sin196 = -0.275637;
$sin197 = -0.292372;
$sin198 = -0.309017;
$sin199 = -0.325568;
$sin200 = -0.342020;
$sin201 = -0.358368;
$sin202 = -0.374607;
$sin203 = -0.390731;
$sin204 = -0.406737;
$sin205 = -0.422618;
$sin206 = -0.438371;
$sin207 = -0.453990;
$sin208 = -0.469472;
$sin209 = -0.484810;
$sin210 = -0.5;
$sin211 = -0.515038;
$sin212 = -0.529919;
$sin213 = -0.544639;
$sin214 = -0.559193;
$sin215 = -0.573576;
$sin216 = -0.587785;
$sin217 = -0.601815;
$sin218 = -0.615661;
$sin219 = -0.629320;
$sin220 = -0.642788;
$sin221 = -0.656059;
$sin222 = -0.669131;
$sin223 = -0.681998;
$sin224 = -0.694658;
$sin225 = -0.707107;
$sin226 = -0.719340;
$sin227 = -0.731354;
$sin228 = -0.743145;
$sin229 = -0.754710;
$sin230 = -0.766044;
$sin231 = -0.777146;
$sin232 = -0.788011;
$sin233 = -0.798636;
$sin234 = -0.809017;
$sin235 = -0.819152;
$sin236 = -0.829038;
$sin237 = -0.838671;
$sin238 = -0.848048;
$sin239 = -0.857167;
$sin240 = -0.866025;
$sin241 = -0.874620;
$sin242 = -0.882948;
$sin243 = -0.891007;
$sin244 = -0.898794;
$sin245 = -0.906308;
$sin246 = -0.913545;
$sin247 = -0.920505;
$sin248 = -0.927184;
$sin249 = -0.933580;
$sin250 = -0.939693;
$sin251 = -0.945519;
$sin252 = -0.951057;
$sin253 = -0.956305;
$sin254 = -0.961262;
$sin255 = -0.965926;
$sin256 = -0.970296;
$sin257 = -0.974370;
$sin258 = -0.978148;
$sin259 = -0.981627;
$sin260 = -0.984808;
$sin261 = -0.987688;
$sin262 = -0.990268;
$sin263 = -0.992546;
$sin264 = -0.994522;
$sin265 = -0.996195;
$sin266 = -0.997564;
$sin267 = -0.998630;
$sin268 = -0.999391;
$sin269 = -0.999848;
$sin270 = -1;
$sin271 = -0.999848;
$sin272 = -0.999391;
$sin273 = -0.998630;
$sin274 = -0.997564;
$sin275 = -0.996195;
$sin276 = -0.994522;
$sin277 = -0.992546;
$sin278 = -0.990268;
$sin279 = -0.987688;
$sin280 = -0.984808;
$sin281 = -0.981627;
$sin282 = -0.978148;
$sin283 = -0.974370;
$sin284 = -0.970296;
$sin285 = -0.965926;
$sin286 = -0.961262;
$sin287 = -0.956305;
$sin288 = -0.951057;
$sin289 = -0.945519;
$sin290 = -0.939693;
$sin291 = -0.933580;
$sin292 = -0.927184;
$sin293 = -0.920505;
$sin294 = -0.913545;
$sin295 = -0.906308;
$sin296 = -0.898794;
$sin297 = -0.891007;
$sin298 = -0.882948;
$sin299 = -0.874620;
$sin300 = -0.866025;
$sin301 = -0.857167;
$sin302 = -0.848048;
$sin303 = -0.838671;
$sin304 = -0.829038;
$sin305 = -0.819152;
$sin306 = -0.809017;
$sin307 = -0.798636;
$sin308 = -0.788011;
$sin309 = -0.777146;
$sin310 = -0.766044;
$sin311 = -0.754710;
$sin312 = -0.743145;
$sin313 = -0.731354;
$sin314 = -0.719340;
$sin315 = -0.707107;
$sin316 = -0.694658;
$sin317 = -0.681998;
$sin318 = -0.669131;
$sin319 = -0.656059;
$sin320 = -0.642788;
$sin321 = -0.629320;
$sin322 = -0.615661;
$sin323 = -0.601815;
$sin324 = -0.587785;
$sin325 = -0.573576;
$sin326 = -0.559193;
$sin327 = -0.544639;
$sin328 = -0.529919;
$sin329 = -0.515038;
$sin330 = -0.5;
$sin331 = -0.484810;
$sin332 = -0.469472;
$sin333 = -0.453990;
$sin334 = -0.438371;
$sin335 = -0.422618;
$sin336 = -0.406737;
$sin337 = -0.390731;
$sin338 = -0.374607;
$sin339 = -0.358368;
$sin340 = -0.342020;
$sin341 = -0.325568;
$sin342 = -0.309017;
$sin343 = -0.292372;
$sin344 = -0.275637;
$sin345 = -0.258819;
$sin346 = -0.241922;
$sin347 = -0.224951;
$sin348 = -0.207912;
$sin349 = -0.190809;
$sin350 = -0.173648;
$sin351 = -0.156434;
$sin352 = -0.139173;
$sin353 = -0.121869;
$sin354 = -0.104528;
$sin355 = -0.087156;
$sin356 = -0.069756;
$sin357 = -0.052335;
$sin358 = -0.034899;
$sin359 = -0.017452;
$sin360 = 0;

$cos0 = 1;
$cos1 = 0.999848;
$cos2 = 0.999391;
$cos3 = 0.998630;
$cos4 = 0.997564;
$cos5 = 0.996195;
$cos6 = 0.994522;
$cos7 = 0.992546;
$cos8 = 0.990268;
$cos9 = 0.987688;
$cos10 = 0.984808;
$cos11 = 0.981627;
$cos12 = 0.978148;
$cos13 = 0.974370;
$cos14 = 0.970296;
$cos15 = 0.965926;
$cos16 = 0.961262;
$cos17 = 0.956305;
$cos18 = 0.951057;
$cos19 = 0.945519;
$cos20 = 0.939693;
$cos21 = 0.933580;
$cos22 = 0.927184;
$cos23 = 0.920505;
$cos24 = 0.913545;
$cos25 = 0.906308;
$cos26 = 0.898794;
$cos27 = 0.891007;
$cos28 = 0.882948;
$cos29 = 0.874620;
$cos30 = 0.866025;
$cos31 = 0.857167;
$cos32 = 0.848048;
$cos33 = 0.838671;
$cos34 = 0.829038;
$cos35 = 0.819152;
$cos36 = 0.809017;
$cos37 = 0.798636;
$cos38 = 0.788011;
$cos39 = 0.777146;
$cos40 = 0.766044;
$cos41 = 0.754710;
$cos42 = 0.743145;
$cos43 = 0.731354;
$cos44 = 0.719340;
$cos45 = 0.707107;
$cos46 = 0.694658;
$cos47 = 0.681998;
$cos48 = 0.669131;
$cos49 = 0.656059;
$cos50 = 0.642788;
$cos51 = 0.629320;
$cos52 = 0.615661;
$cos53 = 0.601815;
$cos54 = 0.587785;
$cos55 = 0.573576;
$cos56 = 0.559193;
$cos57 = 0.544639;
$cos58 = 0.529919;
$cos59 = 0.515038;
$cos60 = 0.5;
$cos61 = 0.484810;
$cos62 = 0.469472;
$cos63 = 0.453990;
$cos64 = 0.438371;
$cos65 = 0.422618;
$cos66 = 0.406737;
$cos67 = 0.390731;
$cos68 = 0.374607;
$cos69 = 0.358368;
$cos70 = 0.342020;
$cos71 = 0.325568;
$cos72 = 0.309017;
$cos73 = 0.292372;
$cos74 = 0.275637;
$cos75 = 0.258819;
$cos76 = 0.241922;
$cos77 = 0.224951;
$cos78 = 0.207912;
$cos79 = 0.190809;
$cos80 = 0.173648;
$cos81 = 0.156434;
$cos82 = 0.139173;
$cos83 = 0.121869;
$cos84 = 0.104528;
$cos85 = 0.087156;
$cos86 = 0.069756;
$cos87 = 0.052336;
$cos88 = 0.034899;
$cos89 = 0.017452;
$cos90 = 0;
$cos91 = -0.017452;
$cos92 = -0.034899;
$cos93 = -0.052336;
$cos94 = -0.069756;
$cos95 = -0.087156;
$cos96 = -0.104528;
$cos97 = -0.121869;
$cos98 = -0.139173;
$cos99 = -0.156434;
$cos100 = -0.173648;
$cos101 = -0.190809;
$cos102 = -0.207912;
$cos103 = -0.224951;
$cos104 = -0.241922;
$cos105 = -0.258819;
$cos106 = -0.275637;
$cos107 = -0.292372;
$cos108 = -0.309017;
$cos109 = -0.325568;
$cos110 = -0.342020;
$cos111 = -0.358368;
$cos112 = -0.374607;
$cos113 = -0.390731;
$cos114 = -0.406737;
$cos115 = -0.422618;
$cos116 = -0.438371;
$cos117 = -0.453990;
$cos118 = -0.469472;
$cos119 = -0.484810;
$cos120 = -0.5;
$cos121 = -0.515038;
$cos122 = -0.529919;
$cos123 = -0.544639;
$cos124 = -0.559193;
$cos125 = -0.573576;
$cos126 = -0.587785;
$cos127 = -0.601815;
$cos128 = -0.615661;
$cos129 = -0.629320;
$cos130 = -0.642788;
$cos131 = -0.656059;
$cos132 = -0.669131;
$cos133 = -0.681998;
$cos134 = -0.694658;
$cos135 = -0.707107;
$cos136 = -0.719340;
$cos137 = -0.731354;
$cos138 = -0.743145;
$cos139 = -0.754710;
$cos140 = -0.766044;
$cos141 = -0.777146;
$cos142 = -0.788011;
$cos143 = -0.798636;
$cos144 = -0.809017;
$cos145 = -0.819152;
$cos146 = -0.829038;
$cos147 = -0.838671;
$cos148 = -0.848048;
$cos149 = -0.857167;
$cos150 = -0.866025;
$cos151 = -0.874620;
$cos152 = -0.882948;
$cos153 = -0.891007;
$cos154 = -0.898794;
$cos155 = -0.906308;
$cos156 = -0.913545;
$cos157 = -0.920505;
$cos158 = -0.927184;
$cos159 = -0.933580;
$cos160 = -0.939693;
$cos161 = -0.945519;
$cos162 = -0.951057;
$cos163 = -0.956305;
$cos164 = -0.961262;
$cos165 = -0.965926;
$cos166 = -0.970296;
$cos167 = -0.974370;
$cos168 = -0.978148;
$cos169 = -0.981627;
$cos170 = -0.984808;
$cos171 = -0.987688;
$cos172 = -0.990268;
$cos173 = -0.992546;
$cos174 = -0.994522;
$cos175 = -0.996195;
$cos176 = -0.997564;
$cos177 = -0.998630;
$cos178 = -0.999391;
$cos179 = -0.999848;
$cos180 = -1;
$cos181 = -0.999848;
$cos182 = -0.999391;
$cos183 = -0.998630;
$cos184 = -0.997564;
$cos185 = -0.996195;
$cos186 = -0.994522;
$cos187 = -0.992546;
$cos188 = -0.990268;
$cos189 = -0.987688;
$cos190 = -0.984808;
$cos191 = -0.981627;
$cos192 = -0.978148;
$cos193 = -0.974370;
$cos194 = -0.970296;
$cos195 = -0.965926;
$cos196 = -0.961262;
$cos197 = -0.956305;
$cos198 = -0.951057;
$cos199 = -0.945519;
$cos200 = -0.939693;
$cos201 = -0.933580;
$cos202 = -0.927184;
$cos203 = -0.920505;
$cos204 = -0.913545;
$cos205 = -0.906308;
$cos206 = -0.898794;
$cos207 = -0.891007;
$cos208 = -0.882948;
$cos209 = -0.874620;
$cos210 = -0.866025;
$cos211 = -0.857167;
$cos212 = -0.848048;
$cos213 = -0.838671;
$cos214 = -0.829038;
$cos215 = -0.819152;
$cos216 = -0.809017;
$cos217 = -0.798636;
$cos218 = -0.788011;
$cos219 = -0.777146;
$cos220 = -0.766044;
$cos221 = -0.754710;
$cos222 = -0.743145;
$cos223 = -0.731354;
$cos224 = -0.719340;
$cos225 = -0.707107;
$cos226 = -0.694658;
$cos227 = -0.681998;
$cos228 = -0.669131;
$cos229 = -0.656059;
$cos230 = -0.642788;
$cos231 = -0.629320;
$cos232 = -0.615661;
$cos233 = -0.601815;
$cos234 = -0.587785;
$cos235 = -0.573576;
$cos236 = -0.559193;
$cos237 = -0.544639;
$cos238 = -0.529919;
$cos239 = -0.515038;
$cos240 = -0.5;
$cos241 = -0.484810;
$cos242 = -0.469472;
$cos243 = -0.453990;
$cos244 = -0.438371;
$cos245 = -0.422618;
$cos246 = -0.406737;
$cos247 = -0.390731;
$cos248 = -0.374607;
$cos249 = -0.358368;
$cos250 = -0.342020;
$cos251 = -0.325568;
$cos252 = -0.309017;
$cos253 = -0.292372;
$cos254 = -0.275637;
$cos255 = -0.258819;
$cos256 = -0.241922;
$cos257 = -0.224951;
$cos258 = -0.207912;
$cos259 = -0.190809;
$cos260 = -0.173648;
$cos261 = -0.156434;
$cos262 = -0.139173;
$cos263 = -0.121869;
$cos264 = -0.104528;
$cos265 = -0.087156;
$cos266 = -0.069756;
$cos267 = -0.052336;
$cos268 = -0.034899;
$cos269 = -0.017452;
$cos270 = 0;
$cos271 = 0.017452;
$cos272 = 0.034899;
$cos273 = 0.052336;
$cos274 = 0.069756;
$cos275 = 0.087156;
$cos276 = 0.104528;
$cos277 = 0.121869;
$cos278 = 0.139173;
$cos279 = 0.156434;
$cos280 = 0.173648;
$cos281 = 0.190809;
$cos282 = 0.207912;
$cos283 = 0.224951;
$cos284 = 0.241922;
$cos285 = 0.258819;
$cos286 = 0.275637;
$cos287 = 0.292372;
$cos288 = 0.309017;
$cos289 = 0.325568;
$cos290 = 0.342020;
$cos291 = 0.358368;
$cos292 = 0.374607;
$cos293 = 0.390731;
$cos294 = 0.406737;
$cos295 = 0.422618;
$cos296 = 0.438371;
$cos297 = 0.453990;
$cos298 = 0.469472;
$cos299 = 0.484810;
$cos300 = 0.5;
$cos301 = 0.515038;
$cos302 = 0.529919;
$cos303 = 0.544639;
$cos304 = 0.559193;
$cos305 = 0.573576;
$cos306 = 0.587785;
$cos307 = 0.601815;
$cos308 = 0.615661;
$cos309 = 0.629320;
$cos310 = 0.642788;
$cos311 = 0.656059;
$cos312 = 0.669131;
$cos313 = 0.681998;
$cos314 = 0.694658;
$cos315 = 0.707107;
$cos316 = 0.719340;
$cos317 = 0.731354;
$cos318 = 0.743145;
$cos319 = 0.754710;
$cos320 = 0.766044;
$cos321 = 0.777146;
$cos322 = 0.788011;
$cos323 = 0.798636;
$cos324 = 0.809017;
$cos325 = 0.819152;
$cos326 = 0.829038;
$cos327 = 0.838671;
$cos328 = 0.848048;
$cos329 = 0.857167;
$cos330 = 0.866025;
$cos331 = 0.874620;
$cos332 = 0.882948;
$cos333 = 0.891007;
$cos334 = 0.898794;
$cos335 = 0.906308;
$cos336 = 0.913545;
$cos337 = 0.920505;
$cos338 = 0.927184;
$cos339 = 0.933580;
$cos340 = 0.939693;
$cos341 = 0.945519;
$cos342 = 0.951057;
$cos343 = 0.956305;
$cos344 = 0.961262;
$cos345 = 0.965926;
$cos346 = 0.970296;
$cos347 = 0.974370;
$cos348 = 0.978148;
$cos349 = 0.981627;
$cos350 = 0.984808;
$cos351 = 0.987688;
$cos352 = 0.990268;
$cos353 = 0.992546;
$cos354 = 0.994522;
$cos355 = 0.996195;
$cos356 = 0.997564;
$cos357 = 0.998630;
$cos358 = 0.999391;
$cos359 = 0.999848;
$cos360 = 1;

function getVector(%deltaX, %deltaY)
{
   %tan = (%deltaY / %deltaX);   
   if((%tan >= -0.008728)&&(%tan < 0.008728))
   {
      if(%deltaX > 0)
      {
         %angle = 0;
      }
      else if(%deltaX < 0)
      {
         %angle = 180;
      }
      return %angle;
   }
   else if((%tan >= 0.008728)&&(%tan < 0.026188))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 1;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 181;
      }
      return %angle;
   }
   else if((%tan >= 0.026188)&&(%tan < 0.043664))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 2;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 182;
      }
      return %angle;
   }
   else if((%tan >= 0.043664)&&(%tan < 0.061167))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 3;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 183;
      }
      return %angle;
   }
   else if((%tan >= 0.061167)&&(%tan < 0.078708))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 4;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 184;
      }
      return %angle;
   }
   else if((%tan >= 0.078708)&&(%tan < 0.096296))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 5;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 185;
      }
      return %angle;
   }
   else if((%tan >= 0.096296)&&(%tan < 0.113944))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 6;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 186;
      }
      return %angle;
   }
   else if((%tan >= 0.113944)&&(%tan < 0.131663))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 7;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 187;
      }
      return %angle;
   }
   else if((%tan >= 0.131663)&&(%tan < 0.149463))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 8;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 188;
      }
      return %angle;
   }
   else if((%tan >= 0.149463)&&(%tan < 0.167356))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 9;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 189;
      }
      return %angle;
   }
   else if((%tan >= 0.167356)&&(%tan < 0.185354))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 10;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 190;
      }
      return %angle;
   }
   else if((%tan >= 0.185354)&&(%tan < 0.203468))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 11;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 191;
      }
      return %angle;
   }
   else if((%tan >= 0.203468)&&(%tan < 0.221712))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 12;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 192;
      }
      return %angle;
   }
   else if((%tan >= 0.221712)&&(%tan < 0.240098))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 13;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 193;
      }
      return %angle;
   }
   else if((%tan >= 0.240098)&&(%tan < 0.258639))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 14;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 194;
      }
      return %angle;
   }
   else if((%tan >= 0.258639)&&(%tan < 0.277347))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 15;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 195;
      }
      return %angle;
   }
   else if((%tan >= 0.277347)&&(%tan < 0.296238))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 16;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 196;
      }
      return %angle;
   }
   else if((%tan >= 0.296238)&&(%tan < 0.315325))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 17;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 197;
      }
      return %angle;
   }
   else if((%tan >= 0.315325)&&(%tan < 0.334624))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 18;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 198;
      }
      return %angle;
   }
   else if((%tan >= 0.334624)&&(%tan < 0.354149))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 19;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 199;
      }
      return %angle;
   }
   else if((%tan >= 0.354149)&&(%tan < 0.373917))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 20;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 200;
      }
      return %angle;
   }
   else
   {
      getVector2(%deltaX, %deltaY);
   }
}

function getVector2(%deltaX, %deltaY)
{
   %tan = (%deltaY / %deltaX);
   if((%tan >= 0.373917)&&(%tan < 0.393945))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 21;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 201;
      }
      return %angle;
   }
   else if((%tan >= 0.393945)&&(%tan < 0.414251))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 22;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 202;
      }
      return %angle;
   }
   else if((%tan >= 0.414251)&&(%tan < 0.434852))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 23;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 203;
      }
      return %angle;
   }
   else if((%tan >= 0.434852)&&(%tan < 0.455768))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 24;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 204;
      }
      return %angle;
   }
   else if((%tan >= 0.455768)&&(%tan < 0.477020))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 25;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 205;
      }
      return %angle;
   }
   else if((%tan >= 0.477020)&&(%tan < 0.498629))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 26;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 206;
      }
      return %angle;
   }
   else if((%tan >= 0.498629)&&(%tan < 0.520617))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 27;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 207;
      }
      return %angle;
   }
   else if((%tan >= 0.520617)&&(%tan < 0.543009))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 28;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 208;
      }
      return %angle;
   }
   else if((%tan >= 0.543009)&&(%tan < 0.565830))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 29;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 209;
      }
      return %angle;
   }
   else if((%tan >= 0.565830)&&(%tan < 0.589105))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 30;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 210;
      }
      return %angle;
   }
   else if((%tan >= 0.589105)&&(%tan < 0.612865))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 31;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 211;
      }
      return %angle;
   }
   else if((%tan >= 0.612865)&&(%tan < 0.637138))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 32;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 212;
      }
      return %angle;
   }
   else if((%tan >= 0.637138)&&(%tan < 0.661958))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 33;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 213;
      }
      return %angle;
   }
   else if((%tan >= 0.661958)&&(%tan < 0.687358))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 34;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 214;
      }
      return %angle;
   }
   else if((%tan >= 0.687358)&&(%tan < 0.713375))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 35;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 215;
      }
      return %angle;
   }
   else if((%tan >= 0.713375)&&(%tan < 0.740048))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 36;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 216;
      }
      return %angle;
   }
   else if((%tan >= 0.740048)&&(%tan < 0.767420))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 37;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 217;
      }
      return %angle;
   }
   else if((%tan >= 0.767420)&&(%tan < 0.795535))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 38;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 218;
      }
      return %angle;
   }
   else if((%tan >= 0.795535)&&(%tan < 0.824442))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 39;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 219;
      }
      return %angle;
   }
   else if((%tan >= 0.824442)&&(%tan < 0.854193))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 40;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 220;
      }
      return %angle;
   }
   else
   {
      getVector3(%deltaX, %deltaY);
   }
}

function getVector3(%deltaX, %deltaY)
{
   %tan = (%deltaY / %deltaX);
   if((%tan >= 0.854193)&&(%tan < 0.884845))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 41;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 221;
      }
      return %angle;
   }
   else if((%tan >= 0.884845)&&(%tan < 0.916460))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 42;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 222;
      }
      return %angle;
   }
   else if((%tan >= 0.916460)&&(%tan < 0.949102))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 43;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 223;
      }
      return %angle;
   }
   else if((%tan >= 0.949102)&&(%tan < 0.982844))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 44;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 224;
      }
      return %angle;
   }
   else if((%tan >= 0.982844)&&(%tan < 1.017765))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 45;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 225;
      }
      return %angle;
   }
   else if((%tan >= 1.017765)&&(%tan < 1.053950))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 46;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 226;
      }
      return %angle;
   }
   else if((%tan >= 1.053950)&&(%tan < 1.091491))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 47;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 227;
      }
      return %angle;
   }
   else if((%tan >= 1.091491)&&(%tan < 1.130490))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 48;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 228;
      }
      return %angle;
   }
   else if((%tan >= 1.130490)&&(%tan < 1.171061))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 49;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 229;
      }
      return %angle;
   }
   else if((%tan >= 1.171061)&&(%tan < 1.213325))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 50;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 230;
      }
      return %angle;
   }
   else if((%tan >= 1.213325)&&(%tan < 1.257419))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 51;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 231;
      }
      return %angle;
   }
   else if((%tan >= 1.257419)&&(%tan < 1.303493))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 52;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 232;
      }
      return %angle;
   }
   else if((%tan >= 1.303493)&&(%tan < 1.351713))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 53;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 233;
      }
      return %angle;
   }
   else if((%tan >= 1.351713)&&(%tan < 1.402265))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 54;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 234;
      }
      return %angle;
   }
   else if((%tan >= 1.402265)&&(%tan < 1.455354))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 55;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 235;
      }
      return %angle;
   }
   else if((%tan >= 1.455354)&&(%tan < 1.511213))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 56;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 236;
      }
      return %angle;
   }
   else if((%tan >= 1.511213)&&(%tan < 1.570100))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 57;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 237;
      }
      return %angle;
   }
   else if((%tan >= 1.570100)&&(%tan < 1.632307))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 58;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 238;
      }
      return %angle;
   }
   else if((%tan >= 1.632307)&&(%tan < 1.698165))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 59;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 239;
      }
      return %angle;
   }
   else if((%tan >= 1.698165)&&(%tan < 1.768049))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 60;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 240;
      }
      return %angle;
   }
   else
   {
      getVector4(%deltaX, %deltaY);
   }
}

function getVector4(%deltaX, %deltaY)
{
   %tan = (%deltaY / %deltaX);
   if((%tan >= 1.768049)&&(%tan < 1.842387))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 61;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 241;
      }
      return %angle;
   }
   else if((%tan >= 1.842387)&&(%tan < 1.921668))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 62;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 242;
      }
      return %angle;
   }
   else if((%tan >= 1.921668)&&(%tan < 2.006457))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 63;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 243;
      }
      return %angle;
   }
   else if((%tan >= 2.006457)&&(%tan < 2.097405))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 64;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 244;
      }
      return %angle;
   }
   else if((%tan >= 2.097405)&&(%tan < 2.195272))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 65;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 245;
      }
      return %angle;
   }
   else if((%tan >= 2.195272)&&(%tan < 2.300945))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 66;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 246;
      }
      return %angle;
   }
   else if((%tan >= 2.300945)&&(%tan < 2.415470))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 67;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 247;
      }
      return %angle;
   }
   else if((%tan >= 2.415470)&&(%tan < 2.540088))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 68;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 248;
      }
      return %angle;
   }
   else if((%tan >= 2.540088)&&(%tan < 2.676283))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 69;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 249;
      }
      return %angle;
   }
   else if((%tan >= 2.676283)&&(%tan < 2.825844))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 70;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 250;
      }
      return %angle;
   }
   else if((%tan >= 2.825844)&&(%tan < 2.990947))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 71;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 251;
      }
      return %angle;
   }
   else if((%tan >= 2.990947)&&(%tan < 3.174268))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 72;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 252;
      }
      return %angle;
   }
   else if((%tan >= 3.174268)&&(%tan < 3.379134))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 73;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 253;
      }
      return %angle;
   }
   else if((%tan >= 3.379134)&&(%tan < 3.609733))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 74;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 254;
      }
      return %angle;
   }
   else if((%tan >= 3.609733)&&(%tan < 3.871416))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 75;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 255;
      }
      return %angle;
   }
   else if((%tan >= 3.871416)&&(%tan < 4.171128))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 76;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 256;
      }
      return %angle;
   }
   else if((%tan >= 4.171128)&&(%tan < 4.518053))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 77;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 257;
      }
      return %angle;
   }
   else if((%tan >= 4.518053)&&(%tan < 4.924592))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 78;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 258;
      }
      return %angle;
   }
   else if((%tan >= 4.924592)&&(%tan < 5.407918))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 79;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 259;
      }
      return %angle;
   }
   else if((%tan >= 5.407918)&&(%tan < 5.992517))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 80;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 260;
      }
      return %angle;
   }
   else
   {
      getVector5(%deltaX, %deltaY);
   }
}

function getVector5(%deltaX, %deltaY)
{
   %tan = (%deltaY / %deltaX);
   if((%tan >= 5.992517)&&(%tan < 6.714561))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 81;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 261;
      }
      return %angle;
   }
   else if((%tan >= 6.714561)&&(%tan < 7.629858))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 82;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 262;
      }
      return %angle;
   }
   else if((%tan >= 7.629858)&&(%tan < 8.829355))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 83;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 263;
      }
      return %angle;
   }
   else if((%tan >= 8.829355)&&(%tan < 10.472208))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 84;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 264;
      }
      return %angle;
   }
   else if((%tan >= 10.472208)&&(%tan < 12.865359))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 85;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 265;
      }
      return %angle;
   }
   else if((%tan >= 12.865359)&&(%tan < 16.690901))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 86;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 266;
      }
      return %angle;
   }
   else if((%tan >= 16.690901)&&(%tan < 23.858695))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 87;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 267;
      }
      return %angle;
   }
   else if((%tan >= 23.858695)&&(%tan < 42.963107))
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 88;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 268;
      }
      return %angle;
   }
   else if(%tan >= 42.963107)
   {
      if((%deltaX > 0)&&(%deltaY > 0))
      {
         %angle = 89;
      }
      else if((%deltaX < 0)&&(%deltaY < 0))
      {
         %angle = 269;
      }
      return %angle;
   }
   else if(%deltaX == 0)
   {
      if(%deltaY > 0)
      {
         %angle = 90;
      }
      else if(%deltaY < 0)
      {
         %angle = 270;
      }
      return %angle;
   }
   else if(%tan < -42.963107)
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 91;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 271;
      }
      return %angle;
   }
   else if((%tan < -23.858695)&&(%tan >= -42.963107))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 92;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 272;
      }
      return %angle;
   }
   else if((%tan < -16.690901)&&(%tan >= -23.858695))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 93;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 273;
      }
      return %angle;
   }
   else if((%tan < -12.865359)&&(%tan >= -16.690901))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 94;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 274;
      }
      return %angle;
   }
   else if((%tan < -10.472208)&&(%tan >= -12.865359))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 95;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 275;
      }
      return %angle;
   }
   else if((%tan < -8.829355)&&(%tan >= -10.472208))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 96;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 276;
      }
      return %angle;
   }
   else if((%tan < -7.629858)&&(%tan >= -8.829355))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 97;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 277;
      }
      return %angle;
   }
   else if((%tan < -6.714561)&&(%tan >= -7.629858))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 98;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 278;
      }
      return %angle;
   }
   else if((%tan < -5.992517)&&(%tan >= -6.714561))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 99;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 279;
      }
      return %angle;
   }
   else if((%tan < -5.407918)&&(%tan >= -5.992517))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 100;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 280;
      }
      return %angle;
   }
   else
   {
      getVector6(%deltaX, %deltaY);
   }
}

function getVector6(%deltaX, %deltaY)
{
   %tan = (%deltaY / %deltaX);
   if((%tan < -4.924592)&&(%tan >= -5.407918))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 101;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 281;
      }
      return %angle;
   }
   else if((%tan < -4.518053)&&(%tan >= -4.924592))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 102;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 282;
      }
      return %angle;
   }
   else if((%tan < -4.171128)&&(%tan >= -4.518053))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 103;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 283;
      }
      return %angle;
   }
   else if((%tan < -3.871416)&&(%tan >= -4.171128))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 104;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 284;
      }
      return %angle;
   }
   else if((%tan < -3.609733)&&(%tan >= -3.871416))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 105;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 285;
      }
      return %angle;
   }
   else if((%tan < -3.379134)&&(%tan >= -3.609733))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 106;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 286;
      }
      return %angle;
   }
   else if((%tan < -3.174268)&&(%tan >= -3.379134))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 107;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 287;
      }
      return %angle;
   }
   else if((%tan < -2.990947)&&(%tan >= -3.174268))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 108;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 288;
      }
      return %angle;
   }
   else if((%tan < -2.825844)&&(%tan >= -2.990947))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 109;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 289;
      }
      return %angle;
   }
   else if((%tan < -2.676283)&&(%tan >= -2.825844))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 110;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 290;
      }
      return %angle;
   }
   else if((%tan < -2.540088)&&(%tan >= -2.676283))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 111;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 291;
      }
      return %angle;
   }
   else if((%tan < -2.415470)&&(%tan >= -2.540088))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 112;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 292;
      }
      return %angle;
   }
   else if((%tan < -2.300945)&&(%tan >= -2.415470))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 113;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 293;
      }
      return %angle;
   }
   else if((%tan < -2.195272)&&(%tan >= -2.300945))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 114;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 294;
      }
      return %angle;
   }
   else if((%tan < -2.097405)&&(%tan >= -2.195272))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 115;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 295;
      }
      return %angle;
   }
   else if((%tan < -2.006457)&&(%tan >= -2.097405))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 116;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 296;
      }
      return %angle;
   }
   else if((%tan < -1.921668)&&(%tan >= -2.006457))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 117;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 297;
      }
      return %angle;
   }
   else if((%tan < -1.842387)&&(%tan >= -1.921668))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 118;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 298;
      }
      return %angle;
   }
   else if((%tan < -1.768049)&&(%tan >= -1.842387))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 119;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 299;
      }
      return %angle;
   }
   else if((%tan < -1.698165)&&(%tan >= -1.768049))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 120;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 300;
      }
      return %angle;
   }
   else
   {
      getVector7(%deltaX, %deltaY);
   }
}

function getVector7(%deltaX, %deltaY)
{
   %tan = (%deltaY / %deltaX);
   if((%tan < -1.632307)&&(%tan >= -1.698165))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 121;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 301;
      }
      return %angle;
   }
   else if((%tan < -1.570100)&&(%tan >= -1.632307))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 122;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 302;
      }
      return %angle;
   }
   else if((%tan < -1.511213)&&(%tan >= -1.570100))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 123;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 303;
      }
      return %angle;
   }
   else if((%tan < -1.455354)&&(%tan >= -1.511213))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 124;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 304;
      }
      return %angle;
   }
   else if((%tan < -1.402265)&&(%tan >= -1.455354))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 125;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 305;
      }
      return %angle;
   }
   else if((%tan < -1.351713)&&(%tan >= -1.402265))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 126;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 306;
      }
      return %angle;
   }
   else if((%tan < -1.303493)&&(%tan >= -1.351713))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 127;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 307;
      }
      return %angle;
   }
   else if((%tan < -1.257419)&&(%tan >= -1.303493))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 128;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 308;
      }
      return %angle;
   }
   else if((%tan < -1.213325)&&(%tan >= -1.257419))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 129;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 309;
      }
      return %angle;
   }
   else if((%tan < -1.171061)&&(%tan >= -1.213325))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 130;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 310;
      }
      return %angle;
   }
   else if((%tan < -1.130490)&&(%tan >= -1.171061))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 131;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 311;
      }
      return %angle;
   }
   else if((%tan < -1.091491)&&(%tan >= -1.130490))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 132;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 312;
      }
      return %angle;
   }
   else if((%tan < -1.053950)&&(%tan >= -1.091491))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 133;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 313;
      }
      return %angle;
   }
   else if((%tan < -1.017765)&&(%tan >= -1.053950))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 134;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 314;
      }
      return %angle;
   }
   else if((%tan < -0.982844)&&(%tan >= -1.017765))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 135;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 315;
      }
      return %angle;
   }
   else if((%tan < -0.949102)&&(%tan >= -0.982844))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 136;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 316;
      }
      return %angle;
   }
   else if((%tan < -0.916460)&&(%tan >= -0.949102))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 137;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 317;
      }
      return %angle;
   }
   else if((%tan < -0.884845)&&(%tan >= -0.916460))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 138;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 318;
      }
      return %angle;
   }
   else if((%tan < -0.854193)&&(%tan >= -0.884845))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 139;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 319;
      }
      return %angle;
   }
   else if((%tan < -0.824442)&&(%tan >= -0.854193))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 140;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 320;
      }
      return %angle;
   }
   else
   {
      getVector8(%deltaX, %deltaY);
   }
}

function getVector8(%deltaX, %deltaY)
{
   %tan = (%deltaY / %deltaX);
   if((%tan < -0.795535)&&(%tan >= -0.824442))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 141;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 321;
      }
      return %angle;
   }
   else if((%tan < -0.767420)&&(%tan >= -0.795535))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 142;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 322;
      }
      return %angle;
   }
   else if((%tan < -0.740048)&&(%tan >= -0.767420))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 143;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 323;
      }
      return %angle;
   }
   else if((%tan < -0.713375)&&(%tan >= -0.740048))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 144;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 324;
      }
      return %angle;
   }
   else if((%tan < -0.687358)&&(%tan >= -0.713375))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 145;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 325;
      }
      return %angle;
   }
   else if((%tan < -0.661958)&&(%tan >= -0.687358))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 146;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 326;
      }
      return %angle;
   }
   else if((%tan < -0.637138)&&(%tan >= -0.661958))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 147;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 327;
      }
      return %angle;
   }
   else if((%tan < -0.612865)&&(%tan >= -0.637138))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 148;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 328;
      }
      return %angle;
   }
   else if((%tan < -0.589105)&&(%tan >= -0.612865))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 149;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 329;
      }
      return %angle;
   }
   else if((%tan < -0.565830)&&(%tan >= -0.589105))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 150;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 330;
      }
      return %angle;
   }
   else if((%tan < -0.543009)&&(%tan >= -0.565830))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 151;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 331;
      }
      return %angle;
   }
   else if((%tan < -0.520617)&&(%tan >= -0.543009))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 152;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 332;
      }
      return %angle;
   }
   else if((%tan < -0.498629)&&(%tan >= -0.520617))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 153;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 333;
      }
      return %angle;
   }
   else if((%tan < -0.477020)&&(%tan >= -0.498629))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 154;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 334;
      }
      return %angle;
   }
   else if((%tan < -0.455768)&&(%tan >= -0.477020))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 155;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 335;
      }
      return %angle;
   }
   else if((%tan < -0.434852)&&(%tan >= -0.455768))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 156;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 336;
      }
      return %angle;
   }
   else if((%tan < -0.414251)&&(%tan >= -0.434852))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 157;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 337;
      }
      return %angle;
   }
   else if((%tan < -0.393945)&&(%tan >= -0.414251))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 158;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 338;
      }
      return %angle;
   }
   else if((%tan < -0.373917)&&(%tan >= -0.393945))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 159;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 339;
      }
      return %angle;
   }
   else if((%tan < -0.354149)&&(%tan >= -0.373917))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 160;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 340;
      }
      return %angle;
   }
   else
   {
      getVector9(%deltaX, %deltaY);
   }
}

function getVector9(%deltaX, %deltaY)
{
   %tan = (%deltaY / %deltaX);
   if((%tan < -0.334624)&&(%tan >= -0.354149))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 161;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 341;
      }
      return %angle;
   }
   else if((%tan < -0.315325)&&(%tan >= -0.334624))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 162;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 342;
      }
      return %angle;
   }
   else if((%tan < -0.296238)&&(%tan >= -0.315325))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 163;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 343;
      }
      return %angle;
   }
   else if((%tan < -0.277347)&&(%tan >= -0.296238))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 164;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 344;
      }
      return %angle;
   }
   else if((%tan < -0.258639)&&(%tan >= -0.277347))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 165;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 345;
      }
      return %angle;
   }
   else if((%tan < -0.240098)&&(%tan >= -0.258639))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 166;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 346;
      }
      return %angle;
   }
   else if((%tan < -0.221712)&&(%tan >= -0.240098))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 167;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 347;
      }
      return %angle;
   }
   else if((%tan < -0.203468)&&(%tan >= -0.221712))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 168;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 348;
      }
      return %angle;
   }
   else if((%tan < -0.185354)&&(%tan >= -0.203468))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 169;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 349;
      }
      return %angle;
   }
   else if((%tan < -0.167356)&&(%tan >= -0.185354))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 170;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 350;
      }
      return %angle;
   }
   else if((%tan < -0.149463)&&(%tan >= -0.167356))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 171;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 351;
      }
      return %angle;
   }
   else if((%tan < -0.131663)&&(%tan >= -0.149463))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 172;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 352;
      }
      return %angle;
   }
   else if((%tan < -0.113944)&&(%tan >= -0.131663))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 173;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 353;
      }
      return %angle;
   }
   else if((%tan < -0.096296)&&(%tan >= -0.113944))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 174;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 354;
      }
      return %angle;
   }
   else if((%tan < -0.078708)&&(%tan >= -0.096296))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 175;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 355;
      }
      return %angle;
   }
   else if((%tan < -0.061167)&&(%tan >= -0.078708))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 176;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 356;
      }
      return %angle;
   }
   else if((%tan < -0.043664)&&(%tan >= -0.061167))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 177;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 357;
      }
      return %angle;
   }
   else if((%tan < -0.026188)&&(%tan >= -0.043664))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 178;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 358;
      }
      return %angle;
   }
   else if((%tan < -0.008728)&&(%tan >= -0.026188))
   {
      if((%deltaX < 0)&&(%deltaY > 0))
      {
         %angle = 179;
      }
      else if((%deltaX > 0)&&(%deltaY < 0))
      {
         %angle = 359;
      }
      return %angle;
   }
}