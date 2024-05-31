using Scripts.Logic;
using Scripts.RaidScreen;
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Editor
{
    //[CustomEditor(typeof(UniqueId))]
    //public class UniqueIdEditor : UnityEditor.Editor
    //{
    //    private void OnEnable()
    //    {
    //        var uniqueId = (UniqueId)target;

    //        if (IsPrefab(uniqueId))
    //            return;

    //        if (string.IsNullOrEmpty(uniqueId.Id.ToString()))
    //            Generate(uniqueId);
    //        else
    //        {
    //            UniqueId[] uniqueIds = FindObjectsOfType<UniqueId>();

    //            if (uniqueIds.Any(other => other != uniqueId && other.Id == uniqueId.Id))
    //                Generate(uniqueId);
    //        }
    //    }

    //    private bool IsPrefab(UniqueId uniqueId) =>
    //        uniqueId.gameObject.scene.rootCount == 0;

    //    private void Generate(UniqueId uniqueId)
    //    {
    //        RaidsObject raidsObject = FindObjectOfType<RaidsObject>();
    //        foreach (var raid in raidsObject.UniqueIdList)
    //        {
    //            int id = raidsObject.Id;
    //            id++;
    //            raid.Id = id;
    //            uniqueId.Id = raid.Id;
    //        }
    //        //uniqueId.Id = //$"{uniqueId.gameObject.scene.name}_{Guid.NewGuid().ToString()}";
    //        if (!Application.isPlaying)
    //        {
    //            EditorUtility.SetDirty(uniqueId);
    //            EditorSceneManager.MarkSceneDirty(uniqueId.gameObject.scene);
    //        }
    //    }
    //}
}