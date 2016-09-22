using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using Zombie3D;



public class GameParametersXML
{


    public SpawnConfig Load(string path, int levelNum)
    {

        SpawnConfig spawnConfig = new SpawnConfig();
        spawnConfig.Waves = new List<Wave>();
        XmlReader reader = null;
        StringReader s = null;
        Stream stream = null;
        Debug.Log("Load");
        XmlDocument xmlDoc = new XmlDocument();
        if (path != null)
        {
            Debug.Log("path not null");
            path = Application.dataPath + path;


            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            stream = File.Open(path + "config.xml", FileMode.Open);
            xmlDoc.Load(stream);
            //reader = XmlReader.Create(stream);

        }
        else
        {
            //TextAsset XMLFile = (TextAsset)Resources.Load(path);
            TextAsset XMLFile = GameApp.GetInstance().GetResourceConfig().configXml;

            xmlDoc.LoadXml(XMLFile.text);

            //s = new StringReader(XMLFile.text);
            //reader = XmlReader.Create(s);


        }
        Wave wave = null;
        Round round = null;


        XmlNodeList levelList = xmlDoc.SelectNodes("Config/EnemySpawns/Level");
        if (levelNum <= levelList.Count)
        {
            levelNum = (levelNum - 1);
        }
        else
        {
            int rnd = Random.Range(levelList.Count-10, levelList.Count);
            levelNum = rnd;
        }
        
        Debug.Log("levelNum" + levelNum);
        XmlNodeList waveList = levelList[levelNum].SelectNodes("Wave");

        foreach (XmlNode waveNode in waveList)
        {
            wave = new Wave();
            wave.Rounds = new List<Round>();
            spawnConfig.Waves.Add(wave);
            wave.intermission = int.Parse(waveNode.Attributes["intermission"].Value);
            XmlNodeList roundList = waveNode.SelectNodes("Round");

            foreach (XmlNode roundNode in roundList)
            {
                round = new Round();
                round.EnemyInfos = new List<EnemyInfo>();
                wave.Rounds.Add(round);

                round.intermission = int.Parse(roundNode.Attributes["intermission"].Value);
                XmlNodeList enemyList = roundNode.SelectNodes("Enemy");
                foreach (XmlNode enemyNode in enemyList)
                {



                    EnemyInfo enemyInfo = new EnemyInfo();
                    round.EnemyInfos.Add(enemyInfo);

                    string eType = enemyNode.Attributes["id"].Value;


                    if (eType == "zombie")
                    {
                        enemyInfo.EType = EnemyType.E_ZOMBIE;
                    }
                    else if (eType == "nurse")
                    {
                        enemyInfo.EType = EnemyType.E_NURSE;
                    }
                    else if (eType == "tank")
                    {
                        enemyInfo.EType = EnemyType.E_TANK;
                    }
                    else if (eType == "hunter")
                    {
                        enemyInfo.EType = EnemyType.E_HUNTER;
                    }
                    else if (eType == "boomer")
                    {
                        enemyInfo.EType = EnemyType.E_BOOMER;
                    }
                    else if (eType == "swat")
                    {
                        enemyInfo.EType = EnemyType.E_SWAT;
                    }


                    enemyInfo.Count = int.Parse(enemyNode.Attributes["count"].Value);

                    string spawnFrom = enemyNode.Attributes["from"].Value;


                    if (spawnFrom == "grave")
                    {
                        enemyInfo.From = SpawnFromType.Grave;
                    }
                    else if (spawnFrom == "door")
                    {
                        enemyInfo.From = SpawnFromType.Door;
                    }



                }


            }

        }



        /*

        Debug.Log("level num " + levelNum);
        int level = 0;
        while (reader.Read())
        {

            if (reader.NodeType == XmlNodeType.Element)
            {
                if (reader.Name == "Level")
                {
                    level++;
                    if (level == levelNum)
                    {
                        break;
                    }
                }

            }



        }
        Debug.Log("level Num" + level);
        bool levelEnd = false;
        while (reader.Read() && !levelEnd)
        {
            Debug.Log(reader.Name);
            switch (reader.NodeType)
            {

                case XmlNodeType.Element:

                    if (reader.Name == "Wave")
                    {
                        wave = new Wave();
                        wave.Rounds = new List<Round>();
                        spawnConfig.Waves.Add(wave);
                        if (reader.HasAttributes)
                        {
                            while (reader.MoveToNextAttribute())
                            {
                                if (reader.Name == "intermission")
                                {
                                    wave.intermission = int.Parse(reader.Value);
                                }
                            }
                        }
                    }
                    else if (reader.Name == "Round")
                    {
                        round = new Round();
                        round.EnemyInfos = new List<EnemyInfo>();
                        wave.Rounds.Add(round);

                        if (reader.HasAttributes)
                        {
                            while (reader.MoveToNextAttribute())
                            {
                                if (reader.Name == "intermission")
                                {
                                    round.intermission = int.Parse(reader.Value);
                                }
                            }
                        }

                    }
                    else if (reader.Name == "Enemy")
                    {

                    }

                    break;
                case XmlNodeType.Text:
                    Debug.Log("Text " + reader.Value);
                    break;

                case XmlNodeType.EndElement:
                    if (reader.Name == "Level")
                    {
                        levelEnd = true;
                    }
                    break;
            }
        }
       

        if (reader != null)
            reader.Close();
         
        if (s != null)
        {
            s.Close();
        }
        */
        if (stream != null)
        {
            stream.Close();
        }

        return spawnConfig;


    }





}
