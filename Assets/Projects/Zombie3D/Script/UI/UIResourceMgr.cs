// Copyright (c) 2011, Triniti Interactive Company, China.
// All rights reserved.
//
// Created by Cheng Haitao at 4/20/2011 1:09:05 PM.
// E-Mail: chenghaitao@trinitigame.com.cn


using UnityEngine;
using System.Collections;



class MaterialInfo
{
    public string name;
    public Material material;
}

public class UIResourceMgr
{
    protected ArrayList materialList = new ArrayList();
    protected static UIResourceMgr instance = new UIResourceMgr();
    protected string m_ui_material_path;  //最后带反斜杠
    public static UIResourceMgr GetInstance()
    {
        return instance;
    }

    public Material GetMaterial(string name)
    {
        foreach (MaterialInfo mInfo in materialList)
        {
            if (mInfo.name == name)
            {
                return mInfo.material;
            }
        }

        Material material = LoadUIMaterial(name);
        MaterialInfo info = new MaterialInfo();
        info.material = material;
        info.name = name;
        materialList.Add(info);
        return material;

    }


    protected Material LoadUIMaterial(string name)
    {
        //Debug.Log("Try load "+name);

        string path_material = m_ui_material_path + name;
        
		//! 小一倍的贴图
        if (ResolutionConstant.R == 0.5f)
        {
            //Debug.Log(ResolutionConstant.R);
            path_material += "_3gs";
        }

        Material material = Resources.Load(path_material) as Material;
        if (material == null)
        {
            Debug.Log("load material error: " + path_material);
        }
        else
        {
            Texture texture = material.mainTexture;
            Debug.Log("UI       " + name + "   Resource  Loaded.");

        }
        return material;
    }

    public void PrintExistingMaterials()
    {
        /*
        string str = "Print : ";
        foreach (MaterialInfo mInfo in materialList)
        {

            str += mInfo.name + ", ";


        }
        Debug.Log(str);
        */
    }

    protected void UnLoadMaterial(string name)
    {
        string path_material = m_ui_material_path + name;


        for (int i = materialList.Count - 1; i > 0; i--)
        {
            MaterialInfo info = materialList[i] as MaterialInfo;
            if (info.name == name)
            {

                //info.material.mainTexture = null;
                info.material = null;
                materialList.Remove(info);
                Debug.Log("UnLoad UI Resource:" + name);
                return;
            }
        }

        Debug.Log("Unload material error: " + path_material);


    }

    /*
    public void DestroyTexture(string name)
    {
        for (int i = materialList.Count - 1; i > 0; i--)
        {
            MaterialInfo info = materialList[i] as MaterialInfo;
            if (info.name == name)
            {


                GameObject.Destroy(info.material.mainTexture);
                //materialList.Remove(info);
                Debug.Log("Destroy Texture:" + name);
                return;
            }
        }
    }
    */

    public void LoadStartMenuUIMaterials()
    {
        UIResourceMgr.GetInstance().GetMaterial("StartMenu");
        UIResourceMgr.GetInstance().GetMaterial("Dialog");
        UIResourceMgr.GetInstance().GetMaterial("Buttons");
        UIResourceMgr.GetInstance().PrintExistingMaterials();
    }

    public void LoadAllUIMaterials()
    {

        UIResourceMgr.GetInstance().GetMaterial("ArenaMenu");
        UIResourceMgr.GetInstance().GetMaterial("Buttons");
        UIResourceMgr.GetInstance().GetMaterial("ShopUI");
        UIResourceMgr.GetInstance().GetMaterial("Weapons");
        UIResourceMgr.GetInstance().GetMaterial("Weapons2");
        UIResourceMgr.GetInstance().GetMaterial("Weapons3");
        UIResourceMgr.GetInstance().GetMaterial("Avatar");
        UIResourceMgr.GetInstance().GetMaterial("Dialog");

        UIResourceMgr.GetInstance().PrintExistingMaterials();
    }

    public void LoadMapUIMaterials()
    {
        UIResourceMgr.GetInstance().GetMaterial("ShopUI");
        UIResourceMgr.GetInstance().GetMaterial("Buttons");
        UIResourceMgr.GetInstance().GetMaterial("Map");
        UIResourceMgr.GetInstance().GetMaterial("ArenaMenu");
    }

    public void LoadAllGameUIMaterials()
    {
        UIResourceMgr.GetInstance().GetMaterial("GameUI");
        UIResourceMgr.GetInstance().GetMaterial("Buttons");
        UIResourceMgr.GetInstance().PrintExistingMaterials();

    }

    public void UnloadAllUIMaterials()
    {

        materialList.Clear();
        Resources.UnloadUnusedAssets();
        //Debug.Log("UI    All Resources Cleared");
        UIResourceMgr.GetInstance().PrintExistingMaterials();
    }



}

