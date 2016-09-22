// Copyright (c) 2011, Triniti Interactive Company, China.
// All rights reserved.
//
// Created by Cheng Haitao at 5/12/2011 8:56:00 AM.
// E-Mail: chenghaitao@trinitigame.com.cn


using UnityEngine;
using UnityEditor;



class Avatar
{
    static GameObject oPrefab = null;
    static GameObject goboy = null;


    static GameObject LoadAvatarMesh(string name)
    {
        return AssetDatabase.LoadAssetAtPath("Assets/Projects/Zombie3D/characters/Soldier/"+name, typeof(GameObject)) as GameObject;
    }




    //[MenuItem("PreProcess/Avatar_Create")]
    static void AnimationClipBoy()
	{

		//! 装载 fbx 模型
        goboy = LoadAvatarMesh("Soldier_Mesh.FBX");

		// 动作
		//AnimationClipCommon(goboy);
		//goboy.animation.AddClip(CustomAnimation.HorseAnimationResourceLoad("Boy@Ball"), AnimateName.AN_Ball);



        CreateNew();
        CustomHuman();
        Replace("Human");


        CreateNew();
        CustomPlumber();
        Replace("Plumber");

        CreateNew();
        CustomNerd();
        Replace("Nerd");

        CreateNew();
        CustomSwat();
        Replace("Swat");

        CreateNew();
        CustomMarine();
        Replace("Marine");



        goboy = LoadAvatarMesh("Doctor.FBX");
        CreateNewDoctor();
        CustomDoctor();
        Replace("Doctor");

        goboy = LoadAvatarMesh("Cowboy.FBX");
        CreateNewCowboy();
        CustomCowboy();
        Replace("Cowboy");


	}


    public static void CreateNew()
    {
       
        oPrefab = EditorUtility.InstantiatePrefab(goboy) as GameObject;
        oPrefab.AddComponent<CharacterController>().center = new Vector3(0, 0.9444f, 0);
        oPrefab.AddComponent<ControllerHitScript>();

        GameObject shadow = AssetDatabase.LoadAssetAtPath("Assets/Projects/Zombie3D/Prefabs/characters/Avatar/shadow.prefab", typeof(GameObject)) as GameObject;
        GameObject shadowObj = GameObject.Instantiate(shadow) as GameObject;
        shadowObj.name = "shadow";
        shadowObj.transform.parent = oPrefab.transform;

        GameObject audio = AssetDatabase.LoadAssetAtPath("Assets/Projects/Zombie3D/Prefabs/characters/Avatar/Audio.prefab", typeof(GameObject)) as GameObject;
        GameObject audioObj = GameObject.Instantiate(audio) as GameObject;
        audioObj.name = "Audio";
        audioObj.transform.parent = oPrefab.transform;

        
        

        oPrefab.transform.Find("Marines").name = "Avatar_Suit";
        oPrefab.transform.Find("Marines_Cap").name = "Avatar_Cap";
        oPrefab.transform.Find("Avatar_Cap").GetComponent<Renderer>().enabled = false;

        ChangeMaterial(oPrefab.transform.Find("Avatar_Cap").GetComponent<Renderer>(), "marine_hat");

        AlphaAnimationScript aas = oPrefab.transform.Find("Avatar_Cap").gameObject.AddComponent<AlphaAnimationScript>();
        oPrefab.transform.Find("Avatar_Suit").gameObject.AddComponent<AlphaAnimationScript>();

        oPrefab.layer = 8;

    }

    public static void CreateNewDoctor()
    {
        oPrefab = EditorUtility.InstantiatePrefab(goboy) as GameObject;
        oPrefab.AddComponent<CharacterController>().center = new Vector3(0, 0.9444f, 0);
        oPrefab.AddComponent<ControllerHitScript>();

        GameObject shadow = AssetDatabase.LoadAssetAtPath("Assets/Projects/Zombie3D/Prefabs/characters/Avatar/shadow.prefab", typeof(GameObject)) as GameObject;
        GameObject shadowObj = GameObject.Instantiate(shadow) as GameObject;
        shadowObj.name = "shadow";
        shadowObj.transform.parent = oPrefab.transform;

        GameObject audio = AssetDatabase.LoadAssetAtPath("Assets/Projects/Zombie3D/Prefabs/characters/Avatar/Audio.prefab", typeof(GameObject)) as GameObject;
        GameObject audioObj = GameObject.Instantiate(audio) as GameObject;
        audioObj.name = "Audio";
        audioObj.transform.parent = oPrefab.transform;




        oPrefab.transform.Find("Doctor").name = "Avatar_Suit";
        
        //oPrefab.transform.Find("Marines_Cap").name = "Avatar_Cap";
        //oPrefab.transform.Find("Avatar_Cap").renderer.enabled = false;

        //ChangeMaterial(oPrefab.transform.Find("Avatar_Cap").renderer, "marine_hat");

        //AlphaAnimationScript aas = oPrefab.transform.Find("Avatar_Cap").gameObject.AddComponent<AlphaAnimationScript>();
        oPrefab.transform.Find("Avatar_Suit").gameObject.AddComponent<AlphaAnimationScript>();

        oPrefab.layer = 8;


    }


