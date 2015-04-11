using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(UIDamage))]
public class UIDamageInspector : Editor
{
	UIDamage m;
	string errorMessage = "";

	/// <summary>
	/// Draw the inspector.
	/// </summary>

	public override void OnInspectorGUI()
	{
#if UNITY_3_5 || UNITY_4_0 || UNITY_4_1 || UNITY_4_2
		EditorGUIUtility.LookLikeControls(160f);
#else
		EditorGUIUtility.labelWidth = 160f;
#endif
		m = target as UIDamage;

		Transform mTarget = (Transform)EditorGUILayout.ObjectField("Target", m.target, typeof(Transform), true);
		string targetTag = EditorGUILayout.TagField("Target Tag", m.targetTag);
		UIWidget screenFlash = (UIWidget)EditorGUILayout.ObjectField("Screen Flash", m.screenFlash, typeof(UIWidget), true);
		GameObject damageIndicator = (GameObject)EditorGUILayout.ObjectField("Damage Indicator Prefab", m.damageIndicator, typeof(GameObject), true);
		float damageFadeSpeed = (float)EditorGUILayout.Slider(new GUIContent("Damage Fade Speed", "How fast the damage indicators should be faded out. Greater > faster."), m.damageFadeSpeed, 0f, 2f);
		float screenFadeSpeed = (float)EditorGUILayout.Slider(new GUIContent("Screen Flash Fade Speed", "How fast the screen flash should be faded out. Greater > faster."), m.screenFadeSpeed, 0f, 2f);
		float minAlpha = (float)EditorGUILayout.Slider(new GUIContent("Minimun Alpha", "The minimun alpha that is going to use the screen flash on fadeout. "), m.minAlpha, 0f, 1f);
		float refreshRate = (float)EditorGUILayout.Slider(new GUIContent("Update Rate", "The minimun alpha that is going to use the screen flash on fadeout. "), m.refreshRate, 0.01f, 1f);

		if (mTarget != m.target ||
			targetTag != m.targetTag ||
			screenFlash != m.screenFlash ||
			damageIndicator != m.damageIndicator ||
			damageFadeSpeed != m.damageFadeSpeed ||
			screenFadeSpeed != m.screenFadeSpeed ||
			minAlpha != m.minAlpha ||
			refreshRate != m.refreshRate)
		{
			m.target = mTarget;
			m.targetTag = targetTag;
			m.screenFlash = screenFlash;
			m.damageIndicator = damageIndicator;
			m.damageFadeSpeed = damageFadeSpeed;
			m.screenFadeSpeed = screenFadeSpeed;
			m.minAlpha = minAlpha;
			m.refreshRate = refreshRate;
			NGUIEditorTools.RegisterUndo("UIDamage Settings", m);
		}

		if (m.damageIndicator == null) errorMessage = "No 'Damage Indicator Prefab' has been defined";
		else errorMessage = "";

		if (!string.IsNullOrEmpty(errorMessage))
		{
			EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
		}

		bool shouldBeOn = NGUIEditorTools.DrawHeader("Shake Effect");
		if (shouldBeOn)
		{
			NGUIEditorTools.BeginContents();
			Transform camTrans = (Transform)EditorGUILayout.ObjectField("Camera Transform", m.shakeEffect.camera, typeof(Transform), true);
			int repeats = (int)EditorGUILayout.Slider("Shake Repeats", m.shakeEffect.repeats, 0f, 200f);
			float speed = (float)EditorGUILayout.Slider("Shake Speed", m.shakeEffect.speed, 0f, 500f);
			Vector2 distance = EditorGUILayout.Vector2Field("Shake Distance", m.shakeEffect.distance);

			if (m.shakeEffect.camera != camTrans ||
				m.shakeEffect.repeats != repeats ||
				m.shakeEffect.speed != speed ||
				m.shakeEffect.distance != distance)
			{
				m.shakeEffect.camera = camTrans;
				m.shakeEffect.repeats = repeats;
				m.shakeEffect.speed = speed;
				m.shakeEffect.distance = distance;
				NGUIEditorTools.RegisterUndo("UIDamage Shake Settings", m);
			}
			NGUIEditorTools.EndContents();
		}
		
		shouldBeOn = NGUIEditorTools.DrawHeader("Debug Actions");
		if (shouldBeOn)
		{
			NGUIEditorTools.BeginContents();

			//m.testDirection = EditorGUILayout.Vector3Field("Test Direction", m.testDirection);
			GUI.enabled = Application.isPlaying;
			GUI.backgroundColor = Color.red;
			if (GUILayout.Button("Add Damage Indicator"))
			{
				UIDamage.Show(Vector3.one * NGUITools.RandomRange(-100, 100));
			}

			GUI.backgroundColor = Color.green;
			if (GUILayout.Button("Clear Screen"))
			{
				UIDamage.ClearScreen();
			}
			GUI.backgroundColor = Color.white;
			GUI.enabled = true;

			if (!Application.isPlaying)
			{
				EditorGUILayout.HelpBox("You can only use this when the unity is playing", MessageType.Warning);
			}
			
			NGUIEditorTools.EndContents();
		}
		GUI.backgroundColor = Color.white;
	}

}
