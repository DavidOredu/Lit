//////////////////////////////////////////////////////
// MK Glow Camera Data URP  	    	    	    //
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2020 All rights reserved.            //
//////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK.Glow.URP
{
    internal class CameraDataURP : CameraData
    {
        public static implicit operator CameraDataURP(Camera input)
        {
            CameraDataURP data = new CameraDataURP();

            data.width = input.scaledPixelWidth;
            data.height = input.scaledPixelHeight;
            data.stereoEnabled = input.stereoEnabled;
            data.aspect = input.aspect;
            data.worldToCameraMatrix = input.worldToCameraMatrix;
            
            return data;
        }
    }
}
