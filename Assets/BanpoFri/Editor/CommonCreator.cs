using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CommonCreator
{

    [MenuItem("GameObject/BanpoFri Common UI/Dim", false, 8)]
    public static void CommonDim()
    {
        var loadResource =
            AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/Arts/Prefabs/Common/Dim.prefab");

        var obj = PrefabUtility.InstantiatePrefab(loadResource) as GameObject;
        if (Selection.activeGameObject != null)
            obj.transform.SetParent(Selection.activeGameObject.transform, false);
    }

    [MenuItem("GameObject/BanpoFri Common UI/LayoutHorizontal", false, 8)]
    public static void CommonLayoutHorizontal()
    {
        var loadResource =
            AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/Arts/Prefabs/Common/Layout_Horizontal.prefab");

        var obj = PrefabUtility.InstantiatePrefab(loadResource) as GameObject;
        if (Selection.activeGameObject != null)
            obj.transform.SetParent(Selection.activeGameObject.transform, false);
    }

    [MenuItem("GameObject/BanpoFri Common UI/LayoutVertical", false, 8)]
    public static void CommonLayoutVertical()
    {
        var loadResource =
            AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/Arts/Prefabs/Common/Layout_Vertical.prefab");

        var obj = PrefabUtility.InstantiatePrefab(loadResource) as GameObject;
        if (Selection.activeGameObject != null)
            obj.transform.SetParent(Selection.activeGameObject.transform, false);
    }

    [MenuItem("GameObject/BanpoFri Common UI/BtnAds", false, 8)]
    public static void CommonBtnAds()
    {
        var loadResource =
            AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/Arts/Prefabs/Common/Btn_Ads.prefab");

        var obj = PrefabUtility.InstantiatePrefab(loadResource) as GameObject;
        if (Selection.activeGameObject != null)
            obj.transform.SetParent(Selection.activeGameObject.transform, false);
    }

    [MenuItem("GameObject/BanpoFri Common UI/BtnImage", false, 8)]
    public static void CommonBtnImage()
    {
        var loadResource =
            AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/Arts/Prefabs/Common/Btn_Image.prefab");

        var obj = PrefabUtility.InstantiatePrefab(loadResource) as GameObject;
        if (Selection.activeGameObject != null)
            obj.transform.SetParent(Selection.activeGameObject.transform, false);
    }

    [MenuItem("GameObject/BanpoFri Common UI/BtnIcon", false, 8)]
    public static void CommonBtnIcon()
    {
        var loadResource =
            AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/Arts/Prefabs/Common/Btn_Icon.prefab");

        var obj = PrefabUtility.InstantiatePrefab(loadResource) as GameObject;
        if (Selection.activeGameObject != null)
            obj.transform.SetParent(Selection.activeGameObject.transform, false);
    }

    [MenuItem("GameObject/BanpoFri Common UI/BtnIconText", false, 8)]
    public static void CommonBtnIconText()
    {
        var loadResource =
            AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/Arts/Prefabs/Common/Btn_IconText.prefab");

        var obj = PrefabUtility.InstantiatePrefab(loadResource) as GameObject;
        if (Selection.activeGameObject != null)
            obj.transform.SetParent(Selection.activeGameObject.transform, false);
    }

    [MenuItem("GameObject/BanpoFri Common UI/BtnText", false, 8)]
    public static void CommonBtnText()
    {
        var loadResource =
            AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/Arts/Prefabs/Common/Btn_Text.prefab");

        var obj = PrefabUtility.InstantiatePrefab(loadResource) as GameObject;
        if (Selection.activeGameObject != null)
            obj.transform.SetParent(Selection.activeGameObject.transform, false);
    }

    [MenuItem("GameObject/BanpoFri Common UI/BtnVertical", false, 8)]
    public static void CommonBtnVertical()
    {
        var loadResource =
            AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/Arts/Prefabs/Common/Btn_Vertical.prefab");

        var obj = PrefabUtility.InstantiatePrefab(loadResource) as GameObject;
        if (Selection.activeGameObject != null)
            obj.transform.SetParent(Selection.activeGameObject.transform, false);
    }

    [MenuItem("GameObject/BanpoFri Common UI/Image", false, 8)]
    public static void CommonImage()
    {
        var loadResource =
            AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/Arts/Prefabs/Common/Image.prefab");

        var obj = PrefabUtility.InstantiatePrefab(loadResource) as GameObject;
        if (Selection.activeGameObject != null)
            obj.transform.SetParent(Selection.activeGameObject.transform, false);
    }

    [MenuItem("GameObject/BanpoFri Common UI/Popup", false, 8)]
    public static void CommonPopup()
    {
        var loadResource =
            AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/Arts/Prefabs/Common/Popup.prefab");

        var obj = PrefabUtility.InstantiatePrefab(loadResource) as GameObject;
        if (Selection.activeGameObject != null)
            obj.transform.SetParent(Selection.activeGameObject.transform, false);
    }

    [MenuItem("GameObject/BanpoFri Common UI/Progress", false, 8)]
    public static void CommonProgress()
    {
        var loadResource =
            AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/Arts/Prefabs/Common/Progress.prefab");

        var obj = PrefabUtility.InstantiatePrefab(loadResource) as GameObject;
        if (Selection.activeGameObject != null)
            obj.transform.SetParent(Selection.activeGameObject.transform, false);
    }


    [MenuItem("GameObject/BanpoFri Common UI/Reddot", false, 8)]
    public static void CommonReddot()
    {
        var loadResource =
            AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/Arts/Prefabs/Common/Reddot.prefab");

        var obj = PrefabUtility.InstantiatePrefab(loadResource) as GameObject;
        if (Selection.activeGameObject != null)
            obj.transform.SetParent(Selection.activeGameObject.transform, false);
    }

    [MenuItem("GameObject/BanpoFri Common UI/ScrollView", false, 8)]
    public static void CommonScrollView()
    {
        var loadResource =
            AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/Arts/Prefabs/Common/ScrollView.prefab");

        var obj = PrefabUtility.InstantiatePrefab(loadResource) as GameObject;
        if (Selection.activeGameObject != null)
            obj.transform.SetParent(Selection.activeGameObject.transform, false);
    }

    [MenuItem("GameObject/BanpoFri Common UI/TMP", false, 8)]
    public static void CommonTMP()
    {
        var loadResource =
            AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/Arts/Prefabs/Common/TMP.prefab");

        var obj = PrefabUtility.InstantiatePrefab(loadResource) as GameObject;
        if (Selection.activeGameObject != null)
            obj.transform.SetParent(Selection.activeGameObject.transform, false);
    }

    [MenuItem("GameObject/BanpoFri Common UI/RayEffect", false, 8)]
    public static void CommonRayEffect()
    {
        var loadResource =
            AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/Arts/Prefabs/Common/RayEffect.prefab");

        var obj = PrefabUtility.InstantiatePrefab(loadResource) as GameObject;
        if (Selection.activeGameObject != null)
            obj.transform.SetParent(Selection.activeGameObject.transform, false);
    }

    [MenuItem("GameObject/BanpoFri Common UI/RecycleScrollViewer", false, 8)]
    public static void CommonRecycleScrollViewer()
    {
        var loadResource =
            AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/Arts/Prefabs/Common/RecycleScrollViewer.prefab");

        var obj = PrefabUtility.InstantiatePrefab(loadResource) as GameObject;
        if (Selection.activeGameObject != null)
            obj.transform.SetParent(Selection.activeGameObject.transform, false);
    }
    
    [MenuItem("GameObject/BanpoFri Common UI/Tile", false, 8)]
    public static void CommonTile()
    {
        var loadResource =
            AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/Arts/Prefabs/Common/Tile.prefab");

        var obj = PrefabUtility.InstantiatePrefab(loadResource) as GameObject;
        if (Selection.activeGameObject != null)
            obj.transform.SetParent(Selection.activeGameObject.transform, false);
    }

}
