using Scripts.Logic;
using Scripts.Logic.EnemySpawners;
using Scripts.RaidScreen;
using Scripts.StaticData;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    [CustomEditor(typeof(LevelStaticData))]
    public class LevelStaticDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI ()
        {
            base.OnInspectorGUI ();

            LevelStaticData levelData = (LevelStaticData)target;
            if (GUILayout.Button("Collect"))
            {
                levelData.EnemySpawners = FindObjectsOfType<SpawnMarker>()
                    .Select(x => new EnemySpawnerData(x.Id,x.MonsterTypeId,x.transform.position,x.IdRaid))
                    .ToList();

                //levelData.LevelKey = SceneManager.GetActiveScene().name;
                levelData.LevelKey = FindObjectOfType<RaidsObject>().Id.ToString();
            }

            EditorUtility.SetDirty(target);
        }
    }
}