    public static void CreateNewCowboy()
    {
        oPrefab = EditorUtility.InstantiatePrefab(goboy) as GameObject;
        oPrefab.AddComponent<CharacterController>().center = new Vector3(0, 0.9444f, 0);
        oPrefab.AddComponent<ControllerHitScript>();

        GameObject shadow = AssetDatabase.LoadAssetAtPath("Assets/Projects/Zombie3D/Prefabs/characters/Avatar/shadow.prefab", typeof(GameObject)) as GameObject;
        GameObject shadowObj = GameObject.Instantiate(shadow) as GameObject;
        shadowObj.name = "shadow";
        shadowObj.transform.parent = oPrefab.transform;

        GameObject audio = AssetDatabase.LoadAssetAtPath("Assets/Projects/Zombie3D/Prefabs/characters/Avatar/Audio.prefab", typeof(GameObject)) as GameObject;
        GameObject audioObj = GameObject.Instantiate(audio) as GameObject;
        audioObj.name = "Audio";
        audioObj.transform.parent = oPrefab.transform;




        oPrefab.transform.Find("Cowboy").name = "Avatar_Suit";

        oPrefab.transform.Find("Cowboy_Cap02").name = "Avatar_Cap";

        ChangeMaterial(oPrefab.transform.Find("Avatar_Cap").GetComponent<Renderer>(), "cowboy_hat");

        AlphaAnimationScript aas = oPrefab.transform.Find("Avatar_Cap").gameObject.AddComponent<AlphaAnimationScript>();
        oPrefab.transform.Find("Avatar_Suit").gameObject.AddComponent<AlphaAnimationScript>();

        oPrefab.layer = 8;
    }


    public static void Replace(string name)
    {
        // 创建Prefab
        Object prefab = EditorUtility.CreateEmptyPrefab("Assets/Projects/Zombie3D/Prefabs/characters/Avatar/"+name+".prefab");
        EditorUtility.ReplacePrefab((GameObject)oPrefab, prefab);
        AssetDatabase.Refresh();
        
        GameObject.DestroyImmediate(oPrefab);
    }

    public static void ChangeMaterial(Renderer r, string m)
    {
        r.sharedMaterial = AssetDatabase.LoadAssetAtPath("Assets/Projects/Zombie3D/characters/Materials/Avatar/"+m+".mat", typeof(Material)) as Material;
    }

    public static void CustomHuman()
    {
        ChangeMaterial(oPrefab.transform.Find("Avatar_Suit").GetComponent<Renderer>(), "human");
        GameObject.DestroyImmediate(oPrefab.transform.Find("Plumber_Cap").gameObject);
        GameObject.DestroyImmediate(oPrefab.transform.Find("Swat_Cap").gameObject);
        GameObject.DestroyImmediate(oPrefab.transform.Find("Wonk_Eyeglass").gameObject);
        
    }

    public static void CustomPlumber()
    {
        ChangeMaterial(oPrefab.transform.Find("Avatar_Suit").GetComponent<Renderer>(), "plumber");
        ChangeMaterial(oPrefab.transform.Find("Plumber_Cap").GetComponent<Renderer>(), "plumber_hat");
        GameObject.DestroyImmediate(oPrefab.transform.Find("Swat_Cap").gameObject);
        GameObject.DestroyImmediate(oPrefab.transform.Find("Wonk_Eyeglass").gameObject);
    }

    public static void CustomNerd()
    {
        ChangeMaterial(oPrefab.transform.Find("Avatar_Suit").GetComponent<Renderer>(), "nerd");
        ChangeMaterial(oPrefab.transform.Find("Wonk_Eyeglass").GetComponent<Renderer>(), "wearings");
        GameObject.DestroyImmediate(oPrefab.transform.Find("Plumber_Cap").gameObject);
        GameObject.DestroyImmediate(oPrefab.transform.Find("Swat_Cap").gameObject);
    }

    public static void CustomSwat()
    {
        ChangeMaterial(oPrefab.transform.Find("Avatar_Suit").GetComponent<Renderer>(), "swat");
        ChangeMaterial(oPrefab.transform.Find("Swat_Cap").GetComponent<Renderer>(), "swat_hat");
        GameObject.DestroyImmediate(oPrefab.transform.Find("Plumber_Cap").gameObject);
        GameObject.DestroyImmediate(oPrefab.transform.Find("Wonk_Eyeglass").gameObject);
    }

    public static void CustomMarine()
    {
        ChangeMaterial(oPrefab.transform.Find("Avatar_Suit").GetComponent<Renderer>(), "marine");
        
        GameObject.DestroyImmediate(oPrefab.transform.Find("Plumber_Cap").gameObject);
        GameObject.DestroyImmediate(oPrefab.transform.Find("Swat_Cap").gameObject);
        GameObject.DestroyImmediate(oPrefab.transform.Find("Wonk_Eyeglass").gameObject);
        oPrefab.transform.Find("Avatar_Cap").GetComponent<Renderer>().enabled = true;
    }

    public static void CustomDoctor()
    {
        ChangeMaterial(oPrefab.transform.Find("Avatar_Suit").GetComponent<Renderer>(), "doctor");
    }

    public static void CustomCowboy()
    {
        ChangeMaterial(oPrefab.transform.Find("Avatar_Suit").GetComponent<Renderer>(), "cowboy");
    }

}

