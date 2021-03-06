﻿using UnityEngine;
using UnityEditor;
using kTools.Portals;

namespace kTools.PortalsEditor
{
	[CustomEditor(typeof(PortalSystem))]
	public class PortalSystemEditor : Editor
	{
		// -------------------------------------------------- //
        //                    EDITOR STYLES                   //
        // -------------------------------------------------- //

        internal class Styles
        {
            public static GUIContent volumesSettingsText = EditorGUIUtility.TrTextContent("Volume Settings");
			public static GUIContent modeText = EditorGUIUtility.TrTextContent("Mode");
            public static GUIContent subdivisionsText = EditorGUIUtility.TrTextContent("Subdivisions");
            public static GUIContent bakeSettingsText = EditorGUIUtility.TrTextContent("Bake Settings");
            public static GUIContent rayDensityText = EditorGUIUtility.TrTextContent("Ray Density");
            public static GUIContent coneAngleText = EditorGUIUtility.TrTextContent("Cone Angle");
            public static GUIContent debugSettingsText = EditorGUIUtility.TrTextContent("Debug Settings");
            public static GUIContent drawOccludersText = EditorGUIUtility.TrTextContent("Draw Occluders");
            public static GUIContent drawVolumesText = EditorGUIUtility.TrTextContent("Draw Volumes");
            public static GUIContent drawVisibilityText = EditorGUIUtility.TrTextContent("Draw Visibility");
			public static GUIContent bakeToolsText = EditorGUIUtility.TrTextContent("Bake Tools");
        }

        // -------------------------------------------------- //
        //                   PRIVATE FIELDS                   //
        // -------------------------------------------------- //
		
		SerializedProperty m_VolumeModeProp;
        SerializedProperty m_SubdivisionsProp;
        SerializedProperty m_RayDensityProp;
        SerializedProperty m_ConeAngleProp;
        SerializedProperty m_DrawOccludersProp;
        SerializedProperty m_DrawVolumesProp;
        SerializedProperty m_DrawVisibilityProp;

		// -------------------------------------------------- //
        //                   PUBLIC METHODS                   //
        // -------------------------------------------------- //

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawVolumeSettings();
            DrawBakeSettings();
            DrawDebugSettings();
            
			EditorGUILayout.LabelField(Styles.bakeToolsText, EditorStyles.boldLabel);
            BakeEditor.DrawBakeTools(target as IBake, GetStatistics());
            serializedObject.ApplyModifiedProperties();
        }
		
		// -------------------------------------------------- //
        //                   PRIVATE METHODS                  //
        // -------------------------------------------------- //

        private void OnEnable()
        {
            m_VolumeModeProp = serializedObject.FindProperty("m_VolumeMode");
            m_SubdivisionsProp = serializedObject.FindProperty("m_Subdivisions");
            m_RayDensityProp = serializedObject.FindProperty("m_RayDensity");
            m_ConeAngleProp = serializedObject.FindProperty("m_ConeAngle");
            m_DrawOccludersProp = serializedObject.FindProperty("m_DrawOccluders");
            m_DrawVolumesProp = serializedObject.FindProperty("m_DrawVolumes");
            m_DrawVisibilityProp = serializedObject.FindProperty("m_DrawVisibility");
        }

        private string GetStatistics()
        {
            var actualTarget = target as PortalSystem;
            if(actualTarget.bakeState != BakeState.Active)
                return "No bake data available.";

            var passedRays = actualTarget.rayStatistics.x;
            var occludedRays = actualTarget.rayStatistics.y;
            var totalRays = actualTarget.rayStatistics.z;
            var passedRaysPercentage = (actualTarget.rayStatistics.x / actualTarget.rayStatistics.z) * 100f;
            var occludedRaysPercentage = (actualTarget.rayStatistics.y / actualTarget.rayStatistics.z) * 100f;
            var passedOccludees = actualTarget.occludeeStatistics.x;
            var allOccludees = actualTarget.occludeeStatistics.y;
            var occludeesPercentage = (actualTarget.occludeeStatistics.x / actualTarget.occludeeStatistics.y) * 100f;
            var elapsedTime = actualTarget.bakeTime;

            return string.Format(
@"Passing rays: {0}/{1} ({2}%)
Occluded rays: {3}/{1} ({4}%)
Processed Occludees: {5}/{6} ({7}%)
Elapsed time: {8}s", 
    passedRays, totalRays, string.Format("{0:0.00}", passedRaysPercentage),
    occludedRays, string.Format("{0:0.00}", occludedRaysPercentage),
    passedOccludees, allOccludees, string.Format("{0:0.00}", occludeesPercentage),
    string.Format("{0:0.00}", elapsedTime));
        }

        private void DrawVolumeSettings()
        {
            EditorGUILayout.LabelField(Styles.volumesSettingsText, EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_VolumeModeProp, Styles.modeText);
            if(m_VolumeModeProp.enumValueIndex != (int)VolumeMode.Manual)
                EditorGUILayout.PropertyField(m_SubdivisionsProp, Styles.subdivisionsText);
            EditorGUILayout.Space();
        }

        private void DrawDebugSettings()
        {
            EditorGUILayout.LabelField(Styles.debugSettingsText, EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_DrawOccludersProp, Styles.drawOccludersText);
            EditorGUILayout.PropertyField(m_DrawVolumesProp, Styles.drawVolumesText);
            EditorGUILayout.PropertyField(m_DrawVisibilityProp, Styles.drawVisibilityText);
            EditorGUILayout.Space();
        }

        private void DrawBakeSettings()
        {
            EditorGUILayout.LabelField(Styles.bakeSettingsText, EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_RayDensityProp, Styles.rayDensityText);
            EditorGUILayout.PropertyField(m_ConeAngleProp, Styles.coneAngleText);
            EditorGUILayout.Space();
        }
	}
}
