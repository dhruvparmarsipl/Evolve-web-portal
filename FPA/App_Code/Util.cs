﻿using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for Util
/// </summary>

namespace Utilities
{
    public class Util
    {
        private string[] arr_FCColors;
        private int FC_ColorCounter;
        public Util()
	{
		/*    
         * This page contains an array of colors to be used as default set of colors for FusionCharts
         * arr_FCColors is the array that would contain the hex code of colors 
         * ALL COLORS HEX CODES TO BE USED WITHOUT #
         * 
         * We also initiate a counter variable to help us cyclically rotate through
         * the array of colors.
         */ 
           
        FC_ColorCounter = 0;
        arr_FCColors=new string [40];
        //arr_FCColors[0] = "1941A5"; //Dark Blue
        //arr_FCColors[1] = "AFD8F8";
        //arr_FCColors[2] = "F6BD0F";
        //arr_FCColors[3] = "8BBA00";
        //arr_FCColors[4] = "A66EDD";
        //arr_FCColors[5] = "F984A1";
        arr_FCColors[1] = "4471A5";
        arr_FCColors[2] = "E26b09";
        arr_FCColors[3] = "494429";
        arr_FCColors[4] = "00ae4f";
        arr_FCColors[5] = "Fcbe00";
        arr_FCColors[6] = "CCCC00"; //Chrome Yellow+Green
        arr_FCColors[7] = "999999"; //Grey
        arr_FCColors[8] = "0099CC"; //Blue Shade
        arr_FCColors[9] = "FF0000"; //Bright Red 
        arr_FCColors[10] = "006F00"; //Dark Green
        arr_FCColors[11] = "0099FF"; //Blue (Light)
        arr_FCColors[12] = "FF66CC"; //Dark Pink
        arr_FCColors[13] = "669966"; //Dirty green
        arr_FCColors[14] = "7C7CB4"; //Violet shade of blue
        arr_FCColors[15] = "FF9933"; //Orange
        arr_FCColors[16] = "9900FF"; //Violet
        arr_FCColors[17] = "99FFCC"; //Blue+Green Light
        arr_FCColors[18] = "CCCCFF"; //Light violet
        arr_FCColors[19] = "669900"; //Shade of green

        
        arr_FCColors[20] = "669966"; //Dark Blue
        arr_FCColors[21] = "333399";
        arr_FCColors[22] = "339999";
        arr_FCColors[23] = "CCCC99";
        arr_FCColors[24] = "CCFF66";
        arr_FCColors[25] = "CC3300";
        arr_FCColors[26] = "666666"; 
        arr_FCColors[27] = "0000FF"; 
        arr_FCColors[28] = "FF00FF"; 
        arr_FCColors[29] = "CCFFFF"; 
        arr_FCColors[30] = "9900FF"; 
        arr_FCColors[31] = "99CC66"; 
        arr_FCColors[32] = "000066"; 
        arr_FCColors[33] = "00FFCC"; 
        arr_FCColors[34] = "FF9999"; 
        arr_FCColors[35] = "FFCC66"; 
        arr_FCColors[36] = "CCFFFF"; 
        arr_FCColors[37] = "CCCCCC"; 
        arr_FCColors[38] = "CC3399"; 
        arr_FCColors[39] = "339999"; 
	}
        //getFCColor method helps return a color from arr_FCColors array. It uses
        //cyclic iteration to return a color from a given index. The index value is
        //maintained in FC_ColorCounter

        public string getFCColor()
        {

            //Update index
            FC_ColorCounter++;
            //Return color
            return arr_FCColors[FC_ColorCounter % arr_FCColors.Length];
        }

        public string GlobalDate(string sDate)
        {
            string dt = string.Empty;
            if (sDate.Contains("/"))
                sDate = sDate.Replace("/", "-");
            if (sDate != "")
            {
                string Day = Convert.ToString(sDate.Split('-')[0]);
                string Month = Convert.ToString(sDate.Split('-')[1]);
                string Year = Convert.ToString(sDate.Split('-')[2]);
                dt = Month + '/' + Day + '/' + Year;
            }
            return dt;
        }
    }
}
