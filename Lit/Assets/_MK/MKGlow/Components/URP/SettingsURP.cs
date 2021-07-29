//////////////////////////////////////////////////////
// MK Glow Settings URP 	    	    	        //
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
    internal sealed class SettingsURP : MK.Glow.Settings
    {
        public static implicit operator SettingsURP(MK.Glow.URP.MKGlow input)
        {
            SettingsURP settings = new SettingsURP();
            
            //Main
            settings.allowComputeShaders = false;
            settings.allowGeometryShaders = false;
            settings.renderPriority = input.renderPriority.value;
            settings.debugView = input.debugView.value;
            settings.quality = input.quality.value;
            settings.antiFlickerMode = input.antiFlickerMode.value;
            settings.workflow = input.workflow.value;
            settings.selectiveRenderLayerMask = input.selectiveRenderLayerMask.value;
            settings.anamorphicRatio = input.anamorphicRatio.value;
            settings.lumaScale = input.lumaScale.value;

            //Bloom
            settings.bloomThreshold = input.bloomThreshold.value;
            settings.bloomScattering = input.bloomScattering.value;
            settings.bloomIntensity = input.bloomIntensity.value;
            settings.blooming = input.blooming.value;

            //LensSurface
            settings.allowLensSurface = input.allowLensSurface.value;
            settings.lensSurfaceDirtTexture = input.lensSurfaceDirtTexture.value;
            settings.lensSurfaceDirtIntensity = input.lensSurfaceDirtIntensity.value;
            settings.lensSurfaceDiffractionTexture = input.lensSurfaceDiffractionTexture.value;
            settings.lensSurfaceDiffractionIntensity = input.lensSurfaceDiffractionIntensity.value;

            //LensFlare
            settings.allowLensFlare = input.allowLensFlare.value;
            settings.lensFlareStyle = input.lensFlareStyle.value;
            settings.lensFlareGhostFade = input.lensFlareGhostFade.value;
            settings.lensFlareGhostIntensity = input.lensFlareGhostIntensity.value;
            settings.lensFlareThreshold = input.lensFlareThreshold.value;
            settings.lensFlareScattering = input.lensFlareScattering.value;
            settings.lensFlareColorRamp = input.lensFlareColorRamp.value;
            settings.lensFlareChromaticAberration = input.lensFlareChromaticAberration.value;
            settings.lensFlareGhostCount = input.lensFlareGhostCount.value;
            settings.lensFlareGhostDispersal = input.lensFlareGhostDispersal.value;
            settings.lensFlareHaloFade = input.lensFlareHaloFade.value;
            settings.lensFlareHaloIntensity = input.lensFlareHaloIntensity.value;
            settings.lensFlareHaloSize = input.lensFlareHaloSize.value;

            settings.SetLensFlarePreset(input.lensFlareStyle.value);

            //Glare
            settings.allowGlare = input.allowGlare.value;
            settings.glareBlend = input.glareBlend.value;
            settings.glareIntensity = input.glareIntensity.value;
            settings.glareThreshold = input.glareThreshold.value;
            settings.glareStreaks = input.glareStreaks.value;
            settings.glareScattering = input.glareScattering.value;
            settings.glareStyle = input.glareStyle.value;
            settings.glareAngle = input.glareAngle.value;

            //Sample0
            settings.glareSample0Scattering = input.glareSample0Scattering.value;
            settings.glareSample0Angle = input.glareSample0Angle.value;
            settings.glareSample0Intensity = input.glareSample0Intensity.value;
            settings.glareSample0Offset = input.glareSample0Offset.value;
            //Sample1
            settings.glareSample1Scattering = input.glareSample1Scattering.value;
            settings.glareSample1Angle = input.glareSample1Angle.value;
            settings.glareSample1Intensity = input.glareSample1Intensity.value;
            settings.glareSample1Offset = input.glareSample1Offset.value;
            //Sample2
            settings.glareSample2Scattering = input.glareSample2Scattering.value;
            settings.glareSample2Angle = input.glareSample2Angle.value;
            settings.glareSample2Intensity = input.glareSample2Intensity.value;
            settings.glareSample2Offset = input.glareSample2Offset.value;
            //Sample3
            settings.glareSample3Scattering = input.glareSample3Scattering.value;
            settings.glareSample3Angle = input.glareSample3Angle.value;
            settings.glareSample3Intensity = input.glareSample3Intensity.value;
            settings.glareSample3Offset = input.glareSample3Offset.value;

            settings.SetGlarePreset(input.glareStyle.value);

            return settings;
        }
    }
}
